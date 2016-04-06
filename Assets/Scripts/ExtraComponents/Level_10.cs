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

		level.room [0].trigger [1].OnTriggerEnterPlayer += DisableGravity;
		level.room [0].trigger [1].OnTriggerExitPlayer += EnableGravity;

		level.room [0].trigger [1].transform.localPosition += Vector3.up * 7;


		center = new GameObject ("Center").transform;
		center.position = level.transform.position + Vector3.right * 5;
		level.transform.parent = center;
	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.LogWarning("gravity");
			gravity = !gravity;
		}

//		if (Input.GetKey (KeyCode.Space)) {
//			level.transform.localEulerAngles -= Vector3.forward * 0.5f;
//			//Debug.LogWarning("gravity");
//			//gravity = !gravity;
//		}
	}

	float previousAngle = -1f;
	bool half = false;

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
		if (gravity) 
		{
			//level.transform.localEulerAngles = Vector3.zero;
			Vector3 p = Player.player.transform.position;
			p.z = 0;
			Vector3 c = semi.transform.position;
			c.z = 0;

			Vector3 dir = (p - c).normalized;
//			float x =  (semi.transform.localPosition.x - Player.player.transform.localPosition.x);
//			float y = (semi.transform.localPosition.y - Player.player.transform.localPosition.y);
//			float angle = Mathf.Atan2 (y, x) * Mathf.Rad2Deg;
//
//			dir.z = 0;
//			Physics.gravity = -dir * 9.8f;
//			Debug.DrawRay(semi.transform.position, -dir);
//
//
//			Vector3 p = Player.player.transform.localPosition;
//			p.z = 0;
//
//			Vector3 c = semi.transform.localPosition;
//			c.z = 0;
//
//			Debug.LogWarning (Vector3.Angle(dir, semi.transform.right));
//
//			if (angle > 0)
//				angle = 360f - (angle - 90f);
//			else if (angle < 0)
//				angle = 270f - (angle + 180f);
//			else
//				angle = level.transform.localEulerAngles.z;

			if (Player.player.transform.parent != null)
				Player.player.transform.parent = null;

			float angle = Vector3.Angle (dir, level.transform.up); //semi.transform.right


			if (!half && angle <= 90f)
			{
				//Player.player.transform.parent = level.transform;
				//level.transform.localPosition -= Vector3.up * 10;
				level.transform.parent = null;
				center.transform.position += Vector3.right * 10;
				level.transform.parent = center;
//				for (int i = 0; i < level.transform.childCount; ++i)
//				{
//					level.transform.GetChild (i).localPosition -= new Vector3 (0, 10, 0);
//
//					//Player.player.transform.position
//				}
//				level.transform.position += Vector3.right * 10f;
				half = true;
			} else if (half && angle > 90f)
			{
				//Player.player.transform.parent = level.transform;
				//level.transform.localPosition += Vector3.up * 10;
				level.transform.parent = null;
				center.transform.position -= Vector3.right * 10;
				level.transform.parent = center;
//				for (int i = 0; i < level.transform.childCount; ++i)
//				{
//					level.transform.GetChild (i).localPosition += new Vector3 (0, 10, 0);
//				}
//				level.transform.position -= Vector3.right * 10f;
				half = false;
			}


//			if (previousAngle == -1)
//				previousAngle = angle;
//
//			if (Mathf.Abs (previousAngle - angle) < 5)
//				return;
//			else
//				previousAngle = angle;
//			float n = 0f;
//			if (level.transform.localEulerAngles.z <= 270f && angle > 0)
//				n = 90f;
//			angle = Mathf.Abs (angle - 90f) + n;
//
////
			//if(angle < 90f)
			center.eulerAngles = Vector3.Lerp(center.eulerAngles, Vector3.forward * (180f + angle), 1);

			//level.transform.LookAt (Player.player.transform);
			//level.transform.eulerAngles = new Vector3 (0, 0, level.transform.eulerAngles.y);


			//float a = Mathf.Abs(level.transform.eulerAngles.z - (180f + angle));

			//level.transform.RotateAround(level.transform.position, Vector3.forward, -a*Time.fixedDeltaTime);
			//Debug.LogWarning (angle);



			//Vector3 euler = Player.player.transform.localEulerAngles;

			//Player.player.transform.di
			//Player.player.transform.up = dir;
			//Debug.LogWarning(Player.player.transform.InverseTransformDirection(Player.player.transform.localEulerAngles));

			//euler = new Vector3(Player.player.transform.localEulerAngles.x, Player.player.transform.localEulerAngles.y, euler.z);
			//Player.player.transform.localEulerAngles = euler;
			//Player.player.transform
			//Player.player.GetComponent<Rigidbody>().ro
		}
	}

}
