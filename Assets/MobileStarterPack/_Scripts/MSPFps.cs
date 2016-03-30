//Mobile Starter Pack #1 FPS v1.2c
//by Stéphane 'Azert2k' JAMIN
 
using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
public class MSPFps : MonoBehaviour {
	public enum RotateC{
		Classic,
		Accelerometer,
		gyroscope
	}
	public RotateC RotateControl = RotateC.Classic;
	public float AccelerometerSensibility = 1.5f;
	public float AccelAngleCorrector = 135f;
	
	public float GyroSmooth = 0.1f;
	
	enum GUIComponent{
		Health,
		Bullet,
		Clip,
		Init
	}
	[SerializeField]
	public MSP MSPControl = new MSP();
	public GameObject Sparkle;
	public Transform AxeArms;
	public Transform PlayerCam;
	public float Sensitivity = 1.5f;
    private Vector3 moveDirection = Vector3.zero;
	public float speed = 10;
    public float gravity = 8.0F;
    private CharacterController controller;
	private float rotationY = 0.0f;
	private float rotationX = 0.0f;
	private float sensitivityX = 2000f;
	private float sensitivityY = 900F;
	private float minimumY = -60F;
	private float maximumY = 60F;
	private float originalRotation;
    private Transform myTransform;
	private int Health;
	private int BulletGUI;
	private RaycastHit hit;
	private float AxeXPos = 0;
	private float AxeYPos = 0;
	private float AxeZPos = 0;
	private float AxeYCoef=0;
	private float ZsmoothVal = 8;
	private float GUIPosX;
	private float GUIPosXMax;
	private float GUIPosY;
	private float GUIPosYMax;
	private float GUIPosY2;
	private float GUIPosY2Max;
	private float InputX;
	private float InputY;
	private float MaxHealth;
	private float MaxClip;
	public Renderer Weapon1R;
	public Renderer Weapon2R;
	public Renderer Arm1R;
	public Renderer Arm2R;
	private float Inclin;
	public float TimeBeforeHitAgain = 2;
	private float TBHA;
	
	//New in 1.2 AIM POSITION
	private float RotateCoef;
	
    [System.Serializable]
	public class WeaponClass{
		public bool firearms;
		public float fireRate;
		public int bulletperClip;
		public int WeaponPower;
		public Renderer Muzzle;
		public AudioClip shootSound;
		public Texture[] WeaponBulletGUI;
		[System.NonSerialized]
		public int MaxNbrClip;
		[System.NonSerialized]
		public float NbClip;
		[System.NonSerialized]
		public int bulletleft;
		[System.NonSerialized]
		public float nextFireTime;
		[System.NonSerialized]
		public float bulletinMagasine;
		[System.NonSerialized]
		//New in 1.2 AIM POSITION
		public Vector3 AimPosition = new Vector3{
			x =-0.1615f,
			y = -0.044f,
			z=-0.45f	
		};
		public float AimAngle = 358.5694f;
	}
	
	private WeaponClass CurrentWeapon;
	
	[SerializeField]
	public WeaponClass[] WeaponList;
	
	public LayerMask collisionLayers = -1;
	private int muzzleRotate = 45;
	private bool reload;
	
	Transform Fakecam;
		
