using UnityEngine;
using System.Collections;

static public class FibrumController {
	
	static public VRSensor vrs;
	static public VRCamera vrCamera;
	static public GameObject vrSetup;
	static public float distanceBetweenLens=0f;
	static public bool isHandOriented;
	static public bool useAntiDrift;
	static public bool useCompassForAntiDrift;
	static public float dpi;
	
	static bool initialized = false;
	static public void Init()
	{
		if( initialized ) return;
		CalculateDPI();
		#if UNITY_ANDROID && !UNITY_EDITOR
		if( vrs == null  )
		{
			GameObject vrsGO = GameObject.Instantiate((GameObject)Resources.Load("FibrumResources/VRSensor",typeof(GameObject))) as GameObject;
			vrs = vrsGO.GetComponent<VRSensor>();
		}
		#endif
		distanceBetweenLens = PlayerPrefs.GetFloat("FIB_lensDistance",0f);
		initialized = true;
	}
	
	public static void CalculateDPI()
	{
		#if UNITY_ANDROID
		if( Application.platform == RuntimePlatform.Android )
		{
			DisplayMetricsAndroid.DisplayMetricsAndroidInit();
			dpi = DisplayMetricsAndroid.XDPI;
		}
		else
		{
			dpi = Screen.dpi;
		}
		#else
		dpi = Screen.dpi;
		#endif
	}
	
}