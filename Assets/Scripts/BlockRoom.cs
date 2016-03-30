using UnityEngine;
using System.Collections;

public class BlockRoom : MonoBehaviour 
{
	public Cell[] cell;
	public GameObject[] side, side_;

	static float size = 4.02f;
	static float thick = 0.005f;
	static float animTime = 1f;
	static Vector3 originalScaleForSide;

	int currentActiveCell = 0;

	void SetOriginalScaleForSides()
	{
		foreach(GameObject s in side)
		{
			s.transform.localScale = originalScaleForSide;
			s.SetActive(false);
		}

		foreach(GameObject s in side_)
		{
			s.transform.localScale = originalScaleForSide;
			s.SetActive(false);
		}
	}

	IEnumerator Wait(float waitTime, int index, string s1, string s2) 
	{
		yield return new WaitForSeconds(waitTime+0.01f);

		SetOriginalScaleForSides();

		side[index].SetActive(true);
		side_[index].SetActive(true);
		side[index].GetComponent<Animation>().Play(s1);
		side_[index].GetComponent<Animation>().Play(s2);
	}

	void Update () 
	{
		int newActive = -1;

		for(int i=0; i<cell.Length; ++i)
		{
			if(cell[i].IsActive)
			{
				newActive = i;
				for(int u=i+1; u<cell.Length; ++u)
					if(cell[u].IsActive)
						return;
				//for(int u=0; u<cell.Length; ++u)
				//	if(u!=i)
				//		cell[u].trigger.collider.enabled = false;
						//cell[u].trigger.gameObject.SetActive(false);

				break;
			}
		}

		if(newActive == -1)
		{
			for(int i=0; i<cell.Length; ++i)
				cell[i].trigger.GetComponent<Collider>().enabled = true;
				//cell[i].trigger.gameObject.SetActive(true);
		}
		else if(newActive != currentActiveCell)
		{
			SetOriginalScaleForSides();
			switch(currentActiveCell)
			{
				case 0:
				{
					switch(newActive)
					{
						case 1:
						{
							side[3].SetActive(true);
							side_[3].SetActive(true);
							side[3].GetComponent<Animation>().Play("scaleDownZ");
							side_[3].GetComponent<Animation>().Play("scaleUpX");

							StartCoroutine( Wait(animTime, 2, "scaleDownX", "scaleUpZ") );
							//SetOriginalScaleForSides();
							//side[2].animation.Play("scaleDownX");
							//side_[2].animation.Play("scaleUpZ");
						
						}break;
						case 2:
						{
							side[3].SetActive(true);
							side_[3].SetActive(true);
							side[3].GetComponent<Animation>().Play("scaleDownZ");
							side_[3].GetComponent<Animation>().Play("scaleUpX");
						}break;
						case 3:
						{
							side[0].SetActive(true);
							side_[0].SetActive(true);
							side[0].GetComponent<Animation>().Play("scaleDownZ");
							side_[0].GetComponent<Animation>().Play("scaleUpX");
						}break;
						default: break;
					}
				}break;

				case 1:
				{
					switch(newActive)
					{
						case 0:
						{
					side[1].SetActive(true);
					side_[1].SetActive(true);
							side[1].GetComponent<Animation>().Play("scaleDownZ");
							side_[1].GetComponent<Animation>().Play("scaleUpX");
							
					StartCoroutine( Wait(animTime, 0, "scaleDownX", "scaleUpZ") );
							//SetOriginalScaleForSides();
							//side[0].animation.Play("scaleDownX");
							//side_[0].animation.Play("scaleUpZ");
							
						}break;
						case 2:
						{
					side[2].SetActive(true);
					side_[2].SetActive(true);
							side[2].GetComponent<Animation>().Play("scaleDownZ");
							side_[2].GetComponent<Animation>().Play("scaleUpX");
						}break;
						case 3:
						{
					side[1].SetActive(true);
					side_[1].SetActive(true);
							side[1].GetComponent<Animation>().Play("scaleDownZ");
							side_[1].GetComponent<Animation>().Play("scaleUpX");
						}break;
						default: break;
					}
				}break;

				case 2:
				{
					switch(newActive)
					{
					case 3:
					{
					side[2].SetActive(true);
					side_[2].SetActive(true);
						side[2].GetComponent<Animation>().Play("scaleDownX");
						side_[2].GetComponent<Animation>().Play("scaleUpZ");
						
					StartCoroutine( Wait(animTime, 1, "scaleDownZ", "scaleUpX") );
						//SetOriginalScaleForSides();
						//side[1].animation.Play("scaleDownZ");
						//side_[1].animation.Play("scaleUpX");
						
					}break;
					case 0:
					{
					side[3].SetActive(true);
					side_[3].SetActive(true);
						side[3].GetComponent<Animation>().Play("scaleDownX");
						side_[3].GetComponent<Animation>().Play("scaleUpZ");
					}break;
					case 1:
					{
					side[2].SetActive(true);
					side_[2].SetActive(true);
						side[2].GetComponent<Animation>().Play("scaleDownX");
						side_[2].GetComponent<Animation>().Play("scaleUpZ");
					}break;
					default: break;
					}
				}break;

				case 3:
				{
					switch(newActive)
					{
					case 2:
					{
					side[0].SetActive(true);
					side_[0].SetActive(true);
						side[0].GetComponent<Animation>().Play("scaleDownX");
						side_[0].GetComponent<Animation>().Play("scaleUpZ");
						
					StartCoroutine( Wait(animTime, 3, "scaleDownZ", "scaleUpX") );
						//SetOriginalScaleForSides();
						//side[3].animation.Play("scaleDownZ");
						//side_[3].animation.Play("scaleUpX");
						
					}break;
					case 0:
					{
					side[0].SetActive(true);
					side_[0].SetActive(true);
						side[0].GetComponent<Animation>().Play("scaleDownX");
						side_[0].GetComponent<Animation>().Play("scaleUpZ");
					}break;
					case 1:
					{
					side[1].SetActive(true);
					side_[1].SetActive(true);
						side[1].GetComponent<Animation>().Play("scaleDownX");
						side_[1].GetComponent<Animation>().Play("scaleUpZ");
					}break;
					default: break;
					}
				}break;
				default: break;
			}

			currentActiveCell = newActive;
		}

	}

