//Mobile Starter Pack #1 FPS v1.2c
//by Stéphane 'Azert2k' JAMIN
#pragma warning disable 0649
using UnityEngine;
using System.Collections;

[System.Serializable]
public class MSP{
	public enum HUDPos{
		LeftUpper,
		RightUpper,
		LeftLower,
		RightLower,
		None
	}
	public enum SeparateScreen{
		_HalfHalf,
		_1third2thirds,
		_2thirds1third,
		_Full
	}
	public enum HandType{
		RightHanded,
		LeftHanded
	}
	public enum GUIPad{
		AnalogPad,
		Slide,
		Button,
		None
	}
	public enum FirePos{
		window1,
		window2,
		None
	}
	public enum AxeLimit{
		AllAxis,
		XAxis,
		YAxis
	}
	public enum TOC{
		TouchCtrl,
		PauseMenu
	}
	public enum OnOff{
		On,
		Off
	}
	public enum GyroAccel{
		Accelero,
		Gyro,
		None
	}
	
	//GUI Objects Class
    [System.Serializable]
	public class GUIObj
    {
		public Texture SocleGUI;
		public Texture StickGUI;
		public Texture PauseGUI;
		public Texture WeaponGUI;
		public Texture LifeGUI;
		public Texture FourthCornerGui;
		public Texture FireGui;
		public Texture CrossHairGui;
    }
	
	//Pad Class
	private class Btn{
		public bool FingerDown = false;
		public Rect FingerBound;
		public int FingerId= -1;
		public Vector2 FingerCenter;
		public  Rect FingerStickRect;
		public  Rect FingerSocleRect;
	}
	
	//Local Var
	
	private float deadZoneinPurcent = 20;
	private float PadCorrector;
	private float PadSize;
	private Color Window1PadColor;
	private Color Window2PadColor;
	private Color TranspColor = new Color(0,0,0,0);
	private TOC TypeOfControl;
	public Gyroscope Gyro;
	private float CrossHairS;
	private float InterfWidth;
	private float InterfHeight;
	//Right/Left Handed Var
	private Rect LeftBound;
	private Rect RightBound;
	//Each Buttons Instance
	private Btn Left=new Btn();
	private Btn Right=new Btn();
	private Btn Fire=new Btn();
	private Btn AimBtn = new Btn();
	private Btn WeaponBtn = new Btn();
	private Btn PauseBtn = new Btn();
	private Rect LifeGUI = new Rect();
	private Rect FourthCornerGUI = new Rect();
	private Rect WeaponGUI = new Rect();
	private Rect PauseGUI = new Rect();
	private Vector2 FixePos;
 	private Matrix4x4 matrix ;
	
	//Public 
	public float PadSizeCorrector = 1f;
	public float CrossHSizeCorrector = 1f;
	public Vector2 FireBtnPosCorrector = new Vector2();
	public bool debugMode =false;
	public SeparateScreen LeftRightParts = SeparateScreen._1third2thirds;
	public GUIPad LeftGUIPadStyle = GUIPad.AnalogPad;
	public GUIPad RightGUIPadStyle = GUIPad.AnalogPad;
	public AxeLimit LeftAxeLimit = AxeLimit.AllAxis;
	public AxeLimit RightAxeLimit = AxeLimit.AllAxis;
	public HUDPos PausePosition = HUDPos.RightUpper ;
	public HUDPos WeaponGUIPosition = HUDPos.LeftUpper;
	public HUDPos LifeGUIPosition = HUDPos.RightLower ;
	public HUDPos FourthGUIPosition = HUDPos.LeftLower;
	public FirePos BtnFirePos = FirePos.window2;
	public OnOff Window1TapCount = OnOff.Off;
	public OnOff Window2TapCount = OnOff.Off;
	public GyroAccel AccelGyroCtrl = GyroAccel.None; 
	public Color GUIColor = new Color(1,1,1,0.8f);
	public HandType HandStyle = HandType.RightHanded;
	public Texture[] LifeCounterGUI;
	public Texture[] CrossHairGUI;
	public GUIObj GUIObject;
	
	
	[System.NonSerialized]
	public bool HideCrossHair = false;
	[System.NonSerialized]
	public bool WeaponActionEnded;
	[System.NonSerialized]
	public float WeaponTouchDiff;
	[System.NonSerialized]
	public float WeaponTouchPurcentDist;
	[System.NonSerialized]
	public float DPI;
	[System.NonSerialized]
	public Vector2 HalfScreen;
	[System.NonSerialized]
	public Vector2 HUDPosition;
	[System.NonSerialized]
	public Vector2 ScreenSize;
	
	
	//Outputs
	[System.NonSerialized]
	public Vector2 Window1InputPad;
	[System.NonSerialized]
	public Vector2 Window2InputPad;
	[System.NonSerialized]
	public Vector2 Window1InputSlide;
	[System.NonSerialized]
	public Vector2 Window2InputSlide;
	[System.NonSerialized]
	public bool WindowFireBtnPressed;
	[System.NonSerialized]
	public bool Window1Pressed;
	[System.NonSerialized]
	public bool Window2Pressed;
	[System.NonSerialized]
	public int Window1TapNbr;
	[System.NonSerialized]
	public int Window2TapNbr;
	[System.NonSerialized]
	public Quaternion GyroCoord;
	[System.NonSerialized]
	public Vector2 Acceleration;
	//[System.NonSerialized]
	public bool PauseStatus;
	//[System.NonSerialized]
	public bool AimPos;
	
