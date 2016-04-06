using UnityEngine;
using System.Collections;

public class VRCamera : MonoBehaviour { 

	[HideInInspector]
	public Camera[] cameras;
	public float mouseSensitivity=0.2f;
	private Vector3 oldMousePosition;
	#if !UNITY_EDITOR
	private float rotationY=0f,oldRotationY;
	#endif
	
	private  float FPSupdateInterval = 0.5F;
	private float FPSaccum   = 0; // FPS accumulated over the interval
	private int   FPSframes  = 0; // Frames drawn over the interval
	private float FPStimeleft; // Left time for current interval
	[HideInInspector]
	public float fps;
	//public bool checkFPS=true;
	//public float badFPSrate=30f;
	public bool useAntiDrift=true;
	public bool useCompassForAntiDrift=false;
	//private float blackScreen=0,needBlackScreen=0;
	//public Texture blackFadeTex; 
	private Transform vrCameraLocal;
    [SerializeField] [HideInInspector]
    private bool _useLensCorrection=true;
    [HideInInspector]
    public bool useLensCorrection
	{
		get{ return _useLensCorrection; }
		set
		{
			_useLensCorrection = value;
            Debug.Log("UseCorrection: " + _useLensCorrection);
			NewLensCorrection[] nlc = GetComponentsInChildren<NewLensCorrection>();
			for( int k=0; k<nlc.Length; k++ ) nlc[k].useDistortion = _useLensCorrection;
#if UNITY_EDITOR
			//if( Application.isEditor ) UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
#endif
		}
	}


    [SerializeField] [HideInInspector]
    private float _distanceBetweenEyes=0.7f;
    public float distanceBetweenEyes
	{
		get { return _distanceBetweenEyes; }
		set
		{
			_distanceBetweenEyes = value;
			if( _distanceBetweenEyes<0.0001f ) _distanceBetweenEyes = 0.0001f;
			Camera[] _cameras = gameObject.GetComponentsInChildren<Camera>();
			for( int k=0; k<_cameras.Length; k++ )
			{
				if( _cameras[k].transform.localPosition.x<0f ) _cameras[k].transform.localPosition = -Vector3.right*_distanceBetweenEyes*0.5f;
				if( _cameras[k].transform.localPosition.x>0f ) _cameras[k].transform.localPosition = Vector3.right*_distanceBetweenEyes*0.5f;
			}
		}
	}

	public Transform vrCameraHeading
	{
		get
		{
			Init ();
			return vrCameraLocal;
		}
	}

	private Vector3 meanAcceleration;


	
	
	#if UNITY_ANDROID && !UNITY_EDITOR
	private float initCompassHeading=0f;
	private float gyroYaccel;
	private float gyroBiasPause;
	private float oldDeltaRotation;
	private float gyroBias;

	void InitCompassHeading()
	{
		initCompassHeading = -rotationY+Input.compass.magneticHeading;
	}

	private int numInGyroBiasArray;
	void AddToGyroBiasArray(float f)
	{
		gyroBiasArray[numInGyroBiasArray]=f;
		numInGyroBiasArray++;
		if( numInGyroBiasArray>=gyroBiasArray.Length ) numInGyroBiasArray=0;
	}
	private float[] gyroBiasArray;
	private int numRecalculatesGyroBias=0;
	void RecalculateGyroBias()
	{
		if( !FibrumController.useAntiDrift ) return;
		float tempGyroBias=0;
		int num=0;
		for( int k=0; k<gyroBiasArray.Length; k++ )
		{
			tempGyroBias+=gyroBiasArray[k];
			num++;
		}
		gyroBias = tempGyroBias/(float)num;
		numRecalculatesGyroBias++;
	}

	public void EnableCompass(bool on)
	{
		if( on )
		{
			Input.compass.enabled = true;
			CancelInvoke ("InitCompassHeading");
			Invoke ("InitCompassHeading",0.1f);
		}
		else
		{
			Input.compass.enabled = false;
			CancelInvoke ("InitCompassHeading");
		}
	}

