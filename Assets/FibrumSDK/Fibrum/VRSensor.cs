using UnityEngine;
using System.Collections;

public class VRSensor : MonoBehaviour {

	#if UNITY_ANDROID
	public AndroidJavaObject _ao;
	AndroidJavaClass jc;
	#endif

	void Start () {
	
		#if UNITY_IPHONE && !UNITY_EDITOR
		Input.gyro.enabled = true;
		Input.gyro.updateInterval = 0.0001f;
		#endif
		
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		DontDestroyOnLoad(transform.gameObject);
		
		#if UNITY_ANDROID && !UNITY_EDITOR
		jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject obj = jc.GetStatic<AndroidJavaObject>("currentActivity");
		_ao = new AndroidJavaObject("com.fibrum.fibrumsdk.SensorClass", obj);
		
		_ao.Call<bool>("ActivateSensor");
		#endif

	}


	
}