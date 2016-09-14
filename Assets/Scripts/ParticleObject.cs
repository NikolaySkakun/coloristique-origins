using UnityEngine;
using System.Collections.Generic;

public class ParticleObject : MonoBehaviour 
{
	GameObject view;

	void Start () 
	{
		Mesh mesh = CreateMesh ();
		//Word.GetGameObject (mesh).transform.SetParent (transform);

		CreateSpheres (mesh, -Vector3.forward * 8f);

		transform.position = new Vector3 (-4.96f, 1.8814f, -2.16f);
		transform.eulerAngles = -Vector3.up * 90f;


	}

	void Update()
	{
		if (Mathf.Abs (Player.player.transform.position.x - view.transform.position.x) < 0.5f &&
		   Mathf.Abs (Player.player.transform.position.z - view.transform.position.z) < 0.5f)
			Player.player.transform.position = new Vector3 (view.transform.position.x , Player.player.transform.position.y, view.transform.position.z);
	}

	void CreateSpheres(Mesh mesh, Vector3 viewPoint)
	{
		int count = 16;
		float size = 0.1f;

		Vector3[] verts = mesh.vertices;
		int i = 0;

		foreach (Vector3 v in verts)
		{
			if (i % 2 == 0)
			{
				++i;
				continue;
			}

			float d = Vector3.Distance (v, viewPoint);
			float dist = Random.Range(0, d - d/5f);
			float d2 = d;


			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Destroy (sphere.GetComponent<SphereCollider> ());
			sphere.transform.position = Vector3.MoveTowards (v, viewPoint, dist);

			d = Vector3.Distance (sphere.transform.position, viewPoint - Vector3.right*0.3f);
			d2 = ((100f / d2) * d)/200f;
			d = 100f / d;



			sphere.GetComponent<Renderer> ().material.color = Color.black;
			float scale = (size/100f) * d;



			sphere.transform.localScale = (Vector3.one * (size - scale)) * (0.5f + d2);
			sphere.transform.SetParent (transform);

			++i;
		}

		view = new GameObject ();
		view.transform.position = viewPoint;
		view.transform.parent = transform;

//		for (int i = 0; i < count - 1; ++i)
//		{
//			for (int u = 0; u < count - 1; ++u)
//			{
//				GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//				sphere.transform.position = Vector3.MoveTowards(
//			}
//		}
	}

	Mesh CreateMesh()
	{
		Mesh mesh = new Mesh ();

		int count = 16;
		float size = 0.1f;
		float zero = -(size * count / 2f);

		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();

		for (int i = 0; i < count - 1; ++i)
		{
			for (int u = 0; u < count - 1; ++u)
			{
				verts.Add (new Vector3 (zero + u * size, zero + i * size, 0));
				verts.Add (new Vector3 (zero + u * size, zero + (i + 1) * size, 0));
				verts.Add (new Vector3 (zero + (u + 1) * size, zero + i * size, 0));
				verts.Add (new Vector3 (zero + (u + 1) * size, zero + (i + 1) * size, 0));

				int index = verts.Count;

				tris.Add (index - 4);
				tris.Add (index - 3);
				tris.Add (index - 2);

				tris.Add (index - 1);
				tris.Add (index - 2);
				tris.Add (index - 3);
			}
		}

		mesh.SetVertices (verts);
		mesh.triangles = tris.ToArray ();

		return mesh;
	}

	void Create(Mesh mesh, Vector3 position, Vector3 viewPoint)
	{

	}
}
