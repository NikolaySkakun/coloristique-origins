using UnityEngine;
using System.Collections;
using System.Xml;

public class Ledge : Obj 
{
	static public float stageHeight = 3.8f;
	static public float ledgeWidth = 1.5f;
	static float ledgeThick = 0.008f;

	public GameObject ledge, stairs, ledgePlane, stairsPlane;
	GameObject[] ledgeLines, stairsLines;

	public Cell cell;
	bool up = false;

	static public void Join(XmlNode xml, Level level)
	{
		level.room[ Game.GetInt(xml, "ledgeRoom")].ledge[ Game.GetInt(xml, "ledgeIndex") ].cell = level.room[ Game.GetInt(xml, "cellRoom")].cell[ Game.GetInt(xml, "cellIndex") ];
	}

	override public void Draw()
	{
		Animation anim;// = line[i].AddComponent<Animation>() as Animation;
		AnimationClip clip;// = new AnimationClip();
		AnimationCurve[] scale = new AnimationCurve[3];
		AnimationCurve[] position = new AnimationCurve[3];
		for(int i=0; i<ledgeLines.Length; ++i)
		{
			anim = ledgeLines[i].AddComponent<Animation>() as Animation;
			clip = new AnimationClip();
			clip.legacy = true;
			for(int u=0; u<scale.Length; ++u)
			{
				scale[u] = new AnimationCurve(
					new Keyframe((i/2)*Game.drawTime/4f, u!=0 ? ledgeLines[i].transform.localScale[u] : 0), 
					new Keyframe((i/2 + 1)*Game.drawTime/4f, ledgeLines[i].transform.localScale[u]));

				if(i==1)
				{
					position[u] = new AnimationCurve(
						new Keyframe((i/2)*Game.drawTime/4f, (i<2 && u==0 || i>1 && u==2) ? -0.5f : ledgeLines[0].transform.localPosition[u]),
						new Keyframe((i/2 + 1)*Game.drawTime/4f, ledgeLines[0].transform.localPosition[u]),
						new Keyframe((i/2 + 2)*Game.drawTime/4f, ledgeLines[i].transform.localPosition[u]));
				}
				else
				{
					position[u] = new AnimationCurve(
						new Keyframe((i/2)*Game.drawTime/4f, (i<2 && u==0 || i>1 && u==2) ? -0.5f : ledgeLines[i].transform.localPosition[u]), 
						new Keyframe((i/2 + 1)*Game.drawTime/4f, ledgeLines[i].transform.localPosition[u]));
				}
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

		anim = ledgePlane.AddComponent<Animation>() as Animation;
		clip = new AnimationClip();
		clip.legacy = true;
		for(int u=0; u<scale.Length; ++u)
		{
			scale[u] = new AnimationCurve(
				new Keyframe(0, 0),
				new Keyframe(Game.drawTime/4f, 0),
				new Keyframe(Game.drawTime/4f, u!=2 ? ledgePlane.transform.localScale[u] : 0), 
				new Keyframe(Game.drawTime/2f, ledgePlane.transform.localScale[u]));
			
			position[u] = new AnimationCurve(
				new Keyframe(Game.drawTime/4f, u!=2 ? ledgePlane.transform.localPosition[u] : -0.5f), 
				new Keyframe(Game.drawTime/2f, ledgePlane.transform.localPosition[u]));
		}
		
		clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
		clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
		clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);
		
		clip.SetCurve("", typeof(Transform), "localPosition.x", position[0]);
		clip.SetCurve("", typeof(Transform), "localPosition.y", position[1]);
		clip.SetCurve("", typeof(Transform), "localPosition.z", position[2]);



		anim.AddClip(clip, "Draw");
		anim.Play("Draw");







		if(stairs != null)
		{
			for(int i=0; i<stairsLines.Length; ++i)
			{
				anim = stairsLines[i].AddComponent<Animation>() as Animation;
				clip = new AnimationClip();
				clip.legacy = true;
				for(int u=0; u<scale.Length; ++u)
				{
					scale[u] = new AnimationCurve(
						new Keyframe(0, 0),
						new Keyframe(Game.drawTime/2f, 0),
						new Keyframe(Game.drawTime/2f , (u==0 && i>1) ? 0 : stairsLines[i].transform.localScale[u]), 
						new Keyframe(Game.drawTime, stairsLines[i].transform.localScale[u]));

					if(i==1)
					{
						position[u] = new AnimationCurve(
							new Keyframe(Game.drawTime/2f, ((i>1) && u==2) ? -0.5f : stairsLines[0].transform.localPosition[u]),
							new Keyframe(Game.drawTime/2f, ((i>1) && u==2) ? -0.5f : stairsLines[0].transform.localPosition[u]), 
							new Keyframe(Game.drawTime, stairsLines[i].transform.localPosition[u]));
					}
					else
					{
						position[u] = new AnimationCurve(
							new Keyframe(Game.drawTime/2f, ((i>1) && u==2) ? -0.5f : stairsLines[i].transform.localPosition[u]), 
							new Keyframe(Game.drawTime, stairsLines[i].transform.localPosition[u]));
					}
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

			anim = stairsPlane.AddComponent<Animation>() as Animation;
			clip = new AnimationClip();
			clip.legacy = true;
			for(int u=0; u<scale.Length; ++u)
			{
				scale[u] = new AnimationCurve(
					new Keyframe(0, 0),
					new Keyframe(Game.drawTime/2f, 0),
					new Keyframe(Game.drawTime/2f, u!=2 ? stairsPlane.transform.localScale[u] : 0), 
					new Keyframe(Game.drawTime, stairsPlane.transform.localScale[u]));
				
				position[u] = new AnimationCurve(
					new Keyframe(Game.drawTime/2f, u!=2 ? stairsPlane.transform.localPosition[u] : -0.5f), 
					new Keyframe(Game.drawTime, stairsPlane.transform.localPosition[u]));
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
	}

	static GameObject CreateBorderCube(Ledge l, Obj.Colour color, Vector3 size, string type = "Ledge")
	{
		Color borderColor = Game.GetColor(Game.ReverseColor(color));
		Color sideColor = Game.GetColor(color);

		GameObject container = new GameObject("Container");
		GameObject cube = CustomObject.CreatePrimitive(PrimitiveType.Cube);
		GameObject[] side = new GameObject[4];
		float width = size.x;
		float height = size.z;
		for(int i=0; i<side.Length; ++i)
		{
			float x = 0, y = 0;

			side[i] = CustomObject.CreatePrimitive(PrimitiveType.Cube);
			side[i].GetComponent<Collider>().enabled = false;
			side[i].name = i.ToString();

			side[i].GetComponent<Renderer>().material.color = borderColor;
			side[i].transform.localScale = Vector3.one - Vector3.forward*(1f - Line.height/(i<2 ? height : width));

			side[i].transform.parent = container.transform;
		}

		cube.transform.localScale = new Vector3(1f - Line.height/width, 0.8f, 1f - Line.height/height);
		cube.transform.parent = container.transform;

		side[0].transform.position = Vector3.back / 2f;
		side[1].transform.position = Vector3.forward / 2f;
		side[2].transform.position = Vector3.left / 2f;
		side[3].transform.position = Vector3.right / 2f;

		side[1].transform.eulerAngles = Vector3.up * 180f;
		side[2].transform.eulerAngles = Vector3.up * 90f;
		side[3].transform.eulerAngles = Vector3.down * 90f;

		if(type == "Ledge")
		{
			l.ledgeLines = side;
			l.ledgePlane = cube;
		}
		else
		{
			l.stairsLines = side;
			l.stairsPlane = cube;
		}
		//cube.SetActive(false);
		return container;
	}

	void Update()
	{

		float animTime = 1f;
		
		if(stairs)
		{
			if(Input.GetKeyUp(KeyCode.M))
			{
				stairsPlane.GetComponent<Animation>()["Draw"].speed = -1;
				stairsPlane.GetComponent<Animation>()["Draw"].time = stairsPlane.GetComponent<Animation>()["Draw"].length;
				stairsPlane.GetComponent<Animation>().Play("Draw");
			}
		}

		if(cell != null)
		{
			if(up)
			{
				if((cell.IsActive && cell.color == Obj.Colour.WHITE) || (!cell.IsActive && cell.color == Obj.Colour.BLACK))
				{
					AnimationCurve[] curve = new AnimationCurve[4];
					Quaternion zero = Quaternion.Euler(Vector3.zero), down = Quaternion.Euler(Vector3.right * 44.5f);
					AnimationClip clip = stairs.GetComponent<Animation>().GetClip("Down");

					for(int i=0; i<curve.Length; ++i)
					{
						curve[i] = new AnimationCurve(
							new Keyframe(0, stairs.transform.localRotation[i]), 
							new Keyframe(animTime, down[i]));
					}
					
					clip.SetCurve("", typeof(Transform), "localRotation.x", curve[0]);
					clip.SetCurve("", typeof(Transform), "localRotation.y", curve[1]);
					clip.SetCurve("", typeof(Transform), "localRotation.z", curve[2]);
					clip.SetCurve("", typeof(Transform), "localRotation.w", curve[3]);

					stairs.GetComponent<Animation>().Play("Down");
					up = false;
				}
			}
			else
			{
				if((!cell.IsActive && cell.color == Obj.Colour.WHITE) || (cell.IsActive && cell.color == Obj.Colour.BLACK))
				{
//					stairs.GetComponent<Animation>().Stop();
//					stairs.GetComponent<Rigidbody>().MoveRotation(Quaternion.Euler(Vector3.Lerp(stairs.transform.localEulerAngles, Vector3.zero, Time.deltaTime)));
//
//					if(stairs.transform.localEulerAngles == Vector3.zero)
//						up = true;

					AnimationCurve[] curve = new AnimationCurve[4];
					Quaternion zero = Quaternion.Euler(Vector3.zero), down = Quaternion.Euler(Vector3.right * 44.5f);
					AnimationClip clip = stairs.GetComponent<Animation>().GetClip("Up");

					for(int i=0; i<curve.Length; ++i)
					{
						curve[i] = new AnimationCurve(
							new Keyframe(0, stairs.transform.localRotation[i]), 
							new Keyframe(animTime, zero[i]));
					}
					
					clip.SetCurve("", typeof(Transform), "localRotation.x", curve[0]);
					clip.SetCurve("", typeof(Transform), "localRotation.y", curve[1]);
					clip.SetCurve("", typeof(Transform), "localRotation.z", curve[2]);
					clip.SetCurve("", typeof(Transform), "localRotation.w", curve[3]);

					stairs.GetComponent<Animation>().animatePhysics = true;
					stairs.GetComponent<Animation>().Play("Up");
					up = true;
				}
			}
		}
	}

	public static void JoinToRoom(Ledge ledge, Room room, int sideIndex, int stage, float x)
	{
		//Room room = level.room[int.Parse(xml.Attributes["roomIndex"].Value)];
		//Ledge ledge = room.ledge[int.Parse(xml.Attributes["ledgeIndex"].Value)];
		
		//int sideIndex = int.Parse(xml.Attributes["sideIndex"].Value);
		int originalSideIndex = sideIndex;
		Side side = room.side[sideIndex];
		
		Vector3 position = room.transform.position + Vector3.up*stage*stageHeight;
		position[sideIndex/2] = side.transform.position[sideIndex/2] + (ledgeWidth/2f + Line.height/2f)*(sideIndex%2==0 ? 1 : -1);
		
		sideIndex = sideIndex/2 == 0 ? 2 : 0;
		
		position[sideIndex] = side.transform.position[sideIndex] - side.Size[0]/2f + ledge.ledge.transform.localScale.x/2f + x*((side.Size[0] - ledge.ledge.transform.localScale.x)/100f);
		switch(originalSideIndex)
		{
		case 0: sideIndex = 90; break;
		case 1: sideIndex = 270; break;
		case 4: sideIndex = 0; break;
		case 5: sideIndex = 180; break;
		default: sideIndex = 0; break;
		}
		//sideIndex -= 90;
		
		ledge.transform.localEulerAngles = Vector3.up*sideIndex;
		
		
		
		ledge.transform.position = position;
	}

	static public Ledge Create(XmlNode xml)
	{

		Ledge ledge = Obj.Create<Ledge>();
		float length;

		ledge.ledge = CreateBorderCube(ledge, Obj.Colour.WHITE, new Vector3(length = Game.GetFloat(xml, "length"), ledgeThick, ledgeWidth));//GameObject.CreatePrimitive(PrimitiveType.Cube);

		ledge.ledge.transform.localScale = new Vector3(length = Game.GetFloat(xml, "length"), ledgeThick, ledgeWidth);


		ledge.ledge.transform.parent = ledge.transform;


		if(bool.Parse(xml.Attributes["stairs"].Value) )
		{
			ledge.stairs = new GameObject("Stairs");
			//ledge.stairs.AddComponent<Rigidbody>().useGravity = false;
			GameObject stairs = CreateBorderCube(ledge, Obj.Colour.WHITE, new Vector3(ledgeWidth, ledgeThick, int.Parse(xml.Attributes["stage"].Value) * stageHeight * Mathf.Sqrt(2.03f)), "Stairs");//GameObject.CreatePrimitive(PrimitiveType.Cube);
			stairs.transform.localScale = new Vector3(ledgeWidth, ledgeThick, int.Parse(xml.Attributes["stage"].Value) * stageHeight * Mathf.Sqrt(2.03f));
			ledge.stairs.transform.localPosition = new Vector3(-(length - ledgeWidth)/2f + ((length - ledgeWidth)/100f)*Game.GetFloat(xml, "stairsPosition"), 0, ledgeWidth/2f);
			stairs.transform.localPosition = ledge.stairs.transform.localPosition + Vector3.forward*(stairs.transform.localScale.z/2f);
			
			stairs.transform.parent = ledge.stairs.transform;
			ledge.stairs.transform.parent = ledge.transform;
			
			ledge.stairs.transform.localEulerAngles = Vector3.right * 44.5f;

			float animTime = 1f;
			Animation anim = ledge.stairs.AddComponent<Animation>() as Animation;

			anim.playAutomatically = false;

			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			AnimationCurve[] curve = new AnimationCurve[4];

			Quaternion zero = Quaternion.Euler(Vector3.zero), down = Quaternion.Euler(Vector3.right * 45f);

			for(int i=0; i<curve.Length; ++i)
			{
				curve[i] = new AnimationCurve(
					new Keyframe(0, ledge.stairs.transform.localRotation[i]), 
					new Keyframe(animTime, down[i]));
			}
			
			clip.SetCurve("", typeof(Transform), "localRotation.x", curve[0]);
			clip.SetCurve("", typeof(Transform), "localRotation.y", curve[1]);
			clip.SetCurve("", typeof(Transform), "localRotation.z", curve[2]);
			clip.SetCurve("", typeof(Transform), "localRotation.w", curve[3]);



			anim.AddClip(clip, "Down");

			clip = new AnimationClip();
			clip.legacy = true;
			for(int i=0; i<curve.Length; ++i)
			{
				curve[i] = new AnimationCurve(
					new Keyframe(0, ledge.stairs.transform.localRotation[i]), 
					new Keyframe(animTime, zero[i]));
			}
			
			clip.SetCurve("", typeof(Transform), "localRotation.x", curve[0]);
			clip.SetCurve("", typeof(Transform), "localRotation.y", curve[1]);
			clip.SetCurve("", typeof(Transform), "localRotation.z", curve[2]);
			clip.SetCurve("", typeof(Transform), "localRotation.w", curve[3]);



			anim.AddClip(clip, "Up");

		}

		JoinToRoom(ledge, Obj.objStack[Obj.objStack.Count-1] as Room, Game.GetInt(xml, "sideIndex"), Game.GetInt(xml, "stage"), Game.GetFloat(xml, "x"));
		return ledge;
	}
}
