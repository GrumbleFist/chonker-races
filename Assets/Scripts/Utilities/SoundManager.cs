using UnityEngine;
using System.Collections.Generic;

namespace ChonkerRaces.Utilities
{
    public class SoundManager : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioSource uiSource;
        [SerializeField] private AudioSource ambientSource;
        
        [Header("Volume Settings")]
        [SerializeField] private float masterVolume = 1f;
        [SerializeField] private float musicVolume = 0.7f;
        [SerializeField] private float sfxVolume = 1f;
        [SerializeField] private float uiVolume = 1f;
        [SerializeField] private float ambientVolume = 0.5f;
        
        [Header("Audio Clips")]
        [SerializeField] private AudioClip[] engineSounds;
        [SerializeField] private AudioClip[] tireSounds;
        [SerializeField] private AudioClip[] crashSounds;
        [SerializeField] private AudioClip[] powerUpSounds;
        [SerializeField] private AudioClip[] uiSounds;
        [SerializeField] private AudioClip[] ambientSounds;
        
        [Header("Music")]
        [SerializeField] private AudioClip menuMusic;
        [SerializeField] private AudioClip raceMusic;
        [SerializeField] private AudioClip victoryMusic;
        [SerializeField] private AudioClip defeatMusic;
        
        [Header("Settings")]
        [SerializeField] private bool playMusicOnStart = true;
        [SerializeField] private bool loopMusic = true;
        [SerializeField] private float fadeTime = 1f;
        
        private Dictionary<string, AudioClip> soundLibrary = new Dictionary<string, AudioClip>();
        private Coroutine musicFadeCoroutine;
        
        public static SoundManager Instance { get; private set; }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                InitializeSoundManager();
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            LoadVolumeSettings();
            
