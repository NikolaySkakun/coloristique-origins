using UnityEngine;
using System.Collections;

public class Level_8 : MonoBehaviour 
{
	Level level;
	GameObject plane;

	bool whiteLevel = false;
	bool surrealLevel = false;

	Room tempWhite, tempBlack, downWhite, downBlack, downHelpForBlack, downHelpForWhite;

	Trigger[] zone;

	void Start () 
	{
		level = Level.current;

		downBlack = level.room[8];
		downHelpForBlack = level.room[9];

		downWhite = level.room[10];
		downHelpForWhite = level.room[11];

		tempWhite = level.room[12];
		tempBlack = level.room[13];

		level.room[2].ledge[0].transform.localPosition += Vector3.right * (Ledge.ledgeWidth + Line.height);
		level.room[2].cell[0].transform.localPosition += Vector3.up * level.room[2].Size.y;




		plane = GameObject.CreatePrimitive(PrimitiveType.Quad);
		plane.transform.parent = level.transform;
		plane.transform.position = level.room[2].transform.position - Vector3.up*0.001f;
		(plane.GetComponent<Renderer>().material = Game.BaseMaterial).color = Color.black;
		plane.transform.localEulerAngles = Vector3.right * 90f;
		plane.transform.localScale = Vector3.one * 1000f;

		Player.camera.GetComponent<Camera>().backgroundColor = Color.black;

		level.room[0].side[4].GetComponent<MeshRenderer>().enabled = true;

		zone = level.room[7].trigger;


		for(int i=0; i<3; ++i)
		{
			foreach(Side side in level.room[i].side)
			{
				if(side.GetComponent<BoxCollider>() && side.GetComponent<BoxCollider>().enabled)
				{
					Destroy(side.GetComponent<BoxCollider>());
					side.gameObject.AddComponent<MeshCollider>();
				}
				

			}
		}

		for(int i=3; i<=5; ++i)
		{
			foreach(Side side in level.room[i].side)
			{
				if(side.GetComponent<BoxCollider>())
					Destroy(side.GetComponent<BoxCollider>());

				if(side.GetComponent<Collider>())
					Destroy(side.GetComponent<Collider>());
			}
		}


		level.room[7].transform.position = level.room[6].transform.position - Vector3.up * 0.001f;
		level.room[7].transform.parent = level.room[6].transform;
		downBlack.transform.parent = level.room[6].transform;
		downWhite.transform.parent = level.room[6].transform;

		tempBlack.transform.parent = level.room[6].transform;
		tempWhite.transform.parent = level.room[6].transform;



		//level.room[8].transform.parent = level.room[6].transform;
		level.room[6].transform.parent = level.room[5].transform;


		level.room[3].transform.position = level.room[0].transform.position;
		level.room[4].transform.position = level.room[1].transform.position;
		level.room[5].transform.position = level.room[2].transform.position;


		level.room[9].transform.position = level.room[8].transform.position;
		level.room[9].transform.parent = level.room[8].transform;
		level.room[9].side[5].gameObject.SetActive(false);




		Room room = level.room[9];
		Vector3 tempPosition = room.side[0].transform.position;
		
		room.side[0].transform.position = room.side[1].transform.position;
		room.side[1].transform.position = tempPosition;

		tempPosition = room.side[2].transform.position;
		room.side[2].transform.position = room.side[3].transform.position;
		room.side[3].transform.position = tempPosition;

		room.side[4].transform.localScale = new Vector3(
			room.side[4].transform.localScale.x, 
			room.side[4].transform.localScale.y, 
			-room.side[4].transform.localScale.z);



		level.room[11].transform.position = level.room[10].transform.position;
		level.room[11].transform.parent = level.room[10].transform;
		level.room[11].side[4].gameObject.SetActive(false);


		room = level.room[11];
		tempPosition = room.side[0].transform.position;
		
		room.side[0].transform.position = room.side[1].transform.position;
		room.side[1].transform.position = tempPosition;
		
		tempPosition = room.side[2].transform.position;
		room.side[2].transform.position = room.side[3].transform.position;
		room.side[3].transform.position = tempPosition;
		
		room.side[5].transform.localScale = new Vector3(
			room.side[5].transform.localScale.x, 
			room.side[5].transform.localScale.y, 
			-room.side[5].transform.localScale.z);

		foreach(Side side in downHelpForBlack.side)
		{
			if(side.gameObject.GetComponent<BoxCollider>())
				side.gameObject.GetComponent<BoxCollider>().enabled = false;
			
			if(side.gameObject.GetComponent<MeshCollider>())
				side.gameObject.GetComponent<MeshCollider>().enabled = false;
			
			foreach(Line line in side.line)
			{
				line.gameObject.SetActive(false);
			}
		}



		foreach(Side side in downHelpForWhite.side)
		{
			if(side.gameObject.GetComponent<BoxCollider>())
				side.gameObject.GetComponent<BoxCollider>().enabled = false;
			
			if(side.gameObject.GetComponent<MeshCollider>())
				side.gameObject.GetComponent<MeshCollider>().enabled = false;
			
			foreach(Line line in side.line)
			{
				line.gameObject.SetActive(false);
			}
		}

		TurnInsideOutRooms();

		if(level.room[6].side[5].GetComponent<BoxCollider>())
			level.room[6].side[5].GetComponent<BoxCollider>().enabled = false;
		
		if(level.room[6].side[5].GetComponent<MeshCollider>())
			level.room[6].side[5].GetComponent<MeshCollider>().enabled = false;


		if(level.room[6].side[4].GetComponent<BoxCollider>())
			level.room[6].side[4].GetComponent<BoxCollider>().enabled = false;
		
		if(level.room[6].side[4].GetComponent<MeshCollider>())
			level.room[6].side[4].GetComponent<MeshCollider>().enabled = false;


		Vector3 size = downBlack.Size;
		size.z = size.x;

		GameObject semicircleRoom = CustomObject.SemicircleRoom(size, Obj.Colour.BLACK);
		semicircleRoom.transform.position = downBlack.side[4].transform.position + Vector3.forward*size.x;
		semicircleRoom.transform.parent = downBlack.transform;
		semicircleRoom.transform.localEulerAngles += Vector3.up * 180f;



		foreach(Side side in level.room[7].side)
		{
			foreach(Line line in side.line)
			{
				line.gameObject.SetActive(false);
			}
		}


		foreach(Side side in tempBlack.side)
		{
			foreach(Line line in side.line)
			{
				line.gameObject.SetActive(false);
			}
		}

		foreach(Side side in tempWhite.side)
		{
			foreach(Line line in side.line)
			{
				line.gameObject.SetActive(false);
			}
		}

		foreach(Line line in level.room[6].side[5].line)
		{
			line.gameObject.AddComponent<BoxCollider>();
		}


		for(int i=0; i < level.room[6].side.Length; ++i)
		{
			if(i == 5)
			{
				level.room[6].side[i].gameObject.GetComponent<MeshRenderer>().enabled = false;
				continue;
			}

			level.room[6].side[i].gameObject.SetActive(false);
		}

		level.room[6].transform.parent = level.transform;

		originalPositionOfOutletDoor = level.outletDoor.transform.position = downWhite.side[5].transform.position - Vector3.up*Door.sizeTemplate.y/2f - Vector3.forward*Door.sizeTemplate.x;
		level.outletDoor.transform.position -= Vector3.up*10f;
		//level.outletDoor.transform.parent = downWhite.transform;
		downWhite.side[5].gameObject.SetActive(false);

		foreach(Line line in level.room[6].side[5].line)
		{
			line.gameObject.SetActive(true);
		}


		/*GameObject message = Word.WriteString("krasavchik", 0.1f);
		message.transform.parent = level.outletDoor.transform;

		message.transform.localPosition = Vector3.up*Door.sizeTemplate.y/2f + Vector3.forward - Vector3.right;
		message.transform.localEulerAngles = new Vector3(0, -90, 90);*/

		Level.ZeroRoom.transform.parent.gameObject.SetActive(false);
		//level.inletDoor.gameObject.SetActive(false);

		Invoke("PostDrawing", Game.drawTime * 1.5f);

	}

