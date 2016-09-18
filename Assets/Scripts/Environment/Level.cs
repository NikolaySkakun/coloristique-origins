using UnityEngine;
using System.Xml;
using System.Linq;
using System.Collections;
using System;

public class Level : Obj 
{
	int index = 0;

	static public Level current, previous;

	public bool drawing = true;
	
	public bool IsDrawing
	{
		get
		{
			return drawing;
		}
	}
	
	public int Index
	{
		get
		{
			return index;	
		}
		set
		{
			if(index == 0 && value > 0)
				index = value;
		}
	}

	static public Room ZeroRoom
	{
		get
		{
			return zeroRoom;
		}
		set
		{
			if(value.levelIndex == 0)
				zeroRoom = value;
		}
	}

	static Room zeroRoom;

	public Room[] room;
	public Ball[] ball;
	public Door[] door;
	public Lift[] lift;
	public Gun[] gun;
	public Tree[] tree;

	public Door inletDoor = null, outletDoor = null;

	public static Door lastOutletDoor = null;
	
	//public Trigger finish;

	static public Level Create(XmlNode xml)
	{
		Level level = Create<Level>();
		if(Level.current == null)
		{
			Level.current = level;
		}
		else
		{
			Level.previous = Level.current;
			Level.current = level;
		}
		level.levelIndex = level.Index = int.Parse(xml.Attributes["index"].Value);

		if(level.Index == 0)
			Game.DestroyEvent -= level.Destroy;
		//Debug.LogError(level.levelIndex);

		return Obj.Create<Level>(level, xml);
	}
	
	override public void Draw()
	{
		Game.DrawEvent -= Draw;
		drawing = true;
		beginDrawing = Time.time;
		if(Index > 0)
			StartCoroutine (OnPostDrawing());

		//if(Index > 0)
		//	Player.ActiveControl (true);
	}

	bool postDrawing = false;

	IEnumerator OnPostDrawing()
	{
		postDrawing = true;
		PostDrawing ();
		Player.speed = 0f;

		MouseLook pm = Player.player.GetComponent<MouseLook>(), cm = Player.camera.GetComponent<MouseLook>();

		float step = Game.drawTime / 10f;
		float t = 1f / Game.drawTime;

		pm.sensitivityX = cm.sensitivityY = 0;

		for (float i = 0; i <= Game.drawTime; i += i < Game.drawTime/4f ? step/2f : step)
		{
			yield return new WaitForSeconds (step);

			Player.speed = i * t;

			pm.sensitivityX = cm.sensitivityY = Player.speed * MouseLook.sensitivity;
		}

		Player.speed = 1f;
		pm.sensitivityX = cm.sensitivityY = MouseLook.sensitivity;

		postDrawing = false;
//		yield return new WaitForSeconds (Game.drawTime);
//		PostDrawing ();
	}

	override public void PostDrawing()
	{
		if(Index > 0)
			Player.ActiveControl (true);
	}

	public override void Destroy ()
	{

		
		/*if(this != null)
		{
			if(gameObject.GetComponent("Level_" + index.ToString()) != null)
				Destroy(gameObject.GetComponent("Level_" + index.ToString()));

			Game.DestroyEvent -= Destroy;
		}*/

	}

	/*override public void Join(XmlNode xml)
	{

	}*/

	float beginDrawing = 0;

	void Start () 
	{

	}
	
	int GetNextLevel()
	{
		if(index == 0)
		{
			//Debug.LogWarning("_1");
			return (gameObject.GetComponent("Level_0") as Level_0).nextLevel;
		}
		else
		{
			if(Player.inZero)
			{
				//Debug.LogWarning("_2");
				Player.inZero = false;
				return Level.ZeroRoom.transform.parent.GetComponent<Level_0>().nextLevel;
			}

			//Debug.LogWarning("_3");
//			if(index == 2)
//				return 4;
//			else
				return index + 1;	
		}	

		//return index + 1;
	}

	static public bool isNextLevelLoaded = false;
	public bool isNextLevelLoadedLocal = false;

	public void PostLoad()
	{
		foreach(Room r in room)
			foreach(Side s in r.side)
				s.AddDrawAnimation();

		if(door != null)
		foreach(Door d in door)
			foreach(Side s in d.room.side)
				s.AddDrawAnimation();
	}

	bool nextLevelBubbles = false;