	private Quaternion quatMult;
	public float GyroUpdateInterval = 1.0f;
	
	//Awake/Start Part
	public void InitControl(){
		
		//Screen Size
		ScreenSize = new Vector2(Screen.width,Screen.height);
		
		//HalfScreen Size
		HalfScreen = ScreenSize/2;
		
		//DPI Initialisation
		DPI= Screen.dpi/100;
		if (DPI == 0){DPI = 1.6f;}
		
		//Pad DPI Corrector
		PadSize = DPI * 56.25f;
		
		//CrossHair DPI Corrector
		CrossHairS = DPI*23.75f;
		
		//Alpha of Pad at Start
		Window1PadColor = TranspColor;
		Window2PadColor = TranspColor;
		
		
		
		//Screen Initialisation
		switch (LeftRightParts){
			case SeparateScreen._1third2thirds:
				LeftBound = new Rect (0,0, ScreenSize.x/3, ScreenSize.y);
				RightBound = new Rect(ScreenSize.x/3,0, ScreenSize.x/3*2, ScreenSize.y);
				break;
			case SeparateScreen._2thirds1third:
				LeftBound = new Rect (0,0, ScreenSize.x/3*2, ScreenSize.y);
				RightBound = new Rect(ScreenSize.x/3*2,0, ScreenSize.x/3, ScreenSize.y);
				break;
			case SeparateScreen._HalfHalf:
				LeftBound = new Rect (0,0, ScreenSize.x/2, ScreenSize.y);
				RightBound = new Rect(ScreenSize.x/2,0, ScreenSize.x/2, ScreenSize.y);
				break;
			case SeparateScreen._Full:
				LeftBound = new Rect (0,0, ScreenSize.x, ScreenSize.y);
				RightBound = new Rect(0,0,0,0);
				break;
		}
		//Right/Left Handed Init
		switch (HandStyle){
			case HandType.RightHanded:
				Left.FingerBound = LeftBound;
				Right.FingerBound = RightBound;
				break;
			case HandType.LeftHanded:
				Left.FingerBound = RightBound;
				Right.FingerBound = LeftBound;
				break;
		}
		
		//Gyroscope Init
		if(AccelGyroCtrl== GyroAccel.Gyro && SystemInfo.supportsGyroscope){
				Gyro = Input.gyro;
				Gyro.enabled = true;
				Gyro.updateInterval =GyroUpdateInterval;
				quatMult = AltQuatMult();
		}
		
		
		//GUI Start
		InterfWidth = (ScreenSize.x*28.69f)/100;
		InterfHeight = 137f/350f*InterfWidth;
		
		//Apply GUI Corners Bounds
		WeaponGUI = SwitchBound((int)WeaponGUIPosition,"GUI");
		PauseGUI = SwitchBound((int)PausePosition,"GUI");
		FourthCornerGUI = SwitchBound((int)FourthGUIPosition,"GUI");
		LifeGUI = SwitchBound((int)LifeGUIPosition,"GUI");
		
		//Apply Btn Bounds
		PauseBtn.FingerBound = SwitchBound((int)PausePosition,"Btn");
		WeaponBtn.FingerBound = SwitchBound((int)WeaponGUIPosition,"Btn");
		AimBtn.FingerBound = SwitchBound((int)FourthGUIPosition,"Btn");
	
		//PlayerPrefs Init
		PadSizeCorrector=1;
		
		//Apply Pad Size Corrector 
		PadCorrector = PadSize*PadSizeCorrector;
		
		//Fire Button Initialisation
		switch (BtnFirePos){
			case FirePos.window2:
				FixePos = new Vector2((RightBound.x + RightBound.width/3*2), (RightBound.height/4*2));
			break;
		case FirePos.window1:
				FixePos = new Vector2((LeftBound.x + LeftBound.width/3*2), (LeftBound.height/4*2));
			break;
		}
	}
	
