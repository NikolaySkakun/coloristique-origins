
using UnityEngine;
using System.Collections;

public class FibrumPhoneStart : MonoBehaviour {
	
	float timeToLoadScene=10f;
	[HideInInspector]
	public string sceneNameToLoad;
	private GameDisplayScript gds;

	void Start () {
		timeToLoadScene=10f;
		Invoke ("ReturnTimeScale",timeToLoadScene-3f);
		gds = GameObject.FindObjectOfType<GameDisplayScript>();
		Screen.orientation = ScreenOrientation.Landscape;
		#if UNITY_ANDROID && !UNITY_EDITOR
		FibrumController.Init();
		#endif
	}

	void ReturnTimeScale()
	{
		Time.timeScale = 1f;
		Invoke ("LoadScene",3f);
	}

	void Update()
	{
		if( Input.GetMouseButtonDown(0) )
		{
			Time.timeScale = 100f;
		}
	}

	void OnGUI()
	{
		if( Time.timeScale>1f )
		{
			GUIUtility.RotateAroundPivot(90f, Vector2.zero);
			GUI.DrawTexture(new Rect(Screen.height,0f,-Screen.height,-Screen.width),gds.calibratingTex);
		}
	}

	void LoadScene() {
		Time.timeScale = 1f;
		Application.LoadLevel(sceneNameToLoad);
	}
}
