using UnityEngine;
using System.Collections;
using System.Xml;
using System.Collections.Generic;

public class Level_7 : MonoBehaviour 
{
	Level level;
	Cell cell;
	GameObject circle;

	InfoTable info;
	GameObject arrow;

	bool drawingEnded = false;

	void AddAnimationForInfo()
	{
		Vector3[] points = new Vector3[]
		{new Vector3(1.5f, 1.5f, -1.5f),
			(new Vector3(1.5f, 1.5f, -1.5f))*1.15f,
			new Vector3(1.5f, 1.5f, -1.5f)};
		
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, points, 1f);
		//clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(2, 2f, -2f), new Vector3(1.5f, 1.5f, -1.5f), 1f, 0, clip);
		
		/**/
		
		if(info.text.GetComponent<Animation>() == null)
			info.text.AddComponent<Animation>();
		
		info.text.GetComponent<Animation>().AddClip(clip, "Pulse");
		
	}

	Vector3 originalArrowPosition;

	void AddAnimationForPointer()
	{
		originalArrowPosition = arrow.transform.position;

		Vector3[] points = new Vector3[]
		{arrow.transform.position,
			arrow.transform.position + Vector3.up*0.1f,
			arrow.transform.position};
		
		AnimationClip clip = Game.CreateAnimationClip (Game.AnimationClipType.POSITION, points, 1f);//Game.CreateAnimationClip(Game.AnimationClipType.SCALE, points, 1f);
		//clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(2, 2f, -2f), new Vector3(1.5f, 1.5f, -1.5f), 1f, 0, clip);
		
		/**/
		
		if(arrow.GetComponent<Animation>() == null)
			arrow.AddComponent<Animation>();
		
		arrow.GetComponent<Animation>().AddClip(clip, "Pulse");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.one, Game.drawTime);

		arrow.GetComponent<Animation>().AddClip(clip, "Draw");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, Vector3.one, new Vector3 (1, 0, 1), 0.35f);
		//clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3[]{ new Vector3 (1, 0, 1), Vector3.zero}, 0.2f, 0.35f, clip);
		
		arrow.GetComponent<Animation>().AddClip(clip, "ScaleDown");

		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.one, 0.35f);
		
		arrow.GetComponent<Animation>().AddClip(clip, "ScaleUp");


		clip = Game.CreateAnimationClip (Game.AnimationClipType.SCALE, new Vector3 (1, 0, 1), Vector3.zero, 0.2f);
		arrow.GetComponent<Animation>().AddClip(clip, "Zero");
	}

	void CreateInfo()
	{
		string useItemButtonName = Input.GetJoystickNames().Length > 0 ? "O" : "E";
		info = InfoTable.NonXmlCreate (useItemButtonName, level.ball [0].gameObject, 0.2f, 0, -0.5f, 0.2f);//0.153f);
		info.transform.GetChild(0).localScale = new Vector3(1.5f, 1.5f, -1.5f);
		info.transform.parent = Level.current.transform;
		if(!info.drawing)
			info.Draw();
	}

	Mesh GetUnityMesh()
	{
		Debug.LogWarning ("Unity");
		XmlDocument doc = new XmlDocument();
		doc.LoadXml( (Resources.Load("Sprites/Unity") as TextAsset).text );

		List<Vector3> verts = new List<Vector3> ();

		Mesh mesh = Triangulator.CreateMesh (SVGImporter.GetEdgeCollider (doc.DocumentElement as XmlNode, 10, 15));

		for (int i = 0; i < mesh.vertexCount; ++i)
		{
			verts.Add ((mesh.vertices[i] - Vector3.right*0.7095f)); // * 1.002f
		}
		mesh.SetVertices (verts);


		Mesh mesh2 = Object.Instantiate (mesh) as Mesh;
		verts = new List<Vector3> ();
		for (int i = 0; i < mesh2.vertexCount; ++i)
		{
			verts.Add (Quaternion.Euler(0, 0, 120) * mesh.vertices[i]);
		}
		mesh2.SetVertices (verts);

		mesh = LetterAnimation.CombineMeshes (new Mesh[]{ mesh, mesh2 });

		mesh2 = Object.Instantiate (mesh) as Mesh;
		verts = new List<Vector3> ();
		for (int i = 0; i < mesh2.vertexCount; ++i)
		{
			verts.Add (Quaternion.Euler(0, 0, 240) * mesh.vertices[i]);
		}
		mesh2.SetVertices (verts);

		mesh = LetterAnimation.CombineMeshes (new Mesh[]{ mesh, mesh2 });

		verts = new List<Vector3> ();
		for (int i = 0; i < mesh.vertexCount; ++i)
		{
			verts.Add (Vector3.MoveTowards( mesh.vertices[i], -Vector3.forward * 5f, UnityEngine.Random.Range(1f, 4f)));
		}
		mesh.SetVertices (verts);

		return mesh;
	}

	void CreateUnity()
	{
		


		Mesh mesh = GetUnityMesh (); //unity.GetComponent<MeshFilter> ().sharedMesh;
		GameObject unity = Word.GetGameObject(mesh);
		unity.name = "U";

		unity.transform.parent = new GameObject ("Unity").transform;
		//unity.transform.localPosition = -Vector3.forward * 0.7095f;
		unity.transform.localEulerAngles = -Vector3.up * 90f;
		//unity = unity.transform.parent.gameObject;

		unity.transform.position = new Vector3 (-4.9f, 1.88f, 1);






	}

	IEnumerator Start() 
	{
		level = Level.current;
		cell = level.room[0].cell[0];

		level.room[0].side[2].GetComponent<MeshRenderer>().enabled = true;
		level.room[0].cell[0].plane.AddComponent<BoxCollider>().size = new Vector3(1, 1, 0);
		//level.room[6].DestroyAnyway();

		//level.room[0].side[0].gameObject.AddComponent<KineticSide>();
		//info = InfoTable.NonXmlCreate("press E to grab the ball", level.ball[1].gameObject, 2.2f, 0.238f);

		CreateInfo ();

		circle = level.room[0].cell[0].plane.transform.GetChild(0).gameObject;
		if(circle.GetComponent<Animation>() == null)
			circle.AddComponent<Animation>();

		//Test ();

		arrow = CustomObject.Arrow ();

		arrow.transform.parent = level.transform;
		arrow.transform.position = level.ball [0].transform.position + Vector3.up;//*0.85f;
//info.text.transform.localScale = Vector3.zero;
		AddAnimationForPointer ();
		arrow.GetComponent<Animation> ().Play ("Draw");

		info.text.transform.localScale = Vector3.zero;
		AddAnimationForInfo();


		CreateUnity ();
//
		yield return new WaitForSeconds(Game.drawTime);

		int[] m_queues = new int[]{3000};
		Material[] materials = level.room[0].cell[0].plane.GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
			materials[i].renderQueue = m_queues[i];
		
		circle.GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
		level.room[0].side[2].GetComponent<MeshRenderer>().enabled = false;

		circle.AddComponent<BoxCollider>().isTrigger = true;

		drawingEnded = true;

		Game.DestroyEvent += Destroy;




		//info.gameObject.SetActive (false);
	}

	bool IsTriggerEmpty()
	{
		foreach(GameObject obj in cell.trigger.innerObjs)
		{
			if(obj.GetComponent<Ball>() != null)
				return false;
		}

		return true;
	}

	void Destroy()
	{
		Game.DestroyEvent -= Destroy;

		//Destroy(transform.GetChild(0).gameObject);

		Destroy(this);

	}
	/*void LateUpdate()
	{
		if(Level.current.Index != level.Index)
			Destroy(this);
	}*/

	bool needAim = false;
	bool helpVisible = false;

	IEnumerator HideArrow()
	{
		yield return new WaitForSeconds (0.35f);

			arrow.transform.position = originalArrowPosition;

		if (info != null) {
			while (info != null && info.text.GetComponent<Animation>().isPlaying)
				yield return null;

			if (info != null)
			{
			info.text.GetComponent<Animation> ().Play ("Pulse");
			info.text.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
			}
		}
	}

	void Update()
	{
//		if(Player.HasBall && info.drawing)
//		{
//			info.Destroy();
//		}

		if (arrow) {
			if (Player.HasBall) {

				if(info != null)
				{
					info.transform.parent = level.ball [0].transform;
					info.Destroy();

				}
				Object.Destroy (arrow, 0.35F);

			} else {


				arrow.transform.LookAt (Player.camera.transform);
				//arrow.transform.localEulerAngles -= transform.localEulerAngles.x * Vector3.right;

				if (Vector3.Distance (arrow.transform.position, Player.camera.transform.position) < Ball.distance) {
					if (!arrow.GetComponent<Animation> ().IsPlaying ("ScaleDown")
						&& !helpVisible) {

						arrow.GetComponent<Animation> ().Stop ();
						arrow.GetComponent<Animation> ().PlayQueued ("ScaleDown");
						arrow.GetComponent<Animation> ().wrapMode = WrapMode.Once;
						arrow.GetComponent<Animation> ().PlayQueued ("Zero");

						info.text.transform.localScale = new Vector3 (1.5f, 1.5f, -1.5f);
						info.Draw();

						StartCoroutine (HideArrow ());
						//arrow.GetComponent<Animation>()["ScaleDown"].speed = 5;
						helpVisible = true;

					}
				} else if (helpVisible) {
					arrow.GetComponent<Animation> ().wrapMode = WrapMode.Once;
					//arrow.GetComponent<Animation>().Stop();
					//arrow.GetComponent<Animation>()["Draw"].speed = 5f;
					//arrow.transform.localScale = new Vector3(1, 0, 1);
					arrow.GetComponent<Animation> ().PlayQueued ("ScaleUp");
					info.Hide();
					helpVisible = false;

				} else if (!arrow.GetComponent<Animation> ().isPlaying) {
					//arrow.GetComponent<Animation>()["Pulse"].speed = 1f;
					arrow.GetComponent<Animation> ().Play ("Pulse");
					arrow.GetComponent<Animation> ().wrapMode = WrapMode.Loop;
				}
			}
		}


//		if(info.drawing && !info.text.GetComponent<Animation>().isPlaying)
//		{
////			info.text.GetComponent<Animation>().Play("Pulse");
////			info.text.GetComponent<Animation>().wrapMode = WrapMode.Loop;
//
//			arrow.GetComponent<Animation>().Play("Pulse");
//			arrow.GetComponent<Animation>().wrapMode = WrapMode.Loop;
//		}



		if(drawingEnded)
		{
			if(cell.IsActive && !level.room[0].side[2].GetComponent<MeshRenderer>().enabled)
			{
				level.room[0].side[2].GetComponent<MeshRenderer>().enabled = true;
				(level.room[0].cell[0].plane.transform.GetChild(0).GetComponent<Renderer>().material = Game.BaseMaterial).color = Color.black;
			}
			else if(!cell.IsActive && level.room[0].side[2].GetComponent<MeshRenderer>().enabled && !cell.cell.GetComponent<Animation>().isPlaying)
			{
				level.room[0].side[2].GetComponent<MeshRenderer>().enabled = false;
				level.room[0].cell[0].plane.transform.GetChild(0).GetComponent<Renderer>().material = new Material(Shader.Find("DepthMask"));
			}


			RaycastHit hit;
			if(Physics.Raycast(Player.camera.transform.position, Player.camera.transform.forward, out hit))
			{
				if(hit.transform == circle.transform && IsTriggerEmpty() && circle.transform.localScale.x < 1.5f)
				{
					Player.AimActive(true);
					needAim = true;
				}
				else if(needAim)
				{
					needAim = false;
					Player.AimActive(false);
				}

				if(hit.transform == circle.transform && Game.IsInputActionButtonClick() && IsTriggerEmpty())
				{
					if(circle.transform.localScale.x < 1.5f)
					{
						if(circle.GetComponent<Animation>().isPlaying)
						{
							circle.GetComponent<Animation>().Stop();
						}

						circle.transform.localScale += Vector3.one * Time.deltaTime * time;

						if(circle.transform.localScale.x >= 1.5f)
						{
							cell.plane.GetComponent<BoxCollider>().enabled = false;
							cell.trigger.enabled = false;
						}
					}
					
				}
				else if(circle.transform.localScale.x < 1.5f && circle.transform.localScale.x > 0.1f && !circle.GetComponent<Animation>().isPlaying)
				{
					CircleScaleDown();
					if(needAim)
					{
						Player.AimControl(false);
						needAim = false;
					}
				}
				else if(!cell.trigger.enabled && Player.HasBall)
				{
					CircleScaleDown();
					if(needAim)
					{
						Player.AimControl(false);
						needAim = false;
					}
				}
			}

		}



		//Debug.LogWarning(Player.HasBall);
	}
	float time = 1f;
	void CircleScaleDown()
	{
		Debug.LogWarning("SCALE");

		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, 
		                                              circle.transform.localScale, 
		                                              Vector3.one * 0.1f, 
		                                              (time/1.4f) * (circle.transform.localScale.x));
		
		
		
		if(circle.GetComponent<Animation>().GetClip("ScaleDown") != null)
		{
			circle.GetComponent<Animation>().RemoveClip("ScaleDown");
		}
		
		circle.GetComponent<Animation>().AddClip(clip, "ScaleDown");
		circle.GetComponent<Animation>().Play("ScaleDown");

		cell.plane.GetComponent<BoxCollider>().enabled = true;
		cell.trigger.enabled = true;
	}

}
