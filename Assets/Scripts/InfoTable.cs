using UnityEngine;
using System.Collections;

public class InfoTable : Obj
{
	public GameObject text;
	public float timeForDrawingTable = 0;
	public bool drawing = false;

	static Material materialForMask;

	static InfoTable()
	{
		materialForMask = new Material(Shader.Find("DepthMask"));
	}

	void Start()
	{
		SetLayer(transform);
	}

	void SetLayer(Transform obj)
	{
		obj.gameObject.layer = LayerMask.NameToLayer("Gun");

		for(int i=0; i<obj.childCount; ++i)
		{
			SetLayer(obj.GetChild(i));
		}
	}

	void FixedUpdate()
	{
		//transform.LookAt(Player.camera.transform.position, Vector3.up);
		transform.LookAt(Player.camera.transform);
		transform.localEulerAngles -= transform.localEulerAngles.x * Vector3.right;
	}

	override public void Draw()
	{
		try
		{
		if(this.gameObject != null)
		{
		drawing = true;
		for(int i=0; i<transform.childCount; ++i)
		{
			if(transform.GetChild(i).GetComponent<Animation>() != null && transform.GetChild(i).GetComponent<Animation>().GetClip("Draw") != null)
			{
						transform.GetChild(i).GetComponent<Animation>().wrapMode = WrapMode.Once;
				transform.GetChild(i).GetComponent<Animation>().Play("Draw");
			}


		}
		}

		if(GetComponent<Animation>() != null && GetComponent<Animation>().GetClip("Draw") != null)
			GetComponent<Animation>().Play("Draw");
		}
		catch
		{

		}
		Game.DrawEvent -= Draw;


	}

	override public void Destroy()
	{


		drawing = false;
		for(int i=0; i<transform.childCount; ++i)
		{
			if(transform.GetChild(i).GetComponent<Animation>() != null && transform.GetChild(i).GetComponent<Animation>().GetClip("Destroy") != null)
				transform.GetChild(i).GetComponent<Animation>().Play("Destroy");
		}
		if(GetComponent<Animation>() != null && GetComponent<Animation>().GetClip("Destroy") != null)
			GetComponent<Animation>().Play("Destroy");
		
		Object.Destroy(gameObject, 0.35f);
		Game.DestroyEvent -= Destroy;
		this.enabled = false;
	}

	public void Hide()
	{
		text.GetComponent<Animation> ().Stop ();
		text.GetComponent<Animation> ().wrapMode = WrapMode.Once;
		text.GetComponent<Animation> ().Play("Destroy");
	}

	static void ApplyMaskToText(GameObject text)
	{
		if(text.GetComponent<Renderer>() != null)
		{
			text.GetComponent<Renderer>().material.renderQueue = 3000;
			//text.GetComponent<Renderer>().material = materialForMask;
		}

		if(text.transform.childCount > 0)
		{
			for(int i=0; i<text.transform.childCount; ++i)
			{
				ApplyMaskToText(text.transform.GetChild(i).gameObject);
			}
		}
	}



