using UnityEngine;
using System.Collections;

public class KineticSide : MonoBehaviour 
{
	Side side;
	public float length = 1f, minDistance = 1f;

	int direction;
	Vector3 directionVector = Vector3.zero;
	Vector3 originalPosition;

	void Awake()
	{
		originalPosition = transform.position;

		side = gameObject.GetComponent<Side>();

		if(side.index < 2)
			direction = 0;
		else
			direction = 2;

		directionVector[direction] = side.index%2 == 0 ? 1 : -1;

		//SetLength(1f);
	}

	public void SetLength(float l)
	{
		//if(originalPosition == Vector3.zero)
		//	originalPosition = transform.position;

		length = l;
		transform.position = originalPosition + directionVector*length;
	}


	void Update () 
	{
		float currentDistance = Mathf.Abs( Player.player.transform.position[direction] - transform.position[direction] );
		float len = originalPosition[direction] + Mathf.Abs( directionVector[direction])*length - transform.position[direction];
			//Mathf.Abs( transform.position[direction] - originalPosition[direction] );

		//Debug.LogWarning(len);

		if(currentDistance < minDistance && len <= length)
		{
			//Debug.LogWarning("__");
			transform.position -= directionVector*(minDistance - currentDistance);


		}
		else if(len > length)
		{
			transform.position = originalPosition - directionVector*0.001f;
		}
		/*else if(currentDistance > minDistance && len < length)
		{
			transform.localPosition = originalPosition;
		}*/
		/*else if(currentDistance > minDistance && len < length)
		{
			Debug.LogWarning("+++");
			transform.position += directionVector*(length - len);
		}*/



	}
}
