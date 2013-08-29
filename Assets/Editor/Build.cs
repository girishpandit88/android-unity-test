using System.Collections.Generic;
using System;
using UnityEditor;

class Build {
	static string[] SCENES = FindEnabledEditorScenes();

	static string APP_NAME = "UnityAndroidTest";
	static string TARGET_DIR = "target";
	
	// Command line arguments, pass in via -arg="value"
	private const string productNameCommandLineArg = "productName";
	private const string productSuffixNameCommandLineArg = "productSuffixName";
	private const string bundleIdentifierCommandLineArg = "bundleIdentifier";
	private const string bundleVersionCommandLineArg = "bundleVersion";
	private const string bundleVersionCodeCommandLineArg = "bundleVersionCode";
	
	[MenuItem ("Custom/CI/Build iPhone")]
	static void PerformIPhoneBuild () {
		string target_dir = APP_NAME;
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.iPhone,BuildOptions.None);
	}
	
	[MenuItem ("Custom/CI/Build Android")]
	static void PerformAndroidBuild () {
		string target_dir = APP_NAME + ".apk";
		
		if (CommandLineArgExists(bundleVersionCodeCommandLineArg)) {
			PlayerSettings.Android.bundleVersionCode = Convert.ToInt32(ExtractCommandLineArg(bundleVersionCodeCommandLineArg));
		}
		
		GenericBuild(SCENES, TARGET_DIR + "/" + target_dir, BuildTarget.Android,BuildOptions.None);
	}
	
	private static string[] FindEnabledEditorScenes() {
		List<string> EditorScenes = new List<string>();
		foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes) {
			if (!scene.enabled) continue;
			EditorScenes.Add(scene.path);
		}
		return EditorScenes.ToArray();
	}

	static void GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options) {
		if (CommandLineArgExists(productNameCommandLineArg)) {
			PlayerSettings.productName = ExtractCommandLineArg(productNameCommandLineArg);
		}
	
		if (CommandLineArgExists(productSuffixNameCommandLineArg)) {
			PlayerSettings.productName = PlayerSettings.productName + ExtractCommandLineArg(productSuffixNameCommandLineArg);
		}
	
		if (CommandLineArgExists(bundleIdentifierCommandLineArg)) {
			PlayerSettings.bundleIdentifier = ExtractCommandLineArg(bundleIdentifierCommandLineArg);
		}
		
		if (CommandLineArgExists(bundleVersionCommandLineArg)) {
			PlayerSettings.bundleVersion = ExtractCommandLineArg(bundleVersionCommandLineArg);
		}
		
		EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
		string res = BuildPipeline.BuildPlayer(scenes,target_dir,build_target,build_options);
		if (res.Length > 0) {
			throw new Exception("BuildPlayer failure: " + res);
		}
	}
	
    protected static bool CommandLineArgExists(string argName)
    {
        foreach (string cmdLineArg in System.Environment.GetCommandLineArgs())
        {
            string commandLineString = string.Format("-{0}", argName);
            if (cmdLineArg.Equals(commandLineString) || cmdLineArg.StartsWith(commandLineString))
            {
                return true;
            }
        }
        return false;
    }

    protected static string ExtractCommandLineArg(string argName)
    {
        foreach (string cmdLineArg in System.Environment.GetCommandLineArgs())
        {
            if (cmdLineArg.StartsWith(string.Format("-{0}", argName)))
            {
                return cmdLineArg.Substring(string.Format("-{0}=", argName).Length);
            }
        }
        return "";
    }
}