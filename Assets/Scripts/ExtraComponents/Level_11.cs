using UnityEngine;
using System.Collections;

public class Level_11 : MonoBehaviour 
{
	Level level;

	void Start () 
	{
		level = Level.current;

		Player.aimParent.SetActive (false);

		GameObject gameName = Word.WriteString("coloristique", 0.5f, Obj.Colour.WHITE, true); //"abcdefghijklmn|opqrstuvwxyz" //"coloristique"
		gameName.transform.localEulerAngles = new Vector3(0, 90, 90);

		gameName.transform.localScale = Vector3.one * 0.8f;
		//gameName.transform.localScale = Vector3.one * 0.4f;

		gameName.transform.position = level.room[0].side[4].transform.position;
		gameName.transform.position += Vector3.forward*0.001f + Vector3.right*level.room[0].Size.x/2.22f;
		gameName.transform.parent = level.transform;



		CreateMobiusStrip ();
		CreateTesseract ();
		CreatePenroseTriangle ();

		//level.room[0].side[0].collider.enabled = true;
	
	}

	GameObject symbol;
	GameObject symbol2;
	GameObject symbol3;

	void CreateMobiusStrip()
	{
		float thick = 0.18f;
		float contourThick = 0.02f;
		float radius = 0.8f;
		Symbol firstStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, contourThick);
		Symbol secondStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick, radius, Obj.Colour.WHITE, -contourThick);
		//Destroy (firstStrip.GetComponent<Symbol> ());
		//Destroy (firstStrip.GetComponent<Symbol> ());

		Symbol mainStrip = Symbol.Create(Symbol.Type.MOBIUS_STRIP, thick - contourThick, radius, Obj.Colour.BLACK);
		mainStrip.firstStrip = firstStrip.gameObject;
		mainStrip.secondStrip = secondStrip.gameObject;

		GameObject[] symbols = new GameObject[] {
			firstStrip.gameObject,
			secondStrip.gameObject,
			mainStrip.gameObject
		};

		symbol = new GameObject ("Symbol");
		foreach (GameObject obj in symbols)
		{
			obj.transform.parent = symbol.transform;
		}


		symbol.transform.parent = level.room [0].transform;
		symbol.transform.localEulerAngles = Vector3.up * 90f;
		Player.SetPosition (symbol, level.room [0], new Vector3 (25, 1.65f, 50)); //30, 1.65f, 50

		symbol.AddComponent<SphereCollider> ().radius = 1;
	}

	void CreateTesseract()
	{
		GameObject tes = symbol2 = Tesseract.Create ().gameObject;
		//tes.transform.localEulerAngles = Vector3.up * 45f;
		Player.SetPosition(tes, level.room [0], new Vector3 (75, 1.6f, 50));

		tes.AddComponent<SphereCollider> ().radius = 1;

//		foreach (Side s in level.room[2].side)
//		{
//			foreach (Line l in s.line)
//				l.Repaint ();
//		}
	}

	void CreatePenroseTriangle()
	{
		symbol3 = PenroseTriangle.Create (GameObject.FindObjectsOfType<Camera>());

		symbol3.transform.parent = level.room [0].transform;
		symbol3.transform.localScale = Vector3.one * 0.025f;
		symbol3.transform.localEulerAngles += Vector3.up * 90f;
		//symbol.transform.localEulerAngles = Vector3.up * 90f;
		Player.SetPosition (symbol3, level.room [0], new Vector3 (45, 1.65f, 90));
		symbol3.AddComponent<SphereCollider> ().radius = 1f/symbol3.transform.localScale.x;
	}
	

	void Update () 
	{
	
	}
}
