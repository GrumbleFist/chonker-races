using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

namespace ChonkerRaces.Editor
{
    public class QuickBuild : EditorWindow
    {
        [MenuItem("Chonker Races/Quick Build")]
        public static void ShowWindow()
        {
            GetWindow<QuickBuild>("Quick Build");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Chonker Races - Quick Build", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            GUILayout.Label("Build your game for different platforms:");
            GUILayout.Space(10);
            
            if (GUILayout.Button("Build for Windows", GUILayout.Height(30)))
            {
                BuildForWindows();
            }
            
            if (GUILayout.Button("Build for Android", GUILayout.Height(30)))
            {
                BuildForAndroid();
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Build and Run (Windows)", GUILayout.Height(25)))
            {
                BuildAndRun();
            }
            
            GUILayout.Space(10);
            
            GUILayout.Label("Build Location: Assets/../Builds/");
        }
        
        private void BuildForWindows()
        {
            string buildPath = Path.Combine(Application.dataPath, "../Builds/Windows/ChonkerRaces.exe");
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetBuildScenes();
            buildPlayerOptions.locationPathName = buildPath;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"✅ Windows build successful! Size: {summary.totalSize} bytes");
                Debug.Log($"📁 Build location: {buildPath}");
                
                // Open build folder
                EditorUtility.RevealInFinder(buildPath);
            }
            else
            {
                Debug.LogError($"❌ Windows build failed: {summary.result}");
            }
        }
        
        private void BuildForAndroid()
        {
            string buildPath = Path.Combine(Application.dataPath, "../Builds/Android/ChonkerRaces.apk");
            
            // Set Android settings
            PlayerSettings.Android.bundleVersionCode = 1;
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetBuildScenes();
            buildPlayerOptions.locationPathName = buildPath;
            buildPlayerOptions.target = BuildTarget.Android;
            buildPlayerOptions.options = BuildOptions.None;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"✅ Android build successful! Size: {summary.totalSize} bytes");
                Debug.Log($"📁 Build location: {buildPath}");
                
                // Open build folder
                EditorUtility.RevealInFinder(buildPath);
            }
            else
            {
                Debug.LogError($"❌ Android build failed: {summary.result}");
            }
        }
        
        private void BuildAndRun()
        {
            string buildPath = Path.Combine(Application.dataPath, "../Builds/Windows/ChonkerRaces.exe");
            
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = GetBuildScenes();
            buildPlayerOptions.locationPathName = buildPath;
            buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
            buildPlayerOptions.options = BuildOptions.AutoRunPlayer;
            
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log("✅ Build and run successful! Game should be launching...");
            }
            else
            {
                Debug.LogError($"❌ Build and run failed: {summary.result}");
            }
        }
        
        private string[] GetBuildScenes()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            
            return scenes;
        }
    }
}
