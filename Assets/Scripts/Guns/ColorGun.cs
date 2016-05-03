using UnityEngine;
using System.Collections;

public class ColorGun : MonoBehaviour 
{
	public GameObject gun, nameGun;
	Gun gunComponent;
	
	static float distance = 12f;

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
		GameObject topPart = CustomObject.CreatePrimitive(PrimitiveType.Cylinder);
		
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
		
		(gunComponent = gameObject.GetComponent<Gun>() as Gun).gun = parent;

		nameGun = Word.WriteString("color gun", 0.15f);
		nameGun.transform.SetParent(parent.transform);
		nameGun.transform.localEulerAngles = -Vector3.right * 90;
		nameGun.transform.localScale = Vector3.one * 0.4f;
		nameGun.layer = LayerMask.NameToLayer("Gun");
		nameGun.transform.localPosition = new Vector3(0, 1.1f, -0.5f);
		
		nameGun.SetActive(false);
		
		SetLayerForName(nameGun);

		gun.AddComponent<BoxCollider> ().size = new Vector3 (1, 5, 1);
		gun.GetComponent<BoxCollider> ().isTrigger = true;
	}
	
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

	IEnumerator WaitAndRepaint(Ball ball, float time)
	{
		//if(ball.InHands)
		//	Destroy(ball.GetComponent<Rigidbody>());

		//ball.GetComponent<Rigidbody>().Sleep();
		//ball.GetComponent<Rigidbody>().isKinematic = true;
		yield return new WaitForSeconds(time);
		//ball.GetComponent<Rigidbody>().Sleep();

		ball.Repaint();

		if(ball.InHands)
		{
			yield return new WaitForSeconds(time);
			//ball.gameObject.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
			ball.Take();
		}
		//ball.GetComponent<Rigidbody>().isKinematic = false;
		//ball.GetComponent<Rigidbody>().Sleep();
	}
	bool needAim = false;

	IEnumerator AwakeBall(Ball ball, float time)
	{
		yield return new WaitForSeconds(time);
		ball.GetComponent<Rigidbody>().isKinematic = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if( gunComponent.inHands )
		{
			if(!nameGun.activeSelf)
				nameGun.SetActive(true);

			RaycastHit hit;

			//#######################
//			if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
//			{
//				if(hit.transform.tag == "Ball" && !Player.HasBall)
//				{
//					Player.AimControl(true);
//					needAim = true;
//				}
//				else
//				{
//					
//					if(needAim)
//					{
//						needAim = false;
//						Player.AimControl(false);
//					}
//
//				}
//			}
			//#######################

			if(Game.IsInputActionButtonClickDown())
			{
				/*Vector3[] points = new Vector3[]{
					transform.localPosition,
					transform.localPosition - Vector3.forward*0.12f,
					transform.localPosition};
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, points, 0.45f);*/
				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(.4f, -0.3f, .38f), new Vector3(.4f, -0.3f, .5f), 0.23f);

				GetComponent<Animation>().AddClip(clip, "Recoil");
				GetComponent<Animation>().Play("Recoil");


				//RaycastHit hit;
				//if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
				if(Player.HasBall || (Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit) && hit.transform.tag == "Ball"))
				{
					//if(hit.transform.tag == "Ball")
					//{
					Ball ball;
					if(Player.HasBall)
						ball = Player.lastBall;
					else
						ball = hit.transform.GetComponent<Ball>();

						//ball.Repaint();
						//ball.Repaint();

						//ball.GetComponent<SphereCollider>().isTrigger = true;
						AnimationClip scaleDown = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one, Vector3.one*0.001f, 0.2f);
						AnimationClip scaleUp = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one*0.001f, Vector3.one, 0.2f);

						Animation anim = ball.GetComponent<Animation>();
						anim.AddClip(scaleDown, "ScaleDown");
						anim.AddClip(scaleUp, "ScaleUp");
						anim.Play("ScaleDown");
						ball.GetComponent<Rigidbody>().Sleep();
						anim.PlayQueued("ScaleUp");
						StartCoroutine(WaitAndRepaint(ball, 0.2f));
						//StartCoroutine(AwakeBall(ball, 2f));
						//hit.transform.GetComponent<Ball>().Repaint();

					//}
				}
			}
			
		}
	}
}
