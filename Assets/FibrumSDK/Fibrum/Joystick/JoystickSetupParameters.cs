using UnityEngine;
using System.Collections;

public class JoystickSetupParameters : MonoBehaviour {

	public string title;
	public enum CheckDirection:int {Disable=0,CheckForward=1,CheckBack=2};

	[System.Serializable]
	public struct ParametersField
	{
		public FibrumInput.Control controlToSetup;
		public CheckDirection checkDirection;
		public Texture picture;
		public string description;
	}

	public ParametersField[] controlsToSetup;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
