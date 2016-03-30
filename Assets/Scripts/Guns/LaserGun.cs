using UnityEngine;
using System.Collections;

public class LaserGun : MonoBehaviour 
{
	GameObject gun;
	Gun gunComponent;

	GameObject topPart;

	LaserRay[] ray;

	
	static float distance = 7f;
	
	void Awake () 
	{
		GameObject parent = new GameObject("Gun");
		GameObject downPart = CustomObject.CreatePrimitive(PrimitiveType.Cylinder);
		topPart = CustomObject.CreatePrimitive(PrimitiveType.Cylinder);
		
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
	}
	
	void Start()
	{
		ray = new LaserRay[10];

		for(int i=0; i<ray.Length; ++i)
			ray[i] = new LaserRay();


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
	
	// Update is called once per frame
	void Update () 
	{
		if( gunComponent.inHands )
		{
			if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
			{
				Vector3 startPoint = Player.camera.transform.position, direction = Player.camera.transform.forward;
				Vector3 rayOrigin = topPart.transform.position;
				RaycastHit hit;
				for(int i=0; i<ray.Length; ++i)
				{

					if(Physics.Raycast(startPoint, direction, out hit))
					{
						//if(hit.transform.tag == "Side")
						//{
							ray[i].Update(rayOrigin, direction, hit);
							//int index;
							/*if(hit.transform.GetComponent<Side>() != null)
								index = hit.transform.GetComponent<Side>().index;
							else
								index = hit.transform.parent.GetComponent<Side>().index;
							*/
							//Vector3 normal = Vector3.zero;
							//normal[index/2] = 1 * (index%2==0 ? 1 : -1);

							Vector3 normal = hit.normal;

							direction = Vector3.Reflect(direction, normal);
							startPoint = rayOrigin = hit.point;
						//}
					}
				}


				/*RaycastHit hit;
				if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
				{
					if(hit.transform.tag == "Side")
					{
						ray.Update(topPart.transform.position, Player.camera.transform.forward, hit);
						int index;
						if(hit.transform.GetComponent<Side>() != null)
							index = hit.transform.GetComponent<Side>().index;
						else
							index = hit.transform.parent.GetComponent<Side>().index;
						
						Vector3 normal = Vector3.zero;
						normal[index/2] = 1 * (index%2==0 ? 1 : -1);

						Vector3 reflect = Vector3.Reflect(Player.camera.transform.forward, normal);

						RaycastHit hit2;
						if(Physics.Raycast(hit.point, reflect, out hit2))
						{
							if(hit.transform.tag == "Side")
							{
								ray2.Update(hit.point, reflect, hit2);
							}
						}

						Debug.Log("SIDE");
					}
				}*/
			}
			
		}
	}
}