	static public InfoTable NonXmlCreate(string text, GameObject join, float x = 1f, float y = 0.5f, float z = 0f, float height = 1f)
	{
		GameObject obj = new GameObject("InfoTable");
		obj.transform.localScale = new Vector3(1, 1, -1);
		InfoTable info = obj.AddComponent<InfoTable>();


		/*GameObject line = new GameObject("Line");
		GameObject tmpLine = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);

		tmpLine.transform.parent = line.transform;
		tmpLine.GetComponent<Renderer>().material.color = Color.black;
		tmpLine.transform.localPosition += Vector3.up*(0.5f);
*/
		/*GameObject lineDown = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		//.transform.parent = lineDown.transform;
		lineDown.GetComponent<Renderer>().material.color = Color.black;
		GameObject lineUp = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		//.transform.parent = lineUp.transform;
		lineUp.GetComponent<Renderer>().material.color = Color.black;
		GameObject lineLeft = new GameObject("Line"); //GameObject.CreatePrimitive(PrimitiveType.Quad);
		GameObject tmpLineLeft = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		tmpLineLeft.transform.parent = lineLeft.transform;
		tmpLineLeft.GetComponent<Renderer>().material.color = Color.black;
		tmpLineLeft.transform.localPosition += Vector3.up*(0.5f);
		GameObject lineRight = new GameObject("Line"); //GameObject.CreatePrimitive(PrimitiveType.Quad);
		GameObject tmpLineRight = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		tmpLineRight.transform.parent = lineRight.transform;
		tmpLineRight.GetComponent<Renderer>().material.color = Color.black;
		tmpLineRight.transform.localPosition += Vector3.up*(0.5f);

		GameObject whiteTable = new GameObject("WhiteTable");
		GameObject tmpWhiteTable = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);

		GameObject maskTable = new GameObject("MaskTable");
		GameObject tmpMaskTable = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		tmpMaskTable.GetComponent<Renderer>().material = materialForMask;
		
		tmpWhiteTable.transform.parent = whiteTable.transform;
		tmpWhiteTable.transform.localPosition += Vector3.up*0.5f;

		//tmpWhiteTable2.transform.parent = whiteTable.transform;
		//tmpWhiteTable2.transform.localPosition += Vector3.up*0.5f + Vector3.forward * 0.0015f;

		tmpMaskTable.transform.parent = maskTable.transform;
		tmpMaskTable.transform.localPosition -= Vector3.up*0.5f;




		GameObject leftMask = new GameObject("MaskTable");
		GameObject tmpLeftMask = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		tmpLeftMask.GetComponent<Renderer>().material = materialForMask;
		tmpLeftMask.transform.parent = leftMask.transform;
		tmpLeftMask.transform.localPosition -= Vector3.right*0.5f;


		GameObject rightMask = new GameObject("MaskTable");
		GameObject tmpRightMask = CustomObject.CreatePrimitive(PrimitiveType.Quad, false);
		tmpRightMask.GetComponent<Renderer>().material = materialForMask;
		tmpRightMask.transform.parent = rightMask.transform;
		tmpRightMask.transform.localPosition += Vector3.right*0.5f;

		//line.transform.parent = obj.transform;
		lineDown.transform.parent = obj.transform;
		lineUp.transform.parent = obj.transform;
		lineLeft.transform.parent = obj.transform;
		lineRight.transform.parent = obj.transform;
		whiteTable.transform.parent = obj.transform;
		leftMask.transform.parent = lineLeft.transform;
		rightMask.transform.parent = lineRight.transform;
		maskTable.transform.parent = obj.transform;


		//line.transform.localScale = new Vector3(Line.height, 0, 1);
		lineDown.transform.localScale = new Vector3(0.001f, Line.height, 1);
		lineUp.transform.localScale = new Vector3(0.001f, Line.height, 1);
		lineLeft.transform.localScale = new Vector3(Line.height, 0, 1);
		lineRight.transform.localScale = new Vector3(Line.height, 0, 1);
		whiteTable.transform.localScale = new Vector3(x, 0.001f, 1);
		maskTable.transform.localScale = new Vector3(x, y, 1);
		//leftMask.transform.localScale = new Vector3(x*0.5f, y, 1);
		//rightMask.transform.localScale = new Vector3(x*0.5f, y, 1);
		leftMask.transform.localScale = new Vector3(110, 1, 1);
		rightMask.transform.localScale = new Vector3(110, 1, 1);

		lineDown.transform.localPosition = new Vector3(0, height, 0);
		whiteTable.transform.localPosition = new Vector3(0, height, 0.002f);
		maskTable.transform.localPosition = new Vector3(0, height + y, 0);
		//leftMask.transform.localPosition = new Vector3(x*0.5f, height + y*0.5f, 0);
		//rightMask.transform.localPosition = new Vector3(-x*0.5f, height + y*0.5f, 0);
		leftMask.transform.localPosition = new Vector3(0, 0.5f, 0);
		rightMask.transform.localPosition = new Vector3(0, 0.5f, 0);
		lineLeft.transform.localPosition = new Vector3(-x*0.5f, height, 0);
		lineRight.transform.localPosition = new Vector3(x*0.5f, height, 0);
*/

		/*int[] m_queues = new int[]{3000};
		Material[] materials = tmpWhiteTable.GetComponent<Renderer>().materials;
		for (int i = 0; i < materials.Length && i < m_queues.Length; ++i)
			materials[i].renderQueue = m_queues[i];
*/


		info.text = Word.WriteString(text, 0.04f, Colour.BLACK, true);

		ApplyMaskToText(info.text);
		Word.ApplyReverseColorShaderToString(info.text);
		//Word.SetupInfoTableText(info.text);
		//info.text.SetActive(false);
		info.text.transform.parent = obj.transform;
		info.text.transform.localEulerAngles = new Vector3(0, 270, 60);
		info.text.transform.localPosition = new Vector3(x*0.5f - 0.02f, height + 0.12f, z + 0.001f);

		AnimationClip clip = Game.CreateAnimationClip(Quaternion.Euler(new Vector3(0, 270, 60)), Quaternion.Euler(new Vector3(0, 270, -45)), 0.35f);
		info.text.AddComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Quaternion.Euler(new Vector3(0, 270, -45)), Quaternion.Euler(new Vector3(0, 270, 60)), 0.35f);
		info.text.GetComponent<Animation>().AddClip(clip, "Draw");

