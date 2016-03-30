using UnityEngine;
using System.Collections;

public class Portal 
{
	GameObject gameObject;
	FillScreen controller;

	public Portal()
	{
		gameObject = new GameObject("Portal");
		controller = gameObject.AddComponent<FillScreen>();

		//controller.cam = Player.player.transform.GetComponentInChildren<Camera>();

		controller.portal1Cam = (new GameObject("Camera")).AddComponent<Camera>();
		controller.portal1Cam.clearFlags = CameraClearFlags.SolidColor;
		controller.portal1Cam.backgroundColor = Color.white;
		controller.portal1Cam.depth = -4;
		controller.portal1Cam.transform.parent = gameObject.transform;

		controller.portal2Cam = (new GameObject("Camera")).AddComponent<Camera>();
		controller.portal2Cam.clearFlags = CameraClearFlags.Nothing;
		controller.portal2Cam.depth = -2;
		controller.portal2Cam.transform.parent = gameObject.transform;


		controller.portal1 = new GameObject("Portal").transform;
		controller.portal1.gameObject.AddComponent<MeshRenderer>().material.shader = Shader.Find("JustZValue");
		controller.portal1.gameObject.AddComponent<MeshFilter>().mesh = CustomMesh.Quad();
		
//		GameObject child = new GameObject("Portal");
//		child.AddComponent<MeshRenderer>().material.shader = Shader.Find("FillZValue");
//		child.AddComponent<MeshFilter>().mesh = CustomMesh.Quad();
//		child.transform.parent = controller.portal1;
		
		controller.portal1.localEulerAngles = Vector3.right * 270f;




		controller.portal2 = new GameObject("Portal").transform;
		controller.portal2.gameObject.AddComponent<MeshRenderer>().material.shader = Shader.Find("JustZValue");
		controller.portal2.gameObject.AddComponent<MeshFilter>().mesh = CustomMesh.Quad();
		
//		child = new GameObject("Portal");
//		child.AddComponent<MeshRenderer>().material.shader = Shader.Find("FillZValue");
//		child.AddComponent<MeshFilter>().mesh = CustomMesh.Quad();
//		child.transform.parent = controller.portal2;
		
		controller.portal2.localEulerAngles = Vector3.right * 270f;


	}
}
