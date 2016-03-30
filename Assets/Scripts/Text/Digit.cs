using UnityEngine;
using System.Collections;

public class Digit : MonoBehaviour
{
	static Digit digitObject;
	static public Obj.Colour digitColor = Obj.Colour.BLACK;

	void Awake()
	{
		digitObject = this;
	}

	static GameObject CreateLine(int index, float thick = 0.0f)
	{
		float radius = Word.radius;


		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(-radius/10f - radius*thick, 0, -radius);
		verts[1] = new Vector3(radius/10f + radius*thick, 0, -radius);
		verts[2] = new Vector3(radius/10f + radius*thick, 0, radius);
		verts[3] = new Vector3(-radius/10f - radius*thick, 0, radius);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;


		GameObject line = Word.GetGameObject(mesh, digitColor);
		line.name = "Digit";

		Vector3 pos = Vector3.zero, euler = Vector3.zero;
		radius = Word.radius*1.4f;
		switch(index)
		{
			case 0:
			{
				pos = -Vector3.right * radius*2f;
			} break;
			case 1:
			{
				pos = -Vector3.forward * radius - Vector3.right * radius;
				euler = Vector3.up * 90f;
			} break;
			case 2:
			{
				pos = Vector3.forward * radius - Vector3.right * radius;
				euler = Vector3.up * 90f;
			} break;
			case 4:
			{
				pos = -Vector3.forward * radius + Vector3.right * radius;
				euler = Vector3.up * 90f;
			} break;
			case 5:
			{
				pos = Vector3.forward * radius + Vector3.right * radius;
				euler = Vector3.up * 90f;
			} break;
		case 6:
		{
			pos = Vector3.right * radius*2f;
		} break;

		case 7:
		{
			pos = -Vector3.forward * radius;
			euler = Vector3.up * 90f;
		} break;
		case 8:
		{
			pos = Vector3.forward * radius;
			euler = Vector3.up * 90f;
		} break;
			default: break;
		}

		line.transform.position = pos;
		line.transform.eulerAngles = euler;

		return line;
	}

	static GameObject CreateRounding(int index, int variation, float thick = 0.0f)
	{
	
		GameObject rounding;

		if(variation == 2)
		{
			float radius = Word.radius;
			
			
			Mesh mesh = new Mesh();
			Vector3[] verts = new Vector3[4];
			
			verts[0] = new Vector3(-radius/4f + 0.125f, 0, -radius/10f + 0.2f - radius*thick);
			verts[1] = new Vector3(-radius/4f + 0.125f, 0, radius/10f + 0.2f + radius*thick);
			verts[2] = new Vector3(radius/4f + 0.125f, 0, radius/10f + 0.2f + radius*thick);
			verts[3] = new Vector3(radius/4f + 0.125f, 0, -radius/10f + 0.2f - radius*thick);
			
			int[] tris = new int[] {
				0, 1, 2,
				0, 2, 3
			};
			
			mesh.vertices = verts;
			mesh.triangles = tris;
			
			
			rounding = Word.GetGameObject(mesh, digitColor);
		}
		else if(variation == 3)
		{
			float radius = Word.radius;
			
			
			Mesh mesh = new Mesh();
			Vector3[] verts = new Vector3[4];
			
			verts[0] = new Vector3(-radius/10f + 0.2f - radius*thick, 0, -radius/4f + 0.125f);
			verts[1] = new Vector3(radius/10f + 0.2f + radius*thick, 0, -radius/4f + 0.125f);
			verts[2] = new Vector3(radius/10f + 0.2f + radius*thick, 0, radius/4f + 0.125f);
			verts[3] = new Vector3(-radius/10f + 0.2f - radius*thick, 0, radius/4f + 0.125f);
			
			int[] tris = new int[] {
				2,1,0,
				2, 0, 3
			};
			
			mesh.vertices = verts;
			mesh.triangles = tris;
			
			
			rounding = Word.GetGameObject(mesh, digitColor);
		}
		else
		{
			rounding = Word.GetGameObject(Word.QuarterCircle(0.5f, 90, thick), digitColor);
		}

		rounding.name = "Rounding";

		rounding.transform.localScale = new Vector3(1, -1, 1);
		Vector3 pos = Vector3.zero, scale = new Vector3((index/2)%2 == 0 ? -1 : 1, -1, index%2 == 0 ? -1 : 1);
		//scale.y *= -1;
		//float x = 1.2f, z = 0.5f;

		pos.x = (index < 4 ? -1 : 1) * (index > 1 && index < 6 ? 0.2f : 1.2f);
		pos.z = (index%2 == 0 ? -1 : 1) * 0.5f;



		rounding.transform.position = pos;
		rounding.transform.localScale = scale;

		return rounding;
	}

