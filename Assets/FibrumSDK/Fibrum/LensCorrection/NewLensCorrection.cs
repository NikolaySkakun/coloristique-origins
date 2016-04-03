using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class NewLensCorrection : MonoBehaviour {

	public float strengthX = 0.55f;
	public float strengthY = 0.55f;
	float initStrengthX,initStrengthY;
	
	public Shader LensCorrectionShader = null;
	public static Material LensCorrectionMaterial = null;	
	
	public bool useDistortion = true;
	
	void Start() {
		
		if (useDistortion) {
			if (!LensCorrectionMaterial) {
				CreateMat();
			}
		}
	}

	void CreateMat() {
		if (LensCorrectionMaterial)	return;
		initStrengthX = strengthX;
		initStrengthY = strengthY;
		LensCorrectionMaterial = new Material( LensCorrectionShader );
		LensCorrectionMaterial.hideFlags = HideFlags.DontSave;
		LensCorrectionMaterial.SetFloat ("_k" , strengthX );
		LensCorrectionMaterial.SetFloat ("_kcube" , strengthY );
	}

	void OnRenderImage (RenderTexture source, RenderTexture destination) {	
		if (!enabled)
			return;

		if( initStrengthX!=strengthX || initStrengthY!=strengthY ) LensCorrectionMaterial = null;
		CreateMat ();

		if (useDistortion) {
			Graphics.Blit (source, destination, LensCorrectionMaterial); 	
		} else {
			Graphics.Blit (source, destination);
		}
	}
}