	Vector3 originalPositionOfOutletDoor;

	void PostDrawing()
	{
		downBlack.gameObject.SetActive(false);
		downWhite.gameObject.SetActive(false);

		tempBlack.gameObject.SetActive(false);
		tempWhite.gameObject.SetActive(false);

		level.ball[1].gameObject.SetActive(false);
	}

	void OnEscape()
	{
		level.inletDoor.gameObject.SetActive(true);
		//level.inletDoor.door.SetActive(true);
		for(int i=3; i<=5; ++i)
		{
			level.room[i].gameObject.SetActive(false);
		}

		Level.ZeroRoom.transform.parent.gameObject.SetActive(true);
		Player.camera.GetComponent<Camera>().backgroundColor = Color.white;

		CancelInvoke();

		this.enabled = false;
	}
	
	void TurnInsideOutRooms()
	{
		for(int i=3; i<=6; ++i)
		{
			Room room = level.room[i];
			Vector3 tempPosition = room.side[0].transform.position;

			room.side[0].transform.position = room.side[1].transform.position;
			room.side[1].transform.position = tempPosition;

			if(i == 5 || i == 3)
			{

				room.side[2].transform.localScale = new Vector3(
					room.side[2].transform.localScale.x, 
					room.side[2].transform.localScale.y, 
					-room.side[2].transform.localScale.z);
				
				room.side[3].transform.localScale = new Vector3(
					room.side[3].transform.localScale.x, 
					room.side[3].transform.localScale.y, 
					-room.side[3].transform.localScale.z);

			}
			else
			{
				tempPosition = room.side[2].transform.position;
				room.side[2].transform.position = room.side[3].transform.position;
				room.side[3].transform.position = tempPosition;

				if(i == 4)
				{
					room.side[3].gameObject.SetActive(false);
					room.side[2].gameObject.SetActive(false);
				}
			}

			if(i == 2 || i == 5)
			{
				room.side[4].transform.localScale = new Vector3(
					room.side[4].transform.localScale.x, 
					room.side[4].transform.localScale.y, 
					-room.side[4].transform.localScale.z);

				room.side[5].transform.localScale = new Vector3(
					room.side[5].transform.localScale.x, 
					room.side[5].transform.localScale.y, 
					-room.side[5].transform.localScale.z);

			}
			else
			{
				tempPosition = room.side[4].transform.position;
				room.side[4].transform.position = room.side[5].transform.position;
				room.side[5].transform.position = tempPosition;
			}
		}
	}

