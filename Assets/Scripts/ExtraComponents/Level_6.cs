using UnityEngine;
using System.Collections;

public class Level_6 : MonoBehaviour 
{
	Level level;

	void Start() 
	{
		level = Level.current;

		Vector2 size = new Vector2(level.room[2].Size[0], level.room[2].Size[1]*Mathf.Sqrt(2f));
		
		Side side = Side.Create(Obj.Colour.WHITE, size);
		side.transform.localEulerAngles = Vector3.right*45.5f;
		
		side.transform.localScale = new Vector3(size.x, size.y, 1);
		
		GameObject hill = new GameObject("Hill");
		
		side.transform.parent = hill.transform;
		//side.transform.localPosition = Vector3.up * (size.x/2f + Line.height/2f + 0.05f);
		
		hill.transform.position = level.room[2].side[5].transform.position - Vector3.forward*level.room[2].Size[1]/2f + Vector3.up * 0.0015f;
		//hill.transform.localEulerAngles = Vector3.up * (-90);
		
		hill.transform.parent = level.transform;

		level.outletDoor.trigger.gameObject.SetActive(false);
		//level.outletDoor.destroyPreviousLevelTrigger.gameObject.SetActive(false);

		foreach(Renderer r in level.outletDoor.destroyPreviousLevelTrigger.transform.GetComponentsInChildren<Renderer>(true))
		{
			if(!r.enabled)
				r.gameObject.name = "NoRenderer";
			r.enabled = false;
		}

		//string msg = MessageController.GetMessage(level.Index, situation);
		GameObject message = Word.WriteString("nicely done. but remember:", 0.1f);
		message.transform.parent = level.room[0].transform;
		message.transform.position = level.outletDoor.transform.position + Vector3.forward * 5f + Vector3.up*Door.sizeTemplate.y*0.9f
			- Vector3.right * Door.sizeTemplate.x/1.3f;
		message.transform.localEulerAngles = new Vector3(0, -90, 90);


		message = Word.WriteString("here", 0.12f);
		message.transform.parent = level.room[0].transform;
		message.transform.position = level.outletDoor.transform.position + Vector3.forward * 5f + Vector3.up*Door.sizeTemplate.y*0.7f
			- Vector3.right * Door.sizeTemplate.x/8f;
		message.transform.localEulerAngles = new Vector3(0, -90, 90);


		message = Word.WriteString("thinking outside the box", 0.1f);
		message.transform.parent = level.room[0].transform;
		message.transform.position = level.outletDoor.transform.position + Vector3.forward * 5f + Vector3.up*Door.sizeTemplate.y*0.5f
			- Vector3.right * Door.sizeTemplate.x/1.5f;
		message.transform.localEulerAngles = new Vector3(0, -90, 90);

		message = Word.WriteString("for you is the way to be born", 0.1f);
		message.transform.parent = level.room[0].transform;
		message.transform.position = level.outletDoor.transform.position + Vector3.forward * 5f + Vector3.up*Door.sizeTemplate.y*0.3f
			- Vector3.right * Door.sizeTemplate.x/1.08f;
		message.transform.localEulerAngles = new Vector3(0, -90, 90);


		
		
		GameObject restart = Word.WriteString("press esc", 0.05f);
		restart.transform.parent = level.room[0].transform;
		restart.transform.position = level.outletDoor.transform.position + Vector3.forward * 3f - Vector3.up * 2f + Vector3.up*Door.sizeTemplate.y*0.9f
			- Vector3.right * Door.sizeTemplate.x/8f;
			//level.room[0].side[4].transform.position /*- Vector3.right * level.room[2].Size.x/6.1f*/ - Vector3.up * level.room[2].Size.y/4.5f + Vector3.forward * 0.001f;
		restart.transform.localEulerAngles = new Vector3(0, -90, 90);


		/*
		GameObject text = Word.WriteString("procedurally generated environment");
		text.transform.localEulerAngles = new Vector3(0, 180, 90);
		text.transform.localScale = Vector3.one * 0.3f;
		text.transform.position = level.room[0].side[0].transform.position;
		text.transform.position += Vector3.right*0.001f - Vector3.up*1.25f - Vector3.forward*5.6f;
		text.transform.parent = level.transform;
*/
	}

	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.R))
		{
			level.outletDoor.trigger.gameObject.SetActive(true);
			//level.outletDoor.destroyPreviousLevelTrigger.gameObject.SetActive(true);
			foreach(Renderer r in level.outletDoor.destroyPreviousLevelTrigger.transform.GetComponentsInChildren<Renderer>(true))
			{
				if(r.gameObject.name != "NoRenderer")
					r.enabled = true;
			}
		}
	}
}
