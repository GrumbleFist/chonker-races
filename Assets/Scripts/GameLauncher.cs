using UnityEngine;
using UnityEngine.SceneManagement;

namespace ChonkerRaces
{
    public class GameLauncher : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField] private bool autoStart = true;
        [SerializeField] private float startDelay = 2f;
        
        [Header("Debug Info")]
        [SerializeField] private bool showDebugInfo = true;
        
        private void Start()
        {
            if (autoStart)
            {
                Invoke(nameof(StartGame), startDelay);
            }
            
            if (showDebugInfo)
            {
                ShowDebugInfo();
            }
        }
        
        private void StartGame()
        {
            Debug.Log("🏎️ Chonker Races - Game Starting!");
            Debug.Log("Controls: WASD or Arrow Keys to drive");
            Debug.Log("Press Space for handbrake, Shift for boost");
        }
        
        private void ShowDebugInfo()
        {
            Debug.Log("=== CHONKER RACES DEBUG INFO ===");
            Debug.Log($"Unity Version: {Application.unityVersion}");
            Debug.Log($"Platform: {Application.platform}");
            Debug.Log($"Scene: {SceneManager.GetActiveScene().name}");
            Debug.Log($"FPS Target: {Application.targetFrameRate}");
            Debug.Log("================================");
        }
        
        private void Update()
        {
            // Quick restart
            if (Input.GetKeyDown(KeyCode.R))
            {
                RestartGame();
            }
            
            // Quick quit
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                QuitGame();
            }
        }
        
        public void RestartGame()
        {
            Debug.Log("🔄 Restarting game...");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        
        public void QuitGame()
        {
            Debug.Log("👋 Quitting game...");
            
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        
        public void LoadMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
        
        public void LoadRaceScene()
        {
            SceneManager.LoadScene("RaceTrack");
        }
    }
}
