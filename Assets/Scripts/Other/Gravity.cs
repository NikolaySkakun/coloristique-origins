using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour 
{
	/*void OnControllerColliderHit(ControllerColliderHit hit)
	{
		hit.collider.
	}*/
	bool r = false;

	CharacterController controller;
	ControllerColliderHit hit;

	GameObject semicircleRoom;

	void FixedUpdate()
	{
		if(r  && hit.gameObject.tag == "Side")
		{
			Debug.LogWarning(transform.rotation.w);
			//Physics.gravity = -hit.normal*9.8f*Time.deltaTime;//.fixedDeltaTime;
			//Physics.gravity = Vector3.Lerp(Physics.gravity, -hit.normal*9.8f, Time.deltaTime);
			
			Debug.DrawRay(hit.point, Physics.gravity/3f, Color.red, 1);

			Vector3 dir = (Player.player.transform.position - semicircleRoom.transform.position).normalized;
			Physics.gravity = dir * 9.8f;

			
			//Vector3 euler = transform.localEulerAngles;
			Quaternion rotation = transform.rotation;
			
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, -dir);

			//Debug.LogWarning("1: " + transform.rotation.ToString());
			//transform.up = hit.normal;
			//Quaternion rot = Quaternion.FromToRotation(Vector3.up, Vector3.Lerp(Physics.gravity.normalized, -hit.normal, Time.deltaTime)); //hit.normal
			//rot.z = rotation.z;
			
			transform.rotation = rot;
			
			//transform.RotateAround(transform.position, Vector3.up, rotation.eulerAngles.y);
			//transform.rotation.Set(transform.rotation.x, rotation.y, transform.rotation.z, transform.rotation.w);
			//.gravity = Vector3.Lerp(Physics.gravity, -hit.normal*9.8f, Time.deltaTime);

			//Debug.LogWarning("2: " + transform.rotation.ToString());
		}
		else
			Physics.gravity = -transform.up*9.8f;
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		this.hit = hit;
		/*if(r  && hit.gameObject.tag == "Side")
		{
			Debug.LogWarning(transform.rotation.w);
			Physics.gravity = -hit.normal*9.8f*Time.deltaTime;

			
			Debug.DrawRay(hit.point, Physics.gravity, Color.red, 1);
			
			//Vector3 euler = transform.localEulerAngles;
			Quaternion rotation = transform.rotation;


			//Debug.LogWarning("1: " + transform.rotation.ToString());
			//transform.up = hit.normal;
			Quaternion rot = Quaternion.FromToRotation(Vector3.up, hit.normal);
			//rot.z = rotation.z;

			transform.rotation = rot;

			//transform.RotateAround(transform.position, Vector3.up, rotation.eulerAngles.y);
			//transform.rotation.Set(transform.rotation.x, rotation.y, transform.rotation.z, transform.rotation.w);

			//Debug.LogWarning("2: " + transform.rotation.ToString());
		}
		else
			Physics.gravity = -transform.up*9.8f;*/

	}
	// Use this for initialization
	void Start () {
				controller = gameObject.GetComponent<CharacterController>();
		semicircleRoom = GameObject.Find("SemicircleRoom");
		this.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

		//Debug.LogWarning(transform.rotation.w);
		/*if(Input.GetKey(KeyCode.Z))
			transform.up += Vector3.right*0.01f;
		else if(Input.GetKey(KeyCode.X))
			transform.up += Vector3.up*0.01f;
		else if(Input.GetKey(KeyCode.C))
			transform.up += Vector3.forward*0.01f;
		else if(Input.GetKey(KeyCode.V))
			transform.up -= Vector3.right*0.01f;
		else if(Input.GetKey(KeyCode.B))
			transform.up -= Vector3.up*0.01f;
		else if(Input.GetKey(KeyCode.N))
			transform.up -= Vector3.forward*0.01f;*/


		//Debug.LogWarning(transform.rotation);
		if(Input.GetKeyDown(KeyCode.G))
			r = !r;
	}
}
