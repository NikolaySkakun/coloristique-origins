using UnityEngine;
using System.Collections;
using System.Xml;

public class Door : Obj 
{
	public enum State {OPEN, OPENING, CLOSE, CLOSING, DRAWING};

	public State CurrentState
	{
		get
		{
			return state;
		}
	}

	private State state = State.CLOSE;

	public Room room;

	public GameObject door;
	public Trigger trigger, destroyPreviousLevelTrigger, openDoorTrigger;

	public Cell[] cell;
	public bool open = false;
	static public float timeForUpDoor = 1f;
	public int orientation = 2;

	public bool drawing = false;

	public GameObject whiteBubble, blackBubble;
	public GameObject[] dot;
	public int whites = 0, blacks = 0;

	public bool IsOpen
	{
		get
		{
			return open;
		}
	}

	static public Vector3 sizeTemplate = new Vector3(3.5f, 2.8f, 0.2f); //0.2


	/*public IEnumerator Start()
	{
		Repaint();
		yield return new WaitForSeconds(Game.drawTime*1.495f);
		Repaint();
	}*/

	IEnumerator RepaintDoor()
	{
		Repaint();
		yield return new WaitForSeconds(Game.drawTime*1.495f);
		//door.SetActive(false);
		Repaint();
		//door.SetActive(true);
		drawing = false;
	}

	void Start()
	{
		/*foreach(Side s in room.side)
		{
			if(!s.gameObject.activeSelf)
			{
				s.gameObject.SetActive(true);
				s.line[3].gameObject.SetActive(false);
			}
			
			
		}*/
	}

	IEnumerator ChangeAnimationSpeedOfDots()
	{
		yield return new WaitForSeconds (Game.drawTime);

		foreach (GameObject obj in dot) {
			foreach (AnimationState state in obj.GetComponent<Animation>()) {
				state.speed = 4f;
			}
		}

	}

	void CreateDots()
	{

		dot = new GameObject[cell.Length];
		whites = blacks = 0;
		for (int i=0, b = 0, w = 0; i<cell.Length; ++i) 
		{
			dot[i] = CustomObject.CreateSingleBubble(0.05f, Game.ReverseColor(cell[i].color), 0, Game.drawTime, 0.1f);
			Game.SetRenderQueue (dot [i], -1);
				//CustomObject.Circle(0.05f, Game.ReverseColor(cell[i].color));

			if(cell[i].color == Colour.BLACK)
				++whites;
			else
				++blacks;

			dot[i].transform.parent = room.transform;
			dot[i].transform.localEulerAngles = Vector3.right * 90f;
			dot[i].transform.position = 
				room.side[4].transform.position 
					- Vector3.forward*0.001f 
					- Vector3.right*sizeTemplate.x*0.5f 
					+ Vector3.up*(sizeTemplate.y*0.5f - 0.1f)
					+ Vector3.right*(0.1f * (cell[i].color == Obj.Colour.BLACK ?  1f : -1f ));

			dot[i].GetComponent<Animation>().Play("Draw");
			StartCoroutine( ChangeAnimationSpeedOfDots() );
		}
	}

	override public void Draw()
	{
		state = State.DRAWING;
		Game.DrawEvent -= Draw;

		drawing = true;

		GameObject[] c = new GameObject[]{ CustomObject.CreateBubbles(13, 16, sizeTemplate, color, -1), CustomObject.CreateBubbles(13, 16, sizeTemplate, color, -1) };

		c[0].transform.localEulerAngles += Vector3.right * 90f;
		c[1].transform.localEulerAngles += Vector3.right * 90f;
		
		c[0].transform.position = door.transform.position;
		c[1].transform.position = door.transform.position;

		Vector3 pos = Vector3.zero;
		pos[orientation] += sizeTemplate.z*0.5f - 0.001f;

		c[0].transform.localPosition += pos;
		c[1].transform.localPosition -= pos;


		if(orientation == 0)
		{
			c[0].transform.localEulerAngles += Vector3.up * 90f;
			c[1].transform.localEulerAngles += Vector3.up * 90f;
		}

		c[1].transform.localEulerAngles += Vector3.up * 180f;

		c[0].transform.parent = c[1].transform.parent = door.transform;

		UnityEngine.Object.Destroy(c[0], Game.drawTime * 1.5f);
		UnityEngine.Object.Destroy(c[1], Game.drawTime * 1.5f);

		if(cell != null && cell.Length > 0)
			CreateDots ();

		StartCoroutine(RepaintDoor());
	}

