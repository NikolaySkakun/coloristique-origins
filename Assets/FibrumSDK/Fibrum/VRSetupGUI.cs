using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VRSetupGUI : MonoBehaviour {

	public Texture lineTex;
	public Texture setupTex;
	public GameObject setupGO;
	public GameObject VRDevicePanel;
	public GameObject ParametersPanel;

	public Toggle enableAntidrift;
	public Toggle enableCompass;

	public Toggle[] HMDtoggle;

	GameObject tempEventSystem;

	void OnGUI()
	{
		if( FibrumController.vrCamera==null ) return;
		if( !FibrumController.vrCamera.gameObject.activeSelf ) return;
		if( !setupGO.activeSelf )
			GUI.DrawTexture(new Rect(Screen.width/2f-1f,0.01f*Screen.width,2f,Screen.height-0.01f*Screen.width-0.06f*Screen.width),lineTex);
		GUIStyle style = GUI.skin.GetStyle("label");
		style.alignment = TextAnchor.MiddleCenter;
		if( GUI.Button(new Rect(Screen.width/2f-0.025f*Screen.width,Screen.height-0.05f*Screen.width,0.05f*Screen.width,0.05f*Screen.width),setupTex,style) )
		{
			setupGO.SetActive(!setupGO.activeSelf);
			if( setupGO.activeSelf )
			{
				if( GameObject.FindObjectOfType<EventSystem>() )
				{
					GameObject eventSystem = GameObject.FindObjectOfType<EventSystem>().gameObject;
					if( eventSystem.GetComponent<VRInputModule>() == null )
					{
						if( eventSystem.GetComponent<StandaloneInputModule>()!=null ) eventSystem.GetComponent<StandaloneInputModule>().enabled=true;
						if( eventSystem.GetComponent<TouchInputModule>()!=null ) eventSystem.GetComponent<TouchInputModule>().enabled=true;
					}
					else
					{
						if( eventSystem.GetComponent<StandaloneInputModule>()!=null ) eventSystem.GetComponent<StandaloneInputModule>().enabled=true;
						if( eventSystem.GetComponent<TouchInputModule>()!=null ) eventSystem.GetComponent<TouchInputModule>().enabled=true;
						if( eventSystem.GetComponent<VRInputModule>()!=null ) eventSystem.GetComponent<VRInputModule>().enabled=false;
					}
				}
				else
				{
					tempEventSystem = GameObject.Instantiate((GameObject)Resources.Load("FibrumResources/EventSystem",typeof(GameObject))) as GameObject;
				}
				for( int k=0; k<HMDtoggle.Length; k++ )
				{
					if( (int)FibrumController.distanceBetweenLens==(int)lensDistance[k] )
						HMDtoggle[k].isOn = true;
					//else HMDtoggle[k].isOn = false;
				}
			}
			else
			{
				if( tempEventSystem!=null ) Destroy (tempEventSystem);
				else
				{
					if( GameObject.FindObjectOfType<EventSystem>()!=null )
					{
						GameObject eventSystem = GameObject.FindObjectOfType<EventSystem>().gameObject;
						if( eventSystem.GetComponent<StandaloneInputModule>()!=null ) eventSystem.GetComponent<StandaloneInputModule>().enabled=false;
						if( eventSystem.GetComponent<TouchInputModule>()!=null ) eventSystem.GetComponent<TouchInputModule>().enabled=false;
						if( eventSystem.GetComponent<VRInputModule>()!=null ) eventSystem.GetComponent<VRInputModule>().enabled=true;
					}
				}
			}
		}
	}

	public void ToggleVRDevice(bool on)
	{
		VRDevicePanel.SetActive(on);
		ParametersPanel.SetActive(false);
	}

	public void ToggleParameters(bool on)
	{
		ParametersPanel.SetActive(true);
		VRDevicePanel.SetActive(false);
#if UNITY_ANDROID
		enableAntidrift.isOn = FibrumController.useAntiDrift;
		enableCompass.isOn = FibrumController.useCompassForAntiDrift;
#else
		enableAntidrift.gameObject.SetActive(false);
		enableCompass.gameObject.SetActive(false);
#endif
	}

	public float[] lensDistance = {0f,55f,56f,60f};

	public void ToogleVRDevice_FullScreen(bool on)
	{
		if( on ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=lensDistance[0]);
	}
	public void ToogleVRDevice_Fibrum(bool on)
	{
		if( on ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=lensDistance[1]);
	}
	public void ToogleVRDevice_CardBoard(bool on)
	{
		if( on ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=lensDistance[2]);
	}
	public void ToogleVRDevice_GearVR(bool on)
	{
		if( on ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=lensDistance[3]);
	}

	public void ToggleParameters_Antidrift(bool on)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		FibrumController.useAntiDrift = enableAntidrift.isOn;
		#endif
	}

	public void ToggleParameters_Compass(bool on)
	{
		#if UNITY_ANDROID && !UNITY_EDITOR
		FibrumController.useCompassForAntiDrift = enableCompass.isOn;
		FibrumController.vrCamera.EnableCompass(enableCompass.isOn);
		#endif
	}

}
