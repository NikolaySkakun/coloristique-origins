using UnityEngine;
using System.Collections.Generic;
using System.Xml;

public class ParticleObject : MonoBehaviour 
{
	GameObject view;

	List<EdgeCollider2D> edges = new List<EdgeCollider2D>();

	void Start () 
	{
		Mesh mesh = CreateMesh (); //GetUnityMesh (); //
		//Word.GetGameObject (mesh).transform.SetParent (transform);

//		gameObject.AddComponent<MeshRenderer> ();
//		gameObject.AddComponent<MeshFilter> ().mesh = GetUnityMesh ();


		Debug.LogWarning ("Unity");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml( (Resources.Load("Sprites/Unity") as TextAsset).text );
		EdgeCollider2D edge = gameObject.AddComponent<EdgeCollider2D> ();
		List<Vector2> points = SVGImporter.GetEdgeCollider (doc.DocumentElement as XmlNode, 10, 15);

		float up = 0.02f;

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.7095f;
		}

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.0905f + Vector2.up * up;
		}


		edge.points = points.ToArray ();
		edges.Add (edge);


		//- Vector3.right*0.7095f
		GameObject tempObj = new GameObject();
		tempObj.transform.SetParent (transform);
		edge = tempObj.AddComponent<EdgeCollider2D> ();

		points = SVGImporter.GetEdgeCollider (doc.DocumentElement as XmlNode, 10, 15);

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.7095f;
		}

		for (int i = 0; i < points.Count; ++i)
		{
			Vector3 p = Quaternion.Euler (0, 0, 120) * new Vector3 (points[i].x, points[i].y, 0);
			//points [i].x = p.x;
			//points [i].y = p.y;
			points [i] = new Vector2 (p.x, p.y);
		}

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.0905f + Vector2.up * up;
		}

		edge.points = points.ToArray ();
		edges.Add (edge);


		tempObj = new GameObject();
		tempObj.transform.SetParent (transform);
		edge = tempObj.AddComponent<EdgeCollider2D> ();

		points = SVGImporter.GetEdgeCollider (doc.DocumentElement as XmlNode, 10, 15);

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.7095f;
		}

		for (int i = 0; i < points.Count; ++i)
		{
			Vector3 p = Quaternion.Euler (0, 0, 240) * new Vector3 (points[i].x, points[i].y, 0);
			points [i] = new Vector2 (p.x, p.y);
		}

		for (int i = 0; i < points.Count; ++i)
		{
			points [i] = points [i] - Vector2.right * 0.0905f + Vector2.up * up;
		}

		edge.points = points.ToArray ();
		edges.Add (edge);


		//verts.Add (Quaternion.Euler(0, 0, 120) * mesh.vertices[i]);


		CreateSpheres (mesh, -Vector3.forward * 8f);// + Vector3.right*4);






		transform.position = new Vector3 (-4.96f, 1.8814f, -2.16f);
		transform.eulerAngles = -Vector3.up * 90f;


	}

	public bool IntoContour(Collider2D edge, Vector2 origin, Vector2 dir)
	{
		//Vector2 p = new Vector2 (edge.transform.position.x, edge.transform.position.y);
		//dir = ( p - origin).normalized;

		int layer = edge.gameObject.layer;
		edge.gameObject.layer = LayerMask.NameToLayer ("TempPlatform");

		bool finalPoint = false;
		int points = 0;
		//Vector2 origin = new Vector2 (transform.position.x, transform.position.y);
		RaycastHit2D hit;

		while (!finalPoint)
		{
			hit = Physics2D.Raycast (origin, dir, 100f, 1 << LayerMask.NameToLayer ("TempPlatform"));

			if (hit)
			{
				++points;
				origin = hit.point + dir * 0.0001f;
			} 
			else
			{
				finalPoint = true;
			}
		}

		edge.gameObject.layer = layer;

		return !(points % 2 == 0);
	}

	void Update()
	{
		float step = 0.05f;

		if (Mathf.Abs (Player.player.transform.position.x - view.transform.position.x) < step &&
			Mathf.Abs (Player.player.transform.position.z - view.transform.position.z) < step)
			Player.player.transform.position = new Vector3 (view.transform.position.x , Player.player.transform.position.y, view.transform.position.z);
	}

	void CreateSpheres(Mesh mesh, Vector3 viewPoint)
	{
		int count = 128;
		float size = 0.03f;//4

		Vector3[] verts = mesh.vertices;
		int i = 0;
		int counter = 0;



		foreach (Vector3 v in verts)
		{
			if (i % 2 == 0)
			{
				++i;
				continue;
			}

			if (counter % 2 == 0)
			{
				++counter;
				++i;
				continue;

			}

			bool intoContour = false;

			foreach (EdgeCollider2D e in edges)
			{
				if (IntoContour (e, new Vector2 (v.x, v.y), Vector3.up))
				{
					intoContour = true;

					break;
				}
			}



			if (!intoContour)
			{
				++i;
				++counter;
				continue;
			}


			float d = Vector3.Distance (v, viewPoint);
			float coef = (d - d / 5f) / ((float)verts.Length);
			//float dist = 0;
			//float dist = (float)i * coef;
			float dist = Random.Range(0, d - d/5f);

			float test = (100f / 1.5f) * v.magnitude;  //d - d / 5f;
			test *= (d - d/5f)/100f;

			//dist = (d - d/5f) - test;

			GameObject sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
			Destroy (sphere.GetComponent<SphereCollider> ());
			sphere.transform.position = Vector3.MoveTowards (v, viewPoint, dist);

			d = (Vector3.Distance (sphere.transform.position, viewPoint) / (d)) * size;

			sphere.GetComponent<Renderer> ().material.color = Color.black;

			sphere.transform.localScale = Vector3.one * d;
			sphere.transform.SetParent (transform);
			sphere.name = d.ToString ();

			++counter;
			++i;
		}

		view = new GameObject ();
		view.transform.position = viewPoint;
		view.transform.parent = transform;
	}


	static public Mesh GetUnityMesh()
	{
		Debug.LogWarning ("Unity");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml( (Resources.Load("Sprites/Unity") as TextAsset).text );

		List<Vector3> verts = new List<Vector3> ();

		Mesh mesh = Triangulator.CreateMesh (SVGImporter.GetEdgeCollider (doc.DocumentElement as XmlNode, 10, 15));
		//CustomMesh.OptimizeMesh (mesh);
		for (int i = 0; i < mesh.vertexCount; ++i)
		{
			verts.Add ((mesh.vertices[i] - Vector3.right*0.7095f - Vector3.right * 0.0905f + Vector3.up * 0.015f)); // * 1.002f
		}
		mesh.SetVertices (verts);


		Mesh mesh2 = Object.Instantiate (mesh) as Mesh;
		verts = new List<Vector3> ();
		for (int i = 0; i < mesh2.vertexCount; ++i)
		{
			verts.Add (Quaternion.Euler(0, 0, 120) * mesh.vertices[i]);
		}
		mesh2.SetVertices (verts);

		mesh = LetterAnimation.CombineMeshes (new Mesh[]{ mesh, mesh2 });

		mesh2 = Object.Instantiate (mesh) as Mesh;
		verts = new List<Vector3> ();
		for (int i = 0; i < mesh2.vertexCount; ++i)
		{
			verts.Add (Quaternion.Euler(0, 0, 240) * mesh.vertices[i]);
		}
		mesh2.SetVertices (verts);

		mesh = LetterAnimation.CombineMeshes (new Mesh[]{ mesh, mesh2 });

		//		List<Vector3> v = new List<Vector3> ();
		//		List<int> t = new List<int> ();
		//
		//		for (int i = 0; i < mesh.triangles.Length; ++i)
		//		{
		//			int triangle = mesh.triangles [i];
		//
		//			Vector3 vertex = mesh.vertices [triangle];
		//
		//			v.Add (vertex);
		//			t.Add (i);
		//		}
		//
		//		mesh.SetVertices (v);
		//		mesh.triangles = t.ToArray();
		//
		//		verts = new List<Vector3> ();
		//		for (int i = 0; i < mesh.vertexCount; ++i)
		//		{
		//			verts.Add (Vector3.MoveTowards( mesh.vertices[i], -Vector3.forward * 5f, UnityEngine.Random.Range(1f, 4f)));
		//		}
		//		mesh.SetVertices (verts);

		return mesh;
	}


	Mesh CreateMesh()
	{
		Mesh mesh = new Mesh ();

		int count = 75;
		float size = 0.042f;
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