	void SetLevelVisible(bool visible)
	{
		whiteLevel = !visible;
		
		for(int i=0; i<3; ++i)
		{
			foreach(Side side in level.room[i].side)
			{
				foreach(Line line in side.line)
				{
					line.Paint(visible ? Obj.Colour.BLACK : Obj.Colour.WHITE);
				}

				foreach(Line line in side.transform.GetComponentsInChildren<Line>())
				{
					line.Paint(visible ? Obj.Colour.BLACK : Obj.Colour.WHITE);
				}
			}
			
		}

		level.lift[0].gameObject.SetActive(visible);
	}

	void DoorVisibleControl()
	{
		if(!level.room[0].trigger[0].PlayerStay && level.inletDoor.gameObject.activeSelf)
		{
			level.inletDoor.gameObject.SetActive(false);
		}
		else if(level.room[0].trigger[0].PlayerStay && !level.inletDoor.gameObject.activeSelf)
		{
			level.inletDoor.gameObject.SetActive(true);
		}
	}

	void NormalVisibleControl()
	{
		if(level.room[2].cell[0].IsActive && !whiteLevel)
		{
			SetLevelVisible(false);
		}
		else if(!level.room[2].cell[0].IsActive && whiteLevel)
		{
			SetLevelVisible(true);
		}
	}

	void ConversionLevelControl()
	{
		if(whiteLevel && level.room[2].trigger[0].PlayerStay && !level.room[7].trigger[0].PlayerStay)
		{
			level.room[7].gameObject.SetActive(true);

			for(int i=0; i<=5; ++i)
			{
				level.room[i].gameObject.SetActive(false);
			}

			plane.SetActive(false);

			level.room[6].side[5].gameObject.SetActive(true);
			level.room[6].side[5].gameObject.GetComponent<MeshRenderer>().enabled = true;
			level.room[6].side[5].Paint(Obj.Colour.BLACK);


			level.room[6].side[4].gameObject.SetActive(true);
			level.room[6].side[4].gameObject.GetComponent<MeshRenderer>().enabled = true;
			level.room[6].side[4].Paint(Obj.Colour.BLACK);

			level.room[7].Paint(Obj.Colour.WHITE);

			surrealLevel = true;
		}
	}

