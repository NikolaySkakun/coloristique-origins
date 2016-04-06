using UnityEngine;
using System.Collections;
using System.Xml;
using System;

//[RequireComponent (typeof (Rigidbody))]
[RequireComponent (typeof (Animation))]

public class Ball : Obj 
{
	static public Material blackBall, whiteBall;
	public enum BallType { NORMAL, SMALL }
	public BallType type = BallType.NORMAL;

	static public float timeForTake = 0.5f, distance = 2f;

	//public bool destruction = false;

	bool inHands = false;

	public bool InHands
	{
		get
		{
			return inHands;
		}
	}

	void Awake()
	{
		gameObject.AddComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
		gameObject.GetComponent<Rigidbody>().drag = 0.5f;
		//rigidbody.isKinematic = true;
	}

	override public void PostDrawing()
	{
		Debug.LogError ("post");
		GetComponent<Rigidbody>().isKinematic = false;
		//GetComponent<Rigidbody>().velocity = Vector3.zero;
	}

	bool tmp = false;

	void Start()
	{
		GetComponent<Rigidbody>().velocity = Vector3.zero;
	}


	public void Take()
	{
		//tmp = true;
		if(GetComponent<Animation>() && GetComponent<Animation>().IsPlaying("ConnectBall"))
			GetComponent<Animation>().Stop();

		GetComponent<Rigidbody>().velocity = velocity = Vector3.zero;
		GetComponent<Rigidbody>().useGravity = false;
		GetComponent<Rigidbody> ().isKinematic = false;
		GetComponent<Rigidbody>().Sleep();
		inHands = true;
		//GetComponent<Rigidbody>().isKinematic = true;
		Player.HasBall = true;

		transform.parent = Level.current.transform;
//		GetComponent<Rigidbody>().Sleep();
//		GetComponent<Rigidbody>().useGravity = false;
//		transform.parent = Player.camera.transform;
//
//		if(type == BallType.NORMAL)
//			transform.localScale = Vector3.one;
//		GetComponent<Rigidbody>().isKinematic = true;
//
//		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, transform.localPosition, Vector3.forward*distance, timeForTake);
//
//		GetComponent<Animation>().AddClip(clip, "anim");
//		GetComponent<Animation>().Play("anim");
//
//		inHands = true;
//		Player.HasBall = true;
//
//		StartCoroutine(ConnectBall(timeForTake));



		//GetComponent<Rigidbody>().isKinematic = true;

		//Player.camera.hingeJoint.connectedBody = rigidbody;
	}

	IEnumerator ConnectBall(float waitTime) 
	{
		yield return new WaitForSeconds(waitTime);

		if(inHands)
		{
			GetComponent<Rigidbody>().isKinematic = false;

//			Player.camera.GetComponent<HingeJoint>().connectedBody = GetComponent<Rigidbody>();
//			Player.camera.GetComponent<HingeJoint>().autoConfigureConnectedAnchor = true;

			Player.camera.GetComponent<FixedJoint>().connectedBody = GetComponent<Rigidbody>();
			Player.camera.GetComponent<FixedJoint>().autoConfigureConnectedAnchor = true;

		}
	}

	public void Throw()
	{
		//Debug.LogError("SADDDD");
		GetComponent<Rigidbody>().useGravity = true;
		inHands = false;
		GetComponent<Rigidbody>().isKinematic = false;
		Player.HasBall = false;
		//GetComponent<Rigidbody> ().velocity = Vector3.zero;
//		if(GetComponent<Animation>().isPlaying)
//		{
//			if(GetComponent<Animation>().IsPlaying("ScaleUp") || GetComponent<Animation>().IsPlaying("ScaleDown"))
//				return;
//			GetComponent<Animation>().Stop();
//		}
//		transform.parent = Level.current.transform;
//		GetComponent<Rigidbody>().isKinematic = false;
//		GetComponent<Rigidbody>().useGravity = true;
//		inHands = false;
//		Player.HasBall = false;
//		//Player.camera.GetComponent<HingeJoint>().connectedBody = null;
//		Player.camera.GetComponent<FixedJoint>().connectedBody = null;

		//Debug.LogWarning("ThrowBall");
	}

	static Ball()
	{
		//string shaderText = 


		whiteBall = new Material(Shader.Find("BallShader"));
		whiteBall.SetColor("_BorderColor", Color.black);
		whiteBall.SetColor("_Color", Color.white);
		
		
		
		blackBall = new Material(Shader.Find("BallShader"));
		blackBall.SetColor("_BorderColor", Color.white);
		blackBall.SetColor("_Color", Color.black);
			
	}


