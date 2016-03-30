//Mobile Starter Pack #1 FPS v1.2c
//by Stéphane 'Azert2k' JAMIN

using UnityEngine;
using System.Collections;
using UnityEditor;
[CanEditMultipleObjects]
[CustomEditor(typeof(MSPFps))]

 class MSPEditor : Editor  {
	GUIContent[] SeparationScr = new GUIContent[4];
	GUIContent[] PositionScr = new GUIContent[5];
	string[] controlTypeList = new string[4];
	string[] AxeType = new string[3];
	string[] FirePosLabel = new string[3];
	SerializedProperty Sensibility;
	SerializedProperty PlayerCam2;
	SerializedProperty LeftRightParts;
	SerializedProperty Pause;
	SerializedProperty Weapon;
	SerializedProperty Life;
	SerializedProperty Fourth;
	SerializedProperty StickTexture;
	SerializedProperty SocleTexture;
	SerializedProperty PauseTexture;
	SerializedProperty LifeTexture;
	SerializedProperty WeaponTexture;
	SerializedProperty LeftType;
	SerializedProperty RightType;
	SerializedProperty LeftAxisLimit;
	SerializedProperty RightAxisLimit;
	SerializedProperty FirePos;
	SerializedProperty window1Tap;
	SerializedProperty window2Tap;
	SerializedProperty AccelGyro;
	SerializedProperty FourthCorner;
	SerializedProperty FireTexture;
	SerializedProperty CrossHairTexture;
	SerializedProperty WeaponListGUI;
	SerializedProperty LifeCnt;
	SerializedProperty CHairGUI;
	SerializedProperty shootSoundC;
	SerializedProperty MuzzleFlash;
	SerializedProperty WeaponAxis;
	SerializedProperty TBeforeHitAgain;
	SerializedProperty GUIC;
	SerializedProperty ShootLayerMask;
	SerializedProperty Weapon1RS;
	SerializedProperty Weapon2RS;
	SerializedProperty Arm1RS;
	SerializedProperty Arm2RS;
	SerializedProperty SparkleE;
	SerializedProperty WalkerSpeed;
	SerializedProperty gravity;
	SerializedProperty PadSizeC;
	SerializedProperty CrossHSizeC;
	SerializedProperty FireBtnPosC;
	SerializedProperty HandStyleC;
	SerializedProperty DebugModeC;
	SerializedProperty RotateCtrl;
	SerializedProperty AcceleroSensitive;
	SerializedProperty AcceleroAngleCorr;
	SerializedProperty GyroSensitive;
	SerializedProperty GyroSmooth;
	
			
	void OnEnable () {
		RotateCtrl = serializedObject.FindProperty ("RotateControl");
		AcceleroSensitive = serializedObject.FindProperty ("AccelerometerSensibility");
		AcceleroAngleCorr = serializedObject.FindProperty ("AccelAngleCorrector");
		GyroSensitive = serializedObject.FindProperty ("MSPControl.GyroUpdateInterval");
		GyroSmooth = serializedObject.FindProperty ("GyroSmooth");
		
		//Check and serialize all variable on object
        Sensibility = serializedObject.FindProperty ("Sensitivity");
		PlayerCam2 = serializedObject.FindProperty ("PlayerCam");
		WeaponListGUI = serializedObject.FindProperty ("WeaponList");
		WeaponAxis = serializedObject.FindProperty ("AxeArms");
		ShootLayerMask = serializedObject.FindProperty("collisionLayers");
		Weapon1RS = serializedObject.FindProperty("Weapon1R");
		Weapon2RS = serializedObject.FindProperty("Weapon2R");
		Arm1RS = serializedObject.FindProperty("Arm1R");
		Arm2RS = serializedObject.FindProperty("Arm2R");
		SparkleE = serializedObject.FindProperty("Sparkle");
		WalkerSpeed= serializedObject.FindProperty ("speed");
		gravity= serializedObject.FindProperty ("gravity");
		
		PadSizeC= serializedObject.FindProperty ("MSPControl.PadSizeCorrector");
		CrossHSizeC= serializedObject.FindProperty ("MSPControl.CrossHSizeCorrector");
		FireBtnPosC= serializedObject.FindProperty ("MSPControl.FireBtnPosCorrector");
		HandStyleC= serializedObject.FindProperty ("MSPControl.HandStyle");
		DebugModeC= serializedObject.FindProperty ("MSPControl.debugMode");
		
		GUIC = serializedObject.FindProperty ("MSPControl.GUIColor");
		LeftRightParts = serializedObject.FindProperty ("MSPControl.LeftRightParts");
		LeftType= serializedObject.FindProperty ("MSPControl.LeftGUIPadStyle");
		RightType= serializedObject.FindProperty ("MSPControl.RightGUIPadStyle");
		LeftRightParts = serializedObject.FindProperty ("MSPControl.LeftRightParts");
		Pause = serializedObject.FindProperty ("MSPControl.PausePosition");
		Weapon = serializedObject.FindProperty ("MSPControl.WeaponGUIPosition");
		Life = serializedObject.FindProperty ("MSPControl.LifeGUIPosition");
		Fourth = serializedObject.FindProperty ("MSPControl.FourthGUIPosition");
		TBeforeHitAgain = serializedObject.FindProperty ("TimeBeforeHitAgain");
		StickTexture=serializedObject.FindProperty ("MSPControl.GUIObject.StickGUI");
		SocleTexture=serializedObject.FindProperty ("MSPControl.GUIObject.SocleGUI");
		PauseTexture=serializedObject.FindProperty ("MSPControl.GUIObject.PauseGUI");
		WeaponTexture=serializedObject.FindProperty ("MSPControl.GUIObject.WeaponGUI");
		LifeTexture= serializedObject.FindProperty("MSPControl.GUIObject.LifeGUI");
		FourthCorner= serializedObject.FindProperty("MSPControl.GUIObject.FourthCornerGui");
		FireTexture= serializedObject.FindProperty("MSPControl.GUIObject.FireGui");
		CrossHairTexture= serializedObject.FindProperty("MSPControl.GUIObject.CrossHairGui");
		LeftAxisLimit = serializedObject.FindProperty ("MSPControl.LeftAxeLimit");
		RightAxisLimit = serializedObject.FindProperty ("MSPControl.RightAxeLimit");
		FirePos= serializedObject.FindProperty ("MSPControl.BtnFirePos");
		window1Tap = serializedObject.FindProperty ("MSPControl.Window1TapCount");
		window2Tap = serializedObject.FindProperty ("MSPControl.Window2TapCount");
		AccelGyro = serializedObject.FindProperty ("MSPControl.AccelGyroCtrl");
		LifeCnt = serializedObject.FindProperty ("MSPControl.LifeCounterGUI");
		CHairGUI = serializedObject.FindProperty ("MSPControl.CrossHairGUI");
			
		//Populate the images and text lists
		SeparationScr[0] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/HalfHalf.png", typeof(Texture2D))as Texture);
		SeparationScr[1] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/1T2T.png", typeof(Texture2D))as Texture);
		SeparationScr[2] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/2T1T.png", typeof(Texture2D))as Texture);
		SeparationScr[3] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/Full.png", typeof(Texture2D))as Texture);
		PositionScr[0] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/LeftUp.png", typeof(Texture2D))as Texture);
		PositionScr[1] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/RightUp.png", typeof(Texture2D))as Texture);
		PositionScr[2] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/LeftLow.png", typeof(Texture2D))as Texture);
		PositionScr[3] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/RightLow.png", typeof(Texture2D))as Texture);
		PositionScr[4] = new GUIContent(AssetDatabase.LoadAssetAtPath("Assets/MobileStarterPack/Editor/GUI/None.png", typeof(Texture2D))as Texture);
		
		controlTypeList[0] = "Pad";
		controlTypeList[1] = "Slide";
		controlTypeList[2] = "Button";
		controlTypeList[3] = "None";
		
		AxeType[0] = "All";
		AxeType[1] = "Just X";
		AxeType[2] = "Just Y";
		
		FirePosLabel[0] = "Window 1";
		FirePosLabel[1] = "Window 2";
		FirePosLabel[2] = "None";
		
    }
	
	public override void OnInspectorGUI()
    {
		serializedObject.Update ();
		//Title
		GUILayout.BeginHorizontal("box", GUILayout.Width(292));
			GUILayout.FlexibleSpace();
				GUILayout.Label("Mobile Starter Pack #1 : FPS v1.2c", EditorStyles.boldLabel);
			GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		
		
		
		//Fps Var
		GUILayout.Space (5);
		GUILayout.Label("MSP FPS Script variables", EditorStyles.boldLabel);
		GUILayout.BeginVertical("Box", GUILayout.Width(292));
			EditorGUILayout.PropertyField(RotateCtrl,true);
			EditorGUILayout.PropertyField(AcceleroSensitive,true);
			EditorGUILayout.PropertyField(AcceleroAngleCorr,true);
			EditorGUILayout.PropertyField(GyroSensitive,true);
			EditorGUILayout.PropertyField(GyroSmooth,true);
			EditorGUILayout.PropertyField(WalkerSpeed,true);
			EditorGUILayout.PropertyField(gravity,true);
			EditorGUILayout.PropertyField(SparkleE,true);
			EditorGUILayout.PropertyField(TBeforeHitAgain,true);
			EditorGUILayout.PropertyField(GUIC,true);
			EditorGUILayout.PropertyField(ShootLayerMask,true);
			EditorGUILayout.PropertyField(PlayerCam2,true);
			EditorGUILayout.PropertyField(WeaponAxis,true);
			EditorGUILayout.PropertyField(Weapon1RS,true);
			EditorGUILayout.PropertyField(Weapon2RS,true);
			EditorGUILayout.PropertyField(Arm1RS,true);
			EditorGUILayout.PropertyField(Arm2RS,true);
			GUILayout.Label("Controls Sensitivity : "+Sensibility.floatValue.ToString(), EditorStyles.boldLabel);
			Sensibility.floatValue = GUILayout.HorizontalScrollbar (Sensibility.floatValue, 1.0f, 0.0f, 10.0f);
			GUILayout.Label("GUI Animation Components", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(WeaponListGUI,true);
			EditorGUILayout.PropertyField(LifeCnt,true);
			EditorGUILayout.PropertyField(CHairGUI,true);
			GUILayout.EndVertical();
		
			GUILayout.Space (5);
			
		GUILayout.EndVertical();
		
		GUILayout.Space (5);
		
			GUILayout.Label("MSP Script variables", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box", GUILayout.Width(285));
		
			EditorGUILayout.PropertyField(DebugModeC,true);	
		
			GUILayout.Space (5);
		
			//Corrector
			GUILayout.Label("Correctors", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			EditorGUILayout.PropertyField(PadSizeC,true);
			EditorGUILayout.PropertyField(CrossHSizeC,true);
			EditorGUILayout.PropertyField(FireBtnPosC,true);
			GUILayout.EndVertical();
			GUILayout.Space (5);
		
			//Windows parts
			GUILayout.Label("Windows parts", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			LeftRightParts.enumValueIndex = GUILayout.Toolbar (LeftRightParts.enumValueIndex, SeparationScr, GUILayout.Width(275), GUILayout.Height(40));
			GUILayout.EndVertical();
			GUILayout.Space (5);
		
			if(LeftRightParts.enumValueIndex == 3){
				RightType.enumValueIndex = 3;
			}
			
			//HUD Texture
			GUILayout.Label("In-Game HUD", EditorStyles.boldLabel);
			GUILayout.BeginVertical("box", GUILayout.Width(260));
			GUILayout.Label("Common GUI");
				GUILayout.BeginHorizontal();
					GUILayout.Label("    Stick", GUILayout.Width(66));
					GUILayout.Label("   Socle", GUILayout.Width(66));
					GUILayout.Label("     Fire", GUILayout.Width(66));
					GUILayout.Label("CrossHair", GUILayout.Width(66));
					
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					StickTexture.objectReferenceValue =EditorGUILayout.ObjectField(StickTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(60)) as Texture;
					SocleTexture.objectReferenceValue =EditorGUILayout.ObjectField(SocleTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(60)) as Texture;
					FireTexture.objectReferenceValue =EditorGUILayout.ObjectField(FireTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(60)) as Texture;
					CrossHairTexture.objectReferenceValue =EditorGUILayout.ObjectField(CrossHairTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(65), GUILayout.Height(60)) as Texture;
				GUILayout.EndHorizontal();
				GUILayout.Space (5);
				GUILayout.Label("Bound GUI");
				GUILayout.BeginHorizontal();
					GUILayout.Label("   Pause", GUILayout.Width(66));
					GUILayout.Label(" Weapon", GUILayout.Width(66));
					GUILayout.Label("     Life", GUILayout.Width(66));
					GUILayout.Label("   Fourth", GUILayout.Width(66));
					
				GUILayout.EndHorizontal();
				GUILayout.BeginHorizontal();
					PauseTexture.objectReferenceValue =EditorGUILayout.ObjectField(PauseTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(34)) as Texture;
					WeaponTexture.objectReferenceValue =EditorGUILayout.ObjectField(WeaponTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(34)) as Texture;
					LifeTexture.objectReferenceValue =EditorGUILayout.ObjectField(LifeTexture.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(34)) as Texture;
					FourthCorner.objectReferenceValue =EditorGUILayout.ObjectField(FourthCorner.objectReferenceValue, typeof(Texture) ,false, GUILayout.Width(66), GUILayout.Height(34)) as Texture;
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();
				GUILayout.Space (5);
			
			
			GUILayout.BeginVertical("Box");
				//Control Type
				GUILayout.Label("Control Type", EditorStyles.boldLabel);
				GUILayout.Label("In the window 1");
				LeftType.enumValueIndex = GUILayout.Toolbar (LeftType.enumValueIndex, controlTypeList, GUILayout.Width(275), GUILayout.Height(20));
				if(LeftRightParts.enumValueIndex != 3){
					GUILayout.Label("In the window 2");
				RightType.enumValueIndex = GUILayout.Toolbar (RightType.enumValueIndex, controlTypeList, GUILayout.Width(275), GUILayout.Height(20));
				}
			GUILayout.EndVertical();
				GUILayout.Space (5);
		
			//Fire Button
			if (LeftType.enumValueIndex == 1 || RightType.enumValueIndex ==1){
				GUILayout.BeginVertical("Box");
				if (LeftType.enumValueIndex == 1 && RightType.enumValueIndex !=1){
					GUILayout.Label("Fire Button position", EditorStyles.boldLabel);
					if(FirePos.enumValueIndex == 1){
						FirePos.enumValueIndex = 0;
					}
					FirePos.enumValueIndex = GUILayout.Toolbar (FirePos.enumValueIndex, FirePosLabel, GUILayout.Width(275), GUILayout.Height(20));
				}else if (LeftType.enumValueIndex != 1 && RightType.enumValueIndex ==1){
					GUILayout.Label("Fire Button position", EditorStyles.boldLabel);
					if(FirePos.enumValueIndex == 0){
						FirePos.enumValueIndex = 1;
					}
					FirePos.enumValueIndex = GUILayout.Toolbar (FirePos.enumValueIndex, FirePosLabel, GUILayout.Width(275), GUILayout.Height(20));
				}else if(LeftType.enumValueIndex == 1 && RightType.enumValueIndex ==1){
					GUILayout.Label("Fire Button position", EditorStyles.boldLabel);
					FirePos.enumValueIndex = GUILayout.Toolbar (FirePos.enumValueIndex, FirePosLabel, GUILayout.Width(275), GUILayout.Height(20));
				}
				GUILayout.EndVertical();
				GUILayout.Space (5);	
			}else{ 
				FirePos.enumValueIndex = 2;
			}
		
		
			//Availables Axes
			if (LeftType.enumValueIndex != 2 && LeftType.enumValueIndex != 3 || RightType.enumValueIndex !=2 && RightType.enumValueIndex != 3){
				GUILayout.Label("Axes Ouput", EditorStyles.boldLabel);
				GUILayout.BeginVertical("Box");
					if(LeftType.enumValueIndex != 2 && LeftType.enumValueIndex != 3){
						GUILayout.Label("In the window 1");
						LeftAxisLimit.enumValueIndex = GUILayout.Toolbar (LeftAxisLimit.enumValueIndex, AxeType, GUILayout.Width(275), GUILayout.Height(20));
					}
					if(RightType.enumValueIndex != 2 && RightType.enumValueIndex != 3){
						GUILayout.Label("In the window 2");
						RightAxisLimit.enumValueIndex = GUILayout.Toolbar (RightAxisLimit.enumValueIndex, AxeType, GUILayout.Width(275), GUILayout.Height(20));
					}
				GUILayout.EndVertical();
				GUILayout.Space (5);
			}
			
			//Accelerometre/Gyro
			GUILayout.Label("Accelerometer/Gyro Vector Ouput", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			AccelGyro.enumValueIndex = GUILayout.Toolbar (AccelGyro.enumValueIndex, AccelGyro.enumNames, GUILayout.Width(275), GUILayout.Height(20));
			GUILayout.EndVertical();
			GUILayout.Space (5);
		
			//Tap controler
			if (LeftType.enumValueIndex != 0 && LeftType.enumValueIndex !=3|| RightType.enumValueIndex !=0 && RightType.enumValueIndex !=3){
				GUILayout.Label("Tap Count Ouput", EditorStyles.boldLabel);
				GUILayout.BeginVertical("Box");
				if(LeftType.enumValueIndex != 0 && LeftType.enumValueIndex !=3){
					GUILayout.Label("In the window 1");
					window1Tap.enumValueIndex = GUILayout.Toolbar (window1Tap.enumValueIndex, window1Tap.enumNames, GUILayout.Width(185), GUILayout.Height(20));
				}
				if(RightType.enumValueIndex != 0  && RightType.enumValueIndex !=3){
					GUILayout.Label("In the window 2");
					window2Tap.enumValueIndex = GUILayout.Toolbar (window2Tap.enumValueIndex, window2Tap.enumNames, GUILayout.Width(185), GUILayout.Height(20));
				}
				GUILayout.EndVertical();
				GUILayout.Space (5);
			}
		
			//Hand Style
			GUILayout.Label("Hand Style", EditorStyles.boldLabel);
				GUILayout.BeginVertical("Box");
				GUILayout.Label("* reverses the controls of the windows 1 and 2");
				HandStyleC.enumValueIndex = GUILayout.Toolbar (HandStyleC.enumValueIndex, HandStyleC.enumNames, GUILayout.Width(185), GUILayout.Height(20));
				GUILayout.EndVertical();
			GUILayout.Space (5);
		
			//Pause HUD position
			GUILayout.Label("Pause HUD position", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			Pause.enumValueIndex = GUILayout.Toolbar (Pause.enumValueIndex, PositionScr, GUILayout.Width(275), GUILayout.Height(40));
			GUILayout.EndVertical();
			GUILayout.Space (5);
			
			//Weapon HUD position
			GUILayout.Label("Weapon HUD position", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			Weapon.enumValueIndex = GUILayout.Toolbar (Weapon.enumValueIndex, PositionScr, GUILayout.Width(275), GUILayout.Height(40));
			GUILayout.EndVertical();
			GUILayout.Space (5);
		
			//Pause HUD position
			GUILayout.Label("Life HUD position", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			Life.enumValueIndex = GUILayout.Toolbar (Life.enumValueIndex, PositionScr, GUILayout.Width(275), GUILayout.Height(40));
			GUILayout.EndVertical();
			GUILayout.Space (5);
		
			//Pause HUD position
			GUILayout.Label("Fourth Bound HUD position", EditorStyles.boldLabel);
			GUILayout.BeginVertical("Box");
			Fourth.enumValueIndex = GUILayout.Toolbar (Fourth.enumValueIndex, PositionScr, GUILayout.Width(275), GUILayout.Height(40));
			GUILayout.EndVertical();
			GUILayout.Space (5);
			
		
		
		GUILayout.EndVertical();
		
		
		serializedObject.ApplyModifiedProperties ();
	}
	
}