	static public float thick = 0.0f;

	static public GameObject CreateDigit(int[] rounding, int[] line)
	{
		GameObject digit = new GameObject("Digit");
		digit.tag = "Digit";


		foreach(int i in line)
		{
			GameObject obj = CreateLine(i, thick);
			obj.name = "l_" + i.ToString();
			obj.transform.parent = digit.transform;
		}

		for(int i=0; i<rounding.Length; ++i)
		{
			if(rounding[i] == 0)
				continue;
			GameObject obj = CreateRounding(i, rounding[i], thick);
			obj.name = "r_" + i.ToString();
			obj.transform.parent = digit.transform;
		}

		return digit;
	}

	static public GameObject GetDigit(int dig)
	{
		return typeof(Digit).GetMethod("_" + dig.ToString()).Invoke(null, null) as GameObject;
	}

	static public GameObject Shift(int from, int to)
	{
		//Debug.LogWarning(from.ToString() + "___" + to.ToString());
		try
		{
			return (GameObject)typeof(Digit).GetMethod("From" + from.ToString() + "To" + to.ToString()).Invoke(null, null);
		}
		catch
		{
			return (GameObject)typeof(Digit).GetMethod("_" + to.ToString()).Invoke(null, null);
			//return new GameObject();
		}
	}

	static public GameObject Shift(int to, bool del = false, float t = 0.3f)
	{
		time = t;

		//return new GameObject();
		if(del)
			return (GameObject)typeof(Digit).GetMethod("From" + to.ToString() + "ToEmpty").Invoke(null, null);
		else
		{
			try
			{
				return (GameObject)typeof(Digit).GetMethod("FromEmptyTo" + to.ToString()).Invoke(null, null);
			}
			catch
			{
				return (GameObject)typeof(Digit).GetMethod("_" + to.ToString()).Invoke(null, null);
			}
		}
	}

	static float startTime = 0, timeForRoundingAnim = 0.07f, scaleStartTime = 0;
	static ArrayList list = new ArrayList(), scaleList = new ArrayList(), delList = new ArrayList();

	static float time = 0.3f;

