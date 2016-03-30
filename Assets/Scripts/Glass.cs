using UnityEngine;
using System.Collections;


public class Glass : MonoBehaviour
{
	static float thick = 0.01f;
	Vector2 size = Vector2.zero;
	//Texture2D txtr;

	// Use this for initialization

	static public Glass Create(Vector2 s)
	{
		GameObject obj = new GameObject("Glass");
		obj.AddComponent<BoxCollider>().size = new Vector3(s.x, 0.01f, s.y);

		Glass glass = obj.AddComponent<Glass>() as Glass;


		return glass;
	}

	void Start () 
	{
		Vector3 tmp = gameObject.GetComponent<BoxCollider>().size;
		size = new Vector2(tmp.x, tmp.z);

		transform.localEulerAngles += Vector3.right*90f;
		//gameObject.AddComponent<BoxCollider>().size = new Vector3(size.x, 0.01f, size.y);

		//txtr = new Texture2D(512, 512);
	}

	GameObject DrawCleft(Vector3 point)
	{
		float length = Random.Range(0.3f, 1f);

		
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(0, 0, -thick);
		verts[1] = new Vector3(0, 0, thick);
		verts[2] = new Vector3(length, 0, thick);
		verts[3] = new Vector3(length, 0, -thick);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;


		GameObject obj = new GameObject();
		
		obj.AddComponent<MeshFilter>().mesh = mesh;
		obj.AddComponent<MeshRenderer>();
		obj.GetComponent<Renderer>().material.color = Color.black;

		//obj.transform.localScale -= Vector3.up*2;

		GameObject cleft = new GameObject("Cleft");

		obj.transform.parent = cleft.transform;
		obj.transform.localPosition = Vector3.zero;
		obj.transform.localEulerAngles = Vector3.right*90f;
		cleft.transform.localEulerAngles = Vector3.forward*Random.Range(0f, 360f);

		cleft.transform.position = point;
		cleft.transform.parent = transform;

		return cleft;

	}

	void OnCollisionEnter(Collision c)
	{
		if(c.transform.tag == "Ball")
		{
			//Debug.LogWarning(c.contacts[0].point

			for(int i=0; i<5; ++i)
				DrawCleft(c.contacts[0].point);

		}
	}

}
