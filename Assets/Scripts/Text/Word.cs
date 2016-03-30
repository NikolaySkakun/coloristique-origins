using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Word
{
	static int vertsCount = 38;
	static public float radius = 0.5f;
	static public float thick = 10f;

	static public GameObject WriteString(string s, float rad = 0.5f, Obj.Colour color = Obj.Colour.BLACK, bool anim = false, bool del = false, int from = -1)
	{
		radius = rad;

		GameObject str = new GameObject(s);
		GameObject[] letter = new GameObject[s.Length];
		//Type type = Type.GetType(node.ChildNodes[i].Name);
		//type.GetMethod("Join").Invoke(null, new object[]{node.ChildNodes[i], obj});

		for(int i=0; i<s.Length; ++i)
		{
			//Debug.Log(typeof(Word).GetMethod(s[0].ToString().ToUpper()));

			if(s[i] == ' ')
			{
				letter[i] = new GameObject("Space");
				letter[i].transform.parent = str.transform;
				
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				continue;
			}
			else if(s[i] == '?')
			{
				letter[i] = QuestionMark();
				letter[i].transform.parent = str.transform;
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				continue;
			}
			else if(s[i] == ':')
			{
				letter[i] = Colon();
				letter[i].transform.parent = str.transform;
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				continue;
			}
			else if(s[i] == '.')
			{
				letter[i] = Dot();
				letter[i].transform.parent = str.transform;
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				continue;
			}
			else if(s[i] == '@')
			{
				letter[i] = At();
				letter[i].transform.parent = str.transform;
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				continue;
			}
			else if(s[i] >= '0' && s[i] <= '9')
			{
				if(anim)
				{
					if(from >= 0)
						letter[i] = Digit.Shift(from, int.Parse(s[i].ToString()));
					else
						letter[i] = Digit.Shift(int.Parse(s[i].ToString()), del);
				}
				else
					letter[i] = (GameObject)typeof(Digit).GetMethod("_" + s[i].ToString()).Invoke(null, null);

				//letter[i].name += "_" + s[i].ToString();
				letter[i].name = s[i].ToString();
				letter[i].transform.parent = str.transform;
				
				letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i]));
				letter[i].transform.localScale = new Vector3(1, -1, -1);
				//Debug.Log(s[i]);

				if(color == Obj.Colour.WHITE)
				{
					if(letter[i].GetComponent<Renderer>() != null)
						Game.Paint(letter[i], color);
					
					Renderer[] rend = letter[i].GetComponentsInChildren<Renderer>();
					
					foreach(Renderer r in rend)
						r.material.color = Game.GetColor(color);
				}

				continue;
			}
			else if(typeof(Word).GetMethod(s[i].ToString().ToUpper()) == null)
				break;

