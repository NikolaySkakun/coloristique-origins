using UnityEngine;
using System.Collections;
using System.Xml;

public class Ledge_ : Obj 
{
	static float stageHeight = 3f;
	static float ledgeWidth = 1.2f;
	static float ledgeThick = 0.008f;

	public GameObject ledge, stairs;
	//GameObject[] side;

	public Trigger trigger;

	static GameObject CreateBorderCube(Obj.Colour color, Vector3 size)
	{
		Color borderColor = Game.GetColor(Game.ReverseColor(color));
		Color sideColor = Game.GetColor(color);

		GameObject cube = new GameObject("BorderCube");
		GameObject[] side = new GameObject[6];

		for(int i=0; i<side.Length; ++i)
		{
			float x = 0, y = 0;

			
			float width = size.x;
			float height = size.z;


			side[i] = GameObject.CreatePrimitive(PrimitiveType.Quad);
			if(i < 4)
				side[i].GetComponent<Renderer>().material.color = borderColor;
			else
			{
				Mesh mesh = side[i].GetComponent<MeshFilter>().mesh;
				
				
				float line = Side.GetMaterial(color).GetFloat("_LineWidth");
				
				
				x = line*(width-1)/width;
				y = line*(height-1)/(height);
				mesh.vertices = new Vector3[] {new Vector3(-0.5f, -0.5f, 0), new Vector3(-0.5f, 0.5f, 0), new Vector3(0.5f, 0.5f, 0), new Vector3(0.5f, -0.5f, 0)};
				mesh.triangles = new int[] {0, 1, 2, 0, 2, 3};
				mesh.uv = new Vector2[] {new Vector2(x, y), new Vector2(x, 1-y), new Vector2(1-x, 1-y), new Vector2(1-x, y)};

				side[i].GetComponent<Renderer>().material = Side.GetMaterial(color);//Side.whiteSide;
				//side[i].renderer.material.color = sideColor;
			}
				

			side[i].transform.parent = cube.transform;
		}

		side[0].transform.position = Vector3.back / 2f;
		side[1].transform.position = Vector3.forward / 2f;
		side[2].transform.position = Vector3.left / 2f;
		side[3].transform.position = Vector3.right / 2f;
		side[4].transform.position = Vector3.down / 2f;
		side[5].transform.position = Vector3.up / 2f;

		side[1].transform.eulerAngles = Vector3.up * 180f;
		side[2].transform.eulerAngles = Vector3.up * 90f;
		side[3].transform.eulerAngles = Vector3.down * 90f;
		side[4].transform.eulerAngles = Vector3.left * 90f;
		side[5].transform.eulerAngles = Vector3.right * 90f;

		return cube;
	}

	/*public static void Join(XmlNode xml, Level level)
	{
		Room room = level.room[int.Parse(xml.Attributes["roomIndex"].Value)];
		Ledge ledge = room.ledge[int.Parse(xml.Attributes["ledgeIndex"].Value)];
		
		int sideIndex = int.Parse(xml.Attributes["sideIndex"].Value);
		
		Side side = room.side[sideIndex];
		
		Vector3 position = room.transform.position + Vector3.up*int.Parse(xml.Attributes["stage"].Value)*stageHeight;
		position[sideIndex/2] = side.transform.position[sideIndex/2] + ledgeWidth/2f*(sideIndex%2==0 ? 1 : -1);
		
		sideIndex = sideIndex/2 == 0 ? 2 : 0;
		
		position[sideIndex] = side.transform.position[sideIndex] - side.Size[0]/2f + ledge.ledge.transform.localScale.x/2f + int.Parse(xml.Attributes["x"].Value)*((side.Size[0] - ledge.ledge.transform.localScale.x)/100f);
		
		switch(int.Parse(xml.Attributes["sideIndex"].Value))
		{
		case 0: sideIndex = 90; break;
		case 1: sideIndex = 270; break;
		case 4: sideIndex = 0; break;
		case 5: sideIndex = 180; break;
		default: sideIndex = 0; break;
		}
		
		
		ledge.transform.localEulerAngles = Vector3.up*sideIndex;
		
		
		
		ledge.transform.position = position;
	}*/

	public static void Join(Ledge ledge, Room room, int sideIndex, int stage, float x)
	{
		//Room room = level.room[int.Parse(xml.Attributes["roomIndex"].Value)];
		//Ledge ledge = room.ledge[int.Parse(xml.Attributes["ledgeIndex"].Value)];
		
		//int sideIndex = int.Parse(xml.Attributes["sideIndex"].Value);
		int originalSideIndex = sideIndex;
		Side side = room.side[sideIndex];
		
		Vector3 position = room.transform.position + Vector3.up*stage*stageHeight;
		position[sideIndex/2] = side.transform.position[sideIndex/2] + ledgeWidth/2f*(sideIndex%2==0 ? 1 : -1);
		
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



		ledge.ledge = CreateBorderCube(Obj.Colour.WHITE, new Vector3(length = Game.GetFloat(xml, "length"), ledgeThick, ledgeWidth));//GameObject.CreatePrimitive(PrimitiveType.Cube);




		ledge.ledge.transform.localScale = new Vector3(length = Game.GetFloat(xml, "length"), ledgeThick, ledgeWidth);


		ledge.ledge.transform.parent = ledge.transform;


		if( bool.Parse(xml.Attributes["stairs"].Value) )
		{
			ledge.stairs = new GameObject("Stairs");		
			GameObject stairs = CreateBorderCube(Obj.Colour.WHITE, new Vector3(ledgeWidth, ledgeThick, int.Parse(xml.Attributes["stage"].Value) * stageHeight * Mathf.Sqrt(2f)));//GameObject.CreatePrimitive(PrimitiveType.Cube);
			stairs.transform.localScale = new Vector3(ledgeWidth, ledgeThick, int.Parse(xml.Attributes["stage"].Value) * stageHeight * Mathf.Sqrt(2f));
			ledge.stairs.transform.localPosition = new Vector3(-length/2f + (length/100f)*(float)int.Parse(xml.Attributes["stairsPosition"].Value), 0, ledgeWidth/2f);
			stairs.transform.localPosition = ledge.stairs.transform.localPosition + Vector3.forward*(stairs.transform.localScale.z/2f);
			
			stairs.transform.parent = ledge.stairs.transform;
			ledge.stairs.transform.parent = ledge.transform;
			
			ledge.stairs.transform.localEulerAngles = Vector3.right * 45f;

		}

		Join(ledge, Obj.objStack[Obj.objStack.Count-1] as Room, Game.GetInt(xml, "sideIndex"), Game.GetInt(xml, "stage"), Game.GetFloat(xml, "x"));
		return ledge;
	}
}
