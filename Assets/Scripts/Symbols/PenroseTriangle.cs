using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenroseTriangle : MonoBehaviour {

	public Material mat;
	private Vector3 startVertex;
	private Vector3 mousePos;
	static public Transform first, second;

	static Vector3[] verts;
	static int[] tris;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//first.localScale = Vector3.one * 0.02f;
	}

	void OnPostRender() {
		mat = Game.BaseMaterial;
		mat.SetPass(0);
		float half = 0.501f;
		GL.Begin(GL.LINES);
		GL.Color(Color.red);





//		for (int i = 0; i < 4; ++i)
//		{
//			for (int u = 0; u < 200; ++u)
//			{
//				GL.Vertex(first.position + Quaternion.Euler(first.eulerAngles) * (verts[tris[i*200 + u]]));
//			}
//		}


		for (int i = 0; i < tris.Length; ++i)
		{
			if ((i + 2) % 3 == 0)
			{
				i += 1;
				continue;
			}
			GL.Vertex(first.position + (Quaternion.Euler(first.eulerAngles) * (verts[tris[i]]))*first.transform.lossyScale.x);

		}
		GL.End();
	}

	static float height = 10f;
	static float width = 5f;
	static float space = 2f;
	static int sections = 25;

	static public GameObject Create(Camera[] cameras)
	{
		first = Word.GetGameObject (LetterAnimation.CombineMeshes (new Mesh[] {
//			GetMesh(1, new Vector3(-10, 20, -10), Vector3.zero),
//			GetMesh(-1, Vector3.zero, Vector3.up * 90f),
//			GetMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1)
			GetMesh(1, new Vector3(-10, 20, -10), Vector3.zero),
			GetMesh(-1, Vector3.zero, Vector3.up * 90f),
			GetMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1)
		})).transform;
		first.transform.eulerAngles = Vector3.forward * 315 + Vector3.up * 45;
		Mesh mesh = first.GetComponent<MeshFilter> ().mesh;
		verts = mesh.vertices;
		tris = mesh.triangles;

		foreach(Camera camera in cameras)
			camera.gameObject.AddComponent<PenroseTriangle> ();

		return first.gameObject;
	}

	static public Mesh GetMesh(int dir, Vector3 pos, Vector3 euler, int dir2 = 1)
	{
		
		List<int> tris = new List<int> ();
		List<Vector3> verts = new List<Vector3> ();

		float angle = 0;//270f;
		float angleStep = angle / (float)(sections-1);

		float c = 180f / (float)(sections-10);

		for (int i=0; i<=sections; ++i) 
		{
			for(int u=0; u<4; ++u)
			{
				int k = i < 5 ? 0 : (i >= 20 ? sections - 10 : i - 5);
				verts.Add(
					(Quaternion.Euler(euler) *
						new Vector3(
							width * (u%3 == 0 ? -1f : 1f) + Mathf.Cos(Mathf.Deg2Rad * (c * (float)k)) * height * (float)dir, 
							width * (u < 2 ? -1f : 1f) + Mathf.Cos(Mathf.Deg2Rad * (c * (float)k)) * height * ((float)dir2),  
							(space * ((float)i)) - space*(sections*0.5f)) + (dir < 0 ? /*Vector3.one*10f*/Vector3.zero : Vector3.zero)) + pos
					);
			}
		}

		for (int i = 0; i<sections; ++i) 
		{
			int step = (i)*4;

			for(int u=0; u<4; ++u)
			{
				int tmp = u < 3 ? 0 : -4;

				tris.Add(u + 0 + step);
				tris.Add(u + 1 + step + tmp);
				tris.Add(u + 4 + step);


				tris.Add(u + 4 + step);
				tris.Add(u + 1 + step + tmp);

				tris.Add(u + 5 + step + tmp);
			}

		}



		Mesh mesh = new Mesh();

		mesh.SetVertices(verts);
		mesh.SetTriangles(tris, 0);
		//mesh.vertices = verts;
		//mesh.triangles = tris;
		mesh.RecalculateNormals();
		return mesh;
	}
}
