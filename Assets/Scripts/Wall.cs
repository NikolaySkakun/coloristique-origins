using UnityEngine;
using System.Collections;
using System.Xml;

public class Wall : Obj 
{
	static public readonly float thick = 0.3f;
	public GameObject wall;
	public Room room;
	Cell[] cell;
	bool open = false;
	float timeForUpWall = 1f;
	public int direction = 0;

	public bool IsClosed
	{
		get
		{
			if(wall.transform.localPosition.y < 0.15f)
				return true;
			else
				return false;
		}
	}

	/*IEnumerator Start()
	{
		Repaint();
		yield return new WaitForSeconds(Game.drawTime*1.495f);
		//StartCoroutine(Wait(Game.drawTime*1.495f));
		Repaint();
	}*/

	override public void Draw()
	{
		Game.DrawEvent -= Draw;

		foreach(Animation anim in gameObject.GetComponentsInChildren<Animation>())
		{
			if(anim.GetClip("Draw") != null)
				anim.Play("Draw");
		}

		for(int i=0; i<wall.transform.childCount; ++i)
		{
			int u = int.Parse( wall.transform.GetChild(i).gameObject.name );
			
			Vector3 zeroScale = wall.transform.GetChild(i).localScale;
			zeroScale[u/4] = 0;
			
			Vector3 oneScale = wall.transform.GetChild(i).localScale;
			oneScale[u/4] = 1f;
			
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, zeroScale, oneScale, Game.drawTime);
			wall.transform.GetChild(i).GetComponent<Animation>().AddClip(clip, "Draw");
			
			//if(anim.GetClip("Draw") != null)
			wall.transform.GetChild(i).GetComponent<Animation>().Play("Draw");
		}



		/*foreach(Animation anim in wall.transform.GetComponentsInChildren<Animation>())
		{
			int i = int.Parse( anim.gameObject.name );

			Vector3 zeroScale = anim.transform.localScale;
			zeroScale[i/4] = 0;
			
			Vector3 oneScale = anim.transform.localScale;
			oneScale[i/4] = 1f;
			
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, zeroScale, oneScale, Game.drawTime);
			anim.AddClip(clip, "Draw");

			//if(anim.GetClip("Draw") != null)
				anim.Play("Draw");
		}*/

