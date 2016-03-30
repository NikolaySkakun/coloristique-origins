// Copyright (C) Stanislaw Adaszewski, 2013
// http://algoholic.eu

using UnityEngine;
using System.Collections;

public class FillScreen : MonoBehaviour {

	public enum Mode { ONE_SIDE_NORMAL, PARALLEL_SIDE_NORMAL};

	[SerializeField]
	Mode mode = Mode.PARALLEL_SIDE_NORMAL;

	public Camera cam;
	
	public Transform portal1;
	public Camera portal1Cam;
	
	public Transform portal2;
	public Camera portal2Cam;


	void Update()
	{
		cam = Player.camera.GetComponent<Camera>();
	}




	void LateUpdate () 
	{
		switch(mode)
		{
		case Mode.PARALLEL_SIDE_NORMAL:
		{
			Quaternion q = Quaternion.FromToRotation(-portal1.up, cam.transform.forward);
			portal1Cam.transform.position = portal2.position + ( cam.transform.position - portal1.position ) ;
			portal1Cam.transform.LookAt(portal1Cam.transform.position + q * portal2.up, portal2.transform.forward);
			portal1Cam.nearClipPlane = (portal1Cam.transform.position - portal2.position).magnitude - 0.3f;

			q = Quaternion.FromToRotation(-portal2.up, cam.transform.forward);
			portal2Cam.transform.position = portal1.position + (cam.transform.position - portal2.position);
			portal2Cam.transform.LookAt (portal2Cam.transform.position + q * portal1.up, portal1.transform.forward);
			portal2Cam.nearClipPlane = (portal2Cam.transform.position - portal1.position).magnitude - 0.3f;
		} break;

		case Mode.ONE_SIDE_NORMAL:
		{
			Quaternion q = Quaternion.FromToRotation(-portal1.up, cam.transform.forward);
			//Vector3 tmp = (cam.transform.position - portal1.position);
			//tmp.x = portal2.position.x - tmp.x;
			//tmp.y = 0;
			portal1Cam.transform.position = portal2.position + (  portal1.position - cam.transform.position) ;
			portal1Cam.transform.LookAt(portal1Cam.transform.position + q * portal2.up, portal2.transform.forward);
			portal1Cam.nearClipPlane = (portal1Cam.transform.position - portal2.position).magnitude - 0.3f;
			portal1Cam.transform.localEulerAngles = new Vector3(-portal1Cam.transform.localEulerAngles.x, portal1Cam.transform.localEulerAngles.y, portal1Cam.transform.localEulerAngles.z);// Vector3.forward * 180f;
			
			//portal1Cam.transform.localEulerAngles = new Vector3(-portal1Cam.transform.localEulerAngles.x, -portal1Cam.transform.localEulerAngles.y, portal1Cam.transform.localEulerAngles.z + 180f);// Vector3.forward * 180f;
			
			q = Quaternion.FromToRotation(-portal2.up, cam.transform.forward);
			portal2Cam.transform.position = portal1.position + (cam.transform.position - portal2.position);
			portal2Cam.transform.LookAt (portal2Cam.transform.position + q * portal1.up, portal1.transform.forward);
			portal2Cam.nearClipPlane = (portal2Cam.transform.position - portal1.position).magnitude - 0.3f;
		} break;


		default: break;
		}
		


	}
}