	public static void Join(XmlNode xml, Level level)
	{
		Door door = level.door[Game.ParseInt(xml, "doorIndex")];
		Room r1, r2;

		if(xml.Attributes["type"].Value == "d")
		{
			r1 = level.room[int.Parse( xml.Attributes["roomIndex"].Value )];
			r2 = door.room;

			if(xml.ChildNodes.Count > 0)
			{
				door.cell = new Cell[xml.ChildNodes.Count];
				for(int i=0; i<door.cell.Length; ++i)
				{
					door.cell[i] = 
						level.room[Game.GetInt(xml.ChildNodes[i], "room")].cell[Game.GetInt(xml.ChildNodes[i], "index")];

					GameObject circle = CustomObject.Circle(0.05f, Game.ReverseColor(door.cell[i].color));
					circle.transform.SetParent(door.cell[i].plane.transform);
					circle.transform.localPosition = -Vector3.forward*0.003f;
					Game.SetRenderQueue (circle, 5);
					circle.transform.localEulerAngles = -Vector3.right*90f * (door.cell[i].transform.localScale.y < 0 ? -1 : 1);
					// int.Parse( xml.ChildNodes[i].Attributes["room"].Value )
					// int.Parse( xml.ChildNodes[i].Attributes["index"].Value )
				}
			}
		}
		else// if(xml.Attributes["type"].Value == "r")
		{
			r2 = level.room[int.Parse( xml.Attributes["roomIndex"].Value )];
			r1 = door.room;
		}

		door.room.transform.parent = null;
		door.transform.parent = door.room.transform;

		Room.NonXmlJoin(r1, r2, int.Parse( xml.Attributes["sideIndex"].Value ), 
		                float.Parse( xml.Attributes["x"].Value ), float.Parse( xml.Attributes["y"].Value ), level);
		//level.room[int.Parse( xml.Attributes["roomIndex"].Value )]
		//door.trigger = level.room[int.Parse(xml.Attributes["room"].Value)].cell[int.Parse(xml.Attributes["cell"].Value)].trigger;

		//door.room.side[ Game.GetInt(xml, "sideIndex") ].gameObject.SetActive(false);
		int sideIndex = Game.GetInt(xml, "sideIndex");
		door.room.side[sideIndex].line[3].gameObject.SetActive(false);
		door.room.side[sideIndex%2==0 ? sideIndex+1 : sideIndex-1].line[3].gameObject.SetActive(false);

		door.room.side[sideIndex].GetComponent<Renderer>().enabled = false;
		door.room.side[sideIndex%2==0 ? sideIndex+1 : sideIndex-1].GetComponent<Renderer>().enabled = false;

		door.transform.parent = level.transform;
		door.room.transform.parent = door.transform;
	}

	public Animation whiteCircleAnimation, blackCircleAnimation;

	public void PlayLoadingLevelAnimation()
	{
		//Player.DisableControl (Game.drawTime);
		//Player.gunCamera.SetActive(true);
		if (blackCircleAnimation != null && blackCircleAnimation.GetClip ("Draw") != null)
		{
//			blackCircleAnimation.transform.parent = null;
//			Object.Destroy(blackCircleAnimation.gameObject, blackCircleAnimation.GetClip ("Draw").length);
			blackCircleAnimation.Play ("Draw");

		}

		if (whiteCircleAnimation != null && whiteCircleAnimation.GetClip ("Draw") != null) 
		{
//			whiteCircleAnimation.transform.parent = null;
//			Object.Destroy(whiteCircleAnimation.gameObject, whiteCircleAnimation.GetClip ("Draw").length);
			whiteCircleAnimation.Play ("Draw");
		}
		//Debug.LogWarning(blackCircleAnimation);
	}
	
