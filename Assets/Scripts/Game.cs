/*
 * Axes:
Left stick X: X axis
Left stick Y: Y axis
Right stick X: 3rd axis
Right stick Y: 4th axis

Buttons:
Up: 4
Right: 5
Down: 6
Left: 7
Triangle: 12
Circle: 13
X: 14
Square: 15
L1: 10
L2: 8
L3: 1
R1: 11
R2: 9
R3: 2
Start: 0
Select: 3
*/

using UnityEngine;
using System.Xml;
using System.Collections;
using System;
using System.Collections.Generic;

public class Game : MonoBehaviour 
{
	public enum AnimationClipType { POSITION, ROTATION, SCALE };
	public enum VRMode
	{
		NONE,
		FIBRUM,
		GEARVR,
		OCULUS,
		CARDBOARD
	};

	public GameObject VRCam;

	public delegate void DVoid();

	static public event DVoid DrawEvent;
	static public event DVoid DestroyEvent;
	static public event DVoid PostDrawEvent;

	static public readonly float drawTime = 1.5f; //1.5f;
	static public readonly int levelsCount = 6;

	static public bool debugMode = true;

	public string buildVersion = "0.0.0";
	public int major, minor, build;
	public GameObject mobilePlayer;

	static private int gameProgress = 1;
	static Level level;
	static int currentLevel;
	static string xmlPath = "Levels/";

	Texture2D aim;
	Rect aimRect;


	static public Material BaseMaterial
	{
		get
		{
			return new Material(Shader.Find("Base"));
		}
	}

	static public int Progress
	{
		get
		{
			return gameProgress;
		}
		set
		{
			if( value > gameProgress )
			{
				gameProgress = value;
				//PlayerPrefs.SetInt("progress", gameProgress);
				PlayerPrefs.SetInt("progress", 3);
				PlayerPrefs.Save();
			}

		}
	}

	static public bool IsJoystickConnected
	{
		get
		{
			return Input.GetJoystickNames ().Length > 0;
		}
	}

	static string GetCoordinate(int index)
	{
		switch(index)
		{
			case 0:
				return "x"; break;
			case 1:
				return "y"; break;
			case 2:
				return "z"; break;
			default:
				return "w"; break;
		}
	}

	static public void CleanRAM()
	{
		System.GC.Collect();
		Resources.UnloadUnusedAssets();
	}

	static public AnimationClip CreateAnimationClip(Quaternion begin, Quaternion end, float time, float beginTime = 0f, AnimationClip clip = null)
	{
		if(clip == null)
			clip = new AnimationClip();
		clip.legacy = true;

		for(int i=0; i<4; ++i)
			clip.SetCurve("", typeof(Transform), "localRotation." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(beginTime, begin[i]), new Keyframe(beginTime + time, end[i])));

