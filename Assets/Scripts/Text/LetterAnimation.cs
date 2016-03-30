using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LetterAnimation : MonoBehaviour 
{
	Mesh originalMesh;
	MeshFilter meshFilter;

	static int vertsCount = 38;
	static public float radius = 0.5f;

	static public float thick = 10f;

	float drawTime = 1.5f;

	float progress = 0f;

	bool draw = false;

	void Start () 
	{

		meshFilter = GetComponent<MeshFilter>();

		originalMesh = meshFilter.mesh;

		meshFilter.mesh = null;

		//yield return new WaitForSeconds (1f);
		draw = true;

		//gameObject.GetComponent<Renderer>().material.color = Game.ReverseColor(gameObject.GetComponent<Renderer>().material.color);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (draw) {
			progress += Time.deltaTime / drawTime;

			if(progress < 0.12f)
				progress -= Time.deltaTime/drawTime*0.8f;

			if (progress > 1f) {
				//meshFilter.mesh = originalMesh;
			
				//gameObject.GetComponent<Renderer>().material.color = Game.ReverseColor(gameObject.GetComponent<Renderer>().material.color);
				this.enabled = false;

				return;
			}

			meshFilter.mesh = (Mesh)typeof(LetterAnimation).GetMethod (gameObject.name.ToUpper ()).Invoke (null, new object[]{progress}); //O(progress);
		}
	}

	static public Mesh Q(float drawProgress)
	{
		return CombineMeshes (new Mesh[]{ O (drawProgress), I_ (new Vector2(drawProgress, 1f), -Vector3.forward * radius + Vector3.forward * radius / thick - Vector3.right * radius, false) });
	}

	static public Mesh T(float drawProgress)
	{
//		GameObject l = L (), i = I_ ();//, i2 = I_();
//		i.transform.parent = l.transform;
//		//i2.transform.parent = l.transform;
//		
//		i.transform.localEulerAngles = Vector3.up * 90;
//		i.transform.localScale -= Vector3.right*(radius/3f);

		return CombineMeshes (new Mesh[]{L (drawProgress), I__(new Vector2(drawProgress, 1f - radius/3f), Vector3.zero)});
	}

	static public Mesh QuarterCircle(Vector2 pos, float r = 1f, float angle = 90f, float thick = 0.0f)
	{
		int vertsCount_ = 20;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount_/2 * 2];
		int[] tris = new int[2*(vertsCount_/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = i;//(i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*(((angle/2f)/(vertsCount_/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*(((angle/2f)/(vertsCount_/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
//			float tempRadius = i%2==0 ? radius*r - radius/(Word.thick*0.5f*thick) - radius*thick : radius*r + radius*thick;
			float tempRadius = i%2==0 ? radius*r - (radius/(Word.thick*0.5f))*thick : radius*r;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius + pos.x;
			verts[i].z = sin * tempRadius + pos.y;
			
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
		
		return mesh;
	}

	static public Mesh R(float drawProgress)
	{

		Mesh q = QuarterCircle (new Vector2(radius/5f, radius/1.8f - (radius/10f)*7f), 0.8f, 90, drawProgress);


		return CombineMeshes (
			new Mesh[]{
			q, 
			I_(new Vector2(drawProgress, 0.6f), -Vector3.right * 2 * radius/5f + Vector3.forward*radius/1.8f),
			I__(new Vector2(drawProgress, 0.25f), Vector3.right*radius - Vector3.right*radius/10f - Vector3.forward*(radius - radius/20f) + Vector3.forward*radius/1.8f, false)
		});

	}

	static public Mesh S(float drawProgress)
	{
		float r = 0.5f + (0.05f / (thick / 10f));
		return CombineMeshes(new Mesh[]{ U_ (drawProgress, new Vector2(-radius/4f, radius*r - radius/thick), r, true),
			U__ (drawProgress, new Vector2(-radius/4f, radius*r - radius/thick), r, true)});

//		float r = 0.5f + (0.05f/(thick/10f));
//		GameObject[] u = new GameObject[]{ U_ (r), U_ (r) };
//		GameObject s = new GameObject();
//		u[0].transform.GetChild(0).GetComponent<MeshFilter>().mesh = null;
//		Object.Destroy(u[0].transform.GetChild(0).gameObject);
//		u[0].transform.localEulerAngles = Vector3.up * 90;
//		u[1].transform.localEulerAngles = -Vector3.up * 90;
//		
//		u[0].transform.parent = u[1].transform.parent = s.transform;
//		
//		u[0].transform.localPosition = new Vector3(radius*r - radius/thick, 0, radius/4f);
//		u[1].transform.localPosition = new Vector3(-(radius*r - radius/thick), 0, -radius/4f);
//		
//		
//		//u[0].transform.GetChild(0).localScale = new Vector3(0.2f, 1, 1);
//		u[1].transform.GetChild(0).localScale = new Vector3(0.25f, 1, 1);
//		u[1].transform.GetChild(0).localPosition = new Vector3((radius*0.25f), u[1].transform.GetChild(0).localPosition.y, u[1].transform.GetChild(0).localPosition.z);//-= Vector3.right * (radius*0.25f);//0.001f;
//		
//		u[0].transform.GetChild(1).localPosition = new Vector3(radius/2f, 0, radius*r - radius/thick);
//		u[1].transform.GetChild(1).localPosition = new Vector3(radius/2f, 0, radius*r - radius/thick);
//		
//		return s;
	}

	static public Mesh U__(float drawProgress, Vector2 pos, float r = 1f, bool center = false)
	{
		//float r = 1f;
		
		center = true;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? 
				( center ? (radius*r - (radius/(thick*0.5f))*0.5f - ((radius/(thick*0.5f))*0.5f)*drawProgress) : radius*r - (radius/(thick*0.5f))*drawProgress )
					: ( center ? (radius*r - (radius/(thick*0.5f))*0.5f + ((radius/(thick*0.5f))*0.5f)*drawProgress) : radius*r );
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius + pos.x;
			verts[i].z = sin * tempRadius + pos.y;
			
			verts[i] = Quaternion.Euler(90, -90, 0) * new Vector3(verts[i].x, verts[i].z);
			
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
		
		return CombineMeshes (
			new Mesh[]{
			mesh, 
			I_WithoutDot(new Vector2(drawProgress, 0.5f), new Vector3(-radius + radius/thick, 0, (radius/thick)*2.5f), 90f)
		});
		
	}


	static public Mesh U_(float drawProgress, Vector2 pos, float r = 1f, bool center = false)
	{
		//float r = 1f;

		center = true;
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? 
				( center ? (radius*r - (radius/(thick*0.5f))*0.5f - ((radius/(thick*0.5f))*0.5f)*drawProgress) : radius*r - (radius/(thick*0.5f))*drawProgress )
					: ( center ? (radius*r - (radius/(thick*0.5f))*0.5f + ((radius/(thick*0.5f))*0.5f)*drawProgress) : radius*r );
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius + pos.x;
			verts[i].z = sin * tempRadius + pos.y;

			verts[i] = Quaternion.Euler(90, 90, 0) * new Vector3(verts[i].x, verts[i].z);
			
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
		
		return CombineMeshes (
			new Mesh[]{
			mesh, 
			I_WithoutDot(new Vector2(drawProgress, 0.5f), new Vector3(radius - radius/thick, 0, (-radius/thick)*2.5f), 90f), 
			I_WithoutDot(new Vector2(drawProgress, 0.25f), Vector3.zero, 90f)
		});
		
	}

	static public Mesh U(float drawProgress)
	{
		//return U_ (drawProgress, 1, true);
		float r = 1f;

		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount/2 * 2];
		int[] tris = new int[2*(vertsCount/2 - 1) * 3];
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i+verts.Length/2-1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((90f/(vertsCount/2-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? 
				radius*r - (radius/(thick*0.5f))*drawProgress 
					: radius*r ;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
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
		
		return CombineMeshes (
			new Mesh[]{
			mesh, 
			I_(new Vector2(drawProgress, 0.5f), new Vector3(radius*r/2f, 0, radius*r - radius/thick)), 
			I_(new Vector2(drawProgress, 0.5f), new Vector3(radius*r/2f, 0, -radius*r + radius/thick), false)
		});
		
	}

	static public Mesh I_WithoutDot(Vector2 scale, Vector3 pos, float angle = 0f, bool center = true)
	{
		Mesh mesh = new Mesh();
		
		List<Vector3> v = new List<Vector3> ();
		
		Vector3[] verts = new Vector3[4];
		Vector3 p = new Vector3 (pos.x, 0, pos.z);

		Vector3 rot = Vector3.up * angle;

		if (center) 
		{
			verts [0] = Quaternion.Euler (rot) * new Vector3 (-radius * scale.y, 0, -(radius / thick) * scale.x) + p;
			verts [1] = Quaternion.Euler (rot) * new Vector3 (-radius * scale.y, 0, (radius / thick) * scale.x) + p;
			verts [2] = Quaternion.Euler (rot) * new Vector3 (radius * scale.y, 0, (radius / thick) * scale.x) + p;
			verts [3] = Quaternion.Euler (rot) * new Vector3 (radius * scale.y, 0, -(radius / thick) * scale.x) + p;
		}
		else 
		{
			float y = (radius / thick) * scale.x*2f;
			verts [0] = Quaternion.Euler (rot) * new Vector3 (-radius * scale.y + y, 0, (radius / thick) * 1f - (radius / thick) * scale.x*2f) + p;
			verts [1] = Quaternion.Euler (rot) * new Vector3 (-radius * scale.y + y*1.1f, 0, (radius / thick) * 1f) + p;
			verts [2] = Quaternion.Euler (rot) * new Vector3 (radius * scale.y - (radius / thick)/5f, 0, (radius / thick) * 1f) + p;
			verts [3] = Quaternion.Euler (rot) * new Vector3 (radius * scale.y - (radius / thick)/5f, 0, (radius / thick) * 1f - (radius / thick) * scale.x*2f) + p;
		}

//		bool left = true;
//		float temp = left ? (radius / thick) - (2f * radius / thick) * scale.x : -(radius / thick) + (2f * radius / thick) * scale.x ;
//		float tmp = left ? 1.0f : -1.0f;
//		
//		verts [0] = new Vector3 (-radius * scale.y, 0, temp) + p;
//		verts [1] = new Vector3 (-radius * scale.y, 0, (radius / thick)*tmp) + p;
//		verts [2] = new Vector3 (radius * scale.y, 0, (radius / thick)*tmp) + p;
//		verts [3] = new Vector3 (radius * scale.y, 0, temp) + p;
//
//		for (int i=0; i<verts.Length; ++i)
//			verts [i] = Quaternion.Euler (rot) * verts[i];


//		verts[0] = Quaternion.Euler(0, 0, angle).eulerAngles + new Vector3(-radius * scale.y, 0, -(radius/thick) * scale.x) + p;
//		verts[1] = Quaternion.Euler(0, 0, angle).eulerAngles + new Vector3(-radius * scale.y, 0, (radius/thick) * scale.x) + p;
//		verts[2] = Quaternion.Euler(0, 0, angle).eulerAngles + new Vector3(radius * scale.y, 0, (radius/thick) * scale.x) + p;
//		verts[3] = Quaternion.Euler(0, 0, angle).eulerAngles + new Vector3(radius * scale.y, 0, -(radius/thick) * scale.x) + p;
		
		int[] tris = new int[6];
		tris [0] = 0;
		tris [1] = 1;
		tris [2] = 2;
		tris [3] = 0;
		tris [4] = 2;
		tris [5] = 3;
		
		
		mesh.vertices = verts;
		mesh.triangles = tris;

		return mesh;
	}

	static public Mesh I_(Vector2 scale, Vector3 pos, bool left = true)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		Vector3 p = new Vector3 (pos.x, 0, pos.z);

		float temp = left ? (radius / thick) - (2f * radius / thick) * scale.x : -(radius / thick) + (2f * radius / thick) * scale.x ;
		float tmp = left ? 1.0f : -1.0f;

		verts [0] = new Vector3 (-radius * scale.y, 0, temp) + p;
		verts [1] = new Vector3 (-radius * scale.y, 0, (radius / thick)*tmp) + p;
		verts [2] = new Vector3 (radius * scale.y, 0, (radius / thick)*tmp) + p;
		verts [3] = new Vector3 (radius * scale.y, 0, temp) + p;

//		verts[0] = new Vector3(-radius, 0, -(radius/thick)*drawProgress);
//		verts[1] = new Vector3(-radius, 0, (radius/thick)*drawProgress);
//		verts[2] = new Vector3(radius, 0, (radius/thick)*drawProgress);
//		verts[3] = new Vector3(radius, 0, -(radius/thick)*drawProgress);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return mesh;
	}

	static Mesh I__(Vector2 scale, Vector3 pos, bool center = true)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];
		Vector3 p = new Vector3 (pos.x, 0, pos.z);
		
		verts [0] = new Vector3 (center ? (-(radius / thick) * scale.x) : (radius / thick -2f* (radius/thick)*scale.x), 0, -radius * scale.y) + p;
		verts [1] = new Vector3 ((radius / thick) * (center ?  scale.x : 1f), 0, -radius * scale.y) + p;
		verts [2] = new Vector3 ((radius / thick) * (center ?  scale.x : 1f), 0, radius * scale.y) + p;
		verts [3] = new Vector3 (center ? (-(radius / thick) * scale.x) : (radius / thick -2f* (radius/thick)*scale.x), 0, radius * scale.y) + p;
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return mesh;
	}

	static public Mesh L(float drawProgress)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[4];

		//float thick = 10f;//5000f - 4990f * drawProgress;
		
		verts[0] = new Vector3(-radius, 0, -(radius/thick)*drawProgress);
		verts[1] = new Vector3(-radius, 0, (radius/thick)*drawProgress);
		verts[2] = new Vector3(2f*radius, 0, (radius/thick)*drawProgress);
		verts[3] = new Vector3(2f*radius, 0, -(radius/thick)*drawProgress);
		
		int[] tris = new int[] {
			0, 1, 2,
			0, 2, 3
		};
		
		mesh.vertices = verts;
		mesh.triangles = tris;
		
		return mesh;
	}

	static public Mesh CombineMeshes(Mesh[] meshes)
	{
		Mesh mesh = new Mesh ();
		List<Vector3> verts = new List<Vector3> ();
		List<int> tris = new List<int> ();

		for (int i=0, vertsCount = 0; i<meshes.Length; vertsCount += meshes[i++].vertexCount) 
		{
			foreach(int t in meshes[i].triangles)
				tris.Add(t + vertsCount);

			foreach(Vector3 v in meshes[i].vertices)
				verts.Add(v);
		}

		mesh.SetVertices (verts);
		mesh.SetTriangles (tris, 0);

		return mesh;
	}

	static public Mesh I(float drawProgress)
	{
		Mesh mesh = new Mesh();

		Mesh circle = CustomMesh.Circle(20, (radius/2f/(thick/2.222f))*drawProgress, Vector2.right * radius*1.4f);

		List<Vector3> v = new List<Vector3> ();

		Vector3[] verts = new Vector3[4];
		
		verts[0] = new Vector3(-radius, 0, -(radius/thick)*drawProgress);
		verts[1] = new Vector3(-radius, 0, (radius/thick)*drawProgress);
		verts[2] = new Vector3(radius, 0, (radius/thick)*drawProgress);
		verts[3] = new Vector3(radius, 0, -(radius/thick)*drawProgress);

		int[] tris = new int[6];
		tris [0] = 0;
		tris [1] = 1;
		tris [2] = 2;
		tris [3] = 0;
		tris [4] = 2;
		tris [5] = 3;

		
		mesh.vertices = verts;
		mesh.triangles = tris;
//
//		int[] t = new int[circle.triangles.Length + tris.Length];
//
//
//		for (int i=0; i<verts.Length; ++i) {
//			v.Add(verts[i]);
//		}
//
//		for (int i=0; i<circle.vertices.Length; ++i) {
//			v.Add(circle.vertices[i]);
//		}
//
//
//		for (int i=0; i<tris.Length; ++i) {
//			t[i] = tris[i];
//		}
//		
//		for (int i=tris.Length; i<t.Length; ++i) {
//			t[i] = circle.triangles[i-tris.Length] + verts.Length;
//		}

//		Mesh m = new Mesh ();
//		m.subMeshCount = 2;
//		m.SetVertices (v);
//		m.triangles = t;
//
//
//		return m;

		return CombineMeshes (new Mesh[]{mesh, circle});
	}

	static public Mesh O(float drawProgress)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];

		//float thick = 10f;//5000f - 4990f * drawProgress;
		
		for(int i=0; i<verts.Length; ++i)
		{
			float cos = Mathf.Cos( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((180f/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
			
			float tempRadius = i%2==0 ? radius - (radius/(thick*0.5f))*drawProgress : radius;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = sin * tempRadius;
			
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
		
		return mesh;
	}

	//static public Mesh O(float drawProgress)
//	{
//		Mesh mesh = new Mesh();
//		Vector3[] verts = new Vector3[vertsCount * 2];
//		int[] tris = new int[2*(vertsCount - 1) * 3];
//
//		float angle = 180f * drawProgress;
//
//		for(int i=0; i<verts.Length; ++i)
//		{
//			float cos = Mathf.Cos( Mathf.Deg2Rad*(180  - angle + (angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
//			float sin = Mathf.Sin( Mathf.Deg2Rad*(180 -angle + (angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );
//			
//			float tempRadius = i%2==0 ? radius - radius/5f : radius;
//			
//			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
//			verts[i].x = cos * tempRadius;
//			verts[i].z = sin * tempRadius;
//			
//			if(i*3 + 2 < tris.Length)
//			{
//				if(i%2==0)
//				{
//					tris[i*3] = i + 1;
//					tris[i*3 + 1] = i;
//					tris[i*3 + 2] = i+2;
//				}
//				else
//				{
//					tris[i*3] = i;
//					tris[i*3 + 1] = i+1;
//					tris[i*3 + 2] = i+2;
//				}
//			}
//		}
//		
//		mesh.vertices = verts;
//		mesh.triangles = tris;
//
//		return mesh;
//	}

	static public Mesh C(float drawProgress)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];
		
		//float thick = 10f;//5000f - 4990f * drawProgress;
		
		float angle = 135f;
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i-verts.Length/6);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			float tempRadius = i%2==0 ? radius - (radius/(thick*0.5f))*drawProgress : radius;
			
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius;
			verts[i].z = (-radius/4f) + sin * tempRadius;
			
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
		
		return mesh;
	}



	static public Mesh E(float drawProgress)
	{
		Mesh mesh = new Mesh();
		Vector3[] verts = new Vector3[vertsCount * 2];
		int[] tris = new int[2*(vertsCount - 1) * 3];

		float angle = 162f;
		
		for(int i=0; i<verts.Length; ++i)
		{
			int tmp = (i-verts.Length/4 + 1);
			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
			
			//float tempRadius = i%2==0 ? radius - radius/5f : radius;
			float tempRadius = i%2==0 ? radius - (radius/(thick*0.5f))*drawProgress : radius;
			
			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
			verts[i].x = cos * tempRadius + (i==0 ? cos * tempRadius/3.5f : 0f)*drawProgress;
			verts[i].z = sin * tempRadius;
			
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

		return CombineMeshes(new Mesh[]{ 
			mesh,
			I_WithoutDot(new Vector2(drawProgress, 1f), new Vector3(radius/thick, 0, 0), 90f, false)
		});
		
//		GameObject e = GetGameObject(mesh);
//		GameObject i_ = I_();
//		
//		i_.transform.parent = e.transform;
//		i_.transform.localEulerAngles = Vector3.up * 90;
//		i_.transform.localScale -= 2*Vector3.right/100f;
//		i_.transform.localPosition += Vector3.right*radius/10f;
		
		//return e;

//		//float thick = 10f;//5000f - 4990f * drawProgress;
//		
//		float angle = 162f;
//		
//		for(int i=0; i<verts.Length; ++i)
//		{
//			int tmp = (i-verts.Length/6);
//			float cos = Mathf.Cos( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
//			float sin = Mathf.Sin( Mathf.Deg2Rad*((angle/(vertsCount-1)) * (tmp%2==0 ? tmp : tmp-1)) );
//			
//			float tempRadius = i%2==0 ? radius - (radius/(thick*0.5f))*drawProgress : radius;
//			
//			
//			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
//			verts[i].x = cos * tempRadius;
//			verts[i].z = (-radius/4f) + sin * tempRadius;
//			
//			if(i*3 + 2 < tris.Length)
//			{
//				if(i%2==0)
//				{
//					tris[i*3] = i + 1;
//					tris[i*3 + 1] = i;
//					tris[i*3 + 2] = i+2;
//				}
//				else
//				{
//					tris[i*3] = i;
//					tris[i*3 + 1] = i+1;
//					tris[i*3 + 2] = i+2;
//				}
//			}
//		}
//		
//		mesh.vertices = verts;
//		mesh.triangles = tris;
//		
//		return mesh;
	}



//	static public Mesh C(float drawProgress)
//	{
//		Mesh mesh = new Mesh();
//		Vector3[] verts = new Vector3[vertsCount * 2];
//		int[] tris = new int[2*(vertsCount - 1) * 3];
//		
//		float angle = 135f * drawProgress;
//		
//		for(int i=0; i<verts.Length; ++i)
//		{
//			int tmp = (i-verts.Length/6);
//			float cos = Mathf.Cos( Mathf.Deg2Rad*(-44f + (angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );//* (tmp%2==0 ? tmp : tmp-1)) );
//			float sin = Mathf.Sin( Mathf.Deg2Rad*(-44f + (angle/(vertsCount-1)) * (i%2==0 ? i : i-1)) );//* (tmp%2==0 ? tmp : tmp-1)) );
//			
//			float tempRadius = i%2==0 ? radius - radius/5f : radius;
//			
//			
//			verts[i].y = 0;//i%2==0 ? -0.5f : 0.5f;
//			verts[i].x = cos * tempRadius;
//			verts[i].z = (-radius/4f) + sin * tempRadius;
//			
//			if(i*3 + 2 < tris.Length)
//			{
//				if(i%2==0)
//				{
//					tris[i*3] = i + 1;
//					tris[i*3 + 1] = i;
//					tris[i*3 + 2] = i+2;
//				}
//				else
//				{
//					tris[i*3] = i;
//					tris[i*3 + 1] = i+1;
//					tris[i*3 + 2] = i+2;
//				}
//			}
//		}
//		
//		mesh.vertices = verts;
//		mesh.triangles = tris;
//
//		
//		return mesh;
//	}

}
