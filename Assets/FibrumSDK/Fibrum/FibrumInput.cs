using UnityEngine;
using System.Collections;

public static class FibrumInput{

	public enum FibJoystick {Vertical,Horizontal,ButLeft,ButRight,ButUp,ButDown,ButPower};
	static public int[] joystickButtons = {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19};
	static public int[] joystickAxis = {1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20};
	static public float[] joystickAxisDir = {1,-1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1};
	public enum Axis:int {Horizontal1=0,Vertical1=1,Scroll=2,Horizontal2=3,Vertical2=4,Horizontal3=5,Vertical3=6,Axis8=7,AnalogLeft=8,AnalogRight=9,Axis11=10,Axis12=11,Axis13=12,Axis14=13,Axis15=14,Axis16=15,Axis17=16,Axis18=17,Axis19=18,Axis20=19};
	public enum Button:int {A=0,B=1,X=2,Y=3,LeftControl=4,RightControl=5,Back=6,Start=7,LeftJoystick=8,RightJoystick=9,Button10=10,Button11=11,Button12=12,Button13=13,Button14=14,Button15=15,Button16=16,Button17=17,Button18=18,Button19=19};
	public enum Control:int {Horizontal1=0,Vertical1=1,Scroll=2,Horizontal2=3,Vertical2=4,Horizontal3=5,Vertical3=6,Axis8=7,AnalogLeft=8,AnalogRight=9,Axis11=10,Axis12=11,Axis13=12,Axis14=13,Axis15=14,Axis16=15,Axis17=16,Axis18=17,Axis19=18,Axis20=19,A=100,B=101,X=102,Y=103,LeftControl=104,RightControl=105,Back=106,Start=107,LeftJoystick=108,RightJoystick=109,Button10=110,Button11=111,Button12=112,Button13=113,Button14=114,Button15=115,Button16=116,Button17=117,Button18=118,Button19=119};

#if UNITY_IOS
	//					         {A=0,                B=1,                 X=2,            Y=3,     LeftControl=4,          RightControl=5,           Back=6,     Start=7,     LeftJoystick=8,           RightJoystick=9,            Button10=10,    Button11=11,Button12=12,Button13=13,Button14=14,Button15=15,Button16=16,Button17=17,Button18=18,Button19=19};
	static int[] iosButtons = {(int)Button.LeftJoystick,(int)Button.RightJoystick,(int)Button.Button10,(int)Button.A,(int)Button.Button19,     (int)Button.Button19,      (int)Button.Button19,(int)Button.Button19,(int)Button.LeftJoystick,      (int)Button.RightJoystick,       (int)Button.Button12,(int)Button.Button11   ,(int)Button.Button12,   (int)Button.Button13,   (int)Button.Button14,   (int)Button.Button15,   (int)Button.Button16,   (int)Button.Button17,   (int)Button.Button18,   (int)Button.Button19};
#endif

	public static float GetJoystickAxis(Axis axisNum)
	{
#if UNITY_IOS //&& !UNITY_EDITOR
		if( axisNum == Axis.Horizontal1 )	return (Input.GetKey("joystick button "+(int)Button.RightControl)?1f:0f)-(Input.GetKey("joystick button "+(int)Button.Start)?1f:0f);
		else if( axisNum == Axis.Vertical1 ) return (Input.GetKey("joystick button "+(int)Button.LeftControl)?1f:0f)-(Input.GetKey("joystick button "+(int)Button.Back)?1f:0f);
		return 0f;
#else
		if( axisNum == Axis.Horizontal1 ) return Input.GetAxis("Axis"+joystickAxis[(int)axisNum])*joystickAxisDir[(int)axisNum]+Input.GetAxis("Horizontal");
		else if( axisNum == Axis.Vertical1 ) return Input.GetAxis("Axis"+joystickAxis[(int)axisNum])*joystickAxisDir[(int)axisNum]+Input.GetAxis("Vertical");
		else return Input.GetAxis("Axis"+joystickAxis[(int)axisNum])*joystickAxisDir[(int)axisNum];
#endif
	}

	public static bool GetJoystickButton(Button buttonNum)
	{
#if UNITY_IOS
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKey("joystick button "+iosButtons[(int)buttonNum])||(Input.GetMouseButton(0));
		}
		else return Input.GetKey("joystick button "+iosButtons[(int)buttonNum]);
		//buttonNum = iosButtons[(int)buttonNum];
#else
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKey("joystick button "+joystickButtons[(int)buttonNum])||(Input.GetMouseButton(0));
		}
		else return Input.GetKey("joystick button "+joystickButtons[(int)buttonNum]);