	#endif


	bool noGyroscope=false;
	Quaternion rotation;
#if UNITY_EDITOR
	#elif UNITY_IPHONE
	private float q0,q1,q2,q3;
	Quaternion rot;
	#elif UNITY_ANDROID
	private float meanDeltaGyroY;
	#elif UNITY_WP8 || UNITY_WP_8_1
	private WindowsPhoneVRController.Controller c = null;
	#endif



	bool initialized=false;
	public void Init()
	{
		if( initialized ) return;

		NewLensCorrection[] nlc = GetComponentsInChildren<NewLensCorrection>();
		cameras = new Camera[nlc.Length];
		for( int k=0; k<cameras.Length; k++ ) cameras[k] = nlc[k].gameObject.GetComponent<Camera>();

		FibrumController.useAntiDrift = useAntiDrift;
		FibrumController.useCompassForAntiDrift = useCompassForAntiDrift;

		FPStimeleft = FPSupdateInterval;
		/*if( blackFadeTex==null ) checkFPS=false;
		if( checkFPS ) 
		{
			blackScreen = 1f;
			needBlackScreen = 1f;
			Invoke ("CheckFPS",1.6f);
		} */


		if( transform.FindChild("VRCamera") ) vrCameraLocal = transform.FindChild("VRCamera").transform;
		else vrCameraLocal = transform;

		Screen.sleepTimeout = SleepTimeout.NeverSleep;
		Screen.orientation = ScreenOrientation.LandscapeLeft;

#if !UNITY_STANDALONE && !UNITY_EDITOR
		if( !SystemInfo.supportsGyroscope )
		{
			noGyroscope = true;
		}
		else
		{
			noGyroscope = false;
		}
#endif

		FibrumController.Init();

		#if UNITY_ANDROID && !UNITY_EDITOR
		gyroBiasArray = new float[128]; for( int k=0; k<gyroBiasArray.Length; k++) gyroBiasArray[k]=0f;

		if( noGyroscope )
		{
			transform.Find("VRCamera/WarningPlane").gameObject.SetActive(true);
			EnableCompass(true);
		}

		InvokeRepeating("RecalculateGyroBias",0.1f,0.2f);

		if( FibrumController.useCompassForAntiDrift )
		{
			EnableCompass(true);
		}

		#endif
		
		#if UNITY_IPHONE
		//transform.localRotation = Quaternion.Euler(90, 0 ,0);
		#elif (UNITY_WP8 || UNITY_WP_8_1) && !UNITY_EDITOR
		Input.gyro.enabled = true; 
		//transform.localRotation = Quaternion.Euler(90, 0 ,0);
		c = new WindowsPhoneVRController.Controller();
		rotation = Quaternion.identity;


		#endif

		FibrumController.vrCamera = this;

		bool noDummyUI=true;
		Camera[] allCameras = GetComponentsInChildren<Camera>();
		for( int k=0; k<allCameras.Length; k++ )
		{
			if( allCameras[k].rect.width<=0.5f )
			{
				if( allCameras[k].gameObject.GetComponent<ChangeCameraEye>()==null )
				{
					allCameras[k].gameObject.AddComponent<ChangeCameraEye>();
				}
			}
			else if( allCameras[k].clearFlags==CameraClearFlags.Color && allCameras[k].depth==-100 ) noDummyUI=false;
		}
		if( noDummyUI )
		{
			GameObject dummyGO = GameObject.Instantiate((GameObject)Resources.Load("FibrumResources/VR_UI_dummyCamera",typeof(GameObject))) as GameObject;
			dummyGO.transform.parent = vrCameraLocal;
			dummyGO.transform.localPosition = Vector3.zero;
			dummyGO.transform.localRotation = Quaternion.identity;
		}


		if( !noGyroscope ) Invoke("SensorCalibration", 0.1f);
		FibrumInput.LoadJoystickPrefs();
		
		meanAcceleration = Input.acceleration;

		if( FibrumController.vrSetup == null )
		{
			GameObject _setup = GameObject.Instantiate((GameObject)Resources.Load("FibrumResources/VRSetup",typeof(GameObject))) as GameObject;
			FibrumController.vrSetup = _setup; 
		}

        useLensCorrection = useLensCorrection;

		initialized = true;
	}
	
