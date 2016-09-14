using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Tesseract : Obj 
{
	static float thick = 0.005f;
	public float angle = 0;

	Transform[] bones;

	Transform CreateEdge(int boneIndex0, int boneIndex1, int axis = 2)
	{
		Mesh mesh = new Mesh ();


		List<Vector3> verts = new List<Vector3> ();
		int[] tris;

		if (axis == 0)
		{
			verts.Add (new Vector3 (-0.5f, thick, thick)); // 0
			verts.Add (new Vector3 (-0.5f, thick, -thick)); // 1
			verts.Add (new Vector3 (-0.5f, -thick, -thick)); // 2
			verts.Add (new Vector3 (-0.5f, -thick, thick)); // 3

			verts.Add (new Vector3 (0.5f, thick, thick)); // 4
			verts.Add (new Vector3 (0.5f, thick, -thick)); // 5
			verts.Add (new Vector3 (0.5f, -thick, -thick)); // 6
			verts.Add (new Vector3 (0.5f, -thick, thick)); // 7
		} 
		else if (axis == 1)
		{
			verts.Add (new Vector3 (thick, -0.5f, thick)); // 0
			verts.Add (new Vector3 (thick, -0.5f, -thick)); // 1
			verts.Add (new Vector3 (-thick, -0.5f, -thick)); // 2
			verts.Add (new Vector3 (-thick, -0.5f, thick)); // 3

			verts.Add (new Vector3 (thick, 0.5f, thick)); // 4
			verts.Add (new Vector3 (thick, 0.5f, -thick)); // 5
			verts.Add (new Vector3 (-thick, 0.5f, -thick)); // 6
			verts.Add (new Vector3 (-thick, 0.5f, thick)); // 7
		}
		else
		{
		verts.Add (new Vector3 (thick, thick, -0.5f)); // 0
		verts.Add (new Vector3 (thick, -thick, -0.5f)); // 1
		verts.Add (new Vector3 (-thick, -thick, -0.5f)); // 2
		verts.Add (new Vector3 (-thick, thick, -0.5f)); // 3

		verts.Add (new Vector3 (thick, thick, 0.5f)); // 4
		verts.Add (new Vector3 (thick, -thick, 0.5f)); // 5
		verts.Add (new Vector3 (-thick, -thick, 0.5f)); // 6
		verts.Add (new Vector3 (-thick, thick, 0.5f)); // 7
		}

		tris = new int[]{ 
			0, 4, 1,
			4, 5, 1,

			1, 5, 2,
			5, 6, 2,

			2, 6, 3,
			6, 7, 3,

			0, 7, 4,
			7, 3, 0
		};

		BoneWeight[] weights = new BoneWeight[8];

		for (int i = 0; i < verts.Count; ++i)
		{
			weights [i].boneIndex0 = i < 4 ? 0 : 1;
			weights [i].weight0 = 1;
		}

		mesh.SetVertices (verts);
		mesh.triangles = tris;
		mesh.boneWeights = weights;

		Transform obj = new GameObject ("Edge").transform;//(new GameObject("Tesseract")).AddComponent<Tesseract> ();

		SkinnedMeshRenderer skin = obj.gameObject.AddComponent<SkinnedMeshRenderer> ();
		skin.sharedMesh = mesh;
		Bounds bounds = new Bounds (Vector3.zero, Vector3.one);

		skin.localBounds = bounds;

		Vector3 dir = Vector3.zero;
		dir [axis] = 1;

		List<Transform> bonesList = new List<Transform> ();
		Transform bone = bones [boneIndex0]; //new GameObject ("Bone").transform;


		//bone.parent = obj;
		bone.localPosition = -dir * 0.5f;
		bonesList.Add (bone);

		bone = bones [boneIndex1];
		//bone.parent = obj.transform;
		bone.localPosition = dir * 0.5f;
		bonesList.Add (bone);




		Matrix4x4[] bindPoses = new Matrix4x4[bonesList.Count];

		for (int i = 0; i < bonesList.Count; ++i)
		{
			bindPoses[i] = bonesList[i].worldToLocalMatrix * transform.localToWorldMatrix;
		}

		mesh.bindposes = bindPoses;

		skin.bones = bonesList.ToArray();
		(skin.material = Game.BaseMaterial).color = Game.White;
		//(skin.material = new Material(Shader.Find("InverseColor"))).color = Color.white;
		obj.SetParent (transform);
		return obj;
	}

	public void FixedUpdate()
	{
		//transform.localEulerAngles += Vector3.up * Time.fixedDeltaTime * 22.5f;
	}

	static public Tesseract Create()
	{
		float smallCube = 0.25f;
		float largeCube = 0.5f;

		Tesseract obj = (new GameObject("Tesseract")).AddComponent<Tesseract> ();
		obj.bones = new Transform[16];

		for (int i = 0; i < obj.bones.Length; ++i)
		{
			obj.bones [i] = Word.GetGameObject (OctahedronSphereCreator.Create (3, 0.5f* 0.1f)).transform;
				//CustomObject.CreatePrimitive(PrimitiveType.Sphere, false).transform;//new GameObject ("Bone" + i.ToString()).transform;
			obj.bones[i].GetComponent<Renderer>().material = Ball.GetMaterial(Colour.BLACK);
			obj.bones[i].localScale = Vector3.one * 0.38f;
			obj.bones [i].SetParent (obj.transform);

		}



		obj.CreateEdge (0, 1, 1);
		obj.CreateEdge (0, 2);
		obj.CreateEdge (0, 4);
		obj.CreateEdge (0, 8, 0);

		obj.CreateEdge (1, 3);
		obj.CreateEdge (1, 5);
		obj.CreateEdge (1, 9, 0);

		obj.CreateEdge (2, 3, 1);
		obj.CreateEdge (2, 6);
		obj.CreateEdge (2, 10, 0);

		obj.CreateEdge (3, 7);
		obj.CreateEdge (3, 11, 0);

		obj.CreateEdge (4, 5, 1);
		obj.CreateEdge (4, 6);
		obj.CreateEdge (4, 12, 0);

		obj.CreateEdge (5, 7);
		obj.CreateEdge (5, 13, 0);

		obj.CreateEdge (6, 7, 1);
		obj.CreateEdge (6, 14, 0);

		obj.CreateEdge (7, 15, 0);

		obj.CreateEdge (8, 9, 1);
		obj.CreateEdge (8, 10);
		obj.CreateEdge (8, 12);

		obj.CreateEdge (9, 11);
		obj.CreateEdge (9, 13);

		obj.CreateEdge (10, 11, 1);
		obj.CreateEdge (10, 14);

		obj.CreateEdge (11, 15);

		obj.CreateEdge (12, 13, 1);
		obj.CreateEdge (12, 14);

		obj.CreateEdge (13, 15);

		obj.CreateEdge (14, 15, 1);



		obj.bones [0].localPosition = new Vector3 (smallCube, -smallCube, -smallCube);
		obj.bones [1].localPosition = new Vector3 (smallCube, smallCube, -smallCube);
		obj.bones [2].localPosition = new Vector3 (smallCube, -smallCube, smallCube);
		obj.bones [3].localPosition = new Vector3 (smallCube, smallCube, smallCube);

		obj.bones [4].localPosition = new Vector3 (largeCube, -largeCube, -largeCube);
		obj.bones [5].localPosition = new Vector3 (largeCube, largeCube, -largeCube);
		obj.bones [6].localPosition = new Vector3 (largeCube, -largeCube, largeCube);
		obj.bones [7].localPosition = new Vector3 (largeCube, largeCube, largeCube);

		obj.bones [8].localPosition = new Vector3 (-smallCube, -smallCube, -smallCube);
		obj.bones [9].localPosition = new Vector3 (-smallCube, smallCube, -smallCube);
		obj.bones [10].localPosition = new Vector3 (-smallCube, -smallCube, smallCube);
		obj.bones [11].localPosition = new Vector3 (-smallCube, smallCube, smallCube);

		obj.bones [12].localPosition = new Vector3 (-largeCube, -largeCube, -largeCube);
		obj.bones [13].localPosition = new Vector3 (-largeCube, largeCube, -largeCube);
		obj.bones [14].localPosition = new Vector3 (-largeCube, -largeCube, largeCube);
		obj.bones [15].localPosition = new Vector3 (-largeCube, largeCube, largeCube);


		string clipName = "Anim";
		Vector3[] points = new Vector3[4];
		AnimationClip clip;
		float time = 8f;

		points = new Vector3[]{ obj.bones[0].localPosition, obj.bones[4].localPosition, obj.bones[6].localPosition, obj.bones[2].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [0].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);



		points = new Vector3[]{ obj.bones[1].localPosition, obj.bones[5].localPosition, obj.bones[7].localPosition, obj.bones[3].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [1].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);


		points = new Vector3[]{ obj.bones[2].localPosition, obj.bones[0].localPosition, obj.bones[4].localPosition, obj.bones[6].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [2].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[3].localPosition, obj.bones[1].localPosition, obj.bones[5].localPosition, obj.bones[7].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [3].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[4].localPosition, obj.bones[6].localPosition, obj.bones[2].localPosition, obj.bones[0].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [4].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[5].localPosition, obj.bones[7].localPosition, obj.bones[3].localPosition, obj.bones[1].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [5].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[6].localPosition, obj.bones[2].localPosition, obj.bones[0].localPosition, obj.bones[4].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [6].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[7].localPosition, obj.bones[3].localPosition, obj.bones[1].localPosition, obj.bones[5].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [7].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[8].localPosition, obj.bones[12].localPosition, obj.bones[14].localPosition, obj.bones[10].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [8].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[9].localPosition, obj.bones[13].localPosition, obj.bones[15].localPosition, obj.bones[11].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [9].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[10].localPosition, obj.bones[8].localPosition, obj.bones[12].localPosition, obj.bones[14].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [10].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[11].localPosition, obj.bones[9].localPosition, obj.bones[13].localPosition, obj.bones[15].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [11].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[12].localPosition, obj.bones[14].localPosition, obj.bones[10].localPosition, obj.bones[8].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [12].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[13].localPosition, obj.bones[15].localPosition, obj.bones[11].localPosition, obj.bones[9].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [13].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[14].localPosition, obj.bones[10].localPosition, obj.bones[8].localPosition, obj.bones[12].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [14].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		points = new Vector3[]{ obj.bones[15].localPosition, obj.bones[11].localPosition, obj.bones[9].localPosition, obj.bones[13].localPosition};
		clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, time);
		obj.bones [15].gameObject.AddComponent<Animation> ().AddClip (clip, clipName);

		for (int i = 0; i < obj.bones.Length; ++i)
		{
			obj.bones [i].GetComponent<Animation> ().wrapMode = WrapMode.Loop;
			//obj.bones [i].GetComponent<Animation> ().
			obj.bones [i].GetComponent<Animation> ().Play (clipName);
		}

		obj.angle = Time.time;

		return obj;
	}



}
