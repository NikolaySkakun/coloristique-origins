using UnityEngine;
using System.Collections;

public class temp : MonoBehaviour {


	void Update()
	{
		Quaternion rot = transform.localRotation;

		if(Input.GetKey(KeyCode.X))
			rot.x += 0.01f;
		else if(Input.GetKey(KeyCode.Y))
			rot.y += 0.01f;
		else if(Input.GetKey(KeyCode.Z))
			rot.z += 0.01f;
		else if(Input.GetKey(KeyCode.W))
			rot.w += 0.01f;


		transform.localRotation = rot;
	}

}