	//Update Part
	public void Command()
	{
		//Controler Switch
		switch(TypeOfControl){
			case TOC.TouchCtrl:
				InGameTouchCtrl();
				break;
			case TOC.PauseMenu:
				break;
		}
	}
	
	//Update Touch
	private void InGameTouchCtrl()
	{
		switch (AccelGyroCtrl){
			case GyroAccel.Gyro:
           	 	GyroCoord = Gyro.attitude*quatMult;
				break;
			case GyroAccel.Accelero:
				Acceleration = new Vector2(Input.acceleration.x, Input.acceleration.y);
				break;
		}
		foreach  (Touch touch in Input.touches)
		{
			//Began Touch Phase
			if (touch.phase == TouchPhase.Began) 
			{
				//Pause Btn
				if (!PauseBtn.FingerDown){
					PauseBtn.FingerDown = PauseBtn.FingerBound.Contains(touch.position);
					if (PauseBtn.FingerDown){
						PauseBtn.FingerId = touch.fingerId;
						PauseStatus = !PauseStatus;
						continue;
					}
			 	}
				
				//Weapon btn
				if (!WeaponBtn.FingerDown){
					WeaponBtn.FingerDown = WeaponBtn.FingerBound.Contains(touch.position);
					if (WeaponBtn.FingerDown){
						WeaponBtn.FingerId = touch.fingerId;
						WeaponBtn.FingerCenter = new Vector2(touch.position.x,0);
						continue;
					}
			 	}
				
				//Aim btn New 1.2
				if (!AimBtn.FingerDown){
					AimBtn.FingerDown = AimBtn.FingerBound.Contains(touch.position);
					if (AimBtn.FingerDown){
						AimBtn.FingerId = touch.fingerId;
						AimPos = !AimPos;
						continue;
					}
			 	}
				
				//Left Pad Control
				switch (LeftGUIPadStyle){
					case GUIPad.AnalogPad:
						if (!Left.FingerDown){
							Left.FingerDown = Left.FingerBound.Contains(touch.position);
							if(Left.FingerDown){
								Left.FingerId = touch.fingerId;
								Left.FingerCenter = touch.position;
								Left.FingerSocleRect = Left.FingerStickRect= new Rect(Left.FingerCenter.x- PadCorrector/2, ScreenSize.y-(Left.FingerCenter.y)-PadCorrector/2,PadCorrector,PadCorrector);
								Window1PadColor = GUIColor; 
								continue;
							}
						}
						break;
					case GUIPad.Slide:
						if(!Left.FingerDown){
							Left.FingerDown = Left.FingerBound.Contains(touch.position);
							if(Left.FingerDown){
								Left.FingerId = touch.fingerId;
								switch (BtnFirePos){
									case FirePos.window1:
											if(Fire.FingerBound.Contains(touch.position)){
												WindowFireBtnPressed = true;
											}
										break;
								}
								switch (Window1TapCount){
									case OnOff.On:
										Window1TapNbr = touch.tapCount;
									break;
								}
								continue;
							}
						}
						break;
					case GUIPad.Button:
						if (!Left.FingerDown){
							Left.FingerDown = Left.FingerBound.Contains(touch.position);
							if(Left.FingerDown){
								Left.FingerId = touch.fingerId;
								Window1Pressed = true;
								switch (Window1TapCount){
									case OnOff.On:
										Window1TapNbr = touch.tapCount;
									break;
								}
								continue;
							}
						}
						break;
				}
				//Right Pad Control
				switch (RightGUIPadStyle){
					case GUIPad.AnalogPad:
						if (!Right.FingerDown){
							Right.FingerDown = Right.FingerBound.Contains(touch.position);
							if(Right.FingerDown){
								Right.FingerId = touch.fingerId;
								Right.FingerCenter = touch.position;
								Right.FingerSocleRect = Right.FingerStickRect= new Rect(Right.FingerCenter.x- PadCorrector/2, ScreenSize.y-(Right.FingerCenter.y)-PadCorrector/2,PadCorrector,PadCorrector);
								Window2PadColor = GUIColor; 
								continue;
							}
						}
						break;
					case GUIPad.Slide:
						if(!Right.FingerDown){
							Right.FingerDown = Right.FingerBound.Contains(touch.position);
							if(Right.FingerDown){
								Right.FingerId = touch.fingerId;
								switch (BtnFirePos){
									case FirePos.window2:
											if(Fire.FingerBound.Contains(touch.position)){
												WindowFireBtnPressed = true;
											}
									break;
								}
								switch (Window2TapCount){
									case OnOff.On:
									Window2TapNbr = touch.tapCount;
									break;
								}
								continue;
							}
						}
						break;
					case GUIPad.Button:
						if (!Right.FingerDown){
							Right.FingerDown = Right.FingerBound.Contains(touch.position);
							if(Right.FingerDown){
								Right.FingerId = touch.fingerId;
								Window2Pressed = true;
								switch (Window2TapCount){
									case OnOff.On:
									Window2TapNbr = touch.tapCount;
									break;
								}
								continue;
							}
						}
						break;
				}
				
				
			}
			//Move Phase Begin
			else if (touch.phase == TouchPhase.Moved)
			{
				//Weapon btn
				if (WeaponBtn.FingerDown && WeaponBtn.FingerId == touch.fingerId){
						WeaponTouchDiff = touch.position.x - WeaponBtn.FingerCenter.x; 
						WeaponTouchPurcentDist = Mathf.Abs(WeaponTouchDiff*100/WeaponBtn.FingerBound.width);
				}
				
				
				//Left Pad Control
				switch (LeftGUIPadStyle){
					case GUIPad.AnalogPad:
						if (Left.FingerDown && Left.FingerId == touch.fingerId){
							float StickDistanceMax = PadCorrector/1.5f;
							Vector2 MDiff = new Vector2();
							switch (LeftAxeLimit){
								case AxeLimit.AllAxis:
									MDiff = touch.position - Left.FingerCenter;
									break;
								case AxeLimit.XAxis:
									MDiff = new Vector2(touch.position.x,Left.FingerCenter.y) - Left.FingerCenter;
									break;
								case AxeLimit.YAxis:
									MDiff = new Vector2(Left.FingerCenter.x, touch.position.y) - Left.FingerCenter;
									break;
							}
							float distancePourcentage = (MDiff.magnitude*100)/StickDistanceMax;
							if(distancePourcentage >=100)distancePourcentage =100;
							if (distancePourcentage > deadZoneinPurcent){
								Window1InputPad = (MDiff.normalized*distancePourcentage)/100;
							}
							else {
								Window1InputPad= Vector2.zero;
							}
							Vector2 ClampVector = Vector2.ClampMagnitude(MDiff,StickDistanceMax);
							Left.FingerStickRect = new Rect((Left.FingerCenter.x + ClampVector.x)-PadCorrector/2, ScreenSize.y-(Left.FingerCenter.y+ClampVector.y)-PadCorrector/2,PadCorrector,PadCorrector);
						}
						break;
					case GUIPad.Slide:
						if (Left.FingerDown && Left.FingerId == touch.fingerId){
						switch (LeftAxeLimit){
							case AxeLimit.AllAxis:
								Window1InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
								break;
							case AxeLimit.XAxis:
								Window1InputSlide = new Vector2(touch.deltaPosition.x,0)*Time.smoothDeltaTime;
								break;
							case AxeLimit.YAxis:
								Window1InputSlide = new Vector2(0,touch.deltaPosition.y)*Time.smoothDeltaTime;
								break;
							}
						}
						break;
				}
				
				//Right Pad Control
				switch (RightGUIPadStyle){
					case GUIPad.AnalogPad:
						if (Right.FingerDown && Right.FingerId == touch.fingerId){
							float StickDistanceMax = PadCorrector/1.5f;
							Vector2 MDiff = new Vector2();
								switch (RightAxeLimit){
								case AxeLimit.AllAxis:
									MDiff = touch.position - Right.FingerCenter;
									break;
								case AxeLimit.XAxis:
									MDiff = new Vector2(touch.position.x,Right.FingerCenter.y) - Right.FingerCenter;
									break;
								case AxeLimit.YAxis:
									MDiff = new Vector2(Right.FingerCenter.x, touch.position.y) - Right.FingerCenter;
									break;
							}
							float distancePourcentage = (MDiff.magnitude*100)/StickDistanceMax;
							if(distancePourcentage >=100)distancePourcentage =100;
							if (distancePourcentage > deadZoneinPurcent){
								Window2InputPad = (MDiff.normalized*distancePourcentage)/100;
							}else {
								Window2InputPad= Vector2.zero;
							}
							Vector2 ClampVector = Vector2.ClampMagnitude(MDiff,StickDistanceMax);
							Right.FingerStickRect = new Rect((Right.FingerCenter.x + ClampVector.x)-PadCorrector/2, ScreenSize.y-(Right.FingerCenter.y+ClampVector.y)-PadCorrector/2,PadCorrector,PadCorrector);
						}
						break; 
					case GUIPad.Slide:
						if (Right.FingerDown && Right.FingerId == touch.fingerId){
							switch (RightAxeLimit){
									case AxeLimit.AllAxis:
										Window2InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
										break;
									case AxeLimit.XAxis:
										Window2InputSlide = new Vector2(touch.deltaPosition.x,0)*Time.smoothDeltaTime;
										break;
									case AxeLimit.YAxis:
										Window2InputSlide = new Vector2(0,touch.deltaPosition.y)*Time.smoothDeltaTime;
										break;
								}
						}
						break;
				}
			}
			//Stationary Phase Begin
			else if (touch.phase == TouchPhase.Stationary)
			{
				switch (LeftGUIPadStyle){
					case GUIPad.Slide:
					if (Left.FingerDown && Left.FingerId == touch.fingerId){
						Window1InputSlide= Vector2.zero;
					}
					break;
				}
				
				switch (RightGUIPadStyle){
					case GUIPad.Slide:
					if (Right.FingerDown && Right.FingerId == touch.fingerId){
						Window2InputSlide= Vector2.zero;
					}
					break;
				}
			}
			else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)//End/Canceled Phase Begin
			{
				if (WeaponBtn.FingerDown && WeaponBtn.FingerId == touch.fingerId){
						WeaponActionEnded = true;
						WeaponBtn.FingerDown = false;
						WeaponBtn.FingerCenter = Vector2.zero;
						WeaponBtn.FingerId = -1;
					}
				
				//Aim btn New 1.2
				if (AimBtn.FingerDown && AimBtn.FingerId == touch.fingerId){
						AimBtn.FingerDown = false;
						AimBtn.FingerId = -1;
				}
				
				//Left Pad Control
				switch (LeftGUIPadStyle){
					case GUIPad.AnalogPad:
						if (Left.FingerDown && Left.FingerId == touch.fingerId){
							Window1InputPad =  Vector2.zero;
							Left.FingerDown = false;
							Left.FingerId = -1;
							Window1PadColor = TranspColor;
						}
						break;
					case GUIPad.Slide:
						if (Left.FingerDown && Left.FingerId == touch.fingerId){
							Window1InputSlide =  Vector2.zero;
							Left.FingerDown = false;
							Left.FingerId = -1;
							switch (BtnFirePos){
								case FirePos.window1:	
										WindowFireBtnPressed = false;
								break;
							}
							switch (Window1TapCount){
								case OnOff.On:
								Window1TapNbr = 0;
								break;
							}
						}
						break;
					case GUIPad.Button:
						if (Left.FingerDown && Left.FingerId == touch.fingerId){
							Window1Pressed = false;
							Left.FingerDown = false;
							Left.FingerId = -1;
							switch (Window1TapCount){
								case OnOff.On:
								Window1TapNbr = 0;
								break;
							}
						}
						break;
				}
				
				//Right Pad Control
				switch (RightGUIPadStyle){
					case GUIPad.AnalogPad:
						if (Right.FingerDown && Right.FingerId == touch.fingerId){
							Window2InputPad =  Vector2.zero;
							Right.FingerDown = false;
							Right.FingerId = -1;
							Window2PadColor = TranspColor;
						}
						break;
					case GUIPad.Slide:
						if (Right.FingerDown && Right.FingerId == touch.fingerId){
							Window2InputSlide =  Vector2.zero;
							Right.FingerDown = false;
							Right.FingerId = -1;
							switch (BtnFirePos){
							case FirePos.window2:
								WindowFireBtnPressed = false;
								break;
							}
							switch (Window2TapCount){
								case OnOff.On:
								Window2TapNbr = 0;
								break;
							}
						}
						break;
					case GUIPad.Button:
						if (Right.FingerDown && Right.FingerId == touch.fingerId){
							Window2Pressed = false;
							Right.FingerDown = false;
							Right.FingerId = -1;
							switch (Window2TapCount){
								case OnOff.On:
								Window2TapNbr = 0;
								break;
							}
						}
					break;
				}
			}
		}
	}
	
	//OnGUI Part
	public void OnGUIComponents(){
		
		//To See input values
		if(debugMode){
			DebugInfo();
		}	
		//Left Pad Control
		if (LeftGUIPadStyle == GUIPad.AnalogPad){
				GUI.color = Window1PadColor;
				GUI.DrawTexture (Left.FingerSocleRect,GUIObject.SocleGUI);//Socle
				GUI.DrawTexture (Left.FingerStickRect,GUIObject.StickGUI);//Stick
		}else if(Window1PadColor != TranspColor){
				GUI.color = TranspColor;
		}
		//Right Pad Control
		if (RightGUIPadStyle == GUIPad.AnalogPad){
				GUI.color = Window2PadColor;
				GUI.DrawTexture (Right.FingerSocleRect,GUIObject.SocleGUI);//Socle
				GUI.DrawTexture (Right.FingerStickRect,GUIObject.StickGUI);//Stick
		}else if(Window2PadColor != TranspColor){
				GUI.color = TranspColor;
		}
		//color of the rest
		GUI.color = GUIColor;
		
		
		//Fire Button
		Fire.FingerBound = new Rect(FixePos.x-PadSize*PadSizeCorrector/2+FireBtnPosCorrector.x, FixePos.y - PadSize*PadSizeCorrector/2-FireBtnPosCorrector.y, PadSize*PadSizeCorrector,PadSize*PadSizeCorrector);
		GUI.DrawTexture (new Rect(Fire.FingerBound.x,ScreenSize.y-Fire.FingerBound.y-PadSize*PadSizeCorrector, Fire.FingerBound.width,Fire.FingerBound.height),GUIObject.FireGui);//Fire
		
		//Move GUI PART
		matrix = Matrix4x4.TRS(new Vector3(HUDPosition.x,HUDPosition.y,0), Quaternion.identity, Vector3.one);
		GUI.matrix = matrix;
		
		//Render each bounds
		if(GUIObject.WeaponGUI != null && WeaponGUI != new Rect(0,0,0,0)){
			GUI.DrawTexture (WeaponGUI, GUIObject.WeaponGUI);
		}
		if(GUIObject.LifeGUI != null && LifeGUI != new Rect(0,0,0,0)){
		GUI.DrawTexture (LifeGUI, GUIObject.LifeGUI);
		}
		if(GUIObject.PauseGUI != null && PauseGUI != new Rect(0,0,0,0)){
			GUI.DrawTexture (PauseGUI, GUIObject.PauseGUI);
		}
		if(GUIObject.FourthCornerGui != null && FourthCornerGUI != new Rect(0,0,0,0)){
			GUI.DrawTexture (FourthCornerGUI, GUIObject.FourthCornerGui);
		}
		
		//GUI Crosshair
		if(!HideCrossHair && GUIObject.CrossHairGui){
			GUI.DrawTexture (new Rect (HalfScreen.x-CrossHairS*CrossHSizeCorrector/2,HalfScreen.y-CrossHairS*CrossHSizeCorrector/2,CrossHairS*CrossHSizeCorrector,CrossHairS*CrossHSizeCorrector), GUIObject.CrossHairGui);
		}
	}
	
	//Assign bounds Side 0 = LeftUp 1 = RightUp 2 = LeftLow 3= RightLow .Y values must be reverse between GUI and Touch
	public Rect SwitchBound(int Side, string GUIBtn){
		Rect Bound = new Rect();
		float YPosUp;
		float YPosDwn;
		if(GUIBtn == "GUI"){
			YPosUp = 0;	
			YPosDwn	= ScreenSize.y-InterfHeight;
		}else{
			YPosUp = ScreenSize.y-InterfHeight;	
			YPosDwn	= 0;
		}
		
		switch (Side){
			case 0:
				Bound = new Rect (0,YPosUp,InterfWidth,InterfHeight);
			break;
			case 1:
				Bound = new Rect (ScreenSize.x-InterfWidth,YPosUp,InterfWidth,InterfHeight);
			break;
			case 2:
				Bound = new Rect (0,YPosDwn,InterfWidth,InterfHeight);
			break;
			case 3:
				Bound = new Rect (ScreenSize.x-InterfWidth,YPosDwn,InterfWidth,InterfHeight);
			break;
		}
		return Bound;
	}
	
	 Quaternion AltQuatMult()
    {
        Quaternion QuatMult = Quaternion.identity;
		 #if UNITY_IPHONE
        if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
            QuatMult = new Quaternion(0f,0f,0.7071f,0.7071f);
        } else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
            QuatMult = new Quaternion(0f,0f,-0.7071f,0.7071f);
        } else if (Screen.orientation == ScreenOrientation.Portrait) {
            QuatMult = new Quaternion(0f,0f,1f,0f);
        } else if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
            QuatMult = new Quaternion(0f,0f,0f,1f);
        }
		#endif
        #if UNITY_ANDROID
		if (Screen.orientation == ScreenOrientation.PortraitUpsideDown) {
             QuatMult = new Quaternion(0f,0f,0.7071f,-0.7071f);
        } else if (Screen.orientation == ScreenOrientation.Portrait) {
             QuatMult = new Quaternion(0f,0f,-0.7071f,-0.7071f);
        } else if (Screen.orientation == ScreenOrientation.LandscapeRight) {
            QuatMult = new Quaternion(0f,0f,0f,1f);
		} else if (Screen.orientation == ScreenOrientation.LandscapeLeft) {
			QuatMult = new Quaternion(0f,0f,1f,0f);
		}
		#endif
        return(QuatMult);
    }
	
	private void DebugInfo(){
			GUI.Box(new Rect(0,130,320,350), "");
			GUI.Label(new Rect(0,130,300,30), "GyroCorrector : "+ quatMult.ToString());
			GUI.Label(new Rect(0,160,300,30), "Fire : "+ PauseStatus.ToString());
			GUI.Label(new Rect(0,190,300,30), "PadLeft : "+ Window1InputPad.ToString());
			GUI.Label(new Rect(0,220,300,30), "PadRight : "+ Window2InputPad.ToString());
			GUI.Label(new Rect(0,250,300,30), "SlideLeft : "+ Window1InputSlide.ToString());
			GUI.Label(new Rect(0,280,300,30), "SlideRight : "+ Window2InputSlide.ToString());
			GUI.Label(new Rect(0,310,300,30), "Window1Pressed : "+ Window1Pressed.ToString());
			GUI.Label(new Rect(0,340,300,30), "Window2Pressed : "+ Window2Pressed.ToString());
			GUI.Label(new Rect(0,370,300,30), "Window1TapNbr : "+ Window1TapNbr.ToString());
			GUI.Label(new Rect(0,400,300,30), "Window2TapNbr : "+ Window2TapNbr.ToString());
			GUI.Label(new Rect(0,430,300,30), "Accelerometer : "+ Acceleration.ToString());
			GUI.Label(new Rect(0,460,300,30), "Gyro Rotation: "+ GyroCoord.ToString());
	}
}