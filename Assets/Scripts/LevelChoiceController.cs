using UnityEngine;
using System.Collections;

public class LevelChoiceController : MonoBehaviour 
{
	public Level level;
	//GameObject test, test2;
	//GameObject number, number2;
	//GameObject n, n2;
	private int progress = 1;

	GameObject[] buttons = new GameObject[Game.levelsCount];

	void Start () 
	{
		progress = Game.Progress;
		CreateLevelButtons();
		Digit.thick = 0f;
	}

	void CreateLevelButtons()
	{
		float radius = 0.5f;



		for(int i=0; i<Game.levelsCount; ++i)
		{
			if(buttons[i] != null)
				Destroy(buttons[i]);

			buttons[i] = CustomObject.CircleBorder(radius, Obj.Colour.BLACK, 0.03f);
			buttons[i].transform.parent = transform;
			buttons[i].transform.localEulerAngles = Vector3.right * 270f;
			buttons[i].transform.localPosition = new Vector3(1f + 1.6f*(float)i, 4, 0);
			buttons[i].AddComponent<BoxCollider>().isTrigger = true;
			buttons[i].AddComponent<Animation>();

			GameObject whiteCircle = CustomObject.Circle(radius - 0.03f, Obj.Colour.WHITE);
			whiteCircle.transform.parent = buttons[i].transform;
			whiteCircle.transform.localEulerAngles = Vector3.zero;
			whiteCircle.transform.localPosition = Vector3.zero;

			Digit.digitColor = Obj.Colour.BLACK;
			Digit.thick = (i+1) > progress ? 0.15f : 0.08f;
			GameObject n = Digit.GetDigit(i + 1);//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
			n.transform.parent = buttons[i].transform;
			n.transform.localEulerAngles = new Vector3(0, 90, 180);
			n.transform.localScale = Vector3.one * 0.18f;
			n.transform.localPosition = Vector3.zero + Vector3.up * 0.001f + (i==0 ? -0.1f : 0)*Vector3.right;

			if(i+1 > progress)
			{
				Digit.thick = 0.08f;
				Digit.digitColor = Obj.Colour.WHITE;
				GameObject n2 = Digit.GetDigit(i + 1);//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
				n2.transform.parent = buttons[i].transform;
				n2.transform.localEulerAngles = new Vector3(0, 90, 180);
				n2.transform.localScale = Vector3.one * 0.18f;
				n2.transform.localPosition = Vector3.zero + Vector3.up * 0.002f + (i==0 ? -0.1f : 0)*Vector3.right;
			}
		}

		Digit.thick = 0.0f;
	}



	void DrawButton(int index)
	{
		for(int i=0; i<buttons[index].transform.childCount; ++i)
		{
			if(buttons[index].transform.GetChild(i).tag == "Digit")
			{
				Object.Destroy(buttons[index].transform.GetChild(i).gameObject);
				break;
			}
		}

		Digit.digitColor = Obj.Colour.BLACK;
		Digit.thick = (index+1) > progress ? 0.15f : 0.08f;
		GameObject n = Digit.Shift(index+1);//Digit.GetDigit(index + 1);//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
		n.transform.parent = buttons[index].transform;
		n.transform.localEulerAngles = new Vector3(0, 90, 180);
		n.transform.localScale = Vector3.one * 0.18f;
		n.transform.localPosition = Vector3.zero + Vector3.up * 0.001f + (index==0 ? -0.1f : 0)*Vector3.right;
		
		if(index+1 > progress)
		{
			Digit.thick = 0.08f;
			Digit.digitColor = Obj.Colour.WHITE;
			GameObject n2 = Digit.Shift(index+1);//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
			n2.transform.parent = buttons[index].transform;
			n2.transform.localEulerAngles = new Vector3(0, 90, 180);
			n2.transform.localScale = Vector3.one * 0.18f;
			n2.transform.localPosition = Vector3.zero + Vector3.up * 0.002f + (index==0 ? -0.1f : 0)*Vector3.right;
		}

	}

	bool mouseButtonPressed = false;

	void LateUpdate()
	{
		if(progress != Game.Progress)
		{
			progress = Game.Progress;
			CreateLevelButtons();
		}
	}

	bool needAim = false, activeAim = false;

