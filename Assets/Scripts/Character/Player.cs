using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	static public Player component;
	static public GameObject camera, gunCamera, player, invisCamera, aim, smallAim;
	static public CharacterController controller;
	static public AudioController audio;
	static public bool aimBall = false, inZero = false, scaleSmallAim = false;
	
	static bool hasBall = false, canRestart = false, scaleAim = false, needAim = false;
	
	float takeDistance = 2f;
	static public Ball lastBall;
	
	float rotationX = 0f;
	float rotationY = 0f;
	private float sensitivityX = 2000f;
	private float sensitivityY = 900F;
	public float Sensitivity = 1.5f;
	float DPI= Screen.dpi/100;
	float RotateCoef = 1f;
	private float minimumY = -90f;//-60F;
	private float maximumY = 90f;//60F;
	float originalRotation;
	static public float speed = 1f;
	
	
	Texture2D tex = null;
	
	static public bool CanRestart
	{
		set
		{
			canRestart = value;
		}
		get
		{
			return canRestart;
		}
	}
	
	static public bool InZeroRoom
	{
		get
		{
			return Level.ZeroRoom.trigger[2].PlayerStay;
		}
	}
	
	static public bool HasBall
	{
		get
		{
			return hasBall;
		}
		set
		{
			hasBall = value;
			
			//			if(hasBall)
			//				camera.GetComponent<MouseLook>().minimumY = -60f;
			//			else
			//				camera.GetComponent<MouseLook>().minimumY = -80f;
		}
	}
	
	static public void ActiveControl(bool active)
	{
		Player.speed = active ? 1f : 0f;
		
		Player.player.GetComponent<MouseLook> ().enabled = active;
		Player.camera.GetComponent<MouseLook> ().enabled = active;
	}
	
	static public IEnumerator DisableControl(float time)
	{
		//controller.enabled = false;
		Player.speed = 0f;
		Player.player.GetComponent<MouseLook> ().enabled = false;
		Player.camera.GetComponent<MouseLook> ().enabled = false;
		
		yield return new WaitForSeconds(time);
		
		Player.speed = 1f;
		controller.enabled = true;
		Player.player.GetComponent<MouseLook> ().enabled = true;
		Player.camera.GetComponent<MouseLook> ().enabled = true;
	}
	
	static public void Create()
	{
		Game.ShowMessage("Creating Player...");
		player = new GameObject("Player");
		
		controller = player.AddComponent<CharacterController>() as CharacterController;//.radius = 0.08f;
		//controller = player.AddComponent<CapsuleCollider>() as CapsuleCollider;
		controller.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");//"Player");
		controller.height = 2f;
		controller.radius = 0.4f;
		
		//player.AddComponent<Rigidbody>();
		controller.stepOffset = 0.3f;
		
		player.AddComponent<BoxCollider>().isTrigger = true;
		player.GetComponent<BoxCollider>().size *= 1.5f;
		
		player.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
		
		
		(camera = new GameObject("Camera")).AddComponent<Camera>();
		camera.tag = "MainCamera";
		camera.GetComponent<Camera>().depth = -1;
		camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Color; //CameraClearFlags.Depth;
		
		//camera.AddComponent<ExampleClass>();
		(gunCamera = new GameObject("GunCamera")).AddComponent<Camera>().depth = 1;
		gunCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
		
		gunCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Gun"); // | LayerMask.NameToLayer("Invisible");
		
		gunCamera.transform.SetParent(camera.transform);
		
		
		(invisCamera = new GameObject("InvisibleCamera")).AddComponent<Camera>().depth = 2;
		invisCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
		invisCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Invisible"); // | LayerMask.NameToLayer("Invisible");
		invisCamera.transform.SetParent(camera.transform);
		
		
		camera.transform.SetParent(player.transform);
		
		camera.transform.localEulerAngles = Vector3.right * (-5f);
		
		camera.GetComponent<Camera>().backgroundColor = Color.white;
		//camera.GetComponent<Camera>().nearClipPlane = 0.2f;
		camera.transform.localPosition = Vector3.up * 0.8f;//1.3f;
		camera.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
		//camera.AddComponent<TouchLook>();
		Rigidbody rb = camera.AddComponent<Rigidbody>() as Rigidbody;
		//HingeJoint joint = camera.AddComponent<HingeJoint>() as HingeJoint;
		FixedJoint joint = camera.AddComponent<FixedJoint>() as FixedJoint;
		
		rb.useGravity = false;
		rb.isKinematic = true;
		
		joint.enableCollision = true;
		joint.enablePreprocessing = false;
		//joint.useLimits = true;
		joint.anchor = Vector3.forward * 0.5f;
		joint.axis = Vector3.forward;
		
		joint.autoConfigureConnectedAnchor = true;
		
		player.tag = "Player";
		//player.AddComponent("CharacterMotor");
		//player.AddComponent("FPSInputController");
		player.AddComponent<Player>();
		audio = player.AddComponent<AudioController>();
		
		
		GameObject aim = CustomObject.Circle(0.0018f, Obj.Colour.WHITE, false, 16);
		aim.transform.parent = camera.transform;
		aim.transform.localEulerAngles = Vector3.right * (-90f);
		aim.transform.localPosition = Vector3.forward * (camera.GetComponent<Camera>().nearClipPlane + 0.0002f);
		aim.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
		aim.AddComponent<Animation>();
		aim.layer = LayerMask.NameToLayer("Ignore Raycast");
		
		GameObject aim2 = CustomObject.Circle(0.0018f, Obj.Colour.WHITE, false, 16); //30
		aim2.transform.parent = camera.transform;
		aim2.transform.localEulerAngles = Vector3.right * (-90f);
		aim2.transform.localScale = Vector3.zero;
		aim2.transform.localPosition = Vector3.forward * (camera.GetComponent<Camera>().nearClipPlane + 0.0001f);
		aim2.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
		aim2.AddComponent<Animation>();
		aim2.layer = LayerMask.NameToLayer("Ignore Raycast");
		
		//player.GetComponent<Player>().aim = aim;
		//player.GetComponent<Player>().smallAim = aim2;
		Player.aim = aim;
		Player.smallAim = aim2;
		
		//audio.PlayForTest(true);
		
		//player.AddComponent<Gravity>();
		
		
	}
	
	/*static public void Create(GameObject player)
	{
		//player = new GameObject("Player");
		//player.transform.eulerAngles -= Vector3.right*90f;
		//Debug.LogError("a");
		//return;
		controller = player.GetComponent<CharacterController>() as CharacterController;//.radius = 0.08f;
		//controller.height = 1f;
		//controller.radius = 0.4f;
		//controller.stepOffset = 0.1f;
		//controller.w
		
		//
		
		//player.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
		
		
		camera = player.GetComponentInChildren<Camera>().gameObject;//new GameObject("Camera")).AddComponent<Camera>();
		//camera.AddComponent<ExampleClass>();
		(gunCamera = new GameObject("GunCamera")).AddComponent<Camera>().depth = 1;
		gunCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
		
		gunCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Gun");
		
		gunCamera.transform.SetParent(camera.transform);
		//camera.transform.SetParent(player.transform);
		
		camera.GetComponent<Camera>().backgroundColor = Color.white;
		camera.GetComponent<Camera>().nearClipPlane = 0.2f;
		camera.transform.localPosition = Vector3.up * 1.3f;
		//camera.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
		Rigidbody rb = camera.AddComponent<Rigidbody>() as Rigidbody;
		HingeJoint joint = camera.AddComponent<HingeJoint>() as HingeJoint;
		
		rb.useGravity = false;
		rb.isKinematic = true;
		
		joint.enableCollision = true;
		joint.useLimits = true;
		joint.anchor = Vector3.forward * 0.5f;
		joint.axis = Vector3.forward;
		
		joint.autoConfigureConnectedAnchor = true;
		
		player.tag = "Player";
		//player.AddComponent("CharacterMotor");
		//player.AddComponent("FPSInputController");
		player.AddComponent<Player>();
		player.AddComponent<AudioController>();

		//AudioController.PlayForTest(true);
		
		//player.AddComponent<Gravity>();
		
		Player.player = player;
	}*/
	
	
	void Start()
	{
		//Test2 ();
		transform.localPosition = Vector3.up;
		//Player.camera = Camera.main.gameObject;
		//Player.gunCamera = (Player.camera.transform.GetChild(0)).gameObject;
		component = GetComponent<Player>() as Player;
		Player.gunCamera.SetActive(false);
		
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		originalRotation = transform.rotation.eulerAngles.y;// + 180f;
	}
	
	static public void SetPosition(Room room, Vector2 position)
	{
		Vector3 pos = room.side[2].transform.position;
		pos.y += controller.height * 0.51f;
		pos.x -= controller.radius - room.Size.x/2f + position.x*(room.Size.x - controller.radius*2f)/100f;
		pos.z += controller.radius - room.Size.z/2f + position.y*(room.Size.z - controller.radius*2f)/100f;
		player.transform.position = pos;
		
		Game.ShowWarning("POS");
	}
	
	static public void SetPosition(GameObject target)
	{
		Vector3 pos = target.transform.position;
		pos.y += controller.height - target.transform.localScale.y*0.5f;
		//pos.x -= controller.radius - room.Size.x/2f + position.x*(room.Size.x - controller.radius*2f)/100f;
		//pos.z += controller.radius - room.Size.z/2f + position.y*(room.Size.z - controller.radius*2f)/100f;
		player.transform.position = pos;
	}
	
	static public void EnableLook(bool en)
	{
		player.GetComponent<MouseLook>().enabled = en;
		camera.GetComponent<MouseLook>().enabled = en;
	}
	
	/*static public bool Aim(System.Type type)
	{

	}*/
	
	Vector2 tapPoint = Vector2.zero;
	
	public void TouchTakeThrowBall()
	{
		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began)
			{
				tapPoint = touch.position;
			}
			else if(touch.phase == TouchPhase.Ended)
			{
				if(tapPoint == touch.position)
				{
					Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(tapPoint);
					
					RaycastHit hit;
					
					if(Physics.Raycast(ray, out hit, takeDistance))
					{
						if(hit.transform.tag == "Ball")
						{
							
							Ball ball = lastBall = hit.transform.GetComponent<Ball>() as Ball;
							if(ball.InHands)
							{
								ball.Throw();
							}
							else
							{
								ball.Take();
							}
						}
						//						else
						//						{
						//							if(lastBall != null && lastBall.InHands)
						//							{
						//								lastBall.Throw();
						//								lastBall = null;
						//							}
						//						}
					}
					//					else if(lastBall != null && lastBall.InHands)
					//					{
					//						lastBall.Throw();
					//						lastBall = null;
					//					}
					
				}
			}
		}
	}
	
	
	public bool TakeThrowBall()
	{
		
		//Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		//if(Physics.Raycast(ray, out hit, takeDistance))
		
		//RaycastHit hit;
		if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
		{
			if(hit.transform.tag == "Ball")
			{
				
				Ball ball = lastBall = hit.transform.GetComponent<Ball>() as Ball;
				if(ball.InHands)
				{
					ball.Throw();
					aimBall = false;
					return false;
				}
				else
				{
					ball.Take();
					//ball.GetComponent<Rigidbody>().isKinematic = false;
					aimBall = true;
					return true;
				}
			}
			else
			{
				aimBall = false;
				
				if(lastBall != null && lastBall.InHands)
				{
					lastBall.Throw();
					lastBall = null;
					return false;
				}
			}
		}
		else if(lastBall != null && lastBall.InHands)
		{
			aimBall = false;
			lastBall.Throw();
			lastBall = null;
			return false;
		}
		
		return false;
		//Debug.Log(lastBall);
		//return aimBall;
	}
	
	void OnTriggerEnter(Collider col)
	{
		if(col.gameObject.tag == "Ball")
		{
			if(!col.gameObject.GetComponent<Ball>().InHands)
				Player.controller.stepOffset = 0.1f;
		}
	}
	
	
	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Ball")
		{
			Player.controller.stepOffset = 0.3f;
		}
	}
	
	/*void OnCollisionEnter(Collision col)
	{
		Debug.LogWarning("COL");
		if(col.gameObject.tag == "Ball")
		{
			controller.stepOffset = 0.1f;
		}
	}

	void OnCollisionExit(Collision col)
	{
		if(col.gameObject.tag == "Ball")
		{
			controller.stepOffset = 0.3f;
		}
	}*/
	
	void GravityControl()
	{
		if(Input.GetKeyUp(KeyCode.Q))
		{
			Physics.gravity = Vector3.right * 9.8f;
			//transform.localEulerAngles = Vector3.forward * 90f;
		}
	}

	Vector3 JoystickMovement()
	{
		Vector3 joyKeyControl = Vector3.zero;
		
		if(Input.GetKeyDown(KeyCode.JoystickButton4)) // Up
			joyKeyControl.z = 1;
		if(Input.GetKeyDown(KeyCode.JoystickButton6)) // Down
			joyKeyControl.z = -1;
		
		
		if(Input.GetKeyDown(KeyCode.JoystickButton7)) // Left
			joyKeyControl.x = -1;
		if(Input.GetKeyDown(KeyCode.JoystickButton5)) // Right
			joyKeyControl.x = 1;
		
		
		if(Input.GetKeyUp(KeyCode.JoystickButton4))
		{
			if(Input.GetKey(KeyCode.JoystickButton6))
				joyKeyControl.z = -1;
			else
				joyKeyControl.z = 0;
		}
		
		if(Input.GetKeyUp(KeyCode.JoystickButton6))
		{
			if(Input.GetKey(KeyCode.JoystickButton4))
				joyKeyControl.z = 1;
			else
				joyKeyControl.z = 0;
		}
		
		if(Input.GetKeyUp(KeyCode.JoystickButton7))
		{
			if(Input.GetKey(KeyCode.JoystickButton5))
				joyKeyControl.x = 1;
			else
				joyKeyControl.x = 0;
		}
		
		if(Input.GetKeyUp(KeyCode.JoystickButton5))
		{
			if(Input.GetKey(KeyCode.JoystickButton7))
				joyKeyControl.x = -1;
			else
				joyKeyControl.x = 0;
		}
		
		return joyKeyControl;
	}
	
	Vector3 KeyboardMovement()
	{
		return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
	}
	
	Vector2 beganVirtualStickPoint = Vector2.zero;
	
	void Control()
	{
		Vector3 joyKeyControl = JoystickMovement();
		Vector3 keyboardControl = KeyboardMovement();
		
		Vector3 directionVector = joyKeyControl != Vector3.zero ? joyKeyControl : keyboardControl;
		
		foreach (Touch touch in Input.touches)
		{
			if(touch.position.x > Screen.width/3f)
				//if(touch.position.x > -(Screen.width/3f) * 2f)
			{
				Vector2 Window2InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
				
				float InputX = (Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01f)) /DPI;
				float InputY = (Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01f)) /DPI;
				rotationX += InputX * RotateCoef;
				rotationY += InputY * RotateCoef;
				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
				transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.5f);
				camera.transform.localRotation =  Quaternion.Slerp (camera.transform.localRotation, Quaternion.Euler(camera.transform.localRotation.x-rotationY, 0, 0),  0.5f);
			}
			else
			{
				if(touch.phase == TouchPhase.Began)
				{
					beganVirtualStickPoint = touch.position;
				}
				else
				{
					float inputX = 0f, inputY = 0f;
					
					float max =  Screen.height/7f;
					float m = 1f/ max;
					
					inputY = touch.position.y - beganVirtualStickPoint.y;
					inputX = touch.position.x - beganVirtualStickPoint.x;
					
					inputX = Mathf.Abs(inputX) > max ? inputX/Mathf.Abs(inputX) : m * inputX;
					inputY = Mathf.Abs(inputY) > max ? inputY/Mathf.Abs(inputY) : m * inputY;
					
					directionVector = new Vector3(inputX, 0, inputY);
				}
				
				//				Vector2 Window2InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
				//				
				//				float InputX = (Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01f)) /DPI;
				//				float InputY = (Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01f)) /DPI;
				//
				//
				//				directionVector = new Vector3(InputX, 0, InputY);
			}
		}
		
		
		
		
		directionVector = transform.TransformDirection(directionVector);
		
		directionVector *= 4.5f;
		
		directionVector -= -Physics.gravity;
		controller.Move(directionVector  * Time.deltaTime * speed);
		
		//GravityControl();
	}
	
	
	void OnGUI()
	{
		if(tex != null)
			GUI.DrawTexture(new Rect(0,0,Screen.width/4f,Screen.height/4f), (Texture)tex);
	}
	
	IEnumerator TakeScreenCapture()
	{
		yield return new WaitForEndOfFrame();
		
		tex = new Texture2D(Screen.width, Screen.height);
		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
		tex.Apply();
	}
	
	
	
	static public void AimActive(bool aimbool, int vertsCount = 30)
	{
		float min = 0.0036f;
		float max = 0.006f;
		
		float time = 0.2f;
		
		//float timeForUp = ((max - aim.transform.localScale.x)) * ( time/(max - min) );
		//float timeForDown = ((aim.transform.localScale.x - min)) * ( time/(max - min) );
		
		Animation anim = aim.GetComponent<Animation>();
		Animation smallAnim = smallAim.GetComponent<Animation>();
		
		
		if(aimbool)
		{
			if(!scaleSmallAim)
			{
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.one * 0.002f, time);
				if(smallAnim.GetClip("Up") != null)
					smallAnim.RemoveClip("Up");
				
				smallAnim.AddClip(clip, "Up");
				smallAnim.Play("Up");
				
				
				scaleSmallAim = true;
			}
		}
		else if(scaleSmallAim)
		{
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.zero, time);
			if(smallAnim.GetClip("Down") != null)
				smallAnim.RemoveClip("Down");
			
			smallAnim.AddClip(clip, "Down");
			smallAnim.Play("Down");
			
			//Game.ScaleDownForAimTexture();
			scaleSmallAim = false;
		}
		
	}
	
	static public void AimControl(bool aimbool, int vertsCount = 30)
	{
		float min = 0.0036f;
		//float max = 0.006f;
		float max = 0.01f;
		
		float time = 0.2f;
		
		float timeForUp = ((max - aim.transform.localScale.x)) * ( time/(max - min) );
		float timeForDown = ((aim.transform.localScale.x - min)) * ( time/(max - min) );
		
		Animation anim = aim.GetComponent<Animation>();
		Animation smallAnim = smallAim.GetComponent<Animation>();
		
		
		if(aimbool)
		{
			if(!scaleAim)
			{
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, aim.transform.localScale, Vector3.one * max, timeForUp); 
				if(anim.GetClip("Up") != null)
					anim.RemoveClip("Up");
				
				anim.AddClip(clip, "Up");
				anim.Play("Up");
				
				
				clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.one * (max - min/2f), timeForUp);
				if(smallAnim.GetClip("Up") != null)
					smallAnim.RemoveClip("Up");
				
				smallAnim.AddClip(clip, "Up");
				smallAnim.Play("Up");
				
				
				scaleAim = true;
			}
		}
		else if(scaleAim)
		{
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, aim.transform.localScale, Vector3.one * min, timeForDown); 
			if(anim.GetClip("Down") != null)
				anim.RemoveClip("Down");
			
			anim.AddClip(clip, "Down");
			anim.Play("Down");
			
			
			clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.zero, timeForDown);
			if(smallAnim.GetClip("Down") != null)
				smallAnim.RemoveClip("Down");
			
			smallAnim.AddClip(clip, "Down");
			smallAnim.Play("Down");
			
			//Game.ScaleDownForAimTexture();
			scaleAim = false;
		}
		
	}
	
	int contactsCount = 0;
	bool contactOnlyBall = false;
	
	void LateUpdate()
	{
		if(contactOnlyBall && Player.lastBall)
		{
			//Player.lastBall.gameObject.GetComponent<Rigidbody>().Sleep();
			//Debug.LogWarning("______");
			//controller.Move(-Vector3.up*10f);
		}
		
		Cursor.visible = false;
		
	}

	Ball flyBall = null;
	
	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (hit.gameObject.tag != "Ball") {
			contactOnlyBall = false;
			flyBall = null;
		}
		else if(contactOnlyBall){
			Ball ball = hit.gameObject.GetComponent<Ball>();
			if(ball.InHands)
				flyBall = ball;
				//ball.Throw();
		}
	}
	
	bool squareAim = false;
	float squareVerts = 16f;

	
	void Update ()
	{
		//contactsCount = 0;
		if (contactOnlyBall && flyBall != null)
			flyBall.Throw ();
		contactOnlyBall = true;
		//Debug.LogWarning(GetComponent<CharacterController>().bounds.);
		//Debug.DrawRay(camera.transform.position, camera.transform.forward, Color.red);
		Control();
		
		if(HasBall && !squareAim)
		{
			//squareAim = true;
			AimControl(true);
			
			if(squareVerts > 4)
			{
				squareVerts -= 12f * Time.deltaTime * 5f;
				
				if(squareVerts < 4)
					squareVerts = 4;
				
				aim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
				smallAim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
			}
			else
			{
				squareAim = true;
			}
			
			
		}
		else if(!HasBall && squareAim)
		{
			//squareAim = false;
			//AimControl();
			
			if(squareVerts < 16)
			{
				squareVerts += 12f * Time.deltaTime * 5f;
				
				if(squareVerts > 16)
					squareVerts = 16;
				
				aim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
				smallAim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
			}
			else
			{
				squareAim = false;
			}
			
		}
		
		
		if(Input.GetKeyUp(KeyCode.C))
		{
			//RenderTexture txtr = Player.camera.GetComponent<Camera>().targetTexture;
			
			/*tex = new Texture2D(Screen.width, Screen.height);
			tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
			tex.Apply();*/
			
			//StartCoroutine(TakeScreenCapture());
		}
		
		//Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		
		
		if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
		{
			if(hit.transform.tag == "Ball" && !HasBall)
			{
				AimControl(true);
				needAim = true;
			}
			else if(needAim && !HasBall)
			{
				AimControl(false);
				needAim = false;
				if(lastBall != null && lastBall.InHands)
				{
					aimBall = false;
					//lastBall.Throw();
					//lastBall = null;
				}
			}
			else if(HasBall && hit.transform.tag != "Ball")
			{
				RaycastHit[] hits = Physics.RaycastAll(camera.transform.position, camera.transform.forward, takeDistance);
				
				foreach(RaycastHit h in hits)
				{
					if(h.transform.tag == "Ball")// && !h.transform.GetComponent<Ball>().destruction)
						h.transform.gameObject.GetComponent<Ball>().Throw();
				}
				
			}
			
		}
		else if(needAim && !HasBall)
		{
			AimControl(false);
			needAim = false;
			if(lastBall != null && lastBall.InHands)
			{
				aimBall = false;
				//lastBall.Throw();
				//lastBall = null;
			}
		}
		
		
		/*if(Input.GetMouseButtonUp(0))
		{
			TakeThrowBall();
		}
		else */if( Game.IsInputUseItemDown() )
		{
			TakeThrowBall();
		}
		
		if(Input.touchCount > 0)
			TouchTakeThrowBall();
		//		else if(aimBall)
		//		{
		//			//RaycastHit hit;
		//			if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
		//			{
		//				if(hit.transform.tag != "Ball")
		//				{
		//					if(lastBall != null && lastBall.InHands)
		//					{
		//						aimBall = false;
		//						lastBall.Throw();
		//						lastBall = null;
		//					}
		//				}
		//			}
		//		}
		
		
		
		if(Level.current.Index != 0 && !InZeroRoom)
		{
			if( Game.IsInputEscape() )
			{
				transform.parent = null;
				canRestart = false;
				
				ActiveControl(true);
				
				foreach(Ball ball in Level.current.ball)
					ball.Destroy();
				
				if(!Level.ZeroRoom.transform.parent.gameObject.activeSelf)
					Level.ZeroRoom.transform.parent.gameObject.SetActive(true);
				
				if(!Level.current.inletDoor.gameObject.activeSelf)
					Level.current.inletDoor.gameObject.SetActive(true);
				
				//if( Game.IsInputEscape() )
				SetPosition(Level.ZeroRoom, Vector2.one*50f);
				
				if(Level.current.gun != null)
				{
					if(Level.current.gun[0].inHands)
					{
						Level.current.gun[0].Destroy();
					}
					else
					{
						Level.current.gun[0].gameObject.SetActive(false);
					}
				}
				
				Level zero = Level.ZeroRoom.transform.parent.GetComponent<Level>();
				Door last = zero.outletDoor;
				
				
				foreach(GameObject obj in Level.current.outletDoor.dot)
				{
					obj.SetActive(false);
				}
				
				Level.current.outletDoor.cell = null;
				Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				Level.current.outletDoor.transform.position = last.transform.position;
				//Level.current.outletDoor.room.side[5].gameObject.GetComponent<Renderer>().enabled = true;
				
				if(Level.lastOutletDoor != null)
				{
					Level.lastOutletDoor.cell = null;
					Level.lastOutletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				}
				
				if(Level.current.inletDoor != null)
				{
					Level.current.inletDoor.cell = null;
					Level.current.inletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
					Level.current.inletDoor.gameObject.SetActive(false);
				}
				
				if(Level.current.outletDoor != null)
				{
					Level.current.outletDoor.cell = null;
					Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				}
				
				Level.ZeroRoom.transform.parent.gameObject.GetComponent<Level_0>().UpdateLevelIndex(Level.current.outletDoor);
				
				inZero = true;
				GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index; //Game.Progress;
				
				Player.player.transform.localEulerAngles = Vector3.zero;
				Player.camera.GetComponent<MouseLook>().rotationY = 0f;
				Player.camera.transform.localEulerAngles = Vector3.zero;
				
				
				for(int i=0; i<Level.current.transform.childCount; ++i)
				{
					Transform temp = Level.current.transform.GetChild(i);
					
					if(temp.GetComponent<Door>())
						continue;
					else
						temp.gameObject.SetActive(false);
				}
				
				/*if(canRestart && Game.IsInputRestart())
				{
					Level.lastOutletDoor = Level.current.outletDoor;
					Level.isNextLevelLoaded = true;
					Level.current.isNextLevelLoadedLocal = true;
					GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index;
					
					SetPosition(Level.lastOutletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>(), Vector2.one*50f + Vector2.up * 50f);
					//transform.position += Vector3.forward * 0.5f;
					
					StartCoroutine(Level.current.DestroyPreviousLevel());
				}*/
			}
			else if(canRestart && Game.IsInputRestart())
			{
				
				Level.current.outletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>().side[5].gameObject.GetComponent<Renderer>().enabled = true;





				gunCamera.SetActive(true);
				canRestart = false;
				
				StartCoroutine(DisableGun());
				
				foreach(Ball ball in Level.current.ball)
					ball.Destroy();
				
				if(!Level.ZeroRoom.transform.parent.gameObject.activeSelf)
					Level.ZeroRoom.transform.parent.gameObject.SetActive(true);
				
				if(!Level.current.inletDoor.gameObject.activeSelf)
					Level.current.inletDoor.gameObject.SetActive(true);
				

				Level.current.outletDoor.whiteBubble.layer = Level.current.outletDoor.blackBubble.layer = LayerMask.NameToLayer("Gun");

				
				if(Level.current.gun != null)
				{
					if(Level.current.gun[0].inHands)
					{
						Level.current.gun[0].Destroy();
					}
					else
					{
						Level.current.gun[0].gameObject.SetActive(false);
					}
				}
				
				Level zero = Level.ZeroRoom.transform.parent.GetComponent<Level>();
				Door last = zero.outletDoor;
				Level.current.outletDoor.cell = null;
				Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				Level.current.outletDoor.transform.position = last.transform.position;
				
				if(Level.lastOutletDoor != null)
				{
					Level.lastOutletDoor.cell = null;
					Level.lastOutletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				}
				
				if(Level.current.inletDoor != null)
				{
					Level.current.inletDoor.cell = null;
					Level.current.inletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
					Level.current.inletDoor.gameObject.SetActive(false);
				}
				
				if(Level.current.outletDoor != null)
				{
					Level.current.outletDoor.cell = null;
					Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
				}
				
				Level.ZeroRoom.transform.parent.gameObject.GetComponent<Level_0>().UpdateLevelIndex(Level.current.outletDoor);
				
				inZero = true;
				//GameObject.FindObjectOfType<Level_0>().binary = Game.Progress;
				
				Player.player.transform.localEulerAngles = Vector3.zero;
				Player.camera.GetComponent<MouseLook>().rotationY = 0f;
				Player.camera.transform.localEulerAngles = Vector3.zero;
				
				Level.lastOutletDoor = Level.current.outletDoor;
				Level.isNextLevelLoaded = true;
				Level.current.isNextLevelLoadedLocal = true;
				GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index;
				
				SetPosition(Level.lastOutletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>(), Vector2.one*50f + Vector2.up * 50f);
				//transform.position += Vector3.forward * 0.5f;
				
				StartCoroutine(Level.current.DestroyPreviousLevel());
			}
		}
	}
	
	IEnumerator DisableGun()
	{
		yield return new WaitForSeconds (Game.drawTime);
		
		gunCamera.SetActive (false);
	}
	
	void SetZeroRotation()
	{
		Player.player.transform.localEulerAngles = Vector3.zero;
		Player.camera.transform.localEulerAngles = Vector3.zero;
	}
	
	
}












