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

		GUILayout.Label("Last build version: " + game.major.ToString() + "." + game.minor.ToString() + "." + (game.build < 10 ? "0" : "") + game.build.ToString());

		game.major = EditorGUILayout.IntField(game.major);
		game.minor = EditorGUILayout.IntField(game.minor);
		game.build = EditorGUILayout.IntField(game.build);

		if(GUILayout.Button("Build"))
		{
			BuildPipeline.BuildPlayer(new string[] {"Assets/Scenes/Game.unity"}, "/Users/mac/Desktop/builds/qqq.app", BuildTarget.StandaloneOSXUniversal, BuildOptions.None);
		}

		//EditorGUILayout.l
	}
}
