using UnityEngine;
using System.Collections;

public class GravityGun : MonoBehaviour 
{
	GameObject gun, indicator, nameGun;
	Gun gunComponent;
	MeshFilter arrowMeshFilter;

	static float distance = 12f;

	static float minPower = 0f, maxPower = 11f;

	void SetLayerForName(GameObject obj)
	{
		obj.layer = LayerMask.NameToLayer("Gun");
		
		for(int i=0; i<obj.transform.childCount; ++i)
		{
			SetLayerForName(obj.transform.GetChild(i).gameObject);
		}
	}

	void Awake () 
	{
		GameObject parent = new GameObject("Gun");
		GameObject downPart = CustomObject.CreatePrimitive(PrimitiveType.Cylinder);
		GameObject topPart = GameObject.CreatePrimitive(PrimitiveType.Cylinder); //CustomObject.CreatePrimitive(PrimitiveType.Cylinder);

		downPart.tag = topPart.tag = "Gun";

		parent.layer = downPart.layer = topPart.layer = LayerMask.NameToLayer("Gun");

		Object.Destroy( downPart.GetComponent<Collider>() );
		Object.Destroy( topPart.GetComponent<Collider>() );

		downPart.transform.localScale = new Vector3(1, 2, 1);
		topPart.transform.localScale = new Vector3(1, .3f, 1);

		downPart.transform.localPosition = new Vector3(0, -0.3f, 0);
		topPart.transform.localPosition = new Vector3(0, 2f, 0);

		//downPart.renderer.material.color = Color.white;
		//topPart.renderer.material.color = Color.black;
		downPart.GetComponent<Renderer>().material = Ball.whiteBall;
		topPart.GetComponent<Renderer>().material = Ball.blackBall;

		downPart.transform.parent = topPart.transform.parent = parent.transform;

		parent.transform.localScale = Vector3.one * .2f;
		parent.transform.eulerAngles += 90f * Vector3.right;

		parent.transform.parent = transform;
		parent.AddComponent<Animation>();
		parent.GetComponent<Animation>().wrapMode = WrapMode.Loop;

		gun = parent;
		gun.tag = "Gun";
		transform.tag = "Gun";

		(gunComponent = gameObject.GetComponent<Gun>() as Gun).gun = parent;

		nameGun = Word.WriteString("gravity gun", 0.15f);
		nameGun.transform.SetParent(parent.transform);
		nameGun.transform.localEulerAngles = -Vector3.right * 90;
		nameGun.transform.localScale = Vector3.one * 0.4f;
		nameGun.layer = LayerMask.NameToLayer("Gun");
		nameGun.transform.localPosition = new Vector3(0, 1.25f, -0.5f);

		nameGun.SetActive(false);

		SetLayerForName(nameGun);

		indicator = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		indicator.transform.SetParent(parent.transform);
		(indicator.GetComponent<Renderer>().material = Game.BaseMaterial).color = Color.black;
		indicator.transform.localEulerAngles = Vector3.forward * 45;
		indicator.transform.localScale = Vector3.one * 0.4f;
		indicator.layer = LayerMask.NameToLayer("Gun");
		indicator.transform.localPosition = new Vector3(0, 2f, -0.5f); //1.85f

		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(0, 1.7f, -0.5f), new Vector3(0, 2f, -0.5f), 0.5f);


