using UnityEngine;
using System.Collections;

public class VR_UIpointer : MonoBehaviour {
	
	public GameObject target;
	public float maxDistance=5f;
	private Transform vrCameraHead;
	private GameObject aimUI;
	private GameObject arrowUI;
	
	// Use this for initialization
	void Start () {
		vrCameraHead = GameObject.FindObjectOfType<VRCamera>().vrCameraHeading;
		aimUI = transform.Find("AimUIholder").gameObject;
		arrowUI = transform.Find("Arrow/Arrow").gameObject;
	}


	bool visible=false;
	// Update is called once per frame
	void LateUpdate () {
		if( Vector3.Distance(transform.position,target.transform.position)<maxDistance )
		{
			if( !visible )
			{
				Renderer[] r = gameObject.GetComponentsInChildren<Renderer>();
				foreach( Renderer _r in r ) _r.enabled=true;
				visible=true;
			}
		}
		else
		{
			if( visible )
			{
				Renderer[] r = gameObject.GetComponentsInChildren<Renderer>();
				foreach( Renderer _r in r ) _r.enabled=false;
				visible=false;
			}
		}
		gameObject.SetActive(true);
		transform.position = vrCameraHead.position;
		transform.rotation = vrCameraHead.rotation;
		if( visible ) 
		{
			aimUI.transform.LookAt(target.transform.position);
			arrowUI.transform.LookAt(target.transform.position,target.transform.position-arrowUI.transform.position);
			arrowUI.transform.localRotation = Quaternion.Euler(0f,arrowUI.transform.localRotation.eulerAngles.y,0f);
		}
	}
}
