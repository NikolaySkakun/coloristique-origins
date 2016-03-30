using UnityEngine;
using System.Collections;
using System;

public class ExitButton : MonoBehaviour 
{
	bool scale = false;
	float scaleTime = 0.3f;
	int mousePointCount = 32;
	Vector3 originalSize = Vector3.zero;
	
	static float mouseSensitivityValue = 0f;
	
	GameObject mouseSensitivity;
	GameObject settings;
	GameObject[] mousePoint;
	Animation[] mousePointAnimation;
	
	Switcher[] gameModeSwitcher;
	GameObject gameModeSwitchersParent;
	
	public Level level;
	public Transform exitLine;
	public Vector3 exitLineOriginalSize = Vector3.zero;
	
	static GameObject about;
	public Settings gameSettings;

	void SetAnimationForExitButton()
	{
		gameObject.AddComponent<BoxCollider>().size = Vector3.one * 1.1f;
		gameObject.GetComponent<BoxCollider>().isTrigger = true;
		
		Animation anim = gameObject.GetComponent<Animation>();

		if(!anim)
			anim = gameObject.AddComponent<Animation>();
		
		anim.playAutomatically = false;
		
		//anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, transform.localScale*1.5f, scaleTime), "ScaleUp");
		//anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale*1.5f, transform.localScale, scaleTime), "ScaleDown");
	}

	void Start () 
	{
		SetAnimationForExitButton();
		originalSize = transform.localScale;
	}

	bool settingsClick = false, animMouseSensitivity = false;

	public bool exitVisible = false;

	IEnumerator HideExitLine()
	{
		yield return new WaitForSeconds(Game.drawTime * 1.5f);
		exitLine.localScale = new Vector3(exitLine.localScale.x, exitLineOriginalSize.y, exitLine.localScale.z);
	}

	public void Escape()
	{
		if(!exitVisible)
		{
			
			level.door[2].openDoorTrigger = level.room[1].trigger[1];
			level.door[2].room.gameObject.SetActive(true);
			
			foreach(Side s in level.door[2].room.side)
				foreach(Line l in s.line)
					l.Draw();
			level.door[2].SetColor(Obj.Colour.BLACK);
			level.door[2].Draw();
			exitVisible = true;
			
			StartCoroutine(HideExitLine());
		}
		else
		{
			exitLine.localScale = new Vector3(exitLine.localScale.x, 1.475f, exitLine.localScale.z);
			level.door[2].openDoorTrigger = null;
			level.door[2].room.gameObject.SetActive(false);
			level.door[2].SetColor(Obj.Colour.WHITE);
			level.door[2].Draw();
			exitVisible = false;
		}
	}

	bool needAim = false;

	void ScaleUp()
	{
		Animation anim = GetComponent<Animation>();
		
		if(anim.isPlaying)
			anim.Stop();
		
		if(anim.GetClip("ScaleUp"))
			anim.RemoveClip("ScaleUp");
		
		
		float full = originalSize.x * 1.5f - originalSize.x;
		float time = Mathf.Abs( originalSize.x * 1.5f - transform.localScale.x );
		
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, originalSize*1.5f, scaleTime * (time/full)), "ScaleUp");
		
		GetComponent<Animation>().Play("ScaleUp");
		Player.AimActive(true);
	}

	void ClickDown()
	{
		

		Animation anim = GetComponent<Animation>();
		
		if(anim.isPlaying)
			anim.Stop();
		
		if(anim.GetClip("ClickDown"))
			anim.RemoveClip("ClickDown");
		
		
		//float full = originalSize.x * 1.5f - originalSize.x;
		//float time = originalSize.x * 1.5f - transform.localScale.x;
		Vector3 size = new Vector3(originalSize.x*1.6f, transform.localScale.y, originalSize.z*1.6f);
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, size, scaleTime/4f), "ClickDown");
		
		GetComponent<Animation>().Play("ClickDown");

	}
	
	void ScaleDown()
	{
		Animation anim = GetComponent<Animation>();
		
		if(anim.isPlaying)
			anim.Stop();
		
		if(anim.GetClip("ScaleDown"))
			anim.RemoveClip("ScaleDown");
		
		float full = originalSize.x * 1.5f - originalSize.x;
		float time = full - (originalSize.x * 1.5f - transform.localScale.x);
		
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, transform.localScale, originalSize, scaleTime * (time/full)), "ScaleDown");
		
		GetComponent<Animation>().Play("ScaleDown");
		Player.AimActive(false);
	}

	void Update () 
	{
	
		if (GetComponent<Animation> ().IsPlaying ("Hide") || GetComponent<Animation> ().IsPlaying ("Show"))
			return;

		RaycastHit hit;
		if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
		{
			if(hit.transform == transform)
			{

				if(!scale && (!GetComponent<Animation>().IsPlaying("Hide") && !GetComponent<Animation>().IsPlaying("Show"))) // && !GetComponent<Animation>().isPlaying)
				{
					ScaleUp();
					//GetComponent<Animation>().Play("ScaleUp");
					scale = true;

					//needAim = true;
					//Player.AimControl(true);
				}

				if( Game.IsInputActionButtonClickDown())
				{
					ClickDown();

					if(!level.door[2].drawing && !level.door[2].open && level.door[2].CurrentState != Door.State.CLOSING)
					{
						if(!gameSettings.anim)
						{
							if(gameSettings.settingsClick)
							{
								gameSettings.VisibleAnimation();
								Invoke("Escape", Game.drawTime);
								return;
							}
							Escape();
						}
					}

				}
				else if(Game.IsInputActionButtonClickUp())
				{
					ScaleUp();
				}
			}
			else
			{
				if(scale && (!GetComponent<Animation>().IsPlaying("Hide") && !GetComponent<Animation>().IsPlaying("Show"))) // && !GetComponent<Animation>().isPlaying)
				{
					/*if(needAim)
					{
						needAim = false;
						Player.AimControl(false);
					}*/
					ScaleDown();
					//GetComponent<Animation>().Play("ScaleDown");
					scale = false;
				}

			}
			
		}
	}
}