//			if(char.IsUpper(s[i]) && (s[i] == 'c' 
//			                              || s[i] == 'o' 
//			                              || s[i] == 'l' 
//			                              || s[i] == 'i' 
//			                              || s[i] == 'u' 
//			                              || s[i] == 'q' 
//			                              || s[i] == 't' 
//			                              || s[i] == 'r'
//			                              || s[i] == 's'
//			                              || s[i] == 'e'))
//			{
//				letter[i] = GetGameObject((Mesh)typeof(LetterAnimation).GetMethod (s[i].ToString().ToUpper()).Invoke (null, new object[]{1f}));
//			}
//			else
//			{
				if(s[i] == 'E')
				{
					float tempRadius = LetterAnimation.radius;
					LetterAnimation.radius = rad;
					letter[i] = GetGameObject((Mesh)typeof(LetterAnimation).GetMethod (s[i].ToString().ToUpper()).Invoke (null, new object[]{1f}));
					//letter[i].transform.localScale = Vector3.one * rad;
					LetterAnimation.radius = tempRadius;
				}
				else
				letter[i] = (GameObject)typeof(Word).GetMethod(s[i].ToString().ToUpper()).Invoke(null, null);
			//}
			letter[i].name = s[i].ToString();
			letter[i].transform.parent = str.transform;







			if(char.IsUpper(s[i]))
			{
				GameObject border = GetButtonBorder();
				border.transform.parent = letter[i].transform;
				border.transform.localPosition = Vector3.zero;
			}

			CombineMeshes(letter[i]);

			if(s[i] == 'w')
				letter[i].transform.localEulerAngles = Vector3.zero;
			else if(anim && (s[i] == 'c' 
			                 || s[i] == 'o' 
			                 || s[i] == 'l' 
			                 || s[i] == 'i' 
			                 || s[i] == 'u' 
			                 || s[i] == 'q' 
			                 || s[i] == 't' 
			                 || s[i] == 'r'
			                 || s[i] == 's'
			                 || s[i] == 'e'))
			{
				letter[i].AddComponent<LetterAnimation>();
			}

			letter[i].GetComponent<Renderer>().material = Game.BaseMaterial;
			letter[i].GetComponent<Renderer>().material.color = Game.GetColor(color);

			letter[i].transform.localPosition += Vector3.forward * ( (i>0 ? letter[i-1].transform.localPosition.z-GetSize(s[i-1]) : 0) - GetSize(s[i])); // + (s[i] == 'j' ? -0.15f : 0)

			if(color == Obj.Colour.WHITE)
			{
				if(letter[i].GetComponent<Renderer>() != null)
					Game.Paint(letter[i], color);

				Renderer[] rend = letter[i].GetComponentsInChildren<Renderer>();
				
				foreach(Renderer r in rend)
					r.material.color = Game.GetColor(color);
			}
		}
		radius = 0.5f;
		return str;
	}

	static public void ApplyReverseColorShaderToString(GameObject word)
	{
		for(int i=0; i<word.transform.childCount; ++i)
		{
			if(word.transform.GetChild(i).GetComponent<Renderer>() != null)
			{
				word.transform.GetChild(i).GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
				word.transform.GetChild(i).GetComponent<Renderer>().material.color = Color.white;
			}
		}
	}

	static void CombineMeshes(GameObject letter)
	{

		MeshFilter[] meshFilters = letter.GetComponentsInChildren<MeshFilter>();
		CombineInstance[] combine = new CombineInstance[meshFilters.Length];
		//Material[] materials = new Material[meshFilters.Length];
		int i = 0;
		if(meshFilters.Length > 0)
		{
			while (i < meshFilters.Length) {

				/*foreach(Material mat in meshFilters[i].gameObject.GetComponent<Renderer>().sharedMaterials)
				{
					materials.Add(mat);
				}*/
				//materials[i] = meshFilters[i].gameObject.GetComponent<Renderer>().sharedMaterial;

				combine[i].mesh = meshFilters[i].sharedMesh;
				combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
				meshFilters[i].gameObject.active = false;
				//UnityEngine.Object.Destroy(meshFilters[i].gameObject);
				i++;
			}

			if(letter.GetComponent<MeshFilter>() == null)
			{
				letter.AddComponent<MeshFilter>();
				letter.AddComponent<MeshRenderer>();
			}

			letter.GetComponent<MeshFilter>().mesh = new Mesh();
			//letter.GetComponent<Renderer>().sharedMaterials = materials;//.ToArray();
			letter.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);

			letter.gameObject.active = true;
		}
		/*
		letter.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
		letter.GetComponent<Renderer>().material.color = Color.white;*/
	}

	static public GameObject GetButtonBorder()
	{
		//float posQ = 0.04f, posL = 0.076f;;
		float posQ = radius, posL = radius * 1.9f; //*1.3f//1.9f //2.2f
		//float posQ = 0.06f, posL = 0.096f;
		GameObject border = new GameObject("ButtonBorder");

		GameObject[] quarter = new GameObject[4];
		GameObject[] lines = new GameObject[4];

		for(int i=0; i<quarter.Length; ++i)
		{
			quarter[i] = GetGameObject(QuarterCircle());
			quarter[i].transform.parent = border.transform;
			quarter[i].transform.localEulerAngles = Vector3.up * 90f*i;

			lines[i] = I_();
			lines[i].transform.parent = border.transform;


			//lines[i].transform.localScale = Vector3.one + Vector3.right * 0.3f;
		}

		quarter[0].transform.localPosition = new Vector3(posQ, 0, posQ);
		quarter[1].transform.localPosition = new Vector3(posQ, 0, -posQ);
		quarter[2].transform.localPosition = new Vector3(-posQ, 0, -posQ);
		quarter[3].transform.localPosition = new Vector3(-posQ, 0, posQ);

		lines[0].transform.localPosition = new Vector3(0, 0, posL);
		lines[1].transform.localPosition = new Vector3(0, 0, -posL);
		lines[2].transform.localPosition = new Vector3(posL, 0, 0);
		lines[3].transform.localPosition = new Vector3(-posL, 0, 0);

		lines[2].transform.eulerAngles = Vector3.up * 90f;
		lines[3].transform.eulerAngles = Vector3.up * 90f;

		/*for(int i=3; i>=0; --i)
		{
			Object.Destroy(quarter[i]);
			Object.Destroy(lines[i]);
		}*/

		CombineMeshes(border);

		return border;
	}

	static public Mesh QuarterCircle(float r = 1f, float angle = 90f, float thick = 0.0f)
	{
		int vertsCount_ = 20;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount_/2 * 2];
		int[] tris = new int[2*(vertsCount_/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = i;//(i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*(((angle/2f)/(vertsCount_/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*(((angle/2f)/(vertsCount_/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius*r - radius/(Word.thick*0.5f) - radius*thick : radius*r + radius*thick;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;

		return mesh;
	}

	static float GetSize(char c)
	{
		float size = 0, free = radius/5f;



		switch(c)
		{
		//case 'c': size = radius - radius/4f; break;
		case 'k': case 'r': size = radius - radius/4f; break;
		case 't': case 's': size = radius - radius/8f; break;
		case 'i': case 'l': case ':': case '.': size = radius/10f; break;
		case 'm': case 'w': case '@': size = radius*1.5f; break;
		case 'j': size = radius - radius/2f; break;

		//case 'k': size = radius - radius/4f; break;
		default: size = radius; break;
		}

		if(c >= '0' && c <= '9')
			size *= 1.5f;

		return size + free;
	}

	static public GameObject GetGameObject(Mesh mesh, Obj.Colour color = Obj.Colour.BLACK)
	{
		GameObject obj = new GameObject();
		
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>().material = Game.BaseMaterial;
		obj.GetComponent<Renderer>().material.color = Game.GetColor(color);
		
		return obj;
	}

	static public GameObject A()
	{
		GameObject o = O (), i = I_A();
		i.transform.localPosition = -Vector3.forward*radius + Vector3.forward* radius/(thick);
		i.transform.localPosition -= Vector3.right*radius;///2f
		i.transform.localScale = new Vector3(1, 1, -1);
		//i.transform.localScale = new Vector3(0.5f, 1, 1);
		
		i.transform.parent = o.transform;
		
		return o;
	}

	static public GameObject At()
	{
		GameObject o = O (), i = I_A_();
		i.transform.localPosition = -Vector3.forward*radius + Vector3.forward* radius/(thick);
		i.transform.localPosition -= Vector3.right*radius;///2f
		i.transform.localScale = new Vector3(1, 1, -1);
		//i.transform.localScale = new Vector3(0.5f, 1, 1);

		GameObject u = U_(0.35f);
		GameObject c = C_(0.735f);

		u.transform.parent = o.transform;

		u.transform.localPosition = new Vector3(-0.4f, 0, -0.575f);

		c.transform.parent = o.transform;

		c.transform.localEulerAngles = Vector3.up * 40f;
		i.transform.parent = o.transform;
		
		return o;
	}

	static public GameObject B()
	{
		GameObject o = O (), i = I_P();
		i.transform.localPosition = Vector3.forward*radius - Vector3.forward* radius/(thick);
		i.transform.localPosition += Vector3.right*radius;
		i.transform.localScale = new Vector3(-1, 1, 1);
		
		//i.transform.localScale -= Vector3.right*radius;
		
		i.transform.parent = o.transform;
		
		return o;
	}
	
	static public GameObject C()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];

		float angle = 135f;

		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i-verts.Length/6);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius - radius/(thick*0.5f) : radius;

			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = (-radius/4f) + sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject C_(float r = 1)
	{
		float tmpRadius = radius;
		radius = r;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i-verts.Length/6);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((150f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((150f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius - tmpRadius/(thick*0.5f) : radius;
			
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = (-radius/4f) + sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;

		radius = tmpRadius;

		return GetGameObject(mesh);
	}

	static public GameObject D()
	{
		GameObject o = O (), i = I_();
		i.transform.localPosition = -Vector3.forward*radius + Vector3.forward* radius/thick;
		i.transform.localPosition += Vector3.right*radius;
		
		//i.transform.localScale -= Vector3.right*radius;
		
		i.transform.parent = o.transform;
		
		return o;
	}
	
//	static public GameObject E()
//	{
//		//int vertsCount_ = 40;
//		Mesh mesh = new Mesh();
//		Vector3[] verts = new Vector3[vertsCount * 2];
//		int[] tris = new int[2*(vertsCount - 1) * 3];
//		
//		for(int i=0; i<verts.Length; ++i)
//		{
//			int tmp = (i-verts.Length/4 + 1);
//			float cos = Mathf.Cos( Mathf.Deg2Rad*((162f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
//			float sin = Mathf.Sin( Mathf.Deg2Rad*((162f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
//			
//			float tempRadius = i%2==0 ? radius - radius/(thick*0.5f) : radius;
//			
//			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
//			verts[i].x = cos * tempRadius;
//			verts[i].z = sin * tempRadius;
//
//			if(i==0)
//			{
//				verts[i].x = cos * tempRadius*1.29f;
//			}
//			else if(i==1)
//			{
//				verts[i].x = cos * tempRadius*1.03f;
//			}
//			
//			if(i*3 + 2 < tris.Length)
//			{
//				if(i%2==0)
//				{
//					tris[i*3] = i + 1;
//					tris[i*3 + 1] = i;
//					tris[i*3 + 2] = i+2;
//				}
//				else
//				{
//					tris[i*3] = i;
//					tris[i*3 + 1] = i+1;
//					tris[i*3 + 2] = i+2;
//				}
//			}
//
//
//		}
//		
//		mesh.vertices = verts;
//		mesh.triangles = tris;
//		
//		GameObject e = GetGameObject(mesh);
//		GameObject i_ = I__(radius*2f, 2f*radius/thick);
//		
//		i_.transform.parent = e.transform;
//		i_.transform.localEulerAngles = Vector3.up * 90;
//		//i_.transform.localScale += 2*Vector3.right*i_.transform.localScale.x;// /100f
//		//i_.transform.localPosition += Vector3.right*radius/thick;
//		i_.transform.localPosition += Vector3.forward * radius + Vector3.right*radius/thick;
//		return e;
//	}

	static public GameObject E()
	{
		//int vertsCount_ = 40;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i-verts.Length/4 + 1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((162f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((162f/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius - radius/5f : radius;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		GameObject e = GetGameObject(mesh);
		GameObject i_ = I_();
		
		i_.transform.parent = e.transform;
		i_.transform.localEulerAngles = Vector3.up * 90;
		i_.transform.localScale -= 2*Vector3.right/100f;
		i_.transform.localPosition += Vector3.right*radius/10f;
		
		return e;
	}

	static public GameObject F()
	{
		GameObject l = I_ (), i = I_();
		i.transform.parent = l.transform;
		
		i.transform.localEulerAngles = Vector3.up * 90;
		i.transform.localScale -= Vector3.right*0.2f;
		i.transform.localPosition += Vector3.right*radius/thick;

		GameObject top = GetGameObject(QuarterCircle());
		top.transform.parent = l.transform;

		top.transform.localPosition = new Vector3(radius, 0, -radius + radius/thick);


		
		return l;
	}

	static public GameObject G()
	{
		GameObject a = A (), c = GetGameObject(QuarterCircle()), i = I_();
		
		
		i.transform.parent = c.transform.parent = a.transform;
		c.transform.localEulerAngles += Vector3.up * 180f;
		c.transform.localPosition -= Vector3.right * radius;
		
		i.transform.localScale = new Vector3(0.25f, 1, 1);
		i.transform.localEulerAngles += Vector3.up * 90f;
		
		i.transform.localPosition = new Vector3(-radius*2 + radius/10f, 0, radius/4f);
		
		return a;
	}

	static public GameObject H()
	{
		GameObject h = new GameObject("h"), u = U (), i;// = I_();
		i = I_P();
		i.transform.localPosition = Vector3.forward*radius - Vector3.forward* radius/thick;
		i.transform.localPosition += Vector3.right*radius;
		i.transform.localScale = new Vector3(-1, 1, 1);
		
		u.transform.parent = i.transform.parent = h.transform;
		u.transform.localEulerAngles += Vector3.up * 180f;
		
		//i.transform.localPosition = Vector3.forward*radius - Vector3.forward* radius/10f;
		//i.transform.localPosition += Vector3.right*radius;
		
		return h;
	}

	static GameObject I_()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(-radius, 0, -radius/thick);
		verts[1] = new Vector3(-radius, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, radius/thick);
		verts[3] = new Vector3(radius, 0, -radius/thick);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject I__(float height, float thick)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(0, 0, -thick/2f);
		verts[1] = new Vector3(0, 0, thick/2f);
		verts[2] = new Vector3(height, 0, thick/2f);
		verts[3] = new Vector3(height, 0, -thick/2f);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static GameObject I_E()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[6];
		
		verts[0] = new Vector3(-radius*0.8144f, 0, -radius/thick);
		verts[1] = new Vector3(-radius*0.788f, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, radius/thick);
		verts[3] = new Vector3(radius, 0, -radius/thick);

		verts[4] = new Vector3(-radius*0.8156f, 0, -radius/15.15f);
		verts[5] = new Vector3(-radius*0.8008f, 0, radius/18.07f);

//		verts[0] = new Vector3(-radius*(thick/12.278f), 0, -radius/thick);
//		verts[1] = new Vector3(-radius*(thick/12.69f), 0, radius/thick);
//		verts[2] = new Vector3(radius, 0, radius/thick);
//		verts[3] = new Vector3(radius, 0, -radius/thick);
//		
//		verts[4] = new Vector3(-radius*0.8156f, 0, -radius/15.15f);
//		verts[5] = new Vector3(-radius*0.8008f, 0, radius/18.07f);
		
		int[] tris = new int[] {
			0, 4, 2,
			0, 2, 3,
			4, 5, 2,
			5, 1, 2
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static GameObject I_P()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[7];
		
		verts[0] = new Vector3(-radius, 0, -radius/thick);
		verts[1] = new Vector3(-radius, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, radius/thick);
		verts[3] = new Vector3(radius/2.5f, 0, -radius/thick);
		verts[4] = new Vector3(radius/1.822f, 0, -radius/150f);
		verts[5] = new Vector3(radius/1.41f, 0, radius/18f);
		verts[6] = new Vector3(radius/1.21f, 0, radius/12f);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 4, 3,
			5, 4, 0,
			5, 0, 6,
			0, 6, 2
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static GameObject I_A()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[8];
		
		verts[0] = new Vector3(0, 0, -radius/thick);
		verts[1] = new Vector3(0, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, radius/thick);
		verts[3] = new Vector3(radius/2.5f, 0, -radius/thick);
		verts[4] = new Vector3(radius/1.822f, 0, -radius/150f);
		verts[5] = new Vector3(radius/1.41f, 0, radius/18f);
		verts[6] = new Vector3(radius/1.21f, 0, radius/12f);
		verts[7] = new Vector3(0, 0, 0);
		
		int[] tris = new int[] {
			6, 1, 2,
			0, 4, 3,
			5, 4, 0,
			5, 0, 6,
			1, 6, 5,
			5, 0, 1
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static GameObject I_A_()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[8];
		
		verts[0] = new Vector3(radius/5f, 0, -radius/thick);
		verts[1] = new Vector3(radius/5f, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, radius/thick);
		verts[3] = new Vector3(radius/2.5f, 0, -radius/thick);
		verts[4] = new Vector3(radius/1.822f, 0, -radius/150f);
		verts[5] = new Vector3(radius/1.41f, 0, radius/18f);
		verts[6] = new Vector3(radius/1.21f, 0, radius/12f);
		verts[7] = new Vector3(0, 0, 0);
		
		int[] tris = new int[] {
			6, 1, 2,
			0, 4, 3,
			5, 4, 0,
			5, 0, 6,
			1, 6, 5,
			5, 0, 1
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}
	
	static public GameObject I()
	{
		GameObject obj = I_();
		GameObject circle = GetGameObject(CustomMesh.Circle(20));
		circle.transform.localScale = Vector3.one * radius/(thick/2.222f);
		circle.transform.parent = obj.transform;
		
		circle.transform.localPosition = Vector3.right * radius*1.4f;
		
		return obj;
	}

	static public GameObject J()
	{
		GameObject j = new GameObject("J"), c = GetGameObject(QuarterCircle()), i_ = I (); //, i = I_()
		
		
		i_.transform.parent = c.transform.parent = j.transform; //= i.transform.parent
		c.transform.localEulerAngles += Vector3.up * 180f;
		c.transform.localPosition -= Vector3.right * radius;
		c.transform.localPosition += Vector3.forward * (radius - radius/(thick*0.5f));
		//i.transform.localScale = new Vector3(0.25f, 1, 1);
		//i.transform.localEulerAngles += Vector3.up * 90f;
		
		//i.transform.localPosition = new Vector3(-radius*2 + radius/10f, 0, radius/4f);

		i_.transform.localPosition -= Vector3.forward*(radius/thick);

		i_.transform.localPosition -= Vector3.forward * (radius/3.5f);
		c.transform.localPosition -= Vector3.forward * (radius/3.5f);
		
		return j;
	}

	static public GameObject K()
	{
		GameObject k = new GameObject("K"), l = L ();

		l.transform.parent = k.transform;

		l.transform.localPosition = new Vector3(0, 0, radius - radius/5f);

		GameObject obj;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[8];
		
		//float r = radius * 1.1f;
		
		verts[0] = new Vector3(-radius/4.7f, 0, radius*0.9f-radius/5f);
		verts[1] = new Vector3(-radius/5f, 0, radius*0.9f);
		verts[2] = new Vector3(radius, 0, -radius*0.5f);
		verts[3] = new Vector3(radius, 0, -radius*0.5f + radius/4.5f);
		
		verts[4] = new Vector3(radius/5f, 0, radius/2f);
		verts[5] = new Vector3(radius/4.5f, 0, radius/2f - radius/4.5f);
		verts[6] = new Vector3(-radius, 0, -radius*0.65f);
		verts[7] = new Vector3(-radius, 0, radius/4.5f - radius*0.65f);
		
		int[] tris = new int[] {
			0, 1, 3,
			3, 2, 0,
			6, 4, 5,
			7, 4, 6
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;

		obj = GetGameObject(mesh);
		obj.transform.parent = k.transform;

		l.transform.localPosition -= Vector3.forward * (radius / 3.33f); // 0.15f
		obj.transform.localPosition -= Vector3.forward * (radius / 3.33f);

		//U_(0.5f).transform.parent = k.transform;

		return k;
	}

	static public GameObject L()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(-radius, 0, -radius/thick);
		verts[1] = new Vector3(-radius, 0, radius/thick);
		verts[2] = new Vector3(2f*radius, 0, radius/thick);
		verts[3] = new Vector3(2f*radius, 0, -radius/thick);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject M()
	{
		float r = 0.75f;
		GameObject m = new GameObject ("M"), n = U__ (r), n2 = U__ (r);//, i = I_();
		n.transform.localEulerAngles += Vector3.up * 180f;
		n2.transform.localEulerAngles += Vector3.up * 180f;
		//i.transform.localScale -= Vector3.right/2f;
		n2.transform.parent = n.transform.parent = m.transform;
		//i.transform.parent = 	
		n.transform.localPosition = new Vector3(radius*r/3f, 0, -(radius*r - radius/thick));//radius + radius/10f);
		n2.transform.localPosition = new Vector3(radius*r/3f, 0, radius*r - radius/thick);//radius - radius/10f);
		return m;
	}

	static public GameObject N()
	{
		/*GameObject n = U_();
		n.transform.localScale = new Vector3(-1, 1, 1);
		n.transform.localEulerAngles += Vector3.up * 180f;
		return n;*/
		float r = 1f;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*(180 + (90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*(180 + (90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius*r - radius/(thick*0.5f) : radius*r;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		GameObject u = GetGameObject(mesh);
		GameObject[] i_ = new GameObject[]{I_(), I_()};
		
		i_[0].transform.parent = i_[1].transform.parent = u.transform;
		i_[0].transform.localScale -= Vector3.right/2f;
		i_[1].transform.localScale -= Vector3.right/2f;
		
		i_[0].transform.localPosition = new Vector3(-radius*r/2f, 0, radius*r - radius/thick);
		i_[1].transform.localPosition = new Vector3(-radius*r/2f, 0, -radius*r + radius/thick);
		
		return u;
	}
	
	static public GameObject O()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			
			float tempRadius = i%2==0 ? radius - radius/(thick*0.5f) : radius;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject P()
	{
		GameObject o = O (), i = I_P();
		i.transform.localPosition = Vector3.forward*radius - Vector3.forward* radius/thick;
		i.transform.localPosition -= Vector3.right*radius;
		
		//i.transform.localScale -= Vector3.right*radius;
		
		i.transform.parent = o.transform;
		
		return o;
	}

	static public GameObject Q()
	{
		GameObject o = O (), i = I_();
		i.transform.localPosition = -Vector3.forward*radius + Vector3.forward* radius/thick;
		i.transform.localPosition -= Vector3.right*radius;
		
		i.transform.parent = o.transform;
		
		return o;
	}
	
	static public GameObject R()
	{
		GameObject[] i_ = new GameObject[]{ I_(), I_() };
		GameObject r_ = new GameObject();
		//thick = 10;
		GameObject m = GetGameObject(QuarterCircle(0.8f));
		
		m.transform.parent = i_[0].transform.parent = i_[1].transform.parent = r_.transform;
		
		m.transform.localPosition = new Vector3(radius/5f, 0, -(radius/10f)*7f);//-0.35f);
		
		i_[1].transform.localEulerAngles = Vector3.up * 90;
		
		//i.transform.parent = l.transform;
		
		i_[0].transform.localScale = new Vector3(0.6f, 1, 1);
		i_[0].transform.localPosition -= Vector3.right * 2 * radius/5f;
		
		//i.transform.localEulerAngles = Vector3.up * 90;
		i_[1].transform.localScale -= Vector3.right*0.75f;
		i_[1].transform.localPosition += Vector3.right*radius - Vector3.right*radius/10f - Vector3.forward*(radius - radius/20f);//*0.475f;
		
		i_[0].transform.localPosition += Vector3.forward*radius/1.8f;
		i_[1].transform.localPosition += Vector3.forward*radius/1.8f;
		m.transform.localPosition += Vector3.forward*radius/1.8f;
		
		return r_;
	}

	static public GameObject S()
	{
		float r = 0.5f + (0.05f/(thick/10f));
		GameObject[] u = new GameObject[]{ U_ (r), U_ (r) };
		GameObject s = new GameObject();
		u[0].transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
		Object.Destroy(u[0].transform.GetChild(0).gameObject);
		u[0].transform.localEulerAngles = Vector3.up * 90;
		u[1].transform.localEulerAngles = -Vector3.up * 90;

		u[0].transform.parent = u[1].transform.parent = s.transform;

		u[0].transform.localPosition = new Vector3(radius*r - radius/thick, 0, radius/4f);
		u[1].transform.localPosition = new Vector3(-(radius*r - radius/thick), 0, -radius/4f);


		//u[0].transform.GetChild(0).localScale = new Vector3(0.2f, 1, 1);
		u[1].transform.GetChild(0).localScale = new Vector3(0.25f, 1, 1);
		u[1].transform.GetChild(0).localPosition = new Vector3((radius*0.25f), u[1].transform.GetChild(0).localPosition.y, u[1].transform.GetChild(0).localPosition.z);//-= Vector3.right * (radius*0.25f);//0.001f;

		u[0].transform.GetChild(1).localPosition = new Vector3(radius/2f, 0, radius*r - radius/thick);
		u[1].transform.GetChild(1).localPosition = new Vector3(radius/2f, 0, radius*r - radius/thick);

		return s;
	}

	static public GameObject T()
	{
		/*GameObject l = L (), i = I_(), i2 = I_();
		i.transform.parent = l.transform;
		i2.transform.parent = l.transform;

		float height = 0.9f;
		float scale = 0.3f;

		i.transform.localEulerAngles = Vector3.up * 90;
		i.transform.localScale -= Vector3.right - Vector3.right*scale;
		i.transform.localPosition += Vector3.right*radius*height - Vector3.forward * (scale + 0.1f) * radius;//0.018f

		i2.transform.localEulerAngles = Vector3.up * 90;
		i2.transform.localScale -= Vector3.right - Vector3.right*scale;
		i2.transform.localPosition += Vector3.right*radius*height + Vector3.forward * (scale + 0.1f) * radius;

		GameObject bottom = GetGameObject(QuarterCircle(0.8f));
		bottom.transform.parent = l.transform;
		bottom.transform.localEulerAngles = Vector3.up * (-90);
		bottom.transform.localPosition = new Vector3(0, 0, -radius + radius/10f);
		
		return l;*/


		GameObject l = L (), i = I_ ();//, i2 = I_();
		i.transform.parent = l.transform;
		//i2.transform.parent = l.transform;

		i.transform.localEulerAngles = Vector3.up * 90;
		i.transform.localScale -= Vector3.right*(radius/3f);
		//i.transform.localPosition += Vector3.right*radius/thick - Vector3.forward * 0.45f * radius;//0.018f

//		i2.transform.localEulerAngles = Vector3.up * 90;
//		i2.transform.localScale -= Vector3.right - Vector3.right*0.35f;
//		i2.transform.localPosition += Vector3.right*radius/thick + Vector3.forward * 0.45f * radius;
		
		return l;


	}

	static public GameObject U()
	{
		return U_(1f);
	}

	static public GameObject U_(float r = 1f)
	{
		
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius*r - radius/(thick*0.5f) : radius*r;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		GameObject u = GetGameObject(mesh);
		GameObject[] i_ = new GameObject[]{I_(), I_()};
		
		i_[0].transform.parent = i_[1].transform.parent = u.transform;
		i_[0].transform.localScale -= Vector3.right/2f;
		i_[1].transform.localScale -= Vector3.right/2f;
		
		i_[0].transform.localPosition = new Vector3(radius*r/2f, 0, radius*r - radius/thick);
		i_[1].transform.localPosition = new Vector3(radius*r/2f, 0, -radius*r + radius/thick);
		
		return u;
	}




	static public GameObject U__(float r = 1f)
	{
		
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius*r - radius/(thick*0.5f) : radius*r;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		GameObject u = GetGameObject(mesh);
		GameObject[] i_ = new GameObject[]{I_(), I_()};
		
		i_[0].transform.parent = i_[1].transform.parent = u.transform;
		i_[0].transform.localScale -= Vector3.right/2f;
		i_[1].transform.localScale -= Vector3.right/2f;
		
		i_[0].transform.localPosition = new Vector3(r, 0, radius*r - radius/thick);
		i_[1].transform.localPosition = new Vector3(r, 0, -radius*r + radius/thick);


		//u.transform.localPosition += Vector3.right*(radius*r/3f);
		i_[0].transform.localScale = new Vector3(0.625f, 1, 1);
		i_[0].transform.localPosition = new Vector3((radius*r) - (radius*r)/6f, i_[0].transform.localPosition.y, i_[0].transform.localPosition.z);

		i_[1].transform.localScale = new Vector3(0.625f, 1, 1); //radius + (radius*r/3f)
		i_[1].transform.localPosition = new Vector3((radius*r) - (radius*r)/6f, i_[1].transform.localPosition.y, i_[1].transform.localPosition.z);
		return u;
	}

	static public GameObject V()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[6];

		float r = radius;

		verts[0] = new Vector3(-radius, 0, -radius/thick);
		verts[1] = new Vector3(-radius, 0, radius/thick);
		verts[2] = new Vector3(radius, 0, r);
		verts[3] = new Vector3(radius, 0, r - radius/(thick*0.5f));
		
		verts[4] = new Vector3(radius, 0, -r);
		verts[5] = new Vector3(radius, 0, radius/(thick*0.5f) - r);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3,
			0, 1, 4,
			1, 5, 4
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject Y()
	{
		GameObject g = G(), u = U ();
		//UnityEngine.Object.Destroy(g.GetComponent<Renderer>());
		g.GetComponent<MeshFilter>().mesh = null;
		u.transform.parent = g.transform;
		return g;
	}

	static public GameObject W()
	{
		GameObject w = M ();
		w.transform.localEulerAngles += Vector3.up * 180;
		return w;
	}

	static public GameObject X()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[8];
		
		float r = radius * 0.9f;

		verts[0] = new Vector3(-radius, 0, r-radius/(thick*0.5f));
		verts[1] = new Vector3(-radius, 0, r);
		verts[2] = new Vector3(radius, 0, -r);
		verts[3] = new Vector3(radius, 0, -r + radius/(thick*0.5f));
		
		verts[4] = new Vector3(radius, 0, r);
		verts[5] = new Vector3(radius, 0, r - radius/(thick*0.5f));
		verts[6] = new Vector3(-radius, 0, -r);
		verts[7] = new Vector3(-radius, 0, radius/(thick*0.5f) - r);
		
		int[] tris = new int[] {
			0, 1, 3,
			3, 2, 0,
			6, 4, 5,
			7, 4, 6
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return GetGameObject(mesh);
	}

	static public GameObject Z()
	{
		GameObject z = new GameObject("Z"), top = I_(), bottom = I_(), center = I_();

		top.transform.parent = bottom.transform.parent = center.transform.parent = z.transform;

		top.transform.localEulerAngles += Vector3.up * 90f;
		bottom.transform.localEulerAngles += Vector3.up * 90f;

		top.transform.localPosition = new Vector3(radius - radius/thick, 0, 0);
		bottom.transform.localPosition = new Vector3(-radius + radius/thick, 0, 0);

		center.transform.localEulerAngles += Vector3.up * 47f;
		center.transform.localScale += Vector3.right * 0.275f;

		return z;
	}



	static public GameObject QuestionMark()
	{
		GameObject question = new GameObject("?");

		GameObject circle = GetGameObject(CustomMesh.Circle(20));
		circle.transform.localScale = Vector3.one * radius/(thick*0.5f);
		circle.transform.parent = question.transform;
		
		circle.transform.localPosition = -Vector3.right * radius + Vector3.right * radius/thick;



		GameObject rounding = GetGameObject(Word.QuarterCircle(0.5f));
		rounding.transform.localPosition = new Vector3(-radius/2f, 0, -radius/2.5f);
		rounding.transform.parent = question.transform;


		rounding = GetGameObject(Word.QuarterCircle(0.5f));
		rounding.transform.localScale = new Vector3(-1, 1, -1);
		rounding.transform.localPosition = new Vector3(radius/3.3333333f, 0, -radius/2.5f);
		rounding.transform.parent = question.transform;




		rounding = GetGameObject(Word.QuarterCircle(0.5f));
		rounding.transform.localScale = new Vector3(1, 1, -1);
		rounding.transform.localPosition = new Vector3(radius/2f, 0, -radius/2.5f);
		rounding.transform.parent = question.transform;


		rounding = GetGameObject(Word.QuarterCircle(0.5f));

		rounding.transform.localPosition = new Vector3(radius/2f, 0, radius/2.5f);
		rounding.transform.parent = question.transform;


		GameObject[] line = new GameObject[]{ I_(), I_() };

		line[0].transform.localScale = new Vector3(0.1f, 1, 1);
		line[0].transform.localPosition = new Vector3(radius/2.5f, 0, -radius/1.25f);


		line[1].transform.localEulerAngles = Vector3.up * 90f;
		line[1].transform.localScale = new Vector3(0.4f, 1, 1);
		line[1].transform.localPosition = new Vector3(radius - radius/thick, 0, 0);

		line[0].transform.parent = line[1].transform.parent = question.transform;


		return question;
	}


	static public GameObject Dot()
	{
		GameObject dot = new GameObject("Dot");
		
		GameObject circle = GetGameObject(CustomMesh.Circle(20));
		circle.transform.localScale = Vector3.one * radius/(thick*0.25f);
		circle.transform.parent = dot.transform;
		
		circle.transform.localPosition = -Vector3.right * radius + Vector3.right * radius/(thick*0.5f);
		

		return dot;
	}

	static public GameObject Colon()
	{
		GameObject obj = new GameObject("Colon");
		GameObject circle = GetGameObject(CustomMesh.Circle(20));
		circle.transform.localScale = Vector3.one * radius/3f;
		circle.transform.parent = obj.transform;
		
		circle.transform.localPosition = Vector3.right * radius*0.6f - Vector3.right * radius/6f;


		circle = GetGameObject(CustomMesh.Circle(20));
		circle.transform.localScale = Vector3.one * radius/3f;
		circle.transform.parent = obj.transform;
		
		circle.transform.localPosition = -Vector3.right * radius + Vector3.right * radius/6f;
		
		return obj;
	}

}
