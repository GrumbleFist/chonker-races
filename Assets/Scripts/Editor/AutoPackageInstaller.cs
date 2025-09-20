using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using System.Collections.Generic;
using System.Collections;

namespace ChonkerRaces.Editor
{
    public class AutoPackageInstaller : EditorWindow
    {
        private List<string> requiredPackages = new List<string>
        {
            "com.unity.inputsystem",
            "com.unity.textmeshpro",
            "https://github.com/vis2k/Mirror.git?path=/Assets/Mirror"
        };
        
        private List<string> packageNames = new List<string>
        {
            "Input System",
            "TextMeshPro",
            "Mirror Networking"
        };
        
        private List<bool> packageStatus = new List<bool>();
        private bool isInstalling = false;
        private string currentPackage = "";
        
        [MenuItem("Chonker Races/Auto Package Installer")]
        public static void ShowWindow()
        {
            GetWindow<AutoPackageInstaller>("Auto Package Installer");
        }
        
        private void OnEnable()
        {
            CheckPackageStatus();
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Auto Package Installer", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            GUILayout.Label("This tool will automatically install all required packages for Chonker Races:", EditorStyles.wordWrappedLabel);
            GUILayout.Space(10);
            
            // Display package status
            for (int i = 0; i < requiredPackages.Count; i++)
            {
                GUILayout.BeginHorizontal();
                
                string status = packageStatus[i] ? "✅" : "❌";
                GUILayout.Label(status, GUILayout.Width(20));
                
                GUILayout.Label(packageNames[i], EditorStyles.boldLabel, GUILayout.Width(120));
                
                if (packageStatus[i])
                {
                    GUILayout.Label("Installed", EditorStyles.miniLabel);
                }
                else
                {
                    GUILayout.Label("Not Installed", EditorStyles.miniLabel);
                }
                
                GUILayout.EndHorizontal();
            }
            
            GUILayout.Space(10);
            
            // Install button
            if (isInstalling)
            {
                GUILayout.Label($"Installing: {currentPackage}", EditorStyles.boldLabel);
                GUILayout.Label("Please wait...", EditorStyles.miniLabel);
            }
            else
            {
                if (GUILayout.Button("Install All Required Packages", GUILayout.Height(30)))
                {
                    StartCoroutine(InstallPackages());
                }
            }
            
            GUILayout.Space(10);
            
            // Manual installation instructions
            GUILayout.Label("Manual Installation:", EditorStyles.boldLabel);
            GUILayout.Label("1. Open Window → Package Manager", EditorStyles.miniLabel);
            GUILayout.Label("2. Click '+' → Add package from git URL", EditorStyles.miniLabel);
            GUILayout.Label("3. Enter the package URL and click Add", EditorStyles.miniLabel);
            
            GUILayout.Space(10);
            
            // Package URLs
            GUILayout.Label("Package URLs:", EditorStyles.boldLabel);
            for (int i = 0; i < requiredPackages.Count; i++)
            {
                if (!packageStatus[i])
                {
                    GUILayout.Label($"{packageNames[i]}:", EditorStyles.miniLabel);
                    GUILayout.Label(requiredPackages[i], EditorStyles.miniLabel);
                    GUILayout.Space(5);
                }
            }
        }
        
        private void CheckPackageStatus()
        {
            packageStatus.Clear();
            
            for (int i = 0; i < requiredPackages.Count; i++)
            {
                bool isInstalled = false;
                
                if (requiredPackages[i] == "com.unity.inputsystem")
                {
                    isInstalled = System.Type.GetType("UnityEngine.InputSystem.InputSystem, UnityEngine.InputSystem") != null;
                }
                else if (requiredPackages[i] == "com.unity.textmeshpro")
                {
                    isInstalled = System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro") != null;
                }
                else if (requiredPackages[i].Contains("mirror"))
                {
                    isInstalled = System.Type.GetType("Mirror.NetworkManager, Mirror") != null;
                }
                
                packageStatus.Add(isInstalled);
            }
        }
        
        private IEnumerator InstallPackages()
        {
            isInstalling = true;
            
            for (int i = 0; i < requiredPackages.Count; i++)
            {
                if (!packageStatus[i])
                {
                    currentPackage = packageNames[i];
                    Repaint();
                    
                    yield return StartCoroutine(InstallPackage(requiredPackages[i]));
                    
                    // Wait a bit between installations
                    yield return new WaitForSeconds(1f);
                }
            }
            
            isInstalling = false;
            currentPackage = "";
            
            // Refresh package status
            CheckPackageStatus();
            Repaint();
            
            Debug.Log("Package installation complete!");
        }
        
        private IEnumerator InstallPackage(string packageUrl)
        {
            Debug.Log($"Installing package: {packageUrl}");
            
            // Add package
            AddRequest request = Client.Add(packageUrl);
            
            while (!request.IsCompleted)
            {
                yield return null;
            }
            
            if (request.Status == StatusCode.Success)
            {
                Debug.Log($"Successfully installed: {packageUrl}");
            }
            else
            {
                Debug.LogError($"Failed to install {packageUrl}: {request.Error.message}");
            }
        }
    }
}