	static public Ball NonXmlCreate(Obj.Colour c, BallType t = BallType.NORMAL)
	{
		GameObject GO = CustomObject.CreatePrimitive(PrimitiveType.Sphere);
		//GO.AddComponent<Rigidbody>();
		//GO.AddComponent<BoxCollider>().isTrigger = true;
		//GO.GetComponent<BoxCollider>().size *= 1.5f;
		GO.tag = GO.name = "Ball";
		GO.layer = LayerMask.NameToLayer("Ball");
		Ball ball = GO.AddComponent<Ball>() as Ball;

		ball.type = t;

		if(ball.type == BallType.SMALL)
			ball.transform.localScale /= 2f;


		ball.color = c;
		//GO.renderer.material = GetMaterial(ball.color);
		GO.GetComponent<Renderer>().material.color = Game.GetColor(ball.color);

		return ball;
	}

	static public Ball Create(XmlNode xml)
	{
		GameObject GO = CustomObject.CreatePrimitive(PrimitiveType.Sphere);
		GO.tag = GO.name = "Ball";
		GO.layer = LayerMask.NameToLayer("Ball");
		Ball ball = GO.AddComponent<Ball>() as Ball;

		//GO.AddComponent<BoxCollider>().isTrigger = true;
		//GO.GetComponent<BoxCollider>().size *= 1.5f;
		//GO.AddComponent<Rigidbody>();
		//GO.

		if(xml.Attributes["type"] != null)
			ball.type = (BallType)Enum.Parse(typeof(BallType), xml.Attributes["type"].Value, true);

		if(ball.type == BallType.SMALL)
			ball.transform.localScale /= 2f;
		
		ball.color = Game.GetColor(xml.Attributes["color"].Value);
		//GO.renderer.material = GetMaterial(ball.color);
		GO.GetComponent<Renderer>().material.color = Game.GetColor(ball.color);
		Room room = Level.current.room[ int.Parse(xml.Attributes["room"].Value) ];
		Vector3 position = Vector3.zero;
		float x = float.Parse(xml.Attributes["x"].Value);
		float z = float.Parse(xml.Attributes["z"].Value);

		if(xml.Attributes["ledge"] != null)
		{
			position = room.ledge[ Game.GetInt(xml, "ledge") ].transform.position;
			position.y += ball.transform.localScale.x/2f;
			//position.y = room.ledge[ Game.GetInt(xml, "ledge") ].transform.position.y + 0.5f;

			GO.transform.position = position;

			ball.GetComponent<Rigidbody>().isKinematic = true;
		}
		else
		{
			position.x = ball.transform.localScale.x/2f - room.Size.x/2f + x*(room.Size.x - GO.transform.localScale.x)/100f;
			position.z = ball.transform.localScale.x/2f - room.Size.z/2f + z*(room.Size.z - GO.transform.localScale.z)/100f;
			position.y = -GO.transform.localScale.y/2f;
			
			GO.transform.position = room.side[2].transform.position - position;
		}


		
		return ball;
	}
	
	static public Material GetMaterial(Obj.Colour color)
	{
		if(color == Obj.Colour.WHITE)
			return whiteBall;
		else if(color == Obj.Colour.BLACK)
			return blackBall;
		else
		{
			Debug.LogError("Game -> GetMaterial()");
			return null;
		}
	}

	override public void Repaint()
	{
		base.Repaint();
		
		GetComponent<Renderer>().material = GetMaterial(color);
	}

	GameObject drawingBorder;
	Mesh drawingBorderMesh;
	
