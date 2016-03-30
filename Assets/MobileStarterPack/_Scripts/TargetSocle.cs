using UnityEngine;
using System.Collections;

public class TargetSocle : MonoBehaviour {
	public static  GameObject target;
	int hit;
	void Start () {
		target = GameObject.FindWithTag ("Player");
	}
	
	void Update () {
		transform.LookAt(target.transform);
		transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
	}
}
