using UnityEngine;
using System.Collections;

public class Switcher : MonoBehaviour 
{
	float switchTime = 0.4f;

	public bool IsActive
	{
		get
		{
			return isActive;
		}
	}

	bool isActive = true;
	Animation circleAnimation, rectAnimation;

	void Start()
	{
		gameObject.AddComponent<BoxCollider>();
		SetActive(false);
	}

	public void SetActive(bool act)
	{
		if(act != isActive)
			Switch();
	}

	public bool Switch()
	{
		if(circleAnimation.isPlaying)
			return isActive;

		if(isActive)
		{
			rectAnimation.Play("Unactive");
			circleAnimation.Play("Unactive");
			//isActive = false;
		}
		else
		{
			circleAnimation.Play("Active");
			rectAnimation.Play("Active");
			//isActive = true;
		}

		isActive = !isActive;
		return isActive;
	}

	void SetAnimations(GameObject circle, GameObject rect)
	{

		rectAnimation = rect.AddComponent<Animation>();
		AnimationClip clip = new AnimationClip();
		clip.legacy = true;
		AnimationCurve[] scale = new AnimationCurve[]{
			new AnimationCurve(new Keyframe(0, 1), new Keyframe(switchTime, 1)),
			new AnimationCurve(new Keyframe(0, 1), new Keyframe(switchTime, 1)),
			new AnimationCurve(new Keyframe(0, 0), new Keyframe(switchTime, 1))
		};
		
		clip.SetCurve("", typeof(Transform), "localScale.x", scale[0]);
		clip.SetCurve("", typeof(Transform), "localScale.y", scale[1]);
		clip.SetCurve("", typeof(Transform), "localScale.z", scale[2]);



		rectAnimation.playAutomatically = false;
		rectAnimation.AddClip(clip, "Unactive");
		
		
		AnimationClip clip_ = new AnimationClip();
		clip_.legacy = true;
		AnimationCurve[] scale_ = new AnimationCurve[]{
			new AnimationCurve(new Keyframe(0, 1), new Keyframe(switchTime, 1)),
			new AnimationCurve(new Keyframe(0, 1), new Keyframe(switchTime, 1)),
			new AnimationCurve(new Keyframe(0, 1), new Keyframe(switchTime, 0))
		};
		
		clip_.SetCurve("", typeof(Transform), "localScale.x", scale_[0]);
		clip_.SetCurve("", typeof(Transform), "localScale.y", scale_[1]);
		clip_.SetCurve("", typeof(Transform), "localScale.z", scale_[2]);



		rectAnimation.AddClip(clip_, "Active");








		circleAnimation = circle.AddComponent<Animation>();
		AnimationClip clip2 = new AnimationClip();
		clip2.legacy = true;
		AnimationCurve[] pos = new AnimationCurve[]{
			new AnimationCurve(new Keyframe(0, 0), new Keyframe(switchTime, 0)),
			new AnimationCurve(new Keyframe(0, 0.0015f), new Keyframe(switchTime, 0.0015f)),
			new AnimationCurve(new Keyframe(0, 0.15f), new Keyframe(switchTime, -0.15f))
		};
		
		clip2.SetCurve("", typeof(Transform), "localPosition.x", pos[0]);
		clip2.SetCurve("", typeof(Transform), "localPosition.y", pos[1]);
		clip2.SetCurve("", typeof(Transform), "localPosition.z", pos[2]);



		circleAnimation.playAutomatically = false;
		circleAnimation.AddClip(clip2, "Unactive");
		
		
		AnimationClip clip2_ = new AnimationClip();
		clip2_.legacy = true;
		AnimationCurve[] pos_ = new AnimationCurve[]{
			new AnimationCurve(new Keyframe(0, 0), new Keyframe(switchTime, 0)),
			new AnimationCurve(new Keyframe(0, 0.0015f), new Keyframe(switchTime, 0.0015f)),
			new AnimationCurve(new Keyframe(0, -0.15f), new Keyframe(switchTime, 0.15f))
		};
		
		clip2_.SetCurve("", typeof(Transform), "localPosition.x", pos_[0]);
		clip2_.SetCurve("", typeof(Transform), "localPosition.y", pos_[1]);
		clip2_.SetCurve("", typeof(Transform), "localPosition.z", pos_[2]);



		circleAnimation.AddClip(clip2_, "Active");



	}

	public void Init()
	{
		float radius = 0.15f;

		GameObject rightCircle, leftCircle, circle, circleBorder, topBorder, bottomBorder, rect;
		leftCircle = CustomObject.SemiircleBorder(radius, Obj.Colour.BLACK);
		rightCircle = CustomObject.Circle(radius, Obj.Colour.BLACK);

		circle = CustomObject.Circle(radius - Line.height, Obj.Colour.WHITE);
		circleBorder = CustomObject.CircleBorder(radius, Obj.Colour.BLACK, 0.01f);

		topBorder = GameObject.CreatePrimitive(PrimitiveType.Quad);
		bottomBorder = GameObject.CreatePrimitive(PrimitiveType.Quad);
		rect = GameObject.CreatePrimitive(PrimitiveType.Quad);


		rightCircle.transform.parent = 
			leftCircle.transform.parent = 
				circle.transform.parent = 
				circleBorder.transform.parent = 
				topBorder.transform.parent = 
				bottomBorder.transform.parent = 
				rect.transform.parent = transform;


		leftCircle.transform.localScale = new Vector3(1, 1, -1);
		topBorder.transform.localEulerAngles = bottomBorder.transform.localEulerAngles = rect.transform.localEulerAngles = Vector3.right*90f;
		topBorder.GetComponent<Renderer>().material.color = bottomBorder.GetComponent<Renderer>().material.color = rect.GetComponent<Renderer>().material.color = Color.black;
		topBorder.GetComponent<Collider>().enabled = false;
		bottomBorder.GetComponent<Collider>().enabled = false;



		rect.transform.localScale = new Vector3(radius*2f, radius*2f, 1);
		rect.GetComponent<Collider>().enabled = false;

		topBorder.transform.localScale = bottomBorder.transform.localScale = new Vector3(Line.height, radius*2f, 1);
		topBorder.transform.localPosition -= Vector3.right*(radius - Line.height/2f);
		bottomBorder.transform.localPosition += Vector3.right*(radius - Line.height/2f);


		leftCircle.transform.localPosition = -Vector3.forward*radius;
		rightCircle.transform.localPosition = Vector3.forward*radius;

		circle.transform.localPosition = Vector3.forward*radius + Vector3.up * 0.001f;
		circleBorder.transform.localPosition = Vector3.forward*radius + Vector3.up * 0.0015f;

		circle.transform.parent = circleBorder.transform;

		GameObject rectParent = new GameObject();
		rectParent.transform.parent = transform;
		rectParent.transform.localPosition = Vector3.forward*radius;
		rect.transform.parent = rectParent.transform;

		SetAnimations(circleBorder, rectParent);

		transform.localEulerAngles = -Vector3.forward*90f;
	}

}