	private void Start(){
		//If the rotation is controled by accelero New In 1.2
		if(RotateControl == RotateC.Accelerometer){
			MSPControl.AccelGyroCtrl = MSP.GyroAccel.Accelero;	
		}else if (RotateControl == RotateC.gyroscope && SystemInfo.supportsGyroscope){	
			MSPControl.AccelGyroCtrl = MSP.GyroAccel.Gyro;
			//Fake Cam4Gyro
			GameObject FakecamObj = new GameObject ("FakeCam4Gyro");
			Fakecam = FakecamObj.transform;
			Transform currentParent = Fakecam.parent;
			GameObject camParent = new GameObject ("camParent");
			camParent.transform.position = Fakecam.position;
			Fakecam.parent = camParent.transform;
			GameObject camGrandparent = new GameObject ("camGrandParent");
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
		
		controller = GetComponent<CharacterController>();
		
		//Initialize the original rotation
		originalRotation = transform.rotation.eulerAngles.y;
		
		//Put my object to variable
		myTransform = transform;
		
		CurrentWeapon = WeaponList[0];
		//Set Health  by number of GUI Texture
		MaxHealth = Health =MSPControl.LifeCounterGUI.Length;
		
		CurrentWeapon.NbClip = CurrentWeapon.MaxNbrClip = CurrentWeapon.WeaponBulletGUI.Length-1;
		CurrentWeapon.bulletleft =  (int)CurrentWeapon.NbClip*CurrentWeapon.bulletperClip;
		
		//GUIMAX
		GUIPosXMax = (1.5f*MSPControl.ScreenSize.x)/100f;
		GUIPosYMax = (1.2f*MSPControl.ScreenSize.y)/100f;
		GUIPosY2Max = (6.6f*MSPControl.ScreenSize.y)/100f;
		
		//Add a full clip at start
		CurrentWeapon.bulletinMagasine =  CurrentWeapon.bulletperClip;
		
		//Update GUI
		UpdateGUI(GUIComponent.Init);

		StartCoroutine(Wait ());
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(0.5f);
		MSPControl.InitControl();
	}
	
	private void Update(){
		//Update Controls of MSP
		MSPControl.Command();
		
		//Update movement
		MovePlayer();
		RotatePlayer();
		/*switch (RotateControl){
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
		OthersPlayerControl();*/
	}
	
	/*private void RotatePlayerbyGyroscope(){
			Fakecam.localRotation = Quaternion.Slerp (Fakecam.localRotation, MSPControl.GyroCoord,  GyroSmooth);
			
			transform.rotation = Quaternion.Euler(0, Fakecam.eulerAngles.y , 0);
			PlayerCam.localRotation = Quaternion.Euler(Fakecam.eulerAngles.x, 0, 0);
	}
	
	private void RotatePlayerbyAccelerometer(){
		float angle = AccelAngleCorrector/270f;
		//Rotate Player
		InputX = -MSPControl.Acceleration.y;
		InputY =  - (angle+MSPControl.Acceleration.x);
		rotationX += InputX * RotateCoef*AccelerometerSensibility;
		rotationY += InputY * RotateCoef*AccelerometerSensibility;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
      	transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.1f);
		PlayerCam.localRotation =  Quaternion.Slerp (PlayerCam.localRotation, Quaternion.Euler(PlayerCam.localRotation.x -rotationY, 0, 0),  0.1f);
	}*/
	
	private void RotatePlayer(){
		//Rotate Player
		InputX = (MSPControl.Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01f)) /MSPControl.DPI;
		InputY = (MSPControl.Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01f)) /MSPControl.DPI;
		rotationX += InputX * RotateCoef;
		rotationY += InputY * RotateCoef;
		rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
      	transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.1f);
		PlayerCam.localRotation =  Quaternion.Slerp (PlayerCam.localRotation, Quaternion.Euler(PlayerCam.localRotation.x-rotationY, 0, 0),  0.1f);
	}
	
	private void MovePlayer(){
		//Move Player
		float inputModifyFactor = (MSPControl.Window1InputPad.x != 0.0f && MSPControl.Window1InputPad.y != 0.0f)? 0.7071f : 1.0f;
        moveDirection = myTransform.TransformDirection(new Vector3(MSPControl.Window1InputPad.x*inputModifyFactor, 0, MSPControl.Window1InputPad.y*inputModifyFactor));
		moveDirection *= speed;
		moveDirection.y -= gravity;
		
		controller.Move(moveDirection * Time.smoothDeltaTime);
	}
	/*
	private void OthersPlayerControl(){
		//Raycast for sight
		Ray ray = PlayerCam.GetComponent<Camera>().ScreenPointToRay(new Vector3(MSPControl.HalfScreen.x+MSPControl.HUDPosition.x,MSPControl.HalfScreen.y-MSPControl.HUDPosition.y,0));
		Physics.Raycast(ray, out hit, 1000, collisionLayers.value);
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
	*/
	private void OnGUI(){
		//Update GUI on MSP script
		MSPControl.OnGUIComponents();
	}
	/*
	private void Shoot(){
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
				GetComponent<AudioSource>().PlayOneShot(CurrentWeapon.shootSound);
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
	IEnumerator Reload(){
		reload = true;
		Inclin = 7.5f;
		if (CurrentWeapon.bulletinMagasine<CurrentWeapon.bulletperClip){
			CurrentWeapon.bulletleft += (int)CurrentWeapon.bulletinMagasine;
			CurrentWeapon.bulletinMagasine = 0;
			
			if (CurrentWeapon.bulletleft > CurrentWeapon.bulletperClip){
				yield return new WaitForSeconds (2);
				CurrentWeapon.bulletinMagasine = CurrentWeapon.bulletperClip;
				CurrentWeapon.bulletleft -= CurrentWeapon.bulletperClip;
			} else {
				yield return new WaitForSeconds (2);
				CurrentWeapon.bulletinMagasine = CurrentWeapon.bulletleft;
				CurrentWeapon.bulletleft = 0;
			}
			CurrentWeapon.NbClip = (float)CurrentWeapon.bulletleft/(float)CurrentWeapon.bulletperClip;
			UpdateGUI(GUIComponent.Clip);
		}
		reload = false;
		UpdateGUI(GUIComponent.Bullet);
		Inclin = 0;
	}
	
	//When you switch the weapon
	void WeaponButtonControl(){
		if (CurrentWeapon == WeaponList[0]){
			CurrentWeapon = WeaponList[1];
			MSPControl.HideCrossHair = true;
			MSPControl.GUIObject.WeaponGUI = (Texture)CurrentWeapon.WeaponBulletGUI.GetValue(0);
		}else if(CurrentWeapon == WeaponList[1]){
			CurrentWeapon = WeaponList[0];
			MSPControl.HideCrossHair = false;
			UpdateGUI(GUIComponent.Clip);
		}
	}
	
	//When the player is touched
	public void Hit(int power){
		if(Health>1 && TBHA ==0){
			Health -= power;
			UpdateGUI(GUIComponent.Health);
			TBHA = TimeBeforeHitAgain;
		}
	}
	*/
	//To change the GUI Components
	private void UpdateGUI(GUIComponent GUICpnt){
		
		switch (GUICpnt){
			case GUIComponent.Init:
				MSPControl.GUIObject.LifeGUI = (Texture)MSPControl.LifeCounterGUI.GetValue(Health-1);
				MSPControl.GUIObject.CrossHairGui = (Texture)MSPControl.CrossHairGUI.GetValue(10);
				MSPControl.GUIObject.WeaponGUI = (Texture)CurrentWeapon.WeaponBulletGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.NbClip));
			break;
			case GUIComponent.Health:
				
				MSPControl.GUIObject.LifeGUI = (Texture)MSPControl.LifeCounterGUI.GetValue(Health-1);
				
			break;
			case GUIComponent.Bullet:
					MSPControl.GUIObject.CrossHairGui = (Texture)MSPControl.CrossHairGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.bulletinMagasine*10.0f/CurrentWeapon.bulletperClip));
			break;
			case GUIComponent.Clip:
					if(CurrentWeapon.NbClip<=0)
						CurrentWeapon.NbClip =0;
					MSPControl.GUIObject.WeaponGUI = (Texture)CurrentWeapon.WeaponBulletGUI.GetValue(Mathf.CeilToInt(CurrentWeapon.NbClip));
			break;
		}
	}
	
	private void SlideGUIandArms(){
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
			if(PlayerCam.GetComponent<Camera>().fieldOfView < 60){
				PlayerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(PlayerCam.GetComponent<Camera>().fieldOfView, 60, 0.1f);
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
			if(PlayerCam.GetComponent<Camera>().fieldOfView > 34){
				PlayerCam.GetComponent<Camera>().fieldOfView = Mathf.Lerp(PlayerCam.GetComponent<Camera>().fieldOfView, 34, 0.1f);
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
	/*
	void OnTriggerEnter(Collider  Obj ) {
		//If you touch a health up object
       if(Obj.tag == "HealthUp"){
			int Up =  Health + 10;
			if(Up >MaxHealth){
				Health = (int)Mathf.Round (MaxHealth);
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
    }*/
}

