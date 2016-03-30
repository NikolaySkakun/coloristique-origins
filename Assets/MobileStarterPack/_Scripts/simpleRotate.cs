using UnityEngine;
using System.Collections;

public class simpleRotate : MonoBehaviour {

	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime*50);
	}
}
