using UnityEngine;
using System.Collections;
using System.Xml;

public class Level_0 : MonoBehaviour 
{
	public int nextLevel = 1;

	Level level;

	GameObject start, settingsButton, levelIndex, exitButton;


	static public float startTime = 0f;

	MeshFilter[] curveBorder = new MeshFilter[4];

	GameObject rightArrow, leftArrow;


	/*void SetupLevelChooseArrows()
	{
		float scaleTime = 0.4f;

		rightArrow = CustomObject.ArrowButton();
		leftArrow = CustomObject.ArrowButton();

		leftArrow.transform.localEulerAngles = new Vector3(-90, 0, 0);
		rightArrow.transform.localEulerAngles = new Vector3(-90, 0, 0);



		leftArrow.transform.localScale = Vector3.one * 0.4f;
		rightArrow.transform.localScale = new Vector3(-1, 1, 1) * 0.4f;
		leftArrow.transform.parent = rightArrow.transform.parent = level.outletDoor.door.transform;
		//leftArrow.transform.localPosition = rightArrow.transform.localPosition = Vector3.zero;
		leftArrow.transform.localPosition = new Vector3(-0.4f, 0, -0.5f);
		rightArrow.transform.localPosition = new Vector3(0.4f, 0, -0.5f);


		rightArrow.AddComponent<BoxCollider>().size = Vector3.one * 0.7f;
		
		Animation anim = rightArrow.AddComponent<Animation>();
		
		anim.playAutomatically = false;
		
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, rightArrow.transform.localScale, rightArrow.transform.localScale*1.5f, scaleTime), "ScaleUp");
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, rightArrow.transform.localScale*1.5f, rightArrow.transform.localScale, scaleTime), "ScaleDown");


		leftArrow.AddComponent<BoxCollider>().size = Vector3.one * 1.1f;
		
		anim = leftArrow.AddComponent<Animation>();
		
		anim.playAutomatically = false;
		
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, leftArrow.transform.localScale, leftArrow.transform.localScale*1.5f, scaleTime), "ScaleUp");
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, leftArrow.transform.localScale*1.5f, leftArrow.transform.localScale, scaleTime), "ScaleDown");

	}*/
	bool lscale = false, rscale = false;
	/*void ChooseLevelControl()
	{
		RaycastHit hit;
		if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
		{
			if(hit.transform == rightArrow.transform)
			{
				
				if(!rscale && !rightArrow.GetComponent<Animation>().isPlaying)
				{
					rightArrow.GetComponent<Animation>().Play("ScaleUp");
					rscale = true;
					
				}
				
				if(Input.GetMouseButtonDown(0))
				{
					if(binary < Game.Progress)
					++binary;
				}
			}
			else if(hit.transform == leftArrow.transform)
			{
				
				if(!lscale && !leftArrow.GetComponent<Animation>().isPlaying)
				{
					leftArrow.GetComponent<Animation>().Play("ScaleUp");
					lscale = true;
					
				}
				
				if(Input.GetMouseButtonDown(0))
				{
					if(binary > 1)
					--binary;
				}
			}
			else
			{

					if(rscale && !rightArrow.GetComponent<Animation>().isPlaying)
					{
						rightArrow.GetComponent<Animation>().Play("ScaleDown");
						rscale = false;
					}
					else if(lscale && !leftArrow.GetComponent<Animation>().isPlaying)
					{
						leftArrow.GetComponent<Animation>().Play("ScaleDown");
						lscale = false;
					}
			}

			
		}
	}
*/

	Vector3 originalExitBtnPos = Vector3.zero, originalSettingsBtnPos = Vector3.zero;

