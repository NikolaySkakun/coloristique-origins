using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Game))]
public class GameEditor : Editor 
{
	Game game;

	void OnEnable()
	{
		if(Selection.activeGameObject)
			game = Selection.activeGameObject.GetComponent<Game>();
	}

	public override void OnInspectorGUI()
	{
		if(game == null)
		{
			if(Selection.activeGameObject)
				game = Selection.activeGameObject.GetComponent<Game>();
		}

		GUILayout.BeginHorizontal ();
		GUILayout.Label("Build version: " + game.BuildVersion);

		if (GUILayout.Button ("Next version"))
		{
			game.build++;

			if (game.build >= 100)
			{
				game.build = 0;
				game.minor++;

				if (game.minor >= 100)
				{
					game.minor = 0;
					game.major++;
				}
			}
		}
		GUILayout.EndHorizontal ();
//		game.major = EditorGUILayout.IntField(game.major);
//		game.minor = EditorGUILayout.IntField(game.minor);
//		game.build = EditorGUILayout.IntField(game.build);


		EditorGUILayout.Space ();

		if(GUILayout.Button("Make a Build"))
		{
			Build ();
		}
	}

	void Build()
	{
		string[] levels = new string[] { "Assets/Scenes/Game.unity" };
		string locationPathName = "/Users/skakun/Desktop/coloristique (" + game.BuildVersion + ").app";
		BuildTarget target = BuildTarget.StandaloneOSXUniversal;
		BuildOptions options = BuildOptions.None;

		BuildPipeline.BuildPlayer (levels, locationPathName, target, options);
	}
}