		indicator.AddComponent<Animation>().AddClip(clip, "Off");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(0, 2f, -0.5f), new Vector3(0, 1.7f, -0.5f), 0.5f);


		indicator.GetComponent<Animation>().AddClip(clip, "On");

		indicator.SetActive(false);

		/*GameObject arrow = CustomObject.ArrowForGravityGun(0.11f, 2f);
		arrowMeshFilter = arrow.GetComponent<MeshFilter>();
		arrow.transform.parent = transform;

		arrow.transform.localPosition = Vector3.zero + Vector3.up*0.15f;
		arrow.transform.localEulerAngles = Vector3.right*180f;//-Vector3.right*90f;

		arrow.transform.RotateAround(transform.position, Vector3.up, 45f);
		arrow.layer = LayerMask.NameToLayer("Gun");*/
	}

	/*void LateUpdate()
	{
		if(!Player.aimBall && arrowDirectionValue > 0)
		{
			arrowDirectionValue -= Time.deltaTime*5f;
			if(arrowDirectionValue < 0)
				arrowDirectionValue = 0;

			arrowMeshFilter.mesh = CustomMesh.ArrowForGravityGun(arrowDirectionValue);
		}
		else if(Player.aimBall && arrowDirectionValue < 1)
		{
			arrowDirectionValue += Time.deltaTime*5f;
			if(arrowDirectionValue > 1)
				arrowDirectionValue = 1;
			
			arrowMeshFilter.mesh = CustomMesh.ArrowForGravityGun(arrowDirectionValue);
		}
	}*/

	float arrowDirectionValue = 0;

	void Start()
	{
		float timeForAnimation = 2f;

		Quaternion start = Quaternion.Euler(new Vector3(90, 0, 0));
		Quaternion qua = Quaternion.Euler(new Vector3(90, 180, 0));//Quaternion.Euler(new Vector3(90, 180, 0));
		Quaternion qua2 = Quaternion.Euler(new Vector3(90, 360, 0));

		/*AnimationCurve curveX = new AnimationCurve(new Keyframe(0, start.x), new Keyframe(timeForAnimation, qua2.x));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, start.y), new Keyframe(timeForAnimation, qua2.y));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, start.z), new Keyframe(timeForAnimation, qua2.z));
		AnimationCurve curveW = new AnimationCurve(new Keyframe(0, start.w), new Keyframe(timeForAnimation, qua2.w));*/

		AnimationCurve curveX = new AnimationCurve(new Keyframe(0, start.x), new Keyframe(timeForAnimation/2f, qua.x), new Keyframe(timeForAnimation, qua2.x));
		AnimationCurve curveY = new AnimationCurve(new Keyframe(0, start.y), new Keyframe(timeForAnimation/2f, qua.y), new Keyframe(timeForAnimation, qua2.y));
		AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, start.z), new Keyframe(timeForAnimation/2f, qua.z), new Keyframe(timeForAnimation, qua2.z));		AnimationCurve curveW = new AnimationCurve(new Keyframe(0, start.w), new Keyframe(timeForAnimation/2f, qua.w), new Keyframe(timeForAnimation, qua2.w));

		curveX.SmoothTangents(0, 0);
		curveX.SmoothTangents(1, 0);
		curveX.SmoothTangents(2, 0);

		curveY.SmoothTangents(0, 0);
		curveY.SmoothTangents(1, 0);
		curveY.SmoothTangents(2, 0);

		curveZ.SmoothTangents(0, 0);
		curveZ.SmoothTangents(1, 0);
		curveZ.SmoothTangents(2, 0);

		curveW.SmoothTangents(0, 0);
		curveW.SmoothTangents(1, 0);
		curveW.SmoothTangents(2, 0);

		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		clip.SetCurve("", typeof(Transform), "localRotation.x", curveX);
		clip.SetCurve("", typeof(Transform), "localRotation.y", curveY);
		clip.SetCurve("", typeof(Transform), "localRotation.z", curveZ);
		clip.SetCurve("", typeof(Transform), "localRotation.w", curveW);
		clip.EnsureQuaternionContinuity();



		gun.GetComponent<Animation>().AddClip(clip, "anim");

		gun.GetComponent<Animation>().Play("anim");




	}

	float power = 0f;
	bool gravity = false;
	bool needAim = false;
	bool canDrag = false;

	void IndicatorUpdate()
	{
		if(!indicator.activeSelf)
			indicator.SetActive(true);

		RaycastHit hit;
		if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit, distance))
		{
			if(hit.transform.tag == "Ball" && !hit.transform.gameObject.GetComponent<Ball>().InHands)
			{
				Player.AimControl(true);
				needAim = true;

				if(!canDrag)
				{
					canDrag = true;

					AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, indicator.transform.localPosition, new Vector3(0, 1.7f, -0.5f), 0.5f);
					
					indicator.GetComponent<Animation>().RemoveClip("On");
					   
					   indicator.GetComponent<Animation>().AddClip(clip, "On");

					indicator.GetComponent<Animation>().Play("On");
				}

			}
			else if(canDrag)
			{
				canDrag = false;
				if(needAim)
				{
					needAim = false;
					Player.AimControl(false);
				}

				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, indicator.transform.localPosition, new Vector3(0, 2f, -0.5f), 0.5f);
				
				indicator.GetComponent<Animation>().RemoveClip("Off");

				indicator.GetComponent<Animation>().AddClip(clip, "Off");
				indicator.GetComponent<Animation>().Play("Off");
			}
		}
		else if(canDrag)
		{
			canDrag = false;
			if(needAim)
			{
				needAim = false;
				Player.AimControl(false);
			}
			
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, indicator.transform.localPosition, new Vector3(0, 2f, -0.5f), 0.5f);
			
			indicator.GetComponent<Animation>().RemoveClip("Off");
			
			indicator.GetComponent<Animation>().AddClip(clip, "Off");
			indicator.GetComponent<Animation>().Play("Off");
		}
	}

	bool TouchControl()
	{
		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
			{
				Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(touch.position);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, distance))
				{
					if(hit.transform.tag == "Ball" || hit.transform.tag == "Gun")
					{
						return true;
					}
				}
			}
		}

		return false;
	}

	bool TouchControlUp()
	{
		if(Input.touchCount == 0 && pressed)
		{
			pressed = false;
			return true;
		}

		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Ended)
			{
				Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(touch.position);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, distance))
				{
					if(hit.transform.tag == "Ball" || hit.transform.tag == "Gun")
					{
						pressed = false;
						return true;
					}
				}
			}
		}
		
		return false;
	}

	bool pressed = false;

	bool TouchControlDown()
	{
		foreach(Touch touch in Input.touches)
		{
			if(touch.phase == TouchPhase.Began)
			{
				Ray ray = Player.camera.GetComponent<Camera>().ScreenPointToRay(touch.position);
				RaycastHit hit;
				if(Physics.Raycast(ray, out hit, distance))
				{
					if(hit.transform.tag == "Ball" || hit.transform.tag == "Gun")
					{
						pressed = true;
						return true;
					}
				}
			}
		}
		
		return false;
	}

	void TouchGravity()
	{
		if(!Player.HasBall && (TouchControl()))
		{
			RaycastHit hit;
			if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit, distance))
			{
				if(hit.transform.tag == "Ball")
				{
					if(Player.component.TakeThrowBall())
					{
						gravity = true;
						return;
					}
					
					if(hit.rigidbody.isKinematic)
						hit.rigidbody.isKinematic = false;
					
					hit.rigidbody.AddForce(-Player.camera.transform.forward * 0.28f, ForceMode.VelocityChange); //14
					//hit.rigidbody.AddForce(-Player.camera.transform.forward * 0.15f, ForceMode.Impulse);
				}
			}
		}
		else if(Player.HasBall && (TouchControl()))
		{
			//Debug.LogWarning(power);
			if(power < maxPower)
				power += Time.deltaTime*maxPower;
			
		}
		else if(Player.HasBall && (TouchControlUp()))
		{
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .38f), new Vector3(.4f, -0.3f, .5f), 0.23f);
			GetComponent<Animation>().AddClip(clip, "Recoil_");
			GetComponent<Animation>().Play("Recoil_");
			
			if(gravity)
			{
				gravity = false;
				return;
			}
			
			Ball ball = Player.lastBall; //Player.camera.transform.GetComponentInChildren<Ball>();
			ball.Throw();
			//Player.component.TakeThrowBall();
			
			//ball.DestroyAnyway();
			ball.GetComponent<Rigidbody>().WakeUp();
			//ball.GetComponent<Rigidbody>().AddForce(Player.camera.transform.forward*power);
			ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
			ball.GetComponent<Rigidbody>().AddForce(Player.camera.transform.forward*power, ForceMode.VelocityChange);
			power = 0;
			
			
		}
		
		
		if(TouchControlUp())
		{
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .38f), new Vector3(.4f, -0.3f, .5f), 0.23f);
			GetComponent<Animation>().AddClip(clip, "Recoil_");
			GetComponent<Animation>().Play("Recoil_");
		}
		else if(TouchControlDown())
		{
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .5f), new Vector3(.4f, -0.3f, .38f), 0.23f);
			GetComponent<Animation>().AddClip(clip, "Recoil");
			GetComponent<Animation>().Play("Recoil");
		}
	}

	void FixedUpdate()
	{
		if(ball)
		{
			ball.AddForce(-Player.camera.transform.forward * maxPower * Time.fixedDeltaTime, ForceMode.VelocityChange); 
		}
	}

	Rigidbody ball = null;

	// Update is called once per frame
	void Update () 
	{
		if( gunComponent.inHands)
		{
			IndicatorUpdate();

			if(!nameGun.activeSelf)
				nameGun.SetActive(true);

			if(Input.touchCount > 0)
			{
				TouchGravity();
				return;
			}

			if(!Player.HasBall && (Input.GetMouseButton(1) || Input.GetMouseButton(0) ||
			                       Input.GetKey(KeyCode.JoystickButton14) || 
			                       Input.GetKey(KeyCode.JoystickButton9) || TouchControl()))
			{
				RaycastHit hit;
				if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit, distance))
				{
					if(hit.transform.tag == "Ball")
					{
						if(Player.component.TakeThrowBall())
						{
							gravity = true;
							return;
						}

						if(hit.rigidbody.isKinematic)
							hit.rigidbody.isKinematic = false;

						ball = hit.rigidbody;

						//hit.rigidbody.AddForce(-Player.camera.transform.forward * 0.28f, ForceMode.VelocityChange); //14 // FIXED UPDATE, SUKA
						//hit.rigidbody.AddForce(-Player.camera.transform.forward * 0.15f, ForceMode.Impulse);
					}
					else
						ball = null;
				}
				else
					ball = null;
			}
			else if(Player.HasBall && (Input.GetMouseButton(1) || Input.GetMouseButton(0) ||
			                           Input.GetKey(KeyCode.JoystickButton14) || 
			                           Input.GetKey(KeyCode.JoystickButton9) || TouchControl()))
			{
				//Debug.LogWarning(power);
				if(power < maxPower)
					power += Time.deltaTime*maxPower;

				ball = null;

			}
			else if(Player.HasBall && (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0) ||
			                           Input.GetKeyUp(KeyCode.JoystickButton14) || 
			                           Input.GetKeyUp(KeyCode.JoystickButton9) || TouchControlUp()))
			{
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .38f), new Vector3(.4f, -0.3f, .5f), 0.23f);
				GetComponent<Animation>().AddClip(clip, "Recoil_");
				GetComponent<Animation>().Play("Recoil_");

				if(gravity)
				{
					gravity = false;
					return;
				}

				Ball ball = Player.lastBall; //Player.camera.transform.GetComponentInChildren<Ball>();
				ball.Throw();
				//Player.component.TakeThrowBall();

				//ball.DestroyAnyway();
				ball.GetComponent<Rigidbody>().WakeUp();
				//ball.GetComponent<Rigidbody>().AddForce(Player.camera.transform.forward*power);
				ball.GetComponent<Rigidbody>().velocity = Vector3.zero;
				ball.GetComponent<Rigidbody>().AddForce(Player.camera.transform.forward*power, ForceMode.VelocityChange);
				power = 0;


			}


			if(Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(0) ||
			   Input.GetKeyUp(KeyCode.JoystickButton14) || 
			   Input.GetKeyUp(KeyCode.JoystickButton9) || TouchControlUp())
			{
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .38f), new Vector3(.4f, -0.3f, .5f), 0.23f);
				GetComponent<Animation>().AddClip(clip, "Recoil_");
				GetComponent<Animation>().Play("Recoil_");

				ball = null;
			}
			else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) ||
			        Input.GetKeyDown(KeyCode.JoystickButton14) || 
			        Input.GetKeyDown(KeyCode.JoystickButton9) || TouchControlDown())
			{
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .5f), new Vector3(.4f, -0.3f, .38f), 0.23f);
				GetComponent<Animation>().AddClip(clip, "Recoil");
				GetComponent<Animation>().Play("Recoil");
			}

		}
	}

}