		return clip;
	}

	static public AnimationClip CreateAnimationClip(AnimationClipType type, Vector3 begin, Vector3 end, float time, float beginTime = 0f, AnimationClip clip = null)
	{
		if(clip == null)
			clip = new AnimationClip();
		clip.legacy = true;
		
		switch(type)
		{
		case AnimationClipType.POSITION:
		{
			for(int i=0; i<3; ++i)
				clip.SetCurve("", typeof(Transform), "localPosition." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(beginTime, begin[i]), new Keyframe(beginTime + time, end[i])));
		} break;
			
		case AnimationClipType.SCALE:
		{
			for(int i=0; i<3; ++i)
				clip.SetCurve("", typeof(Transform), "localScale." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(beginTime, begin[i]), new Keyframe(beginTime + time, end[i])));
		} break;

		/*case AnimationClipType.ROTATION:
		{
			for(int i=0; i<4; ++i)
				clip.SetCurve("", typeof(Transform), "localRotation." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(beginTime, begin[i]), new Keyframe(beginTime + time, end[i])));

		}*/
			
		default: break;
		}
		
		return clip;
	}

	static public AnimationClip CreateAnimationClip(AnimationClipType type, Vector3[] point, float time, float beginTime = 0f, AnimationClip clip = null)
	{
		//AnimationClip clip = new AnimationClip();
		if(clip == null)
			clip = new AnimationClip();
		clip.legacy = true;
		
		switch(type)
		{
		case AnimationClipType.POSITION:
		{
			Keyframe[][] key = new Keyframe[3][];
			key[0] = new Keyframe[point.Length];
			key[1] = new Keyframe[point.Length];
			key[2] = new Keyframe[point.Length];
			//Keyframe[] keyY = new Keyframe[point.Length];
			//Keyframe[] keyZ = new Keyframe[point.Length];





			for(int i=0; i<3; ++i)
			{
				for(int u=0; u<point.Length; ++u)
				{
					key[i][u] = new Keyframe(beginTime + (float)u*(time/(float)point.Length), point[u][i]);
				}

					AnimationCurve c = new AnimationCurve (key [i]);
					for (int u = 0; u < point.Length; ++u)
					{
						c.SmoothTangents (u, 0);
					}


				clip.SetCurve("", typeof(Transform), "localPosition." + GetCoordinate(i), c);
			}
		} break;
			
		case AnimationClipType.SCALE:
		{
			Keyframe[][] key = new Keyframe[3][];
			key[0] = new Keyframe[point.Length];
			key[1] = new Keyframe[point.Length];
			key[2] = new Keyframe[point.Length];
			//Keyframe[] keyY = new Keyframe[point.Length];
			//Keyframe[] keyZ = new Keyframe[point.Length];
			
			
			
			for(int i=0; i<3; ++i)
			{
				for(int u=0; u<point.Length; ++u)
				{
					key[i][u] = new Keyframe(beginTime + (float)u*(time/(float)point.Length), point[u][i]);
				}
				
				clip.SetCurve("", typeof(Transform), "localScale." + GetCoordinate(i), new AnimationCurve(key[i]));
			}

			//for(int i=0; i<3; ++i)
				//clip.SetCurve("", typeof(Transform), "localScale." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(time, end[i])));
		} break;
			
		default: break;
		}
		
		return clip;
	}

	static public AnimationClip CreateAnimationClip(AnimationClipType type, ArrayList point, float time, float beginTime = 0f, AnimationClip clip = null)
	{
		//AnimationClip clip = new AnimationClip();
		if(clip == null)
			clip = new AnimationClip();
		clip.legacy = true;
		
		switch(type)
		{
		case AnimationClipType.POSITION:
		{
			Keyframe[][] key = new Keyframe[3][];
			key[0] = new Keyframe[point.Count];
			key[1] = new Keyframe[point.Count];
			key[2] = new Keyframe[point.Count];
			//Keyframe[] keyY = new Keyframe[point.Length];
			//Keyframe[] keyZ = new Keyframe[point.Length];
			
			
			
			for(int i=0; i<3; ++i)
			{
				for(int u=0; u<point.Count; ++u)
				{
					key[i][u] = new Keyframe(beginTime + (float)u*(time/(float)point.Count), ((Vector3)point[u])[i]);
				}
				
				clip.SetCurve("", typeof(Transform), "localPosition." + GetCoordinate(i), new AnimationCurve(key[i]));
			}
		} break;
			
		case AnimationClipType.SCALE:
		{
			//for(int i=0; i<3; ++i)
			//clip.SetCurve("", typeof(Transform), "localScale." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(time, end[i])));
		} break;
			
		default: break;
		}
		
		return clip;
	}

	static public void Deadlock()
	{
		ShowWarning("Deadlock");
		Player.CanRestart = true;
	}

	/*static public void CreateAnimationClip(ref AnimationClip clip, AnimationClipType type, Vector3 begin, Vector3 end, float time)
	{
		//AnimationClip clip = new AnimationClip();
		//clip.legacy = true;
		
		switch(type)
		{
		case AnimationClipType.POSITION:
		{
			for(int i=0; i<3; ++i)
				clip.SetCurve("", typeof(Transform), "localPosition." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(time, end[i])));
		} break;
			
		case AnimationClipType.SCALE:
		{
			for(int i=0; i<3; ++i)
				clip.SetCurve("", typeof(Transform), "localScale." + GetCoordinate(i), new AnimationCurve(new Keyframe(0, begin[i]), new Keyframe(time, end[i])));
		} break;
			
		default: break;
		}
		
		//return clip;
	}*/


	static public int ParseInt(XmlNode xml, string attribute)
	{
		return int.Parse(xml.Attributes[attribute].Value);
	}

	static public float ParseFloat(XmlNode xml, string attribute)
	{
		return float.Parse(xml.Attributes[attribute].Value);
	}

	static public void Paint(GameObject obj, XmlNode xml)
	{
		obj.GetComponent<Renderer>().material.color = Game.GetColor( Game.GetColor(xml.Attributes["color"].Value) );
	}

	static public void Paint(GameObject obj, Obj.Colour color)
	{
		obj.GetComponent<Renderer>().material.color = GetColor(color);
	}
	
	static public string GetClassName(string name)
	{
		return name[0].ToString().ToUpper() + new string(name.ToCharArray(), 1, name.Length-1);
	}

	static public int GetInt(XmlNode xml, string attribute)
	{
		return int.Parse(xml.Attributes[attribute].Value);
	}

	static public float GetFloat(XmlNode xml, string attribute)
	{
		return float.Parse(xml.Attributes[attribute].Value);
	}
	
	static public Obj.Colour GetColor(string color)
	{
		if(color == "White")
			return Obj.Colour.WHITE;
		else if(color == "Black")
			return Obj.Colour.BLACK;
		else
		{
			Debug.LogError("Game -> GetColor()");
			return 0;
		}
	}
	
	static public Color GetColor(Obj.Colour color)
	{
		if(color == Obj.Colour.WHITE)
			return Color.white;
		else if(color == Obj.Colour.BLACK)
			return Color.black;
		else
		{
			Debug.LogError("Game -> GetColor()");
			return Color.clear;
		}
	}
	
	static public Color ReverseColor(Color color)
	{
		if(color == Color.white)
			return Color.black;
		else if(color == Color.black)
			return Color.white;
		else
		{
			Debug.LogError("Game -> ReverseColor()");
			return Color.clear;
		}
	}
	static public Obj.Colour ReverseColor(Obj.Colour color)
	{
		if(color == Obj.Colour.WHITE)
			return Obj.Colour.BLACK;
		else if(color == Obj.Colour.BLACK)
			return Obj.Colour.WHITE;
		else
		{
			Debug.LogError("Game -> ReverseColor()");
			return Obj.Colour.WHITE;
		}
	}
	/*static public Color ReverseColor(Color color)
	{
		if(color == Color.white)
			return Color.black;
		else if(color == Color.black)
			return Color.white;
		else
		{
			Debug.LogError("Game -> ReverseColor()");
			return Color.clear;
		}
	}*/
	
	
	
	static public void LoadLevel(int index)
	{
		ShowMessage("Loading level " + index.ToString() + "...");
		//Debug.Log("Index: " + index.ToString());
		//if(level != null)
			//Vector3 startPosition = level.door[0].transform.position;
		//OnDestroyEvent();
		XmlDocument doc = new XmlDocument();
		doc.LoadXml( (Resources.Load(xmlPath + "Level_" + index.ToString()) as TextAsset).text );
		Level.current = level = Obj.Create<Level>(doc.DocumentElement as XmlNode);
		//Debug.LogWarning("Level_" + level.Index.ToString());
		//if(index > 0)
		//	level.room[0].gameObject.AddComponent(Type.GetType("Level_" + level.Index.ToString()));
		//else
			level.gameObject.AddComponent(Type.GetType("Level_" + level.Index.ToString()));

		//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(level.gameObject, "Assets/Scripts/Game.cs (153,3)", "Level_" + level.Index.ToString());
		level.PostLoad();



		OnDrawEvent();
		ShowMessage("Level " + index.ToString() + " is created");

	}


	
	static public void OnDestroyEvent()
	{
		if(DestroyEvent != null)
			DestroyEvent();

		CleanRAM();
	}
	
	static public void OnDrawEvent()
	{
		if(DrawEvent != null)
			DrawEvent();
	}



	static bool scaleUp = false;

	static public void ScaleUpForAimTexture()
	{
		scaleUp = true;
	}

	static public void ScaleDownForAimTexture()
	{
		scaleUp = false;
	}

