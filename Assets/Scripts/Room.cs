using UnityEngine;
using System.Collections;
using System.Xml;
using System;

public class Room : Obj 
{
	public Side[] side;
	public Wall[] wall;
	public Ledge[] ledge;
	public Cell[] cell;
	public Trigger[] trigger;

	Vector3 size = Vector3.zero;

	public Vector3 Size
	{
		get
		{
			return size;
		}
		set
		{
			size = value;
		}
	}
	
	public GameObject container;

	static public Room NonXmlCreate(Vector3 size, Obj.Colour c)
	{
		Room room = Obj.Create<Room>();
		room.Size = size;
		room.side = new Side[6];
		room.color = c;
		
		room.container = new GameObject("Container");
		room.container.transform.position = room.transform.position;
		
		for(int i=0; i<room.side.Length; ++i)
		{
			float x = 0, y = 0, z = 0;
			int tmp = i/2;
			switch(tmp)
			{
			case 0: x = 2; y = 1; z = 0; break;
			case 1: x = 0; y = 2; z = 1; break;
			case 2: x = 0; y = 1; z = 2; break;
			default: break;
			}
			
			float width = room.Size[(int)x];
			float height = room.Size[(int)y];
			
			
			room.side[i] = Side.Create(room.color, new Vector3(width, height, room.Size[(int)z]));
			room.side[i].name += i.ToString();
			room.side[i].index = i;
			//room.side[i].color = room.color;
			
			{
				Mesh mesh = room.side[i].GetComponent<MeshFilter>().mesh;
				
				
				float line = Side.GetMaterial(room.color).GetFloat("_LineWidth");
				
				
				x = line*(width-1)/width;
				y = line*(height-1)/(height);
				mesh.vertices = new Vector3[] {new Vector3(-0.5f, -0.5f, 0), new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(0.5f, -0.5f, 0)};
				mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
				mesh.uv = new Vector2[] {new Vector2(x, y), new Vector2(x, 1-y), new Vector2(1-x, 1-y), new Vector2(1-x, y)};
			}
			
			room.side[i].transform.parent = room.container.transform;
			
			Vector3 pos = Vector3.zero;
			Vector3 rot = new Vector3(0, 90, 0);
			
			if(i < 2 || i > 3)
			{
				pos[1] = 0.5f; // если не пол и не потолок, то по Y всегда 0.5
				pos[i/2] = 0.5f * (i%2==0 ? -1 : 1);
				int r = 0;
				switch(i)
				{
				case 0: r = 3; break;
				case 1: r = 1; break;
				case 4: r = 2; break;
				case 5: r = 0; break;
				default: r = 0; break;
				}
				rot *= r;
			}
			else
			{
				rot = new Vector3(90, 0, 0);
				if(i == 3)
				{
					rot *= -1;
					pos[1] = 1;
				}
			}
			
			room.side[i].transform.localEulerAngles = rot;
			room.side[i].transform.localPosition += pos;
			
			
		}
		
		room.container.transform.parent = room.transform;
		room.container.transform.localScale = room.Size;

		return room;
	}

	static public Room Create(XmlNode xml)
	{
		Room room = Room.NonXmlCreate(new Vector3(float.Parse(xml.Attributes["x"].Value), float.Parse(xml.Attributes["y"].Value), float.Parse(xml.Attributes["z"].Value)), Game.GetColor(xml.Attributes["color"].Value));

		return Obj.Create<Room>(room, xml);
	}



