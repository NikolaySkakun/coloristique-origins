using UnityEngine;
using System.Collections;

public class ExampleClass : MonoBehaviour {


	void OnPreRender() {
		GL.wireframe = true;
	}
	void OnPostRender() {
		GL.wireframe = false;
	}

}