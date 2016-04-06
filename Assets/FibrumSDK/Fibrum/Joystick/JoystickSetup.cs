using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class JoystickSetup : MonoBehaviour {

	public Texture pressKeyTex;
	public Texture moveJoysctickTex;
	public RawImage picture;
	public Text desc;
	public Text title;
	public RawImage checkMark;

	private List<int> typeOfControl;
	private List<int> controlNumToSetup;
	private List<string> controlDescription;
	private List<Texture> textures;

	bool initialized=false;
	void Init()
	{
		if( initialized ) return;
		typeOfControl = new List<int>();
		controlNumToSetup = new List<int>();
		controlDescription = new List<string>();
		textures = new List<Texture>();
		GetComponent<Canvas>().enabled = false;
		checkMark.color = new Color(1f,1f,1f,0f);
		initialized=true; 
	}

	// Use this for initialization
	void Start () {
		Init();
	}

	// Update is called once per frame
	void Update () {
		if( scanning )
		{
			if( typeOfControl[currentControl]==0 )
			{
				if( FibrumInput.ScanAndSetupAxis((FibrumInput.Axis)controlNumToSetup[currentControl])!=-1 )
				{
					checkMark.color = new Color(1f,1f,1f,1f);
					scanning = false;
					Invoke ("NextControl",1f);
				}
			}
			else if( typeOfControl[currentControl]==1 )
			{
				if( FibrumInput.ScanAndSetupButton((FibrumInput.Button)controlNumToSetup[currentControl])!=-1 )
				{
					checkMark.color = new Color(1f,1f,1f,1f);
					scanning = false;
					Invoke ("NextControl",1f);
				}
			}
			else if( typeOfControl[currentControl]==2 )
			{
				if( FibrumInput.ScanAndSetupAxisDir((FibrumInput.Axis)controlNumToSetup[currentControl],true)!=-1 )
				{
					checkMark.color = new Color(1f,1f,1f,1f);
					scanning = false;
					Invoke ("NextControl",1f);
				}
			}
			else if( typeOfControl[currentControl]==3 )
			{
				if( FibrumInput.ScanAndSetupAxisDir((FibrumInput.Axis)controlNumToSetup[currentControl],false)!=-1 )
				{
					checkMark.color = new Color(1f,1f,1f,1f);
					scanning = false;
					Invoke ("NextControl",1f);
				}
			}
		}
	}

	void NextControl()
	{
		scanning = true;
		checkMark.color = new Color(1f,1f,1f,0f);
		currentControl++;
		if( currentControl < typeOfControl.Count ) SetUI();
		else Destroy (gameObject);
	}

	public void SetTitle(string titleText)
	{
		title.text = titleText;
	}

	public void SetControl(FibrumInput.Axis axis,string description,Texture tex)
	{
		Init ();
		typeOfControl.Add(0);
		controlNumToSetup.Add((int)axis);
		controlDescription.Add(description);
		if( tex!=null )	textures.Add (tex);
		else textures.Add (moveJoysctickTex);
	}

	public void SetControl(FibrumInput.Axis axis,bool checkAxisDirection,string description,Texture tex)
	{
		Init ();
		if( checkAxisDirection ) typeOfControl.Add(2);
		else typeOfControl.Add(3);
		controlNumToSetup.Add((int)axis);
		controlDescription.Add(description);
		if( tex!=null )	textures.Add (tex);
		else textures.Add (moveJoysctickTex);
	}

	public void SetControl(FibrumInput.Button button,string description,Texture tex)
	{
		Init ();
		typeOfControl.Add(1);
		controlNumToSetup.Add((int)button);
		controlDescription.Add(description);
		if( tex!=null )	textures.Add (tex);
		else textures.Add (pressKeyTex);
	}

	public void ClearControls()
	{
		Init();
		controlNumToSetup.Clear();
		controlDescription.Clear();
		textures.Clear();
		typeOfControl.Clear();
	}

	bool scanning = false;
	int currentControl=0;

	public void StartSetup()
	{
		Init ();
		currentControl=0;
		scanning = true;
		GetComponent<Canvas>().enabled = true;
		SetUI ();
	}

	void SetUI()
	{
		picture.texture = textures[currentControl];
		desc.text = controlDescription[currentControl];
	}

	public void ClearAllControlsToSetup()
	{
		Init ();
	}

	void OnGUI()
	{
		/*GUI.Box (new Rect(0f,0f,300f,30f),"Joystick: "+FibrumInput.currentJoystickName);
		string events="";
		for( int k=0; k<20; k++ )
		{
			bool f = Input.GetKey("joystick button "+k);
			if( f ) events+=""+k+": "+f+"\n";
		}
		GUI.Box (new Rect(300f,30f,300f,300f),events);
		for( int k=1; k<=20; k++ )
		GUI.Box (new Rect(0f,30f+(float)k*30f,300f,30f),""+Input.GetAxis("Axis"+k));*/
	}
}
