using UnityEngine;
using UnityEngine.SceneManagement;
using ChonkerRaces.Networking;

namespace ChonkerRaces.Utilities
{
    public class GameManager : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private GameMode currentGameMode = GameMode.SinglePlayer;
        [SerializeField] private int targetFrameRate = 60;
        [SerializeField] private bool pauseOnFocusLoss = true;
        
        [Header("Scene Management")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string raceScene = "RaceTrack";
        [SerializeField] private string loadingScene = "Loading";
        
        [Header("Game State")]
        [SerializeField] private bool isGamePaused = false;
        [SerializeField] private bool isGameStarted = false;
        [SerializeField] private bool isGameFinished = false;
        
        private NetworkManager networkManager;
        private SoundManager soundManager;
        
        public static GameManager Instance { get; private set; }
        
        public enum GameMode
        {
            SinglePlayer,
            Multiplayer,
            LocalMultiplayer
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeGameManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            SetupGame();
        }
        
        private void InitializeGameManager()
        {
            // Set target frame rate
            Application.targetFrameRate = targetFrameRate;
            
            // Set pause on focus loss
            Application.runInBackground = !pauseOnFocusLoss;
            
            // Find managers
            networkManager = FindObjectOfType<NetworkManager>();
            soundManager = FindObjectOfType<SoundManager>();
        }
        
        private void SetupGame()
        {
            // Initialize game state
            isGamePaused = false;
            isGameStarted = false;
            isGameFinished = false;
            
            // Setup input
            SetupInput();
            
            // Setup audio
            SetupAudio();
        }
        
        private void SetupInput()
        {
            // Enable/disable input based on game state
            if (InputSystem != null)
            {
                if (isGamePaused)
                {
                    InputSystem.Disable();
                }
                else
                {
                    InputSystem.Enable();
                }
            }
        }
        
        private void SetupAudio()
        {
            if (soundManager != null)
            {
                // Play appropriate music based on current scene
                string currentScene = SceneManager.GetActiveScene().name;
                
                if (currentScene == mainMenuScene)
                {
                    soundManager.PlayMenuMusic();
                }
                else if (currentScene == raceScene)
                {
                    soundManager.PlayRaceMusic();
                }
            }
        }
        
        private void Update()
        {
            HandleInput();
            UpdateGameState();
        }
        
        private void HandleInput()
        {
            // Handle pause input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
            
            // Handle fullscreen toggle
            if (Input.GetKeyDown(KeyCode.F11))
            {
                ToggleFullscreen();
            }
        }
        
        private void UpdateGameState()
        {
            // Update game state based on current conditions
            if (isGameStarted && !isGameFinished)
            {
                // Game is running
                Time.timeScale = isGamePaused ? 0f : 1f;
            }
        }
        
        // Game state methods
        public void StartGame()
        {
            isGameStarted = true;
            isGameFinished = false;
            isGamePaused = false;
            
            Debug.Log("Game started!");
        }
        
        public void EndGame()
        {
            isGameFinished = true;
            isGameStarted = false;
            
            Debug.Log("Game ended!");
        }
        
        public void PauseGame()
        {
            isGamePaused = true;
            Time.timeScale = 0f;
            
            Debug.Log("Game paused!");
        }
        
        public void ResumeGame()
        {
            isGamePaused = false;
            Time.timeScale = 1f;
            
            Debug.Log("Game resumed!");
        }
        
        public void TogglePause()
        {
            if (isGamePaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        
        // Scene management methods
        public void LoadMainMenu()
        {
            LoadScene(mainMenuScene);
        }
        
        public void LoadRaceScene()
        {
            LoadScene(raceScene);
        }
        
        public void LoadScene(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty!");
                return;
            }
            
            // Stop current game
            EndGame();
            
            // Load new scene
            SceneManager.LoadScene(sceneName);
        }
        
        public void ReloadCurrentScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            LoadScene(currentScene);
        }
        
        // Game mode methods
        public void SetGameMode(GameMode mode)
        {
            currentGameMode = mode;
            Debug.Log($"Game mode set to: {mode}");
        }
        
        public void StartSinglePlayer()
        {
            SetGameMode(GameMode.SinglePlayer);
            LoadRaceScene();
        }
        
        public void StartMultiplayer()
        {
            SetGameMode(GameMode.Multiplayer);
            LoadRaceScene();
        }
        
        public void StartLocalMultiplayer()
        {
            SetGameMode(GameMode.LocalMultiplayer);
            LoadRaceScene();
        }
        
        // Network methods
        public void StartHost()
        {
            if (networkManager != null)
            {
                networkManager.StartHost();
            }
        }
        
        public void StartClient(string serverAddress)
        {
            if (networkManager != null)
            {
                networkManager.StartClient(serverAddress);
            }
        }
        
        public void StopNetwork()
        {
            if (networkManager != null)
            {
                if (networkManager.isHost)
                {
                    networkManager.StopHost();
                }
                else if (networkManager.isClient)
                {
                    networkManager.StopClient();
                }
            }
        }
        
        // Utility methods
        public void ToggleFullscreen()
        {
            Screen.fullScreen = !Screen.fullScreen;
        }
        
        public void QuitGame()
        {
            Debug.Log("Quitting game...");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        // Event handlers
        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseOnFocusLoss)
            {
                if (pauseStatus)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
        
        private void OnApplicationFocus(bool hasFocus)
        {
            if (pauseOnFocusLoss)
            {
                if (!hasFocus)
                {
                    PauseGame();
                }
                else
                {
                    ResumeGame();
                }
            }
        }
        
        // Getters
        public GameMode GetCurrentGameMode() => currentGameMode;
        public bool IsGamePaused() => isGamePaused;
        public bool IsGameStarted() => isGameStarted;
        public bool IsGameFinished() => isGameFinished;
        public bool IsNetworked() => currentGameMode == GameMode.Multiplayer;
        
        // Setters
        public void SetTargetFrameRate(int frameRate)
        {
            targetFrameRate = frameRate;
            Application.targetFrameRate = targetFrameRate;
        }
        
        public void SetPauseOnFocusLoss(bool pause)
        {
            pauseOnFocusLoss = pause;
            Application.runInBackground = !pauseOnFocusLoss;
        }
    }
}
