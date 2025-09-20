using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ChonkerRaces.Editor
{
    public class PackageChecker : EditorWindow
    {
        [MenuItem("Chonker Races/Check Packages")]
        public static void ShowWindow()
        {
            GetWindow<PackageChecker>("Package Checker");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Package Installation Checker", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            CheckPackages();
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Refresh Check", GUILayout.Height(25)))
            {
                Repaint();
            }
        }
        
        private void CheckPackages()
        {
            // Check Input System
            bool inputSystemInstalled = CheckInputSystem();
            DrawPackageStatus("Input System", inputSystemInstalled, 
                "Required for cross-platform controls");
            
            // Check TextMeshPro
            bool textMeshProInstalled = CheckTextMeshPro();
            DrawPackageStatus("TextMeshPro", textMeshProInstalled, 
                "Required for better text rendering");
            
            // Check Mirror Networking
            bool mirrorInstalled = CheckMirrorNetworking();
            DrawPackageStatus("Mirror Networking", mirrorInstalled, 
                "Required for multiplayer functionality");
            
            GUILayout.Space(10);
            
            // Overall status
            bool allInstalled = inputSystemInstalled && textMeshProInstalled && mirrorInstalled;
            
            if (allInstalled)
            {
                EditorGUILayout.HelpBox("✅ All required packages are installed! You're ready to go!", 
                    MessageType.Info);
            }
            else
            {
                EditorGUILayout.HelpBox("❌ Some packages are missing. Please install them to continue.", 
                    MessageType.Warning);
            }
        }
        
        private void DrawPackageStatus(string packageName, bool isInstalled, string description)
        {
            GUILayout.BeginHorizontal();
            
            // Status icon
            string status = isInstalled ? "✅" : "❌";
            GUILayout.Label(status, GUILayout.Width(20));
            
            // Package name
            GUILayout.Label(packageName, EditorStyles.boldLabel, GUILayout.Width(120));
            
            // Description
            GUILayout.Label(description, GUILayout.ExpandWidth(true));
            
            GUILayout.EndHorizontal();
        }
        
        private bool CheckInputSystem()
        {
            // Check if Input System is available
            try
            {
                var inputSystemType = System.Type.GetType("UnityEngine.InputSystem.InputSystem, UnityEngine.InputSystem");
                return inputSystemType != null;
            }
            catch
            {
                return false;
            }
        }
        
        private bool CheckTextMeshPro()
        {
            // Check if TextMeshPro is available
            try
            {
                var tmpType = System.Type.GetType("TMPro.TextMeshProUGUI, Unity.TextMeshPro");
                return tmpType != null;
            }
            catch
            {
                return false;
            }
        }
        
        private bool CheckMirrorNetworking()
        {
            // Check if Mirror Networking is available
            try
            {
                var mirrorType = System.Type.GetType("Mirror.NetworkManager, Mirror");
                return mirrorType != null;
            }
            catch
            {
                return false;
            }
        }
    }
}