	void AddHideAnimation(GameObject obj, Vector3 original)
	{
		Animation anim = obj.GetComponent<Animation> ();

		if (!anim)
			anim = obj.AddComponent<Animation> ();

		if (anim.GetClip ("Hide"))
			anim.RemoveClip ("Hide");

		float time = Mathf.Abs ((original.z + 1) - obj.transform.position.z)*0.5f;

		AnimationClip clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, obj.transform.position, original + Vector3.forward, time);
		anim.AddClip (clip, "Hide");

	}

	void AddShowAnimation(GameObject obj, Vector3 original)
	{
		Animation anim = obj.GetComponent<Animation> ();
		
		if (!anim)
			anim = obj.AddComponent<Animation> ();

		if (anim.GetClip ("Show"))
			anim.RemoveClip ("Show");

		float time = Mathf.Abs (original.z - obj.transform.position.z)*0.5f;

		AnimationClip clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, obj.transform.position, original, time);
		anim.AddClip (clip, "Show");
	}

	public GameObject infoMouse, infoWalk;

	IEnumerator Start() 
	{
		/*foreach(Cell c in Level.current.room[1].cell)
			c.gameObject.SetActive(false);

		foreach(Ball b in Level.current.ball)
			b.gameObject.SetActive(false);

*/

		level = Level.current;


		//Destroy( level.door[1].gameObject);
		level.outletDoor.gameObject.SetActive(false);

		level.outletDoor.openDoorTrigger = level.room[1].trigger[0];
		level.door[0].openDoorTrigger = level.room[0].trigger[0];

		level.door[2].openDoorTrigger = level.room[1].trigger[1];
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		//level.room[1].side[1].transform.localPosition += Vector3.right * 0.003f;
		//level.room[0].side[5].gameObject.SetActive(false);
		Vector3 size = level.room[0].Size;
		size.z = size.x;
		(start = CustomObject.SemicircleRoom(size)).transform.position = level.room[0].side[5].transform.position;
		start.transform.parent = level.room[0].transform;

		curveBorder[0] = start.transform.GetChild(3).GetComponent<MeshFilter>();
		curveBorder[1] = start.transform.GetChild(4).GetComponent<MeshFilter>();
		curveBorder[2] = start.transform.GetChild(5).GetComponent<MeshFilter>();
		curveBorder[3] = start.transform.GetChild(6).GetComponent<MeshFilter>();

		for(int i=0; i<4; ++i)
		{
			if(i==2)
				continue;
			curveBorder[i].transform.localScale = new Vector3(-1, curveBorder[i].transform.localScale.y, curveBorder[i].transform.localScale.z);
		}

		start.transform.GetChild(2).GetComponent<Renderer>().enabled = false;

		settingsButton = CustomObject.SettingsButton();
		settingsButton.AddComponent<Settings>().level = level;
		settingsButton.GetComponent<Renderer>().enabled = false;
		settingsButton.transform.localEulerAngles = new Vector3(90f, 270f, 0f);
		settingsButton.transform.position = level.room[1].side[1].transform.position + Vector3.forward*(level.room[1].Size.z*0.45f);
		settingsButton.transform.parent = level.transform;
		//settingsButton.SetActive(false);

		originalSettingsBtnPos = settingsButton.transform.position;

		AddHideAnimation (settingsButton, originalSettingsBtnPos);
		AddShowAnimation (settingsButton, originalSettingsBtnPos);


		exitButton = CustomObject.ExitButton();
		//exitButton.transform.localScale *= 2f;
		ExitButton esc = exitButton.AddComponent<ExitButton>();
		esc.level = level;
		exitButton.GetComponent<Renderer>().enabled = false;
		exitButton.transform.localEulerAngles = new Vector3(90f, 270f, 0f);//new Vector3(90f, 90f, 0f);
		exitButton.transform.position = level.room[1].side[1].transform.position 
			+ Vector3.forward*(level.room[1].Size.z*0.45f) 
				- Vector3.up * (level.room[1].Size.y*0.13f)
				- Vector3.right * 0.01f;//level.room[1].side[0].transform.position - Vector3.up * (level.room[1].Size.y*0.1f) ;// + Vector3.forward*(level.room[1].Size.z*0.45f);
		exitButton.transform.parent = level.transform;

		originalExitBtnPos = exitButton.transform.position;

		AddHideAnimation (exitButton, originalExitBtnPos);
		AddShowAnimation (exitButton, originalExitBtnPos);

		level.room[0].trigger[0].transform.localPosition -= Vector3.forward*4.25f;

		(esc.gameSettings = settingsButton.GetComponent<Settings>()).exit = esc;


		Player.SetPosition(level.room[1], new Vector2(50, 50));
		Player.player.transform.localEulerAngles = Vector3.up * 180f;



		GameObject gameName = Word.WriteString("coloristique", 0.5f, Obj.Colour.BLACK, true); //"abcdefghijklmn|opqrstuvwxyz" //"coloristique"
		gameName.transform.localEulerAngles = new Vector3(0, 90, 90);
		
		gameName.transform.localScale = Vector3.one * 0.8f;
		//gameName.transform.localScale = Vector3.one * 0.4f;
		
		gameName.transform.position = level.room[1].side[4].transform.position;
		gameName.transform.position += Vector3.forward*0.001f + Vector3.right*level.room[1].Size.x/2.22f;
		gameName.transform.parent = level.transform;




		infoMouse = Word.WriteString("use mouse to look around", 0.5f,  Obj.Colour.BLACK); //, true
		//Word.ApplyReverseColorShaderToString(infoMouse);
		//float a = Mathf.Atan2( Mathf.Abs (Player.camera.transform.position.z - infoMouse.transform.position.z) , Mathf.Abs (Player.camera.transform.position.y - infoMouse.transform.position.y) ) * Mathf.Rad2Deg;

		//GameObject infoMouse = Word.WriteString("use right stick to look around", 0.5f,  Obj.Colour.BLACK, true);
		infoMouse.transform.localEulerAngles = new Vector3 (0, 90, 90);
		infoMouse.transform.localScale = Vector3.one * 0.08f;
		infoMouse.transform.position = 
			level.room [1].side [4].transform.position 
			+ Vector3.forward * 0.101f 
			- Vector3.up * 0.8f 
			+ Vector3.right * level.room [1].Size.x / 7.85f;


		AnimationClip clip =  Game.CreateAnimationClip (
			Quaternion.Euler (new Vector3 (0, 90, -2)), 
			Quaternion.Euler (infoMouse.transform.localEulerAngles),
			Game.drawTime / 2f, Game.drawTime / 2f);

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, Vector3.zero, infoMouse.transform.localScale, 0.0001f, Game.drawTime / 2f, clip);


		infoMouse.AddComponent<Animation> ().AddClip (clip, "Draw");
		infoMouse.GetComponent<Animation> ().Play ("Draw");

		clip = Game.CreateAnimationClip (
			Game.AnimationClipType.POSITION, 
			infoMouse.transform.position, 
			infoMouse.transform.position - Vector3.up * 2f - Vector3.forward*0.1f, 
			0.85f, 2.25f);
		infoMouse.GetComponent<Animation> ().AddClip(clip, "Destroy");


		infoWalk = Word.WriteString("use S W A D buttons to walk", 0.5f,  Obj.Colour.BLACK); //, true
		//GameObject infoWalk = Word.WriteString("use left stick to walk", 0.5f,  Obj.Colour.BLACK, true);
		infoWalk.transform.localEulerAngles = new Vector3 (0, 90, 90);
		infoWalk.transform.localScale = Vector3.one * 0.08f;
		infoWalk.transform.position = 
			level.room [1].side [4].transform.position 
			+ Vector3.forward * 0.101f 
			- Vector3.up * 1.05f 
			+ Vector3.right * level.room [1].Size.x / 7.08f;//10.7f;


		infoWalk.transform.FindChild ("W").transform.localEulerAngles = Vector3.zero;

		clip =  Game.CreateAnimationClip (
			Quaternion.Euler (new Vector3 (0, 90, -5)), 
			Quaternion.Euler (infoWalk.transform.localEulerAngles),
			Game.drawTime / 2f, Game.drawTime / 2f);

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, Vector3.zero, infoWalk.transform.localScale, 0.0001f, Game.drawTime / 2f, clip);

		infoWalk.AddComponent<Animation> ().AddClip (clip, "Draw");
		infoWalk.GetComponent<Animation> ().Play ("Draw");

		clip = Game.CreateAnimationClip (
			Game.AnimationClipType.POSITION, 
			infoWalk.transform.position, 
			infoWalk.transform.position - Vector3.up * 2f - Vector3.forward*0.1f, 
			1f, 2f);
		infoWalk.GetComponent<Animation> ().AddClip(clip, "Destroy");


		GameObject author = Word.WriteString("the game by nikolay skakun"); //most original gameplay
		author.transform.localEulerAngles = new Vector3(0, 90, 90);
		author.transform.localScale = Vector3.one * 0.2f;
		author.transform.position = level.room[1].side[4].transform.position;
		//author.transform.position += Vector3.forward*0.001f + Vector3.right*level.room[1].Size.x/2.36f - Vector3.up*0.7f;
		author.transform.position += Vector3.forward*0.001f + Vector3.right*level.room[1].Size.x/2.3f - Vector3.up*0.8f;
		author.transform.parent = level.transform;

		GameObject music = Word.WriteString("music by johnny suomu");
		music.transform.parent = author.transform;
		music.transform.localScale = Vector3.one;
		music.transform.localPosition = Vector3.right * (-2.5f);// + Vector3.forward * (-2.3f);
		music.transform.localEulerAngles = Vector3.zero;


		/*GameObject thanks = Word.WriteString("special thanks to aleksander skakun and konstantin kravtsov");
		thanks.transform.localEulerAngles = new Vector3(0, 90, 90);
		thanks.transform.localScale = Vector3.one * 0.0845f;
		thanks.transform.position = level.room[1].side[4].transform.position;
		thanks.transform.position += Vector3.forward*0.001f + Vector3.right*level.room[1].Size.x/2.36f - Vector3.up*1.05f;
		thanks.transform.parent = author.transform;*/

		Settings.SetAbout(author);
		//Player.player.AddComponent<Gravity>();

		level.outletDoor.Repaint();
		foreach(Side s in level.outletDoor.room.side)
			foreach(Line l in s.line)
				l.GetComponent<Animation>().enabled = false;

		GameObject exit = Word.WriteString("go back to the reality", 1, Obj.Colour.WHITE); //"go back to the reality"
		exit.transform.localScale = Vector3.one * 0.06f; // 0.08f if without "back"
		exit.transform.localEulerAngles = new Vector3(0, 0, 90f);
		exit.transform.position = level.door[2].room.side[0].transform.position;
		exit.transform.parent = level.door[2].door.transform;
		exit.transform.localPosition += Vector3.forward * 0.41f;


		esc.exitLine = level.room[1].side[1].line[3].transform;
		esc.exitLineOriginalSize = esc.exitLine.localScale;
		esc.exitLine.localScale = new Vector3(esc.exitLine.localScale.x, 1.475f, esc.exitLine.localScale.z);

		GameObject exitText = Word.WriteString("are you sure?", 0.08f);

		exitText.transform.localEulerAngles = exit.transform.localEulerAngles;// + Vector3.up * 180f;
		exitText.transform.position = exit.transform.position + Vector3.right*3f - Vector3.forward*0.25f;
		exitText.transform.parent = Level.current.transform;

		startTime = Time.time;

		//Glass g = Glass.Create(new Vector2(10, 5));
		//g.transform.position = level.room[1].side[4].transform.position + Vector3.forward*0.1f;
		//CustomObject.CreateAtom(5).transform.position += Vector3.up*1.5f;
		//Player.player.AddComponent<Gravity>();
		//level.room[1].side[4].gameObject.AddComponent<KineticSide>();

		Level.ZeroRoom = level.room[1];

		//SetupLevelChooseArrows();

		levelIndex = new GameObject("LevelIndex");
		levelIndex.transform.parent = level.outletDoor.door.transform;
		levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.125f;
		levelIndex.transform.localEulerAngles = new Vector3(0, 270, 90);
		levelIndex.transform.localScale = Vector3.one*0.15f;

