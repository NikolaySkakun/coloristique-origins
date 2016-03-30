using UnityEngine;
using System.Xml;
using System.Collections;

public class Lift : Obj
{
	public GameObject platform, lift;
	public Cell cell;
	public GameObject[] line;
	public Trigger trigger;

	public float height = 0f;
	float speed = 4f;

	static float platformWidth = 3.5f;
	bool moveDown = false, moveUp = false;
	public bool isImmobile = true;
	public bool inTop = false;


	override public void Draw()
	{
		Animation anim;// = line[i].AddComponent<Animation>() as Animation;
		AnimationClip clip;// = new AnimationClip();
		AnimationCurve[] scale = new AnimationCurve[3];
		AnimationCurve[] position = new AnimationCurve[3];
		for(int i=0; i<line.Length; ++i)
		{
			anim = line[i].AddComponent<Animation>() as Animation;
			line[i].GetComponent<Collider>().enabled = false;
			clip = new AnimationClip();
			clip.legacy = true;
			for(int u=0; u<scale.Length; ++u)
			{
				scale[u] = new AnimationCurve(
					new Keyframe(Cell.AnimationTime(i)*Game.drawTime/4f, u!=1 ? line[i].transform.localScale[u] : 0), 
					new Keyframe((Cell.AnimationTime(i)+1)*Game.drawTime/4f, line[i].transform.localScale[u]));
				
				position[u] = new AnimationCurve(
					new Keyframe(Cell.AnimationTime(i)*Game.drawTime/4f, (i<2 && u==1 || i>1 && u==0) ? ((i>0 && i<3) ? -1 : 1)*0.5f : line[i].transform.localPosition[u]), 
					new Keyframe((Cell.AnimationTime(i)+1)*Game.drawTime/4f, line[i].transform.localPosition[u]));
			}
			
			clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
			clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
			clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);
			
			clip.SetCurve("", typeof(Transform), "localPosition.x", position[0]);
			clip.SetCurve("", typeof(Transform), "localPosition.y", position[1]);
			clip.SetCurve("", typeof(Transform), "localPosition.z", position[2]);



			anim.AddClip(clip, "Draw");
			anim.Play("Draw");
			
			
		}

