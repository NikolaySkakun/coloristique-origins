using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VRCamera))]
public class VRCameraInspector : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		VRCamera comp = (VRCamera)target;
        bool useLensCorrection = EditorGUILayout.Toggle("Use Lens Correction",comp.useLensCorrection);
        float distanceBetweenEyes = EditorGUILayout.FloatField("Distance between eyes",comp.distanceBetweenEyes);
        if (GUI.changed)
        {
            comp.useLensCorrection = useLensCorrection;
            comp.distanceBetweenEyes = distanceBetweenEyes;
        }
	}

}
