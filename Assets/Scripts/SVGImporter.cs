using UnityEngine;
using System.Xml;
using System.Collections;
using System;
using System.Collections.Generic;

public class SVGImporter
{
	static Color borderColor = Color.red, clearColor = Color.clear;

	static public Vector2 GetViewBox(XmlNode xml)
	{
		Vector2 size = Vector2.zero;
		string viewBox = null;

		viewBox = xml.Attributes ["viewBox"].Value;

		if (viewBox != null)
		{
			float[] c = Array.ConvertAll(viewBox.Split(' '), new Converter<string, float>(float.Parse));

			size = new Vector2 (c [2], c [3]);
		}

		return size;
	}

	static public Vector2 GetViewBox(string str)
	{
		XmlDocument doc = new XmlDocument ();
		doc.LoadXml (str);
		XmlNode xml = doc.DocumentElement as XmlNode;

		Vector2 size = Vector2.zero;
		string viewBox = null;

		viewBox = xml.Attributes ["viewBox"].Value;

		if (viewBox != null)
		{
			float[] c = Array.ConvertAll(viewBox.Split(' '), new Converter<string, float>(float.Parse));

			size = new Vector2 (c [2], c [3]);
		}

		return size;
	}

	static Vector2 CalculateBezierPoint(float t,
		Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
	{
		float u = 1f - t;
		float tt = t*t;
		float uu = u*u;
		float uuu = uu * u;
		float ttt = tt * t;

		Vector2 p = uuu * p0; //first term
		p += 3f * uu * t * p1; //second term
		p += 3f * u * tt * p2; //third term
		p += ttt * p3; //fourth term

		return p;
	}

	static float CalculateCirclePoint(float t, float radius)
	{
		return Mathf.Sqrt (radius * radius - t * t);
	}

	static Vector2 CalculateLinePoint(float t, Vector2 p0, Vector2 p1)
	{
		return new Vector2 ( (p1.x - p0.x) * t, (p1.y - p0.y) * t) + p0;
	}

	static Vector2 CalculateQuadraticBezierPoint(float t,
		Vector2 p0, Vector2 p1, Vector2 p2)
	{

		float u = 1f - t;
		float tt = t*t;
		float uu = u*u;

		Vector2 p = uu * p0; //first term
		p += 2f * u * t * p1; //second term
		p += tt * p2; //third term

		return p;
	}

	static void ClearTexture(Texture2D tex)
	{
		for (int i = 0; i < tex.width; ++i)
		{
			for (int u = 0; u < tex.height; ++u)
			{
				tex.SetPixel (i, u, clearColor);
				//tex.SetPixel (i, u, Color.clear);
			}
		}

		tex.Apply ();
	}

	static public string ReadPath(XmlNode xml)
	{
		foreach (XmlNode node in xml.ChildNodes) 
		{
			if (node.Name == "path")
			{
				return node.Attributes ["d"].Value;
			}
		}

		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes)
				if (n.Name == "path")
				{
					return n.Attributes ["d"].Value;
				}
		}


		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				foreach (XmlNode n2 in n.ChildNodes)
					if (n2.Name == "path")
					{
						return n2.Attributes ["d"].Value;
					}
			}
		}

		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				foreach (XmlNode n2 in n.ChildNodes)
				{
					foreach (XmlNode n3 in n2.ChildNodes)
						if (n3.Name == "path")
						{
							return n3.Attributes ["d"].Value;
						}
				}
			}
		}




		return "empty";
	}

	static public string ReadPolygon(XmlNode xml)
	{
		foreach (XmlNode node in xml.ChildNodes) 
		{
			if (node.Name == "polygon")
			{
				return node.Attributes ["points"].Value;
			}
		}

		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes)
				if (n.Name == "polygon")
				{
					return n.Attributes ["points"].Value;
				}
		}


		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes)
			{
				foreach (XmlNode n2 in n.ChildNodes)
					if (n2.Name == "polygon")
					{
						return n2.Attributes ["points"].Value;
					}
			}
		}




		return "empty";
	}

	static float currentScale = 1f;

	static Vector2 GetNextPolygonPoint(ref string str)
	{
		Vector2 point = Vector2.zero;

		for(int i=0; i<str.Length; ++i)
		{
			if (str [i] == ',')
			{
				point.x = float.Parse (new string (str.ToCharArray (), 0, i));
				//Debug.LogWarning ("x: " + point.x.ToString ());

				for (int u = i + 1; u < str.Length; ++u)
				{
					if (str[u] == ' ')
					{
						//Debug.LogError (new string (str.ToCharArray (), i + 1, u - (i + 1)));
						point.y = float.Parse (new string (str.ToCharArray(), i+1, u - (i+1)));
						//Debug.LogWarning ("y: " + point.y.ToString ());
						str = new string (str.ToCharArray(), u+1, str.Length - (u+1));

						for (int j = 0; j < str.Length; ++j)
							if (str [j] != ' ' && str[j] != '\t')
								return point;
						//Debug.LogError (str);

						str = "";
						//str = null;
						return point;
					}
				}
			}
		}

		return point;
	}

	static Vector2 GetNextPoint(ref string str)
	{
		bool letter = false;

		char[] letters = new char[] { 'c', 'C', 's', 'S', 'l', 'L', 'h', 'H', 'v', 'V' };

		bool vh = false;
		bool vertical = false, horizontal = false;

		foreach (char ch in letters)
			if (str [0] == ch)
			{
				letter = true;

				if (str [0] == 'v' || str [0] == 'V')
				{
					vertical = true;
					vh = true;
				} else if (str [0] == 'h' || str [0] == 'H')
				{
					horizontal = true;
					vh = true;
				}
					

				break;
			}

		if (letter)
		{
			str = new string (str.ToCharArray (), 1, str.Length - 1);
		}

		string temp = vh ? new string (str.ToCharArray(), 0, str.IndexOf(',', 1)) :  new string (str.ToCharArray(), 0, str.IndexOf(',', str.IndexOf(',') + 1));
		float[] c = Array.ConvertAll(temp.Split(','), new Converter<string, float>(float.Parse));

		str = new string (str.ToCharArray (), temp.Length + 1, str.Length - temp.Length - 1);

		//Debug.LogWarning (c.Length);


		//Debug.LogWarning (str);


		if (vh)
		{
			Vector2 p = new Vector2 (horizontal ? c [0] : 0, vertical ? c [0] : 0);

			return p * currentScale;
		}
		else
			return new Vector2 (c [0], c [1]) * currentScale;
	}

	static public void PaintTexture(Texture2D tex, Color color)
	{
		for (int i = 0; i < tex.width; ++i)
		{
			for (int u = 0; u < tex.height; ++u)
			{
				if (tex.GetPixel (i, u) != clearColor)//Color.clear)
				{
					tex.SetPixel (i, u, color);
				}
			}
		}

		tex.Apply ();
	}

	enum DrawerLocation { CLEAR, BORDER, FILL };

	static void FillTexture(Texture2D tex, Color color)
	{
		DrawerLocation pos, old;
		bool pix = false;
		int borderLength = 0;



		for (int i = 1; i < tex.height - 1; ++i)
		{

			pos = DrawerLocation.CLEAR;
			old = DrawerLocation.CLEAR;

			borderLength = 0;

			for (int u = 0; u < tex.width; ++u)
			{
				pix = tex.GetPixel (u, i) == borderColor ? true : false;

				if (pos == DrawerLocation.CLEAR)
				{
					if (pix)
					{
						old = pos;
						pos = DrawerLocation.BORDER;
						++borderLength;
					}
				} 
				else if (pos == DrawerLocation.BORDER)
				{
					if (!pix)
					{
						if (old == DrawerLocation.CLEAR)
						{
							old = pos;
							tex.SetPixel (u, i, color);
							pos = DrawerLocation.FILL;

							borderLength = 0;
						} 
						else if (old == DrawerLocation.FILL)
						{
							old = pos;
							pos = DrawerLocation.CLEAR;
							borderLength = 0;
						}
					} 
					else
					{
						++borderLength;
					}
				} 
				else if (pos == DrawerLocation.FILL)
				{
					if (pix)
					{
						old = pos;
						pos = DrawerLocation.BORDER;
						borderLength++;
					} 
					else
					{
						tex.SetPixel (u, i, color);
					}
				}
			}

			if (tex.GetPixel (tex.width - 1, i) == color)
			{
				bool firstBorder = false;

				for (int u = tex.width - 1; u >= 0; --u)
				{
					if (!firstBorder)
					{
						if (tex.GetPixel (u, i) == borderColor)
							firstBorder = true;
						else
							tex.SetPixel (u, i, clearColor);// Color.clear);
					} 
					else if (tex.GetPixel (u, i) != borderColor)
					{
						u = ExtraFillTexture (tex, i, u, color);	
					}
				}
			}

		}


		tex.Apply ();
	}

	static int ExtraFillTexture(Texture2D tex, int str, int col, Color color)
	{
		bool clear = false;

		for (int i = col; i >= 0; --i)
		{
			if (tex.GetPixel (i, str) == borderColor)
			{
				for (int u = i + 1; u <= col; ++u)
					tex.SetPixel (u, str, clear ? clearColor : color);//Color.clear : color);

				return i;
			}

			if (!clear && tex.GetPixel (i, str - 1) == clearColor)//Color.clear)
				clear = true;
		}

		return -1;
	}


	static public List<Vector2> GetEdgeCollider(XmlNode xml, int scale, int[] quality)
	{
		currentScale = scale;

		List<Vector2> points = new List<Vector2> ();

		Vector2 viewBox = GetViewBox (xml);

		//		int width = 2048;//(int)(viewBox.x * scale) + 1;
		//		int height = 2048;//(int)(viewBox.y * scale) + 1;
		//		currentScale = 2048f/ Mathf.Max(viewBox.x, viewBox.y);
		int width = (int)Mathf.Pow(2, scale);//(int)(viewBox.x*scale) + indent*2;
		int height = (int)Mathf.Pow(2, scale);//(int)(viewBox.y*scale) + indent*2;
		currentScale = (Mathf.Pow(2, scale) - 2)/ Mathf.Max(viewBox.x, viewBox.y);



		Vector2 indent = Vector2.zero;
		indent[Mathf.Max (viewBox.x, viewBox.y) == viewBox.x ? 1 : 0] = (Mathf.Pow(2, scale) - (currentScale * Mathf.Min(viewBox.x, viewBox.y)))/2f;
		//indent.y *= -1f;


		string path = ReadPath (xml);

		ConvertPath(ref path);

		Vector2 startPoint = GetNextPoint (ref path) / 100f;// + indent/100f;

		Vector2 secondPoint;
		//Debug.LogWarning (path);
		Vector2[] p = new Vector2[]{};//{ startPoint, GetNextPoint (ref path), GetNextPoint (ref path), GetNextPoint (ref path) };

		int currentQuality = 0;

		while(path.Length > 0)
		{
			CurveType type = CurveType.QUBIC;
			type = GetCurveType (path [0]);

			Vector2 mirrored = Vector2.zero;

			if (path.Length > 0)
			{
				bool relative = char.IsLower (path [0]);

				if (type == CurveType.QUBIC)
				{
					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero) 
					};
				} 
				else if (type == CurveType.QUBIC_S)
				{
					mirrored = p [p.Length - 1] - ( p [p.Length - 2] - p [p.Length - 1]);

					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				}
				else if (type == CurveType.LINE)
				{

					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
					//GetNextPoint (ref path);


				}
				else if (type == CurveType.HORIZONTAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path)/100f;
					if (!relative)
						p1.y = p0.y;

					p = new Vector2[] { 
						p0, 
						p1 + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				} 
				else if (type == CurveType.VERTICAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path)/100f;
					if (!relative)
						p1.x = p0.x;

					p = new Vector2[] { 
						p0, 
						p1 + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				}



			}

			bool line = false;

			if (type == CurveType.LINE || type == CurveType.HORIZONTAL || type == CurveType.VERTICAL)
			{
				line = true;
				Vector2 point;

				//				point = CalculateLinePoint (0, p [1], p [0]);
				//
				//				point -= new Vector2 (width / 2f, height / 2f)/100f;
				//				point += indent / 100f;
				//				point.y *= -1f;
				//				points.Add(point);

				for (float t = 1; t > 0; t -= 1f / (float)(quality [currentQuality]))
				{

					point = CalculateLinePoint (t, p [1], p [0]);

					point -= new Vector2 (width / 2f, height / 2f) / 100f;
					point += indent / 100f;
					point.y *= -1f;
					points.Add (point);


				}

				++currentQuality;

				continue;
			}



			for(float t = 0f; t<=1f; t += 1f/(float)(quality[currentQuality]))
			{
				Vector2 point;

				switch (type)
				{
				case CurveType.QUBIC:
					{
						point = CalculateBezierPoint (t, p [0], p [1], p [2], p [3]);
					}
					break;

				case CurveType.QUBIC_S:
					{
						point = CalculateBezierPoint (t, p [0], mirrored, p [1], p [2]);
					}
					break;

				case CurveType.LINE:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;

				case CurveType.HORIZONTAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;
				case CurveType.VERTICAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;


				default:
					point = Vector2.zero;
					break;
				}

				point -= new Vector2 (width / 2f, height / 2f)/100f;
				point += indent / 100f;
				point.y *= -1f;

				points.Add (point);

				if (points.Count > 1 && point == points [points.Count - 2])
					points.RemoveAt (points.Count - 1);

			}
			points.RemoveAt (points.Count - 1);
			++currentQuality;
		}

		startPoint -= new Vector2 (width / 2f, height / 2f)/100f;
		startPoint.y *= -1f;
		indent.y *= -1f;
		startPoint += indent / 100f;
		points.Add (startPoint);

		return points;//.ToArray ();
	}







	static public List<Vector2> GetEdgeCollider(XmlNode xml, int scale, int quality)
	{
		currentScale = scale;

		List<Vector2> points = new List<Vector2> ();

		Vector2 viewBox = GetViewBox (xml);

//		int width = 2048;//(int)(viewBox.x * scale) + 1;
//		int height = 2048;//(int)(viewBox.y * scale) + 1;
//		currentScale = 2048f/ Mathf.Max(viewBox.x, viewBox.y);
		int width = (int)Mathf.Pow(2, scale);//(int)(viewBox.x*scale) + indent*2;
		int height = (int)Mathf.Pow(2, scale);//(int)(viewBox.y*scale) + indent*2;
		currentScale = (Mathf.Pow(2, scale) - 2)/ Mathf.Max(viewBox.x, viewBox.y);



		Vector2 indent = Vector2.zero;
		indent[Mathf.Max (viewBox.x, viewBox.y) == viewBox.x ? 1 : 0] = (Mathf.Pow(2, scale) - (currentScale * Mathf.Min(viewBox.x, viewBox.y)))/2f;
		//indent.y *= -1f;


		string path = ReadPath (xml);



		if (path == "empty")
		{
			FigureType figureType = GetFigureType (xml);
			//Debug.LogError (figureType.ToString ());

			if (figureType == FigureType.CIRCLE)
			{
				float radius = Mathf.Pow (2, scale) / 2f;
				float step = Mathf.PI / ((float)quality * 2f);

				Vector2 first = Vector2.zero;

				for (float t = 0; t <= Mathf.PI; t += step)
				{
					if (t == 0)
						first = new Vector2 ((radius * Mathf.Cos (t)) / 100f, (-radius * Mathf.Sin (t)) / 100f);
					points.Add (new Vector2 ((radius * Mathf.Cos (t)) / 100f, (-radius * Mathf.Sin (t)) / 100f));
				}

				for (float t = Mathf.PI; t >= 0; t -= step)
					points.Add (new Vector2 ((radius * Mathf.Cos (t)) / 100f, (radius * Mathf.Sin (t)) / 100f));

				points.Add (first);
				return points;//.ToArray ();
			} 
			else if (figureType == FigureType.RECT)
			{
				points.Add (new Vector2 (-Mathf.Pow (2, scale) / 2f, -Mathf.Pow (2, scale) / 2f)/100f);
				points.Add (new Vector2 (-Mathf.Pow (2, scale) / 2f, Mathf.Pow (2, scale) / 2f)/100f);
				points.Add (new Vector2 (Mathf.Pow (2, scale) / 2f, Mathf.Pow (2, scale) / 2f)/100f);
				points.Add (new Vector2 (Mathf.Pow (2, scale) / 2f, -Mathf.Pow (2, scale) / 2f)/100f);
				points.Add (new Vector2 (-Mathf.Pow (2, scale) / 2f, -Mathf.Pow (2, scale) / 2f)/100f);

				return points;//.ToArray ();
			} 
			else if (figureType == FigureType.POLYGON)
			{
				path = ReadPolygon (xml);

				if (path != "empty")
				{
					float xmax = 0, xmin = 0, ymax = 0, ymin = 0;
					bool first = true;

					while(path.Length > 0)
					{
						Vector2 point = (GetNextPolygonPoint(ref path)/100f);

						if (first)
						{
							xmin = xmax = point.x;
							ymin = ymax = point.y;
							first = false;
						} else
						{
							if (point.x > xmax)
								xmax = point.x;

							if (point.x < xmin)
								xmin = point.x;

							if (point.y > ymax)
								ymax = point.y;

							if (point.y < ymin)
								ymin = point.y;
						}
						//point -= new Vector2 (width / 2f, height / 2f)/100f;
						point.y *= -1f;
						points.Add (point);

					}
					Vector2 center = new Vector2 (Mathf.Abs(xmax - xmin), -Mathf.Abs(ymax - ymin) )/2f + new Vector2(xmin, -ymin);///100f;
					//Debug.LogError(center);

					points.Add (points [0]);

					for (int i = 0; i < points.Count; ++i)
					{
						points [i] = points[i] - center;
					}


					return points;
				}
			}

		}

		ConvertPath(ref path);

		Vector2 startPoint = GetNextPoint (ref path) / 100f;// + indent/100f;

		Vector2 secondPoint;
		//Debug.LogWarning (path);
		Vector2[] p = new Vector2[]{};//{ startPoint, GetNextPoint (ref path), GetNextPoint (ref path), GetNextPoint (ref path) };

		while(path.Length > 0)
		{
			CurveType type = CurveType.QUBIC;
			type = GetCurveType (path [0]);

			Vector2 mirrored = Vector2.zero;

			if (path.Length > 0)
			{
				bool relative = char.IsLower (path [0]);

				if (type == CurveType.QUBIC)
				{
					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero) 
					};
				} 
				else if (type == CurveType.QUBIC_S)
				{
					mirrored = p [p.Length - 1] - ( p [p.Length - 2] - p [p.Length - 1]);

					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero), 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				}
				else if (type == CurveType.LINE)
				{

					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path)/100f + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
					//GetNextPoint (ref path);


				}
				else if (type == CurveType.HORIZONTAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path)/100f;
					if (!relative)
					p1.y = p0.y;

					p = new Vector2[] { 
						p0, 
						p1 + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				} 
				else if (type == CurveType.VERTICAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path)/100f;
					if (!relative)
						p1.x = p0.x;

					p = new Vector2[] { 
						p0, 
						p1 + (relative ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero)
					};
				}



			}

			if (quality < 1)
				quality = 1;
			else if (quality > 20)
				quality = 20;


			if (type == CurveType.LINE || type == CurveType.HORIZONTAL || type == CurveType.VERTICAL)
			{
				Vector2 point;

//				point = CalculateLinePoint (0, p [1], p [0]);
//
//				point -= new Vector2 (width / 2f, height / 2f)/100f;
//				point += indent / 100f;
//				point.y *= -1f;
//				points.Add(point);


				point = CalculateLinePoint (1, p [1], p [0]);

				point -= new Vector2 (width / 2f, height / 2f)/100f;
				point += indent / 100f;
				point.y *= -1f;
				points.Add(point);

				continue;
			}

			//

			for(float t = 0f; t<=1f; t += 1f/(float)quality)
			{
				Vector2 point;

				switch (type)
				{
				case CurveType.QUBIC:
					{
						point = CalculateBezierPoint (t, p [0], p [1], p [2], p [3]);
					}
					break;

				case CurveType.QUBIC_S:
					{
						point = CalculateBezierPoint (t, p [0], mirrored, p [1], p [2]);
					}
					break;

				case CurveType.LINE:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;

				case CurveType.HORIZONTAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;
				case CurveType.VERTICAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;


				default:
					point = Vector2.zero;
					break;
				}

				point -= new Vector2 (width / 2f, height / 2f)/100f;
				point += indent / 100f;
				point.y *= -1f;

				points.Add (point);

				if (points.Count > 1 && point == points [points.Count - 2])
					points.RemoveAt (points.Count - 1);

			}
			points.RemoveAt (points.Count - 1);

		}

		startPoint -= new Vector2 (width / 2f, height / 2f)/100f;
		startPoint.y *= -1f;
		indent.y *= -1f;
		startPoint += indent / 100f;
		points.Add (startPoint);