		/*Vector2 size = new Vector2(wall.transform.localScale[direction], wall.transform.localScale.y);

		GameObject[] c = new GameObject[]{ CustomObject.CreateBubbles(13, 16, size, color), CustomObject.CreateBubbles(13, 16, size, color) };
		
		c[0].transform.localEulerAngles += Vector3.right * 90f;
		c[1].transform.localEulerAngles += Vector3.right * 90f;
		
		c[0].transform.position = wall.transform.position;
		c[1].transform.position = wall.transform.position;
		
		Vector3 pos = Vector3.zero;
		pos[direction==0 ? 2 : 0] += thick/1.995f;
		
		c[0].transform.localPosition += pos;
		c[1].transform.localPosition -= pos;
		
		
		
		c[1].transform.localEulerAngles += Vector3.up * 180f;

		if(direction==2)
		{
			c[0].transform.localEulerAngles += Vector3.up * 90f;
			c[1].transform.localEulerAngles += Vector3.up * 90f;
		}
		
		c[0].transform.parent = c[1].transform.parent = wall.transform;
		
		UnityEngine.Object.Destroy(c[0], Game.drawTime * 1.5f);
		UnityEngine.Object.Destroy(c[1], Game.drawTime * 1.5f);*/
	}

	public static void Join(XmlNode xml, Level level)
	{
		Room room = level.room[Game.ParseInt(xml, "roomIndex")];
		//room.side[3].GetComponent<Collider>().enabled = false;
		Wall wall = room.wall[Game.ParseInt(xml, "wallIndex")];
		

		if(xml.ChildNodes.Count > 0)
		{
			wall.cell = new Cell[xml.ChildNodes.Count];
			for(int i=0; i<wall.cell.Length; ++i)
				wall.cell[i] = 
					level.room[int.Parse( xml.ChildNodes[i].Attributes["room"].Value )].cell[int.Parse( xml.ChildNodes[i].Attributes["index"].Value )];
		}
	}


	void Open()
	{
		wall.GetComponent<Rigidbody>().isKinematic = true;
		open = true;
		wall.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.Discrete;
		float time = timeForUpWall * ((room.Size.y*1.5f - wall.transform.localPosition.y)/room.Size.y);
		
		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, wall.transform.localPosition.x), new Keyframe(time, wall.transform.localPosition.x));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, wall.transform.localPosition.y), new Keyframe(time, room.Size.y*0.998f));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, wall.transform.localPosition.z), new Keyframe(time, wall.transform.localPosition.z));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);



		wall.GetComponent<Animation>().AddClip(clip, "anim");
		wall.GetComponent<Animation>().Play("anim");
	}

	void FixedUpdate()
	{

//		wall.GetComponent<Rigidbody>().isKinematic = !wall.GetComponent<Rigidbody>().isKinematic;
//		wall.GetComponent<Rigidbody>().isKinematic = !wall.GetComponent<Rigidbody>().isKinematic;

		//wall.GetComponent<Rigidbody> ();
	}
	
	void Update () 
	{
		//Debug.LogWarning(open);
		if(cell != null)
		{
			int activeCellsCount = 0;
			foreach(Cell c in cell)
			{
				if(c.IsActive)
					++activeCellsCount;
			}

			if(wall.transform.localPosition.y < 0)
				wall.transform.localPosition = Vector3.zero;

			if(activeCellsCount == cell.Length)
			{
				if(!open)
				{
					//wall.GetComponent<Animation>().enabled = true;
					Open();
				}
			}
			else if(open)
			{
				if(wall.GetComponent<Animation>().isPlaying)
					wall.GetComponent<Animation>().Stop();

				//wall.GetComponent<Animation>().enabled = false;
				open = false;
				wall.GetComponent<Rigidbody>().isKinematic = false;
				wall.GetComponent<Rigidbody>().WakeUp();
				wall.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
				//Close();
			}

			if(!open)
			{
				if(wall.transform.localPosition.y > room.Size.y/2f)
				{
					//Debug.Log("____________");
					wall.GetComponent<Rigidbody>().isKinematic = false;
					wall.GetComponent<Rigidbody>().WakeUp();
					//rigidbody.isKinematic = !rigidbody.isKinematic;
				}
				else if(wall.transform.localPosition.y <= 0)
				{
					//Debug.Log("____________");
					wall.GetComponent<Rigidbody>().isKinematic = true;
					//wall.GetComponent<Rigidbody>().WakeUp();
					//rigidbody.isKinematic = !rigidbody.isKinematic;
				}

			}
		}
	}

	static public Wall Create(XmlNode xml)
	{	
		GameObject GO = CustomObject.CreatePrimitive(PrimitiveType.Cube);
		GO.name = "Wall";
		GO.layer = LayerMask.NameToLayer("Wall");
		Wall wall = Obj.Create<Wall>() as Wall;
		wall.wall =	GO;
		wall.wall.transform.parent = wall.transform;
		wall.wall.gameObject.AddComponent<Animation>();
		wall.wall.gameObject.AddComponent<Rigidbody>();
		wall.wall.GetComponent<Rigidbody>().isKinematic = true;
		wall.wall.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
		wall.wall.GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		wall.wall.GetComponent<Rigidbody>().drag = 2f;

		Room room = wall.room = objStack[objStack.Count - 1] as Room;
		room.side[3].gameObject.layer = LayerMask.NameToLayer("Ceiling");

		int dir = wall.direction = Game.GetInt(xml, "direction");
		int dir_ = dir==0 ? 2 : 0;

		Vector3 scale = new Vector3();
		scale.y = room.Size.y;// - 0.001f;
		scale[dir] = room.Size[dir] - 0.001f;
		scale[dir_] = thick;

		wall.wall.transform.localScale = scale;

		wall.color = Game.GetColor(xml.Attributes["color"].Value);

		GameObject cube = CustomObject.AnimationCube(scale, wall.color);
		while(cube.transform.childCount > 1)
		{
			Destroy(cube.transform.GetChild(0).GetComponent<Collider>());
			Transform edge = cube.transform.GetChild(0);

			if(edge.name == "0" || edge.name == "1")
			{
				edge.parent = wall.transform;

				int u = int.Parse( edge.gameObject.name );
				
				Vector3 zeroScale = edge.localScale;
				zeroScale[u/4] = 0;

				
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, zeroScale, edge.localScale, Game.drawTime);
				edge.GetComponent<Animation>().AddClip(clip, "Draw");
			}
			else
				edge.parent = wall.wall.transform;

		}

		Destroy(cube);
		//wall.wall.GetComponent<BoxCollider>().size = Vector3.one + Vector3.up * 0.004f;

		float pos = Game.GetFloat(xml, "position");


		Game.Paint(wall.wall, xml);
		Vector3 position = Vector3.zero;
		
		position[dir_] = thick/2f - room.Size[dir_]/2f + pos*(room.Size[dir_] - thick)/100f;
		position[dir] = room.side[2].transform.position[dir];
		position.y = room.side[2].transform.position.y + room.Size.y/2f;


		if (Game.GetInt (xml, "hill") != 0) {

			/*GameObject hill = CustomObject.Hill(scale[dir], thick/4f);
		hill.transform.parent = wall.transform;
		hill.transform.localPosition = new Vector3(0, -scale.y/2f, 0);
		hill.transform.localScale = new Vector3(2f, 1, 1);*/
			Vector3 hillScale = scale; 
			hillScale.y = hillScale [dir_] = thick / 1.8f;
			hillScale [dir] -= Line.height;

			GameObject hill = CustomObject.Cube (hillScale);
			hill.transform.parent = wall.transform;
			hill.transform.localPosition = new Vector3 (0, -scale.y / 2f - Line.height, 0);
			Vector3 rot = Vector3.zero;
			rot [dir] = 45f;
			hill.transform.localEulerAngles = rot;
			//hill.GetComponentInChildren<BoxCollider>().gameObject.layer = LayerMask.NameToLayer("Hill");

			foreach (BoxCollider b in hill.GetComponentsInChildren<BoxCollider>()) {
				b.gameObject.layer = LayerMask.NameToLayer ("Hill");
				if (b.gameObject.name == "QB") {
					Vector3 euler = Vector3.zero;
					euler [dir] = 32f;

					GameObject box = GameObject.CreatePrimitive (PrimitiveType.Cube);
					box.GetComponent<Renderer> ().enabled = false;
					box.layer = LayerMask.NameToLayer ("Hill");
				
					box.transform.parent = b.transform.parent;
					box.transform.localScale = b.transform.localScale;
					box.transform.localPosition = b.transform.localPosition;
					box.transform.localEulerAngles = euler;//Vector3.right * 32f;
				
					box.GetComponent<BoxCollider> ().size = new Vector3 (1, 3, 1);
					box.GetComponent<BoxCollider> ().center = new Vector3 (0, -1.3f, -0.28f);


					box = GameObject.CreatePrimitive (PrimitiveType.Cube);
					box.GetComponent<Renderer> ().enabled = false;
					box.layer = LayerMask.NameToLayer ("Hill");
				
					box.transform.parent = b.transform.parent;
					box.transform.localScale = b.transform.localScale;
					box.transform.localPosition = b.transform.localPosition;
					box.transform.localEulerAngles = -euler;//Vector3.right * (-32f);
				
					box.GetComponent<BoxCollider> ().size = new Vector3 (1, 1, 3);
					box.GetComponent<BoxCollider> ().center = new Vector3 (0, 0.28f, 1.3f);
					//box.transform.localPosition = new Vector3(0, -0.16f, -0.16f);
				}
			}
		}



		//hill.transform.localScale = new Vector3(2f, 1, 1);

		wall.transform.position = position;
		//cell.transform.position = room.side[2].transform.position - position;
		Game.Paint(wall.wall, xml);

		return wall;
	}

	override public void Repaint()
	{
		base.Repaint();
		
		wall.GetComponent<Renderer>().material.color = Game.GetColor(color);
		
		//room.Repaint();
	}

}
