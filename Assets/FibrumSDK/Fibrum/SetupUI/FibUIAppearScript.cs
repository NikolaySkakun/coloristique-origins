using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FibUIAppearScript : MonoBehaviour {

	public Texture[] animationPanel;
	public RawImage mainUIgo;
	public CanvasGroup alphaPanel;
	public Texture lineTex,setupTex;
	GameObject tempEventSystem;

	void OnGUI()
	{
		if( FibrumController.vrCamera==null ) return;
		if( !FibrumController.vrCamera.gameObject.activeSelf ) return;
		if( !(mainUIgo.gameObject.activeSelf) )
		{
			GUI.DrawTexture(new Rect(Screen.width/2f-1f,0.01f*Screen.width,2f,Screen.height-0.01f*Screen.width-0.09f*Screen.width),lineTex);
			GUIStyle style = GUI.skin.GetStyle("label");
			style.alignment = TextAnchor.MiddleCenter;
			if( GUI.Button(new Rect(Screen.width/2f-0.04f*Screen.width,Screen.height-0.08f*Screen.width,0.08f*Screen.width,0.08f*Screen.width),setupTex,style) )
			{
				UIPanelAction();
			}
		}
	}
	
	void UIPanelAction()
	{
		StartCoroutine (_UIPanelAction());
	}
	
	IEnumerator _UIPanelAction()
	{
		if( !mainUIgo.gameObject.activeSelf )
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
			mainUIgo.gameObject.SetActive(!mainUIgo.gameObject.activeSelf);
			for( int k=0; k<animationPanel.Length; k++ )
			{
				mainUIgo.texture = animationPanel[k];
				mainUIgo.color = Color.Lerp(new Color(0f,0f,0f,0f),new Color(0f,0f,0f,0.75f),(float)k/((float)animationPanel.Length-1));
				alphaPanel.alpha = (float)(k-10)/((float)animationPanel.Length-1-10);
				alphaPanel.transform.localScale = Vector3.Lerp(Vector3.one*1.3f,Vector3.one,(float)(k-10)/((float)animationPanel.Length-1-10));
				yield return new WaitForEndOfFrame();
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
			for( int k=animationPanel.Length-1; k>=0; k-- )
			{
				mainUIgo.texture = animationPanel[k];
				mainUIgo.color = Color.Lerp(new Color(0f,0f,0f,0f),new Color(0f,0f,0f,0.75f),(float)k/((float)animationPanel.Length-1));
				alphaPanel.alpha = (float)(k-10)/((float)animationPanel.Length-1-10);
				alphaPanel.transform.localScale = Vector3.Lerp(Vector3.one*1.3f,Vector3.one,(float)(k-10)/((float)animationPanel.Length-1-10));
				yield return new WaitForEndOfFrame();
			}
			mainUIgo.gameObject.SetActive(!mainUIgo.gameObject.activeSelf);
		}
	}
	
	public void CLoseUIPanel()
	{
		UIPanelAction();
	}
}