	void SurrealControl()
	{

		if(level.room[7].color == Obj.Colour.WHITE) // +_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_WHITE_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
		{
			if(zone[1].PlayerStay && zone[2].PlayerStay && !downBlack.trigger[0].PlayerStay)
			{
				if(!tempBlack.gameObject.activeSelf)
				{
					tempBlack.gameObject.SetActive(true);
					level.room[6].side[4].gameObject.SetActive(false);
					level.room[6].side[5].gameObject.SetActive(false);
				}
				
				return;
			}

			if(!zone[1].PlayerStay && zone[2].PlayerStay && tempBlack.gameObject.activeSelf)
			{
				level.room[7].Paint(Obj.Colour.BLACK);
				tempBlack.gameObject.SetActive(false);
				downBlack.gameObject.SetActive(false);

				level.room[6].side[4].gameObject.SetActive(true);
				level.room[6].side[5].gameObject.SetActive(true);

				level.room[6].side[4].Paint(Obj.Colour.WHITE);
				level.room[6].side[5].Paint(Obj.Colour.WHITE);

				return;
			}

			if(zone[0].PlayerStay && !zone[1].PlayerStay)
			{
				downBlack.gameObject.SetActive(true);
				level.room[6].side[4].gameObject.SetActive(false);
				level.room[6].side[5].gameObject.SetActive(false);
			}

			if(zone[1].PlayerStay && !zone[0].PlayerStay && !downBlack.trigger[0].PlayerStay)
			{
				downBlack.gameObject.SetActive(false);
				level.room[6].side[4].gameObject.SetActive(true);
				level.room[6].side[5].gameObject.SetActive(true);
			}
		}
		else if(level.room[7].color == Obj.Colour.BLACK) // +_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_BLACK_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+_+
		{
			if(zone[0].PlayerStay && zone[3].PlayerStay && !downWhite.trigger[0].PlayerStay)
			{
				if(!tempWhite.gameObject.activeSelf)
				{
					tempWhite.gameObject.SetActive(true);
					level.room[6].side[4].gameObject.SetActive(false);
					level.room[6].side[5].gameObject.SetActive(false);
				}
				
				return;
			}

			if(!zone[0].PlayerStay && zone[3].PlayerStay && tempWhite.gameObject.activeSelf)
			{
				level.room[7].Paint(Obj.Colour.WHITE);
				tempWhite.gameObject.SetActive(false);
				downWhite.gameObject.SetActive(false);
				
				level.room[6].side[4].gameObject.SetActive(true);
				level.room[6].side[5].gameObject.SetActive(true);
				
				level.room[6].side[4].Paint(Obj.Colour.BLACK);
				level.room[6].side[5].Paint(Obj.Colour.BLACK);

				return;
			}

			if(zone[1].PlayerStay && !zone[0].PlayerStay)
			{
				if(!level.ball[1].gameObject.activeSelf)
					level.ball[1].gameObject.SetActive(true);

				downWhite.gameObject.SetActive(true);
				level.outletDoor.transform.position = originalPositionOfOutletDoor;
				level.room[6].side[4].gameObject.SetActive(false);
				level.room[6].side[5].gameObject.SetActive(false);
			}
			
			if(zone[0].PlayerStay && !zone[1].PlayerStay && !downWhite.trigger[0].PlayerStay)
			{
				downWhite.gameObject.SetActive(false);
				level.outletDoor.transform.position -= Vector3.up*10f;
				level.room[6].side[4].gameObject.SetActive(true);
				level.room[6].side[5].gameObject.SetActive(true);
			}
		}
	}

	void LevelControl()
	{
		if(!surrealLevel)
		{
			DoorVisibleControl();
			NormalVisibleControl();
			ConversionLevelControl();
		}
		else
		{
			SurrealControl();
		}
	}

	void Update () 
	{
		if( Game.IsInputEscape() || Game.IsInputRestart() )
		{
			OnEscape();
		}

		LevelControl();
	}
}