	static public Door Create(XmlNode xml)
	{
		if(xml.Attributes["type"] != null)
			if(xml.Attributes["type"].Value == "inlet" && Level.lastOutletDoor != null)
				return Level.current.inletDoor = Level.lastOutletDoor;


		Door door = Obj.Create<Door>();
		Vector3 size = sizeTemplate;
		Vector3 triggerPosition = new Vector3(0, size.y/2f, 0), triggerPosition_ = new Vector3(0, size.y/2f, 0);
		int tmpIndex = 2;
		bool zIsZ = true;
		if(xml.Attributes["orientation"].Value == "x")
		{
			zIsZ = false;
			float tmp = size.x;
			size.x = size.z;
			size.z = tmp;

			tmpIndex = 0;

			triggerPosition_.x = sizeTemplate.z * 4f;

			door.orientation = 0;
		}
		else
			triggerPosition_.z = sizeTemplate.z * 4f;

		door.door = CustomObject.CreatePrimitive(PrimitiveType.Cube);
		door.door.transform.localScale = size - Vector3.one/1000f - (zIsZ ? Vector3.forward : Vector3.right) * 0.005f;
		door.door.GetComponent<Renderer>().material.color = Game.ReverseColor(Game.GetColor(door.color));
		Game.SetRenderQueue (door.door, -2);
		//door.door.GetComponent<Renderer> ().material.renderQueue -= 2;
		door.door.transform.localPosition += new Vector3(0, size.y/2f, 0);
		door.door.transform.parent = door.transform;
		door.door.AddComponent<Animation>();
		//(door.door.AddComponent<Rigidbody>() as Rigidbody).isKinematic = true;

		if(xml.Attributes["type"] != null)
		{
			if(xml.Attributes["type"].Value == "outlet")
			{
				Level.current.outletDoor = door;

				door.trigger = Trigger.NonXmlCreate(size - Vector3.one/1000f);
				door.trigger.transform.localPosition += triggerPosition;
				door.trigger.transform.parent = door.transform;
				door.trigger.type = Trigger.TriggerType.PLAYER;

				Vector3 triggerSize_ = size - Vector3.one/1000f;
				triggerSize_[tmpIndex] *= 3;
				door.destroyPreviousLevelTrigger = Trigger.NonXmlCreate(triggerSize_);
				door.destroyPreviousLevelTrigger.transform.localPosition += triggerPosition_;
				door.destroyPreviousLevelTrigger.transform.parent = door.transform;
				door.destroyPreviousLevelTrigger.type = Trigger.TriggerType.PLAYER;
				door.destroyPreviousLevelTrigger.tag = "Outlet";

				Room r = Room.NonXmlCreate(triggerSize_ + Vector3.forward*sizeTemplate.z*4f, Colour.WHITE);
				r.transform.position = door.destroyPreviousLevelTrigger.transform.position;// - Vector3.up*triggerSize_.y;

				//r.transform.parent = Level.current.transform;
				r.transform.parent = door.destroyPreviousLevelTrigger.transform;
				r.transform.localPosition -= Vector3.up*0.5f;
				r.side[4].gameObject.SetActive(false);

				GameObject whiteBubble, blackBubble;

				door.whiteBubble = whiteBubble = CustomObject.CreateSingleBubble(0.5f, Colour.WHITE, Door.timeForUpDoor/3f, Door.timeForUpDoor, 1.21f);
				door.blackBubble = blackBubble = CustomObject.CreateSingleBubble(0.5f, Colour.BLACK, 0, Door.timeForUpDoor*0.85f, 1.2f, 0.3f);

				whiteBubble.transform.position = blackBubble.transform.position = r.side[5].transform.position + Vector3.forward*4f;//- Vector3.forward * 0.0001f;
				r.side[5].gameObject.GetComponent<Renderer>().enabled = false;
				r.side[5].gameObject.layer = LayerMask.NameToLayer("Outlet");
				whiteBubble.transform.position -= Vector3.forward * 0.005f;
				whiteBubble.transform.parent = blackBubble.transform.parent = r.transform;

				whiteBubble.transform.localEulerAngles = blackBubble.transform.localEulerAngles = -Vector3.right * 90f;
				//whiteBubble.layer = blackBubble.layer = LayerMask.NameToLayer("Gun");

//				whiteBubble.layer = blackBubble.layer = LayerMask.NameToLayer("Gun");

				door.whiteCircleAnimation = whiteBubble.GetComponent<Animation>();
				door.blackCircleAnimation = blackBubble.GetComponent<Animation>();

				whiteBubble.transform.localScale = blackBubble.transform.localScale = Vector3.zero;

				foreach(Side sd in r.side)
					foreach(Line l in sd.line)
						l.Repaint();




			}
		}
		else if(xml.Attributes["withTrigger"] != null)
		{
			if(xml.Attributes["withTrigger"].Value == "true")
			{
				door.trigger = Trigger.NonXmlCreate(size - Vector3.one/1000f);
				door.trigger.transform.localPosition += triggerPosition;
				door.trigger.transform.parent = door.transform;
				door.trigger.type = Trigger.TriggerType.PLAYER;
			}
		}

		//GO.name = "Door";

		//GO.AddComponent<Door>() as Door;
		door.color = Game.GetColor(xml.Attributes["color"].Value);

		door.room = Room.NonXmlCreate(size, Game.ReverseColor(door.color));

		//foreach(Side s in door.room.side)
		//	s.AddDrawAnimation();
			//foreach(Line l in s.line)
				//Game.DrawEvent += l.Draw;

		door.room.transform.parent = door.transform;

		//foreach(Line line in door.room.side[2].line)
			//line.gameObject.SetActive(false);
		
		//door.room.side[2].transform.localPosition += Vector3.up*0.0001f;
		//door.room.side[2].transform.localScale *= 1.1f;

		int ind = xml.Attributes["orientation"].Value == "x" ? 0 : 2;
		int dir = ind==0 ? 0 : 1;

		Vector3 tmpDir = Vector3.zero;
		tmpDir[dir] = Line.height/0.2f;


		Line[] line = {door.room.side[2].line[ind], door.room.side[2].line[ind+1]};

		line[0].transform.localScale += Vector3.right * Line.height;
		line[1].transform.localScale += Vector3.right * Line.height;

		line[0].transform.localScale -= Vector3.up*Line.height/2f;
		line[1].transform.localScale -= Vector3.up*Line.height/2f;



		line[0].transform.localPosition += tmpDir;
		line[1].transform.localPosition -= tmpDir;

		line[0].transform.localPosition -= Vector3.forward*0.001f;
		line[1].transform.localPosition -= Vector3.forward*0.001f;
		Game.SetRenderQueue (line [0].gameObject, 3);
		Game.SetRenderQueue (line [1].gameObject, 3);

		line[0].GetComponent<Renderer>().material.color = Color.white;
		line[1].GetComponent<Renderer>().material.color = Color.white;



		if(xml.Attributes["type"] != null)
			if(xml.Attributes["type"].Value == "outlet")
		{


			if(door.orientation == 0)
			{
				Object.Destroy(door.room.side[0].GetComponent<Collider>());

				if(door.room.side[0].GetComponent<BoxCollider>() != null)
					Object.Destroy(door.room.side[0].GetComponent<BoxCollider>());

				Object.Destroy(door.room.side[1].GetComponent<Collider>());

				if(door.room.side[1].GetComponent<BoxCollider>() != null)
					Object.Destroy(door.room.side[1].GetComponent<BoxCollider>());
			}
			else
			{
				Object.Destroy(door.room.side[5].GetComponent<Collider>());
				Object.Destroy(door.room.side[4].GetComponent<Collider>());

				if(door.room.side[5].GetComponent<BoxCollider>() != null)
					Object.Destroy(door.room.side[5].GetComponent<BoxCollider>());

				if(door.room.side[4].GetComponent<BoxCollider>() != null)
					Object.Destroy(door.room.side[4].GetComponent<BoxCollider>());
			}
			//Vector3 scale = Vector3.zero;
			//scale[xml.Attributes["orientation"].Value == "x" ? 0 : 2] = 0.1f;

			//if(xml.Attributes["orientation"].Value == "x")


				//door.room.side[2].transform.localScale += scale;


			//int i = xml.Attributes["orientation"].Value == "x" ? 0 : 2;
			//door.room.side[2].line[i].gameObject.SetActive(false);
			//door.room.side[2].line[i+1].gameObject.SetActive(false);
			/*door.room.side[4].GetComponent<MeshRenderer>().enabled = true;
			
			for(int i=0; i<door.room.side[4].transform.childCount; ++i)
				{
					door.room.side[4].transform.GetChild(i).gameObject.SetActive(false);
				}

				door.room.side[4].transform.localPosition += Vector3.up;
*/
		}

		Side s = door.room.side[2];
		Object.Destroy(s.gameObject.GetComponent<Collider>());

		if(s.gameObject.GetComponent<BoxCollider>() == null)
			s.gameObject.AddComponent<BoxCollider>().size = new Vector3(1, 20, 0.001f);
		else
			s.gameObject.GetComponent<BoxCollider>().size = new Vector3(1, 20, 0.001f);


		Game.DestroyEvent -= door.Destroy;
		Game.DestroyEvent -= door.room.Destroy;

		foreach(Side side in door.room.side)
		{
			Game.DestroyEvent -= side.Destroy;
			foreach(Line l in side.line)
				Game.DestroyEvent -= l.Destroy;
		}

		return door;
	}

