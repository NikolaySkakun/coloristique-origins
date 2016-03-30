using UnityEngine;
using System.Collections;

public class Level_4 : LevelBahaviour
{
	Level level;
	Cell[] cell;

	Ball white, black;
	bool showMessage = false;
	Lift lift;

	void Start() 
	{
		level = Level.current;
		//level.room[6].DestroyAnyway();
		cell = level.room[2].cell;
		Vector2 size = new Vector2(level.room[3].Size[0], level.room[5].Size[0]*Mathf.Sqrt(2f));

		Side side = Side.Create(Obj.Colour.WHITE, size);
		side.transform.localEulerAngles = Vector3.right*45f;

		side.transform.localScale = new Vector3(size.x, size.y, 1);

		GameObject hill = new GameObject("Hill");

		side.transform.parent = hill.transform;
		side.transform.localPosition = Vector3.up * (size.x/2f + Line.height/2f + 0.05f);

		hill.transform.position = level.room[5].side[2].transform.position;
		hill.transform.localEulerAngles = Vector3.up * (-90);

		hill.transform.parent = level.transform;

		level.room[6].side[2].line[1].gameObject.SetActive(true);

		white = level.ball[0];
		black = level.ball[1];
		lift = level.lift[0];

//		Vector3 boxSize = level.room[4].side[1].GetComponent<BoxCollider>().size;
//		boxSize.z = 0.05f;
//		level.room[4].side[1].GetComponent<BoxCollider>().size = boxSize;
//		Vector3 boxCenter = level.room[4].side[1].GetComponent<BoxCollider>().center;
//		boxCenter.z = 0.025f;
//		level.room[4].side[1].GetComponent<BoxCollider>().center = boxCenter;


		Game.DestroyEvent += Destroy;
		//Player.player.AddComponent<TimeMachine>();
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
		float original = 1.5f;
		switch(situation)
		{
		case 0:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length);
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f);
			message.transform.parent = level.room[2].transform;
			message.transform.position = level.room[2].side[4].transform.position //- Vector3.right * level.room[2].Size.x/6f
				- Vector3.up * level.room[2].Size.y/8f + Vector3.forward * 0.001f;
			message.transform.localEulerAngles = new Vector3(0, 90, 90);
			
			
			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[2].transform;
			restart.transform.position = level.room[2].side[4].transform.position /*- Vector3.right * level.room[2].Size.x/6.1f*/ - Vector3.up * level.room[2].Size.y/4.5f + Vector3.forward * 0.001f;
			restart.transform.localEulerAngles = new Vector3(0, 90, 90);
			//Debug.LogError("SITUATION ZERO");
		} break;
		case 1:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length);
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f);
			message.transform.parent = level.room[0].transform;
			message.transform.position = level.room[0].side[4].transform.position //- Vector3.right * level.room[0].Size.x/6f
				- Vector3.up * level.room[0].Size.y/8f + Vector3.forward * 0.001f;
			message.transform.localEulerAngles = new Vector3(0, 90, 90);
			
			
			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].side[4].transform.position /*- Vector3.right * level.room[0].Size.x/6.1f*/ - Vector3.up * level.room[0].Size.y/4.5f + Vector3.forward * 0.001f;
			restart.transform.localEulerAngles = new Vector3(0, 90, 90);
			//Debug.LogError("SITUATION ONE");
		} break;

		case 2:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length);
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f);
			message.transform.parent = level.room[0].transform;
			message.transform.position = level.room[0].side[4].transform.position //- Vector3.right * level.room[0].Size.x/6f
				- Vector3.up * level.room[0].Size.y/8f + Vector3.forward * 0.001f;
			message.transform.localEulerAngles = new Vector3(0, 90, 90);
			
			
			GameObject restart = Word.WriteString(Message.RESTART, 0.05f);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].side[4].transform.position /*- Vector3.right * level.room[0].Size.x/6.1f*/ - Vector3.up * level.room[0].Size.y/4.5f + Vector3.forward * 0.001f;
			restart.transform.localEulerAngles = new Vector3(0, 90, 90);
			//Debug.LogError("SITUATION ONE");
		} break;

		default: break;
		}
	}

	public void MessageControl()
	{
		bool blackInDown = false, whiteInDown = false;
		foreach(GameObject obj in level.room[2].trigger[0].innerObjs)
		{
			if(obj == black.gameObject)
				blackInDown = true;
			else if(obj == white.gameObject)
				whiteInDown = true;
		}

		if(level.room[2].trigger[0].PlayerStay)
		{



			if(blackInDown && (whiteInDown || white.InHands) && !lift.cell.IsActive && lift.inTop)
			{
				StartCoroutine( ShowMessage(0) );
			}
		}
		else if(level.room[0].trigger[0].PlayerStay)
		{
			bool blackInTop = false, whiteInTop = false;

			foreach(GameObject obj in level.room[0].trigger[0].innerObjs)
			{
				if(obj == black)
					blackInTop = true;
				else if(obj == white)
					whiteInTop = true;
			}

			if(blackInDown && whiteInDown && (!cell[0].IsActive /*|| !cell[1].IsActive*/) && !level.lift[0].cell.IsActive && level.room[0].trigger[0].PlayerStay && (!black.InHands && !white.InHands))
			{
				StartCoroutine( ShowMessage(1) );
			}

			else if(!blackInDown && whiteInDown && !level.lift[0].cell.IsActive)
			{
				StartCoroutine( ShowMessage(2) );
			}
		}
	}

	void Update () 
	{

		if(!showMessage)
			MessageControl();
	}

}
