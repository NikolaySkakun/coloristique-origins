using UnityEngine;
using System.Collections;

public class ChangeCameraEye : MonoBehaviour {

	float lastScreenWidth;
	float lastLensDist;
	private float initCameraRectXmin;
	private float initSidePosition;
	private Camera myCamera;

	// Use this for initialization
	void Start () {
		myCamera = GetComponent<Camera>();
		myCamera.layerCullSpherical = true;
		initCameraRectXmin = myCamera.rect.xMin;
		initSidePosition = transform.localPosition.x;	
	}

	void ProcessCameraView()
	{
		float deviceDiagonal = Mathf.Sqrt((float)(Screen.width*Screen.width)+(float)(Screen.height*Screen.height))/FibrumController.dpi;
		#if UNITY_STANDALONE
		transform.localPosition = new Vector3(-initSidePosition,transform.localPosition.y,transform.localPosition.z);
		myCamera.rect = new Rect(0.5f-initCameraRectXmin,0f,0.5f,1f);
		#else
		if ( Application.isEditor )
		{
			transform.localPosition = new Vector3(-initSidePosition,transform.localPosition.y,transform.localPosition.z);
			myCamera.rect = new Rect(0.5f-initCameraRectXmin,0f,0.5f,1f);
		}
		else if ((deviceDiagonal<3.9f && deviceDiagonal>7.1f) || FibrumController.distanceBetweenLens<1f )
		{
			transform.localPosition = new Vector3(initSidePosition,transform.localPosition.y,transform.localPosition.z);
			myCamera.rect = new Rect(initCameraRectXmin,0f,0.5f,1f);
		}
		else
		{
			float screenLength = (Screen.width/FibrumController.dpi)*25.4f;
			float viewPortCenter = (FibrumController.distanceBetweenLens/2f)/screenLength;
			float viewPortHalfSize = Mathf.Min (viewPortCenter,0.5f-viewPortCenter);
			float screenHeight = (Screen.height/FibrumController.dpi)*25.4f;
			float viewPortYHalfSize = Mathf.Min (0.5f,0.5f*FibrumController.distanceBetweenLens/screenHeight);
			myCamera.rect = new Rect(0.5f+(initCameraRectXmin*4f-1f)*viewPortCenter-viewPortHalfSize,0.5f-viewPortYHalfSize,viewPortHalfSize*2f,viewPortYHalfSize*2f);
		}
		#endif
		lastScreenWidth = Screen.width;
		lastLensDist = FibrumController.distanceBetweenLens;
	}
	
	// Update is called once per frame
	void Update () {
		if( Screen.width != lastScreenWidth ) ProcessCameraView();
		if( lastLensDist != FibrumController.distanceBetweenLens ) ProcessCameraView();
	}
}
