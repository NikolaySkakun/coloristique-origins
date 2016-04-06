using UnityEngine;
using System.Collections;

public class VRCalibration : MonoBehaviour {

	public string LevelToLoad;
	bool isLogo = false;

	void Start () {
		
		Invoke("Loader", 2);

	}
	
	void Loader () {
		
		if(!isLogo){
			GameObject.Find("Logo").SetActive(false);
			isLogo = true;
		} else {
			Application.LoadLevel(LevelToLoad);
		}
		
		Invoke("Loader", 5);

	}
	
}