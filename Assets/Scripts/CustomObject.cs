using UnityEngine;
using System.Collections;

public class CustomObject
{
	static public GameObject CreateObject(string type = "GameObject", Obj.Colour color = Obj.Colour.WHITE)
	{
		GameObject obj = new GameObject(type);
		obj.AddComponent<MeshFilter>();//.mesh = (Mesh) typeof(CustomMesh).GetMethod(type).Invoke(null, null); //CustomMesh.CurveWall();
		obj.AddComponent<MeshRenderer>().material = Game.BaseMaterial;
		obj.GetComponent<Renderer>().material.color = Game.GetColor(color);


		return obj;
		//type.GetMethod("Join").Invoke(null, new object[]{node.ChildNodes[i], obj});
	}

	static public GameObject CreateBlockRoom()
	{
		GameObject room = BlockRoom.Create();

		return room;
	}

	static public GameObject CreatePrimitive(PrimitiveType type, bool collision = true, bool twoSideMesh = false)
	{
		GameObject obj = GameObject.CreatePrimitive(type);

		if(type == PrimitiveType.Sphere)
		{
			obj.GetComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(5, 0.5f);
		}
		else if(type == PrimitiveType.Quad && twoSideMesh)
		{
			Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
			//int[] tris = new int[mesh.triangles.Length * 2];
			string s = "";
			foreach(int i in mesh.triangles)
				s += i.ToString() + " ";
			int[] tris = new int[] {
				0, 1, 2,
				1, 0, 3,
				2, 1, 0,
				3, 0, 1
			};
			//Debug.LogWarning(s);
			/*for(int i=0; i<mesh.triangles.Length*2; ++i)
			{
				// 0 1 2 3 4 5 6 7 8 9
				if(i >= mesh.triangles.Length)
				{
					if(i%3 == 0)
					{
						tris[i] = mesh.triangles[i - mesh.triangles.Length + 1];
					}
					else if((i-1)%3 == 0)
					{
						tris[i] = mesh.triangles[i - mesh.triangles.Length - 1];
					}
				}
				else
				{

					tris[i] = mesh.triangles[i];
				}
			}*/

			mesh.triangles = tris;
		}


		obj.GetComponent<Renderer>().material = Game.BaseMaterial;
		if(!collision)
		{
			Object.Destroy(obj.GetComponent<Collider>());
		}
		return obj;
	}

	static public GameObject CreateAtom(int eCount)
	{
		float coreSize = 0.4f;

		GameObject atom, core;
		GameObject[] e = new GameObject[eCount];

		atom = new GameObject("Atom");
		core = CustomObject.CreatePrimitive(PrimitiveType.Sphere);
		core.transform.localScale = Vector3.one*coreSize;
		Object.Destroy(core.GetComponent<Collider>());
		core.GetComponent<Renderer>().material = Ball.whiteBall;
		core.transform.parent = atom.transform;

		for(int i=0; i<eCount; ++i)
		{
			GameObject tmp = CustomObject.CreatePrimitive(PrimitiveType.Sphere);
			e[i] = new GameObject("Electron");
			Object.Destroy(tmp.GetComponent<Collider>());
			tmp.transform.localScale = Vector3.one*0.05f;
			tmp.transform.parent = e[i].transform;

			tmp.transform.localPosition = Vector3.up*coreSize/2f + Vector3.up * Random.value * 0.6f;
			e[i].transform.transform.parent = atom.transform;
			e[i].transform.localRotation = Random.rotationUniform;

			tmp.GetComponent<Renderer>().material = Ball.blackBall;


			e[i].AddComponent<Animation>().wrapMode = WrapMode.Loop;


			float timeForAnimation = 2f;

			Quaternion start = e[i].transform.localRotation;//Quaternion.Euler(new Vector3(90, 0, 0));
			Quaternion qua = Quaternion.Euler(e[i].transform.localEulerAngles + new Vector3(0, 180, 0));//Quaternion.Euler(new Vector3(90, 180, 0));//Quaternion.Euler(new Vector3(90, 180, 0));
			Quaternion qua2 = Quaternion.Euler(e[i].transform.localEulerAngles + new Vector3(0, 360, 0));//Quaternion.Euler(new Vector3(90, 360, 0));

			
			AnimationCurve curveX = new AnimationCurve(new Keyframe(0, start.x), new Keyframe(timeForAnimation/2f, qua.x), new Keyframe(timeForAnimation, qua2.x));
			AnimationCurve curveY = new AnimationCurve(new Keyframe(0, start.y), new Keyframe(timeForAnimation/2f, qua.y), new Keyframe(timeForAnimation, qua2.y));
			AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, start.z), new Keyframe(timeForAnimation/2f, qua.z), new Keyframe(timeForAnimation, qua2.z));		AnimationCurve curveW = new AnimationCurve(new Keyframe(0, start.w), new Keyframe(timeForAnimation/2f, qua.w), new Keyframe(timeForAnimation, qua2.w));
			