	void Update () 
	{
		if(Player.InZeroRoom)
		{
			RaycastHit hit;
			if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
			{
				needAim = false;

				for(int i=0; i<buttons.Length; ++i)
				{
					if(!buttons[i].GetComponent<Animation>().isPlaying && !mouseButtonPressed)
					{
						float distance = Vector3.Distance(hit.point, buttons[i].transform.position);
						float maxSize = 1f, minSize = 0.7f, maxDistance = 3.5f;;
						if(distance < maxDistance)
						{
							//buttons[i].SetActive(true);
							//DrawButton(i);
							if(distance <= maxSize)
								buttons[i].transform.localScale = Vector3.one;
							else
								buttons[i].transform.localScale = Vector3.one * (minSize + ((maxSize - minSize)/(maxDistance - maxSize))*(maxDistance - distance));
						}
						else
						{
							buttons[i].transform.localScale = Vector3.one * minSize;
							//buttons[i].SetActive(false);
						}
					}
					if(i<progress && hit.transform == buttons[i].transform )
					{
						/*if(!needAim)
						{
							Player.AimActive(true);
							needAim = true;
						}*/

						needAim = true;
						activeAim = true;

						if( Game.IsInputActionButtonClickDown() )//Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)|| Input.GetKeyDown(KeyCode.JoystickButton14) )
						{
							level.GetComponent<Level_0>().binary = i + 1;

							if(buttons[i].GetComponent<Animation>().GetClip("ScaleUp") != null)
								buttons[i].GetComponent<Animation>().RemoveClip("ScaleUp");

							AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, buttons[i].transform.localScale, Vector3.one*1.1f, 0.08f);
							buttons[i].GetComponent<Animation>().AddClip(clip, "ScaleUp");
							buttons[i].GetComponent<Animation>().Play("ScaleUp");
							mouseButtonPressed = true;

							return;
						}
						else if( Game.IsInputActionButtonClickUp() ) //Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)|| Input.GetKeyUp(KeyCode.JoystickButton14) )
						{
							if(buttons[i].GetComponent<Animation>().GetClip("ScaleDown") != null)
								buttons[i].GetComponent<Animation>().RemoveClip("ScaleDown");

							AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, buttons[i].transform.localScale, Vector3.one, 0.08f);
							buttons[i].GetComponent<Animation>().AddClip(clip, "ScaleDown");
							buttons[i].GetComponent<Animation>().PlayQueued("ScaleDown");

							mouseButtonPressed = false;
						}
					}
					/*else
					{
						if(needAim)
						{
							Player.AimActive(false);
							needAim = false;
						}
						//if(Player.scaleSmallAim)
						//	
					}*/
				}

				if(needAim && !Player.scaleSmallAim)
				{
					Player.AimActive(true);
				}
				else if(!needAim && Player.scaleSmallAim && activeAim)
				{
					Player.AimActive(false);
					activeAim = false;
				}
			}



			if(Input.GetKeyUp(KeyCode.L))
			{
				/*if(number != null)
					Object.Destroy(number);
				
				if(number2 != null)
					Object.Destroy(number2);
				
				Digit.thick = 0.08f;
				
				number = Digit._2();//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
				number.transform.parent = test.transform;
				number.transform.localEulerAngles = new Vector3(0, 90, 180);
				number.transform.localScale = Vector3.one * 0.18f;
				number.transform.localPosition = Vector3.zero + Vector3.up * 0.001f;
				
				/*Digit.thick = 0.08f;
				Digit.digitColor = Obj.Colour.WHITE;
				number2 = Digit._8();//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
				number2.transform.parent = test.transform;
				number2.transform.localEulerAngles = new Vector3(0, 90, 180);
				number2.transform.localScale = Vector3.one * 0.18f;
				number2.transform.localPosition = Vector3.zero + Vector3.up * 0.002f;*/



				/*if(n != null)
					Object.Destroy(n);
				
				if(n2 != null)
					Object.Destroy(n2);
				
				Digit.thick = 0.15f;
				
				n = Digit._3();//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
				n.transform.parent = test2.transform;
				n.transform.localEulerAngles = new Vector3(0, 90, 180);
				n.transform.localScale = Vector3.one * 0.18f;
				n.transform.localPosition = Vector3.zero + Vector3.up * 0.001f;
				
				Digit.thick = 0.08f;
				Digit.digitColor = Obj.Colour.WHITE;
				n2 = Digit._3();//Digit.Shift(2, false, 0.15f);//Word.WriteString("3", 0.5f, Obj.Colour.BLACK, true);
				n2.transform.parent = test2.transform;
				n2.transform.localEulerAngles = new Vector3(0, 90, 180);
				n2.transform.localScale = Vector3.one * 0.18f;
				n2.transform.localPosition = Vector3.zero + Vector3.up * 0.002f;*/
			}
		}
	}
}
