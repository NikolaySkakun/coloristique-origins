using UnityEngine;
using System.Collections;

public class Level_18 : MonoBehaviour 
{
	Level level;
	GameObject pointer, surCube;
	Cell cell;
	Room whiteRoom, blackRoom;
	//GameObject[] wall;
	bool whiteWorld = true;
	Room portal;
	Ball ball;


	void Awake() 
	{ 
		level = Level.current;

		pointer = CustomObject.Pointer();
		surCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

		surCube.transform.localScale = Door.sizeTemplate - Vector3.forward*Door.sizeTemplate.z*0.99f;
		surCube.transform.position = level.door[1].room.side[4].transform.position;
		surCube.transform.parent = level.transform;
		surCube.transform.localEulerAngles += Vector3.up * 180;
		surCube.GetComponent<Renderer>().material = Game.BaseMaterial;//.color = Color.black;
		surCube.GetComponent<Renderer>().material.color = Color.black;
		Destroy(surCube.GetComponent<Collider>());
		surCube.SetActive(false);

		pointer.transform.position = level.lift[0].transform.position;

		pointer.transform.parent = level.transform;
		pointer.transform.localEulerAngles = Vector3.up * 270f;
		pointer.transform.localScale *= 0.8f;
	
		//Player.camera.camera.backgroundColor = Color.black;


		level.door[1].door.gameObject.SetActive(false);
		//level.door[1].trigger.gameObject.SetActive(false);
		//level.door[1].destroyPreviousLevelTrigger.gameObject.SetActive(false);


		//level.door[2].gameObject.SetActive(false);

		level.door[2].transform.parent = level.room[5].transform;

		level.door[3].DestroyAnyway();
		level.door[4].DestroyAnyway();

		level.room[5].transform.position = level.room[4].transform.position = level.room[2].transform.position;

		level.room[5].transform.position += Vector3.forward * 10f;
		level.room[5].side[4].gameObject.SetActive(false);

		level.room[4].side[5].gameObject.SetActive(false);
		level.room[4].side[4].gameObject.SetActive(false); //!!!!!!!!!-



		/*
		GameObject text = Word.WriteString("only black and white");
		text.transform.localEulerAngles = new Vector3(0, 270, 90);
		text.transform.localScale = Vector3.one * 0.3f;
		text.transform.position = level.room[0].side[5].transform.position;
		text.transform.position += -Vector3.forward*0.001f - Vector3.right*3.4f;// - Vector3.up*1.05f - Vector3.right*3f;
		text.transform.parent = level.transform;
		
		
		GameObject text2 = Word.WriteString("surreal levels", 0.5f, Obj.Colour.WHITE);
		text2.transform.localEulerAngles = new Vector3(0, 180, 90);
		text2.transform.localScale = Vector3.one * 0.3f;
		text2.transform.position = level.room[4].side[0].transform.position;
		text2.transform.position += Vector3.right*0.001f - Vector3.forward*2f;// - Vector3.up*1.05f - Vector3.right*3f;
		text2.transform.parent = level.transform;
*/
		Level.ZeroRoom.transform.parent.gameObject.SetActive(false);

		Vector3 size = level.room[4].Size;
		size.z = size.x;
		GameObject semicircleRoom = CustomObject.SemicircleRoom(size, Obj.Colour.BLACK);
		semicircleRoom.transform.position = level.room[4].side[4].transform.position;
		semicircleRoom.transform.parent = level.room[4].transform;
		semicircleRoom.transform.localEulerAngles += Vector3.up * 180f;

		foreach(Side s in level.door[1].room.side)
			if(s.index == 4)
			{
				s.transform.localScale = new Vector3(1, 1, -1);
			}	
			else
				s.gameObject.SetActive(false);

		level.room[3].transform.position = level.room[2].transform.position - Vector3.up*0.001f;
		(cell = level.room[2].cell[0]).transform.localPosition += Vector3.up * Ledge.stageHeight;

		foreach(Side s in level.room[3].side)
			foreach(Line l in s.line)
				l.gameObject.SetActive(false);

		level.room[2].ledge[0].transform.localPosition += Vector3.right * (Ledge.ledgeWidth + Line.height);

		for(int i=0; i<3; ++i) // < 3
		{
			foreach(Side s in level.room[i].side)
			{
				//Debug.LogError(1);
				//Debug.LogWarning(s.line[0].transform.localScale);

				if(i > 0)
				{
					if(s.GetComponent<BoxCollider>() && s.GetComponent<BoxCollider>().enabled)
					s.gameObject.AddComponent<MeshCollider>();

					if(s.GetComponent<BoxCollider>())
				s.GetComponent<BoxCollider>().enabled = false;
				}
				GameObject side = (GameObject)Instantiate(s.gameObject);
				Destroy(side.GetComponent<Collider>());
				if(side.GetComponent<BoxCollider>() != null)
					Destroy(side.GetComponent<BoxCollider>());
				//side.GetComponent<Renderer>().enabled = false;
				side.transform.position = s.transform.position;
				side.transform.parent = s.transform;

				side.transform.localScale = Vector3.one;

				if(s.index == 2)
					side.transform.localPosition += Vector3.forward * 0.0001f;

				if(i>1 && i<4)
					side.transform.localScale = new Vector3(1, -1, 1);

				side.transform.localEulerAngles += ((i>1 && i<4) ? Vector3.right : Vector3.up) * 180f;
			}
		}

		for(int u=4; u<6; ++u)
		{
		GameObject[] wall = new GameObject[] { 
			(GameObject)Instantiate(level.room[u].side[0].gameObject),
			(GameObject)Instantiate(level.room[u].side[1].gameObject) };

			Destroy(wall[0].GetComponent<Collider>());
			Destroy(wall[1].GetComponent<Collider>());
		
		for(int i=0; i<wall[0].transform.childCount; ++i)
		{
			wall[0].transform.GetChild(i).gameObject.SetActive(false);
			wall[1].transform.GetChild(i).gameObject.SetActive(false);
		}
		
		for(int i=0; i<wall.Length; ++i)
		{
			wall[i].transform.parent = level.room[u].side[i].transform;
			
			wall[i].transform.localScale = Vector3.one + Vector3.up*10 + Vector3.right;
			wall[i].transform.position = level.room[u].side[i].transform.position;
			wall[i].transform.localPosition += Vector3.right*0.5f*(i==0 ? -1 : 1)*(u==4 ? 1 : -1);
			wall[i].transform.localEulerAngles += Vector3.up * 180f;
			wall[i].GetComponent<Renderer>().material.color = Game.ReverseColor(Game.GetColor(level.room[u].color));
		}
		}


		level.room[3].transform.position += Vector3.forward * (level.door[1].room.side[4].transform.position.z - level.room[3].transform.position.z);

		(portal = level.room[6]).transform.position = level.room[3].transform.position + (Vector3.forward*level.room[6].Size.z)/2f + Vector3.up*0.001f;
		
		
		foreach(Side s in level.room[6].side)
			foreach(Line l in s.line)
				l.gameObject.SetActive(false);
		
		
		//portal.transform.localScale -= Vector3.forward*2;


		GameObject[] w = new GameObject[] { 
			(GameObject)Instantiate(level.room[6].side[0].gameObject),
			(GameObject)Instantiate(level.room[6].side[1].gameObject) };
		
		Destroy(w[0].GetComponent<Collider>());
		Destroy(w[1].GetComponent<Collider>());
		
		for(int i=0; i<w[0].transform.childCount; ++i)
		{
			w[0].transform.GetChild(i).gameObject.SetActive(false);
			w[1].transform.GetChild(i).gameObject.SetActive(false);
		}
		
		for(int i=0; i<w.Length; ++i)
		{
			w[i].transform.parent = level.room[6].side[i].transform;
			
			w[i].transform.localScale = Vector3.one + Vector3.up*10 + Vector3.right;
			w[i].transform.position = level.room[6].side[i].transform.position;
			w[i].transform.localPosition += Vector3.right*0.5f*(i==0 ? -1 : 1)*(-1);
			w[i].transform.localEulerAngles += Vector3.up * 180f;
			w[i].GetComponent<Renderer>().material.color = Game.ReverseColor(Game.GetColor(level.room[6].color));
		}

		foreach(Side s in level.room[1].side)
		{
			if(s.gameObject.GetComponent<Collider>().enabled)
			{
			Destroy(s.gameObject.GetComponent<Collider>());
			//s.gameObject.AddComponent<BoxCollider>();
			}
		}


		foreach(Side s in level.room[2].side)
		{
			if(s.gameObject.GetComponent<Collider>().enabled)
			{
			Destroy(s.gameObject.GetComponent<Collider>());
			//s.gameObject.AddComponent<BoxCollider>();
			}
		}


		ball = level.ball[1];
		ball.transform.position = level.room[5].transform.position + Vector3.up*0.5f;
		ball.transform.parent = level.room[5].transform;
		ball.gameObject.SetActive(false);

		blackRoom = level.room[4];
		whiteRoom = level.room[5];
		
		portal.gameObject.SetActive(false);
		t = Time.time;

		StartCoroutine(Wait());


	}

