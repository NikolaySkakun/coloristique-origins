using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Xml;

public class LevelConstructor : EditorWindow
{
	static LevelConstructor window;
	List <TextAsset> levels = new List<TextAsset>();
	int selectedLevel = 0;

	XmlDocument doc;

	[MenuItem ("coloristique/Constructor")]
	static void Init () 
	{
		window = (LevelConstructor)EditorWindow.GetWindow (typeof (LevelConstructor));
		window.Show();
	}

	void OnFocus()
	{
		//levels.Clear (); 
		
		if(window == null)
			window = EditorWindow.GetWindow<LevelConstructor> ();

		doc = new XmlDocument();
		doc.LoadXml( (Resources.Load("Levels/Level_15") as TextAsset).text );
		//Obj.Create<Level>(doc.DocumentElement as XmlNode);
		//LoadLevels ();
	}

	void LoadLevels()
	{
		int i = 0;
		while (Resources.Load<TextAsset> ("Levels/Level_" + i.ToString ())) 
		{
			levels.Add (Resources.Load<TextAsset> ("Levels/Level_" + i.ToString ()));
			++i;
		}
	}

	string[] GetLevelsNames()
	{
		string[] names = new string[levels.Count + 1];

		for (int i = 0; i < names.Length - 1; ++i) 
		{
			names [i] = levels [i].name;
		}

		names [names.Length - 1] = "Add level...";

		return names;
	}

	void OnGUI()
	{
		window.titleContent.text = "Constructor";










//		selectedLevel = EditorGUILayout.Popup (selectedLevel, GetLevelsNames ());
//
//		if (selectedLevel == levels.Count)
//		{
//			XmlDocument doc = new XmlDocument();
//			doc.LoadXml( (Resources.Load("Levels/Level_0") as TextAsset).text );
//			 
//
//			System.IO.File.WriteAllText(Application.dataPath + "/Resources/Levels/Level_" + selectedLevel.ToString() + ".txt", doc.OuterXml);
//
//			LoadLevels ();
//		}
	}
}
