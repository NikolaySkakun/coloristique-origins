using UnityEngine;
using System.Collections;

public class TimeMachine : MonoBehaviour 
{
	ArrayList position;// = new ArrayList(
	ArrayList rotation;
	//Vector3[] position;
	//Vector3[] rotation;

	float step = 0.5f;
	float maxTime = 10f;
	float lastUpdate = 0;
	int count = 0;

	void Start()
	{
		count = (int)(maxTime / step);

		position = new ArrayList(count); //new Vector3[count];
		rotation = new ArrayList(count); //new Vector3[count];
	}

	void AddFrameData()
	{
		Debug.LogWarning("Upd");
		if(position.Count > count)
		{
			position.RemoveAt(0);
		}
		position.Add(transform.position);


		if(rotation.Count > count)
		{
			rotation.RemoveAt(0);
		}
		rotation.Add(transform.eulerAngles);
	}

	void Update()
	{
		if(Time.time - lastUpdate >= step)
		{
			AddFrameData();
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			Player.player.GetComponent<CharacterController>().enabled = false;
			Player.component.enabled = false;
			gameObject.AddComponent<Animation>().AddClip(Game.CreateAnimationClip(Game.AnimationClipType.POSITION, position, maxTime), "Time");
			gameObject.GetComponent<Animation>().Play("Time");

		}
	}


}
