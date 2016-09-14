using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenroseTriangle
{

	public Material mat;
	private Vector3 startVertex;
	private Vector3 mousePos;
	static public Transform first, second;

	static Vector3[] verts;
	static int[] tris;

	static float height = 10f;
	static float width = 5f;
	static float space = 2f;
	static int sections = 25;

	static GameObject GetContour()
	{
		GameObject obj = Word.GetGameObject (LetterAnimation.CombineMeshes (new Mesh[] {
			//			GetMesh(1, new Vector3(-10, 20, -10), Vector3.zero),
			//			GetMesh(-1, Vector3.zero, Vector3.up * 90f),
			//			GetMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1)

			GetContourMesh(1, new Vector3(-10, 20, -10), Vector3.zero, 1, 0, false),
			GetContourMesh(1, new Vector3(-10, 20, -10), Vector3.zero, 1, 2, true, false),
			GetContourMesh(1, new Vector3(-10, 20, -10), Vector3.zero, 1, 3),
			GetContourMesh(1, new Vector3(-10, 20, -10), Vector3.zero, 1, 1, false, false),


			GetContourMesh(-1, Vector3.zero, Vector3.up * 90f, 1, 0),
			GetContourMesh(-1, Vector3.zero, Vector3.up * 90f, 1, 2, false, false),
			GetContourMesh(-1, Vector3.zero, Vector3.up * 90f, 1, 1, false),
			GetContourMesh(-1, Vector3.zero, Vector3.up * 90f, 1, 3, true, false),

			GetContourMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1, 0, false, false),
			GetContourMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1, 2),
			GetContourMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1, 3, false),
			GetContourMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1, 1, true, false)
		
		}));
		obj.transform.eulerAngles = Vector3.forward * 315 + Vector3.up * 45;
		Mesh mesh = obj.GetComponent<MeshFilter> ().mesh;
		verts = mesh.vertices;
		tris = mesh.triangles;


		//obj.GetComponent<
		//foreach(Camera camera in cameras)
		//	camera.gameObject.AddComponent<PenroseTriangle> ();

		return obj;
	}

	static public GameObject Create(Camera[] cameras)
	{
		first = Word.GetGameObject (LetterAnimation.CombineMeshes (new Mesh[] {
//			GetMesh(1, new Vector3(-10, 20, -10), Vector3.zero),
//			GetMesh(-1, Vector3.zero, Vector3.up * 90f),
//			GetMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1)
			GetMesh(1, new Vector3(-10, 20, -10), Vector3.zero),
			GetMesh(-1, Vector3.zero, Vector3.up * 90f),
			GetMesh(1, new Vector3(10, 10, -20), Vector3.right * 270f, -1)
		}), Obj.Colour.WHITE).transform;
		first.transform.eulerAngles = Vector3.forward * 315 + Vector3.up * 45;
		Mesh mesh = first.GetComponent<MeshFilter> ().mesh;
		verts = mesh.vertices;
		tris = mesh.triangles;



		//foreach(Camera camera in cameras)
		//	camera.gameObject.AddComponent<PenroseTriangle> ();

		GetContour ().transform.SetParent(first);

		return first.gameObject;
	}

	static float spaceForContour = 0;

	static public Mesh GetContourMesh(int dir, Vector3 pos, Vector3 euler, int dir2 = 1, int index = 0, bool up = true, bool down = true)
	{
		width = 5.1f;

		int mainCorner, oppositeCorner, leftCorner, rightCorner;
		mainCorner = index;
		if (index == 0)
		{
			oppositeCorner = 2;
			leftCorner = 1;
			rightCorner = 3;
		} else if (index == 1)
		{
			oppositeCorner = 3;
			leftCorner = 0;
			rightCorner = 2;
		} else if (index == 2)
		{
			oppositeCorner = 0;
			leftCorner = 3;
			rightCorner = 1;
		} else
		{
			oppositeCorner = 1;
			leftCorner = 2;
			rightCorner = 0;
		}

		List<int> tris = new List<int> ();
		List<Vector3> verts = new List<Vector3> ();

		float angle = 0;//270f;
		float angleStep = angle / (float)(sections-1);

		float c = 180f / (float)(sections-10);

		Vector3 lastVert = Vector3.zero;

		float plus = 0.15f;
		width += plus;

		for (int i=0; i<=sections; ++i) 
		{
			for(int u=0; u<4; ++u)
			{

					
					int k = i < 5 ? 0 : (i >= 20 ? sections - 10 : i - 5);

				spaceForContour = (space * ((float)i)) - space * (sections * 0.5f);
					verts.Add (
						(Quaternion.Euler (euler) *

						new Vector3 (
							(width - (u == leftCorner || u==oppositeCorner ? (2*width - plus*2.5f) : 0) ) * (u % 3 == 0 ? -1f : 1f) + Mathf.Cos (Mathf.Deg2Rad * (c * (float)k)) * height * (float)dir, 
							(width - (u == rightCorner || u==oppositeCorner ? (2*width - plus*2.5f) : 0) ) * (u < 2 ? -1f : 1f) + Mathf.Cos (Mathf.Deg2Rad * (c * (float)k)) * height * ((float)dir2),  
							spaceForContour
						) + (dir < 0 ? /*Vector3.one*10f*/Vector3.zero : Vector3.zero)
					
						) + pos
					);

			}
		}

		for (int i = (up ? 0 : 5); i<sections - (down ? 0 : 5); ++i) 
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
