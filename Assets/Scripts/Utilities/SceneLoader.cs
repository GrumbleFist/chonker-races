using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace ChonkerRaces.Utilities
{
    public class SceneLoader : MonoBehaviour
    {
        [Header("Loading Settings")]
        [SerializeField] private float loadingDelay = 1f;
        [SerializeField] private bool showLoadingScreen = true;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private UnityEngine.UI.Slider loadingBar;
        [SerializeField] private UnityEngine.UI.Text loadingText;
        
        [Header("Scene Names")]
        [SerializeField] private string mainMenuScene = "MainMenu";
        [SerializeField] private string raceScene = "RaceTrack";
        [SerializeField] private string loadingScene = "Loading";
        
        private static SceneLoader instance;
        private string targetScene;
        
        public static SceneLoader Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<SceneLoader>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("SceneLoader");
                        instance = go.AddComponent<SceneLoader>();
                        DontDestroyOnLoad(go);
                    }
                }
                return instance;
            }
        }
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
        }
        
        // Public methods for loading scenes
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
            
            targetScene = sceneName;
            
            if (showLoadingScreen)
            {
                StartCoroutine(LoadSceneWithLoadingScreen());
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
        
        public void LoadSceneAsync(string sceneName)
        {
            if (string.IsNullOrEmpty(sceneName))
            {
                Debug.LogError("Scene name is null or empty!");
                return;
            }
            
            targetScene = sceneName;
            StartCoroutine(LoadSceneAsyncCoroutine());
        }
        
        public void ReloadCurrentScene()
        {
            string currentScene = SceneManager.GetActiveScene().name;
            LoadScene(currentScene);
        }
        
        // Coroutines for loading
        private IEnumerator LoadSceneWithLoadingScreen()
        {
            // Show loading screen
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(true);
            }
            
            // Wait for loading delay
            yield return new WaitForSeconds(loadingDelay);
            
            // Load scene
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
            
            // Update loading bar
            while (!asyncLoad.isDone)
            {
                if (loadingBar != null)
                {
                    loadingBar.value = asyncLoad.progress;
                }
                
                if (loadingText != null)
                {
                    loadingText.text = $"Loading... {Mathf.Round(asyncLoad.progress * 100)}%";
                }
                
                yield return null;
            }
            
            // Hide loading screen
            if (loadingScreen != null)
            {
                loadingScreen.SetActive(false);
            }
        }
        
        private IEnumerator LoadSceneAsyncCoroutine()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(targetScene);
            
            // Don't allow the scene to activate until we're ready
            asyncLoad.allowSceneActivation = false;
            
            // Update loading progress
            while (asyncLoad.progress < 0.9f)
            {
                if (loadingBar != null)
                {
                    loadingBar.value = asyncLoad.progress;
                }
                
                if (loadingText != null)
                {
                    loadingText.text = $"Loading... {Mathf.Round(asyncLoad.progress * 100)}%";
                }
                
                yield return null;
            }
            
            // Allow scene to activate
            asyncLoad.allowSceneActivation = true;
            
            // Wait for scene to fully load
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
        
        // Utility methods
        public void SetLoadingDelay(float delay)
        {
            loadingDelay = delay;
        }
        
        public void SetShowLoadingScreen(bool show)
        {
            showLoadingScreen = show;
        }
        
        public void SetLoadingScreen(GameObject screen)
        {
            loadingScreen = screen;
        }
        
        public void SetLoadingBar(UnityEngine.UI.Slider bar)
        {
            loadingBar = bar;
        }
        
        public void SetLoadingText(UnityEngine.UI.Text text)
        {
            loadingText = text;
        }
        
        // Getters
        public string GetTargetScene()
        {
            return targetScene;
        }
        
        public bool IsLoading()
        {
            return loadingScreen != null && loadingScreen.activeInHierarchy;
        }
        
        public float GetLoadingDelay()
        {
            return loadingDelay;
        }
        
        public bool GetShowLoadingScreen()
        {
            return showLoadingScreen;
        }
    }
}
