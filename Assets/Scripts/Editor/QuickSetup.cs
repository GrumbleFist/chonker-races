using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

namespace ChonkerRaces.Editor
{
    public class QuickSetup : EditorWindow
    {
        [MenuItem("Chonker Races/Quick Setup")]
        public static void ShowWindow()
        {
            GetWindow<QuickSetup>("Chonker Races Setup");
        }
        
        private void OnGUI()
        {
            GUILayout.Label("Chonker Races - Quick Setup", EditorStyles.boldLabel);
            GUILayout.Space(10);
            
            GUILayout.Label("This will create a basic racing scene with:");
            GUILayout.Label("• Car with physics");
            GUILayout.Label("• Basic track");
            GUILayout.Label("• Camera controller");
            GUILayout.Label("• UI elements");
            GUILayout.Space(10);
            
            if (GUILayout.Button("Create Basic Racing Scene", GUILayout.Height(30)))
            {
                CreateBasicScene();
            }
            
            GUILayout.Space(10);
            
            if (GUILayout.Button("Create Car Prefab", GUILayout.Height(25)))
            {
                CreateCarPrefab();
            }
            
            if (GUILayout.Button("Create Basic Track", GUILayout.Height(25)))
            {
                CreateBasicTrack();
            }
            
            if (GUILayout.Button("Setup UI Canvas", GUILayout.Height(25)))
            {
                SetupUI();
            }
        }
        
        private void CreateBasicScene()
        {
            // Create a new scene
            var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
            
            // Create car
            CreateCarPrefab();
            
            // Create track
            CreateBasicTrack();
            
            // Setup camera
            SetupCamera();
            
            // Setup UI
            SetupUI();
            
            // Add race manager
            CreateRaceManager();
            
            // Save scene
            EditorSceneManager.SaveScene(scene, "Assets/Scenes/RaceTrack.unity");
            
            Debug.Log("Basic racing scene created! You can now play the game.");
        }
        
        private void CreateCarPrefab()
        {
            // Create car GameObject
            GameObject car = new GameObject("Car");
            car.transform.position = new Vector3(0, 1, 0);
            
            // Add Rigidbody
            Rigidbody rb = car.AddComponent<Rigidbody>();
            rb.mass = 1000f;
            rb.centerOfMass = new Vector3(0, -0.5f, 0);
            
            // Add CarController
            CarController carController = car.AddComponent<CarController>();
            
            // Create visual representation
            GameObject carBody = GameObject.CreatePrimitive(PrimitiveType.Cube);
            carBody.transform.SetParent(car.transform);
            carBody.transform.localScale = new Vector3(2, 0.5f, 4);
            carBody.transform.localPosition = new Vector3(0, 0, 0);
            carBody.name = "CarBody";
            
            // Create wheels
            CreateWheels(car);
            
            // Add tag
            car.tag = "Player";
            
            Debug.Log("Car created!");
        }
        
        private void CreateWheels(GameObject car)
        {
            // Create wheel colliders
            string[] wheelNames = { "FrontLeft", "FrontRight", "RearLeft", "RearRight" };
            Vector3[] wheelPositions = {
                new Vector3(-1, -0.5f, 1.5f),
                new Vector3(1, -0.5f, 1.5f),
                new Vector3(-1, -0.5f, -1.5f),
                new Vector3(1, -0.5f, -1.5f)
            };
            
            for (int i = 0; i < 4; i++)
            {
                // Create wheel collider
                GameObject wheel = new GameObject(wheelNames[i] + "WheelCollider");
                wheel.transform.SetParent(car.transform);
                wheel.transform.localPosition = wheelPositions[i];
                
                WheelCollider wheelCollider = wheel.AddComponent<WheelCollider>();
                wheelCollider.radius = 0.3f;
                wheelCollider.suspensionDistance = 0.3f;
                
                // Create visual wheel
                GameObject wheelVisual = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
                wheelVisual.transform.SetParent(wheel.transform);
                wheelVisual.transform.localScale = new Vector3(0.6f, 0.2f, 0.6f);
                wheelVisual.transform.localRotation = Quaternion.Euler(0, 0, 90);
                wheelVisual.name = wheelNames[i] + "WheelVisual";
            }
        }
        
        private void CreateBasicTrack()
        {
            // Create ground
            GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
            ground.transform.position = Vector3.zero;
            ground.transform.localScale = new Vector3(10, 1, 10);
            ground.name = "Track";
            
            // Add some barriers
            for (int i = 0; i < 8; i++)
            {
                GameObject barrier = GameObject.CreatePrimitive(PrimitiveType.Cube);
                barrier.transform.position = new Vector3(
                    Mathf.Cos(i * Mathf.PI / 4) * 25,
                    1,
                    Mathf.Sin(i * Mathf.PI / 4) * 25
                );
                barrier.transform.localScale = new Vector3(2, 2, 0.5f);
                barrier.name = "Barrier" + i;
            }
            
            Debug.Log("Basic track created!");
        }
        
        private void SetupCamera()
        {
            // Find main camera
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                // Add camera controller
                CameraController cameraController = mainCamera.gameObject.AddComponent<CameraController>();
                
                // Position camera
                mainCamera.transform.position = new Vector3(0, 8, -10);
                mainCamera.transform.rotation = Quaternion.Euler(15, 0, 0);
                
                Debug.Log("Camera setup complete!");
            }
        }
        
        private void SetupUI()
        {
            // Create Canvas
            GameObject canvas = new GameObject("Canvas");
            Canvas canvasComponent = canvas.AddComponent<Canvas>();
            canvasComponent.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.AddComponent<UnityEngine.UI.CanvasScaler>();
            canvas.AddComponent<UnityEngine.UI.GraphicRaycaster>();
            
            // Create UI elements
            CreateUIElements(canvas);
            
            Debug.Log("UI setup complete!");
        }
        
        private void CreateUIElements(GameObject canvas)
        {
            // Create speed text
            GameObject speedText = new GameObject("SpeedText");
            speedText.transform.SetParent(canvas.transform);
            
            UnityEngine.UI.Text text = speedText.AddComponent<UnityEngine.UI.Text>();
            text.text = "Speed: 0 km/h";
            text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
            text.fontSize = 24;
            text.color = Color.white;
            
            RectTransform rectTransform = speedText.GetComponent<RectTransform>();
            rectTransform.anchorMin = new Vector2(0, 1);
            rectTransform.anchorMax = new Vector2(0, 1);
            rectTransform.anchoredPosition = new Vector2(100, -50);
            rectTransform.sizeDelta = new Vector2(200, 50);
        }
        
        private void CreateRaceManager()
        {
            GameObject raceManager = new GameObject("RaceManager");
            raceManager.AddComponent<RaceManager>();
            
            Debug.Log("Race manager created!");
        }
    }
}