//		secondPoint = points [1];
//		secondPoint -= new Vector2 (width / 2f, height / 2f)/100f;
//		secondPoint.y *= -1f;
//		//indent.y *= -1f;
//		secondPoint += indent / 100f;
		//points.Add (points[]);
//		Debug.LogWarning ("Before opt: " + points.Count);
//
//
//		var optEdge = new List<Vector2> ();
//		var curIndex = 0;
//		while (curIndex < points.Count) {
//			var low = curIndex + 1;
//			var high = points.Count - 1;
//
//			while (low < high) {
//				var mid = low + (high - low + 1) / 2;
//				var deviation = CalcMaxDeviationFromLine(points, curIndex, mid);
//				if (deviation < MAX_DEVIATION) {
//					low = mid;
//				} else {
//					high = mid - 1;
//				}
//			}
//			optEdge.Add(points[curIndex]);
//			curIndex = low;
//		}
//
//		Debug.LogWarning ("After opt: " + optEdge.Count);

		return points;//.ToArray ();
	}

	static public float MAX_DEVIATION = 0.005f;

	static private float CalcMaxDeviationFromLine(List<Vector2> points, int fromIndex, int toIndex) {
		float maxDist = 0.0f;
		for (int i = fromIndex + 1; i < toIndex - 1; i++) {
			maxDist = Mathf.Max(
				maxDist,
				CalcDistFromPointToLine(
					points[fromIndex],
					points[toIndex],
					points[i]
				)
			);
		}
		return maxDist;
	}

	static private float CalcDistFromPointToLine(Vector2 linePoint1, Vector2 linePoint2, Vector2 point) {
		if (linePoint1.x == linePoint2.x) {
			return Mathf.Abs(linePoint1.x - point.x);
		} else if (linePoint1.y == linePoint2.y) {
			return Mathf.Abs(linePoint1.y - point.y);
		} else {
			var k1 = (linePoint1.y - linePoint2.y) / (linePoint1.x - linePoint2.x);
			var b1 = linePoint1.y - k1 * linePoint1.x;
			var k2 = -k1;
			var b2 = point.y - k2 * point.x;
			return Vector2.Distance (point, IntersectLines (k1, b1, k2, b2));
		}
	}

	private static Vector2 IntersectLines(float k1, float b1, float k2, float b2) {
		if (k1 == k2) {
			throw new Exception("Given lines are parallel!");
		} else {
			float x = (b2 - b1) / (k1 - k2);
			float y = k1 * x + b1;
			return new Vector2(x, y);
		}
	}

	static void ConvertPath(ref string path)
	{
		path = new string (path.ToCharArray (), 1, path.Length - 2);
		path = path.Insert (path.Length, ",");

		for (int i = 0; i < path.Length; ++i)
		{
			if (i > 0)
			{
				bool letter = false;

				char[] letters = new char[] { 'c', 'C', 's', 'S', 'l', 'L', 'h', 'H', 'v', 'V' };

				foreach (char ch in letters)
					if (path [i - 1] == ch)
					{
						letter = true;
						break;
					}

				if (path [i] == '-' && !letter)
				{
					path = path.Insert (i++, ",");
				} else if (letter)
				{
					path = path.Insert ((i++) - 1, ",");
				}
			}
		}
	}

	public enum CurveType { QUBIC_S, QUBIC, LINE, VERTICAL, HORIZONTAL, NONE };

	//static int indent = 20;

	static CurveType GetCurveType(char c)
	{
		switch (c)
		{
		case 'c':
		case 'C':
			return CurveType.QUBIC;
			break;

		case 's':
		case 'S':
			return CurveType.QUBIC_S;
			break;

		case 'l':
		case 'L': 
			return CurveType.LINE;
			break;


		case 'h':
		case 'H':
			return CurveType.HORIZONTAL;
			break;
		
		case 'v': case 'V':
			return CurveType.VERTICAL;
			break;


		default:
			return CurveType.NONE;
			break;
		}
	}

	public enum FigureType { PATH, CIRCLE, RECT, POLYGON, NONE };

	static public FigureType GetFigureType(XmlNode xml)
	{
		foreach (XmlNode node in xml.ChildNodes) 
		{
			if (node.Name == "circle")
				return FigureType.CIRCLE;
			else if (node.Name == "rect")
				return FigureType.RECT;
			else if (node.Name == "polygon")
				return FigureType.POLYGON;
			else if (node.Name == "path")
				return FigureType.PATH;
		}


		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes) 
			{
				if (n.Name == "circle")
					return FigureType.CIRCLE;
				else if (n.Name == "rect")
					return FigureType.RECT;
				else if (n.Name == "polygon")
					return FigureType.POLYGON;
				else if (node.Name == "path")
					return FigureType.PATH;
			}
		}


		foreach (XmlNode node in xml.ChildNodes) 
		{
			foreach (XmlNode n in node.ChildNodes) 
			{
				foreach (XmlNode n2 in n.ChildNodes) 
				{
					if (n2.Name == "circle")
						return FigureType.CIRCLE;
					else if (n2.Name == "rect")
						return FigureType.RECT;
					else if (n2.Name == "polygon")
						return FigureType.POLYGON;
					else if (node.Name == "path")
						return FigureType.PATH;
				}
			}
		}

		return FigureType.NONE;
	}

	static public Texture2D GetTexture2D(XmlNode xml, int scale)
	{
		currentScale = scale;

		Vector2 viewBox = GetViewBox (xml);



		int width = (int)Mathf.Pow(2, scale);//(int)(viewBox.x*scale) + indent*2;
		int height = (int)Mathf.Pow(2, scale);//(int)(viewBox.y*scale) + indent*2;
		currentScale = (Mathf.Pow(2, scale) - 2)/ Mathf.Max(viewBox.x, viewBox.y);

		Vector2 indent = Vector2.zero;
		indent[Mathf.Max (viewBox.x, viewBox.y) == viewBox.x ? 1 : 0] = (Mathf.Pow(2, scale) - (currentScale * Mathf.Min(viewBox.x, viewBox.y)))/2f;
		indent.y *= -1f;
         

		Texture2D tex = new Texture2D (width, height);//, TextureFormat., true);//, TextureFormat, false);
		ClearTexture (tex);

		//return tex;


		string path = ReadPath (xml);

		if (path == "empty")
		{
			FigureType figureType = GetFigureType (xml);


			if (figureType == FigureType.CIRCLE)
			{
				float radius = Mathf.Pow (2, scale) / 2f;
				float step = 1f / Mathf.Pow (2, scale);

				for (float t = -radius; t <= radius; t += step)
				{
					tex.SetPixel ((int)(width / 2f + t), (int)(height / 2f - CalculateCirclePoint (t, radius)), borderColor);
					tex.SetPixel ((int)(width / 2f + t), (int)(height / 2f + CalculateCirclePoint (t, radius)), borderColor);
				}

				tex.Apply ();

				FillTexture (tex, Color.black);
				return tex;
			} 
			else if (figureType == FigureType.RECT)
			{
				for (int i = 0; i < tex.width; ++i)
				{
					for (int u = 0; u < tex.height; ++u)
					{
						tex.SetPixel (i, u, Color.black);
						//tex.SetPixel (i, u, Color.clear);
					}
				}

				tex.Apply ();

				return tex;
			}
		}

		//Debug.LogWarning (path);
		ConvertPath(ref path);

		Vector2 startPoint = GetNextPoint (ref path);
		Vector2[] p = new Vector2[]{};


		while(path.Length > 0)
		{
			CurveType type = CurveType.QUBIC;

			type = GetCurveType (path [0]);

			Vector2 mirrored = Vector2.zero;

			if (path.Length > 0)
			{
				Vector2 relative = char.IsLower (path [0]) ? (p.Length == 0 ? startPoint : p [p.Length - 1]) : Vector2.zero;

				if (type == CurveType.QUBIC)
				{
					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path) + relative, 
						GetNextPoint (ref path) + relative, 
						GetNextPoint (ref path) + relative
					};
				} else if (type == CurveType.QUBIC_S)
				{
					mirrored = p [p.Length - 1] - (p [p.Length - 2] - p [p.Length - 1]);

					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path) + relative, 
						GetNextPoint (ref path) + relative
					};
				} else if (type == CurveType.LINE)
				{
					p = new Vector2[] { 
						p.Length == 0 ? startPoint : p [p.Length - 1], 
						GetNextPoint (ref path) + relative
					};
					//GetNextPoint (ref path);
				} else if (type == CurveType.HORIZONTAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path);
					if (relative == Vector2.zero)
					p1.y = p0.y;

					p = new Vector2[] { 
						p0, 
						p1 + relative
					};
				} 
				else if (type == CurveType.VERTICAL)
				{
					Vector2 p0 = p.Length == 0 ? startPoint : p [p.Length - 1];
					Vector2 p1 = GetNextPoint (ref path);
					if (relative == Vector2.zero)
					p1.x = p0.x;

					p = new Vector2[] { 
						p0, 
						p1 + relative
					};
				}

			}



			for(float t = 0f; t<=1f; t += 0.0001f)
			{
				Vector2 point;

				switch (type)
				{
				case CurveType.QUBIC:
					{
						point = CalculateBezierPoint (t, p [0], p [1], p [2], p [3]);
					}
					break;

				case CurveType.QUBIC_S:
					{
						point = CalculateBezierPoint (t, p [0], mirrored, p [1], p [2]);
					}
					break;

				case CurveType.LINE:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;
				case CurveType.HORIZONTAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;
				case CurveType.VERTICAL:
					{
						point = CalculateLinePoint (t, p [1], p [0]);
					}
					break;

				default:
					point = Vector2.zero;
					break;
				}

				tex.SetPixel ((int)point.x + (int)indent.x,  (int)indent.y + height - (int)point.y - 1, borderColor);
			}
				