			curveX.SmoothTangents(0, 0);
			curveX.SmoothTangents(1, 0);
			curveX.SmoothTangents(2, 0);
			
			curveY.SmoothTangents(0, 0);
			curveY.SmoothTangents(1, 0);
			curveY.SmoothTangents(2, 0);
			
			curveZ.SmoothTangents(0, 0);
			curveZ.SmoothTangents(1, 0);
			curveZ.SmoothTangents(2, 0);
			
			curveW.SmoothTangents(0, 0);
			curveW.SmoothTangents(1, 0);
			curveW.SmoothTangents(2, 0);
			
			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			clip.SetCurve("", typeof(Transform), "localRotation.x", curveX);
			clip.SetCurve("", typeof(Transform), "localRotation.y", curveY);
			clip.SetCurve("", typeof(Transform), "localRotation.z", curveZ);
			clip.SetCurve("", typeof(Transform), "localRotation.w", curveW);
			clip.EnsureQuaternionContinuity();



			e[i].GetComponent<Animation>().AddClip(clip, "anim");
			
			e[i].GetComponent<Animation>().Play("anim");


		}

		return atom;
	}


	static public Switcher CreateSwitcher()
	{
		GameObject obj = new GameObject("Switcher");
		Switcher switcher = obj.AddComponent<Switcher>();
		switcher.Init();


		return switcher;
	}

	static public GameObject ArrowForGravityGun(float radius = 0.5f, float D = 1f, int vertsCount = 25)
	{
		GameObject arrow = CreateObject("GravityGunArrow", Obj.Colour.BLACK);

		arrow.GetComponent<MeshFilter>().mesh = CustomMesh.ArrowForGravityGun(radius, D, vertsCount);

		return arrow;
	}

	static public GameObject Exit()
	{
		GameObject exit = new GameObject("Exit"), ptr = CustomObject.Pointer();

		float height = 1.6f, width = 1, thick = 0.08f, doorHeight = width*0.25f;
		GameObject[] lines = new GameObject[8];

		lines[0] = Word.I__(height/2f - doorHeight, thick);
		lines[7] = Word.I__(height/2f - doorHeight, thick);
		lines[1] = Word.I__(height, thick);
		lines[2] = Word.I__(height, thick);
		lines[3] = Word.I__(width, thick);
		lines[4] = Word.I__(width - doorHeight, thick);
		//lines[4] = Word.I__(width, thick);
		lines[5] = Word.I__(width/4f * Mathf.Sqrt(2), thick);
		lines[6] = Word.I__(width/4f * Mathf.Sqrt(2), thick);



		foreach(GameObject obj in lines)
		{
			obj.transform.parent = exit.transform;
		}
		ptr.transform.parent = exit.transform;
		ptr.transform.localScale = Vector3.one * 0.3f;
		ptr.transform.localPosition = Vector3.right * (-0.5f);

		lines[0].transform.localPosition = new Vector3(-width/2f, 0, -height/2f);
		lines[0].transform.localEulerAngles = new Vector3(0, -90, 0);

		lines[7].transform.localPosition = new Vector3(-width/2f, 0, doorHeight);
		lines[7].transform.localEulerAngles = new Vector3(0, -90, 0);

		lines[1].transform.localPosition = new Vector3(width/2f, 0, -height/2f);
		lines[1].transform.localEulerAngles = new Vector3(0, -90, 0);

		lines[2].transform.localPosition = new Vector3(width/2f - doorHeight, 0, -height/2f - doorHeight);
		lines[2].transform.localEulerAngles = new Vector3(0, -90, 0);

		lines[3].transform.localPosition = new Vector3(-width/2f, 0, height/2f - thick/2f);
		lines[4].transform.localPosition = new Vector3(-width/2f, 0, -height/2f + thick/2f);

		lines[5].transform.localPosition = new Vector3(width/2f - thick/9f, 0, height/2f - thick/2.6f);
		lines[5].transform.localEulerAngles = new Vector3(0, 135, 0);
		lines[6].transform.localPosition = new Vector3(width/2f + thick/9f, 0, -height/2f + thick/2.6f);
		lines[6].transform.localEulerAngles = new Vector3(0, 135, 0);

		return exit;

	}

	static public GameObject AnimationCube(Vector3 size, Obj.Colour color = Obj.Colour.WHITE)
	{
		GameObject cube = new GameObject("CustomCube");
		GameObject qb = CustomObject.CreatePrimitive(PrimitiveType.Cube);
		qb.name = "QB";
		qb.transform.localScale = size;
		qb.GetComponent<Renderer>().material.color = Game.GetColor(color);
		
		Color c = Game.GetColor( Game.ReverseColor(color) );
		
		GameObject[] edge = new GameObject[12];
		
		for(int i=0; i<edge.Length; ++i)
		{
			edge[i] = new GameObject(i.ToString());
			GameObject edgeChild = CustomObject.CreatePrimitive(PrimitiveType.Cube, false);
			edgeChild.transform.parent = edge[i].transform;
			
			edgeChild.GetComponent<Renderer>().material.color = c;
			edge[i].name = i.ToString();
			edge[i].transform.parent = cube.transform;
			
			Vector3 tmp = Vector3.one * Line.height;
			tmp[i/4] = size[i/4];
			
			edge[i].transform.localScale = tmp;
			
			tmp = Vector3.zero;
			
			int ind1 = i/4 == 0 ? 1 : 0;
			int ind2 = i/4 != 2 ? 2 : 1;
			//int ind3 = (ind1==0 && ind2==1) ? 2 : ( (ind1==0 && ind2==2) ? 1 : 0 );
			//Debug.Log(i.ToString() + "__" + ((2 * (int)(i/2))).ToString());
			tmp[ind1] = size[ind1]/2f * ((2 * (int)(i/2))%4==0 ? 1 : -1);
			tmp[ind2] = size[ind2]/2f * (i%2==0 ? 1 : -1);

			tmp[i/4] = size[i/4] * 0.5f;

			edge[i].transform.localPosition = tmp;

			Vector3 childPosition = Vector3.zero;
			childPosition[i/4] = -0.5f;
			edgeChild.transform.localPosition = childPosition;

			edge[i].AddComponent<Animation>();
			/*Vector3 zeroScale = edge[i].transform.localScale;
			zeroScale[i/4] = 0;

			Vector3 oneScale = edge[i].transform.localScale;
			oneScale[i/4] = 1f;

			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, zeroScale, oneScale, Game.drawTime);
			edge[i].AddComponent<Animation>().AddClip(clip, "Draw");*/
		}
		qb.transform.parent = cube.transform;
		return cube;
	}

	static public GameObject Cube(Vector3 size, Obj.Colour color = Obj.Colour.WHITE)
	{
		GameObject cube = new GameObject("CustomCube");
		GameObject qb = CustomObject.CreatePrimitive(PrimitiveType.Cube);
		qb.name = "QB";
		qb.transform.localScale = size;
		qb.GetComponent<Renderer>().material.color = Game.GetColor(color);
		
		Color c = Game.GetColor( Game.ReverseColor(color) );
		
		GameObject[] edge = new GameObject[12];
		
		for(int i=0; i<edge.Length; ++i)
		{
			edge[i] = CustomObject.CreatePrimitive(PrimitiveType.Cube, false);
			
			edge[i].GetComponent<Renderer>().material.color = c;
			edge[i].name = i.ToString();
			edge[i].transform.parent = cube.transform;
			
			Vector3 tmp = Vector3.one * Line.height;
			tmp[i/4] = size[i/4];
			
			edge[i].transform.localScale = tmp;
			
			tmp = Vector3.zero;
			
			int ind1 = i/4 == 0 ? 1 : 0;
			int ind2 = i/4 != 2 ? 2 : 1;
			//Debug.Log(i.ToString() + "__" + ((2 * (int)(i/2))).ToString());
			tmp[ind1] = size[ind1]/2f * ((2 * (int)(i/2))%4==0 ? 1 : -1);
			tmp[ind2] = size[ind2]/2f * (i%2==0 ? 1 : -1);
			
			edge[i].transform.localPosition = tmp;
		}
		qb.transform.parent = cube.transform;
		return cube;
	}

	static public GameObject CreateSingleBubble(float radius, Obj.Colour color, float beginTime = 0.0f, float endTime = 1.5f, float scale = 1f, float beginScale = 0f)
	{
		GameObject circle = CustomObject.Circle(radius, color);
		//circle.GetComponent<Renderer> ().material.renderQueue -= 1;

		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		AnimationCurve curve = new AnimationCurve(new Keyframe(beginTime, beginScale), new Keyframe(endTime, scale));
		
		clip.SetCurve("", typeof(Transform), "localScale.x", curve);
		clip.SetCurve("", typeof(Transform), "localScale.y", curve);
		clip.SetCurve("", typeof(Transform), "localScale.z", curve);

		Animation anim = circle.AddComponent<Animation>();
		anim.AddClip(clip, "Draw");


		AnimationCurve curve2 = new AnimationCurve(new Keyframe(beginTime, scale), new Keyframe(endTime, beginScale));
		AnimationClip clip2 = new AnimationClip();
		clip2.legacy = true;
		clip2.SetCurve("", typeof(Transform), "localScale.x", curve2);
		clip2.SetCurve("", typeof(Transform), "localScale.y", curve2);
		clip2.SetCurve("", typeof(Transform), "localScale.z", curve2);
		anim.AddClip(clip2, "Hide");

		return circle;
	}

	static public GameObject CreateBubbles(int minCount, int maxCount, Vector2 size, Obj.Colour color, int renderQueue = 0)
	{
		int circleCount = UnityEngine.Random.Range(minCount, maxCount);
		
		GameObject circleParent = new GameObject("CircleParent");
		GameObject[] circle = new GameObject[circleCount];
		
		
		float minX = -size.x*0.5f;
		float maxX = -minX;
		
		float minZ = -size.y*0.5f;
		float maxZ = -minZ;
		
		for(int i=0; i<circleCount; ++i)
		{
			//float area = size.x * size.y;
			circle[i] = CustomObject.Circle(i==0 ? size.x*0.7f : UnityEngine.Random.Range(size.x/35f, size.x/2f), color); //size.x/35f, size.x/2f), color);
			Game.SetRenderQueue(circle[i], renderQueue);
			//circle[i].GetComponent<Renderer>().material.renderQueue -= 1;
			circle[i].transform.parent = circleParent.transform;
			
			if(i != 0 )
				circle[i].transform.localPosition = new Vector3(UnityEngine.Random.Range(minX, maxX), 0, UnityEngine.Random.Range(minZ, maxZ));
			
			
			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			AnimationCurve curve = new AnimationCurve(new Keyframe(i==0 ? Game.drawTime/4f : 0, 0), new Keyframe(Game.drawTime + Game.drawTime*(i==0?0.5f:0.25f), circle[i].transform.localScale.x));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curve);
			clip.SetCurve("", typeof(Transform), "localScale.y", curve);
			clip.SetCurve("", typeof(Transform), "localScale.z", curve);



			Animation anim = circle[i].AddComponent<Animation>();
			anim.AddClip(clip, "Draw");
			anim.Play("Draw");
			//curve[0] = new AnimationCurve(new Keyframe(), new Keyframe);
		}
		
		//circleParent.transform.localEulerAngles += Vector3.right * 90f;
		
		//circleParent.transform.position = door.transform.position;
		
		//circleParent.transform.parent = Level.current.transform;
		
		return circleParent;
	}

	static public GameObject Pointer()
	{
		GameObject pointer = CreateObject("Pointer", Obj.Colour.BLACK);
		pointer.GetComponent<MeshFilter>().mesh = CustomMesh.Pointer();
		return pointer;
	}

	static public GameObject Circle(float radius = 0.5f, Obj.Colour color = Obj.Colour.WHITE, bool withCollider = false, int vertsCount = 50)
	{
		GameObject obj = new GameObject("Circle");
		
		obj.AddComponent<MeshFilter>().mesh = CustomMesh.Circle(vertsCount);
		obj.AddComponent<MeshRenderer>().material = Game.BaseMaterial;
		obj.GetComponent<Renderer>().material.color = Game.GetColor(color);
		if(withCollider)
			obj.AddComponent<BoxCollider>();

		obj.transform.localScale *= (radius*2f);
		
		return obj;
	}

	static public GameObject Semiircle(Obj.Colour color = Obj.Colour.WHITE, bool withCollider = false, int vertsCount = 25)
	{
		GameObject obj = new GameObject("Semicircle");
		
		obj.AddComponent<MeshFilter>().mesh = CustomMesh.Semicircle(vertsCount);
		obj.AddComponent<MeshRenderer>().material = Game.BaseMaterial;
		obj.GetComponent<Renderer>().material.color = Game.GetColor(color);
		if(withCollider)
			obj.AddComponent<BoxCollider>();
		
		return obj;
	}

	static public GameObject SettingsButton()
	{
		float radius = 0.2f;
		GameObject border, circle;
		GameObject[] dot = new GameObject[3];
		
		border = CircleBorder(radius, Obj.Colour.BLACK, 0.01f);
		circle = Circle(radius, Obj.Colour.WHITE);
		circle.name = "Button";
	
		//circle.transform.localScale *= (radius*2f);
		
		border.transform.parent = circle.transform;
		border.transform.localPosition = Vector3.up*0.002f;
		for(int i=0; i<dot.Length; ++i)
		{
			dot[i] = Circle(radius/6f, Obj.Colour.BLACK);
			Vector3 pos = Vector3.up*0.001f;
			pos.x = (i-1) * radius/2f;
			dot[i].transform.localPosition = pos;
			dot[i].transform.parent = circle.transform;
		}
		
		return circle;
	}

	static public GameObject Arrow()
	{
		GameObject button;
		
		
		//border = CircleBorder(radius, Obj.Colour.WHITE, 0.01f);
		button = new GameObject();//Circle(radius, Obj.Colour.BLACK);
		button.name = "Button";
		//circle.transform.localScale *= (radius*2f);
		
		//border.transform.parent = circle.transform;
		GameObject arrow = Word.GetGameObject(CustomMesh.ShortPointer(), Obj.Colour.BLACK);

		arrow.transform.parent = button.transform;
		arrow.transform.localScale = new Vector3 (-0.5f, 1, -0.04f);//*1.5f;
		//arrow.transform.localPosition = new Vector3(0.15f, 0.001f, 0);
		arrow.transform.localEulerAngles = new Vector3 (0, 90f, 90f);


		if(arrow.GetComponent<Renderer>() != null)
		{
			arrow.GetComponent<Renderer>().material.renderQueue = 3000;
			//text.GetComponent<Renderer>().material = materialForMask;
		}

		if(arrow.GetComponent<Renderer>() != null)
		{
			arrow.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
			arrow.GetComponent<Renderer>().material.color = Color.white;
		}

		return button;
	}

	static public GameObject ArrowButton()
	{
		GameObject button;

		
		//border = CircleBorder(radius, Obj.Colour.WHITE, 0.01f);
		button = new GameObject();//Circle(radius, Obj.Colour.BLACK);
		button.name = "Button";
		//circle.transform.localScale *= (radius*2f);
		
		//border.transform.parent = circle.transform;
		GameObject arrow = Word.GetGameObject(CustomMesh.ShortPointer(), Obj.Colour.WHITE);
		arrow.transform.parent = button.transform;
		arrow.transform.localScale = new Vector3(-1.2f, 1, -0.25f);
		arrow.transform.localPosition = new Vector3(0.15f, 0.001f, 0);

		//button.AddComponent<BoxCollider>();

		return button;
	}

	static public GameObject ExitButton()
	{
		float radius = 0.2f;
		GameObject border, circle;
		//GameObject[] dot = new GameObject[3];
		
		border = CircleBorder(radius, Obj.Colour.BLACK, 0.01f);
		circle = Circle(radius, Obj.Colour.WHITE);
		circle.name = "Button";
		//circle.transform.localScale *= (radius*2f);
		
		border.transform.parent = circle.transform;
		border.transform.localPosition = Vector3.up*0.002f;
		GameObject exit = Exit();
		exit.transform.parent = circle.transform;
		exit.transform.localScale = new Vector3(-0.38f, 0.4f, -0.38f);
		exit.transform.localPosition = new Vector3(-0.015f, 0.001f, 0);
		/*for(int i=0; i<dot.Length; ++i)
		{
			dot[i] = Circle(radius/6f, Obj.Colour.BLACK);
			Vector3 pos = Vector3.up*0.001f;
			pos.x = (i-1) * radius/2f;
			dot[i].transform.localPosition = pos;
			dot[i].transform.parent = circle.transform;
		}*/
		
		return circle;
	}

	static public GameObject CircleBorder(float radius, Obj.Colour color = Obj.Colour.WHITE, float thick = 0.02f, int vertsCount = 50)
	{
		GameObject border = CreateObject("CircleBorder", color);
		border.GetComponent<MeshFilter>().mesh = CustomMesh.CircleBorder(radius, thick, vertsCount);
		//border.transform.localScale *= (radius*2f);
		return border;
	}

	static public GameObject SemiircleBorder(float radius, Obj.Colour color = Obj.Colour.WHITE, float thick = 0.02f)
	{
		GameObject border = CreateObject("SemircleBorder", color);
		border.GetComponent<MeshFilter>().mesh = CustomMesh.SemicircleBorder(radius, thick*50f);
		//border.transform.localScale *= (radius*2f);
		return border;
	}

	static public GameObject Hill(float length, float radius, Obj.Colour color = Obj.Colour.WHITE, int vertsCount = 25)
	{
		GameObject hill = new GameObject("Hill");
		GameObject wall = CreateObject("CurveWall", color);
		//wall.tag = "Side";
		wall.GetComponent<MeshFilter>().mesh = CustomMesh.CurveWall(vertsCount, 90, radius, true);
		wall.transform.parent = hill.transform;
		wall.transform.localScale = new Vector3(1, length, 1);
		wall.AddComponent<CapsuleCollider>().center = Vector3.zero;
		wall.layer = LayerMask.NameToLayer("Hill");

		float thick = 0.02f;
		GameObject border = SemiircleBorder(radius + thick/2f, Game.ReverseColor(color), thick);
		border.transform.parent = hill.transform;

		GameObject border2 = SemiircleBorder(radius + thick/2f, Game.ReverseColor(color), thick);
		border2.transform.parent = hill.transform;

		border.transform.localPosition = new Vector3(0, -length/2f, 0);
		border2.transform.localPosition = new Vector3(0, length/2f, 0);

		border2.transform.localScale = new Vector3(1, -1, 1);


		hill.transform.localEulerAngles = new Vector3(-90, -90, 0);
		wall.GetComponent<Renderer>().enabled = false;

		return hill;
	}

	static public GameObject SemicircleRoom(Vector3 size, Obj.Colour color = Obj.Colour.WHITE, int vertsCount = 25)
	{
		GameObject up = Semiircle(color, true), down = Semiircle(color, true);
		GameObject wall, upBorder, downBorder, semicircleRoom = new GameObject("SemicircleRoom");

		up.transform.localPosition = Vector3.up/2f;
		down.transform.localPosition = -Vector3.up/2f;

		up.transform.localEulerAngles = Vector3.forward * 180f;

		up.transform.parent = down.transform.parent = semicircleRoom.transform;

		wall = CreateObject("CurveWall", color);
		wall.tag = "Side";
		wall.GetComponent<MeshFilter>().mesh = CustomMesh.CurveWall(vertsCount);
		wall.transform.parent = semicircleRoom.transform;

		wall.AddComponent<MeshCollider>();
		//obj.AddComponent<BoxCollider>();

		downBorder = CreateObject("SemicircleBorder", Game.ReverseColor(color));
		upBorder = CreateObject("SemicircleBorder", Game.ReverseColor(color));

		downBorder.GetComponent<MeshFilter>().mesh = CustomMesh.SemicircleBorder(0.5f, size.x);
		upBorder.GetComponent<MeshFilter>().mesh = CustomMesh.SemicircleBorder(0.5f, size.x);

		upBorder.transform.localEulerAngles = Vector3.forward * 180f;

		upBorder.transform.localPosition = Vector3.up/2f;
		downBorder.transform.localPosition = -Vector3.up/2f;

		upBorder.transform.parent = downBorder.transform.parent = semicircleRoom.transform;


		/*upBorder_ = CreateObject("CurveWall", Obj.Colour.BLACK);
		downBorder_ = CreateObject("CurveWall", Obj.Colour.BLACK);
		upBorder_.transform.localPosition = Vector3.up/2f;
		downBorder_.transform.localPosition = -Vector3.up/2f;*/



		GameObject b1 = CreateObject("Border", Game.ReverseColor(color));
		b1.GetComponent<MeshFilter>().mesh = CustomMesh.CurveWall(vertsCount);
		b1.transform.localScale = new Vector3(1f, Line.height/size.y, 1f);
		b1.transform.localPosition = -Vector3.up/2f + Vector3.up*(Line.height/size.y/2f) - Vector3.forward*0.0001f;


		GameObject b2 = CreateObject("Border", Game.ReverseColor(color));
		b2.GetComponent<MeshFilter>().mesh = CustomMesh.CurveWall(vertsCount);
		b2.transform.localScale = new Vector3(1f, Line.height/size.y, 1f);
		b2.transform.localPosition = Vector3.up/2f - Vector3.up*(Line.height/size.y/2f) - Vector3.forward*0.0001f;

		b1.transform.parent = b2.transform.parent = semicircleRoom.transform;

		semicircleRoom.transform.localScale = size;

		return semicircleRoom;
	}
}