#endif
	}

	public static bool GetJoystickButtonDown(Button buttonNum)
	{
#if UNITY_IOS
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKeyDown("joystick button "+iosButtons[(int)buttonNum])||(Input.GetMouseButtonDown(0));
		}
		return Input.GetKeyDown("joystick button "+iosButtons[(int)buttonNum]);
#else
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKeyDown("joystick button "+joystickButtons[(int)buttonNum])||(Input.GetMouseButtonDown(0));
		}
		return Input.GetKeyDown("joystick button "+joystickButtons[(int)buttonNum]);
#endif
	}

	public static bool GetJoystickButtonUp(Button buttonNum)
	{
#if UNITY_IOS
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKeyUp("joystick button "+iosButtons[(int)buttonNum])||(Input.GetMouseButtonUp(0));
		}
		return Input.GetKeyUp("joystick button "+iosButtons[(int)buttonNum]);
#else
		if( buttonNum == Button.RightControl || buttonNum == Button.A )
		{
			return Input.GetKeyUp("joystick button "+joystickButtons[(int)buttonNum])||(Input.GetMouseButtonUp(0));
		}
		return Input.GetKeyUp("joystick button "+joystickButtons[(int)buttonNum]);
#endif
	}

	public static bool GetJoystickAnyButtonDown()
	{
		foreach(Button but in Button.GetValues(typeof(Button)))
		{
			if( GetJoystickButtonDown(but) ) return true;
		}
		return false;
	}

	public static bool GetJoystickAnyAxis()
	{
		foreach(Axis axis in Axis.GetValues(typeof(Axis)))
		{
			if( Mathf.Abs (GetJoystickAxis(axis))>0.1f ) return true;
		}
		return false;
	}

	public static bool LoadJoystickPrefs()
	{
		//MonoBehaviour.print("Loading joystick: "+currentJoystickName);
		if( PlayerPrefs.HasKey("FIB_joystickPrefs_"+currentJoystickName) )
		{
			//MonoBehaviour.print("Found!");
			string str=PlayerPrefs.GetString("FIB_joystickPrefs_"+currentJoystickName);
			//MonoBehaviour.print(str);
			char[] dc={';'};
			string[] controls = str.Split (dc);
			for( int k=0; k<20 && k<controls.Length; k++ ) {joystickButtons[k]=int.Parse(controls[k]);}
			for( int k=20; k<40 && k<controls.Length; k++ ) {joystickAxis[k-20]=int.Parse(controls[k]);}
			for( int k=40; k<60 && k<controls.Length; k++ ) {joystickAxisDir[k-40]=float.Parse(controls[k]);}
			return true;
		}
		return false;
	}

	public static void SaveJoystickPrefs()
	{
		//MonoBehaviour.print("Saving joystick: "+currentJoystickName);
		string controls = "";
		for( int k=0; k<20; k++ )
		{
			controls += joystickButtons[k]+";";
		}
		for( int k=20; k<40; k++ )
		{
			controls += joystickAxis[k-20]+";";
		}
		for( int k=40; k<60; k++ )
		{
			controls += (int)joystickAxisDir[k-40]+";";
		}
		PlayerPrefs.SetString("FIB_joystickPrefs_"+currentJoystickName,controls);
		//MonoBehaviour.print(controls);
	}

	public static int ScanAndSetupAxis(Axis axisName)
	{
		for( int k=0; k<20; k++ )
		{
			if( Mathf.Abs (Input.GetAxis("Axis"+(k+1)))>0.5f )
			{
				joystickAxisDir[(int)axisName]=1f;
				joystickAxis[(int)axisName]=k+1;
				SaveJoystickPrefs();
				return k;
			}
		}
		return -1;
	}

	public static int ScanAndSetupAxisDir(Axis axisName,bool dirForward=true)
	{
		for( int k=0; k<20; k++ )
		{
			if( Mathf.Abs (GetJoystickAxis((Axis)k))>0.5f )
			{
				if( dirForward!=(GetJoystickAxis((Axis)k)>0f) ) joystickAxisDir[(int)axisName]=-1f;
				else joystickAxisDir[(int)axisName]=1f;
				joystickAxis[(int)axisName]=k+1;
				SaveJoystickPrefs();
				return k;
			}
		}
		return -1;
	}

	public static int ScanAndSetupButton(Button buttonName)
	{
		for( int k=0; k<20; k++ )
		{
#if UNITY_IOS
			if( Input.GetKey("joystick button "+k) )
			{
				iosButtons[(int)buttonName]=k;
				SaveJoystickPrefs();
				return k;
			}
#else
			if( Input.GetKey("joystick button "+k) )
			{
				joystickButtons[(int)buttonName]=k;
				SaveJoystickPrefs();
				return k;
			}
#endif
		}
		return -1;
	}

	public static string currentJoystickName
	{
		get
		{
			string[] joysticks = Input.GetJoystickNames();
			if( joysticks.Length>0 ) return joysticks[0];
			else return "";
		}
	}

	public static float FibAxis(FibJoystick axis)
	{
		if( axis==FibJoystick.Vertical ) return -Input.GetAxis("Axis1");
		else if( axis==FibJoystick.Horizontal ) return -Input.GetAxis ("Axis2");
		else if( axis==FibJoystick.ButLeft ) return Input.GetKey("joystick button 0")?1f:0f;
		else if( axis==FibJoystick.ButRight ) return Input.GetKey("joystick button 3")?1f:0f;
		else if( axis==FibJoystick.ButUp ) return Input.GetKey("joystick button 2")?1f:0f;
		else if( axis==FibJoystick.ButDown ) return Input.GetKey("joystick button 3")?1f:0f;
		else if( axis==FibJoystick.ButPower ) return Input.GetKey("joystick button 10")?1f:0f;
		else return 0f;
	}

	public static bool FibButton(FibJoystick axis)
	{
		if( axis==FibJoystick.ButLeft ) return Input.GetKey("joystick button 0");
		else if( axis==FibJoystick.ButRight ) return Input.GetKey("joystick button 3");
		else if( axis==FibJoystick.ButUp ) return Input.GetKey("joystick button 2");
		else if( axis==FibJoystick.ButDown ) return Input.GetKey("joystick button 3");
		else if( axis==FibJoystick.ButPower ) return Input.GetKey("joystick button 10");
		else return false;
	}

	public static bool FibButtonDown(FibJoystick axis)
	{
		if( axis==FibJoystick.ButLeft ) return Input.GetKeyDown("joystick button 0");
		else if( axis==FibJoystick.ButRight ) return Input.GetKeyDown("joystick button 3");
		else if( axis==FibJoystick.ButUp ) return Input.GetKeyDown("joystick button 2");
		else if( axis==FibJoystick.ButDown ) return Input.GetKeyDown("joystick button 3");
		else if( axis==FibJoystick.ButPower ) return Input.GetKeyDown("joystick button 10");
		else return false;
	}

	public static bool FibButtonUp(FibJoystick axis)
	{
		if( axis==FibJoystick.ButLeft ) return Input.GetKeyUp("joystick button 0");
		else if( axis==FibJoystick.ButRight ) return Input.GetKeyUp("joystick button 3");
		else if( axis==FibJoystick.ButUp ) return Input.GetKeyUp("joystick button 2");
		else if( axis==FibJoystick.ButDown ) return Input.GetKeyUp("joystick button 3");
		else if( axis==FibJoystick.ButPower ) return Input.GetKeyUp("joystick button 10");
		else return false;
	}

	static public JoystickSetup joystickSetupGO;

	static public bool InitializeJoystickSetup()
	{
		if( joystickSetupGO==null )
		{
			joystickSetupGO = (GameObject.Instantiate((GameObject)Resources.Load("FibrumResources/JoystickSetup",typeof(GameObject))) as GameObject).GetComponent<JoystickSetup>();
			JoystickSetupParameters jsp = GameObject.FindObjectOfType<JoystickSetupParameters>();
			if( jsp!=null )
			{
				if( jsp.enabled )
				{
					for( int k=0; k<jsp.controlsToSetup.Length; k++ )
					{
						if( (int)jsp.controlsToSetup[k].controlToSetup<100 )
						{
							if( jsp.controlsToSetup[k].checkDirection == JoystickSetupParameters.CheckDirection.Disable )	joystickSetupGO.SetControl((FibrumInput.Axis)((int)jsp.controlsToSetup[k].controlToSetup),jsp.controlsToSetup[k].description,jsp.controlsToSetup[k].picture);
							else if( jsp.controlsToSetup[k].checkDirection == JoystickSetupParameters.CheckDirection.CheckForward )	joystickSetupGO.SetControl((FibrumInput.Axis)((int)jsp.controlsToSetup[k].controlToSetup),true,jsp.controlsToSetup[k].description,jsp.controlsToSetup[k].picture);
							else if( jsp.controlsToSetup[k].checkDirection == JoystickSetupParameters.CheckDirection.CheckBack )	joystickSetupGO.SetControl((FibrumInput.Axis)((int)jsp.controlsToSetup[k].controlToSetup),false,jsp.controlsToSetup[k].description,jsp.controlsToSetup[k].picture);
						}
						else
						{
							joystickSetupGO.SetControl((FibrumInput.Button)((int)jsp.controlsToSetup[k].controlToSetup-100),jsp.controlsToSetup[k].description,jsp.controlsToSetup[k].picture);
						}
					}
				}
			}
			return true;
		}
		else return false;
	}

}