            if (playMusicOnStart && menuMusic != null)
            {
                PlayMusic(menuMusic);
            }
        }
        
        private void InitializeSoundManager()
        {
            // Create audio sources if they don't exist
            if (musicSource == null)
            {
                musicSource = gameObject.AddComponent<AudioSource>();
                musicSource.loop = true;
                musicSource.playOnAwake = false;
            }
            
            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
                sfxSource.loop = false;
                sfxSource.playOnAwake = false;
            }
            
            if (uiSource == null)
            {
                uiSource = gameObject.AddComponent<AudioSource>();
                uiSource.loop = false;
                uiSource.playOnAwake = false;
            }
            
            if (ambientSource == null)
            {
                ambientSource = gameObject.AddComponent<AudioSource>();
                ambientSource.loop = true;
                ambientSource.playOnAwake = false;
            }
            
            // Build sound library
            BuildSoundLibrary();
        }
        
        private void BuildSoundLibrary()
        {
            // Add engine sounds
            if (engineSounds != null)
            {
                for (int i = 0; i < engineSounds.Length; i++)
                {
                    if (engineSounds[i] != null)
                    {
                        soundLibrary[$"engine_{i}"] = engineSounds[i];
                    }
                }
            }
            
            // Add tire sounds
            if (tireSounds != null)
            {
                for (int i = 0; i < tireSounds.Length; i++)
                {
                    if (tireSounds[i] != null)
                    {
                        soundLibrary[$"tire_{i}"] = tireSounds[i];
                    }
                }
            }
            
            // Add crash sounds
            if (crashSounds != null)
            {
                for (int i = 0; i < crashSounds.Length; i++)
                {
                    if (crashSounds[i] != null)
                    {
                        soundLibrary[$"crash_{i}"] = crashSounds[i];
                    }
                }
            }
            
            // Add power-up sounds
            if (powerUpSounds != null)
            {
                for (int i = 0; i < powerUpSounds.Length; i++)
                {
                    if (powerUpSounds[i] != null)
                    {
                        soundLibrary[$"powerup_{i}"] = powerUpSounds[i];
                    }
                }
            }
            
            // Add UI sounds
            if (uiSounds != null)
            {
                for (int i = 0; i < uiSounds.Length; i++)
                {
                    if (uiSounds[i] != null)
                    {
                        soundLibrary[$"ui_{i}"] = uiSounds[i];
                    }
                }
            }
            
            // Add ambient sounds
            if (ambientSounds != null)
            {
                for (int i = 0; i < ambientSounds.Length; i++)
                {
                    if (ambientSounds[i] != null)
                    {
                        soundLibrary[$"ambient_{i}"] = ambientSounds[i];
                    }
                }
            }
        }
        
        private void LoadVolumeSettings()
        {
            masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.7f);
            sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            uiVolume = PlayerPrefs.GetFloat("UIVolume", 1f);
            ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 0.5f);
            
            UpdateVolumeLevels();
        }
        
        private void UpdateVolumeLevels()
        {
            if (musicSource != null)
                musicSource.volume = masterVolume * musicVolume;
            
            if (sfxSource != null)
                sfxSource.volume = masterVolume * sfxVolume;
            
            if (uiSource != null)
                uiSource.volume = masterVolume * uiVolume;
            
            if (ambientSource != null)
                ambientSource.volume = masterVolume * ambientVolume;
        }
        
        // Music methods
        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) return;
            
            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }
            
            musicFadeCoroutine = StartCoroutine(FadeMusic(clip));
        }
        
        public void StopMusic()
        {
            if (musicFadeCoroutine != null)
            {
                StopCoroutine(musicFadeCoroutine);
            }
            
            musicFadeCoroutine = StartCoroutine(FadeMusic(null));
        }
        
        public void PlayMenuMusic()
        {
            if (menuMusic != null)
            {
                PlayMusic(menuMusic);
            }
        }
        
        public void PlayRaceMusic()
        {
            if (raceMusic != null)
            {
                PlayMusic(raceMusic);
            }
        }
        
        public void PlayVictoryMusic()
        {
            if (victoryMusic != null)
            {
                PlayMusic(victoryMusic);
            }
        }
        
        public void PlayDefeatMusic()
        {
            if (defeatMusic != null)
            {
                PlayMusic(defeatMusic);
            }
        }
        
        // SFX methods
        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (clip == null || sfxSource == null) return;
            
            sfxSource.PlayOneShot(clip, volume);
        }
        
        public void PlaySFX(string soundName, float volume = 1f)
        {
            if (soundLibrary.ContainsKey(soundName))
            {
                PlaySFX(soundLibrary[soundName], volume);
            }
            else
            {
                Debug.LogWarning($"Sound '{soundName}' not found in sound library!");
            }
        }
        
        public void PlayEngineSound(int engineIndex, float volume = 1f)
        {
            PlaySFX($"engine_{engineIndex}", volume);
        }
        
        public void PlayTireSound(int tireIndex, float volume = 1f)
        {
            PlaySFX($"tire_{tireIndex}", volume);
        }
        
        public void PlayCrashSound(int crashIndex, float volume = 1f)
        {
            PlaySFX($"crash_{crashIndex}", volume);
        }
        
        public void PlayPowerUpSound(int powerUpIndex, float volume = 1f)
        {
            PlaySFX($"powerup_{powerUpIndex}", volume);
        }
        
        // UI methods
        public void PlayUISound(AudioClip clip, float volume = 1f)
        {
            if (clip == null || uiSource == null) return;
            
            uiSource.PlayOneShot(clip, volume);
        }
        
        public void PlayUISound(string soundName, float volume = 1f)
        {
            if (soundLibrary.ContainsKey(soundName))
            {
                PlayUISound(soundLibrary[soundName], volume);
            }
        }
        
        public void PlayButtonClick()
        {
            PlayUISound("ui_0");
        }
        
        public void PlayButtonHover()
        {
            PlayUISound("ui_1");
        }
        
        // Ambient methods
        public void PlayAmbientSound(AudioClip clip, float volume = 1f)
        {
            if (clip == null || ambientSource == null) return;
            
            ambientSource.clip = clip;
            ambientSource.volume = masterVolume * ambientVolume * volume;
            ambientSource.Play();
        }
        
        public void StopAmbientSound()
        {
            if (ambientSource != null)
            {
                ambientSource.Stop();
            }
        }
        
        // Volume control methods
        public void SetMasterVolume(float volume)
        {
            masterVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MasterVolume", masterVolume);
            UpdateVolumeLevels();
        }
        
        public void SetMusicVolume(float volume)
        {
            musicVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            UpdateVolumeLevels();
        }
        
        public void SetSFXVolume(float volume)
        {
            sfxVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
            UpdateVolumeLevels();
        }
        
        public void SetUIVolume(float volume)
        {
            uiVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("UIVolume", uiVolume);
            UpdateVolumeLevels();
        }
        
        public void SetAmbientVolume(float volume)
        {
            ambientVolume = Mathf.Clamp01(volume);
            PlayerPrefs.SetFloat("AmbientVolume", ambientVolume);
            UpdateVolumeLevels();
        }
        
        // Coroutines
        private System.Collections.IEnumerator FadeMusic(AudioClip newClip)
        {
            float startVolume = musicSource.volume;
            
            // Fade out current music
            while (musicSource.volume > 0)
            {
                musicSource.volume -= startVolume * Time.deltaTime / fadeTime;
                yield return null;
            }
            
            // Stop current music
            musicSource.Stop();
            
            // Play new music if provided
            if (newClip != null)
            {
                musicSource.clip = newClip;
                musicSource.Play();
                
                // Fade in new music
                while (musicSource.volume < startVolume)
                {
                    musicSource.volume += startVolume * Time.deltaTime / fadeTime;
                    yield return null;
                }
            }
            
            musicSource.volume = startVolume;
            musicFadeCoroutine = null;
        }
        
        // Getters
        public float GetMasterVolume() => masterVolume;
        public float GetMusicVolume() => musicVolume;
        public float GetSFXVolume() => sfxVolume;
        public float GetUIVolume() => uiVolume;
        public float GetAmbientVolume() => ambientVolume;
        
        public bool IsMusicPlaying() => musicSource != null && musicSource.isPlaying;
        public bool IsAmbientPlaying() => ambientSource != null && ambientSource.isPlaying;
    }
}
