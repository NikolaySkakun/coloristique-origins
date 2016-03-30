using UnityEngine;
using System.Collections;
using System.Xml;

public class Line : Obj 
{
	public static float height = 0.012f;//0.008f;//0.003f; 

	public Line clone;
	public Vector3 originalScale, originalPosition;
	public bool hasClone = false;
	
	static public Line Create(float hei)
	{
		GameObject line = CustomObject.CreatePrimitive(PrimitiveType.Quad, false, true);
		//Object.Destroy(line.collider);
		line.GetComponent<Collider>().enabled = false;
		line.transform.localScale = new Vector3(height / hei, 1, 1);
		line.transform.localPosition -= Vector3.forward / 100000f;

		return line.AddComponent<Line>() as Line;
	}

	void Update()
	{

	}

	override public void PostDrawing()
	{
		if(GetComponent<Animation>()!=null && GetComponent<Animation>().isPlaying)
			GetComponent<Animation>().Stop();

		transform.localPosition = originalPosition;
		transform.localScale = originalScale;

		//Debug.Log(originalScale);

		clone.gameObject.SetActive(true);
	}

	public void SetClone(Line c)
	{
		originalScale = transform.localScale;
		originalPosition = transform.localPosition;
		clone = c;
		c.gameObject.SetActive(false);
		hasClone = true;
		//originalLength = transform.localScale
	}

	override public void Draw()
	{
		if(this != null)
		{
			if(GetComponent<Animation>() != null)
				GetComponent<Animation>().Play("Draw");
			Game.DrawEvent -= Draw;
		}
	}
}