//	void CreateAimTexture(int size = 7)
//	{
//		//int size = 7;
//		aim = new Texture2D(size, size);
//		
//		for(int i=0; i<size; ++i)
//		{
//			for(int u=0; u<size; ++u)
//			{
//				aim.SetPixel(i, u, Color.clear);
//				if(i == 0 && (u > 1 && u < 5))
//					aim.SetPixel(i, u, Color.black);
//				else if(i == 6 && (u > 1 && u < 5))
//					aim.SetPixel(i, u, Color.black);
//
//				if(u == 0 && (i > 1 && i < 5))
//					aim.SetPixel(i, u, Color.black);
//				else if(u == 6 && (i > 1 && i < 5))
//					aim.SetPixel(i, u, Color.black);
//
//				if(i > 1 && i < 5 && u > 1 && u < 5)
//					aim.SetPixel(i, u, Color.white);
//				else
//				{
//					if(i == 1 && (u > 1 && u < 5))
//						aim.SetPixel(i, u, Color.white);
//					else if(i == 5 && (u > 1 && u < 5))
//						aim.SetPixel(i, u, Color.white);
//					
//					if(u == 1 && (i > 1 && i < 5))
//						aim.SetPixel(i, u, Color.white);
//					else if(u == 5 && (i > 1 && i < 5))
//						aim.SetPixel(i, u, Color.white);
//				}
//			}
//		}
//		
//		aim.SetPixel(1, 1, Color.black);
//		aim.SetPixel(1, 5, Color.black);
//		
//		aim.SetPixel(5, 1, Color.black);
//		aim.SetPixel(5, 5, Color.black);
//		
//		aim.Apply();
//
//		aimRect = new Rect(Screen.width/2 - aim.width/2, Screen.height/2 - aim.height/2, aim.width, aim.height);
//	}

	#region UNITY_MONOBEHAVIOUR_METHODS

	private bool connected = false;

	IEnumerator CheckForControllers() {
		while (true) {
			var controllers = Input.GetJoystickNames();
			if (!connected && controllers.Length > 0) {
				connected = true;
				message = "Connected";
			} else if (connected && controllers.Length == 0) {
				connected = false;
				message = "Disconnected";
			}
			yield return new WaitForSeconds(1f);
		}
	}

	string message = "Disconnected";