	static public GameObject FromEmptyTo1()
	{
		GameObject obj;
		
		obj = _1();
		
		obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = "0";
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(1.2f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_5").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_8").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_0.transform;
		
		tmp_0.transform.localScale = Vector3.zero;
		
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time, timeForRoundingAnim);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		scaleStartTime = Time.time;
		return obj;
	}

	static public GameObject From1ToEmpty()
	{
		GameObject obj;
		
		obj = _1();

		obj.transform.FindChild("r_7").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_7").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = "0";
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(-1.45f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_5").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_8").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_0.transform;
		
		tmp_0.transform.localScale = Vector3.zero;
		
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time, timeForRoundingAnim);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		startTime = Time.time;

		Destroy(obj, timeForRoundingAnim + time);
		
		return obj;
	}


	static public GameObject FromEmptyTo2()
	{
		GameObject obj;
		
		//time = 1f;
		
		obj = _2();

		obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = (time/2f).ToString();
		
		obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time + timeForRoundingAnim).ToString();
		
		obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_2").name = (time*1.5f + timeForRoundingAnim*2f).ToString();
		
		obj.transform.FindChild("r_0").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_0").transform.localScale += Vector3.forward * 2f;
		obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_0").name = (time*2f + timeForRoundingAnim*3f).ToString();
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(1.4f, 0, -0.75f);
		obj.transform.FindChild("l_6").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_6").transform.parent = tmp_0.transform;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		
		
		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(1.2f, 0, 0.7f);
		obj.transform.FindChild("l_5").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time/2f + timeForRoundingAnim);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");
		
		
		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(0, 0, 0.5f);
		obj.transform.FindChild("l_3").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time + timeForRoundingAnim*2f);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_3 = new GameObject("tmp_3");
		tmp_3.transform.parent = obj.transform;
		tmp_3.transform.localPosition = new Vector3(-0.2f, 0, -0.7f);
		obj.transform.FindChild("l_1").transform.parent = tmp_3.transform;
		tmp_3.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time*1.5f + timeForRoundingAnim*3f);
		tmp_3.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_3.GetComponent<Animation>().Play ("clip");
		
		
		
		GameObject tmp_4 = new GameObject("tmp_4");
		tmp_4.transform.parent = obj.transform;
		tmp_4.transform.localPosition = new Vector3(-1.4f, 0, -0.5f);
		obj.transform.FindChild("l_0").transform.parent = tmp_4.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_4.transform;
		tmp_4.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time*2f + timeForRoundingAnim*4f);
		tmp_4.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_4.GetComponent<Animation>().Play ("clip");
		
		scaleStartTime = Time.time;


		
		return obj;
	}

	static public GameObject From2ToEmpty()
	{
		GameObject obj;
		
		//time = 1f;
		
		obj = _2();

		obj.transform.FindChild("r_7").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_7").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = (time/2f).ToString();
		
		//obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time + timeForRoundingAnim).ToString();


		obj.transform.FindChild("r_2").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_2").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_2").name = (time*1.5f + timeForRoundingAnim*2f).ToString();
		
		//obj.transform.FindChild("r_0").transform.localEulerAngles += Vector3.up * 270f;
		//obj.transform.FindChild("r_0").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_0").name = (time*2f + timeForRoundingAnim*3f).ToString();
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(1.4f, 0, 0.5f);
		obj.transform.FindChild("l_6").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_6").transform.parent = tmp_0.transform;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		
		
		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(0.2f, 0, 0.7f);
		obj.transform.FindChild("l_5").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time/2f + timeForRoundingAnim);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");
		
		
		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(0, 0, -0.5f);
		obj.transform.FindChild("l_3").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time + timeForRoundingAnim*2f);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_3 = new GameObject("tmp_3");
		tmp_3.transform.parent = obj.transform;
		tmp_3.transform.localPosition = new Vector3(-1.2f, 0, -0.7f);
		obj.transform.FindChild("l_1").transform.parent = tmp_3.transform;
		tmp_3.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time*1.5f + timeForRoundingAnim*3f);
		tmp_3.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_3.GetComponent<Animation>().Play ("clip");
		
		
		
		GameObject tmp_4 = new GameObject("tmp_4");
		tmp_4.transform.parent = obj.transform;
		tmp_4.transform.localPosition = new Vector3(-1.4f, 0, 0.75f);
		obj.transform.FindChild("l_0").transform.parent = tmp_4.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_4.transform;
		tmp_4.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time*2f + timeForRoundingAnim*4f);
		tmp_4.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_4.GetComponent<Animation>().Play ("clip");
		
		startTime = Time.time;

		Destroy(obj, time*2.5f + timeForRoundingAnim*4f);
		
		return obj;
	}

	static public GameObject FromEmptyTo4()
	{
		GameObject obj;
		
		//time = 1f;
		
		obj = _4();
		
		
		obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time/2f).ToString();
		
		obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_4").name = (time + timeForRoundingAnim).ToString();
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(1.45f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_5").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_8").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_7").transform.parent = tmp_0.transform;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time*1.25f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(0.2f, 0, -0.7f);
		obj.transform.FindChild("l_4").transform.parent = tmp_1.transform;
		obj.transform.FindChild("r_6").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time + timeForRoundingAnim*2f);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(0, 0, 0.5f);
		obj.transform.FindChild("l_3").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time/2f + timeForRoundingAnim);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");	
		
		scaleStartTime = Time.time;
		
		return obj;
	}


	static public GameObject From4ToEmpty()
	{
		GameObject obj;
		
		//time = 1f;
		
		obj = _4();
		
		
		//obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time/2f).ToString();

		obj.transform.FindChild("r_4").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_4").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_4").name = (time + timeForRoundingAnim).ToString();
		
		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = obj.transform;
		tmp_0.transform.localPosition = new Vector3(-1.45f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_5").transform.parent = tmp_0.transform;
		obj.transform.FindChild("l_8").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_1").transform.parent = tmp_0.transform;
		obj.transform.FindChild("r_7").transform.parent = tmp_0.transform;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time*1.25f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(1.45f, 0, -0.7f);
		obj.transform.FindChild("l_4").transform.parent = tmp_1.transform;
		obj.transform.FindChild("r_6").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time + timeForRoundingAnim*2f);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(0, 0, -0.5f);
		obj.transform.FindChild("l_3").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time/2f + timeForRoundingAnim);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");	
		
		startTime = Time.time;

		Destroy(obj, time*1.5f + timeForRoundingAnim*2f);
		
		return obj;
	}

	static public GameObject FromEmptyTo8()
	{
		GameObject obj;

		obj = _8();
		
		GameObject tmp_0 = obj.transform.FindChild("l_3").gameObject;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");

		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(0.2f, 0, -0.7f);
		obj.transform.FindChild("l_4").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time/2f + timeForRoundingAnim);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");

		GameObject tmp_4 = new GameObject("tmp_4");
		tmp_4.transform.parent = obj.transform;
		tmp_4.transform.localPosition = new Vector3(-0.2f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_4.transform;
		tmp_4.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time/2f + timeForRoundingAnim);
		tmp_4.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_4.GetComponent<Animation>().Play ("clip");


		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(1.4f, 0, -0.5f);
		obj.transform.FindChild("l_6").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time + timeForRoundingAnim*2f);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");

		GameObject tmp_5 = new GameObject("tmp_5");
		tmp_5.transform.parent = obj.transform;
		tmp_5.transform.localPosition = new Vector3(-1.4f, 0, 0.5f);
		obj.transform.FindChild("l_0").transform.parent = tmp_5.transform;
		tmp_5.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time + timeForRoundingAnim*2f);
		tmp_5.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_5.GetComponent<Animation>().Play ("clip");


		GameObject tmp_3 = new GameObject("tmp_3");
		tmp_3.transform.parent = obj.transform;
		tmp_3.transform.localPosition = new Vector3(1.2f, 0, 0.7f);
		obj.transform.FindChild("l_5").transform.parent = tmp_3.transform;
		tmp_3.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time*1.5f + timeForRoundingAnim*3f);
		tmp_3.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_3.GetComponent<Animation>().Play ("clip");

		GameObject tmp_6 = new GameObject("tmp_6");
		tmp_6.transform.parent = obj.transform;
		tmp_6.transform.localPosition = new Vector3(-1.2f, 0, -0.7f);
		obj.transform.FindChild("l_1").transform.parent = tmp_6.transform;
		tmp_6.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time*1.5f + timeForRoundingAnim*3f);
		tmp_6.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_6.GetComponent<Animation>().Play ("clip");

		obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_4").name = (time/2f).ToString();

		obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_3").name = (time/2f).ToString();

		obj.transform.FindChild("r_6").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_6").transform.localScale += Vector3.forward * 2f;
		obj.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_6").name = (time + timeForRoundingAnim).ToString();

		obj.transform.FindChild("r_1").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_1").transform.localScale -= Vector3.forward * 2f;
		obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_1").name = (time + timeForRoundingAnim).ToString();


		obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = (time*1.5f + timeForRoundingAnim*2f).ToString();

		obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_0").name = (time*1.5f + timeForRoundingAnim*2f).ToString();


		obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time*2f + timeForRoundingAnim*3f).ToString();

		obj.transform.FindChild("r_2").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_2").transform.localScale += Vector3.forward * 2f;
		obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_2").name = (time*2f + timeForRoundingAnim*3f).ToString();

		scaleStartTime = Time.time;
		return obj;
	}

	static public GameObject From8ToEmpty()
	{
		GameObject obj;
		
		obj = _8();
		
		GameObject tmp_0 = obj.transform.FindChild("l_3").gameObject;
		tmp_0.transform.localScale = Vector3.zero;
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time*1.5f + timeForRoundingAnim*4f);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = obj.transform;
		tmp_1.transform.localPosition = new Vector3(1.2f, 0, -0.7f);
		obj.transform.FindChild("l_4").transform.parent = tmp_1.transform;
		tmp_1.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, timeForRoundingAnim);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_4 = new GameObject("tmp_4");
		tmp_4.transform.parent = obj.transform;
		tmp_4.transform.localPosition = new Vector3(-1.2f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_4.transform;
		tmp_4.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, timeForRoundingAnim);
		tmp_4.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_4.GetComponent<Animation>().Play ("clip");
		
		
		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(1.4f, 0, 0.5f);
		obj.transform.FindChild("l_6").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time/2f + timeForRoundingAnim*2f);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_5 = new GameObject("tmp_5");
		tmp_5.transform.parent = obj.transform;
		tmp_5.transform.localPosition = new Vector3(-1.4f, 0, -0.5f);
		obj.transform.FindChild("l_0").transform.parent = tmp_5.transform;
		tmp_5.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time/2f + timeForRoundingAnim*2f);
		tmp_5.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_5.GetComponent<Animation>().Play ("clip");
		
		
		GameObject tmp_3 = new GameObject("tmp_3");
		tmp_3.transform.parent = obj.transform;
		tmp_3.transform.localPosition = new Vector3(0.2f, 0, 0.7f);
		obj.transform.FindChild("l_5").transform.parent = tmp_3.transform;
		tmp_3.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time + timeForRoundingAnim*3f);
		tmp_3.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_3.GetComponent<Animation>().Play ("clip");
		
		GameObject tmp_6 = new GameObject("tmp_6");
		tmp_6.transform.parent = obj.transform;
		tmp_6.transform.localPosition = new Vector3(-0.2f, 0, -0.7f);
		obj.transform.FindChild("l_1").transform.parent = tmp_6.transform;
		tmp_6.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time + timeForRoundingAnim*3f);
		tmp_6.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_6.GetComponent<Animation>().Play ("clip");
		
		//obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		obj.transform.FindChild("r_4").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_4").transform.localScale += Vector3.forward * 2f;
		list.Add(obj.transform.FindChild("r_4").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_4").name = "0";


		obj.transform.FindChild("r_3").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_3").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_3").name = "0";
		
		//obj.transform.FindChild("r_6").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_6").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_6").name = (time/2f + timeForRoundingAnim).ToString();
		
		//obj.transform.FindChild("r_1").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_1").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_1").name = (time/2f + timeForRoundingAnim).ToString();
		

		obj.transform.FindChild("r_7").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_7").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_7").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_7").name = (time + timeForRoundingAnim*2f).ToString();

		obj.transform.FindChild("r_0").transform.localEulerAngles += Vector3.up * 270f;
		obj.transform.FindChild("r_0").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_0").name = (time + timeForRoundingAnim*2f).ToString();
		
		
		//obj.transform.FindChild("r_5").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_5").transform.localScale -= Vector3.forward * 2f;
		//obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_5").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_5").name = (time*1.5f + timeForRoundingAnim*3f).ToString();
		
		//obj.transform.FindChild("r_2").transform.localEulerAngles += Vector3.up * 90f;
		//obj.transform.FindChild("r_2").transform.localScale += Vector3.forward * 2f;
		//obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(obj.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_2").name = (time*1.5f + timeForRoundingAnim*3f).ToString();
		
		startTime = Time.time;

		Destroy(obj, time*2f + timeForRoundingAnim*4f);
		
		return obj;
	}

	static public GameObject From2To3()
	{
		GameObject obj = _3(), digit = _2();

		AnimationClip clip;

		digit.transform.parent = obj.transform;

		//digit.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(digit.transform.FindChild("r_2").gameObject.GetComponent<MeshFilter>());
		digit.transform.FindChild("r_2").name = (time + timeForRoundingAnim).ToString();
		
		digit.transform.FindChild("r_0").transform.localEulerAngles += Vector3.up * 270f;
		digit.transform.FindChild("r_0").transform.localScale += Vector3.forward * 2f;
		//digit.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0);
		list.Add(digit.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		digit.transform.FindChild("r_0").name = (time/2f).ToString();


		GameObject tmp_3 = new GameObject("tmp_3");
		tmp_3.transform.parent = digit.transform;
		tmp_3.transform.localPosition = new Vector3(-0.2f, 0, -0.7f);
		digit.transform.FindChild("l_1").transform.parent = tmp_3.transform;
		tmp_3.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time/2f, time/2f + timeForRoundingAnim);
		tmp_3.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_3.GetComponent<Animation>().Play ("clip");
		
		
		
		GameObject tmp_4 = new GameObject("tmp_4");
		tmp_4.transform.parent = digit.transform;
		tmp_4.transform.localPosition = new Vector3(-1.4f, 0, -0.5f);
		digit.transform.FindChild("l_0").transform.parent = tmp_4.transform;
		digit.transform.FindChild("r_1").transform.parent = tmp_4.transform;
		tmp_4.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f);
		tmp_4.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_4.GetComponent<Animation>().Play ("clip");









		obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_3").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_3").name = (time + timeForRoundingAnim*2f).ToString();

		GameObject tmp_2 = new GameObject("tmp_2");
		tmp_2.transform.parent = obj.transform;
		tmp_2.transform.localPosition = new Vector3(-0.2f, 0, 0.7f);
		obj.transform.FindChild("l_2").transform.parent = tmp_2.transform;
		tmp_2.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, 1, 1), Vector3.one, time/2f, time + timeForRoundingAnim*3f);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");

		obj.transform.FindChild("r_1").transform.localEulerAngles += Vector3.up * 90f;
		obj.transform.FindChild("r_1").transform.localScale -= Vector3.forward * 2f;
		obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 0, thick);
		scaleList.Add(obj.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>());
		obj.transform.FindChild("r_1").name = (time*1.5f + timeForRoundingAnim*3).ToString();

		GameObject tmp_5 = new GameObject("tmp_5");
		tmp_5.transform.parent = obj.transform;
		tmp_5.transform.localPosition = new Vector3(-1.4f, 0, 0.5f);
		obj.transform.FindChild("l_0").transform.parent = tmp_5.transform;
		obj.transform.FindChild("r_0").transform.parent = tmp_5.transform;
		tmp_5.transform.localScale = Vector3.zero;
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, 0), Vector3.one, time/2f, time*1.5f + timeForRoundingAnim*4f);
		tmp_5.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_5.GetComponent<Animation>().Play ("clip");


		scaleStartTime = startTime = Time.time;

		return obj;
	}

	static public GameObject From0To1(GameObject digit)
	{
		GameObject obj;

		(obj = _1()).SetActive(false);

		float time = 1f;

		GameObject tmp_0 = new GameObject("tmp_0");
		tmp_0.transform.parent = digit.transform;
		tmp_0.transform.localPosition = new Vector3(1.2f, 0, -0.7f);
		digit.transform.FindChild("l_1").transform.parent = tmp_0.transform;
		digit.transform.FindChild("l_4").transform.parent = tmp_0.transform;
		digit.transform.FindChild("l_7").transform.parent = tmp_0.transform;
		
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(0, 1, 1), time);
		tmp_0.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_0.GetComponent<Animation>().Play ("clip");



		GameObject tmp_1 = new GameObject("tmp_1");
		tmp_1.transform.parent = digit.transform;
		tmp_1.transform.localPosition = new Vector3(1.4f, 0, 0.5f);
		digit.transform.FindChild("l_6").transform.parent = tmp_1.transform;
		
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, time + timeForRoundingAnim);
		tmp_1.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_1.GetComponent<Animation>().Play ("clip");



		GameObject tmp_2 = new GameObject("tmp_1");
		tmp_2.transform.parent = digit.transform;
		tmp_2.transform.localPosition = new Vector3(-1.4f, 0, 0.5f);
		digit.transform.FindChild("l_0").transform.parent = tmp_2.transform;
		
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, new Vector3(1, 1, 0), time/2f, timeForRoundingAnim);
		tmp_2.AddComponent<Animation>().AddClip(clip, "clip");
		tmp_2.GetComponent<Animation>().Play ("clip");

		//digit.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>().mesh = Word.QuarterCircle(0.5f, 45f);

		//digitObject.RoundingAnimation(digit.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>(), time);
		list.Add(digit.transform.FindChild("r_6").gameObject.GetComponent<MeshFilter>());
		digit.transform.FindChild("r_6").name = "1";

		list.Add(digit.transform.FindChild("r_0").gameObject.GetComponent<MeshFilter>());
		digit.transform.FindChild("r_0").name = "0";

		list.Add(digit.transform.FindChild("r_1").gameObject.GetComponent<MeshFilter>());
		digit.transform.FindChild("r_1").transform.localEulerAngles += Vector3.up * 90f;
		digit.transform.FindChild("r_1").transform.localScale -= Vector3.forward * 2f;
		digit.transform.FindChild("r_1").name = (timeForRoundingAnim + time/2f).ToString();



		startTime = Time.time;

		return obj;
	}



	void FixedUpdate()
	{
		if(list.Count > 0)
		{
			foreach(MeshFilter filter in list)
			{
				//Debug.Log(list.Count);
				if(filter == null)
				{
					list.Remove(filter);
					return;
				}
				if(Time.time - startTime >= float.Parse(filter.name))
				{
					filter.transform.localScale += Vector3.up * Time.fixedDeltaTime / timeForRoundingAnim;
					filter.mesh = Word.QuarterCircle(0.5f, 90f * (-filter.transform.localScale.y), thick);
					
					if(filter.transform.localScale.y >= 0)
					{
						list.Remove(filter);
						Destroy(filter.gameObject);
						return;
					}
				}
			}
		}


		if(scaleList.Count > 0)
		{
			foreach(MeshFilter filter in scaleList)
			{
				if(filter == null)
				{
					scaleList.Remove(filter);
					return;
				}
				//if(filter != null)
				//Debug.LogWarning(scaleList.Count);
				if(Time.time - scaleStartTime >= float.Parse(filter.name))
				{
					filter.transform.localScale += Vector3.up * Time.fixedDeltaTime / timeForRoundingAnim;
					filter.mesh = Word.QuarterCircle(0.5f, 90f * (1 + filter.transform.localScale.y), thick);
					
					if(filter.transform.localScale.y >= 0)
					{
						scaleList.Remove(filter);
						filter.mesh = Word.QuarterCircle(0.5f, 90f, thick);
						filter.transform.localScale = new Vector3(filter.transform.localScale.x, -1, filter.transform.localScale.z);
						//Destroy(filter.gameObject);
						return;
					}
				}
			}
		}

	}

	public IEnumerator Wait(float time)
	{
		yield return new WaitForSeconds(time);
	}


	static public GameObject _0()
	{
		int[] rounding = new int[] {1, 1, 0, 0, 0, 0, 1, 1};
		int[] line = new int[] {0, 1, 2, 4, 5, 6, 7, 8};

		return CreateDigit(rounding, line);
	}

	static public GameObject _1()
	{
		int[] rounding = new int[] {0, 2, 0, 0, 0, 0, 0, 1};
		int[] line = new int[] {2, 5, 8};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _2()
	{
		int[] rounding = new int[] {1, 3, 1, 0, 0, 1, 3, 1};
		int[] line = new int[] {6, 5, 3, 1, 0};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _3()
	{
		int[] rounding = new int[] {3, 1, 0, 1, 0, 1, 3, 1};
		int[] line = new int[] {6, 5, 3, 2, 0};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _4()
	{
		int[] rounding = new int[] {0, 2, 0, 0, 1, 1, 2, 2};
		int[] line = new int[] {4, 3, 5, 2, 8};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _5()
	{
		int[] rounding = new int[] {3, 1, 0, 1, 1, 0, 1, 3};
		int[] line = new int[] {6, 4, 3, 2, 0};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _6()
	{
		int[] rounding = new int[] {1, 1, 1, 1, 0, 0, 1, 3};
		int[] line = new int[] {0, 1, 2, 3, 4, 6, 7};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _7()
	{
		int[] rounding = new int[] {0, 2, 0, 0, 0, 0, 3, 1};
		int[] line = new int[] {6, 5, 2, 8};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _8()
	{
		int[] rounding = new int[] {1, 1, 1, 1, 1, 1, 1, 1};
		int[] line = new int[] {0, 1, 2, 3, 4, 5, 6};
		
		return CreateDigit(rounding, line);
	}

	static public GameObject _9()
	{
		int[] rounding = new int[] {3, 1, 0, 0, 1, 1, 1, 1};
		int[] line = new int[] {0, 2, 3, 4, 5, 6, 8};
		
		return CreateDigit(rounding, line);
	}
}
