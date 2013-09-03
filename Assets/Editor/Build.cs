using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
 
public static class AutoBuilder {
	
	static string APP_NAME = "AndroidUnityTest";
	static string TARGET = "/tmp/workspace/android-test-unity";
 
	static string GetProjectName()
	{
		string[] s = Application.dataPath.Split('/');
		return s[s.Length - 2];
	}
 
	static string[] GetScenePaths()
	{
		string[] scenes = new string[EditorBuildSettings.scenes.Length];
 
		for(int i = 0; i < scenes.Length; i++)
		{
			scenes[i] = EditorBuildSettings.scenes[i].path;
		}
 
		return scenes;
	}
 
	[MenuItem("File/AutoBuilder/Android")]
	static void PerformAndroidBuild ()
	{
		string target_dir = TARGET+ "/"+ APP_NAME + ".apk";
		EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
		BuildPipeline.BuildPlayer(GetScenePaths(),target_dir ,BuildTarget.Android,BuildOptions.None);
	}
	
}