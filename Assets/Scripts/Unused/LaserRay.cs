using UnityEngine;
using System.Collections;

public class LaserRay
{
	GameObject ray;

	static float thick = 0.01f;
	Vector3 scale = Vector3.one;

	public LaserRay()
	{
		ray = new GameObject("Ray");

		GameObject child = GameObject.CreatePrimitive(PrimitiveType.Cube);
		child.layer = LayerMask.NameToLayer("Gun");
		child.transform.parent = ray.transform;
		child.GetComponent<Renderer>().material.color = Color.black;

		ray.transform.parent = Level.current.transform;

		child.transform.localScale = Vector3.one * thick;
		child.transform.localPosition -= Vector3.forward * thick/2f;
	}

	public void Update(Vector3 from, Vector3 direction, RaycastHit hit, float value = 0f)
	{
		ray.transform.position = from;
		//Debug.Log("dist: " + hit.distance);
		float dist = Vector3.Distance(from, hit.point);
		scale.z = -(dist)/thick;
		//scale.z = -(hit.distance - value)/thick;
		ray.transform.localScale = scale;
		//ray.transform.forward = direction;
		ray.transform.LookAt(hit.point);

	}

}
