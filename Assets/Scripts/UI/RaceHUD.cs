using UnityEngine;
using UnityEngine.UI;
using ChonkerRaces;

namespace ChonkerRaces.UI
{
    public class RaceHUD : MonoBehaviour
    {
        [Header("Race Info")]
        [SerializeField] private Text lapCounterText;
        [SerializeField] private Text positionText;
        [SerializeField] private Text speedText;
        [SerializeField] private Text raceTimeText;
        [SerializeField] private Text countdownText;
        
        [Header("Car Status")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Text healthText;
        [SerializeField] private Image boostIcon;
        [SerializeField] private Text boostText;
        [SerializeField] private Image damageIcon;
        
        [Header("Mini Map")]
        [SerializeField] private RawImage miniMapImage;
        [SerializeField] private Camera miniMapCamera;
        [SerializeField] private Transform playerCar;
        
        [Header("Race Results")]
        [SerializeField] private GameObject raceResultsPanel;
        [SerializeField] private Text raceResultsText;
        [SerializeField] private Button restartButton;
        [SerializeField] private Button mainMenuButton;
        
        [Header("Pause Menu")]
        [SerializeField] private GameObject pauseMenuPanel;
        [SerializeField] private Button resumeButton;
        [SerializeField] private Button pauseSettingsButton;
        [SerializeField] private Button quitRaceButton;
        
        private CarController carController;
        private RaceManager raceManager;
        private bool isPaused = false;
        private float raceStartTime;
        
        private void Start()
        {
            // Find components
            carController = FindObjectOfType<CarController>();
            raceManager = RaceManager.Instance;
            
            if (carController == null)
            {
                Debug.LogError("CarController not found!");
                return;
            }
            
            if (raceManager == null)
            {
                Debug.LogError("RaceManager not found!");
                return;
            }
            
            // Setup UI
            SetupUI();
            
            // Hide results panel initially
            if (raceResultsPanel != null)
                raceResultsPanel.SetActive(false);
            
            if (pauseMenuPanel != null)
                pauseMenuPanel.SetActive(false);
            
            // Start race timer
            raceStartTime = Time.time;
        }
        
        private void Update()
        {
            UpdateRaceInfo();
            UpdateCarStatus();
            UpdateMiniMap();
            HandleInput();
        }
        
        private void SetupUI()
        {
            // Setup button events
            if (restartButton != null)
                restartButton.onClick.AddListener(OnRestartButtonClicked);
            
            if (mainMenuButton != null)
                mainMenuButton.onClick.AddListener(OnMainMenuButtonClicked);
            
            if (resumeButton != null)
                resumeButton.onClick.AddListener(OnResumeButtonClicked);
            
            if (pauseSettingsButton != null)
                pauseSettingsButton.onClick.AddListener(OnPauseSettingsButtonClicked);
            
            if (quitRaceButton != null)
                quitRaceButton.onClick.AddListener(OnQuitRaceButtonClicked);
            
            // Setup health bar
            if (healthBar != null)
            {
                healthBar.maxValue = carController.MaxHealth;
                healthBar.value = carController.CurrentHealth;
            }
            
            // Setup boost icon
            if (boostIcon != null)
                boostIcon.gameObject.SetActive(false);
            
            // Setup damage icon
            if (damageIcon != null)
                damageIcon.gameObject.SetActive(false);
        }
        
        private void UpdateRaceInfo()
        {
            if (raceManager == null) return;
            
            // Update lap counter
            if (lapCounterText != null)
            {
                int currentLap = raceManager.GetPlayerLap();
                lapCounterText.text = $"Lap: {currentLap}/{raceManager.totalLaps}";
            }
            
            // Update position
            if (positionText != null)
            {
                int position = raceManager.GetPlayerPosition();
                string positionSuffix = GetPositionSuffix(position);
                positionText.text = $"{position}{positionSuffix}";
            }
            
            // Update speed
            if (speedText != null)
            {
                speedText.text = $"{carController.Speed:F0} km/h";
            }
            
            // Update race time
            if (raceTimeText != null)
            {
                float raceTime = Time.time - raceStartTime;
                raceTimeText.text = FormatTime(raceTime);
            }
            
            // Update countdown
            if (countdownText != null)
            {
                if (!raceManager.IsRaceStarted)
                {
                    countdownText.gameObject.SetActive(true);
                    // Countdown text is managed by RaceManager
                }
                else
                {
                    countdownText.gameObject.SetActive(false);
                }
            }
        }
        
        private void UpdateCarStatus()
        {
            if (carController == null) return;
            
            // Update health bar
            if (healthBar != null)
            {
                healthBar.value = carController.CurrentHealth;
            }
            
            if (healthText != null)
            {
                healthText.text = $"{carController.CurrentHealth:F0}/{carController.MaxHealth:F0}";
            }
            
            // Update boost status
            if (boostIcon != null)
            {
                boostIcon.gameObject.SetActive(carController.HasBoost);
            }
            
            if (boostText != null)
            {
                boostText.text = carController.HasBoost ? "BOOST READY" : "NO BOOST";
            }
            
            // Update damage status
            if (damageIcon != null)
            {
                damageIcon.gameObject.SetActive(carController.IsDamaged);
            }
        }
        
        private void UpdateMiniMap()
        {
            if (miniMapCamera != null && playerCar != null)
            {
                // Update mini map camera position to follow player
                Vector3 cameraPos = playerCar.position;
                cameraPos.y = miniMapCamera.transform.position.y;
                miniMapCamera.transform.position = cameraPos;
            }
        }
        
        private void HandleInput()
        {
            // Handle pause input
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        private void TogglePause()
        {
            isPaused = !isPaused;
            
            if (pauseMenuPanel != null)
            {
                pauseMenuPanel.SetActive(isPaused);
            }
            
            // Pause/unpause the game
            Time.timeScale = isPaused ? 0f : 1f;
        }
        
        private string GetPositionSuffix(int position)
        {
            switch (position)
            {
                case 1: return "st";
                case 2: return "nd";
                case 3: return "rd";
                default: return "th";
            }
        }
        
        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            int milliseconds = Mathf.FloorToInt((time % 1) * 100);
            
            return $"{minutes:00}:{seconds:00}.{milliseconds:00}";
        }
        
        // Button event handlers
        private void OnRestartButtonClicked()
        {
            if (raceManager != null)
            {
                raceManager.RestartRace();
            }
            
            if (raceResultsPanel != null)
                raceResultsPanel.SetActive(false);
        }
        
        private void OnMainMenuButtonClicked()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        
        private void OnResumeButtonClicked()
        {
            TogglePause();
        }
        
        private void OnPauseSettingsButtonClicked()
        {
            // Open settings menu
            Debug.Log("Opening pause settings...");
        }
        
        private void OnQuitRaceButtonClicked()
        {
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        }
        
        // Public methods for external access
        public void ShowRaceResults(string results)
        {
            if (raceResultsPanel != null)
            {
                raceResultsPanel.SetActive(true);
            }
            
            if (raceResultsText != null)
            {
                raceResultsText.text = results;
            }
        }
        
        public void HideRaceResults()
        {
            if (raceResultsPanel != null)
            {
                raceResultsPanel.SetActive(false);
            }
        }
        
        public void SetCountdownText(string text)
        {
            if (countdownText != null)
            {
                countdownText.text = text;
            }
        }
    }
}