//	public void OnGUI()
//	{
//		GUILayout.Box (message);
//	}


	void Awake()
	{
		Cursor.visible = false;
		//CreateAimTexture();

		//RenderSettings.fog = false;
		//RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;

		RenderSettings.ambientLight = 
		//RenderSettings.ambientEquatorColor = 
		//	RenderSettings.fogColor = 
		//		RenderSettings.ambientGroundColor = 
		//			RenderSettings.ambientSkyColor = 
						Color.white;


		gameObject.AddComponent<Digit>();

		if( PlayerPrefs.HasKey("progress") )
		{
			gameProgress = PlayerPrefs.GetInt("progress");

			if(Progress < 1)
				Progress = 1;

			//if(Progress > 6)
			//{
				gameProgress = 6;
			//}

			#if UNITY_EDITOR
			//gameProgress = 6;
			//gameProgress = 9;
			#endif
		}
		else
		{
			Progress = 1;
		}

		StartCoroutine(CheckForControllers());
	}

	static public bool IsInputEscape()
	{
		return Input.GetKey(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton0); // || Input.GetKeyUp(KeyCode.R);
	}

	static public bool IsInputRestart()
	{
		return Input.GetKeyDown(KeyCode.JoystickButton12) || Input.GetKeyUp(KeyCode.R);
	}

	static public bool IsInputUseItemDown()
	{
		return Input.GetKeyDown (KeyCode.E) || Input.GetKeyDown (KeyCode.JoystickButton2) || Input.GetKeyDown (KeyCode.JoystickButton13); //|| IsInputActionButtonClickDown(); Input.GetKeyDown (KeyCode.E) || 
	}

	static public bool IsInputActionButtonClickDown()
	{
		return Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)|| Input.GetKeyDown(KeyCode.JoystickButton14); //Input.GetMouseButtonDown(0) || 
	}

	static public bool IsInputActionButtonClickUp()
	{
		return Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)|| Input.GetKeyUp(KeyCode.JoystickButton14); //Input.GetMouseButtonUp(0) || 
	}

	static public bool IsInputActionButtonClick()
	{
		return Input.GetMouseButton(0) || Input.GetMouseButton(1)|| Input.GetKey(KeyCode.JoystickButton14); //Input.GetMouseButton(0) || 
	}

	static public void ShowMessage(string msg)
	{
		Debug.Log("[MESSAGE]\n" + msg);
	}

	static public void ShowWarning(string msg)
	{
		Debug.LogWarning("[WARNING]\n" + msg);
	}

	void TestMethod()
	{
//		GameObject t2, t3;
//		GameObject t1 = Word.GetGameObject (CustomMesh.PenroseTriangle (), Obj.Colour.WHITE);
//		(t2 = Word.GetGameObject (CustomMesh.PenroseTriangle (-1), Obj.Colour.BLACK)).GetComponent<Renderer> ().material.color = Color.red;
//		(t3 = Word.GetGameObject (CustomMesh.PenroseTriangle (), Obj.Colour.BLACK)).GetComponent<Renderer> ().material.color = Color.green;

		//Tesseract.Create ();


//		Word.GetGameObject (LetterAnimation.CombineMeshes (new Mesh[] {
//			CustomMesh.PenroseTriangle (),
//			CustomMesh.PenroseTriangle (-1)
//		}));
//
//		t1.transform.localEulerAngles = Vector3.up * 90f;
//		t1.transform.position = new Vector3 (-27f, 20f, 33.4f);
//
//		t2.transform.position = new Vector3 (6.8f, 0, 0);
//
//		t3.transform.localEulerAngles = Vector3.right * 270f;
//		t3.transform.position = new Vector3 (-13.3f, -14.5f, 15.2f);

		float thick = 0.18f;
		float contourThick = 0.02f;
		float radius = 0.8f;
		Symbol firstStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, contourThick);
		Symbol secondStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, -contourThick);
		//Destroy (firstStrip.GetComponent<Symbol> ());
		//Destroy (firstStrip.GetComponent<Symbol> ());

		Symbol mainStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick - contourThick, radius, Obj.Colour.BLACK);
		mainStrip.firstStrip = firstStrip.gameObject;
		mainStrip.secondStrip = secondStrip.gameObject;
		//Symbol.Create(Symbol.Type.MOBIUS_STRIP, 0.02f, 0.97f, Obj.Colour.BLACK);

		//new Portal ();
		//CustomObject.CreateObject().GetComponent<MeshFilter>().mesh = CustomMesh.Quad();
		//Word.GetGameObject(CustomMesh.Test());
		//CustomObject.Hill(13, 0.1f);
		//Physics.gravity = 9.8f * Vector3.up;
	}

	static public void SetRenderQueue(GameObject obj, int queue)
	{
		obj.GetComponent<Renderer> ().material.renderQueue += queue;
	}

	void Start() 
	{
//		gameProgress = 3;
//		PlayerPrefs.SetInt("progress", 3);
//		PlayerPrefs.Save();
		//Player.Create(mobilePlayer);
		ShowMessage("Starting game...");

		Player.Create();
//
		if (Player.VRMode != VRMode.NONE)
		{
			Player.camera.GetComponent<Camera> ().enabled = false;
			VRCam.transform.SetParent (Player.camera.transform);
			VRCam.transform.localPosition = Vector3.zero;
			VRCam.transform.rotation = Quaternion.identity;
		}
		LoadLevel(0);


		//TestMethod();
		//Player.camera.GetComponent<Camera>().nearClipPlane= 0.01f;


		//CustomObject.ArrowButton();
		//Physics.gravity = Vector3.zero;

		//originalProjection = Player.camera.camera.projectionMatrix;
		//Word.Test();



		//CustomObject.ExitButton();

		//CustomObject.Cube(Vector3.one, Obj.Colour.WHITE);

		//originalProjection = Player.camera.camera.projectionMatrix;
		//CustomObject.CreateBlockRoom();
		//OnDrawEvent();
		//Debug.Log(level.Index);
		//OnDestroyEvent();
		//Player.Create();
		//GL.wireframe = true;
	}

	void Update()
	{
		
		//UpdateAimTextureScale();

		/*if(Input.GetKeyUp(KeyCode.P))
		{
			Renderer[] obj = (Renderer[])GameObject.FindObjectsOfType<Renderer>();

			foreach(Renderer o in obj)
			{


				if(o.GetComponent<Ball>() != null)
					o.GetComponent<Ball>().Repaint();
				else
				{
					o.material.color = Game.ReverseColor(o.material.color);
					if(o.GetComponent<Obj>() != null)
						o.GetComponent<Obj>().color = Game.ReverseColor(o.GetComponent<Obj>().color);

				}
			}
		}*/


		if(debugMode)
		{
//			if(Input.GetKeyUp(KeyCode.Alpha1))
//			{
//				Progress = 1;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha2))
//			{
//				Progress = 2;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha3))
//			{
//				Progress = 3;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha4))
//			{
//				Progress = 4;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha5))
//			{
//				Progress = 5;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha6))
//			{
//				Progress = 6;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha7))
//			{
//				Progress = 7;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha8))
//			{
//				Progress = 8;
//			}
//			else if(Input.GetKeyUp(KeyCode.Alpha9))
//			{
//				Progress = 9;
//			}
//			else if(Input.GetKeyUp(KeyCode.Space))
//			{
////				foreach(Ball obj in GameObject.FindObjectsOfType<Ball>())
////					obj.GetComponent<Rigidbody>().velocity = Vector3.zero;
//			}


		}
	}
	#endregion // UNITY_MONOBEHAVIOUR_METHODS
}