	void Start () {
		
		Init ();
	}
	
	/*void CheckFPS()
	{
		needBlackScreen = 0f;
		Invoke ("DisableBlackScreen",2f);
		if( fps<badFPSrate )
		{
			for( int k=0; k<cameras.Length; k++ )
			{
				if( cameras[k] != null )	cameras[k].SendMessage("DisableLensCorrection");
				else Debug.Log("VRCamera - cameras["+k+"] not assigned!");
			}
		}
	} */
	
	/*void DisableBlackScreen()
	{
		checkFPS = false;
	} */
	
	void SensorCalibration ()
	{
		#if UNITY_EDITOR
		transform.localRotation = Quaternion.Euler(0f, -vrCameraLocal.localEulerAngles.y, 0f);
		#elif UNITY_IPHONE 
		transform.localRotation = Quaternion.Euler(0f, -vrCameraLocal.localEulerAngles.y, 0f);
		#elif UNITY_WP8 || UNITY_WP_8_1 && !UNITY_EDITOR
		transform.localRotation = Quaternion.Euler(0f, -vrCameraLocal.localEulerAngles.y, 0f);
		Vector3 gravity = Input.acceleration;
		float fi   = Mathf.Rad2Deg*Mathf.Atan(-gravity.z/(Mathf.Sign(gravity.y)*Mathf.Sqrt(gravity.y*gravity.y+gravity.x*gravity.x*0.01f)));
		float teta = Mathf.Rad2Deg*Mathf.Atan(-gravity.x/(Mathf.Sqrt(gravity.z*gravity.z+gravity.y*gravity.y)));
		rotation = Quaternion.Euler(-fi,0f,teta);
		#elif !UNITY_IPHONE && !(UNITY_WP8 || UNITY_WP_8_1)
		transform.localRotation = Quaternion.Euler(0f, -vrCameraLocal.localEulerAngles.y, 0f);
		#else
		transform.localRotation = Quaternion.Euler(0f, -vrCameraLocal.localEulerAngles.y, 0f);
		#endif
	}
	
