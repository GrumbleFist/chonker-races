using UnityEngine;

namespace ChonkerRaces
{
    public class Checkpoint : MonoBehaviour
    {
        [Header("Checkpoint Settings")]
        [SerializeField] private int checkpointIndex = 0;
        [SerializeField] private bool isFinishLine = false;
        [SerializeField] private bool isStartLine = false;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject checkpointEffect;
        [SerializeField] private Color checkpointColor = Color.green;
        [SerializeField] private float effectDuration = 2f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip checkpointSound;
        
        private AudioSource audioSource;
        private bool hasBeenTriggered = false;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        private void Start()
        {
            SetupVisualIndicator();
        }
        
        private void SetupVisualIndicator()
        {
            // Add visual components to show this is a checkpoint
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                if (material != null)
                {
                    material.color = checkpointColor;
                }
            }
            
            // Add checkpoint particles or other visual cues
            if (checkpointEffect != null)
            {
                GameObject effect = Instantiate(checkpointEffect, transform.position, transform.rotation, transform);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !hasBeenTriggered)
            {
                CarController car = other.GetComponent<CarController>();
                if (car != null)
                {
                    TriggerCheckpoint(car);
                }
            }
        }
        
        private void TriggerCheckpoint(CarController car)
        {
            hasBeenTriggered = true;
            
            // Play effects
            PlayCheckpointEffects();
            
            // Notify race manager
            if (RaceManager.Instance != null)
            {
                RaceManager.Instance.OnCheckpointPassed(car, checkpointIndex);
            }
            
            // Log checkpoint passage
            string checkpointType = isFinishLine ? "Finish Line" : 
                                  isStartLine ? "Start Line" : 
                                  $"Checkpoint {checkpointIndex}";
            
            Debug.Log($"{car.name} passed {checkpointType}");
            
            // Reset trigger after a short delay to allow for multiple passes
            Invoke(nameof(ResetTrigger), 0.5f);
        }
        
        private void PlayCheckpointEffects()
        {
            // Play sound
            if (checkpointSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(checkpointSound);
            }
            
            // Spawn checkpoint effect
            if (checkpointEffect != null)
            {
                GameObject effect = Instantiate(checkpointEffect, transform.position, transform.rotation);
                Destroy(effect, effectDuration);
            }
        }
        
        private void ResetTrigger()
        {
            hasBeenTriggered = false;
        }
        
        // Public methods for configuration
        public void SetCheckpointIndex(int index)
        {
            checkpointIndex = index;
        }
        
        public void SetAsFinishLine(bool finish)
        {
            isFinishLine = finish;
        }
        
        public void SetAsStartLine(bool start)
        {
            isStartLine = start;
        }
        
        public int GetCheckpointIndex()
        {
            return checkpointIndex;
        }
        
        public bool IsFinishLine()
        {
            return isFinishLine;
        }
        
        public bool IsStartLine()
        {
            return isStartLine;
        }
    }
}
