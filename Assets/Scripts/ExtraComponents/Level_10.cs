using UnityEngine;
using System.Collections;

public class Level_10 : MonoBehaviour 
{
	Level level;
	Transform center;

	bool gravity = false;
	GameObject semi;

	void Start () 
	{
		level = Level.current;

		semi = CustomObject.SemicircleRoom(new Vector3(10, 10, 10), Obj.Colour.WHITE, 100);

		semi.transform.position = level.room[0].side[1].transform.position;

		semi.transform.parent = level.transform;

		semi.transform.localEulerAngles += Vector3.up*90f + Vector3.forward*90f;

		level.room[0].side[1].gameObject.SetActive(false);

		Player.player.AddComponent<Gravity>();


//		for (int i = 0; i < level.transform.childCount; ++i)
//		{
//			level.transform.GetChild (i).localPosition -= new Vector3(5, 0, 0);
//		}

		level.room [1].gameObject.SetActive (false);

		level.room [0].trigger [0].OnTriggerEnterPlayer += DisableGravity;
		level.room [0].trigger [0].OnTriggerExitPlayer += EnableGravity;
		level.room [0].trigger [0].OnTriggerExitPlayer += OriginalRotation;

		level.room [0].trigger [1].OnTriggerEnterPlayer += DisableGravity;
		level.room [0].trigger [1].OnTriggerExitPlayer += EnableGravity;
		level.room [0].trigger [1].OnTriggerExitPlayer += ReverseRotation;

		level.room [0].trigger [1].transform.localPosition += Vector3.up * 7;


		center = new GameObject ("Center").transform;
		center.position = level.transform.position + Vector3.right * 5;
		level.transform.parent = center;

		level.outletDoor.transform.localEulerAngles += Vector3.forward * 180f;
		level.outletDoor.transform.localPosition += Vector3.up * Door.sizeTemplate.y;
	}

//	void Update()
//	{
//		if (Input.GetKeyDown (KeyCode.Space)) {
//			Debug.LogWarning("gravity");
//			gravity = !gravity;
//		}
//
////		if (Input.GetKey (KeyCode.Space)) {
////			level.transform.localEulerAngles -= Vector3.forward * 0.5f;
////			//Debug.LogWarning("gravity");
////			//gravity = !gravity;
////		}
//	}

	float previousAngle = -1f;
	bool half = false;

	void OriginalRotation()
	{
		center.eulerAngles = Vector3.zero;
		//Debug.LogWarning ("zero");
	}

	void ReverseRotation()
	{
		center.eulerAngles = Vector3.forward * 180f;
		//Debug.LogWarning ("180");
	}

	void EnableGravity()
	{
		gravity = true;
	}

	void DisableGravity()
	{
		gravity = false;
	}

	void FixedUpdate()
	{
		bool a = false;
		foreach (Ball ball in level.ball)
		{
			ball.GetComponent<Rigidbody> ().AddForce (Vector3.up * 13.6f * (a ? 1f : -1f), ForceMode.Force);
			a = !a;
		}
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.LogWarning("gravity");
			gravity = !gravity;
		}

		if (gravity) 
		{
			Vector3 p = Player.player.transform.position;
			p.z = 0;
			Vector3 c = semi.transform.position;
			c.z = 0;

			Vector3 dir = (p - c).normalized;

			if (Player.player.transform.parent != null)
				Player.player.transform.parent = null;

			float angle = Vector3.Angle (dir, level.transform.up); //semi.transform.right


			if (!half && angle <= 90f)
			{
				//center.eulerAngles = Vector3.forward * 270f;

				level.transform.parent = null;
				center.transform.position += Vector3.right * 10;
				level.transform.parent = center;

				half = true;
			} else if (half && angle > 90f)
			{
				//center.eulerAngles = Vector3.forward * 270f;

				level.transform.parent = null;
				center.transform.position -= Vector3.right * 10;
				level.transform.parent = center;

				half = false;
			}

			center.eulerAngles = Vector3.forward * (180f + angle);

		}
	}

}