	void LateUpdate () {
		
		
		#if UNITY_EDITOR || UNITY_STANDALONE
		Vector3 euler = rotation.eulerAngles;
        //rotation = Quaternion.Euler(Mathf.Max (-89f,Mathf.Min (89f,Mathf.DeltaAngle(0,euler.x)+mouseSensitivity*(oldMousePosition.y-Input.mousePosition.y))),euler.y-mouseSensitivity*(oldMousePosition.x-Input.mousePosition.x),0f);
        rotation = Quaternion.Euler(Mathf.Max(-89f, Mathf.Min(89f, Mathf.DeltaAngle(0, euler.x) - mouseSensitivity * Input.GetAxis("Mouse Y"))), euler.y + mouseSensitivity * Input.GetAxis("Mouse X"), 0f);
        vrCameraLocal.localRotation = rotation;
		oldMousePosition = Input.mousePosition;

#elif UNITY_ANDROID && !UNITY_EDITOR
		if( !noGyroscope )
		{
			Matrix4x4 matrix = new Matrix4x4();
		
			float[] M = FibrumController.vrs._ao.CallStatic<float[]>("getHeadMatrix");
			
			matrix.SetColumn(0, new Vector4(M[0], M[4], -M[8], M[12]) );
			matrix.SetColumn(1, new Vector4(M[1], M[5], -M[9], M[13]) );
			matrix.SetColumn(2, new Vector4(-M[2], -M[6], M[10], M[14]) );
			matrix.SetColumn(3, new Vector4(M[3], M[7], M[11], M[15]) );
		
			TransformFromMatrix (matrix, vrCameraLocal);
			float deltaRotation = vrCameraLocal.localRotation.eulerAngles.y-oldRotationY;
			while( deltaRotation>180f ) deltaRotation -= 360f;
			while( deltaRotation<-180f ) deltaRotation += 360f;
			gyroYaccel = Mathf.Lerp(gyroYaccel,(deltaRotation-oldDeltaRotation)/Time.deltaTime,Time.deltaTime);
			oldDeltaRotation = deltaRotation;
			oldRotationY = vrCameraLocal.localRotation.eulerAngles.y;

			if( Mathf.Abs(gyroYaccel)>0.2f ) gyroBiasPause = Time.realtimeSinceStartup+1f;
			if( Time.realtimeSinceStartup>gyroBiasPause )
			{
				meanDeltaGyroY = Mathf.Lerp(meanDeltaGyroY,deltaRotation/Time.deltaTime,Time.deltaTime*10f);
				AddToGyroBiasArray(meanDeltaGyroY);
			}

			rotationY += deltaRotation - gyroBias*Time.deltaTime;

			while( rotationY>180f ) rotationY -= 360f;
			while( rotationY<-180f ) rotationY += 360f;
			if( FibrumController.useCompassForAntiDrift )
			{
				float compassDeltaRotationY = rotationY-(Input.compass.magneticHeading-initCompassHeading);
				while( compassDeltaRotationY>180f ) compassDeltaRotationY -= 360f;
				while( compassDeltaRotationY<-180f ) compassDeltaRotationY += 360f;
				if( Time.realtimeSinceStartup>gyroBiasPause-0.7f )
				{
					rotationY -= compassDeltaRotationY*Time.deltaTime*1.5f;
				}
				else
				{
					rotationY -= compassDeltaRotationY*Time.deltaTime*0.2f;
				}
			}
				
			vrCameraLocal.localRotation = Quaternion.Euler(vrCameraLocal.localRotation.eulerAngles.x,rotationY,vrCameraLocal.localRotation.eulerAngles.z);
		}
		else 
		{
			Vector3 gravity = Input.acceleration;
			float fi   = Mathf.Rad2Deg*Mathf.Atan(-gravity.z/(Mathf.Sign(gravity.y)*Mathf.Sqrt(gravity.y*gravity.y+gravity.x*gravity.x*0.01f)));
			float teta = Mathf.Rad2Deg*Mathf.Atan(-gravity.x/(Mathf.Sqrt(gravity.z*gravity.z+gravity.y*gravity.y)));
			rotation = Quaternion.Slerp(rotation,Quaternion.Euler(-fi,Input.compass.magneticHeading-initCompassHeading,teta),Time.deltaTime*4.0f);
			vrCameraLocal.localRotation = rotation;
		}
#elif UNITY_IPHONE
		rot = ConvertRotation(Input.gyro.attitude);
		vrCameraLocal.localRotation = Quaternion.Euler(90f,0f,0f)*rot;
#elif UNITY_WP8 || UNITY_WP_8_1
		//rot = ConvertRotation(Input.gyro.attitude);
		//vrCameraLocal.localRotation = rot;
		//print (vrCameraLocal.localRotation.eulerAngles);
		float vertical_angle_delta = (float)c.AngularVelocityY * Time.deltaTime;
		float horisontal_angle_delta = (float)c.AngularVelocityX * Time.deltaTime;
		float z_angle_delta = (float)c.AngularVelocityZ * Time.deltaTime;
		rotation = Quaternion.Euler(rotation.eulerAngles.x+vertical_angle_delta,rotation.eulerAngles.y-horisontal_angle_delta,rotation.eulerAngles.z+z_angle_delta); 
		Vector3 gravity = Input.acceleration;
		float fi   = Mathf.Rad2Deg*Mathf.Atan(-gravity.z/(Mathf.Sign(gravity.y)*Mathf.Sqrt(gravity.y*gravity.y+gravity.x*gravity.x*0.01f)));
		float teta = Mathf.Rad2Deg*Mathf.Atan(-gravity.x/(Mathf.Sqrt(gravity.z*gravity.z+gravity.y*gravity.y)));
		rotation = Quaternion.Slerp(rotation,Quaternion.Euler(-fi,rotation.eulerAngles.y,teta),Time.deltaTime*2f);
		//vrCameraLocal.localRotation = Quaternion.Euler(90f,0f,0f)*rotation;
        vrCameraLocal.localRotation = rotation;
		//vrCameraLocal.localRotation = Quaternion.Euler(-fi,0f,teta);
#endif


        meanAcceleration = Vector3.Lerp(meanAcceleration,Input.acceleration,Time.deltaTime*2.0f);
		if( meanAcceleration.x>0.25f && Mathf.Abs (meanAcceleration.y)<0.1f && (meanAcceleration-Input.acceleration).magnitude<0.5f )
		{
			FibrumController.isHandOriented=true;
		}
		else
		{
			FibrumController.isHandOriented=false;
		}
		
		////////////////////////////////////////
		// MEASURE FPS
		////////////////////////////////////////
		FPStimeleft -= Time.deltaTime;
		FPSaccum += Time.timeScale/Time.deltaTime;
		++FPSframes;
		if( FPStimeleft <= 0.0 )
		{
			fps = FPSaccum/FPSframes;
			FPStimeleft = FPSupdateInterval;
			FPSaccum = 0.0F;
			FPSframes = 0;
		}
		
		/*if( checkFPS )
		{
			blackScreen = Mathf.Lerp(blackScreen,needBlackScreen,Time.deltaTime*5f);
		} */
	}
	
