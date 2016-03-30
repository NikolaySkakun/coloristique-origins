using UnityEngine;
using System.Collections;

public class Level_10 : MonoBehaviour 
{
	Level level;

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

	}

	void Update()
	{
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.LogWarning("gravity");
			gravity = !gravity;
		}

	}

	void FixedUpdate()
	{
		if (gravity) 
		{
			Vector3 dir = (semi.transform.position - Player.player.transform.position).normalized;
			dir.z = 0;
			Physics.gravity = -dir * 9.8f;
			Debug.DrawRay(semi.transform.position, -dir);
			Vector3 euler = Player.player.transform.localEulerAngles;

			//Player.player.transform.di
			Player.player.transform.up = dir;
			//Debug.LogWarning(Player.player.transform.InverseTransformDirection(Player.player.transform.localEulerAngles));

			euler = new Vector3(Player.player.transform.localEulerAngles.x, Player.player.transform.localEulerAngles.y, euler.z);
			Player.player.transform.localEulerAngles = euler;
			//Player.player.transform
			//Player.player.GetComponent<Rigidbody>().ro
		}
	}

}