//using UnityEngine;
//using System.Collections;
//
//public class Player : MonoBehaviour 
//{
//	static public Player component;
//	static public GameObject camera, gunCamera, player, invisCamera, aim, smallAim;
//	static public CapsuleCollider controller;
//	static public AudioController audio;
//	static public bool aimBall = false, inZero = false, scaleSmallAim = false;
//
//	static bool hasBall = false, canRestart = false, scaleAim = false, needAim = false;
//
//	float takeDistance = 2f;
//	static public Ball lastBall;
//
//	float rotationX = 0f;
//	float rotationY = 0f;
//	private float sensitivityX = 2000f;
//	private float sensitivityY = 900F;
//	public float Sensitivity = 1.5f;
//	float DPI= Screen.dpi/100;
//	float RotateCoef = 1f;
//	private float minimumY = -90f;//-60F;
//	private float maximumY = 90f;//60F;
//	float originalRotation;
//	static public float speed = 1f;
//	
//
//	Texture2D tex = null;
//
//	static public bool CanRestart
//	{
//		set
//		{
//			canRestart = value;
//		}
//		get
//		{
//			return canRestart;
//		}
//	}
//
//	static public bool InZeroRoom
//	{
//		get
//		{
//			return Level.ZeroRoom.trigger[2].PlayerStay;
//		}
//	}
//
//	static public bool HasBall
//	{
//		get
//		{
//			return hasBall;
//		}
//		set
//		{
//			hasBall = value;
//
////			if(hasBall)
////				camera.GetComponent<MouseLook>().minimumY = -60f;
////			else
////				camera.GetComponent<MouseLook>().minimumY = -80f;
//		}
//	}
//
//	static public void ActiveControl(bool active)
//	{
//		Player.speed = active ? 1f : 0f;
//
//		Player.player.GetComponent<MouseLook> ().enabled = active;
//		Player.camera.GetComponent<MouseLook> ().enabled = active;
//	}
//
//	static public IEnumerator DisableControl(float time)
//	{
//		//controller.enabled = false;
//		Player.speed = 0f;
//		Player.player.GetComponent<MouseLook> ().enabled = false;
//		Player.camera.GetComponent<MouseLook> ().enabled = false;
//		
//		yield return new WaitForSeconds(time);
//
//		Player.speed = 1f;
//		controller.enabled = true;
//		Player.player.GetComponent<MouseLook> ().enabled = true;
//		Player.camera.GetComponent<MouseLook> ().enabled = true;
//	}
//
//	static public void Create()
//	{
//		Game.ShowMessage("Creating Player...");
//		player = new GameObject("Player");
//
//		//controller = player.AddComponent<CharacterController>() as CharacterController;//.radius = 0.08f;
//		controller = player.AddComponent<CapsuleCollider>() as CapsuleCollider;
//		controller.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");//"Player");
//		controller.height = 2f;
//		controller.radius = 0.4f;
//
//		player.AddComponent<Rigidbody>();
//		//controller.stepOffset = 0.3f;
//
//		player.AddComponent<BoxCollider>().isTrigger = true;
//		player.GetComponent<BoxCollider>().size *= 1.5f;
//		
//		player.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
//
//		
//		(camera = new GameObject("Camera")).AddComponent<Camera>();
//		camera.tag = "MainCamera";
//		camera.GetComponent<Camera>().depth = -1;
//		camera.GetComponent<Camera>().clearFlags = CameraClearFlags.Color; //CameraClearFlags.Depth;
//
//		//camera.AddComponent<ExampleClass>();
//		(gunCamera = new GameObject("GunCamera")).AddComponent<Camera>().depth = 1;
//		gunCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
//		
//		gunCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Gun"); // | LayerMask.NameToLayer("Invisible");
//		
//		gunCamera.transform.SetParent(camera.transform);
//
//
//		(invisCamera = new GameObject("InvisibleCamera")).AddComponent<Camera>().depth = 2;
//		invisCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
//		invisCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Invisible"); // | LayerMask.NameToLayer("Invisible");
//		invisCamera.transform.SetParent(camera.transform);
//
//
//		camera.transform.SetParent(player.transform);
//
//		camera.transform.localEulerAngles = Vector3.right * (-5f);
//		
//		camera.GetComponent<Camera>().backgroundColor = Color.white;
//		//camera.GetComponent<Camera>().nearClipPlane = 0.2f;
//		camera.transform.localPosition = Vector3.up * 0.8f;//1.3f;
//		camera.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
//		//camera.AddComponent<TouchLook>();
//		Rigidbody rb = camera.AddComponent<Rigidbody>() as Rigidbody;
//		//HingeJoint joint = camera.AddComponent<HingeJoint>() as HingeJoint;
//		FixedJoint joint = camera.AddComponent<FixedJoint>() as FixedJoint;
//		
//		rb.useGravity = false;
//		rb.isKinematic = true;
//		
//		joint.enableCollision = true;
//		joint.enablePreprocessing = false;
//		//joint.useLimits = true;
//		joint.anchor = Vector3.forward * 0.5f;
//		joint.axis = Vector3.forward;
//		
//		joint.autoConfigureConnectedAnchor = true;
//		
//		player.tag = "Player";
//		//player.AddComponent("CharacterMotor");
//		//player.AddComponent("FPSInputController");
//		player.AddComponent<Player>();
//		audio = player.AddComponent<AudioController>();
//
//
//		GameObject aim = CustomObject.Circle(0.0018f, Obj.Colour.WHITE, false, 16);
//		aim.transform.parent = camera.transform;
//		aim.transform.localEulerAngles = Vector3.right * (-90f);
//		aim.transform.localPosition = Vector3.forward * (camera.GetComponent<Camera>().nearClipPlane + 0.0002f);
//		aim.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
//		aim.AddComponent<Animation>();
//		aim.layer = LayerMask.NameToLayer("Ignore Raycast");
//
//		GameObject aim2 = CustomObject.Circle(0.0018f, Obj.Colour.WHITE, false, 16); //30
//		aim2.transform.parent = camera.transform;
//		aim2.transform.localEulerAngles = Vector3.right * (-90f);
//		aim2.transform.localScale = Vector3.zero;
//		aim2.transform.localPosition = Vector3.forward * (camera.GetComponent<Camera>().nearClipPlane + 0.0001f);
//		aim2.GetComponent<Renderer>().material.shader = Shader.Find("InverseColor");
//		aim2.AddComponent<Animation>();
//		aim2.layer = LayerMask.NameToLayer("Ignore Raycast");
//
//		//player.GetComponent<Player>().aim = aim;
//		//player.GetComponent<Player>().smallAim = aim2;
//		Player.aim = aim;
//		Player.smallAim = aim2;
//
//		//audio.PlayForTest(true);
//		
//		//player.AddComponent<Gravity>();
//		
//		
//	}
//
//	/*static public void Create(GameObject player)
//	{
//		//player = new GameObject("Player");
//		//player.transform.eulerAngles -= Vector3.right*90f;
//		//Debug.LogError("a");
//		//return;
//		controller = player.GetComponent<CharacterController>() as CharacterController;//.radius = 0.08f;
//		//controller.height = 1f;
//		//controller.radius = 0.4f;
//		//controller.stepOffset = 0.1f;
//		//controller.w
//		
//		//
//		
//		//player.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseX;
//		
//		
//		camera = player.GetComponentInChildren<Camera>().gameObject;//new GameObject("Camera")).AddComponent<Camera>();
//		//camera.AddComponent<ExampleClass>();
//		(gunCamera = new GameObject("GunCamera")).AddComponent<Camera>().depth = 1;
//		gunCamera.GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
//		
//		gunCamera.GetComponent<Camera>().cullingMask = 1 << LayerMask.NameToLayer("Gun");
//		
//		gunCamera.transform.SetParent(camera.transform);
//		//camera.transform.SetParent(player.transform);
//		
//		camera.GetComponent<Camera>().backgroundColor = Color.white;
//		camera.GetComponent<Camera>().nearClipPlane = 0.2f;
//		camera.transform.localPosition = Vector3.up * 1.3f;
//		//camera.AddComponent<MouseLook>().axes = MouseLook.RotationAxes.MouseY;
//		Rigidbody rb = camera.AddComponent<Rigidbody>() as Rigidbody;
//		HingeJoint joint = camera.AddComponent<HingeJoint>() as HingeJoint;
//		
//		rb.useGravity = false;
//		rb.isKinematic = true;
//		
//		joint.enableCollision = true;
//		joint.useLimits = true;
//		joint.anchor = Vector3.forward * 0.5f;
//		joint.axis = Vector3.forward;
//		
//		joint.autoConfigureConnectedAnchor = true;
//		
//		player.tag = "Player";
//		//player.AddComponent("CharacterMotor");
//		//player.AddComponent("FPSInputController");
//		player.AddComponent<Player>();
//		player.AddComponent<AudioController>();
//
//		//AudioController.PlayForTest(true);
//		
//		//player.AddComponent<Gravity>();
//		
//		Player.player = player;
//	}*/
//
//
//	void Start()
//	{
//		//Test2 ();
//		transform.localPosition = Vector3.up;
//		//Player.camera = Camera.main.gameObject;
//		//Player.gunCamera = (Player.camera.transform.GetChild(0)).gameObject;
//		component = GetComponent<Player>() as Player;
//		Player.gunCamera.SetActive(false);
//
//		Cursor.visible = false;
//		Cursor.lockState = CursorLockMode.Locked;
//		originalRotation = transform.rotation.eulerAngles.y;// + 180f;
//	}
//
//	static public void SetPosition(Room room, Vector2 position)
//	{
//		Vector3 pos = room.side[2].transform.position;
//		pos.y += controller.height * 0.51f;
//		pos.x -= controller.radius - room.Size.x/2f + position.x*(room.Size.x - controller.radius*2f)/100f;
//		pos.z += controller.radius - room.Size.z/2f + position.y*(room.Size.z - controller.radius*2f)/100f;
//		player.transform.position = pos;
//
//		Game.ShowWarning("POS");
//	}
//
//	static public void SetPosition(GameObject target)
//	{
//		Vector3 pos = target.transform.position;
//		pos.y += controller.height - target.transform.localScale.y*0.5f;
//		//pos.x -= controller.radius - room.Size.x/2f + position.x*(room.Size.x - controller.radius*2f)/100f;
//		//pos.z += controller.radius - room.Size.z/2f + position.y*(room.Size.z - controller.radius*2f)/100f;
//		player.transform.position = pos;
//	}
//
//	static public void EnableLook(bool en)
//	{
//		player.GetComponent<MouseLook>().enabled = en;
//		camera.GetComponent<MouseLook>().enabled = en;
//	}
//
//	/*static public bool Aim(System.Type type)
//	{
//
//	}*/
//
//	Vector2 tapPoint = Vector2.zero;
//
//	public void TouchTakeThrowBall()
//	{
//		foreach(Touch touch in Input.touches)
//		{
//			if(touch.phase == TouchPhase.Began)
//			{
//				tapPoint = touch.position;
//			}
//			else if(touch.phase == TouchPhase.Ended)
//			{
//				if(tapPoint == touch.position)
//				{
//					Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(tapPoint);
//
//					RaycastHit hit;
//
//					if(Physics.Raycast(ray, out hit, takeDistance))
//					{
//						if(hit.transform.tag == "Ball")
//						{
//							
//							Ball ball = lastBall = hit.transform.GetComponent<Ball>() as Ball;
//							if(ball.InHands)
//							{
//								ball.Throw();
//							}
//							else
//							{
//								ball.Take();
//							}
//						}
////						else
////						{
////							if(lastBall != null && lastBall.InHands)
////							{
////								lastBall.Throw();
////								lastBall = null;
////							}
////						}
//					}
////					else if(lastBall != null && lastBall.InHands)
////					{
////						lastBall.Throw();
////						lastBall = null;
////					}
//
//				}
//			}
//		}
//	}
//
//
//	public bool TakeThrowBall()
//	{
//
//		//Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
//		
//		RaycastHit hit;
//		//if(Physics.Raycast(ray, out hit, takeDistance))
//
//		//RaycastHit hit;
//		if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
//		{
//			if(hit.transform.tag == "Ball")
//			{
//
//				Ball ball = lastBall = hit.transform.GetComponent<Ball>() as Ball;
//				if(ball.InHands)
//				{
//					ball.Throw();
//					aimBall = false;
//					return false;
//				}
//				else
//				{
//					ball.Take();
//					//ball.GetComponent<Rigidbody>().isKinematic = false;
//					aimBall = true;
//					return true;
//				}
//			}
//			else
//			{
//				aimBall = false;
//
//				if(lastBall != null && lastBall.InHands)
//				{
//					lastBall.Throw();
//					lastBall = null;
//					return false;
//				}
//			}
//		}
//		else if(lastBall != null && lastBall.InHands)
//		{
//			aimBall = false;
//			lastBall.Throw();
//			lastBall = null;
//			return false;
//		}
//
//		return false;
//		//Debug.Log(lastBall);
//		//return aimBall;
//	}
//
//	void OnTriggerEnter(Collider col)
//	{
//		if(col.gameObject.tag == "Ball")
//		{
////			if(!col.gameObject.GetComponent<Ball>().InHands)
////				Player.controller.stepOffset = 0.1f;
//		}
//	}
//	
//	
//	void OnTriggerExit(Collider col)
//	{
//		if(col.gameObject.tag == "Ball")
//		{
//			//Player.controller.stepOffset = 0.3f;
//		}
//	}
//
//	/*void OnCollisionEnter(Collision col)
//	{
//		Debug.LogWarning("COL");
//		if(col.gameObject.tag == "Ball")
//		{
//			controller.stepOffset = 0.1f;
//		}
//	}
//
//	void OnCollisionExit(Collision col)
//	{
//		if(col.gameObject.tag == "Ball")
//		{
//			controller.stepOffset = 0.3f;
//		}
//	}*/
//
//	void GravityControl()
//	{
//		if(Input.GetKeyUp(KeyCode.Q))
//		{
//			Physics.gravity = Vector3.right * 9.8f;
//			//transform.localEulerAngles = Vector3.forward * 90f;
//		}
//	}
//	Vector3 JoystickMovement()
//	{
//		Vector3 joyKeyControl = Vector3.zero;
//
//		if(Input.GetKeyDown(KeyCode.JoystickButton4)) // Up
//			joyKeyControl.z = 1;
//		if(Input.GetKeyDown(KeyCode.JoystickButton6)) // Down
//			joyKeyControl.z = -1;
//		
//		
//		if(Input.GetKeyDown(KeyCode.JoystickButton7)) // Left
//			joyKeyControl.x = -1;
//		if(Input.GetKeyDown(KeyCode.JoystickButton5)) // Right
//			joyKeyControl.x = 1;
//		
//		
//		if(Input.GetKeyUp(KeyCode.JoystickButton4))
//		{
//			if(Input.GetKey(KeyCode.JoystickButton6))
//				joyKeyControl.z = -1;
//			else
//				joyKeyControl.z = 0;
//		}
//		
//		if(Input.GetKeyUp(KeyCode.JoystickButton6))
//		{
//			if(Input.GetKey(KeyCode.JoystickButton4))
//				joyKeyControl.z = 1;
//			else
//				joyKeyControl.z = 0;
//		}
//		
//		if(Input.GetKeyUp(KeyCode.JoystickButton7))
//		{
//			if(Input.GetKey(KeyCode.JoystickButton5))
//				joyKeyControl.x = 1;
//			else
//				joyKeyControl.x = 0;
//		}
//		
//		if(Input.GetKeyUp(KeyCode.JoystickButton5))
//		{
//			if(Input.GetKey(KeyCode.JoystickButton7))
//				joyKeyControl.x = -1;
//			else
//				joyKeyControl.x = 0;
//		}
//
//		return joyKeyControl;
//	}
//
//	Vector3 KeyboardMovement()
//	{
//		return new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
//	}
//
//	Vector2 beganVirtualStickPoint = Vector2.zero;
//
//	void Control()
//	{
//		Vector3 joyKeyControl = JoystickMovement();
//		Vector3 keyboardControl = KeyboardMovement();
//		
//		Vector3 directionVector = joyKeyControl != Vector3.zero ? joyKeyControl : keyboardControl;
//
//		foreach (Touch touch in Input.touches)
//		{
//			if(touch.position.x > Screen.width/3f)
//			//if(touch.position.x > -(Screen.width/3f) * 2f)
//			{
//				Vector2 Window2InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
//
//				float InputX = (Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01f)) /DPI;
//				float InputY = (Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01f)) /DPI;
//				rotationX += InputX * RotateCoef;
//				rotationY += InputY * RotateCoef;
//				rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);
//				transform.rotation =  Quaternion.Slerp (transform.rotation, Quaternion.Euler(0, originalRotation + rotationX, 0),  0.5f);
//				camera.transform.localRotation =  Quaternion.Slerp (camera.transform.localRotation, Quaternion.Euler(camera.transform.localRotation.x-rotationY, 0, 0),  0.5f);
//			}
//			else
//			{
//				if(touch.phase == TouchPhase.Began)
//				{
//					beganVirtualStickPoint = touch.position;
//				}
//				else
//				{
//					float inputX = 0f, inputY = 0f;
//		
//					float max =  Screen.height/7f;
//					float m = 1f/ max;
//
//					inputY = touch.position.y - beganVirtualStickPoint.y;
//					inputX = touch.position.x - beganVirtualStickPoint.x;
//
//					inputX = Mathf.Abs(inputX) > max ? inputX/Mathf.Abs(inputX) : m * inputX;
//					inputY = Mathf.Abs(inputY) > max ? inputY/Mathf.Abs(inputY) : m * inputY;
//
//					directionVector = new Vector3(inputX, 0, inputY);
//				}
//
////				Vector2 Window2InputSlide = touch.deltaPosition*Time.smoothDeltaTime;
////				
////				float InputX = (Window2InputSlide.x  * ((sensitivityX*Sensitivity)*0.01f)) /DPI;
////				float InputY = (Window2InputSlide.y * ((sensitivityY*Sensitivity)*0.01f)) /DPI;
////
////
////				directionVector = new Vector3(InputX, 0, InputY);
//			}
//		}
//
//
//
//
//		directionVector = transform.TransformDirection(directionVector);
//
//		directionVector *= 4.5f;
//
////		if (directionVector == Vector3.zero)
////			Debug.LogWarning ("zero");
//		//directionVector -= -Physics.gravity;
//		//controller.Move(directionVector  * Time.deltaTime * speed);
//		//if(directionVector != Vector3.zero)
//		GetComponent<Rigidbody> ().velocity = (directionVector * Time.deltaTime * speed).normalized*10f;
//		GetComponent<Rigidbody> ().velocity -= Vector3.up * 9.8f;
//
//		GravityControl();
//	}
//
//
//	void OnGUI()
//	{
//		if(tex != null)
//			GUI.DrawTexture(new Rect(0,0,Screen.width/4f,Screen.height/4f), (Texture)tex);
//	}
//
//	IEnumerator TakeScreenCapture()
//	{
//		yield return new WaitForEndOfFrame();
//
//		tex = new Texture2D(Screen.width, Screen.height);
//		tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
//		tex.Apply();
//	}
//
//
//
//	static public void AimActive(bool aimbool, int vertsCount = 30)
//	{
//		float min = 0.0036f;
//		float max = 0.006f;
//		
//		float time = 0.2f;
//		
//		//float timeForUp = ((max - aim.transform.localScale.x)) * ( time/(max - min) );
//		//float timeForDown = ((aim.transform.localScale.x - min)) * ( time/(max - min) );
//		
//		Animation anim = aim.GetComponent<Animation>();
//		Animation smallAnim = smallAim.GetComponent<Animation>();
//		
//		
//		if(aimbool)
//		{
//			if(!scaleSmallAim)
//			{
//				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.one * 0.002f, time);
//				if(smallAnim.GetClip("Up") != null)
//					smallAnim.RemoveClip("Up");
//				
//				smallAnim.AddClip(clip, "Up");
//				smallAnim.Play("Up");
//				
//				
//				scaleSmallAim = true;
//			}
//		}
//		else if(scaleSmallAim)
//		{
//			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.zero, time);
//			if(smallAnim.GetClip("Down") != null)
//				smallAnim.RemoveClip("Down");
//			
//			smallAnim.AddClip(clip, "Down");
//			smallAnim.Play("Down");
//			
//			//Game.ScaleDownForAimTexture();
//			scaleSmallAim = false;
//		}
//		
//	}
//
//	static public void AimControl(bool aimbool, int vertsCount = 30)
//	{
//		float min = 0.0036f;
//		//float max = 0.006f;
//		float max = 0.01f;
//		
//		float time = 0.2f;
//		
//		float timeForUp = ((max - aim.transform.localScale.x)) * ( time/(max - min) );
//		float timeForDown = ((aim.transform.localScale.x - min)) * ( time/(max - min) );
//		
//		Animation anim = aim.GetComponent<Animation>();
//		Animation smallAnim = smallAim.GetComponent<Animation>();
//
//
//		if(aimbool)
//		{
//			if(!scaleAim)
//			{
//				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, aim.transform.localScale, Vector3.one * max, timeForUp); 
//				if(anim.GetClip("Up") != null)
//					anim.RemoveClip("Up");
//				
//				anim.AddClip(clip, "Up");
//				anim.Play("Up");
//				
//				
//				clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.one * (max - min/2f), timeForUp);
//				if(smallAnim.GetClip("Up") != null)
//					smallAnim.RemoveClip("Up");
//				
//				smallAnim.AddClip(clip, "Up");
//				smallAnim.Play("Up");
//				
//				
//				scaleAim = true;
//			}
//		}
//		else if(scaleAim)
//		{
//			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, aim.transform.localScale, Vector3.one * min, timeForDown); 
//			if(anim.GetClip("Down") != null)
//				anim.RemoveClip("Down");
//			
//			anim.AddClip(clip, "Down");
//			anim.Play("Down");
//			
//			
//			clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, smallAim.transform.localScale, Vector3.zero, timeForDown);
//			if(smallAnim.GetClip("Down") != null)
//				smallAnim.RemoveClip("Down");
//			
//			smallAnim.AddClip(clip, "Down");
//			smallAnim.Play("Down");
//			
//			//Game.ScaleDownForAimTexture();
//			scaleAim = false;
//		}
//
//	}
//
//	int contactsCount = 0;
//	bool contactOnlyBall = false;
//
//	void LateUpdate()
//	{
//		if(contactOnlyBall && Player.lastBall)
//		{
//			//Player.lastBall.gameObject.GetComponent<Rigidbody>().Sleep();
//			//Debug.LogWarning("______");
//			//controller.Move(-Vector3.up*10f);
//		}
//
//		Cursor.visible = false;
//
//	}
//
//	void OnControllerColliderHit(ControllerColliderHit hit)
//	{
//		if(hit.gameObject.tag != "Ball")
//			contactOnlyBall = false;
//	}
//
//	bool squareAim = false;
//	float squareVerts = 16f;
//
//	void Update ()
//	{
//		//contactsCount = 0;
//		contactOnlyBall = true;
//		//Debug.LogWarning(GetComponent<CharacterController>().bounds.);
//			//Debug.DrawRay(camera.transform.position, camera.transform.forward, Color.red);
//		Control();
//
//		if(HasBall && !squareAim)
//		{
//			//squareAim = true;
//			AimControl(true);
//
//			if(squareVerts > 4)
//			{
//				squareVerts -= 12f * Time.deltaTime * 5f;
//
//				if(squareVerts < 4)
//					squareVerts = 4;
//
//				aim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
//				smallAim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
//			}
//			else
//			{
//				squareAim = true;
//			}
//
//
//		}
//		else if(!HasBall && squareAim)
//		{
//			//squareAim = false;
//			//AimControl();
//
//			if(squareVerts < 16)
//			{
//				squareVerts += 12f * Time.deltaTime * 5f;
//
//				if(squareVerts > 16)
//					squareVerts = 16;
//
//				aim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
//				smallAim.GetComponent<MeshFilter>().mesh = CustomMesh.Circle((int)squareVerts);
//			}
//			else
//			{
//				squareAim = false;
//			}
//
//		}
//
//
//		if(Input.GetKeyUp(KeyCode.C))
//		{
//			//RenderTexture txtr = Player.camera.GetComponent<Camera>().targetTexture;
//
//			/*tex = new Texture2D(Screen.width, Screen.height);
//			tex.ReadPixels(new Rect(0,0,Screen.width,Screen.height),0,0);
//			tex.Apply();*/
//
//			//StartCoroutine(TakeScreenCapture());
//		}
//
//		//Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
//		
//		RaycastHit hit;
//
//
//		if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
//		{
//			if(hit.transform.tag == "Ball" && !HasBall)
//			{
//				AimControl(true);
//				needAim = true;
//			}
//			else if(needAim && !HasBall)
//			{
//				AimControl(false);
//				needAim = false;
//				if(lastBall != null && lastBall.InHands)
//				{
//					aimBall = false;
//					//lastBall.Throw();
//					//lastBall = null;
//				}
//			}
//			else if(HasBall && hit.transform.tag != "Ball")
//			{
//				RaycastHit[] hits = Physics.RaycastAll(camera.transform.position, camera.transform.forward, takeDistance);
//
//				foreach(RaycastHit h in hits)
//				{
//					if(h.transform.tag == "Ball")
//						h.transform.gameObject.GetComponent<Ball>().Throw();
//				}
//
//			}
//
//		}
//		else if(needAim && !HasBall)
//		{
//			AimControl(false);
//			needAim = false;
//			if(lastBall != null && lastBall.InHands)
//			{
//				aimBall = false;
//				//lastBall.Throw();
//				//lastBall = null;
//			}
//		}
//
//
//		/*if(Input.GetMouseButtonUp(0))
//		{
//			TakeThrowBall();
//		}
//		else */if( Game.IsInputUseItemDown() )
//		{
//			TakeThrowBall();
//		}
//
//		if(Input.touchCount > 0)
//			TouchTakeThrowBall();
////		else if(aimBall)
////		{
////			//RaycastHit hit;
////			if(Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, takeDistance))
////			{
////				if(hit.transform.tag != "Ball")
////				{
////					if(lastBall != null && lastBall.InHands)
////					{
////						aimBall = false;
////						lastBall.Throw();
////						lastBall = null;
////					}
////				}
////			}
////		}
//
//
//
//		if(Level.current.Index != 0 && !InZeroRoom)
//		{
//			if( Game.IsInputEscape() )
//			{
//				canRestart = false;
//
//				ActiveControl(true);
//
//				foreach(Ball ball in Level.current.ball)
//					ball.Destroy();
//
//				if(!Level.ZeroRoom.transform.parent.gameObject.activeSelf)
//					Level.ZeroRoom.transform.parent.gameObject.SetActive(true);
//
//				if(!Level.current.inletDoor.gameObject.activeSelf)
//					Level.current.inletDoor.gameObject.SetActive(true);
//
//				//if( Game.IsInputEscape() )
//					SetPosition(Level.ZeroRoom, Vector2.one*50f);
//
//				if(Level.current.gun != null)
//				{
//					if(Level.current.gun[0].inHands)
//					{
//						Level.current.gun[0].Destroy();
//					}
//					else
//					{
//						Level.current.gun[0].gameObject.SetActive(false);
//					}
//				}
//
//				Level zero = Level.ZeroRoom.transform.parent.GetComponent<Level>();
//				Door last = zero.outletDoor;
//
//
//				foreach(GameObject obj in Level.current.outletDoor.dot)
//				{
//					obj.SetActive(false);
//				}
//
//				Level.current.outletDoor.cell = null;
//				Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				Level.current.outletDoor.transform.position = last.transform.position;
//				//Level.current.outletDoor.room.side[5].gameObject.GetComponent<Renderer>().enabled = true;
//
//				if(Level.lastOutletDoor != null)
//				{
//					Level.lastOutletDoor.cell = null;
//					Level.lastOutletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				}
//
//				if(Level.current.inletDoor != null)
//				{
//					Level.current.inletDoor.cell = null;
//					Level.current.inletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//					Level.current.inletDoor.gameObject.SetActive(false);
//				}
//
//				if(Level.current.outletDoor != null)
//				{
//					Level.current.outletDoor.cell = null;
//					Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				}
//
//				Level.ZeroRoom.transform.parent.gameObject.GetComponent<Level_0>().UpdateLevelIndex(Level.current.outletDoor);
//
//				inZero = true;
//				GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index; //Game.Progress;
//
//				Player.player.transform.localEulerAngles = Vector3.zero;
//				Player.camera.GetComponent<MouseLook>().rotationY = 0f;
//				Player.camera.transform.localEulerAngles = Vector3.zero;
//
//
//				for(int i=0; i<Level.current.transform.childCount; ++i)
//				{
//					Transform temp = Level.current.transform.GetChild(i);
//
//					if(temp.GetComponent<Door>())
//						continue;
//					else
//						temp.gameObject.SetActive(false);
//				}
//
//				/*if(canRestart && Game.IsInputRestart())
//				{
//					Level.lastOutletDoor = Level.current.outletDoor;
//					Level.isNextLevelLoaded = true;
//					Level.current.isNextLevelLoadedLocal = true;
//					GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index;
//					
//					SetPosition(Level.lastOutletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>(), Vector2.one*50f + Vector2.up * 50f);
//					//transform.position += Vector3.forward * 0.5f;
//					
//					StartCoroutine(Level.current.DestroyPreviousLevel());
//				}*/
//			}
//			else if(canRestart && Game.IsInputRestart())
//			{
//
//				Level.current.outletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>().side[5].gameObject.GetComponent<Renderer>().enabled = true;
//
//				gunCamera.SetActive(true);
//				canRestart = false;
//
//				StartCoroutine(DisableGun());
//
//				foreach(Ball ball in Level.current.ball)
//					ball.Destroy();
//				
//				if(!Level.ZeroRoom.transform.parent.gameObject.activeSelf)
//					Level.ZeroRoom.transform.parent.gameObject.SetActive(true);
//				
//				if(!Level.current.inletDoor.gameObject.activeSelf)
//					Level.current.inletDoor.gameObject.SetActive(true);
//				
//
//				
//				if(Level.current.gun != null)
//				{
//					if(Level.current.gun[0].inHands)
//					{
//						Level.current.gun[0].Destroy();
//					}
//					else
//					{
//						Level.current.gun[0].gameObject.SetActive(false);
//					}
//				}
//				
//				Level zero = Level.ZeroRoom.transform.parent.GetComponent<Level>();
//				Door last = zero.outletDoor;
//				Level.current.outletDoor.cell = null;
//				Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				Level.current.outletDoor.transform.position = last.transform.position;
//				
//				if(Level.lastOutletDoor != null)
//				{
//					Level.lastOutletDoor.cell = null;
//					Level.lastOutletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				}
//				
//				if(Level.current.inletDoor != null)
//				{
//					Level.current.inletDoor.cell = null;
//					Level.current.inletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//					Level.current.inletDoor.gameObject.SetActive(false);
//				}
//				
//				if(Level.current.outletDoor != null)
//				{
//					Level.current.outletDoor.cell = null;
//					Level.current.outletDoor.openDoorTrigger = Level.ZeroRoom.trigger[0];
//				}
//				
//				Level.ZeroRoom.transform.parent.gameObject.GetComponent<Level_0>().UpdateLevelIndex(Level.current.outletDoor);
//				
//				inZero = true;
//				//GameObject.FindObjectOfType<Level_0>().binary = Game.Progress;
//				
//				Player.player.transform.localEulerAngles = Vector3.zero;
//				Player.camera.GetComponent<MouseLook>().rotationY = 0f;
//				Player.camera.transform.localEulerAngles = Vector3.zero;
//
//				Level.lastOutletDoor = Level.current.outletDoor;
//				Level.isNextLevelLoaded = true;
//				Level.current.isNextLevelLoadedLocal = true;
//				GameObject.FindObjectOfType<Level_0>().binary = Level.current.Index;
//
//				SetPosition(Level.lastOutletDoor.destroyPreviousLevelTrigger.transform.GetComponentInChildren<Room>(), Vector2.one*50f + Vector2.up * 50f);
//				//transform.position += Vector3.forward * 0.5f;
//
//				StartCoroutine(Level.current.DestroyPreviousLevel());
//			}
//		}
//	}
//
//	IEnumerator DisableGun()
//	{
//		yield return new WaitForSeconds (Game.drawTime);
//
//		gunCamera.SetActive (false);
//	}
//
//	void SetZeroRotation()
//	{
//		Player.player.transform.localEulerAngles = Vector3.zero;
//		Player.camera.transform.localEulerAngles = Vector3.zero;
//	}
//
//
//}
