using UnityEngine;
using System.Collections;

public class Audio : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{

		float[] samples = new float[GetComponent<AudioSource>().clip.samples * GetComponent<AudioSource>().clip.channels];
		//GetComponent<AudioSource>().clip.GetData(samples, 0);
		int i = 0;
		while (i < samples.Length) {
			Debug.Log(samples[i]);
			samples[i]= 0.2F;
			++i;
		}
		GetComponent<AudioSource>().clip.SetData(samples, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
