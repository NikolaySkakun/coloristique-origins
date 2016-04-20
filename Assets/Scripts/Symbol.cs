using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Symbol : MonoBehaviour 
{
	public enum Type
	{
		MOBIUS_STRIP,
		PENROSE_TRIANGLE,
		CARBON,
		UNITY
	};

	public Symbol.Type type;

	int index = 0;
	public bool ts = false;
	public bool test = false;
	bool withSphere = false;

	public float angle = 0f;
	public float thick = 0.05f;
	public float radius = 1f;

	GameObject sphere;

	void Start()
	{
//		if (withSphere)
//		{
//			sphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
//			sphere.transform.localScale = Vector3.one * 0.1f;
//			//sphere.transform.localPosition = Vector3.up;
//
//			(sphere.GetComponent<Renderer> ().material = Game.BaseMaterial).color = Color.white;
//
//			GameObject o = new GameObject ("S");
//			sphere.transform.parent = o.transform;
//			sphere.transform.localPosition = Vector3.up * 0.1f;
//			sphere = o;
//		}
//		if(withSphere)
//		Invoke ("NextBone", 1f);

		Debug.LogWarning (Vector3.Distance (transform.GetChild (0).position, transform.GetChild (1).position));
	}

	int boneIndex = 0;

	void NextBone()
	{
		++boneIndex;

		if (boneIndex == transform.childCount)
			boneIndex = 0;

		sphere.transform.parent = transform.GetChild (boneIndex).GetChild (0);
		sphere.transform.localEulerAngles = Vector3.zero;


		Invoke ("NextBone", 1f/2f);
	}

	void FixedUpdate()
	{
//		if (index == transform.childCount)
//			index = 0;
//
		Create(Type.MOBIUS_STRIP, gameObject, angle += Time.fixedDeltaTime * 60f);

		//Debug.LogWarning (GetComponent<SkinnedMeshRenderer> ().sharedMesh.vertices [0]);

//		for (int i = 0; i < transform.childCount; ++i)
//		{
//			//transform.GetChild (i).localEulerAngles = transform.GetChild (i).localEulerAngles + Vector3.right * Time.fixedDeltaTime;
//		}

//		if(withSphere)
//		{
//			sphere.transform.position = Vector3.MoveTowards (
//				sphere.transform.position, 
//				transform.GetChild (boneIndex).position,// + transform.GetChild (boneIndex).GetChild(0).up*0.3f, 
//				0.06979098f*Time.fixedDeltaTime*2f);
//			//sphere.transform.position = Vector3.Lerp (sphere.transform.position, transform.GetChild (boneIndex).position, 0.001f);
//		}

		//Transform t = transform.GetChild (index++);
		//t.localEulerAngles = new Vector3 (t.localEulerAngles.x + 1, t.localEulerAngles.y, 0);


		//transform.GetChild(index++).localEulerAngles += Vector3.right;
	}


	void Create(Symbol.Type t, GameObject obj, float angle)
	{
		//GameObject obj = new GameObject (t.ToString ());
		SkinnedMeshRenderer skin = obj.GetComponent<SkinnedMeshRenderer> ();
		//skin.material = new Material(Shader.Find("Base"));
		//Mesh mesh = new Mesh ();

		List<Transform> bones = new List<Transform> ();
		List<BoneWeight> weights = new List<BoneWeight> ();

		switch (t)
		{

		case Type.MOBIUS_STRIP:
			{
				Mesh mesh = skin.sharedMesh;
				//float radius = 1f;

				//List<Vector3> verts = new List<Vector3> ();
				//List<int> tris = new List<int> ();

				for (int i = 0, u = 0; i <= 360; i += 5, u += 2)
				{
					//float angle = i * (90f / 360f);
					//Quaternion q = Quaternion.Euler (angle, 0, 0);

//					if (ts)
//					{
//						if (test)
//						{
//							verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, thick - thick / 10f));
//							verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, thick));
//						} else
//						{
//							verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, -thick));
//							verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, -thick + thick / 10f));
//						}
//					} else
//					{
//						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, -thick));
//						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, thick));
//					}
//
//					if (i != 0)
//					{
//						tris.Add (u - 2);
//						tris.Add (u - 1);
//						tris.Add (u);
//
//						tris.Add (u - 1);
//						tris.Add (u);
//						tris.Add (u + 1);
//
//
//					}

					Transform b = obj.transform.GetChild (u / 2).GetChild(0);//new GameObject("Bone").transform;
					//b.parent = obj.transform;
					//b.localPosition = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, 0);
					b.localEulerAngles = Vector3.forward * (-i);
					bones.Add (b);

//					BoneWeight w = new BoneWeight ();
//					w.boneIndex0 = u/2;
//					w.weight0 = 1;
//
//					BoneWeight w2 = new BoneWeight ();
//					w2.boneIndex0 = u/2;
//					w2.weight0 = 1;
//
//					weights.Add (w);
//					weights.Add (w2);

				}

				//mesh.SetVertices (verts);
				//mesh.SetTriangles (tris, 0);
				//mesh.boneWeights = weights.ToArray ();

				//skin.rootBone = obj.transform;
				//skin.bones = bones.ToArray ();


				Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];

				for (int i = 0; i < bones.Count; ++i)
				{
					bindPoses[i] = bones[i].worldToLocalMatrix * obj.transform.localToWorldMatrix;
				}

				mesh.bindposes = bindPoses;

				skin.sharedMesh = mesh;

				float a = 0;
				foreach (Transform bone in bones)
				{
					//bone.RotateAround (Vector3.right, a + angle);
					bone.localEulerAngles += Vector3.right * (a + angle);
					a += 5;
				}
				//obj = Word.GetGameObject (mesh);

			}
			break;

		default:
			obj = new GameObject ();
			break;

		}



		//Symbol symbol = obj.AddComponent<Symbol> ();
		//symbol.type = t;

		//return symbol;
	}




	public static Symbol Create(Symbol.Type t, float thick = 0.05f, float radius = 1f, Obj.Colour color = Obj.Colour.WHITE, float border = 0f)
	{
		GameObject obj = new GameObject (t.ToString ());
		SkinnedMeshRenderer skin = obj.AddComponent<SkinnedMeshRenderer> ();
		skin.material = new Material(Shader.Find("Base"));
		skin.material.color = Game.GetColor (color);
		//Mesh mesh = new Mesh ();

		List<Transform> bones = new List<Transform> ();
		List<BoneWeight> weights = new List<BoneWeight> ();

		switch (t)
		{
			
		case Type.MOBIUS_STRIP:
			{


				Mesh mesh = new Mesh ();
				//float radius = 1f;

				List<Vector3> verts = new List<Vector3> ();
				List<int> tris = new List<int> ();

				for (int i = 0, u = 0; i <= 360; i += 5, u += 2)
				{
					//float angle = i * (90f / 360f);
					//Quaternion q = Quaternion.Euler (angle, 0, 0);

					if (border == 0)
					{
						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, -thick));
						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, thick));
					} 
					else
					{
						float c = border > 0 ? 1 : -1;
						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, thick * c));
						verts.Add (new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, c*thick - border));
					}

					if (i != 0)
					{
						tris.Add (u - 2);
						tris.Add (u - 1);
						tris.Add (u);

						tris.Add (u - 1);
						tris.Add (u);
						tris.Add (u + 1);


					}

					Transform b = new GameObject("Bone").transform;
					b.parent = obj.transform;
					b.localPosition = new Vector3 (Mathf.Sin (Mathf.Deg2Rad * i) * radius, Mathf.Cos (Mathf.Deg2Rad * i) * radius, 0);
					b.localEulerAngles = Vector3.forward * (-i);

					Transform bn = new GameObject("Bone").transform;
					bn.parent = b;
					bn.localEulerAngles = Vector3.zero;
					bn.localPosition = Vector3.zero;

					bones.Add (bn);



					if (border == 0 && i%15==0)
					{
						GameObject o = CustomObject.CreatePrimitive (PrimitiveType.Cube, false);// GameObject.CreatePrimitive (PrimitiveType.Cube);
						o.transform.localScale = Vector3.one * 0.005f + Vector3.forward*0.32f;
						o.GetComponent<Renderer> ().material = Ball.GetMaterial (Obj.Colour.WHITE);
						o.transform.parent = bn;
						o.transform.localPosition = Vector3.zero;
					}

					BoneWeight w = new BoneWeight ();
					w.boneIndex0 = u/2;
					w.weight0 = 1;

					BoneWeight w2 = new BoneWeight ();
					w2.boneIndex0 = u/2;
					w2.weight0 = 1;

					weights.Add (w);
					weights.Add (w2);

				}

				mesh.SetVertices (verts);
				mesh.SetTriangles (tris, 0);
				mesh.boneWeights = weights.ToArray ();

				skin.rootBone = obj.transform;
				skin.bones = bones.ToArray ();


				Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];

				for (int i = 0; i < bones.Count; ++i)
				{
					bindPoses[i] = bones[i].worldToLocalMatrix * obj.transform.localToWorldMatrix;
				}

				mesh.bindposes = bindPoses;

				skin.sharedMesh = mesh;

				float a = 0;
				foreach (Transform bone in bones)
				{
					//bone.RotateAround (Vector3.right, a + 30);
					bone.localEulerAngles += Vector3.right * (a + 30);
					//bone.localEulerAngles = new Vector3 (bone.localEulerAngles.x, 0, bone.localEulerAngles.z);
					a += 5;

//					if (border == 0)
//					{
//						GameObject s = GameObject.CreatePrimitive (PrimitiveType.Cube);
//						(s.GetComponent<Renderer> ().material = Game.BaseMaterial).color = Color.black;
//						s.transform.parent = bone;
//						s.transform.localScale = Vector3.one * 0.03f;
//						s.transform.localPosition = Vector3.up * 0.1f;
//
//
//						s = GameObject.CreatePrimitive (PrimitiveType.Cube);
//						(s.GetComponent<Renderer> ().material = Game.BaseMaterial).color = Color.black;
//						s.transform.parent = bone;
//						s.transform.localScale = Vector3.one * 0.03f;
//						s.transform.localPosition = -Vector3.up * 0.1f;
//					}
				}
				//obj = Word.GetGameObject (mesh);

			}
			break;

		default:
			obj = new GameObject ();
			break;

		}



		Symbol symbol = obj.AddComponent<Symbol> ();
		symbol.type = t;
		symbol.thick = thick;
		symbol.radius = radius;
		if (border == 0)
			symbol.withSphere = true;

		return symbol;
	}

}
