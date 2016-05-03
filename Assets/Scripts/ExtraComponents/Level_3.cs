using UnityEngine;
using System.Collections;

public class Level_2_ : MonoBehaviour 
{
	Level level;


	void Start() 
	{
		level = Level.current;

//		Shader depthMask = Shader.Find("DepthMask");
//
//
//		Transform stairs = Instantiate(level.room[0].ledge[0].stairsPlane.transform);
//		stairs.SetParent(level.room[0].ledge[0].stairsPlane.transform);
//		stairs.localEulerAngles = Vector3.zero;
//		stairs.localScale = Vector3.one;
//		stairs.localPosition = Vector3.zero;
//		
//		
//		stairs.gameObject.layer = LayerMask.NameToLayer("Invisible");
//		stairs.GetComponent<Renderer>().material = new Material(Shader.Find("Invisible"));
//		stairs.GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue - 1;
//
//		//level.room[0].ledge[0].stairsPlane.layer = LayerMask.NameToLayer("Invisible");//.GetComponent<Renderer>().material = new Material(depthMask);
//		//level.room[0].ledge[0].stairsPlane.GetComponent<Renderer>().material.renderQueue = 2016;
//		//for(int i=0; i<level.room[0].ledge[0].stairsPlane.transform.parent.childCount; ++i)
//		//	level.room[0].ledge[0].stairsPlane.transform.parent.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Invisible");
//
//		level.room[0].side[0].GetComponent<Renderer>().material.renderQueue = depthMask.renderQueue;
//		level.room[0].side[2].GetComponent<Renderer>().material.renderQueue = depthMask.renderQueue;
//		level.room[0].side[3].GetComponent<Renderer>().material.renderQueue = depthMask.renderQueue;
//		level.room[0].ledge[0].ledgePlane.GetComponent<Renderer>().material.renderQueue = depthMask.renderQueue;
//
//		Debug.LogWarning(Shader.Find("DepthMask").renderQueue);
//		Debug.LogWarning("b: " + Shader.Find("Base").renderQueue.ToString());
//
//		GameObject[] triangleWall = new GameObject[4];
//		for(int i=0; i<triangleWall.Length; ++i)
//		{
//			triangleWall[i] = Word.GetGameObject(CustomMesh.Test());
//			//triangleWall[i].GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//			triangleWall[i].transform.SetParent(level.room[0].transform);
//			triangleWall[i].layer = LayerMask.NameToLayer("Invisible");
//			triangleWall[i].GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//		}
//
//		triangleWall[0].transform.localPosition = new Vector3(-6.5f, 0, -4.35f);
//		triangleWall[0].transform.localScale = new Vector3(1, -1, 1);
//		triangleWall[0].transform.localEulerAngles = Vector3.right * (-90);
//
//
//		triangleWall[1].transform.localPosition = new Vector3(-6.5f, 0, -4.35f);
//		triangleWall[1].transform.localEulerAngles = Vector3.right * (-90);
//		triangleWall[1].GetComponent<Renderer>().material = new Material(Shader.Find("Invisible"));
//
//
//		
//		triangleWall[2].transform.localPosition = new Vector3(-6.5f, 0, -5.85f);
//		triangleWall[2].transform.localScale = new Vector3(1, -1, 1);
//		triangleWall[2].transform.localEulerAngles = Vector3.right * (-90);
//		triangleWall[2].GetComponent<Renderer>().material = new Material(Shader.Find("Invisible"));
//
//
//		
//		triangleWall[3].transform.localPosition = new Vector3(-6.5f, 0, -5.85f);
//		triangleWall[3].transform.localEulerAngles = Vector3.right * (-90);
//		//[1].GetComponent<Renderer>().material = new Material(Shader.Find("Invisible"));
//		
//		triangleWall[1].GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue - 1;
//		
//		triangleWall[2].GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue - 1;
//
//
//
//		//level.ball[1].GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask2"));
//		//circle.GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
//		level.room[0].side[0].GetComponent<MeshRenderer>().enabled = true;
//		//level.ball[0].GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
//		int u=0;
//		foreach(Side s in level.room[2].side)
//		{
//			s.GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//			s.gameObject.layer = LayerMask.NameToLayer("Invisible");
//
//			for(int i=0; i<s.transform.childCount; ++i)
//			{
//			s.transform.GetChild(i).GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//				s.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Invisible");
//			}
//
//		}
//		foreach(Side s in level.room[3].side)
//		{
//			s.GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//			s.gameObject.layer = LayerMask.NameToLayer("Invisible");
//
//			for(int i=0; i<s.transform.childCount; ++i)
//			{
//				s.transform.GetChild(i).GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue;
//				s.transform.GetChild(i).gameObject.layer = LayerMask.NameToLayer("Invisible");
//			}
//		}
//
//		for(int i=0; i<level.room[0].side[0].transform.childCount; ++i)
//		{
//			Transform t = level.room[0].side[0].transform.GetChild(i);
//			Transform p = t.parent;
//
//			if(t.GetComponent<Line>() == null)
//			{
//				Transform inst = Instantiate(t).transform;
//				
//				//inst.parent = t.parent;
//				inst.SetParent(t);
//				inst.localEulerAngles = Vector3.zero;
//				inst.localScale = Vector3.one;
//				inst.localPosition = Vector3.zero;
//
//
//				inst.gameObject.layer = LayerMask.NameToLayer("Invisible");
//				inst.GetComponent<Renderer>().material = new Material(Shader.Find("Invisible"));
//				inst.GetComponent<Renderer>().material.renderQueue = Shader.Find("Invisible").renderQueue - 1;
//			}
//		}
//
//		level.room[2].side[2].transform.localScale += Vector3.right;
//		level.room[2].side[2].transform.localPosition = Vector3.right * 0.5f;
//
//		level.room[2].side[3].transform.localScale = new Vector3(1.28f, 1, 1);
//		level.room[2].side[3].transform.localPosition = new Vector3(0.139f, 1, 0);
//		level.room[2].side[3].line[0].gameObject.SetActive(false);
//
//		Line line = level.room[2].side[1].line[3];
//		line.gameObject.SetActive(true);
//		line.GetComponent<Renderer>().material.renderQueue = Shader.Find("Base").renderQueue;
//		//line.gameObject.layer = LayerMask.NameToLayer("Default");
//		line.Repaint();
//
//		Line line2 = level.room[2].side[2].line[1];
//		line2.Repaint();

		//line2.transform.localScale = new Vector3(line2.transform.localScale.x*1.5f, line2.transform.localScale.y, line2.transform.localScale.z);
		//line2.transform.position -= Vector3.right * Line.height;

	}
	bool showMessage = false;
	IEnumerator ShowMessage(int situation)
	{
		showMessage = true;
		yield return new WaitForSeconds(1f);
		float original = 1.5f;
		switch(situation)
		{

		case 0:
		{
			string msg = MessageController.GetMessage(level.Index, situation);
			GameObject message = Word.WriteString(msg, original/(float)msg.Length);
			//GameObject message = Word.WriteString(Message.WRONG, 0.172f, Obj.Colour.BLACK);
			message.transform.parent = level.room[0].transform;
			message.transform.position = level.room[0].wall[0].transform.position + Vector3.right * level.room[0].Size.x/6f
				+ Vector3.forward * Wall.thick/1.99f - 
					Vector3.up * level.room[0].Size.y/8f;
			message.transform.localEulerAngles = new Vector3(0, 90, 90);
			
			
			GameObject restart = Word.WriteString(Message.RESTART, 0.05f, Obj.Colour.BLACK);
			restart.transform.parent = level.room[0].transform;
			restart.transform.position = level.room[0].wall[0].transform.position + Vector3.right * level.room[0].Size.x/6.3f +  
				Vector3.forward * Wall.thick/1.99f -  
					Vector3.up * level.room[0].Size.y/4.8f;
			restart.transform.localEulerAngles = new Vector3(0, 90, 90);
		} break;
			
		default: break;
			
		}
		
	}


	void MessageControl()
	{


		if(level.room[0].trigger[0].PlayerStay && level.room[0].wall[0].IsClosed)
		{
			foreach(GameObject obj in level.room[0].trigger[0].innerObjs)
			{
				if(obj == level.ball[1].gameObject)
					return;
			}

			Game.Deadlock();
			StartCoroutine(ShowMessage(0));
			//showMessage = true;
		}
	}

	void Update()
	{

		if(!showMessage)
			MessageControl();


	}

	/*GameObject text = Word.WriteString("minimalism", 0.5f, Obj.Colour.WHITE);
		text.transform.localEulerAngles = new Vector3(0, 270, 90);
		text.transform.localScale = Vector3.one * 0.5f;
		text.transform.position = level.room[0].wall[0].transform.position;
		text.transform.position += -Vector3.forward*0.21f - Vector3.up*0.7f - Vector3.right*2.5f;
		text.transform.parent = level.room[0].wall[0].wall.transform;*/

}
