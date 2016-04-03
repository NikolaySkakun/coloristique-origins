using UnityEngine;
using System.Collections;

public class GameDisplayScript : MonoBehaviour {

	public Material mat;
	public Color color1;
	public Color color2;
	public float t;
	public Texture calibratingTex;
	public float texT;
	void Start () {
		mat.SetColor("_Color",Color.Lerp(color1,color2,t));
	}

	void OnGUI () {
		if( t>0.01f )
		{
			mat.SetColor("_Color",Color.Lerp(color1,color2,t));
		}
		if( texT>0.01f )
		{
			Color tmpColor = GUI.color;
			GUI.color = new Color(1f,1f,1f,texT);
			GUIUtility.RotateAroundPivot(90f, Vector2.zero);
			GUI.DrawTexture(new Rect(Screen.height,0f,-Screen.height,-Screen.width),calibratingTex,ScaleMode.ScaleAndCrop);
			GUI.color = tmpColor;
		}
	}
}