	void FixedUpdate()
	{

		if((nextLevelBubbles || (drawing && index != 0)) && this == Level.current)
		{
			Player.player.transform.localEulerAngles = Vector3.Lerp(Player.player.transform.localEulerAngles, Player.player.transform.localEulerAngles.y < 180 ? Vector3.zero : Vector3.up*360f, Time.fixedDeltaTime*Game.drawTime*(1.5f));
			Player.camera.transform.localEulerAngles = Vector3.Lerp(Player.camera.transform.localEulerAngles, Player.camera.transform.localEulerAngles.x < 180 ? Vector3.zero : Vector3.right*360f, Time.fixedDeltaTime*Game.drawTime*(1.5f));

			//Player.camera.transform.localEulerAngles = Vector3.Lerp(Player.camera.transform.localEulerAngles, Vector3.zero, Time.fixedTime*0.1f);
		}
	}

	void Update()
	{
		if (drawing && Time.time - beginDrawing >= Game.drawTime) {
			drawing = false;
			nextLevelBubbles = false;
		}



		//if(index == 0 && !Player.inZero)
		//	return;

		if(outletDoor != null)
		{
			if(outletDoor.trigger.PlayerStay && !isNextLevelLoadedLocal)
			{
				Animation anim = Player.player.GetComponent<Animation>();

				if(!anim)
					anim = Player.player.AddComponent<Animation>();

				if(!anim.IsPlaying("NextLevel"))
				{

					Level.lastOutletDoor = Level.current.outletDoor;

				//Player.DisableControl(Door.timeForUpDoor);


					AnimationClip clip = Game.CreateAnimationClip(
						Game.AnimationClipType.POSITION, 
						Player.player.transform.position, 
						new Vector3(outletDoor.destroyPreviousLevelTrigger.transform.position.x, Player.player.transform.position.y, outletDoor.destroyPreviousLevelTrigger.transform.position.z), 
						0.2f);

//					clip = Game.CreateAnimationClip(
//						Player.player.transform.rotation, 
//						Quaternion.Euler(Vector3.zero),
//						0.2f,
//						0,
//						clip);


//					AnimationClip clip = Game.CreateAnimationClip(
//						Game.AnimationClipType.ROTATION, 
//						Player.player.transform.eulerAngles,
//						Vector3.zero, 
//						0.2f);

//					Player.player.GetComponent<MouseLook>().enabled = false;
//
//					Player.camera.GetComponent<MouseLook>().enabled = false;

					if(anim.GetClip("NextLevel"))
						anim.RemoveClip("NextLevel");

					anim.AddClip(clip, "NextLevel");
					anim.Play("NextLevel");

					Player.ActiveControl(false, true);
					nextLevelBubbles = true;
					//StartCoroutine( Player.DisableControl(Door.timeForUpDoor + Game.drawTime) );

				//Player.SetPosition(outletDoor.destroyPreviousLevelTrigger.gameObject);

				//Game.ShowMessage("trigger");
				}
			}

			else if(!isNextLevelLoadedLocal && Level.lastOutletDoor!=null && !Level.lastOutletDoor.trigger.PlayerStay && Level.lastOutletDoor.destroyPreviousLevelTrigger.PlayerStay)
			{
				isNextLevelLoadedLocal = true;

				//Level.lastOutletDoor.door.animation.Play("anim");
				StartCoroutine(DestroyPreviousLevel());
			}
		}



	}

	public IEnumerator Restart()
	{
		Vector3 pos = Level.current.outletDoor.transform.position;

			Level.current.outletDoor.cell = null;
			Level.current.outletDoor.Close();
			Level.current.outletDoor.PlayLoadingLevelAnimation();

		//Game.OnDestroyEvent();
		//if( Level.current.gameObject.GetComponent(Type.GetType("Level_" + Level.current.Index.ToString())) != null )
		//	Destroy(Level.current.gameObject.GetComponent(Type.GetType("Level_" + Level.current.Index.ToString())));

		yield return new WaitForSeconds(Door.timeForUpDoor);//
		//Debug.LogError("  ");
		//yield return new WaitForSeconds(Door.timeForUpDoor);
		/*if(index != 0)
		{
			Level.lastOutletDoor.transform.position = Vector3.one*100f;
		}*/
		if(Player.gunCamera.activeSelf)
			Player.gunCamera.SetActive(false);
		
		Game.OnDestroyEvent();

			UnityEngine.Object.Destroy (Level.current.outletDoor.destroyPreviousLevelTrigger.transform.GetChild(0).gameObject);
		
		
		Game.LoadLevel(index);


		//Game.Progress = GetNextLevel();
		if(Level.previous.gun != null)
		{
			foreach(Gun gun in Level.previous.gun)
				gun.Destroy();
		}
		
		foreach(Ball ball in Level.previous.ball)
			if(ball.InHands)
		{
			//Debug.LogWarning("Ball in hands");
			ball.Destroy();
			break;
		}

			foreach(Side side in Level.current.outletDoor.room.side)
				if(side.GetComponent<Collider>() != null)
					side.GetComponent<Collider>().enabled = true;


		Level.current.transform.position = pos;//Level.previous.outletDoor.transform.position;

		//yield return new WaitForSeconds(Game.drawTime);

		//Level.current.inletDoor.Destroy();
		//Level.previous.outletDoor.Destroy();


	}

