using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using ChonkerRaces.Networking;

namespace ChonkerRaces.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [Header("UI Panels")]
        [SerializeField] private GameObject mainMenuPanel;
        [SerializeField] private GameObject multiplayerPanel;
        [SerializeField] private GameObject settingsPanel;
        [SerializeField] private GameObject creditsPanel;
        
        [Header("Main Menu Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button multiplayerButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button quitButton;
        
        [Header("Multiplayer UI")]
        [SerializeField] private Button hostButton;
        [SerializeField] private Button joinButton;
        [SerializeField] private Button backFromMultiplayerButton;
        [SerializeField] private InputField serverAddressInput;
        [SerializeField] private Text connectionStatusText;
        
        [Header("Settings UI")]
        [SerializeField] private Button backFromSettingsButton;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Toggle fullscreenToggle;
        [SerializeField] private Dropdown qualityDropdown;
        
        [Header("Credits UI")]
        [SerializeField] private Button backFromCreditsButton;
        [SerializeField] private Text creditsText;
        
        private NetworkManager networkManager;
        private AudioSource audioSource;
        
        private void Awake()
        {
            networkManager = FindObjectOfType<NetworkManager>();
            audioSource = GetComponent<AudioSource>();
            
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        private void Start()
        {
            SetupUI();
            LoadSettings();
        }
        
        private void SetupUI()
        {
            // Main menu buttons
            if (playButton != null)
                playButton.onClick.AddListener(OnPlayButtonClicked);
            
            if (multiplayerButton != null)
                multiplayerButton.onClick.AddListener(OnMultiplayerButtonClicked);
            
            if (settingsButton != null)
                settingsButton.onClick.AddListener(OnSettingsButtonClicked);
            
            if (creditsButton != null)
                creditsButton.onClick.AddListener(OnCreditsButtonClicked);
            
            if (quitButton != null)
                quitButton.onClick.AddListener(OnQuitButtonClicked);
            
            // Multiplayer buttons
            if (hostButton != null)
                hostButton.onClick.AddListener(OnHostButtonClicked);
            
            if (joinButton != null)
                joinButton.onClick.AddListener(OnJoinButtonClicked);
            
            if (backFromMultiplayerButton != null)
                backFromMultiplayerButton.onClick.AddListener(OnBackFromMultiplayerClicked);
            
            // Settings buttons
            if (backFromSettingsButton != null)
                backFromSettingsButton.onClick.AddListener(OnBackFromSettingsClicked);
            
            if (masterVolumeSlider != null)
                masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
            
            if (fullscreenToggle != null)
                fullscreenToggle.onValueChanged.AddListener(OnFullscreenToggleChanged);
            
            if (qualityDropdown != null)
                qualityDropdown.onValueChanged.AddListener(OnQualityChanged);
            
            // Credits buttons
            if (backFromCreditsButton != null)
                backFromCreditsButton.onClick.AddListener(OnBackFromCreditsClicked);
            
            // Set default server address
            if (serverAddressInput != null)
                serverAddressInput.text = "localhost";
            
            // Show main menu by default
            ShowMainMenu();
        }
        
        private void LoadSettings()
        {
            // Load saved settings
            if (masterVolumeSlider != null)
                masterVolumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 1f);
            
            if (sfxVolumeSlider != null)
                sfxVolumeSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
            
            if (musicVolumeSlider != null)
                musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
            
            if (fullscreenToggle != null)
                fullscreenToggle.isOn = Screen.fullScreen;
            
            if (qualityDropdown != null)
                qualityDropdown.value = QualitySettings.GetQualityLevel();
        }
        
        private void SaveSettings()
        {
            // Save settings
            if (masterVolumeSlider != null)
                PlayerPrefs.SetFloat("MasterVolume", masterVolumeSlider.value);
            
            if (sfxVolumeSlider != null)
                PlayerPrefs.SetFloat("SFXVolume", sfxVolumeSlider.value);
            
            if (musicVolumeSlider != null)
                PlayerPrefs.SetFloat("MusicVolume", musicVolumeSlider.value);
            
            PlayerPrefs.Save();
        }
        
        // Main menu button handlers
        private void OnPlayButtonClicked()
        {
            // Load single player scene
            SceneManager.LoadScene("SinglePlayerRace");
        }
        
        private void OnMultiplayerButtonClicked()
        {
            ShowMultiplayerMenu();
        }
        
        private void OnSettingsButtonClicked()
        {
            ShowSettingsMenu();
        }
        
        private void OnCreditsButtonClicked()
        {
            ShowCreditsMenu();
        }
        
        private void OnQuitButtonClicked()
        {
            Application.Quit();
        }
        
        // Multiplayer button handlers
        private void OnHostButtonClicked()
        {
            if (networkManager != null)
            {
                networkManager.StartHost();
                UpdateConnectionStatus("Starting host...");
            }
        }
        
        private void OnJoinButtonClicked()
        {
            if (networkManager != null && serverAddressInput != null)
            {
                string serverAddress = serverAddressInput.text;
                networkManager.StartClient(serverAddress);
                UpdateConnectionStatus($"Connecting to {serverAddress}...");
            }
        }
        
        private void OnBackFromMultiplayerClicked()
        {
            ShowMainMenu();
        }
        
        // Settings button handlers
        private void OnBackFromSettingsClicked()
        {
            SaveSettings();
            ShowMainMenu();
        }
        
        private void OnMasterVolumeChanged(float value)
        {
            AudioListener.volume = value;
        }
        
        private void OnSFXVolumeChanged(float value)
        {
            // Implement SFX volume control
            Debug.Log($"SFX Volume: {value}");
        }
        
        private void OnMusicVolumeChanged(float value)
        {
            // Implement music volume control
            Debug.Log($"Music Volume: {value}");
        }
        
        private void OnFullscreenToggleChanged(bool value)
        {
            Screen.fullScreen = value;
        }
        
        private void OnQualityChanged(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }
        
        // Credits button handlers
        private void OnBackFromCreditsClicked()
        {
            ShowMainMenu();
        }
        
        // UI panel management
        private void ShowMainMenu()
        {
            HideAllPanels();
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(true);
        }
        
        private void ShowMultiplayerMenu()
        {
            HideAllPanels();
            if (multiplayerPanel != null)
                multiplayerPanel.SetActive(true);
        }
        
        private void ShowSettingsMenu()
        {
            HideAllPanels();
            if (settingsPanel != null)
                settingsPanel.SetActive(true);
        }
        
        private void ShowCreditsMenu()
        {
            HideAllPanels();
            if (creditsPanel != null)
                creditsPanel.SetActive(true);
        }
        
        private void HideAllPanels()
        {
            if (mainMenuPanel != null)
                mainMenuPanel.SetActive(false);
            
            if (multiplayerPanel != null)
                multiplayerPanel.SetActive(false);
            
            if (settingsPanel != null)
                settingsPanel.SetActive(false);
            
            if (creditsPanel != null)
                creditsPanel.SetActive(false);
        }
        
        private void UpdateConnectionStatus(string status)
        {
            if (connectionStatusText != null)
            {
                connectionStatusText.text = status;
            }
        }
        
        // Public methods for external access
        public void OnConnectedToServer()
        {
            UpdateConnectionStatus("Connected to server!");
        }
        
        public void OnDisconnectedFromServer()
        {
            UpdateConnectionStatus("Disconnected from server.");
        }
        
        public void OnConnectionFailed()
        {
            UpdateConnectionStatus("Connection failed!");
        }
    }
}