	float t = 0;

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(Game.drawTime);
		blackRoom.gameObject.SetActive(false);
		whiteRoom.gameObject.SetActive(false);


		level.inletDoor.door.gameObject.SetActive(false);
		//ball.transform.position = whiteRoom.transform.position;
		//ball.gameObject.SetActive(true);
	}


	bool enabledSur = false, nextSur = false;
	

	void Sur(bool enable)
	{
		if(enable == enabledSur)
			return;

		enabledSur = enable;

		if( enable )
		{
			pointer.SetActive(false);
			level.lift[0].gameObject.SetActive(false);
			level.room[1].side[3].gameObject.SetActive(false);

			for(int i=0; i<3; ++i)
			{
				foreach(Side s in level.room[i].side)
				{
					foreach(Line l in s.line)
					{
						if(l.hasClone)
							l.clone.gameObject.SetActive(false);
						l.gameObject.SetActive(false);
					}

					/*Side cloneSide = s.transform.GetComponentInChildren<Side>();

					if(cloneSide != null)
					{
						foreach(Line ln in cloneSide.line)
						{
							if(ln.hasClone)
								ln.clone.gameObject.SetActive(false);
							ln.gameObject.SetActive(false);
						}
					}*/
				}
			}
		}
		else
		{
			pointer.SetActive(true);
			level.lift[0].gameObject.SetActive(true);
			level.room[1].side[3].gameObject.SetActive(true);
			level.room[1].side[2].gameObject.SetActive(false);
			for(int i=0; i<3; ++i)
			{
				foreach(Side s in level.room[i].side)
				{
					foreach(Line l in s.line)
					{
						if(l.hasClone)
							l.clone.gameObject.SetActive(true);
						l.gameObject.SetActive(true);
					}
				}
			}
		}
	}

	void LateUpdate()
	{
			bool inRoom = false;
			foreach(GameObject obj in blackRoom.trigger[0].innerObjs)
			{
				if(obj == ball.gameObject)
				{
					ball.transform.parent = blackRoom.transform;
					inRoom = true;
					break;
				}
			}

			foreach(GameObject obj in whiteRoom.trigger[0].innerObjs)
			{
				if(obj == ball.gameObject)
				{
					ball.transform.parent = whiteRoom.transform;
					inRoom = true;
					break;
				}
			}

			if(!inRoom)
			{
			if(ball != null)
				ball.transform.parent = level.transform;
			}
	}

	void Update () 
	{
		if(Input.GetKey(KeyCode.Escape) || Input.GetKeyUp(KeyCode.R))
		{
			//if(!Level.ZeroRoom.transform.parent.gameObject.activeSelf)
			//	Level.ZeroRoom.transform.parent.gameObject.SetActive(true);

			for(int i=0; i<3; ++i)
				level.door[i].gameObject.SetActive(true);

			level.inletDoor.door.gameObject.SetActive(true);
		}


		if(nextSur)
		{
			if(whiteWorld)
			{
				if(portal.trigger[0].PlayerStay)
				{
					if(level.room[3].trigger[0].gameObject.activeSelf)
						level.room[3].trigger[0].gameObject.SetActive(false);
					
					if(surCube.activeSelf)
						surCube.SetActive(false);
				}
				else
				{
					if(!level.room[3].trigger[0].gameObject.activeSelf)
						level.room[3].trigger[0].gameObject.SetActive(true);
					if(!surCube.activeSelf && !level.room[3].trigger[0].PlayerStay && !blackRoom.trigger[0].PlayerStay)
						surCube.SetActive(true);
				}
				
				if(portal.trigger[0].PlayerStay && !level.room[3].trigger[1].PlayerStay)
				{
					
					
					whiteWorld = false;
					
					surCube.GetComponent<Renderer>().material.color = Color.white;
					surCube.SetActive(true);
					
					level.room[3].Repaint();
					
					portal.transform.localScale = new Vector3(1, 1, -1);
					portal.transform.position = level.room[3].transform.position - (Vector3.forward*level.room[6].Size.z)/2f + Vector3.up*0.001f;
					portal.Repaint();
					portal.gameObject.SetActive(false);
					return;
				}
				
				if(level.room[3].trigger[0].PlayerStay && !blackRoom.gameObject.activeSelf && !portal.trigger[0].PlayerStay)
				{
					if(portal.gameObject.activeSelf)
						portal.gameObject.SetActive(false);
					
					if(!level.room[3].trigger[1].PlayerStay)
					{
						blackRoom.gameObject.SetActive(true);
						surCube.SetActive(false);
					}
				}
				else if(level.room[3].trigger[1].PlayerStay && !blackRoom.trigger[0].PlayerStay)
				{
					if(blackRoom.gameObject.activeSelf)
					{
						blackRoom.gameObject.SetActive(false);
						surCube.SetActive(true);
					}
					if(!level.room[3].trigger[0].PlayerStay && !portal.gameObject.activeSelf)
						portal.gameObject.SetActive(true);
					
				}
			}
			else
			{
				if(portal.trigger[0].PlayerStay)
				{
					if(level.room[3].trigger[1].gameObject.activeSelf)
						level.room[3].trigger[1].gameObject.SetActive(false);
					
					if(surCube.activeSelf)
						surCube.SetActive(false);
				}
				else
				{
					if(!level.room[3].trigger[1].gameObject.activeSelf)
						level.room[3].trigger[1].gameObject.SetActive(true);
					if(!surCube.activeSelf && !level.room[3].trigger[1].PlayerStay && !whiteRoom.trigger[0].PlayerStay)
						surCube.SetActive(true);
				}
				
				if(portal.trigger[0].PlayerStay && !level.room[3].trigger[0].PlayerStay)
				{
					
					
					whiteWorld = true;
					
					surCube.GetComponent<Renderer>().material.color = Color.white;
					surCube.SetActive(true);
					
					level.room[3].Repaint();
					
					
					
					portal.gameObject.SetActive(false);
					return;
				}
				
				if(level.room[3].trigger[1].PlayerStay && !whiteRoom.gameObject.activeSelf && !portal.trigger[0].PlayerStay)
				{
					if(portal.gameObject.activeSelf)
						portal.gameObject.SetActive(false);
					
					if(!level.room[3].trigger[0].PlayerStay)
					{
						whiteRoom.gameObject.SetActive(true);
						surCube.SetActive(false);
					}
				}
				else if(level.room[3].trigger[0].PlayerStay && !whiteRoom.trigger[0].PlayerStay)
				{
					if(whiteRoom.gameObject.activeSelf)
					{
						whiteRoom.gameObject.SetActive(false);
						surCube.SetActive(true);
					}
					if(!level.room[3].trigger[1].PlayerStay && !portal.gameObject.activeSelf)
						portal.gameObject.SetActive(true);
					
				}
			}



			return;
		}

		if(cell.IsActive)
		{
			if(!enabledSur)
				Sur(true);
			else if( !nextSur && level.room[2].trigger[0].PlayerStay )
			{
				nextSur = true;

				surCube.SetActive(true);
				level.room[3].Repaint();
				for(int i=0; i<3; ++i)
					level.room[i].gameObject.SetActive(false);

				for(int i=0; i<2; ++i)
					level.door[i].gameObject.SetActive(false);
			}
		}
		else
		{
			if(enabledSur)
				Sur(false);
		}
	}
}