	override public void Draw()
	{
		Game.DrawEvent -= Draw;

	

		if(color == Colour.BLACK)
		{
			gameObject.GetComponent<Animation>().AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.zero, Vector3.one, Game.drawTime), "Draw");
			gameObject.GetComponent<Animation>().Play("Draw");
			GetComponent<Renderer>().material = GetMaterial(color);
		}
		else
		{
			drawingBorder = CustomObject.CreateObject("SemicircleBorder", Game.ReverseColor(color));
			drawingBorderMesh = drawingBorder.GetComponent<MeshFilter>().mesh = CustomMesh.CircleBorder();
			
			drawingBorder.transform.position = transform.position;
			drawingBorder.transform.parent = transform;
			drawingBorder.transform.localEulerAngles += Vector3.right * 90f;
		}

		/*AnimationClip clip = new AnimationClip();
		AnimationCurve[] scale = new AnimationCurve[3];

		for(int u=0; u<scale.Length; ++u)
			scale[u] = new AnimationCurve(
				new Keyframe(0, 0),
				new Keyframe(Game.drawTime/2f, 0),
				new Keyframe(Game.drawTime, 1));

		clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
		clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
		clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);

		animation.AddClip(clip, "Draw");
		animation.Play("Draw");*/

	}

	public override void Destroy ()
	{
		float destroyTime = 0.5f;

		if(this != null)
		{
			if(levelIndex != Level.current.Index)
			{
				//destruction = true;
				Game.DrawEvent -= Draw;
				Game.DestroyEvent -= Destroy;

				AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, Vector3.zero, destroyTime);

				GetComponent<Animation>().AddClip(clip, "Destroy");
				GetComponent<Animation>().Play("Destroy");

				UnityEngine.Object.Destroy(gameObject, destroyTime);
			}
		}
	}
	
	static public void Init(XmlNode node)
	{
		Debug.Log(node.Name);
			
	}

	float drawTimer = 0f;

	void Update()
	{
		if( color == Colour.WHITE && drawTimer <= 1 )
		{
			drawTimer += Time.deltaTime*(1f/(Game.drawTime));
			transform.LookAt(Player.camera.transform);

			//if(drawTimer > 1)
			//	return;

			if(drawingBorderMesh == null)
				return;

			float radius = transform.localScale.x/2f + 0.01f;
			Vector3[] verts = drawingBorderMesh.vertices;
			int vertsCount = 50;
			
			for(int i=0; i<verts.Length; ++i)
			{
				float cos = Mathf.Cos( Mathf.Deg2Rad*(((180f/(vertsCount-1))*drawTimer) * (i%2==0 ? i : i-1)) );
				float sin = Mathf.Sin( Mathf.Deg2Rad*(((180f/(vertsCount-1))*drawTimer) * (i%2==0 ? i : i-1)) );
				
				float tempRadius = i%2==0 ? radius - 0.02f : radius;
				
				verts[i].y = 0.0001f;//i%2==0 ? -0.5f : 0.5f;
				verts[i].x = cos * tempRadius;
				verts[i].z = sin * tempRadius;
			}
			
			drawingBorderMesh.vertices = verts;

			if(drawTimer >= 1)
			{
				//drawTimer = 1;
				GetComponent<Renderer>().material = GetMaterial(color);
				UnityEngine.Object.Destroy(drawingBorder);
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}


		if(inHands)
		{
			//if(!GetComponent<Renderer>().isVisible)
			//	Throw();
			//if(transform.localPosition.z < distance - 0.5f || transform.localPosition.z > distance + 0.5f)
			//	Throw();

			if(Player.camera.transform.InverseTransformPoint(transform.position).z > distance + 1.5f)
			{
				Throw();
				GetComponent<Rigidbody>().velocity = Vector3.zero;
			}
		}
	}

	void OnBecameInvisible()
	{
		if(InHands)
		{
			Throw();
			GetComponent<Rigidbody>().velocity = Vector3.zero;
		}
	}

	private Vector3 velocity = Vector3.zero;

	void BallUpdate()
	{

		Vector3 point = Player.camera.transform.TransformPoint(0, 0, distance);
		GetComponent<Rigidbody>().MovePosition(Vector3.SmoothDamp(transform.position, point, ref velocity, 0.1f));
		GetComponent<Rigidbody>().velocity = velocity/2f;
	}

	
	void FixedUpdate () 
	{
		if(inHands && GetComponent<Rigidbody>())
		{
			//Vector3.forward * distance
			Vector3 point = Player.camera.transform.TransformPoint(0, 0, distance);//Vector3.Lerp(transform.position, Player.camera.transform.TransformPoint(Vector3.forward * distance), Time.fixedDeltaTime * 30f);
			//Vector3 point = Player.camera.transform.TransformPoint(Player.camera.transform.TransformDirection(Vector3.forward) * distance);
			//Vector3 point = Player.camera.transform.TransformDirection(Vector3.forward) * distance;
			//transform.position = Vector3.SmoothDamp(transform.position, point, ref velocity, 0.1f); //point;
			//GetComponent<Rigidbody>().velocity = Vector3.zero;
			GetComponent<Rigidbody>().MovePosition(Vector3.SmoothDamp(transform.position, point, ref velocity, 0.075f));
			GetComponent<Rigidbody>().velocity = velocity/2f;


		}

		//if(!InHands && Vector3.Distance(transform.position, Player.player.transform.position) < 1f)
		//	Player.controller.stepOffset = 0.1f;

		//GetComponent<Rigidbody>().AddForce(0, 15, 0);
	}
	/*void OnTriggerEnter(Collider col)
	{
		if(!inHands && col.gameObject.tag == "Player")
		{
			Player.controller.stepOffset = 0.1f;
		}
	}


	void OnTriggerExit(Collider col)
	{
		if(col.gameObject.tag == "Player")
		{
			Player.controller.stepOffset = 0.3f;
		}
	}*/



	public void SetType(BallType t)
	{
		if(type == t)
			return;

		type = t;

		if(t == BallType.NORMAL)
		{
			Animation anim;
			if(GetComponent<Animation>() == null)
				anim = gameObject.AddComponent<Animation>() as Animation;
			else
				anim = GetComponent<Animation>();

			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, Vector3.one, (1 - transform.localScale.x)*2);

			anim.AddClip(clip, "ScaleUp");
			anim.Play("ScaleUp");
			GetComponent<Rigidbody>().WakeUp();
		}
		else
		{
			Animation anim;
			if(GetComponent<Animation>() == null)
				anim = gameObject.AddComponent<Animation>() as Animation;
			else
				anim = GetComponent<Animation>();

			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, Vector3.one*0.5f, (transform.localScale.x - 0.5f)*2);

			anim.AddClip(clip, "ScaleDown");
			anim.Play("ScaleDown");
		}
	}
	
	public void t()
	{
		transform.localScale *= 5;	
	}

}

// 287
