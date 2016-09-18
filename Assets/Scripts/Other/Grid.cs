using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Grid : MonoBehaviour {

	List<Transform> bones;
	int size = 20;
	// Use this for initialization
	void Start () 
	{
		GameObject obj = gameObject;

		SkinnedMeshRenderer skin = obj.AddComponent<SkinnedMeshRenderer> ();
		bones = new List<Transform> ();
		List<BoneWeight> weights = new List<BoneWeight> ();


		Mesh mesh = CustomMesh.Plane (size); //gameObject.GetComponent<MeshFilter> ().mesh;

		for (int i = 0; i < mesh.vertices.Length; ++i)
		{
			GameObject bone = new GameObject ("Bone");
			bone.transform.SetParent (transform);
			bone.transform.position = mesh.vertices [i];

			bones.Add (bone.transform);

			BoneWeight w = new BoneWeight ();
			w.boneIndex0 = i;
			w.weight0 = 1;

			weights.Add (w);
		}

		mesh.boneWeights = weights.ToArray ();
		skin.bones = bones.ToArray ();

		Matrix4x4[] bindPoses = new Matrix4x4[bones.Count];

		for (int i = 0; i < bones.Count; ++i)
		{
			bindPoses[i] = bones[i].worldToLocalMatrix * obj.transform.localToWorldMatrix;
		}

		mesh.bindposes = bindPoses;

		skin.sharedMesh = mesh;
		skin.material = new Material (Shader.Find("Grid"));
	}
	
	// Update is called once per frame
	void Update () 
	{
		for(int u=0;u<size; ++u)
			for (int i = 0; i < size; ++i)
		{
				bones [size*u + i].transform.localPosition = new Vector3 (bones [size*u + i].transform.localPosition.x, Mathf.Cos((Mathf.Sin((u)*(180f/size))+i)*(180f/size) + Time.time), bones [size*u + i].transform.localPosition.z);
		}
	}
}