	public override void Destroy ()
	{
		if(this != null)
		{
			if(levelIndex != Level.current.Index)
			{
				
				//Game.DrawEvent -= Draw;
				//Game.DestroyEvent -= Destroy;
				
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, door.transform.localPosition + Vector3.forward*Line.height, 
				                                              new Vector3(0, -sizeTemplate.y/2f + Line.height, Line.height), timeForUpDoor);

				door.GetComponent<Animation>().AddClip(clip, "Destroy");
				door.GetComponent<Animation>().Play("Destroy");


				room.side[5].GetComponent<MeshRenderer>().enabled = true;

				if(room.side[5].gameObject.AddComponent<BoxCollider>() == null)
					room.side[5].gameObject.AddComponent<BoxCollider>();
				
				for(int i=0; i<room.side[5].transform.childCount; ++i)
				{
					room.side[5].transform.GetChild(i).gameObject.SetActive(false);
				}
				
				//room.side[5].transform.localPosition += Vector3.up;
				room.side[5].transform.localScale = new Vector3(1, 1, -1);
			}
		}
	}



	void Open()
	{
		state = State.OPENING;
		open = true;
		float time = timeForUpDoor * ((room.Size.y*1.499f - door.transform.localPosition.y)/room.Size.y);

		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, door.transform.localPosition.x), new Keyframe(time, 0));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, door.transform.localPosition.y), new Keyframe(time, room.Size.y*1.499f));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, door.transform.localPosition.z), new Keyframe(time, 0));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);



		door.GetComponent<Animation>().AddClip(clip, "anim");
		door.GetComponent<Animation>().Play("anim");
	}

	public void Close()
	{
		state = State.CLOSING;
		open = false;
		float time = timeForUpDoor * ((door.transform.localPosition.y - room.Size.y/2f)/room.Size.y);
		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, door.transform.localPosition.x), new Keyframe(time, 0));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, door.transform.localPosition.y), new Keyframe(time, room.Size.y/2f));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, door.transform.localPosition.z), new Keyframe(time, 0));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);



		door.GetComponent<Animation>().AddClip(clip, "anim");
		door.GetComponent<Animation>().Play("anim");
	}

	override public void Repaint()
	{
		base.Repaint();

		door.GetComponent<Renderer>().material.color = Game.GetColor(color);

		//room.Repaint();
	}

	void OnDisable()
	{
		drawing = false;
	}

	void FixedUpdate()
	{
		if (blackBubble && !blackBubble.GetComponent<Animation>().isPlaying) {
			float dist = Vector3.Distance (blackBubble.transform.position, Player.player.transform.position);
			if (dist < 8f) {
				//Player.gunCamera.SetActive (true);
				blackBubble.transform.localScale = Vector3.one * 0.3f * ((8f - dist)/3f);
			} else {
				//Player.gunCamera.SetActive (false);
				blackBubble.transform.localScale = Vector3.zero;
			}
		}
	}

	void Update () 
	{



		if(cell != null)
		{
			int activeCellsCount = 0;
//			foreach(Cell c in cell)
//			{
//				if(c.IsActive)
//					++activeCellsCount;
//			}

			for(int i=0; i<cell.Length; ++i)
			{
				if(cell[i].IsActive)
				{
					++activeCellsCount;
					if(!dot[i].GetComponent<Animation>().IsPlaying("Hide") && dot[i].transform.localScale != Vector3.zero)
					{
						if(dot[i].GetComponent<Animation>().isPlaying)
							dot[i].GetComponent<Animation>().Stop();

						dot[i].GetComponent<Animation>().Play("Hide");
					}

				}
				else
				{
					if(!dot[i].GetComponent<Animation>().IsPlaying("Draw") && dot[i].transform.localScale == Vector3.zero)
					{
						if(dot[i].GetComponent<Animation>().isPlaying)
							dot[i].GetComponent<Animation>().Stop();
						
						dot[i].GetComponent<Animation>().Play("Draw");
					}
				}
			}

			if(activeCellsCount == cell.Length)
			{
				if(!open)
					Open();
			}
			else if(open)
				Close();
		}
		else if(openDoorTrigger != null)
		{
			if(openDoorTrigger.PlayerStay && openDoorTrigger.gameObject.activeSelf && !drawing)
			{
				if(!open)
					Open();
			}
			else if(open)
				Close();
		}


		if( (state == State.CLOSING || state == State.OPENING) && !door.GetComponent<Animation>().isPlaying )
		{
			if(open)
				state = State.OPEN;
			else
				state = State.CLOSE;
		}
		else if(!drawing && state == State.DRAWING)
			state = State.CLOSE;

	}
}
