#pragma strict
//Mobile Starter Pack #1 FPS v1.2c
//by Stéphane 'Azert2k' JAMIN
@script RequireComponent(CharacterController)
	public enum RotateC{
		Classic,
		Accelerometer,
		gyroscope
	}
	public var RotateControl : RotateC = RotateC.Classic;
	public var AccelerometerSensibility :float= 1.5f;
	public var AccelAngleCorrector :float= 135f;
	
	public var GyroSmooth : float = 0.1f;
	
	enum GUIComponent{
		Health,
		Bullet,
		Clip,
		Init 
	}
	@SerializeField
	public var MSPControl : MSP = new MSP();
	public var Sparkle :GameObject;
	public var AxeArms : Transform;
	public var PlayerCam : Transform;
	public var Sensitivity :float= 1.5;
    private var moveDirection :Vector3= Vector3.zero;
	public var speed :float= 10;
    public var gravity :float= 8.0;
    private var controller : CharacterController;
	private var rotationY:float = 0.0;
	private var rotationX:float = 0.0;
	private var sensitivityX:float = 2000;
	private var sensitivityY:float = 900;
	private var minimumY:float = -60;
	private var maximumY:float = 60;
	private var originalRotation:float;
    private var myTransform : Transform;
	private var Health : int;
	private var BulletGUI : int;
	private var hit :RaycastHit;
	private var AxeXPos:float = 0;
	private var AxeYPos:float = 0;
	private var AxeZPos:float = 0;
	private var AxeYCoef:float=0;
	private var ZsmoothVal:float = 8;
	private var GUIPosX:float;
	private var GUIPosXMax:float;
	private var GUIPosY:float;
	private var GUIPosYMax:float;
	private var GUIPosY2:float;
	private var GUIPosY2Max:float;
	private var InputX:float;
	private var InputY:float;
	private var MaxHealth:float;
	private var MaxClip:float;
	public var Weapon1R:Renderer;
	public var Weapon2R:Renderer;
	public var Arm1R:Renderer;
	public var Arm2R:Renderer;
	private var Inclin:float;
	public var TimeBeforeHitAgain:float = 2;
	private var TBHA:float;
	
	
	//New in 1.2 AIM POSITION
	private var RotateCoef:float;
	
	
	public class WeaponClass{
		public var firearms : boolean;
		public var fireRate:float;
		public var bulletperClip : int;
		public var WeaponPower : int;
		public var Muzzle : Renderer;
		public var shootSound:AudioClip;
		public var WeaponBulletGUI :Texture[];
		public var MaxNbrClip:int;
		public var NbClip:float;
		public var bulletleft:int;
		public var nextFireTime:float;
		public var bulletinMagasine:float;
		//New in 1.2 AIM POSITION
		public var AimPosition : Vector3 = new Vector3(-0.1615, -0.044,-0.45);
		public var AimAngle :float = 358.5694f;
	}
	
	private var CurrentWeapon :WeaponClass;
	
	@SerializeField
	public var WeaponList :WeaponClass[];
	
	public var collisionLayers :LayerMask = -1;
	private var muzzleRotate :int = 45;
	private var reload : boolean;
	
	var Fakecam : Transform;
	function Start(){
	 	//If the rotation is controled by accelero New In 1.2
		if(RotateControl == RotateC.Accelerometer){
			MSPControl.AccelGyroCtrl = MSP.GyroAccel.Accelero;	
		}else if (RotateControl == RotateC.gyroscope && SystemInfo.supportsGyroscope){	
			MSPControl.AccelGyroCtrl = MSP.GyroAccel.Gyro;
			//Fake Cam4Gyro
			var FakecamObj : GameObject = new GameObject ("FakeCam4Gyro");
			Fakecam = FakecamObj.transform;
			var currentParent : Transform= Fakecam.parent;
			var camParent : GameObject = new GameObject ("camParent");
			camParent.transform.position = Fakecam.position;
			Fakecam.parent = camParent.transform;
			var camGrandparent : GameObject = new GameObject ("camGrandParent");
			camGrandparent.transform.position = Fakecam.position;
			camParent.transform.parent = camGrandparent.transform;
			camGrandparent.transform.parent = currentParent;
			
			camParent.transform.eulerAngles = new Vector3(90.0f,90.0f,0.0f);
		}else{
			RotateControl =RotateC.Classic;
		}
		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		//Initialize the MSP Script
		MSPControl.InitControl();
		
		controller = GetComponent(CharacterController);
		
		//Initialize the original rotation
		originalRotation = transform.rotation.eulerAngles.y;
		
		//Put my object to variable
		myTransform = transform;
		
		CurrentWeapon = WeaponList[0];
		//Set Health  by number of GUI Texture
		MaxHealth = Health =MSPControl.LifeCounterGUI.Length;
		
		CurrentWeapon.NbClip = CurrentWeapon.MaxNbrClip = CurrentWeapon.WeaponBulletGUI.Length-1;
		CurrentWeapon.bulletleft =  CurrentWeapon.NbClip*CurrentWeapon.bulletperClip;
		
		//GUIMAX
		GUIPosXMax = (1.5f*MSPControl.ScreenSize.x)/100f;
		GUIPosYMax = (1.2f*MSPControl.ScreenSize.y)/100f;
		GUIPosY2Max = (6.6f*MSPControl.ScreenSize.y)/100f;
		
		//Add a full clip at start
		CurrentWeapon.bulletinMagasine =  CurrentWeapon.bulletperClip;
		
		//Update GUI
		UpdateGUI(GUIComponent.Init);
	}
	
	 function Update(){
		//Update Controls of MSP
		MSPControl.Command();
		
		//Update movement
		MovePlayer();
		
		switch (RotateControl){
			case RotateC.Classic:
			//Update Classic Rotation
			RotatePlayer();
			break;
			case RotateC.Accelerometer:
			//Update Accelerometer Rotation
			RotatePlayerbyAccelerometer();
			break;
			case RotateC.gyroscope:
			//Update gyroscope Rotation
			RotatePlayerbyGyroscope();
			break;
		}
		
		//Update Others
		OthersPlayerControl();
	}
	
	private function RotatePlayerbyGyroscope(){
			Fakecam.localRotation = Quaternion.Slerp (Fakecam.localRotation, MSPControl.GyroCoord,  GyroSmooth);
			transform.rotation = Quaternion.Euler(0, Fakecam.eulerAngles.y , 0);
			PlayerCam.localRotation = Quaternion.Euler(Fakecam.eulerAngles.x, 0, 0);
	}
	
	private function RotatePlayerbyAccelerometer(){
		var angle :float = AccelAngleCorrector/270f;
		//Rotate Player
		InputX = -MSPControl.Acceleration.y;
		InputY =  - (angle+MSPControl.Acceleration.x);
		rotationX += InputX * RotateCoef*AccelerometerSensibility;
		rotationY += InputY * RotateCoef*AccelerometerSensibility;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
      	transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.1f);
		PlayerCam.localRotation =  Quaternion.Slerp (PlayerCam.localRotation, Quaternion.Euler(PlayerCam.localRotation.x -rotationY, 0, 0),  0.1f);
	}
	
	private function RotatePlayer(){
		//Rotate Player
		InputX = (MSPControl.Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01)) /MSPControl.DPI;
		InputY = (MSPControl.Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01)) /MSPControl.DPI;
		rotationX += InputX * RotateCoef;
		rotationY += InputY * RotateCoef;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
      	transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.1);
		PlayerCam.localRotation =  Quaternion.Slerp (PlayerCam.localRotation, Quaternion.Euler(PlayerCam.localRotation.x-rotationY, 0, 0),  0.1);
	}
	
	private function MovePlayer(){
		//Move Player
		var inputModifyFactor :float = (MSPControl.Window1InputPad.x != 0.0 && MSPControl.Window1InputPad.y != 0.0)? 0.7071 : 1.0;
        moveDirection = myTransform.TransformDirection(new Vector3(MSPControl.Window1InputPad.x*inputModifyFactor, 0, MSPControl.Window1InputPad.y*inputModifyFactor));
		moveDirection *= speed;
		moveDirection.y -= gravity;
		
		controller.Move(moveDirection * Time.smoothDeltaTime);
	}
	
	private function OthersPlayerControl(){
		//Raycast for sight
		var ray :Ray = PlayerCam.GetComponent.<Camera>().ScreenPointToRay(new Vector3(MSPControl.HalfScreen.x+MSPControl.HUDPosition.x,MSPControl.HalfScreen.y-MSPControl.HUDPosition.y,0));
		Physics.Raycast(ray,  hit, 1000, collisionLayers.value);
		//Shoot
		if(MSPControl.WindowFireBtnPressed && CurrentWeapon.bulletinMagasine>0){
			Shoot();
		}else if (CurrentWeapon.Muzzle && CurrentWeapon.Muzzle.enabled){
			CurrentWeapon.Muzzle.enabled = false;
			AxeZPos = GUIPosY2 = AxeYPos = 0;
		}
		
		//Automatic reload, when you don't have any bullet in current clip
		if (CurrentWeapon.bulletinMagasine == 0 && !reload){
			if(CurrentWeapon.firearms == true && CurrentWeapon.bulletleft>0){
				MSPControl.AimPos = false;
				StartCoroutine(Reload());
			}
		}
		
		//Slide Arms and GUI //Arms Position
		SlideGUIandArms();
		
		//Change or Reload weapon no in AIM mode
		if (MSPControl.WeaponTouchPurcentDist > 30 && MSPControl.WeaponActionEnded && !reload && !MSPControl.AimPos){
			Inclin = 30f;
			WeaponButtonControl();
			MSPControl.WeaponTouchDiff = 0; 
			MSPControl.WeaponTouchPurcentDist = 0;
			MSPControl.WeaponActionEnded = false;
		}else if (MSPControl.WeaponTouchPurcentDist < 30 && MSPControl.WeaponActionEnded && !reload){
			if(CurrentWeapon.firearms == true && CurrentWeapon.bulletleft>0)
			{
				MSPControl.AimPos = false;
				StartCoroutine(Reload());
			}
			MSPControl.WeaponTouchDiff = 0; 
			MSPControl.WeaponTouchPurcentDist = 0;
			MSPControl.WeaponActionEnded = false;
		}else if(MSPControl.WeaponActionEnded) {
			MSPControl.WeaponTouchDiff = 0; 
			MSPControl.WeaponTouchPurcentDist = 0;
			MSPControl.WeaponActionEnded = false;
		}
		
		//Fix Aim issue with no firearms weapon
		if(!CurrentWeapon.firearms && MSPControl.AimPos){
			MSPControl.AimPos = !MSPControl.AimPos;
		}
		
		//When the player is touched by an bullet, we give him a little time before he can touchable again
		if(TBHA > 0){
			TBHA -= Time.deltaTime;	
		}else if(TBHA <0){
			TBHA = 0;	
		}
		
		//When you switch the weapon
		if (AxeArms.localRotation == Quaternion.Euler(30f, 0, 0)){
			if(CurrentWeapon == WeaponList[0]){
				Weapon1R.enabled = true;
				Weapon2R.enabled = false;
				Arm1R.enabled = true;
			}else{
				Weapon1R.enabled = false;
				Weapon2R.enabled = true;
				Arm1R.enabled = false;
			}
			Inclin = 0f;
		}
	}
	
	 function OnGUI(){
		//Update GUI on MSP script
		MSPControl.OnGUIComponents();
	}
	
	private function Shoot(){
		if (CurrentWeapon.firearms){
			if (Time.time > CurrentWeapon.nextFireTime + CurrentWeapon.fireRate){
				CurrentWeapon.bulletinMagasine -=1;
				
				//Apply Slide Effect
				ZsmoothVal = 10f;
				AxeYPos = 0.04f;
				GUIPosY2=GUIPosY2Max;
				AxeZPos = -0.1f;
				
				//sight for a collider
				if (hit.collider){
					hit.collider.SendMessage("Hit",CurrentWeapon.WeaponPower,SendMessageOptions.DontRequireReceiver);
					Instantiate  (Sparkle, hit.point, Quaternion.identity); 
				} 
				UpdateGUI(GUIComponent.Bullet);
				if(CurrentWeapon.Muzzle)
					CurrentWeapon.Muzzle.enabled = true;
				GetComponent.<AudioSource>().PlayOneShot(CurrentWeapon.shootSound);
				muzzleRotate +=90;
				CurrentWeapon.Muzzle.gameObject.transform.localRotation = Quaternion.AngleAxis(muzzleRotate, Vector3.forward);
				CurrentWeapon.nextFireTime = Time.time;
				
				//Instantiate BulletUp
				
			}else{
				if(CurrentWeapon.Muzzle)
					CurrentWeapon.Muzzle.enabled = false;
				GUIPosY2=AxeYPos = 0;
				AxeZPos = 0;
				ZsmoothVal = 8f;
			}
		}else{
			//Put here your code for no fire weapon
			
		}
	}
	
	//Reload
	function Reload(){
		reload = true;
		Inclin = 7.5f;
		if (CurrentWeapon.bulletinMagasine<CurrentWeapon.bulletperClip){
			CurrentWeapon.bulletleft += CurrentWeapon.bulletinMagasine;
			CurrentWeapon.bulletinMagasine = 0;
			
			if (CurrentWeapon.bulletleft > CurrentWeapon.bulletperClip){
				yield WaitForSeconds (2);
				CurrentWeapon.bulletinMagasine = CurrentWeapon.bulletperClip;
				CurrentWeapon.bulletleft -= CurrentWeapon.bulletperClip;
			} else {
				yield  WaitForSeconds (2);
				CurrentWeapon.bulletinMagasine = CurrentWeapon.bulletleft;
				CurrentWeapon.bulletleft = 0;
			}
			CurrentWeapon.NbClip = parseFloat(CurrentWeapon.bulletleft)/parseFloat(CurrentWeapon.bulletperClip);
			UpdateGUI(GUIComponent.Clip);
		}
		reload = false;
		UpdateGUI(GUIComponent.Bullet);
		Inclin = 0;
	}
	
	//When you switch the weapon
	function WeaponButtonControl(){
		if (CurrentWeapon == WeaponList[0]){
			CurrentWeapon = WeaponList[1];
			MSPControl.HideCrossHair = true;
			MSPControl.GUIObject.WeaponGUI = CurrentWeapon.WeaponBulletGUI.GetValue(0) as Texture;
		}else if(CurrentWeapon == WeaponList[1]){
			CurrentWeapon = WeaponList[0];
			MSPControl.HideCrossHair = false;
			UpdateGUI(GUIComponent.Clip);
		}
	}
	
	//When the player is touched
	public function Hit( power :int){
		if(Health>1 && TBHA ==0){
			Health -= power;
			UpdateGUI(GUIComponent.Health);
			TBHA = TimeBeforeHitAgain;
		}
	}
	
	//To change the GUI Components
	private function UpdateGUI(GUICpnt : GUIComponent ){
		
		switch (GUICpnt){
			case GUIComponent.Init:
				MSPControl.GUIObject.LifeGUI = MSPControl.LifeCounterGUI.GetValue(Health-1) as Texture;
				MSPControl.GUIObject.CrossHairGui = MSPControl.CrossHairGUI.GetValue(10) as Texture;
				MSPControl.GUIObject.WeaponGUI = CurrentWeapon.WeaponBulletGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.NbClip)) as Texture;
			break;
			case GUIComponent.Health:
				
				MSPControl.GUIObject.LifeGUI = MSPControl.LifeCounterGUI.GetValue(Health-1) as Texture;
				
			break;
			case GUIComponent.Bullet:
					MSPControl.GUIObject.CrossHairGui = MSPControl.CrossHairGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.bulletinMagasine*10.0/CurrentWeapon.bulletperClip)) as Texture;
			break;
			case GUIComponent.Clip:
					if(CurrentWeapon.NbClip<=0)
						CurrentWeapon.NbClip =0;
					MSPControl.GUIObject.WeaponGUI = CurrentWeapon.WeaponBulletGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.NbClip)) as Texture;
			break;
		}
	}
	
	private function SlideGUIandArms(){
		//Sliding condition of GUI
		if (InputX < -1.5f && AxeArms.localPosition.x > -0.02f){
			AxeXPos = -0.02f;
			GUIPosX = -GUIPosXMax;
			
		}else if (InputX > 1.5f && AxeArms.localPosition.x < 0.02f){
			AxeXPos = 0.02f;
			GUIPosX = GUIPosXMax;
		}else if (InputX == 0.0f && (AxeArms.localPosition.x > 0.019f || AxeArms.localPosition.x < -0.019f))
			GUIPosX =AxeXPos = 0.0f;
	
		if (InputY < -1f && AxeArms.localPosition.y > -0.009f){
			AxeYCoef = 0.009f;
			GUIPosY= GUIPosYMax;
		}else if (InputY > 1f && AxeArms.localPosition.y < 0.009f){
			AxeYCoef = -0.009f;
			GUIPosY= -GUIPosYMax;
		}else if (InputY == 0.0f && (AxeArms.localPosition.y > 0.008f || AxeArms.localPosition.y < -0.008f))
			GUIPosY= AxeYCoef = 0.0f;
		
		//New in 1.2
		if(!MSPControl.AimPos){
			if(PlayerCam.GetComponent.<Camera>().fieldOfView < 42){
				PlayerCam.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(PlayerCam.GetComponent.<Camera>().fieldOfView, 42, 0.1f);
			}
			//Apply Slide Effect on Arms and GUI
			AxeArms.localPosition = Vector3.Lerp(AxeArms.localPosition, new Vector3(AxeXPos,AxeYPos+AxeYCoef,AxeZPos),  Time.deltaTime * ZsmoothVal);
			MSPControl.HUDPosition = Vector2.Lerp(MSPControl.HUDPosition, new Vector2(GUIPosX,-GUIPosY2 + GUIPosY),Time.deltaTime * 4f);
			//Reinit the rotate speed
			AxeArms.localRotation =  Quaternion.Slerp (AxeArms.localRotation, Quaternion.Euler(Inclin, 0, 0),  0.1f);
			RotateCoef = 1f;
			if (MSPControl.HideCrossHair && CurrentWeapon.firearms){
				MSPControl.HideCrossHair = !MSPControl.HideCrossHair;
			}
		}else if (MSPControl.AimPos && !reload && CurrentWeapon.firearms){
			if(PlayerCam.GetComponent.<Camera>().fieldOfView > 34){
				PlayerCam.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(PlayerCam.GetComponent.<Camera>().fieldOfView, 34, 0.1f);
			}
			AxeArms.localPosition = Vector3.Lerp(AxeArms.localPosition, new Vector3(CurrentWeapon.AimPosition.x,AxeYPos+CurrentWeapon.AimPosition.y,AxeZPos/2+CurrentWeapon.AimPosition.z),  Time.deltaTime * ZsmoothVal);
			MSPControl.HUDPosition = Vector2.Lerp(MSPControl.HUDPosition, Vector2.zero,Time.deltaTime * 4f);
			AxeArms.localRotation =  Quaternion.Slerp (AxeArms.localRotation, Quaternion.Euler(CurrentWeapon.AimAngle, 0, 0),  0.1f);
			//divide the rotate speed by 2
			RotateCoef = 0.5f;
			if (!MSPControl.HideCrossHair){
				MSPControl.HideCrossHair = !MSPControl.HideCrossHair;
			}
		}
	}
	
	function OnTriggerEnter(  Obj :Collider) {
		//If you touch a health up object
       if(Obj.tag == "HealthUp"){
			var Up :int =  Health + 10;
			if(Up >MaxHealth){
				Health = Mathf.Round (MaxHealth);
			}else{
				Health = Up;
			}
			UpdateGUI(GUIComponent.Health);
		  	Destroy(Obj.gameObject);
			
			//If you touch a bullet up object
		}else if (Obj.tag == "BulletUp"){
			if(Mathf.CeilToInt(CurrentWeapon.NbClip)< CurrentWeapon.MaxNbrClip){
				CurrentWeapon.NbClip += 1;
				CurrentWeapon.bulletleft+=CurrentWeapon.bulletperClip;
				//For demo we choose to keep the Bullet up package on the scene after touched it.
				//Destroy(Obj.gameObject);
			
				UpdateGUI(GUIComponent.Clip);
			}
		}
    }

