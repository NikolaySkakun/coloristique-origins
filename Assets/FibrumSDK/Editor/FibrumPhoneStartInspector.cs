using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(FibrumPhoneStart))]
public class FibrumPhoneStartInspector : Editor {

	static int sceneNumber = 1;

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();		
		FibrumPhoneStart comp = (FibrumPhoneStart)target;
		int scenesLength = 0;
		for( int k=0; k<EditorBuildSettings.scenes.Length; k++ ) if( EditorBuildSettings.scenes[k].enabled ) scenesLength++;
		string[] scenes = new string[scenesLength];
		char[] dc = {'\\','/'};
		int currentSceneEnum = 0;
		for( int k=0; k<EditorBuildSettings.scenes.Length; k++ )
		{
			if( EditorBuildSettings.scenes[k].enabled )
			{
				int firstIndex = EditorBuildSettings.scenes[k].path.LastIndexOfAny(dc)+1;
				int lastIndex = EditorBuildSettings.scenes[k].path.LastIndexOf('.');
				scenes[currentSceneEnum] = EditorBuildSettings.scenes[k].path.Substring(firstIndex,lastIndex-firstIndex);
				currentSceneEnum++;
			}
		}
		if( scenes.Length>0 )
		{
			if( sceneNumber>=scenes.Length ) sceneNumber=0;
			if(comp != null)
			{
				sceneNumber = EditorGUILayout.Popup("Level to load after:",sceneNumber,scenes);
				comp.sceneNameToLoad = scenes[sceneNumber];
			}
		}
		else
		{
			EditorGUILayout.HelpBox("No scenes in build list! Add scenes in File->Build settings->Scenes In Build",MessageType.Error);
		}
	}
}