	public static void NonXmlJoin(Room room1, Room room2, int sideIndex, float x, float y, Level level)
	{
		Room[] room = new Room[] { room1, room2 };
		int smallRoomX = 0, smallRoomY = 0, smallSideX = 0, largeSideX = 0, smallSideY = 0, largeSideY = 0;
		int largeRoomX = 0, largeRoomY = 0;

		room2.transform.position = Vector3.zero;
		
		int side1 = sideIndex;
		int side2 = side1 + (side1%2==0 ? 1 : -1);

		int[] side = new int[] { side1, side2 };
		
		int xIndex = side1/2 != 0 ? 0 : 2;
		int yIndex = side1/2 != 1 ? 1 : 2;
		
		if(room[0].Size[xIndex] > room[1].Size[xIndex])
		{
			smallRoomX = 1;

			smallSideX = side2;
			largeSideX = side1;
		}
		else 
		{
			largeRoomX = 1;

			smallSideX = side1;
			largeSideX = side2;
		}
		if(room[0].Size[yIndex] > room[1].Size[yIndex])
		{
			smallRoomY = 1;

			smallSideY = side2;
			largeSideY = side1;
		}
		else
		{
			largeRoomY = 1;

			smallSideY = side1;
			largeSideY = side2;
		}
		
		Vector3 position = Vector3.zero;
		
		position[xIndex] = room2.Size[xIndex]/2f - room1.Size[xIndex]/2f + x*(room1.Size[xIndex] - room2.Size[xIndex])/100f;
		position[yIndex] = room2.Size[yIndex]/2f - room1.Size[yIndex]/2f + y*(room1.Size[yIndex] - room2.Size[yIndex])/100f;
		
		room2.transform.position = room1.side[side1].transform.position - room2.side[side2].transform.position + position;
		
		room1.side[side1].GetComponent<Renderer>().enabled = false;
		room2.side[side2].GetComponent<Renderer>().enabled = false;

		if(room1.side[side1].GetComponent<Collider>() != null)
			room1.side[side1].GetComponent<Collider>().enabled = false;

		if(room1.side[side1].GetComponent<BoxCollider>() != null)
			room1.side[side1].GetComponent<BoxCollider>().enabled = false;

		if(room2.side[side2].GetComponent<Collider>() != null)
			room2.side[side2].GetComponent<Collider>().enabled = false;

		if(room2.side[side2].GetComponent<BoxCollider>() != null)
			room2.side[side2].GetComponent<BoxCollider>().enabled = false;
		
		Side tmpSide = null;
		if( smallRoomX==0 && smallRoomY==0 )
			tmpSide = room1.side[side1];
		else if( smallRoomX==1 && smallRoomY==1 )
			tmpSide = room2.side[side2];
		//if( (smallRoomX==0 && smallRoomY==0) || (smallRoomX==1 && smallRoomY==1) )

		if(room1.side[side1].Size == room2.side[side2].Size)
		{
			foreach(Line line in room1.side[side1].line)
				line.gameObject.SetActive(false);

			foreach(Line line in room2.side[side2].line)
				line.gameObject.SetActive(false);


			for(int u=0; u<room.Length; ++u)
			{
				for(int i=0; i<room[u].side.Length; ++i)
				{
					int lineIndex = 0;
					if(i == side1 || i == side2)
						continue;
					else if(side1/2 == 0)
					{
						//Debug.Log("asdasfasfasfasf");
						//room[u].side[i].gameObject.SetActive(false);
						lineIndex = i!=4 ? side[u>0?0:1] : side[u];
						 //i!=4 ? (i + (i%2==0 ? 1 : -1)) : i

						//Debug.Log(room[u].side[i].line[i!=4 ? (i%2==0 ? 1 : 0) : i].gameObject.name);
					}
					else if(side1/2 == 1)
					{
						//Debug.LogError("___");
						lineIndex = side[u==0 ? 1 : 0];
					}
					else
					{
						if(side[u] == 4)
							lineIndex = i%2==0 ? (i+1) : (i-1);
							
						else
							lineIndex = i;
					}
					if(room[u].side[i].line[lineIndex].name[0] != '_')
						room[u].side[i].line[lineIndex].gameObject.SetActive(false);
				}
			}


			/*for(int i=0; i<room1.side.Length; ++i)
			{
				if(i == side1 || i == side2)
					continue;

				int sideIndex = i ;


				room1.side[i].line[index].gameObject.SetActive(false);
			}

			for(int i=0; i<room2.side.Length; ++i)
			{
				if(i == side1 || i == side2)
					continue;



				int ind = i + (i%2==0 ? 1 : -1);
				room2.side[i].line[ind].gameObject.SetActive(false);
			}*/
		}

		if(tmpSide != null)
			for(int i=0; i<tmpSide.line.Length; ++i)
		{
			if(i==3 && y==0)
				tmpSide.line[i].gameObject.SetActive(false);

			tmpSide.line[i].transform.localEulerAngles += Vector3.up * 180;
			tmpSide.line[i].name = "_" + tmpSide.line[i].name;
			
			Vector3 pos = Vector3.zero;
			pos[i/2] = Line.height * (i%2==0 ? 1 : -1);
			//tmpSide.line[i].transform.position += pos;
			
		}


		int parentIndex = largeRoomX==largeRoomY ? largeRoomX : 0;
		Transform[] part = new Transform[4];
		
		for(int i=0; i<part.Length; ++i)
		{
			part[i] = CustomObject.CreatePrimitive(PrimitiveType.Quad).transform;
			part[i].transform.rotation = Quaternion.identity;

			//Destroy(part[i].GetComponent<MeshCollider>());

			float tmp = 1f / room[parentIndex].Size.z;
			//float colliderSize = 0.2f * room[largeRoomX].Size[largeSideX/2];
			//part[i].gameObject.AddComponent<BoxCollider>().size = new Vector3(1, 1, colliderSize);
			//part[i].GetComponent<BoxCollider>().center = new Vector3(0, 0, colliderSize/2f);

			part[i].tag = "Side";
			//part[i].parent = room1.side[side1].transform;
			part[i].name = i.ToString();

			//part[i].localEulerAngles = room1.side[side1].transform.localEulerAngles;
		}
		
		Vector3 scale = Vector3.one;
		scale[1] = room1.Size[yIndex]/2f + (room1.side[side1].transform.position[yIndex] - room2.side[side2].transform.position[yIndex]) - room2.Size[yIndex]/2f;
		scale[0] = room1.Size[xIndex];
		
		position = Vector3.zero;
		
		position[yIndex] = room2.Size[yIndex]/2f + scale[1]/2f;
		position[xIndex] = room1.side[side1].transform.position[xIndex] - room2.side[side2].transform.position[xIndex];
		
		part[0].localScale = scale;
		part[0].position = room2.side[side2].transform.position + position;
		part[0].eulerAngles = room1.side[side1].transform.eulerAngles;
		
		
		
		
		
		
		scale = Vector3.one;
		scale[1] = room1.Size[yIndex]/2f + (room2.side[side2].transform.position[yIndex] - room1.side[side1].transform.position[yIndex]) - room2.Size[yIndex]/2f;
		scale[0] = room1.Size[xIndex];
		
		position = Vector3.zero;
		
		position[yIndex] = room2.Size[yIndex]/2f + scale[1]/2f; //==2 ? 1 : 2
		position[xIndex] = room2.side[side2].transform.position[xIndex] - room1.side[side1].transform.position[xIndex];
		
		part[1].localScale = scale;
		part[1].position = room2.side[side2].transform.position - position;
		part[1].eulerAngles = room1.side[side1].transform.eulerAngles;
		
		
		
		
		
		
		scale = Vector3.one;
		scale[1] = room2.Size[yIndex];
		scale[0] = room1.Size[xIndex]/2f + (room2.side[side2].transform.position[xIndex] - room1.side[side1].transform.position[xIndex]) - room2.Size[xIndex]/2f;
		
		position = Vector3.zero;
		
		position[xIndex] = room2.Size[xIndex]/2f + scale[0]/2f;
		
		part[2].localScale = scale;
		part[2].position = room2.side[side2].transform.position - position;
		part[2].eulerAngles = room1.side[side1].transform.eulerAngles;
		
		
		if(smallRoomY==0 && smallRoomX==1)
		{
			//scale[1] = room[largeRoomY].Size[yIndex]/2f + (room[largeRoomY].side[largeSideY].transform.position[yIndex] - room[smallRoomY].side[smallSideY].transform.position[yIndex]) - room[smallRoomY].Size[yIndex]/2f;
			scale[1] = room1.Size[yIndex]/2f + (room1.side[side1].transform.position[yIndex] - room2.side[side2].transform.position[yIndex]) - room2.Size[yIndex]/2f;
			scale[0] = room[smallRoomX].Size[xIndex];//
			
			position = Vector3.zero;
			
			position[yIndex] = room2.Size[yIndex]/2f + scale[1]/2f;
			
			part[0].localScale = scale;
			part[0].position = room2.side[side2].transform.position + position;
			
			
			
			
			
			
			scale = Vector3.one;
			scale[1] = room1.Size[yIndex]/2f + (room2.side[side2].transform.position[yIndex] - room1.side[side1].transform.position[yIndex]) - room2.Size[yIndex]/2f;
			scale[0] = room[smallRoomX].Size[xIndex];
			
			position = Vector3.zero;
			
			position[yIndex] = room2.Size[yIndex]/2f + scale[1]/2f; //==2 ? 1 : 2
			//position[xIndex] = room2.side[side2].transform.position[xIndex] - room1.side[side1].transform.position[xIndex];
			
			part[1].localScale = scale;
			part[1].position = room2.side[side2].transform.position - position;
		}
		
		
		scale = Vector3.one;
		scale[1] = room2.Size[yIndex];
		scale[0] = room1.Size[xIndex]/2f + (room1.side[side1].transform.position[xIndex] - room2.side[side2].transform.position[xIndex]) - room2.Size[xIndex]/2f;
		
		position = Vector3.zero;
		
		position[xIndex] = room2.Size[xIndex]/2f + scale[0]/2f;
		
		part[3].localScale = scale;
		part[3].position = room2.side[side2].transform.position + position;
		part[3].eulerAngles = room1.side[side1].transform.eulerAngles;
		
		if(smallRoomY==1 && smallRoomX==0)
			for(int i=2; i<part.Length; ++i)
				part[i].eulerAngles += Vector3.up * 180f;
		
		if(smallRoomX==0 && smallRoomY==0)// && !(sideIndex==3 && largeSideX==2 && largeSideY==2) )
		{
			for(int i=0; i<part.Length; ++i)
				part[i].eulerAngles += Vector3.up * 180f;
		}
		
		

		
		for(int i=0; i<part.Length; ++i)
		{
			part[i].parent = room[parentIndex].side[side[parentIndex]].transform;//room1.side[side1].transform;
			part[i].GetComponent<Renderer>().material.color = Game.GetColor(room[parentIndex].side[side[parentIndex]].color);

			if(side2==3 && largeSideX==largeSideY && largeSideX==side2)
				part[i].transform.localEulerAngles += Vector3.up*180f;

			if(sideIndex==3 && largeSideX==2 && largeSideY==2)
				part[i].transform.localEulerAngles += Vector3.up*180f;
		}

		/*if(sideIndex==3 && largeSideX==2 && largeSideY==2)
		{
			room[1].side[2].transform.localScale = new Vector3(1, 1, -1);
			//room[0].side[3].line[3].gameObject.SetActive(true);
		}*/

		if( y == 0 && room1.Size[sideIndex/2] != room2.Size[sideIndex/2] )
		{
			//Debug.LogWarning("____");
			//room[largeRoomX].side[largeSideX].name += "LARGE";
			Transform[] line = {
				room[largeRoomX].side[largeSideX].line[3].transform,
				(Transform)UnityEngine.Object.Instantiate(room[largeRoomX].side[largeSideX].line[3].transform)
			};



			line[1].transform.parent = line[0].transform.parent;
			line[1].transform.eulerAngles = line[0].transform.eulerAngles;


			Vector3 lineScale = line[0].localScale;
			lineScale.y = part[2].localScale.x;
			line[0].localScale = lineScale;
			line[0].name += "____";

			Vector3 pos = line[0].localPosition;
			pos[0] = part[2].localPosition[0];
			line[0].localPosition = pos;
			//line[0].localPosition = part[2].localPosition - Vector3.up*part[2].localScale.y/2f;// + Vector3.up*Line.height/2f;

			//line[0].localPosition

			//lineScale = line[0].localScale;
			lineScale.y = part[3].localScale.x;
			line[1].localScale = lineScale;
			line[1].name += "____";


			//pos = line[1].localPosition;
			pos[0] = part[3].localPosition[0];
			line[1].localPosition = pos;
			//line[1].localPosition = part[3].localPosition - Vector3.up*part[3].localScale.y/2f;// + Vector3.up*Line.height/2f;

			line[1].transform.parent = line[0].transform.parent;

			line[0].GetComponent<Line>().SetClone(line[1].GetComponent<Line>() as Line);
		}

		foreach (Transform p in part) 
		{
			Vector3 euler = p.localEulerAngles;

			for(int i=0; i<3; ++i)
			{
				if(euler[i] !=0 && euler[i] < 5f && euler[i] > -5f)
					euler[i] = 0f;
			}

			p.localEulerAngles = euler;
		}

	}

	public static void Join(XmlNode xml, Level level)
	{
		NonXmlJoin(level.room[int.Parse( xml.Attributes["index1"].Value )], level.room[int.Parse( xml.Attributes["index2"].Value )], int.Parse( xml.Attributes["sideIndex"].Value ), float.Parse( xml.Attributes["x"].Value ), float.Parse( xml.Attributes["y"].Value ), level);
	}

	override public void Repaint()
	{
		base.Repaint();
		
		foreach(Side s in side)
			s.Repaint();
	}

	override public void Paint(Colour c)
	{
		base.Paint(c);
		
		foreach(Side s in side)
			s.Paint(c);
	}
	
	override public void Draw()
	{
		//Debug.Log("Draw room");
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