		/*
		AnimationClip clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(Line.height, 0, 1), new Vector3(Line.height, height, 1), height*0.5f);
		//line.AddComponent<Animation>().AddClip(clip, "Draw");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, Line.height, 1), new Vector3(x, Line.height, 1), x*0.5f, height*0.5f);
		lineDown.AddComponent<Animation>().AddClip(clip, "Draw");
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(x, Line.height, 1), new Vector3(0, Line.height, 1), x*0.5f, height*0.5f);
		lineDown.GetComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, Line.height, 1), new Vector3(x, Line.height, 1), x*0.5f, height*0.5f);
		clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(0, height, 0), new Vector3(0, height + y, 0), y*0.5f, x*0.5f + height*0.5f, clip);
		lineUp.AddComponent<Animation>().AddClip(clip, "Draw");
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(x, Line.height, 1), new Vector3(0, Line.height, 1), x*0.5f, height*0.5f);
		lineUp.GetComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(Line.height, 0, 1), new Vector3(Line.height, y, 1), y*0.5f, x*0.5f + height*0.5f);
		lineLeft.AddComponent<Animation>().AddClip(clip, "Draw");
		clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(-x*0.5f, height, 0), new Vector3(0, height, 0), x*0.5f, height*0.5f);
		lineLeft.GetComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(Line.height, 0, 1), new Vector3(Line.height, y, 1), y*0.5f, x*0.5f + height*0.5f);
		lineRight.AddComponent<Animation>().AddClip(clip, "Draw");
		clip = Game.CreateAnimationClip(Game.AnimationClipType.POSITION, new Vector3(x*0.5f, height, 0), new Vector3(0, height, 0), x*0.5f, height*0.5f);
		lineRight.GetComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(x, 0, 1), new Vector3(x, y, 1), y*0.5f, x*0.5f + height*0.5f);
		whiteTable.AddComponent<Animation>().AddClip(clip, "Draw");
		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(x, y, 1), new Vector3(0, y, 1), x*0.5f, x*0.5f + height*0.5f);
		whiteTable.GetComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(x, y, 1), new Vector3(x, 0, 1), y*0.5f, x*0.5f + height*0.5f);
		maskTable.AddComponent<Animation>().AddClip(clip, "Draw");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(1, 1, -1), new Vector3(0, 1, -1), 0.1f, y*0.5f + x*0.5f + height*0.5f);
		obj.AddComponent<Animation>().AddClip(clip, "Destroy");

		/*clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, y, 1), new Vector3(x*0.5f, y, 1), y*0.5f, x*0.5f + height*0.5f);
		leftMask.AddComponent<Animation>().AddClip(clip, "Destroy");

		clip = Game.CreateAnimationClip(Game.AnimationClipType.SCALE, new Vector3(0, y, 1), new Vector3(x*0.5f, y, 1), y*0.5f, x*0.5f + height*0.5f);
		rightMask.AddComponent<Animation>().AddClip(clip, "Destroy");
*/


		obj.transform.position = join.transform.position;

		info.timeForDrawingTable = x*0.5f + height*0.5f + y*0.5f;

		return info;
	}

}