		for(int i=0; i<platform.transform.childCount; ++i)
		{
			if(platform.transform.GetChild(i).gameObject.GetComponent<Animation>() == null)
				continue;

			try
			{
			int u = int.Parse( platform.transform.GetChild(i).gameObject.name );
			
			Vector3 zeroScale = platform.transform.GetChild(i).localScale;
			zeroScale[u/4] = 0;
			
			Vector3 oneScale = platform.transform.GetChild(i).localScale;
			//oneScale[u/4] = 1f;
			
			clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, zeroScale, oneScale, Game.drawTime);
			platform.transform.GetChild(i).GetComponent<Animation>().AddClip(clip, "Draw");
			
			//if(anim.GetClip("Draw") != null)
			platform.transform.GetChild(i).GetComponent<Animation>().Play("Draw");
			}
			catch
			{

			}
		}
		//platform.GetComponent<Animation>().Play("Draw");

		Game.DrawEvent -= Draw;
	}


	static public Lift Create(XmlNode xml)
	{
		Lift lift = Obj.Create<Lift>();
		lift.lift = new GameObject("lift");

		float yScale = 0.006f;

		GameObject platform = CustomObject.AnimationCube(new Vector3(platformWidth, platformWidth, yScale), Colour.WHITE); //CustomObject.CreatePrimitive(PrimitiveType.Quad);



		GameObject bottom = CustomObject.CreatePrimitive(PrimitiveType.Quad);
		bottom.GetComponent<Collider>().enabled = false;
		bottom.transform.localEulerAngles += Vector3.right*180;
		bottom.transform.parent = platform.transform;

		platform.name = "Platform";

		//platform.GetComponent<Renderer>().material.color = Color.white;

		lift.line = new GameObject[4];
		float lineThick = 0.005f / 3.5f;
		for(int i=0; i<lift.line.Length; ++i)
		{
			GameObject l = CustomObject.CreatePrimitive(PrimitiveType.Quad);

			lift.line[i] = CustomObject.CreatePrimitive(PrimitiveType.Quad);
			lift.line[i].transform.localScale = new Vector3(lineThick, 1 + lineThick*2, 1);
			lift.line[i].GetComponent<Renderer>().material.color = Color.black;
			
			Vector3 position = Vector3.zero;
			position[i/2] = (0.5f + lineThick/2f) * (i%2==0 ? 1 : -1);
			
			lift.line[i].transform.localPosition = position;
			
			if(i > 1)
				lift.line[i].transform.localEulerAngles = Vector3.forward * 90;
			
			lift.line[i].transform.parent = platform.transform;

			l.transform.localScale = lift.line[i].transform.localScale;
			l.GetComponent<Renderer>().material.color = Color.black;
			l.transform.localPosition = lift.line[i].transform.localPosition;
			l.transform.localEulerAngles = lift.line[i].transform.localEulerAngles;
			l.transform.parent = platform.transform;
			l.transform.localEulerAngles += Vector3.up*180f;
		}

		//Cell.CreateLines(platform.transform);
		
		platform.transform.localEulerAngles = Vector3.right * 90;
		platform.transform.localPosition = Vector3.up / 1000f;



		platform.transform.parent = lift.lift.transform;
		//platform.transform.localScale = Vector3.one * platformWidth;// - Vector3.up*(platformWidth-0.001f);

		lift.platform = platform;
		lift.cell = Cell.CreateCell(lift.color = Game.GetColor(xml.Attributes["color"].Value));


		lift.cell.transform.parent = lift.lift.transform;
		lift.lift.transform.parent = lift.transform;
		lift.cell.transform.localPosition = new Vector3(1.2f, yScale/2f + 0.001f, 1.2f);
		//lift.cell.transform.localPosition = new Vector3(0f, .001f, 0f);
		lift.cell.isAnimate = false;


		float x = Game.GetFloat(xml, "x");
		float z = Game.GetFloat(xml, "z");
		
		Room room = Level.current.room[Game.GetInt(xml, "roomIndex")];

		
		Vector3 pos = Vector3.zero;
		
		pos.x = platformWidth/2f + 0.005f - room.Size.x/2f + x*(room.Size.x - platformWidth - 0.01f)/100f;
		pos.z = platformWidth/2f + 0.005f - room.Size.z/2f + z*(room.Size.z - platformWidth - 0.01f)/100f;


		
		lift.transform.position = room.side[2].transform.position - pos;

		lift.transform.localPosition += Vector3.up * 0.001f;
		//lift.transform.parent = room.transform;

		lift.height = Game.GetFloat(xml, "height");

		if(lift.color == Colour.WHITE)
		{
			lift.lift.transform.localPosition += Vector3.up * lift.height;
		}

		lift.lift.AddComponent<Animation>();


		lift.trigger = Trigger.NonXmlCreate(new Vector3(platformWidth, 0.6f, platformWidth), Trigger.TriggerType.LIFT);
		lift.trigger.transform.parent = lift.lift.transform;
		lift.trigger.transform.localPosition = Vector3.up*0.3f;

		return lift;
	}

	void MoveDown()
	{
		inTop = false;
		moveUp = false;
		moveDown = true;
		isImmobile = false;
		float time = lift.transform.localPosition.y/speed;

		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.x), new Keyframe(time, 0));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.y), new Keyframe(time, 0));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.z), new Keyframe(time, 0));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);



		lift.GetComponent<Animation>().AddClip(clip, "down");
		lift.GetComponent<Animation>().Play("down");
	}

	void MoveUp()
	{
		moveDown = false;
		moveUp = true;
		isImmobile = false;
		float time = (height - lift.transform.localPosition.y)/speed;

		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.x), new Keyframe(time, 0));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.y), new Keyframe(time, height));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, lift.transform.localPosition.z), new Keyframe(time, 0));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);



		lift.GetComponent<Animation>().AddClip(clip, "up");
		lift.GetComponent<Animation>().Play("up");

		Invoke("InTop", time);
	}

	void InTop()
	{
		inTop = true;
	}

	
	// Update is called once per frame
	void Update () 
	{
		if(cell.IsActive)
		{
			if(color == Colour.WHITE)
			{
				if(moveUp || !moveDown)
				{
					MoveDown();
				}
			}
			else
			{
				if(!moveUp || moveDown)
				{
					MoveUp();
				}
			}
		}
		else
		{
			if(color == Colour.WHITE)
			{
				if(!moveUp || moveDown)
				{
					MoveUp();
				}
			}
			else
			{
				if(moveUp || !moveDown)
				{
					MoveDown();
				}
			}
		}

		if(!lift.GetComponent<Animation>().isPlaying)
		{
			moveUp = moveDown = false;
			isImmobile = true;
		}



		/*if(trigger.PlayerStay)
			Player.player.transform.parent = lift.transform;
		else if(Player.player.transform.parent == lift.transform)
			Player.player.transform.parent = null;
		*/




	}
}
