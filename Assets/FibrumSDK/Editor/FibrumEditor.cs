using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;

public class FibrumEditor {
	[PostProcessBuildAttribute(1)]
	public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject) {
#if UNITY_IOS
		Debug.Log( pathToBuiltProject );
		StreamReader sr = new StreamReader (pathToBuiltProject + "/Classes/UnityAppController.mm");
		string fileStr = sr.ReadToEnd ();
		sr.Close ();
		Debug.Log (fileStr);
		if (!fileStr.Contains ("iCadeReaderView.h"))
		{
			string addStr1 = "#import \"iCadeReaderView.h\"\n#import \"iCadeUnityLink.h\"\n";
			fileStr = addStr1 + fileStr;
			string addStr2 = "\niCadeReaderView *iCadeReader = [[iCadeReaderView alloc] initWithFrame:CGRectZero];\niCadeUnityLink *iCade = [[iCadeUnityLink alloc] init];\n[iCadeReader setDelegate:iCade];\n[application.keyWindow.rootViewController.view addSubview:iCadeReader];\n[iCadeReader setActive:YES];\n[iCadeReader release];\n[iCade release];\n";
			int index = fileStr.IndexOf("UnitySetPlayerFocus(1);")+23;
			Debug.Log(index);
			string fileStr1 = fileStr.Substring(0,index);
			string fileStr2 = fileStr.Substring(index,fileStr.Length-index);
			fileStr = fileStr1 + addStr2 + fileStr2;
			Debug.Log (fileStr);
			StreamWriter sw = new StreamWriter(pathToBuiltProject + "/Classes/UnityAppController.mm",false);
			sw.Write(fileStr);
			sw.Flush();
			sw.Close();
			string projectPath = pathToBuiltProject.Substring(0,pathToBuiltProject.IndexOf("ios"));
			Debug.Log(projectPath);
			FileUtil.CopyFileOrDirectory(projectPath+"Assets/Plugins/iOS/iCadeReaderView.h",pathToBuiltProject + "/Classes/iCadeReaderView.h");
			//FileUtil.CopyFileOrDirectory(projectPath+"Assets/Plugins/iOS/iCadeReaderView.m",pathToBuiltProject + "/Classes/iCadeReaderView.m");
			FileUtil.CopyFileOrDirectory(projectPath+"Assets/Plugins/iOS/iCadeState.h",pathToBuiltProject + "/Classes/iCadeState.h");
			FileUtil.CopyFileOrDirectory(projectPath+"Assets/Plugins/iOS/iCadeUnityLink.h",pathToBuiltProject + "/Classes/iCadeUnityLink.h");
			//FileUtil.CopyFileOrDirectory(projectPath+"Assets/Plugins/iOS/iCadeUnityLink.mm",pathToBuiltProject + "/Classes/iCadeUnityLink.mm");
		}
#endif
	}
}
