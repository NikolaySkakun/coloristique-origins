using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Xml;
using UnityEditor.AnimatedValues;

public class LevelConstructor : EditorWindow
{
	static LevelConstructor window;
	List <TextAsset> levels = new List<TextAsset>();
	static int selectedLevel = 0;

	Vector2 scrollPosition;

	XmlDocument doc;

	[MenuItem ("coloristique/Level Constructor")]
	static void Init () 
	{
		window = (LevelConstructor)EditorWindow.GetWindow (typeof (LevelConstructor));
		window.Show();
	}

	void OnFocus()
	{
		if (levels != null)
		{
			levels.Clear ();
			levels = new List<TextAsset>();
		}

		if(window == null)
			window = EditorWindow.GetWindow<LevelConstructor> ();

		LoadLevels ();

		if (selectedLevel >= levels.Count)
			selectedLevel = 0;
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
			if (levels.Count > i && levels [i] != null)
				names [i] = levels [i].name;
			else
				break;
		}

		names [names.Length - 1] = "Add level...";

		return names;
	}


	void ShowRoomNode()
	{
		XmlNode roomNode = LevelEditor.levelNode.SelectSingleNode ("room");

		if (roomNode == null || roomNode.ChildNodes == null || roomNode.ChildNodes.Count == 0)
		{
			GUILayout.Label ("There are no rooms");
		} else
		{
			foreach (XmlNode room in roomNode.ChildNodes)
			{
				if (room == null)
					break;

				EditorGUILayout.BeginHorizontal ();
				GUILayout.Box ("Room " + room.Attributes ["index"].Value);

				if (GUILayout.Button ("Remove", GUILayout.Width(100)))
				{
					roomNode.RemoveChild (room);
					Save ();
					return;
				}
				EditorGUILayout.EndHorizontal ();

				{ // COLOR
					Obj.Colour color = Game.GetColor (room.Attributes ["color"].Value);
					Obj.Colour previousColor = color;
					color = (Obj.Colour)EditorGUILayout.EnumPopup ("Color", color);
					if (color == Obj.Colour.NONE)
						color = previousColor;
					room.Attributes ["color"].Value = Game.GetColorName (color);
				}

				{ // SIZE
					room.Attributes ["x"].Value = EditorGUILayout.FloatField ("Width (x):", float.Parse (room.Attributes ["x"].Value)).ToString ();
					room.Attributes ["y"].Value = EditorGUILayout.FloatField ("Height (y):", float.Parse (room.Attributes ["y"].Value)).ToString ();
					room.Attributes ["z"].Value = EditorGUILayout.FloatField ("Length (z):", float.Parse (room.Attributes ["z"].Value)).ToString ();
				}

				EditorGUILayout.Space ();
			}
		}

		if (GUILayout.Button ("Add room", GUILayout.Width(150)))
		{
			string roomIndex = null;

			if (roomNode == null)
			{
				roomNode = LevelEditor.AddNode (LevelEditor.levelNode, "room");
				LevelEditor.AddAttribute (roomNode, "class", "Room");
				roomIndex = "0";
			}

			if (roomNode.ChildNodes == null || roomNode.ChildNodes.Count == 0)
				roomIndex = "0";

			if (roomIndex == null)
			{
				roomIndex = (roomNode.ChildNodes.Count).ToString ();
			}

			XmlNode room = LevelEditor.AddNode (roomNode, "Room");
			LevelEditor.AddAttribute (room, "index", roomIndex);
			LevelEditor.AddAttribute (room, "color", "White");
			LevelEditor.AddAttribute (room, "x", "10");
			LevelEditor.AddAttribute (room, "y", "10");
			LevelEditor.AddAttribute (room, "z", "10");

			Save ();
		}
	}

	void Save()
	{
		LevelEditor.Save (selectedLevel);
		AssetDatabase.Refresh ();
		LevelEditor.LoadLevel (selectedLevel);
	}

	void OnGUI()
	{
		window.titleContent.text = "Constructor";

		ShowPopup ();

		GUILayout.BeginHorizontal ();
		{
			if (GUILayout.Button ("Delete"))
			{
				Delete ();
			}

			if (GUILayout.Button ("Create"))
			{
				CreateNewLevel ();
				LevelEditor.LoadLevel (selectedLevel);
			}
		}
		GUILayout.EndHorizontal ();

		EditorGUILayout.Space ();

		if (LevelEditor.levelNode == null)
			LevelEditor.LoadLevel (selectedLevel);

		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);

		ShowRoomNode ();

		if (GUILayout.Button ("Save"))
		{
			Save ();
		}

		EditorGUILayout.Space ();
		if (GUILayout.Button ("TEST"))
		{
			Game.LoadLevel (selectedLevel);
		}

		EditorGUILayout.EndScrollView ();
	}

	void Delete()
	{
		LevelEditor.DeleteLevel (selectedLevel);

		for (int i = selectedLevel + 1; i < levels.Count; ++i)
		{
			LevelEditor.ReindexLevel (i, i - 1);
		}

		levels.RemoveAt (selectedLevel);

		AssetDatabase.Refresh ();

		if (selectedLevel >= levels.Count)
			selectedLevel = levels.Count - 1;

		OnFocus ();

		LevelEditor.LoadLevel (selectedLevel);
	}

	void CreateNewLevel()
	{
		if(selectedLevel < levels.Count)
		{
			for (int i = ++selectedLevel; i < levels.Count; ++i)
			{
				LevelEditor.ReindexLevel (i, i + 1);
			}
		}

		LevelEditor.CreateLevel (selectedLevel);
		AssetDatabase.Refresh ();

		OnFocus ();
	}

	void ShowPopup()
	{
		if (selectedLevel != (selectedLevel = EditorGUILayout.Popup (selectedLevel, GetLevelsNames (), GUILayout.Width(150))))
		{
			if (selectedLevel == levels.Count)
			{
				CreateNewLevel ();
			}

			LevelEditor.LoadLevel (selectedLevel);
		} 
	}
}
