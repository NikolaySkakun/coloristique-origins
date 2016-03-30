using UnityEngine;
using System.Collections;

public class Console : MonoBehaviour 
{
	void Update () 
	{
		//if(Input.GetKeyDown(KeyCode.Quote))
			Debug.LogWarning(Input.inputString);
	}
}
