using UnityEngine;
using System.Collections;

public class Level_3_ : MonoBehaviour 
{

	InfoTable info, info2;
	Level level;
	Gun gun;

	Trigger trigger;
	Ball black, white;

	bool showMessage = false;
	// Use this for initialization
	void Start () 
	{
		gun = (level = Level.current).gun[0];
		//info = InfoTable.NonXmlCreate("hold the right mouse button to drag the ball", gun.gameObject, 4f, 0.238f, 1.3f);
		//info2 = InfoTable.NonXmlCreate("hold the left mouse button to shoot the ball", gun.gameObject, 4f, 0.238f, 1);

		trigger = level.room[0].trigger[0];
		black = level.ball[1];
		white = level.ball[0];
		Game.DestroyEvent += Destroy;
		//StartCoroutine( ShowMessage(2) );
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gun.inHands)
		{
			if(!showMessage && !Player.InZeroRoom)
				MessageControl();
			//info.Destroy();
			//info2.Destroy();
			//Destroy(this);
		}
	}

	void Destroy()
	{
		Game.DestroyEvent -= Destroy;
		
		//Destroy(transform.GetChild(0).gameObject);
		
		Destroy(this);
		
	}

	IEnumerator ShowMessage(int situation)
	{
		Game.Deadlock();
		showMessage = true;
		yield return new WaitForSeconds(1f);
		float original = 1.35f;
		switch(situation)
		{

			case 0:
			{

			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length); // Message.FASTER
				message.transform.parent = level.room[0].transform;
				message.transform.position = level.room[0].side[0].transform.position + Vector3.forward * level.room[0].Size.z/3.3f + Vector3.right * 0.001f - 
					Vector3.up * level.room[0].Size.y/7f;
				message.transform.localEulerAngles = new Vector3(0, 180, 90);

			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].side[0].transform.position + Vector3.forward * level.room[0].Size.z/3f + Vector3.right * 0.001f - 
				Vector3.up * level.room[0].Size.y/4.8f;
			restart.transform.localEulerAngles = new Vector3(0, 180, 90);
				
		}break;
		case 1:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length); 
			//GameObject message = Word.WriteString(Message.ATTENTION, 0.105f);
			message.transform.parent = level.room[0].transform;
			message.transform.position = level.room[0].side[0].transform.position + Vector3.forward * level.room[0].Size.z/3.3f + Vector3.right * 0.001f - 
				Vector3.up * level.room[0].Size.y/7f;
			message.transform.localEulerAngles = new Vector3(0, 180, 90);


			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].side[0].transform.position + Vector3.forward * level.room[0].Size.z/3f + Vector3.right * 0.001f - 
				Vector3.up * level.room[0].Size.y/4.8f;
			restart.transform.localEulerAngles = new Vector3(0, 180, 90);
		} break;
		case 2:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length); 
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f, Obj.Colour.BLACK);
			message.transform.parent = level.room[0].transform;
			message.transform.position = level.room[0].wall[0].transform.position - Vector3.right * level.room[0].Size.x/6f
				- Vector3.forward * Wall.thick/1.99f - 
				Vector3.up * level.room[0].Size.y/8f;
			message.transform.localEulerAngles = new Vector3(0, -90, 90);


			GameObject restart = Word.WriteString(Message.RESTART, 0.05f, Obj.Colour.BLACK);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].wall[0].transform.position - Vector3.right * level.room[0].Size.x/6.3f -  
				Vector3.forward * Wall.thick/1.99f -  
				Vector3.up * level.room[0].Size.y/4.8f;
			restart.transform.localEulerAngles = new Vector3(0, -90, 90);
		} break;

		default: break;

		}

	}

	void MessageControl()
	{

		//bool isWallDown = false;
		bool isBallIsideTrigger = false;
		bool isBallOutsideTrigger = false;
		
		foreach(GameObject obj in trigger.innerObjs)
		{
			/*if(obj.layer == LayerMask.NameToLayer("Wall"))
			{
				isWallDown = true;
				continue;
			}*/ 
			if(obj.layer == LayerMask.NameToLayer("Ball"))
			{
				if(obj.GetComponent<Ball>().color == Obj.Colour.BLACK)
					isBallIsideTrigger = true;
				continue;
			}
			
		}

		foreach(GameObject obj in level.room[0].trigger[1].innerObjs)
		{
			if(black != null && obj == black.gameObject)
			{
				isBallOutsideTrigger = true;
				break;
			}
		}

		if(trigger.PlayerStay)
		{


			if(level.room[0].wall[0].IsClosed && !isBallIsideTrigger && !black.InHands && isBallOutsideTrigger)
			{
				//Debug.LogError("1");
				showMessage = true;
				StartCoroutine(ShowMessage(0));
				//Invoke("ShowMessage", 1f);
				//ShowMessage();
			}
			else if(level.room[0].wall[0].IsClosed && (isBallIsideTrigger || black.InHands || white.InHands) && !level.room[0].cell[1].IsActive)
			{
				//Debug.LogError("2");
				StartCoroutine(ShowMessage(1));
			}

		}
		else
		{
			if(level.room[0].wall[0].IsClosed && isBallIsideTrigger)
			{
				//Debug.LogError("3");
				StartCoroutine( ShowMessage(2) );
			}
		}
	}
}
