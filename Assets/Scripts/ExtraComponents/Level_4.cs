using UnityEngine;
using System.Collections;

public class Level_4 : MonoBehaviour 
{
	bool showMessage = false;
	Level level;

	void Start() 
	{
		level = Level.current;

		/*foreach(Ball ball in level.ball)
		{
			ball.Repaint();
			ball.Repaint();
		}*/
	}

	void Destroy()
	{
		Game.DestroyEvent -= Destroy;
		Destroy(this);
		
	}

	public IEnumerator ShowMessage(int situation)
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
			GameObject message = Word.WriteString(msg, original/(float)msg.Length);
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f);
			message.transform.parent = level.room[2].transform;
			message.transform.position = level.room[2].side[4].transform.position /*- Vector3.right * level.room[2].Size.x/6f
				- Vector3.up * level.room[2].Size.y/8f*/ + Vector3.forward * 0.001f;
			message.transform.localEulerAngles = new Vector3(0, 90, 90);
			
			
			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[2].transform;
			restart.transform.position = level.room[2].side[4].transform.position /*- Vector3.right * level.room[2].Size.x/6.1f*/ - Vector3.up * level.room[2].Size.y/5f + Vector3.forward * 0.001f;
			restart.transform.localEulerAngles = new Vector3(0, 90, 90);
			//Debug.LogError("SITUATION ZERO");
		} break;

			
		default: break;
		}
	}

	bool wait = false;

	public void MessageControl()
	{
		if(level.room[2].trigger[0].PlayerStay)
		{
			bool ballsInRoom = false;
			foreach(GameObject obj in level.room[2].trigger[0].innerObjs)
			{
				if(obj.GetComponent<Ball>() != null)
				{
					ballsInRoom = true;
					break;
				}
			}

			if(!level.lift[0].trigger.PlayerStay && level.lift[0].cell.IsActive && level.lift[0].isImmobile && level.lift[0].inTop)
			{
				Debug.LogWarning("1");
				StartCoroutine( ShowMessage(0) );
				return;
			}



			if(!ballsInRoom && !wait)
			{
				Debug.LogWarning("2");
				wait = true;
				Invoke("SecondCheck", 1f);
				//StartCoroutine( ShowMessage(0) );
			}
		}

	}

	void SecondCheck()
	{
		bool ballsInRoom = false;
		foreach(GameObject obj in level.room[2].trigger[0].innerObjs)
		{
			if(obj.GetComponent<Ball>() != null)
			{
				ballsInRoom = true;
				break;
			}
		}
		
		if(!ballsInRoom)
		{

			Debug.LogWarning("3");
			StartCoroutine( ShowMessage(0) );
			wait = false;
		}

	}

	void Update () 
	{
		
		if(!showMessage)
			MessageControl();
	}

	/*InfoTable info;//, info2;
	Level level;
	Gun gun;
	
	// Use this for initialization
	void Start () 
	{
		gun = (level = Level.current).gun[0];
		info = InfoTable.NonXmlCreate("click any mouse button to invert the ball color", gun.gameObject, 4f, 0.238f, 1.1f);
		//info2 = InfoTable.NonXmlCreate("hold the left mouse button to shoot the ball", gun.gameObject, 4f, 0.238f, 1);
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(gun.inHands)
		{
			info.Destroy();
			//info2.Destroy();
			Destroy(this);
		}
	}*/
}
