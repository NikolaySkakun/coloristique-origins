using UnityEngine;
using System.Collections;

public class Level_20 : MonoBehaviour 
{
	Level level;
	Cell[] cell;
	Trigger[] trigger;
	GameObject[] pointer = new GameObject[2];

	GameObject leftGroup, rightGroup;

	float startTime = 0;

	void Awake()
	{
		level = Level.current;

		Game.DrawEvent -= level.outletDoor.Draw;

		int[] r = new int[] {3, 9, 6, 12, 13, 14};

		for(int i=0; i<r.Length; ++i)
		{
			Game.DrawEvent -= level.room[r[i]].Draw;

			foreach(Side s in level.room[r[i]].side)
			{
				Game.DrawEvent -= s.Draw;

				foreach(Line l in s.line)
					Game.DrawEvent -= l.Draw;
			}
		}

		/*Game.DrawEvent -= level.room[3].Draw;
		Game.DrawEvent -= level.room[9].Draw;
		Game.DrawEvent -= level.room[6].Draw;
		Game.DrawEvent -= level.room[12].Draw;*/

		int[] r_ = new int[] {7, 8, 9, 10, 11, 12};
		
		for(int i=0; i<r.Length; ++i)
		{
			Side s = level.room[r_[i]].side[2];
			Line[] line = new Line[]{s.line[r_[i]%2==0 ? 0 : 2], s.line[r_[i]%2==0 ? 1 : 3]};
			
			foreach(Line l in line)
			{
				l.Repaint();
				l.transform.localPosition -= Vector3.forward*0.0001f;
				l.transform.localScale = new Vector3(0.15f, 0.99f, 1);
				Game.DrawEvent -= l.Draw;
			}
			//line[0].Repaint();
			//line[1].Repaint();
			
		}
		 
	}

	void Start() 
	{
		level.ball[0].GetComponent<Rigidbody>().isKinematic = true;
		level.ball[1].GetComponent<Rigidbody>().isKinematic = true;



		level.room[3].side[4].gameObject.AddComponent<KineticSide>().SetLength(4);

		level.room[6].side[0].gameObject.AddComponent<KineticSide>().SetLength(5);

		trigger = new Trigger[7];

		for(int i=0; i<trigger.Length; ++i)
			trigger[i] = level.room[i].trigger[0];


		pointer[0] = CustomObject.Pointer();
		pointer[0].transform.position = level.room[1].transform.position + Vector3.up*0.001f;
		pointer[0].transform.parent = level.transform;
		pointer[0].transform.localEulerAngles = Vector3.up * 180f;
		pointer[0].transform.localScale *= 0.8f;
		pointer[0].transform.parent = level.room[1].transform;


		pointer[1] = CustomObject.Pointer();
		pointer[1].transform.position = level.room[4].transform.position + Vector3.up*0.001f;
		pointer[1].transform.parent = level.transform;
		pointer[1].transform.localEulerAngles = Vector3.up * 90f;	//pointer.transform.localEulerAngles = Vector3.up * 270f;
		pointer[1].transform.localScale *= 0.8f;
		pointer[1].transform.parent = level.room[4].transform;


		leftGroup = new GameObject("LeftGroup");
		rightGroup = new GameObject("RightGroup");
		//yield return new WaitForSeconds(Game.drawTime);
		level.outletDoor.transform.parent = leftGroup.transform;

		leftGroup.transform.parent = rightGroup.transform.parent = level.transform;

		int[] lg = new int[] {1, 2, 3, 7, 8, 9};
		int[] rg = new int[] {4, 5, 6, 10, 11, 12};

		for(int i=0; i<lg.Length; ++i)
		{
			level.room[lg[i]].transform.parent = leftGroup.transform;
			level.room[rg[i]].transform.parent = rightGroup.transform;
		}

		level.room[13].transform.parent = level.room[3].transform;
		level.room[14].transform.parent = level.room[6].transform;

		/*
		GameObject text = Word.WriteString("diverse guns");
		text.transform.localEulerAngles = new Vector3(0, 0, 90);
		text.transform.localScale = Vector3.one * 0.3f;
		text.transform.position = level.room[1].side[1].transform.position;
		text.transform.position += -Vector3.right*0.001f - Vector3.up*0.1f + Vector3.forward;
		text.transform.parent = level.transform;
*/
	}

	void VisibleControl(int index)
	{
		if(index == 0)
		{
			leftGroup.SetActive(true);
			rightGroup.SetActive(true);

			if(level.ball[0].transform.parent == level.transform)
				level.ball[0].transform.parent = level.room[0].transform;
			if(level.ball[1].transform.parent == level.transform)
				level.ball[1].transform.parent = level.room[0].transform;

			/*level.room[3].transform.position += Vector3.up*100f;
			level.room[9].transform.position += Vector3.up*100f;

			level.room[6].transform.position += Vector3.up*100f;
			level.room[12].transform.position += Vector3.up*100f;*/
			level.room[3].gameObject.SetActive(false);
			level.room[9].gameObject.SetActive(false);

			level.room[6].gameObject.SetActive(false);
			level.room[12].gameObject.SetActive(false);
		}
		else
		{
			if(index < 4)
			{
				rightGroup.SetActive(false);
				//trigger[index].transform.parent;
				if(level.ball[0].transform.parent == level.transform)
					level.ball[0].transform.parent = leftGroup.transform;
				if(level.ball[1].transform.parent == level.transform)
					level.ball[1].transform.parent = leftGroup.transform;
				//level.room[3].transform.position -= Vector3.up*100f;
				//level.room[9].transform.position -= Vector3.up*100f;
				level.room[3].gameObject.SetActive(true);
				level.room[9].gameObject.SetActive(true);
			}
			else
			{
				leftGroup.SetActive(false);

				if(level.ball[0].transform.parent == level.transform)
					level.ball[0].transform.parent = rightGroup.transform;
				if(level.ball[1].transform.parent == level.transform)
					level.ball[1].transform.parent = rightGroup.transform;
				//level.room[6].transform.position -= Vector3.up*100f;
				//level.room[12].transform.position -= Vector3.up*100f;

				level.room[6].gameObject.SetActive(true);
				level.room[12].gameObject.SetActive(true);
			}
		}


		/*if(index == 3)
			return;
		level.room[trigger.Length - 1 - index].gameObject.SetActive(false);

		if(index != 6)
			level.room[index + 1].gameObject.SetActive(true);*/
	}

	void Update()
	{
		//if(Time.time > startTime + Game.drawTime)
		for(int i=0; i<trigger.Length; ++i)
		{
			if(trigger[i].PlayerStay)
			{
				VisibleControl(i);
				break;
			}
		}
	}

}