	static public GameObject Create(float height = 3.8f)
	{
		GameObject r = new GameObject("BlockRoom");
		BlockRoom room = r.AddComponent<BlockRoom>() as BlockRoom;
		float tmpPos = size/3.2f;

		room.cell = new Cell[4];
		room.side = new GameObject[4];
		room.side_ = new GameObject[4];

		originalScaleForSide = new Vector3(thick, 1, thick);

		for(int i=0; i<room.cell.Length; ++i)
		{
			room.cell[i] = Cell.CreateCell(Obj.Colour.BLACK);
			room.cell[i].name += "_" + i.ToString();
			Vector2 pos = Vector3.zero;
			pos[i/2] = tmpPos * (i%2==0 ? -1 : 1);

			room.cell[i].transform.localPosition = new Vector3(pos.x, 0, pos.y);

			GameObject cube = CustomObject.CreatePrimitive(PrimitiveType.Cube);
			cube.transform.localScale = new Vector3(size, height, size);
			room.side[i] = new GameObject("S_" + i.ToString());
			cube.GetComponent<Renderer>().material.color = Color.black;
			pos = new Vector2(size/2f, size/2f);

			pos[i/2] *= (i%2==0 ? -1 : 1);

			if(i==3)
				pos *= -1;

			room.side[i].transform.localPosition = new Vector3(pos.x, -height/2f, pos.y);


			cube.transform.parent = room.side[i].transform;

			room.side[i].transform.localScale = originalScaleForSide;
			room.side[i].transform.localPosition += Vector3.up * height/2f;

			room.side[i].transform.parent = room.cell[i].transform.parent = room.transform;


			Animation anim = room.side[i].AddComponent<Animation>() as Animation;
			AnimationClip clip = new AnimationClip();
			clip.legacy = true;
			anim.playAutomatically = false;

			AnimationCurve curveX = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, 1));
			AnimationCurve curveY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, 1));
			AnimationCurve curveZ = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, thick));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);



			anim.AddClip(clip, "scaleUpX");

			curveX = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, thick));
			curveY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, 1));
			curveZ = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, 1));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			
			anim.AddClip(clip, "scaleUpZ");

			curveX = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, thick));
			curveY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, 1));
			curveZ = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, thick));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			
			anim.AddClip(clip, "scaleDownX");

			curveX = new AnimationCurve(new Keyframe(0, thick), new Keyframe(animTime, thick));
			curveY = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, 1));
			curveZ = new AnimationCurve(new Keyframe(0, 1), new Keyframe(animTime, thick));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			
			anim.AddClip(clip, "scaleDownZ");

			/*curveX = new AnimationCurve(new Keyframe(0, thick), new Keyframe(Game.drawTime, thick));
			curveY = new AnimationCurve(new Keyframe(0, 0), new Keyframe(Game.drawTime, 1));
			curveZ = new AnimationCurve(new Keyframe(0, thick), new Keyframe(Game.drawTime, thick));
			
			clip.SetCurve("", typeof(Transform), "localScale.x", curveX);
			clip.SetCurve("", typeof(Transform), "localScale.y", curveY);
			clip.SetCurve("", typeof(Transform), "localScale.z", curveZ);
			
			anim.AddClip(clip, "scaleUpY");*/

			room.side_[i] = (GameObject)Instantiate(room.side[i]);
			room.side_[i].transform.parent = room.side[i].transform.parent;
			room.side_[i].transform.localPosition = room.side[i].transform.localPosition;
			room.side_[i].transform.localScale = room.side[i].transform.localScale;

			//anim.Play("scaleUpY");
		}
		room.SetOriginalScaleForSides();
		room.side[room.currentActiveCell].SetActive(true);
		room.side[room.currentActiveCell].GetComponent<Animation>().Play("scaleUpZ");

		return r;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame

}
