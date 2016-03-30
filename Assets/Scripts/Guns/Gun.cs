using UnityEngine;
using System.Xml;
using System;
using System.Collections;

[RequireComponent(typeof(Animation))]
public class Gun : Obj 
{
	public enum GunType { GRAVITY, LASER, COLOR, SCALE };
	public GunType type;

	public Trigger trigger;
	public GameObject gun;

	static float animationTime = 1f;

	public bool inHands = false;

	static public Gun Create(XmlNode xml)
	{
		string type = xml.Attributes["type"].Value;

		Gun gun = Obj.Create<Gun>();
		gun.gameObject.layer = LayerMask.NameToLayer("Gun");
		gun.type = (GunType)Enum.Parse(typeof(GunType), type, true);

		gun.trigger = Trigger.NonXmlCreate(new Vector3(1, 0.5f, 1), Trigger.TriggerType.PLAYER);
		gun.trigger.transform.parent = gun.transform;
		gun.gameObject.AddComponent(System.Type.GetType(type + "Gun"));
		//UnityEngineInternal.APIUpdaterRuntimeServices.AddComponent(gun.gameObject, "Assets/Scripts/Gun.cs (30,3)", type + "Gun");
		//gun.gun.layer = LayerMask.NameToLayer("Gun");

		Join(xml, gun);

		//Debug.Log("GUUUUUUN");

		return gun;
	}


	public static void Join(XmlNode xml, Gun gun)
	{
		float x = Game.GetFloat(xml, "x");
		float z = Game.GetFloat(xml, "z");
		
		Room room = Level.current.room[Game.GetInt(xml, "roomIndex")];
		
		Vector3 position = Vector3.zero;
		
		position.x = 0.5f - room.Size.x/2f + x*(room.Size.x - 1)/100f;
		position.z = 0.5f - room.Size.z/2f + z*(room.Size.z - 1)/100f;
		
		gun.transform.position = room.side[2].transform.position - position + Vector3.up/2f;
	}

	void Start()
	{
		GetComponent<Animation>().playAutomatically = false;


		//animation.Play("destroy");
	}

	void Update()
	{
		if(trigger != null && trigger.PlayerStay)
		{
			//gun.animation.Stop();
			UnityEngine.Object.Destroy( gun.GetComponent<Animation>() );
			//Game.DestroyEvent -= trigger.Destroy;
			//UnityEngine.Object.Destroy( trigger.gameObject );
			trigger.DestroyAnyway();

			gun.transform.localPosition = Vector3.zero;
			gun.transform.localEulerAngles = Vector3.zero;

			transform.parent = Player.camera.transform;

			transform.localEulerAngles = Vector3.right * 90f;
			transform.localPosition = new Vector3(.4f, -0.3f, .5f);


			gun.transform.localEulerAngles = Vector3.up * 45f;




			inHands = true;
			Player.gunCamera.SetActive(true);

			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.z), transform.localPosition, animationTime);

			GetComponent<Animation>().AddClip(clip, "Take");
			GetComponent<Animation>().Play("Take");
		}
	}

	public override void Destroy ()
	{
		if(this != null)
			if(levelIndex != Level.current.Index)
			//if( (transform.root.GetComponent<Level>() as Level) != Level.current && transform.root.tag != "Player") // !!
		{
		//Debug.Log("aaasasasasas");
		Game.DrawEvent -= Draw;
		Game.DestroyEvent -= Destroy;



	//	AnimationCurve curveX = new AnimationCurve(new Keyframe(0, transform.localPosition.x), new Keyframe(animationTime, transform.localPosition.x));
		//AnimationCurve curveY = new AnimationCurve(new Keyframe(0, transform.localPosition.y), new Keyframe(animationTime, transform.localPosition.y));
		//AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, transform.localPosition.z), new Keyframe(animationTime, -transform.localPosition.z));
		
			AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, transform.localPosition, new Vector3(transform.localPosition.x, transform.localPosition.y, -transform.localPosition.z), animationTime);
		//clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
		//clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
		//clip.SetCurve("", typeof(Transform), "localPosition.z", curveZ);
		
			//clip.legacy = true;

		GetComponent<Animation>().AddClip(clip, "destroy");
		GetComponent<Animation>().Play("destroy");

		Player.gunCamera.SetActive(false);
		UnityEngine.Object.Destroy(gameObject, animationTime);
		}
	}
}