//		GameObject levelChoiceController = new GameObject("LevelChoiceController");
//		levelChoiceController.transform.parent = level.transform;
//		levelChoiceController.transform.position = level.room[1].side[5].transform.position
//			- Vector3.right * level.room[1].side[5].Size.x/2f
//				- Vector3.up * level.room[1].side[5].Size.y/2f 
//				- Vector3.forward * 0.02f;
//		levelChoiceController.AddComponent<LevelChoiceController>().level = level;

		//level.door[2].gameObject.SetActive(false);
		level.door[0].openDoorTrigger = null;
		level.door[2].openDoorTrigger = null;



		level.room [1].trigger [3].OnTriggerEnterPlayer += HideButtons;
		level.room [1].trigger [3].OnTriggerExitPlayer += ShowButtons;

		//Player.Create();
		//Player.SetPosition(level.room[0], new Vector2(50, 100));


		//Portal p = new Portal();



		StartCoroutine( Player.audio.PlayForTest(true) );

		Player.EnableLook(false);
		StartCoroutine( Player.DisableControl(Game.drawTime * 1.5f) );
		yield return new WaitForSeconds(Game.drawTime * 1.5f);

		Player.EnableLook(true);



		//yield return new WaitForSeconds(Game.drawTime*0.5f);


		//AudioController.PlayForTest(true);

		level.door[2].room.gameObject.SetActive(false);
		level.door[2].SetColor(Obj.Colour.WHITE);
		level.door[0].openDoorTrigger = level.room[0].trigger[0];

		esc.exitLine.localScale = new Vector3(esc.exitLine.localScale.x, 1.475f, esc.exitLine.localScale.z);






		level.door[0].openDoorTrigger = null;
		//level.door[0].Close();
		level.room[0].trigger[0].gameObject.SetActive(false);
		level.door[0].room.side[4].GetComponent<Collider>().enabled = true;
		if(level.door[0].room.side[4].GetComponent<BoxCollider>() != null)
			level.door[0].room.side[4].GetComponent<BoxCollider>().enabled = true;
		inLoadLevelRoom = true;
		//level.room[1].trigger[0].gameObject.SetActive(false);
		level.outletDoor.openDoorTrigger = null;

		level.door[0].room.side[4].GetComponent<Collider>().enabled = false;
		if(level.door[0].room.side[4].GetComponent<BoxCollider>() != null)
			level.door[0].room.side[4].GetComponent<BoxCollider>().enabled = false;
		Destroy(level.room[0].gameObject);
		level.outletDoor.gameObject.SetActive(true);
		level.outletDoor.transform.position = level.door[0].transform.position;
		Destroy(level.door[0].gameObject);
		
		
		foreach(Line line in level.outletDoor.room.side[4].line)
			line.transform.localEulerAngles += Vector3.up * 180;
		//level.outletDoor.room.side[4].gameObject.SetActive(false);
		
		foreach(Side side in level.outletDoor.room.side)
		{
			for(int i=0; i<side.line.Length; ++i)
			{
				if(!side.line[i].gameObject.activeSelf && i==3 && (side.index<2 || side.index>3))
					continue;
				side.line[i].gameObject.SetActive(true);
			}
			
			binary = 1; //Game.Progress;//1;
			/*foreach(Line line in side.line)
					{
						if(line.in
						line.gameObject.SetActive(true);
					}*/
		}
		//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
		/*foreach(Line l in level.room[1].side[1].line)
		{
			l.transform.localPosition -= Vector3.forward * 0.003f;
		}*/

		//Escape();
		//yield return new WaitForSeconds(Game.drawTime*1.5f);
		//level.door[2].gameObject.SetActive(true);
	}

	static public bool hideInfo = false;
	bool inLoadLevelRoom = false, usedMouse = false, usedKeys = false;
	public int binary = 0, previousBinary = 0;

	float borderSize = 0f;

	public void UpdateLevelIndex(Door door)
	{
		if(levelIndex == null)
			levelIndex = new GameObject("LevelIndex");

		levelIndex.transform.parent = door.door.transform;
		levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.125f;
		levelIndex.transform.localEulerAngles = new Vector3(0, 270, 90);
		levelIndex.transform.localScale = Vector3.one*0.15f;
	}

	bool visibleSettingsButton = true, visibleExitButton = true;

	void HideButtons()
	{
		AddHideAnimation (settingsButton, originalSettingsBtnPos);
		AddHideAnimation (exitButton, originalExitBtnPos);

		settingsButton.GetComponent<Animation> ().Play ("Hide");
		exitButton.GetComponent<Animation> ().Play ("Hide");
	}

	void ShowButtons()
	{
		AddShowAnimation (settingsButton, originalSettingsBtnPos);
		AddShowAnimation (exitButton, originalExitBtnPos);

		settingsButton.GetComponent<Animation> ().Play ("Show");
		exitButton.GetComponent<Animation> ().Play ("Show");
	}

	void Update () 
	{
		/*if(Time.time < startTime + Game.drawTime*1.5f)
		{
			if(Time.time <= startTime + Game.drawTime)
			{
				borderSize += 90f*Time.deltaTime/Game.drawTime;

				curveBorder[0].mesh = CustomMesh.SemicircleBorder(0.5f, level.room[0].Size.x, 25, borderSize);
				curveBorder[1].mesh = CustomMesh.SemicircleBorder(0.5f, level.room[0].Size.x, 25, borderSize);
				
				curveBorder[2].mesh = CustomMesh.CurveWall(25, borderSize);
				curveBorder[3].mesh = CustomMesh.CurveWall(25, borderSize);
			}
			return;
		}

		//Debug.LogWarning(level.door[2].CurrentState);
			

		if( level.door[0].door!=null && !level.door[0].IsOpen )
		{
			if( level.door[0].door.GetComponent<Animation>().isPlaying )
			{
				//if(level.room[1].trigger[2].PlayerStay)
				if(Player.InZeroRoom)
				{
					level.door[0].openDoorTrigger = null;
					//level.door[0].Close();
					level.room[0].trigger[0].gameObject.SetActive(false);
					level.door[0].room.side[4].GetComponent<Collider>().enabled = true;
					if(level.door[0].room.side[4].GetComponent<BoxCollider>() != null)
						level.door[0].room.side[4].GetComponent<BoxCollider>().enabled = true;
					inLoadLevelRoom = true;
					//level.room[1].trigger[0].gameObject.SetActive(false);
					level.outletDoor.openDoorTrigger = null;

				}
			}
			else if(inLoadLevelRoom)
			{


				level.door[0].room.side[4].GetComponent<Collider>().enabled = false;
				if(level.door[0].room.side[4].GetComponent<BoxCollider>() != null)
					level.door[0].room.side[4].GetComponent<BoxCollider>().enabled = false;
				Destroy(level.room[0].gameObject);
				level.outletDoor.gameObject.SetActive(true);
				level.outletDoor.transform.position = level.door[0].transform.position;
				Destroy(level.door[0].gameObject);


				foreach(Line line in level.outletDoor.room.side[4].line)
					line.transform.localEulerAngles += Vector3.up * 180;
				//level.outletDoor.room.side[4].gameObject.SetActive(false);

				foreach(Side side in level.outletDoor.room.side)
				{
					for(int i=0; i<side.line.Length; ++i)
					{
						if(!side.line[i].gameObject.activeSelf && i==3 && (side.index<2 || side.index>3))
							continue;
						side.line[i].gameObject.SetActive(true);
					}

					binary = Game.Progress;//1;
//					foreach(Line line in side.line)
//					{
//						if(line.in
//						line.gameObject.SetActive(true);
//					}
				}
			}
		}*/

		if (!hideInfo)
		{
			if (!usedKeys)
			{
				if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)
					usedKeys = true;
			}

			if (!usedMouse)
			{
				if (Input.GetAxis ("Mouse Y") != 0 || Input.GetAxis ("Axis4") != 0) //"Joy Y"
					usedMouse = true;
			}

			if (usedKeys && usedMouse && (!infoWalk.GetComponent<Animation> ().isPlaying && !infoMouse.GetComponent<Animation> ().isPlaying))
			{
				hideInfo = true;
				infoWalk.GetComponent<Animation> ().Play ("Destroy");
				infoMouse.GetComponent<Animation> ().Play ("Destroy");
			}
		}


		if(Game.debugMode)
		{
			if(Input.GetKeyUp(KeyCode.Alpha1))
			{
				binary = 1;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha2))
			{
				binary = 2;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha3))
			{
				binary = 3;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha4))
			{
				binary = 4;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha5))
			{
				binary = 5;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha6))
			{
				binary = 6;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha7))
			{
				binary = 7;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha8))
			{
				binary = 8;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha9))
			{
				binary = 9;
			}
			else if(Input.GetKeyUp(KeyCode.Alpha0))
			{
				binary = 10;
			}
