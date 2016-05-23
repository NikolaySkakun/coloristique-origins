using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CustomMesh 
{
	static public Mesh Circle(int vertsCount = 50)
	{
		Mesh mesh = new Mesh();
		//int vertsCount = 50;
		float radius = 0.5f;
		
		Vector3[] verts = new Vector3[vertsCount + 1];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[vertsCount * 3];
		
		
		verts[vertsCount] = Vector3.zero;
		
		for(int i=0; i<vertsCount; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((360f/vertsCount) * i) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((360f/vertsCount) * i) );
			
			verts[i].y = 0;
			verts[i].x = cos * radius;
			verts[i].z = sin * radius;
			
			tris[i*3] = i;
			tris[i*3 + 1] = vertsCount;
			tris[i*3 + 2] = i<vertsCount-1 ? i+1 : 0;
			
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		//mesh.normals = normals;
		
		mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		//mesh.Optimize();
		
		return mesh;
	}

	static public Mesh Circle(int vertsCount, float radius, Vector2 pos)
	{
		Mesh mesh = new Mesh();

		
		Vector3[] verts = new Vector3[vertsCount + 1];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[vertsCount * 3];
		
		
		verts[vertsCount] = pos;
		
		for(int i=0; i<vertsCount; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((360f/vertsCount) * i) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((360f/vertsCount) * i) );
			
			verts[i].y = 0;
			verts[i].x = cos * radius + pos.x;
			verts[i].z = sin * radius + pos.y;
			
			tris[i*3] = i;
			tris[i*3 + 1] = vertsCount;
			tris[i*3 + 2] = i<vertsCount-1 ? i+1 : 0;
			
		}
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		//mesh.normals = normals;
		
		mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		//mesh.Optimize();
		
		return mesh;
	}


	static Vector3 GetNormal(Vector3 a, Vector3 b, Vector3 c) {
		Vector3 side1 = b - a;
		Vector3 side2 = c - a;
		return Vector3.Cross(side1, side2).normalized;
	}

	static public Mesh CombineMeshes(Mesh[] meshes)
	{
		Mesh mesh = new Mesh ();
		mesh.subMeshCount = meshes.Length;
		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();

		for (int i=0, vertsCount = 0; i<meshes.Length; vertsCount += meshes[i++].vertexCount) 
		{
			List<int> tr = new List<int> ();

			foreach (int t in meshes[i].triangles)
				tr.Add (t + vertsCount);
			//tris.Add(t + vertsCount);

			foreach(Vector3 v in meshes[i].vertices)
				verts.Add(v);

			mesh.SetVertices (verts);
			mesh.SetTriangles (tr.ToArray (), i);
		}

		mesh.SetVertices (verts);
		//		int s = 0;
		//
		//		foreach (Mesh m in meshes)
		//		{
		//			mesh.SetTriangles (m.triangles, s++);
		//		}

		//mesh.SetTriangles (tris, 0);

		return mesh;
	}

	static public Mesh PenroseTriangle(int dir, Vector3 pos, Vector3 euler)
	{
		float height = 10f;
		float width = 5f;
		float space = 2f;
		int sections = 25;
		List<int> tris = new List<int> ();
		List<Vector3> verts = new List<Vector3> ();

		float angle = 0;//270f;
		float angleStep = angle / (float)(sections-1);

		float c = 180f / (float)(sections-10);

		for (int i=0; i<sections; ++i) 
		{
			for(int u=0; u<4; ++u)
			{
				int k = i < 5 ? 0 : (i >= 20 ? sections-1 - 10 : i - 5);
				verts.Add(
					(Quaternion.Euler(euler) *
					new Vector3(
					width * (u%3 == 0 ? -1f : 1f) + Mathf.Cos(Mathf.Deg2Rad * (c * (float)k)) * height * (float)dir, 
					width * (u < 2 ? -1f : 1f) + Mathf.Cos(Mathf.Deg2Rad * (c * (float)k)) * height * (float)dir,  
							space * (float)i) + (dir < 0 ? Vector3.one*10f : Vector3.zero)) + pos
				);
			}
		}

		for (int i = 0; i<sections-1; ++i) 
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

	static public Mesh TestMesh()
	{
		float width = 5f;
		float space = 2.5f;
		int sections = 25;
		List<int> tris = new List<int> ();
		List<Vector3> verts = new List<Vector3> ();

		float angle = 270f;
		float angleStep = angle / (float)(sections-1);

		for (int i=0; i<sections; ++i) 
		{
			for(int u=0; u<4; ++u)
			{
				verts.Add( (Quaternion.Euler(Vector3.forward * angleStep*(float)i)) * (new Vector3(width * (u%3 == 0 ? -1f : 1f), width * (u < 2 ? -1f : 1f), space * (float)i)) );
			}
		}

		for (int i = 0; i<sections-1; ++i) 
		{
			int step = (i)*4;

			for(int u=0; u<4; ++u)
			{
				int tmp = u < 3 ? 0 : -4;

				tris.Add(u + 0 + step);
				tris.Add(u + 1 + step + tmp);
				tris.Add(u + 4 + step);
				tris.Add(u + 1 + step + tmp);
				tris.Add(u + 4 + step);
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

	static public Mesh Test()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[5];

		verts[0] = Vector3.zero;
		verts[1] = new Vector3(0, 0, Ledge.stageHeight);
		verts[2] = new Vector3(Ledge.ledgeWidth, 0, 0);
		verts[3] = new Vector3(Ledge.ledgeWidth, 0, Ledge.stageHeight);
		verts[4] = new Vector3(Ledge.stageHeight + Ledge.ledgeWidth + 0.065f, 0, 0);

		//verts[0] = new Vector3(0f, 0, -1.2f);
		//verts[1] = new Vector3(0.35f, 0, 0);
		//verts[2] = new Vector3(0f, 0, 1.2f);
		
		
		int[] tris = new int[] {
			0, 2, 1,
			1, 2, 3,
			2, 4, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh Pointer()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[7];
		
		verts[0] = new Vector3(0.5f, 0, -0.5f);
		verts[1] = new Vector3(-1.5f, 0, -0.5f);
		verts[2] = new Vector3(-1.5f, 0, 0.5f);
		verts[3] = new Vector3(0.5f, 0, 0.5f);
		
		verts[4] = new Vector3(0.5f, 0, -1.2f);
		verts[5] = new Vector3(1.85f, 0, 0);
		verts[6] = new Vector3(0.5f, 0, 1.2f);
		
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3,
			4, 6, 5
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh ShortPointer()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[3];

		verts[0] = new Vector3(0f, 0, -1.2f);
		verts [1] = new Vector3 (0.35f, 0, 0);//(0.185f, 0, 0); //0.35
		verts[2] = new Vector3(0f, 0, 1.2f);
		

		int[] tris = new int[] {
0, 2, 1
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh ExitDoor()
	{
		float height = 2, width = 1, thick = 0.05f, doorHeight = height*0.1f;

		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[18];

		//left top corner
		verts[0] = new Vector3(-width/2f, 0, height/2f); // p-
		verts[1] = new Vector3(-width/2f, 0, height/2f - thick); // гo
		verts[2] = new Vector3(-width/2f + thick, 0, height/2f); // o-

		//right top corner
		verts[3] = new Vector3(width/2f, 0, height/2f); 
		verts[4] = new Vector3(width/2f, 0, height/2f - thick);
		verts[5] = new Vector3(width/2f - thick, 0, height/2f);

		//left down corner
		verts[6] = new Vector3(-width/2f, 0, -height/2f);
		verts[7] = new Vector3(-width/2f, 0, -height/2f + thick);
		verts[8] = new Vector3(-width/2f + thick, 0, -height/2f);

		//right down corner
		verts[9] = new Vector3(width/2f, 0, -height/2f);
		verts[10] = new Vector3(width/2f, 0, -height/2f + thick);
		verts[11] = new Vector3(width/2f - thick, 0, -height/2f);


		verts[12] = new Vector3(0, 0, height/2f - doorHeight); 
		verts[13] = new Vector3(thick, 0, height/2f - doorHeight - thick);

		verts[14] = new Vector3(0, 0, -height/2f - doorHeight); 
		verts[15] = new Vector3(thick, 0, -height/2f - doorHeight);

		int[] tris = new int[]
		{
			0, 4, 1,
			4, 0, 3,
			6, 0, 8,
			2, 8, 0,
			11, 3, 9,
			3, 11, 5,

			12, 4, 13,
			4, 12, 5,

			13, 14, 12,
			14, 13, 15
		};

		mesh.vertices = verts;
		mesh.triangles = tris;
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh Semicircle(int vertsCount = 25)
	{
		Mesh mesh = Circle(vertsCount*2);

		//Debug.Log(mesh.normals.Length);

		int[] tris = new int[mesh.triangles.Length/2];
		//Vector3[] verts = new Vector3[mesh.vertexCount/2 + 1];

		//verts[verts.Length - 1] = mesh.vertices[mesh.vertexCount-1];
		//Debug.Log(tris.Length);
		for(int i=0; i<tris.Length; ++i)
		{
			tris[i] = mesh.triangles[i];
			//if(i < verts.Length)
			//verts[i] = mesh.vertices[i];
		}
		
		mesh.triangles = tris;
		//mesh.vertices = verts;
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh Quad()
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4] {
			new Vector3(-0.5f, 0, -0.5f),
			new Vector3(-0.5f, 0, 0.5f),
			new Vector3(0.5f, 0, 0.5f),
			new Vector3(0.5f, 0, -0.5f)
		};

		int[] tris = new int[]
		{
			0, 1, 2,
			2, 3, 0
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;

		return mesh;
	}

	static public Mesh CircleBorder(float radius = 0.5f, float thick = 0.02f, int vertsCount = 50)
	{
		Mesh mesh = new Mesh();
		//int vertsCount = 50;
		//float radius = 0.5f;
		Vector3[] verts = new Vector3[vertsCount * 2];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		//Vector2[] uvs = new Vector2[verts.Length];
		
		//verts[vertsCount] = Vector3.zero;
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			
			float tempRadius = i%2==0 ? radius - thick : radius;
			
			verts[i].y = 0.0001f;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			//Debug.Log(i.ToString() + "__" + verts[i].ToString());
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}

			//uvs[i] = new Vector2(verts[i].x, verts[i].z);
		}
		
		//Debug.Log(tris[tris.Length-1]);
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		//mesh.uv = uvs;
		//mesh.uv1 = uvs;
		//mesh.uv2 = uvs;
		//mesh.normals = normals;
		
		//mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		//mesh.Optimize();
		mesh.RecalculateNormals();
		return mesh;
	}

	static public Mesh SemicircleBorder(float radius = 0.5f, float D = 1f, int vertsCount = 25, float angle = 90f)
	{
		Mesh mesh = new Mesh();
		//int vertsCount = 50;
		//float radius = 0.5f;
		
		Vector3[] verts = new Vector3[vertsCount * 2];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		
		//verts[vertsCount] = Vector3.zero;
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );

			float tempRadius = i%2==0 ? radius - Line.height/D : radius;

			verts[i].y = 0.0001f;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
			//Debug.Log(i.ToString() + "__" + verts[i].ToString());
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 1;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+2;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+1;
					tris[i*3 + 2] = i+2;
				}
			}
		}

		mesh.vertices = verts;
		mesh.triangles = tris;

		mesh.RecalculateNormals();
		return mesh;
	}




	static public Mesh ArrowForGravityGun(float radius = 0.5f, float D = 1f, int vertsCount = 25)
	{
		Mesh mesh = new Mesh();
		//int vertsCount = 50;
		//float radius = 0.5f;
		
		Vector3[] verts = new Vector3[vertsCount * 2];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		float angle = 45f;
		//verts[vertsCount] = Vector3.zero;
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			
			//float tempRadius = //i%2==0 ? radius - Line.height/D : radius;
			
			verts[i].y = i%2==0 ? 0.0001f : 0.0001f + Line.height*D;
			verts[i].x = cos * radius;
			verts[i].z = sin * radius;
			
			//Debug.Log(i.ToString() + "__" + verts[i].ToString());
			
			if(i*3 + 2 < tris.Length)
			{
				if(i%2==0)
				{
					tris[i*3] = i + 2;
					tris[i*3 + 1] = i;
					tris[i*3 + 2] = i+1;
				}
				else
				{
					tris[i*3] = i;
					tris[i*3 + 1] = i+2;
					tris[i*3 + 2] = i+1;
				}
			}
		}
		
		//Debug.Log(tris[tris.Length-1]);
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		//mesh.normals = normals;
		
		//mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		//mesh.Optimize();
		mesh.RecalculateNormals();
		return mesh;




		//value /= 100f;
		/*float width = 0.02f, height = 0.25f;
		//float width = 1, height = 5;

		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[6]
		{
			new Vector3(0, 0, -height/2f - (1 - value)/50f),
			new Vector3(-width/2f, 0, -height/2f),
			new Vector3(-width/2f, 0, height/2f),
			new Vector3(0, 0, height/2f + (value)/50f),
			new Vector3(width/2f, 0, height/2f),
			new Vector3(width/2f, 0, -height/2f),
		};
		int[] tris = new int[4*3]
		{
			0, 1, 2,
			0, 2, 3,
			0, 3, 4,
			0, 4, 5
		};

		mesh.vertices = verts;
		mesh.triangles = tris;


		return mesh;*/
	}

	static public Mesh CurveWall(int vertsCount = 25, float angle = 90f, float radius = 0.5f, bool outside = false)
	{
		Mesh mesh = new Mesh();
		//int vertsCount = 50;
		//float radius = 0.5f;
		
		Vector3[] verts = new Vector3[vertsCount * 2];
		//Vector3[] normals = new Vector3[vertsCount + 1];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		
		//verts[vertsCount] = Vector3.zero;
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			
			verts[i].y = i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * radius;
			verts[i].z = sin * radius;

			//Debug.Log(i.ToString() + "__" + verts[i].ToString());

			if(i*3 + 2 < tris.Length)
			{
				if(!outside)
				{
					if(i%2==0)
					{
						tris[i*3] = i + 1;
						tris[i*3 + 1] = i;
						tris[i*3 + 2] = i+2;
					}
					else
					{
						tris[i*3] = i;
						tris[i*3 + 1] = i+1;
						tris[i*3 + 2] = i+2;
					}
				}
				else
				{
					if(i%2==0)
					{
						tris[i*3 + 1] = i + 1;
						tris[i*3] = i;
						tris[i*3 + 2] = i+2;
					}
					else
					{
						tris[i*3 + 1] = i;
						tris[i*3 ] = i+1;
						tris[i*3 + 2] = i+2;
					}
				}


			}
		}

		//Debug.Log(tris[tris.Length-1]);
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		//mesh.normals = normals;
		
		//mesh.RecalculateNormals();
		//mesh.RecalculateBounds();
		//mesh.Optimize();

		mesh.RecalculateNormals();
		return mesh;
	}

}