	public IEnumerator DestroyPreviousLevel() 
	{
		Game.ShowMessage("Destroy level " + current.index.ToString());

		if(Level.lastOutletDoor != null)
		{
		Level.lastOutletDoor.cell = null;
		Level.lastOutletDoor.Close();
		

		Level.lastOutletDoor.PlayLoadingLevelAnimation();
		}
		else
		{
			Level.current.outletDoor.cell = null;
			Level.current.outletDoor.Close();
			Level.current.outletDoor.PlayLoadingLevelAnimation();
		}

		if(Level.current.inletDoor != null)
		{
			Level.current.inletDoor.gameObject.SetActive(false);
		}
		StartCoroutine( Player.audio.PlayForTest(false) );

		yield return new WaitForSeconds(Door.timeForUpDoor);//*0.25f
		//Debug.LogError("  ");
		//yield return new WaitForSeconds(Door.timeForUpDoor);
		/*if(index != 0)
		{
			Level.lastOutletDoor.transform.position = Vector3.one*100f;
		}*/
		if(Player.gunCamera.activeSelf)
			Player.gunCamera.SetActive(false);

		Game.OnDestroyEvent();
		//Game.OnDrawEvent();

		if(Level.lastOutletDoor != null)
			UnityEngine.Object.Destroy (Level.lastOutletDoor.destroyPreviousLevelTrigger.transform.GetChild(0).gameObject);
		else
			UnityEngine.Object.Destroy (Level.current.outletDoor.destroyPreviousLevelTrigger.transform.GetChild(0).gameObject);
		//Level.lastOutletDoor.Destroy();
		//Level previousLevel = Level.current;

		int progress = 0;
		Game.LoadLevel(progress = GetNextLevel());
		Game.Progress = progress;

		//if(Level.lastOutletDoor != null)
		Level.lastOutletDoor.Destroy();

		if(Level.previous.gun != null)
		{
			foreach(Gun gun in Level.previous.gun)
				gun.Destroy();
		}

		if(Level.previous.ball != null)
		foreach(Ball ball in Level.previous.ball)
			if(ball.InHands)
		{
			//Debug.LogWarning("Ball in hands");
			ball.Destroy();
			break;
		}

		if(Level.lastOutletDoor != null)
		{
			foreach(Side side in Level.lastOutletDoor.room.side)
				if(side.GetComponent<Collider>() != null)
					side.GetComponent<Collider>().enabled = true;

			Level.lastOutletDoor = null;
		}
		else
		{
			foreach(Side side in Level.current.outletDoor.room.side)
				if(side.GetComponent<Collider>() != null)
					side.GetComponent<Collider>().enabled = true;
		}

		isNextLevelLoaded = false;

		if(previous.Index != 0)
			previous.gameObject.SetActive(false);
		//isNextLevelLoadedLocal = true;
		//StartCoroutine(Player.DisableControl(Game.drawTime));
		//Game.OnDrawEvent();

		//Player.gunCamera.SetActive(false);
		//Debug.Log(Level.current.index);
		/*if(outletDoor != null)
		{
			if(outletDoor.trigger.PlayerStay)
			{
				Level.lastOutletDoor = Level.current.outletDoor;
				Level.lastOutletDoor.cell = null;
				Level.lastOutletDoor.Close();
				Game.LoadLevel(GetNextLevel());
				//Debug.Log(Level.lastOutletDoor.door.animation.isPlaying);
				yield return new WaitForSeconds(Door.timeForUpDoor);//

				Game.OnDestroyEvent();
				//Debug.Log(Level.lastOutletDoor.door.animation.isPlaying);
				//Level.lastOutletDoor.Close();
			}
		}*/

		//if(finish.IsInside("Player"))
			//Game.LoadLevel(GetNextLevel());
	}
}
