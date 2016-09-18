using UnityEngine;
using System.Collections;

public class Dig : MonoBehaviour {

	// Use this for initialization

	GameObject d;
	InfoTable info;

	void Start () 
	{
		//info = InfoTable.NonXmlCreate();
		//d = Digit._2();
		GameObject obj = new GameObject();
		obj.AddComponent<MeshFilter>().mesh = OctahedronSphereCreator.Create(4, 0.5f);
		obj.AddComponent<MeshRenderer>();
	}
	
	// Update is called once per frame
	void Update () {


		if(Input.GetKeyUp(KeyCode.Space))
		{
			info.Draw();
			//Digit.From1ToEmpty();
			//d = Digit.Shift(2, 3);
		}
	}
}
