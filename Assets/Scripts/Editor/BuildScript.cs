using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.IO;

namespace ChonkerRaces.Editor
{
    public class BuildScript
    {
        [MenuItem("Build/Build All Platforms")]
        public static void BuildAllPlatforms()
        {
            BuildWindows();
            BuildAndroid();
            BuildMac();
            BuildLinux();
        }
        
        [MenuItem("Build/Build Windows")]
        public static void BuildWindows()
        {
            BuildPlayer(BuildTarget.StandaloneWindows64, "Windows/ChonkerRaces.exe");
        }
        
        [MenuItem("Build/Build Android")]
        public static void BuildAndroid()
        {
            BuildPlayer(BuildTarget.Android, "Android/ChonkerRaces.apk");
        }
        
        [MenuItem("Build/Build Mac")]
        public static void BuildMac()
        {
            BuildPlayer(BuildTarget.StandaloneOSX, "Mac/ChonkerRaces.app");
        }
        
        [MenuItem("Build/Build Linux")]
        public static void BuildLinux()
        {
            BuildPlayer(BuildTarget.StandaloneLinux64, "Linux/ChonkerRaces");
        }
        
        private static void BuildPlayer(BuildTarget target, string buildPath)
        {
            // Get build path
            string buildFolder = Path.Combine(Application.dataPath, "../build");
            string fullBuildPath = Path.Combine(buildFolder, buildPath);
            
            // Create build directory
            Directory.CreateDirectory(Path.GetDirectoryName(fullBuildPath));
            
            // Get scenes
            string[] scenes = GetBuildScenes();
            
            // Build player
            BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
            buildPlayerOptions.scenes = scenes;
            buildPlayerOptions.locationPathName = fullBuildPath;
            buildPlayerOptions.target = target;
            buildPlayerOptions.options = BuildOptions.None;
            
            // Set platform-specific settings
            SetPlatformSettings(target);
            
            // Build
            BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
            BuildSummary summary = report.summary;
            
            if (summary.result == BuildResult.Succeeded)
            {
                Debug.Log($"Build succeeded: {summary.totalSize} bytes");
                Debug.Log($"Build path: {fullBuildPath}");
            }
            else
            {
                Debug.LogError($"Build failed: {summary.result}");
            }
        }
        
        private static string[] GetBuildScenes()
        {
            string[] scenes = new string[EditorBuildSettings.scenes.Length];
            
            for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
            {
                scenes[i] = EditorBuildSettings.scenes[i].path;
            }
            
            return scenes;
        }
        
        private static void SetPlatformSettings(BuildTarget target)
        {
            switch (target)
            {
                case BuildTarget.Android:
                    SetAndroidSettings();
                    break;
                case BuildTarget.StandaloneWindows64:
                case BuildTarget.StandaloneOSX:
                case BuildTarget.StandaloneLinux64:
                    SetDesktopSettings();
                    break;
            }
        }
        
        private static void SetAndroidSettings()
        {
            // Set Android settings
            EditorUserBuildSettings.buildAppBundle = false;
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            
            // Set player settings
            PlayerSettings.Android.bundleVersionCode = GetBuildNumber();
            PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel21;
            PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevelAuto;
            
            // Set scripting backend
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
            
            // Set target architectures
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            
            // Set orientation
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
            PlayerSettings.allowedAutorotateToLandscapeLeft = true;
            PlayerSettings.allowedAutorotateToLandscapeRight = true;
            PlayerSettings.allowedAutorotateToPortrait = false;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        }
        
        private static void SetDesktopSettings()
        {
            // Set desktop settings
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.Mono);
            
            // Set resolution
            PlayerSettings.defaultScreenWidth = 1920;
            PlayerSettings.defaultScreenHeight = 1080;
            PlayerSettings.fullScreenMode = FullScreenMode.Windowed;
        }
        
        private static int GetBuildNumber()
        {
            string buildNumber = System.Environment.GetEnvironmentVariable("BUILD_NUMBER");
            if (int.TryParse(buildNumber, out int number))
            {
                return number;
            }
            return 1;
        }
        
        [MenuItem("Build/Setup Build Scenes")]
        public static void SetupBuildScenes()
        {
            // Add scenes to build settings
            EditorBuildSettingsScene[] scenes = new EditorBuildSettingsScene[]
            {
                new EditorBuildSettingsScene("Assets/Scenes/MainMenu.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/RaceTrack.unity", true),
                new EditorBuildSettingsScene("Assets/Scenes/Loading.unity", true)
            };
            
            EditorBuildSettings.scenes = scenes;
            Debug.Log("Build scenes configured!");
        }
        
        [MenuItem("Build/Clean Build Folder")]
        public static void CleanBuildFolder()
        {
            string buildFolder = Path.Combine(Application.dataPath, "../build");
            
            if (Directory.Exists(buildFolder))
            {
                Directory.Delete(buildFolder, true);
                Debug.Log("Build folder cleaned!");
            }
        }
    }
}