	void OnGUI()
	{
		/*if( checkFPS )
		{
			if (Event.current.type.Equals(EventType.Repaint))
			{
				Graphics.DrawTexture (new Rect(0f,0f,Screen.width,Screen.height),blackFadeTex,new Rect(0f,0f,1f,1f),0,0,0,0,new Color(1f,1f,1f,blackScreen),null);
			}
		}*/
		#if UNITY_ANDROID && !UNITY_EDITOR
		//GUI.Box (new Rect(0f,0f,Screen.width,30f),"   rotationY="+(int)rotationY+"   deltaCompassHeading="+(int)(rotationY-(Input.compass.magneticHeading-initCompassHeading)));
		//GUI.Box (new Rect(0f,0f,Screen.width,30f),"   heading="+(int)Input.compass.magneticHeading+"   initCompass="+(int)initCompassHeading);
		#endif
		//GUI.Box (new Rect (0f, 0f, Screen.width, 30f), "r=" + vrCameraLocal.localRotation.eulerAngles+"  a="+Input.acceleration);
	}
	
	#if UNITY_IPHONE || UNITY_WP8
	private static Quaternion ConvertRotation(Quaternion q)
	{
		return new Quaternion(q.x, q.y, -q.z, -q.w);
	}
	#endif
	
	#if UNITY_ANDROID
	public static void TransformFromMatrix(Matrix4x4 matrix, Transform trans) {
		
		trans.localRotation = QuaternionFromMatrix(matrix);
		
	}
	
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
		
		Quaternion q = new Quaternion();
		q.w = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] + m[1,1] + m[2,2] ) ) / 2; 
		q.x = Mathf.Sqrt( Mathf.Max( 0, 1 + m[0,0] - m[1,1] - m[2,2] ) ) / 2; 
		q.y = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] + m[1,1] - m[2,2] ) ) / 2;
		q.z = Mathf.Sqrt( Mathf.Max( 0, 1 - m[0,0] - m[1,1] + m[2,2] ) ) / 2; 
		q.x *= Mathf.Sign( q.x * ( m[2,1] - m[1,2] ) );
		q.y *= Mathf.Sign( q.y * ( m[0,2] - m[2,0] ) );
		q.z *= Mathf.Sign( q.z * ( m[1,0] - m[0,1] ) );
		
		return q;
		
	}
	#endif
	
}