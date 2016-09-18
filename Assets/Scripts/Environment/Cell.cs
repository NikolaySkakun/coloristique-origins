using UnityEngine;
using System.Collections;
using System.Xml;
using System;
using System.Collections.Generic;

public class Cell : Obj 
{
	public enum CellType { DOWN, SMALL, TOP }
	public CellType type = CellType.DOWN;

	static float lineThick = Line.height;//0.005f;
	static float timeForJoinBall = 0.5f;

	public Trigger trigger;

	GameObject[] line;
	public GameObject cell, plane, colorPlane, reverseColorPlane;
	//GameObject plane;
	bool active = false;
	public bool isAnimate = true;

	public bool IsActive
	{
		get
		{
			return active;
		}
	}

	static public int AnimationTime(int index)
	{
		switch(index)
		{
			case 0: return 0; break;
			case 1: return 2; break;
			case 2: return 3; break;
			case 3: return 1; break;
			default: return 0; break;
		}
	}

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
					new Keyframe(AnimationTime(i)*Game.drawTime/4f, u!=1 ? line[i].transform.localScale[u] : 0), 
					new Keyframe((AnimationTime(i)+1)*Game.drawTime/4f, line[i].transform.localScale[u]));

				position[u] = new AnimationCurve(
					new Keyframe(AnimationTime(i)*Game.drawTime/4f, (i<2 && u==1 || i>1 && u==0) ? ((i>0 && i<3) ? -1 : 1)*(plane.transform.localScale.x/2f) : line[i].transform.localPosition[u]), 
					new Keyframe((AnimationTime(i)+1)*Game.drawTime/4f, line[i].transform.localPosition[u]));
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
		Game.DrawEvent -= Draw;
		anim = plane.AddComponent<Animation>() as Animation;
		clip = new AnimationClip();
		clip.legacy = true;
		for(int u=0; u<scale.Length; ++u)
		{
			scale[u] = new AnimationCurve(
				new Keyframe(0, 0), 
				new Keyframe(Game.drawTime, plane.transform.localScale[u]));
				//new Keyframe(3*Game.drawTime/4f, u!=0 ? plane.transform.localScale[u] : 0), 
				//new Keyframe(Game.drawTime, plane.transform.localScale[u]));
			
			position[u] = new AnimationCurve(
				new Keyframe(3*Game.drawTime/4f, u!=0 ? plane.transform.localPosition[u] : -0.5f), 
				new Keyframe(Game.drawTime, plane.transform.localPosition[u]));
		}

		clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
		clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
		clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);
		
		//clip.SetCurve("", typeof(Transform), "localPosition.x", position[0]);
		//clip.SetCurve("", typeof(Transform), "localPosition.y", position[1]);
		//clip.SetCurve("", typeof(Transform), "localPosition.z", position[2]);



		anim.AddClip(clip, "Draw");
		anim.Play("Draw");

		Destroy (colorPlane, Game.drawTime);
		Destroy (reverseColorPlane, Game.drawTime);
	}

	void Update()
	{
		if(trigger.innerObjs.Count > 0)
		{
			foreach(GameObject obj in trigger.innerObjs)
			{
				if(obj != null)
				{
					Ball ball = obj.GetComponent<Ball>() as Ball;
					if(ball != null)
					{
						if(!active)
						{
							if(ball != null && !ball.InHands && ball.color == color && !ball.GetComponent<Animation>().isPlaying)
							{
								if((ball.type == Ball.BallType.SMALL && type != CellType.SMALL) ||
								   (ball.type != Ball.BallType.SMALL && type == CellType.SMALL))
									continue;
								ball.GetComponent<Rigidbody>().isKinematic = true;
								JoinBall(ball);
								active = true;
							}
						}
						else if(ball.InHands || ball.color != color)
						{
							if(isAnimate)
								cell.GetComponent<Animation>().Play("Anim2");

							ball.GetComponent<Rigidbody>().isKinematic = false;
							active = false;

							if(trigger.transform.childCount > 0)
							{
								foreach(Ball b in trigger.transform.GetComponentsInChildren<Ball>())
								{
									b.transform.parent = Level.current.transform;
									b.transform.localScale = Vector3.one;
								}
							}
						}

						break;
					}
				}
			}
		}
		else if(active)
				{
					active = false;

			if(trigger.transform.childCount > 0)
			{
				foreach(Ball b in trigger.transform.GetComponentsInChildren<Ball>())
				{
					b.transform.parent = Level.current.transform;
					b.transform.localScale = Vector3.one;
				}
			}
				}
	}

	public void JoinBall(Ball ball)
	{



		ball.transform.parent = trigger.transform;



		/*AnimationCurve curveX = new AnimationCurve(new Keyframe(0, ball.transform.localPosition.x), new Keyframe(timeForJoinBall, 0));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, ball.transform.localPosition.y), new Keyframe(timeForJoinBall, 0));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, ball.transform.localPosition.z), new Keyframe(timeForJoinBall, 0));
		
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);*/


		ball.transform.SetParent (trigger.transform);
		ball.GetComponent<Animation>().AddClip(Game.CreateAnimationClip(Game.AnimationClipType.POSITION, ball.transform.localPosition, Vector3.zero, timeForJoinBall), "ConnectBall");
		ball.GetComponent<Animation>().Play("ConnectBall");

		if(ball.type == Ball.BallType.NORMAL)
			ball.transform.localScale = Vector3.one;

		if(isAnimate)
			cell.GetComponent<Animation>().Play("Anim");
		//reverseColorPlane.GetComponent<Animation>().Play("Anim");
		//colorPlane.GetComponent<Animation>().Play("Anim");

	}



	public static void Join(XmlNode xml, Level level, Cell cell)
	{
		float x = Game.GetFloat(xml, "x");// float.Parse(xml.Attributes["x"].Value);
		float z = Game.GetFloat(xml, "z"); //float.Parse(xml.Attributes["z"].Value);
		
		Room room = objStack[objStack.Count - 1] as Room;
		
		Vector3 position = Vector3.zero;

		if(cell.type == CellType.SMALL)
		{
			int side = Game.GetInt(xml, "side");

			Vector3 euler = Vector3.zero;

			if(side < 2)
			{
				euler += Vector3.forward*90f * (side%2==0 ? -1 : 1);
			}
			else
			{
				euler += Vector3.right*90f * (side%2==0 ? 1 : -1);
			}

			int index = Game.GetInt(xml, "side") > 3 ? 0 : 2;

			position[index] = 0.5f + lineThick - room.Size[index]/2f + x*(room.Size[index] - 1 - lineThick*2f)/100f;
			position.y = 0.5f + lineThick - room.Size.y/2f + z*(room.Size.y - 1 - lineThick*2f)/100f;
			
			cell.transform.position = room.side[side].transform.position + position;

			cell.transform.localEulerAngles = euler;



			// ----------------------


			Mesh test = new Mesh();
			
			Vector3 p = new Vector3(0, 1.88f, 0); //Player.controller.height
			
			List<Vector3> verts = new List<Vector3>();
			float size = 0.5f;
			float step = 0.1f;
			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, size, -size), p, UnityEngine.Random.Range(1f, 4f)));
			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, -size, -size), p, UnityEngine.Random.Range(1f, 4f)));
			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, size, size), p, UnityEngine.Random.Range(1f, 4f)));
			
			test.SetVertices(verts);
			test.triangles = new int[]{0, 1, 2};
			
			test.RecalculateNormals();
			
			GameObject obj = Word.GetGameObject(test);
			obj.transform.parent = cell.transform;



			test = new Mesh();
			

			
			verts = new List<Vector3>();

			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, -size, size), p, UnityEngine.Random.Range(1f, 4f)));
			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, -size, -size), p, UnityEngine.Random.Range(1f, 4f)));
			verts.Add(Vector3.MoveTowards( cell.transform.position + new Vector3(0, size, size), p, UnityEngine.Random.Range(1f, 4f)));
			
			test.SetVertices(verts);
			test.triangles = new int[]{0, 1, 2};
			
			test.RecalculateNormals();
			
			obj = Word.GetGameObject(test);
			obj.transform.parent = cell.transform;


		}
		else if(cell.type == CellType.TOP)
		{
			position.x = 0.5f + lineThick - room.Size.x/2f + x*(room.Size.x - 1 - lineThick*2f)/100f;
			position.z = 0.5f + lineThick - room.Size.z/2f + z*(room.Size.z - 1 - lineThick*2f)/100f;
			//cell.transform.localScale -= Vector3.up*2f;
			cell.transform.localEulerAngles += Vector3.forward * 180f;
			cell.transform.position = room.side[3].transform.position - position;
		}
		else
		{
			position.x = 0.5f + lineThick - room.Size.x/2f + x*(room.Size.x - 1 - lineThick*2f)/100f;
			position.z = 0.5f + lineThick - room.Size.z/2f + z*(room.Size.z - 1 - lineThick*2f)/100f;
			
			cell.transform.position = room.side[2].transform.position - position;
		}
	}

	static public Cell CreateCell(Obj.Colour color = Obj.Colour.WHITE, CellType t = CellType.DOWN)
	{
		GameObject gObject = new GameObject("Container"); //GameObject.CreatePrimitive(PrimitiveType.Quad);
		//gObject.name = "Container";
		Cell cell = Obj.Create<Cell>();
		cell.cell = gObject;
		cell.color = color;
		
		cell.type = t;

		cell.CreateLines (gObject.transform, 1);//t==CellType.SMALL ? 0.5f : 1);

		cell.plane = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		cell.plane.GetComponent<Renderer>().material.color = Game.GetColor(cell.color);
		//cell.plane.transform.localPosition = new Vector3(0, 0, -0.001f);
		Game.SetRenderQueue (cell.plane, 3);
		cell.plane.transform.parent = cell.cell.transform;
		if(t == CellType.SMALL)
		{
			cell.type = t;
			//cell.plane.transform.localScale /= 2f;
		}


		gObject.AddComponent<Animation>().AddClip(Game.CreateAnimationClip(Quaternion.Euler(new Vector3(90, 0, 0)), Quaternion.Euler(new Vector3(90, 0, 90)), 0.5f), "Anim");

		gObject.GetComponent<Animation>().AddClip(Game.CreateAnimationClip(Quaternion.Euler(new Vector3(90, 0, 90)), Quaternion.Euler(new Vector3(90, 0, 0)), 0.5f), "Anim2");
		


		cell.colorPlane = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		cell.colorPlane.GetComponent<Renderer>().material.color = Game.GetColor(cell.color);
		cell.colorPlane.transform.parent = cell.cell.transform;
		cell.colorPlane.transform.localScale = Vector3.zero;
		cell.colorPlane.AddComponent<Animation>().AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, Vector3.zero, 0.75f), "Anim");
		cell.colorPlane.transform.localPosition = new Vector3(0, 0, -0.002f);


		cell.reverseColorPlane = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		cell.reverseColorPlane.GetComponent<Renderer>().material.color = Game.ReverseColor( Game.GetColor(cell.color) );
		cell.reverseColorPlane.transform.parent = cell.cell.transform;
		cell.reverseColorPlane.transform.localScale = Vector3.zero;
		cell.reverseColorPlane.AddComponent<Animation>().AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, Vector3.zero, 0.75f, 0.015f), "Anim");
		cell.reverseColorPlane.transform.localPosition = new Vector3(0, 0, -0.001f);


		gObject.transform.localEulerAngles = Vector3.right * 90;
		gObject.transform.localPosition = Vector3.up / 500f;
		gObject.transform.parent = cell.transform;
		
		cell.trigger = Trigger.NonXmlCreate(t == CellType.SMALL ? Vector3.one/2f : Vector3.one, Trigger.TriggerType.BALL);
		//cell.trigger.type = Trigger.TriggerType.BALL;
		cell.trigger.color = cell.color;
		cell.trigger.transform.localPosition += Vector3.up*cell.plane.transform.localScale.x/2f;
		cell.trigger.transform.parent = cell.transform;




		
		return cell;
	}

	static void AddParticleSystem(Cell cell)
	{
		GameObject psystem = new GameObject("Particle System");
		psystem.transform.parent = cell.cell.transform;
		psystem.transform.localPosition = Vector3.zero;
		psystem.transform.localEulerAngles = Vector3.right * (180);
		
		ParticleSystem prtcl = psystem.AddComponent<ParticleSystem>();
		prtcl.GetComponent<Renderer>().material = Game.BaseMaterial;
		prtcl.GetComponent<Renderer>().material.color = Game.Black;
		prtcl.startSize = 0.1f;
		//Join(xml, Level.current, cell);
	}

	public override void Repaint ()
	{
		foreach(GameObject l in line)
			l.GetComponent<Renderer>().material.color = Game.GetColor(color);

		plane.GetComponent<Renderer>().material.color = Game.GetColor( color = Game.ReverseColor(color) );
	}
	
	static public Cell Create(XmlNode xml)
	{

		CellType t = CellType.DOWN;
		if(xml.Attributes["type"] != null)
			t = (CellType)Enum.Parse(typeof(CellType), xml.Attributes["type"].Value, true);
		Cell cell = Cell.CreateCell(Game.GetColor(xml.Attributes["color"].Value), t);

		Join(xml, Level.current, cell);

		return cell;
	}

	public void CreateLines(Transform container, float size = 1f)
	{
		line = new GameObject[4];

		for(int i=0; i<line.Length; ++i)
		{
			line[i] = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
			line[i].transform.localScale = new Vector3(lineThick, size + lineThick*2, size);
			line[i].GetComponent<Renderer>().material.color = Game.ReverseColor(Game.GetColor(color));
			line[i].name = "Line_" + i.ToString();

			Vector3 position = Vector3.zero;
			position[i/2] = (size/2f + lineThick/2f) * (i%2==0 ? 1 : -1);

			line[i].transform.localPosition = position;

			if(i > 1)
				line[i].transform.localEulerAngles = Vector3.forward * 90;

			line[i].transform.parent = container;
		}

	}

}