//			else if(Input.GetKeyUp(KeyCode.Space))
//			{
//				infoWalk.GetComponent<Animation> ().Play ("Destroy");
//				infoMouse.GetComponent<Animation> ().Play ("Destroy");
//			}
		}

		//binary = 0;
		//ChooseLevelControl();

		if(inLoadLevelRoom)
		{
//			if(visibleSettingsButton && Vector3.Distance(Player.player.transform.position, settingsButton.transform.position) < 5f)
//			{
//				settingsButton.GetComponent<Animation>().PlayQueued("Hide");
//				visibleSettingsButton = false;
//			}
//			else if(!visibleSettingsButton && Vector3.Distance(Player.player.transform.position, settingsButton.transform.position) > 5f)
//			{
//				settingsButton.GetComponent<Animation>().PlayQueued("Show");
//				visibleSettingsButton = true;
//			}


			if( binary > 0 )
			{
				if(binary == 1)
					levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.22f;

				//Debug.Log(binary);
				if(levelIndex.transform.childCount == 0 || previousBinary != binary || (levelIndex.transform.childCount > 0 && levelIndex.transform.GetChild(0).name == "DELETED"))
				{
					level.outletDoor.openDoorTrigger = level.room[1].trigger[0];
					nextLevel = binary;

				 	if(levelIndex.transform.childCount > 0)
						Destroy(levelIndex.transform.GetChild(0).gameObject);

					if(binary == 1)
						levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.22f;
					else
						levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.125f;

					GameObject nxtlvl;

					if(previousBinary != binary && levelIndex.transform.childCount > 0)
						nxtlvl = Word.WriteString(nextLevel.ToString(), 0.5f, Obj.Colour.WHITE, true, false, previousBinary);
					else
						nxtlvl = Word.WriteString(nextLevel.ToString(), 0.5f, Obj.Colour.WHITE, true);

					nxtlvl.transform.parent = levelIndex.transform;
					nxtlvl.transform.localPosition = Vector3.zero;
					nxtlvl.transform.localEulerAngles = Vector3.zero;
					nxtlvl.transform.localScale = Vector3.one;

					previousBinary = binary;
				}
				/*else 
				{
					if(previousBinary != binary)
					{
						nextLevel = binary;
						
						if(levelIndex.transform.childCount > 0)
							Destroy(levelIndex.transform.GetChild(0).gameObject);
						
						if(binary == 1)
							levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.22f;
						else
							levelIndex.transform.localPosition = Vector3.forward * (-Door.sizeTemplate.z*3f) - Vector3.right * 0.125f;
						
						GameObject nxtlvl = Word.WriteString(nextLevel.ToString(), 0.5f, Obj.Colour.WHITE);
						
						nxtlvl.transform.parent = levelIndex.transform;
						nxtlvl.transform.localPosition = Vector3.zero;
						nxtlvl.transform.localEulerAngles = Vector3.zero;
						nxtlvl.transform.localScale = Vector3.one;

						previousBinary = binary;
					}
				}*/
			}
			else
			{
				level.outletDoor.openDoorTrigger = null;

				if(levelIndex.transform.childCount > 0)
				{

					if(levelIndex.transform.GetChild(0).name != "DELETED")
					{
						Destroy(levelIndex.transform.GetChild(0).gameObject);

						//Digit.thick = 0f;
						GameObject nxtlvl = Word.WriteString(nextLevel.ToString(), 0.5f, Obj.Colour.WHITE, true, true);
						//Renderer[] rend = nxtlvl.GetComponentsInChildren<Renderer>();
						//foreach(Renderer r in rend)
						//	r.material.color = Color.white;
						nxtlvl.name = "DELETED";
						nxtlvl.transform.parent = levelIndex.transform;
						nxtlvl.transform.localPosition = Vector3.zero;
						nxtlvl.transform.localEulerAngles = Vector3.zero;
						nxtlvl.transform.localScale = Vector3.one;

						//Destroy(nxtlvl, 1f);
					}
					else
					{
						if(levelIndex.transform.GetChild(0).childCount == 0)
							Destroy(levelIndex.transform.GetChild(0).gameObject);
					}
					/*Transform parent = levelIndex.transform.GetChild(0);

					if(parent.childCount > 0 && parent.name != "Digit")
					{
						for(int i=0; i<parent.childCount; ++i)
						{
							//Debug.LogWarning(parent.GetChild(i).name);
							//if(parent.GetChild(i).name == "Digit")
							//	break;
							GameObject dig = (GameObject)typeof(Digit).GetMethod("From" + parent.GetChild(i).name + "ToEmpty").Invoke(null, null);
							dig.transform.parent = levelIndex.transform;
							dig.transform.localPosition = Vector3.zero;
							dig.transform.localEulerAngles = Vector3.zero;
							dig.transform.localScale = Vector3.one;
							//dig.transform.parent = null;

							//Destroy(parent.GetChild(i).gameObject);
						}
						Destroy(parent.gameObject);
					}*/
					//else
						
				}
			}


			/*if(level.room[1].trigger[2].PlayerStay && Input.GetKeyDown(KeyCode.Escape))
			{
				if(!level.door[2].drawing && !level.door[2].open && level.door[2].CurrentState != Door.State.CLOSING)
					Escape();


			}*/

			if(level.door[2].trigger.PlayerStay)
			{
				Application.Quit();
			}
		}
	}






}
