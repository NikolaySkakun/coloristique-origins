using UnityEngine;
using System.Collections;
using System;

public class Settings : MonoBehaviour 
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

	MouseLook xSens, ySens;

	static GameObject about;
	public Level level;
	public ExitButton exit; 

	static public void SetAbout(GameObject obj)
	{
		about = obj;


		about.AddComponent<Animation>().AddClip(Game.CreateAnimationClip(
			Game.AnimationClipType.POSITION, 
			about.transform.position + Vector3.right*8f,
			about.transform.position,
			1.2f), "Enable");



		about.GetComponent<Animation>().AddClip(Game.CreateAnimationClip(
			Game.AnimationClipType.POSITION, 
			about.transform.position,
			about.transform.position + Vector3.right*8f,
			1.2f), "Disable");


		about.transform.position += Vector3.right*8f;
		about.SetActive(false);
	}

	// Use this for initialization

	void SetAnimationForPoint(Animation anim, float size)
	{
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.zero, Vector3.one*size, scaleTime), "ScaleUp");
		anim.AddClip(Game.CreateAnimationClip(Game.AnimationClipType.SCALE, Vector3.one*size, Vector3.zero, scaleTime), "ScaleDown");
	}

	void SetAnimationForSettingsButton()
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

	void CreateMouseSensitivityOption()
	{
		mouseSensitivity = Word.WriteString("mouse sensitivity", 0.1f);
		mouseSensitivity.transform.localEulerAngles = Vector3.forward * 90;
		mouseSensitivity.transform.parent =settings.transform;
		mouseSensitivity.transform.localPosition = Vector3.right*2f + Vector3.up*2.5f;


		mousePoint = new GameObject[mousePointCount];
		mousePointAnimation = new Animation[mousePoint.Length];

		for(int i=0; i<mousePoint.Length; ++i)
		{
			float radius = 0.15f;

			mousePoint[i] = CustomObject.CircleBorder(radius, Obj.Colour.BLACK);
			Game.SetRenderQueue (mousePoint [i], 2);
			mousePoint[i].transform.localEulerAngles = Vector3.forward * 90;

			mousePoint[i].transform.parent = mouseSensitivity.transform;
			//mousePoint[i].transform.localPosition = new Vector3(-0.4f, 0, -0.3549f*i - radius);

			mousePoint[i].transform.localPosition = new Vector3(-Mathf.Sin(i*(2f*Mathf.PI/(float)(mousePoint.Length))) * 2, 0, Mathf.Cos(i*(2f*Mathf.PI/(float)(mousePoint.Length))) * 2 - 2 + radius*1.7f);

			mousePoint[i].AddComponent<BoxCollider>();

			GameObject point = CustomObject.Circle(radius, Obj.Colour.BLACK);
			Game.SetRenderQueue (point, 2);
			point.transform.parent = mousePoint[i].transform;
			point.transform.localPosition = Vector3.zero;
			point.transform.localEulerAngles = Vector3.zero;
			point.transform.localScale = Vector3.zero;

			mousePointAnimation[i] = point.AddComponent<Animation>();
			SetAnimationForPoint(mousePointAnimation[i], radius*2f);
		}
	}

	void CreateGameModeSwitchers()
	{
		gameModeSwitcher = new Switcher[3];

		gameModeSwitchersParent = new GameObject("Switchers");
		gameModeSwitchersParent.transform.position = transform.position + Vector3.up*1.5f - Vector3.forward*8f + Vector3.right*0.0015f;
		gameModeSwitchersParent.transform.parent = Level.current.transform;

		for(int i=0; i<gameModeSwitcher.Length; ++i)
		{
			gameModeSwitcher[i] = CustomObject.CreateSwitcher();

			gameModeSwitcher[i].transform.position = gameModeSwitchersParent.transform.position - Vector3.up*i;
			gameModeSwitcher[i].transform.SetParent(gameModeSwitchersParent.transform);


			gameModeSwitcher[i].transform.localEulerAngles = new Vector3(0, 180f, -90f);
			/*white.transform.position = black.transform.position = transform.position;
			white.transform.eulerAngles = black.transform.eulerAngles = Vector3.forward * 90f;
			
			white.transform.parent = black.transform.parent = Level.current.transform;
			white.transform.localPosition -= Vector3.right*0.0005f;
			black.transform.localPosition -= Vector3.right*0.0005f;*/
		}



	}

	float ringScale = 0.6f;
	float speed = 18f, minScale = 0.6f, maxScale = 25f;
	public bool anim = false;

	void LateUpdate()
	{

		if(settingsClick && ringScale < maxScale)
		{
			ringScale += Time.deltaTime*speed;

			float radius = 0.5f;


			blackRing.mesh = CustomMesh.CircleBorder(ringScale*radius, 0.02f, 100);
			whiteRing.mesh = CustomMesh.CircleBorder(25f, 25f - ringScale*radius, 100);

			Game.SetRenderQueue (blackRing.gameObject, 1);
			Game.SetRenderQueue (whiteRing.gameObject, 1);
			//whiteRing.GetComponent<Renderer>().material.renderQueue = Shader.Find("Base").renderQueue - 1;

			anim = true;
		}
		else if(!settingsClick && ringScale < maxScale)
		{
			ringScale += Time.deltaTime*speed;

			float radius = 0.5f;


			blackRing.mesh = CustomMesh.CircleBorder(ringScale*radius, 0.02f, 100);
			whiteRing.mesh = CustomMesh.CircleBorder(ringScale*radius - 0.01f, ringScale*radius - 0.01f - minScale/2f, 100);
			Game.SetRenderQueue (blackRing.gameObject, 1);
			Game.SetRenderQueue (whiteRing.gameObject, 1);
			//whiteRing.GetComponent<Renderer>().material.renderQueue = Shader.Find("Base").renderQueue - 1;

			if(ringScale >= maxScale)
			{
				SetActiveSettings(false);
			}
			anim = true;
		}
		else
		{
			if(black.activeSelf)
			{
				blackRing.mesh = CustomMesh.CircleBorder(minScale/2f, 0.02f, 100);
				black.SetActive(false);

			}
			anim = false;
			//ringScale = minScale;
		}

		if(!Player.InZeroRoom && Level.current.Index != 0)
		{
			if(white.activeSelf)
				white.SetActive(false);
		}
		else if(!white.activeSelf && !neutralMode)
		{
			white.SetActive(true);
		}
	}

	bool neutralMode = true;

	void CreateRings()
	{
		float radius = 0.3f, thick = 0.01f;


		white = CustomObject.CircleBorder(maxScale, Obj.Colour.WHITE, maxScale - minScale/2f, 30);
		black = CustomObject.CircleBorder(minScale/2f, Obj.Colour.BLACK, thick, 30);



		white.transform.position = black.transform.position = transform.position;
		white.transform.eulerAngles = black.transform.eulerAngles = Vector3.forward * 90f;

		white.transform.parent = black.transform.parent = Level.current.transform;
		white.transform.localPosition -= Vector3.right*0.005f;
		black.transform.localPosition -= Vector3.right*0.005f;

		whiteRing = white.GetComponent<MeshFilter>();
		blackRing = black.GetComponent<MeshFilter>();

		whiteRing.GetComponent<Renderer>().material.shader = Shader.Find("BaseBack");
		//whiteRing.GetComponent<Renderer>().material.color = Color.white;
		//whiteRing.GetComponent<Renderer>().material.renderQueue = Shader.Find("Base").renderQueue + 1;

		black.SetActive(false);
	}
	
	MeshFilter whiteRing, blackRing;
	GameObject white, black;
	float originalZ;

	public void SetActiveSettings(bool active)
	{
		if(active)
		{
			settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, 0, settings.transform.localPosition.z);
			//settings.SetActive(true);
			whiteRing.gameObject.SetActive(true);
		}
		else
		{
			neutralMode = true;
			settings.transform.localPosition = new Vector3(settings.transform.localPosition.x, level.room[1].Size.y, settings.transform.localPosition.z);
			//settings.SetActive(false);
			whiteRing.gameObject.SetActive(false);

			foreach(Line l in level.room[1].side[1].line)
			{
				l.transform.localPosition = new Vector3(
					l.transform.localPosition.x, 
					l.transform.localPosition.y, 
					originalZ - 0.001f);// -= Vector3.forward * 0.003f;
			}
		}
	}

	void Start () 
	{
		SetAnimationForSettingsButton();

		settings = new GameObject("Settings");
		Room room = Level.current.room[1];
		settings.transform.position = room.side[1].transform.position - Vector3.up*room.Size.y/2f + Vector3.forward*room.Size.z/2f - Vector3.right*0.001f;
		settings.transform.localEulerAngles = Vector3.up*90f;
		settings.transform.parent = Level.current.transform;

		if(Player.player.GetComponent<MouseLook>() != null && Player.camera.GetComponent<MouseLook>() != null)
		{
			xSens = Player.player.GetComponent<MouseLook>();
			ySens = Player.camera.GetComponent<MouseLook>();
		}

		CreateMouseSensitivityOption();
		StartCoroutine( SetMouseSensitivity(11) );


		CreateRings();

		neutralMode = true;
		SetActiveSettings(false);
		//CreateGameModeSwitchers();

		//CustomObject.CreateSwitcher();

		originalZ = level.room[1].side[1].line[0].transform.localPosition.z;

		originalSize = transform.localScale;
		//anim.Play("Scale");
	}

	int GetCurrentMouseSensitivityPoint()
	{
		return (int)(mouseSensitivityValue / 2f) - 1;
	}

	IEnumerator SetMouseSensitivity(int i)
	{
		animMouseSensitivity = true;

		if(i > GetCurrentMouseSensitivityPoint())
		{
			for(int u=GetCurrentMouseSensitivityPoint()+1; u<=i; ++u)
			{
				mousePointAnimation[u].Play("ScaleUp");
				yield return new WaitForSeconds(0.03f);
			}
		}
		else
		{
			for(int u=GetCurrentMouseSensitivityPoint(); u>i; --u)
			{
				mousePointAnimation[u].Play("ScaleDown");
				yield return new WaitForSeconds(0.03f);
			}
		}

		mouseSensitivityValue = (i+1)*2;

		/*if(mouseSensitivityValue < (mousePointCount/2)*2)
			xSens.sensitivityX = ySens.sensitivityY = 1f - 1f/(float)mouseSensitivityValue;
		else


			xSens.sensitivityX = ySens.sensitivityY = (mouseSensitivityValue - (mousePointCount - 1)) * 0.4f;*/
		//ySens.sensitivityY =1;
		//xSens.sensitivityX = ySens.sensitivityY = mouseSensitivityValue
		if(xSens != null)
			MouseLook.sensitivity = xSens.sensitivityX = ySens.sensitivityY = mouseSensitivityValue/10f;
		animMouseSensitivity = false;
	}
	
	public bool settingsClick = false;
	bool animMouseSensitivity = false, needAim = false;

	public void VisibleAnimation()
	{
		if (!Level_0.hideInfo)
		{
			Level_0.hideInfo = true;
			Level_0 lvl = GameObject.FindObjectOfType<Level_0> ();

			lvl.infoMouse.GetComponent<Animation> ().Play ("Destroy");
			lvl.infoWalk.GetComponent<Animation> ().Play ("Destroy");
		}

		settingsClick = !settingsClick;
		neutralMode = false;
		SetActiveSettings(true);
		//level.room[1].side[1].transform.localPosition += Vector3.right * 0.003f;
		foreach(Line l in level.room[1].side[1].gameObject.GetComponentsInChildren<Line>())
		{
			l.transform.localPosition = new Vector3(
				l.transform.localPosition.x, 
				l.transform.localPosition.y, 
				originalZ - 0.001f);// -= Vector3.forward * 0.003f;
		}
		
		black.SetActive(true);
		about.SetActive(true);
		about.GetComponent<Animation>().Play(settingsClick ? "Enable" : "Disable");

		if(!settingsClick)
			Invoke("DisableAbout", 1.2f);

		if(ringScale >= maxScale)
			ringScale = 0.6f;
	}

	void DisableAbout()
	{
		about.SetActive(false);
	}

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

	void Update () 
	{

		if (GetComponent<Animation> ().IsPlaying ("Hide") || GetComponent<Animation> ().IsPlaying ("Show"))
			return;

		RaycastHit hit;
		if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
		{
			if(hit.transform == transform)
			{
				if( Game.IsInputActionButtonClickDown())
				{
					ClickDown();

					if(!black.activeSelf)
					{
						if(level.door[2].CurrentState != Door.State.DRAWING && !level.door[2].open)
						{
							if(exit.exitVisible)
							{
								exit.Escape();
								Invoke("VisibleAnimation", Game.drawTime*1.1f);
								return;
							}

							VisibleAnimation();
					}
					}
				}
				else if(Game.IsInputActionButtonClickUp())
				{
					
					ScaleUp();
				}
				if(!scale && (!GetComponent<Animation>().IsPlaying("Hide") && !GetComponent<Animation>().IsPlaying("Show"))) // && !GetComponent<Animation>().isPlaying)
				{
					
					ScaleUp();
					//GetComponent<Animation>().Play("ScaleUp");
					scale = true;

					//needAim = true;
					//Player.AimControl(true);
				}
			}
			else
			{
				if(scale && (!GetComponent<Animation>().IsPlaying("Hide") && !GetComponent<Animation>().IsPlaying("Show"))) // && !GetComponent<Animation>().isPlaying)
				{
					ScaleDown();
					//GetComponent<Animation>().Play("ScaleDown");
					scale = false;

					/*if(needAim)
					{
						needAim = false;
						Player.AimControl(false);
					}*/
				}

				if( Game.IsInputActionButtonClick() && settingsClick)
				{
					for(int i=0; i<mousePoint.Length; ++i)
					{
						if(!mousePointAnimation[i].isPlaying && hit.transform == mousePoint[i].transform && !animMouseSensitivity)
						{
							//Debug.LogWarning(i);
							StartCoroutine( SetMouseSensitivity(i) );
						}
					}


					if(gameModeSwitcher != null)
						for(int i=0; i<gameModeSwitcher.Length; ++i)
						{
							if((hit.transform != null) && hit.transform == gameModeSwitcher[i].transform)
							{
								int index = i;
								//Game.gameMode = (Game.GameMode)index;//(Game.GameMode)Enum.ToObject(typeof(Game.GameMode), index);
								gameModeSwitcher[i].SetActive(true);
								for(int u=0; u<gameModeSwitcher.Length; ++u)
								{
									if(u == index)
										continue;
									else
										gameModeSwitcher[u].SetActive(false);
								}
								break;
							}
						}

				}
			}

		}
	}
}
