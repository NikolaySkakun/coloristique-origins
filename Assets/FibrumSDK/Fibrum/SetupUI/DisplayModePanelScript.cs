using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayModePanelScript : MonoBehaviour {

	[System.Serializable]
	public struct ToggleDisplayMode
	{
		public string name;
		public Toggle toggle;
		public float distanceBetweenLens;
	}
	public ToggleDisplayMode[] displayModes;

	void OnEnable()
	{
		for( int k=0; k<displayModes.Length; k++ )
		{
			if( (int)FibrumController.distanceBetweenLens==(int)displayModes[k].distanceBetweenLens )	displayModes[k].toggle.isOn = true;
			else displayModes[k].toggle.isOn = false;
		}
	}

	public void ToogleVRDevice_FullScreen(bool on)
	{
		if( displayModes[0].toggle.isOn ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=displayModes[0].distanceBetweenLens);
	}
	public void ToogleVRDevice_Fibrum(bool on)
	{
		if( displayModes[1].toggle.isOn ) PlayerPrefs.SetFloat("FIB_lensDistance",FibrumController.distanceBetweenLens=displayModes[1].distanceBetweenLens);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