//			if (type == CurveType.LINE)
//			{
//				tex.SetPixel (indent + (int)p[0].x,  -height - (int)p[0].y - 1, Color.cyan);
//				tex.SetPixel (indent + (int)p[1].x,  -indent + height - (int)p[1].y - 1, Color.cyan);
//			}

		}





		tex.Apply ();

		FillTexture (tex, Color.black);

		//Debug.LogWarning (Application.dataPath + "test.png");



		//Texture2D txtr = new Texture2D (tex.width, tex.height, TextureFormat.PVRTC_RGBA4, tex.mipmapCount > 1);
		//txtr.filterMode = FilterMode.Trilinear;
		//txtr.LoadRawTextureData(txtr.GetRawTextureData());
		//txtr.Apply ();
		return tex;
	}


	static public bool ByteArrayToFile(string _FileName, byte[] _ByteArray)
	{
		try
		{
			// Open file for reading
			System.IO.FileStream _FileStream = 
				new System.IO.FileStream(_FileName, System.IO.FileMode.Create,
					System.IO.FileAccess.Write);
			// Writes a block of bytes to this stream using data from
			// a byte array.
			_FileStream.Write(_ByteArray, 0, _ByteArray.Length);

			// close file stream
			_FileStream.Close();

			return true;
		}
		catch (Exception _Exception)
		{
			// Error
			//Console.WriteLine("Exception caught in process: {0}",
				//_Exception.ToString());
		}

		// error occured, return false
		return false;
	}

}
