using UnityEngine;

namespace ChonkerRaces
{
    public class DamageZone : MonoBehaviour
    {
        [Header("Damage Settings")]
        [SerializeField] private float damageAmount = 10f;
        [SerializeField] private float damageInterval = 1f;
        [SerializeField] private bool continuousDamage = false;
        [SerializeField] private bool instantDamage = true;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject damageEffect;
        [SerializeField] private Color damageColor = Color.red;
        [SerializeField] private float effectDuration = 1f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip damageSound;
        
        private AudioSource audioSource;
        private float lastDamageTime;
        
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
            // Set up visual indicator for damage zone
            SetupVisualIndicator();
        }
        
        private void SetupVisualIndicator()
        {
            // Add a visual component to show this is a damage zone
            Renderer renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                Material material = renderer.material;
                if (material != null)
                {
                    material.color = damageColor;
                }
            }
            
            // Add warning particles or other visual cues
            if (damageEffect != null)
            {
                GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation, transform);
            }
        }
        
        public void ApplyDamage(CarController car)
        {
            if (car == null) return;
            
            // Check if enough time has passed for continuous damage
            if (continuousDamage && Time.time - lastDamageTime < damageInterval)
            {
                return;
            }
            
            // Apply damage
            car.TakeDamage(damageAmount);
            lastDamageTime = Time.time;
            
            // Play effects
            PlayDamageEffects();
            
            // Log damage for debugging
            Debug.Log($"Car took {damageAmount} damage! Health: {car.CurrentHealth}/{car.MaxHealth}");
        }
        
        private void PlayDamageEffects()
        {
            // Play sound
            if (damageSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(damageSound);
            }
            
            // Spawn damage effect
            if (damageEffect != null)
            {
                GameObject effect = Instantiate(damageEffect, transform.position, transform.rotation);
                Destroy(effect, effectDuration);
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                CarController car = other.GetComponent<CarController>();
                if (car != null)
                {
                    if (instantDamage)
                    {
                        ApplyDamage(car);
                    }
                }
            }
        }
        
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player") && continuousDamage)
            {
                CarController car = other.GetComponent<CarController>();
                if (car != null)
                {
                    ApplyDamage(car);
                }
            }
        }
        
        // Public methods for configuration
        public void SetDamageAmount(float amount)
        {
            damageAmount = amount;
        }
        
        public void SetDamageInterval(float interval)
        {
            damageInterval = interval;
        }
        
        public void SetContinuousDamage(bool continuous)
        {
            continuousDamage = continuous;
        }
        
        public void SetInstantDamage(bool instant)
        {
            instantDamage = instant;
        }
    }
}
