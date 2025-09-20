using UnityEngine;

namespace ChonkerRaces
{
    public enum PowerUpType
    {
        Boost,
        Repair,
        Shield,
        SpeedBoost,
        Jump
    }
    
    public class PowerUp : MonoBehaviour
    {
        [Header("Power-up Settings")]
        [SerializeField] private PowerUpType powerUpType = PowerUpType.Boost;
        [SerializeField] private float respawnTime = 10f;
        [SerializeField] private float effectDuration = 5f;
        [SerializeField] private float effectValue = 1f;
        
        [Header("Visual Effects")]
        [SerializeField] private GameObject collectEffect;
        [SerializeField] private GameObject powerUpModel;
        [SerializeField] private float rotationSpeed = 90f;
        [SerializeField] private float bobHeight = 0.5f;
        [SerializeField] private float bobSpeed = 2f;
        
        [Header("Audio")]
        [SerializeField] private AudioClip collectSound;
        
        private Vector3 startPosition;
        private bool isCollected = false;
        private float respawnTimer = 0f;
        private AudioSource audioSource;
        private Collider powerUpCollider;
        
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            powerUpCollider = GetComponent<Collider>();
            startPosition = transform.position;
        }
        
        private void Start()
        {
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }
        
        private void Update()
        {
            if (!isCollected)
            {
                AnimatePowerUp();
            }
            else
            {
                HandleRespawn();
            }
        }
        
        private void AnimatePowerUp()
        {
            // Rotate the power-up
            if (powerUpModel != null)
            {
                powerUpModel.transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            }
            
            // Bob up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
        
        private void HandleRespawn()
        {
            respawnTimer += Time.deltaTime;
            
            if (respawnTimer >= respawnTime)
            {
                RespawnPowerUp();
            }
        }
        
        public void Collect(CarController car)
        {
            if (isCollected) return;
            
            isCollected = true;
            respawnTimer = 0f;
            
            // Apply power-up effect
            ApplyPowerUpEffect(car);
            
            // Play effects
            PlayCollectEffects();
            
            // Hide power-up
            SetPowerUpVisibility(false);
        }
        
        private void ApplyPowerUpEffect(CarController car)
        {
            switch (powerUpType)
            {
                case PowerUpType.Boost:
                    car.CollectBoost();
                    break;
                    
                case PowerUpType.Repair:
                    car.RepairCar();
                    break;
                    
                case PowerUpType.Shield:
                    // Shield effect would be implemented in CarController
                    Debug.Log("Shield activated!");
                    break;
                    
                case PowerUpType.SpeedBoost:
                    // Temporary speed boost
                    Debug.Log("Speed boost activated!");
                    break;
                    
                case PowerUpType.Jump:
                    // Jump ability
                    Debug.Log("Jump power-up collected!");
                    break;
            }
        }
        
        private void PlayCollectEffects()
        {
            // Play sound
            if (collectSound != null && audioSource != null)
            {
                audioSource.PlayOneShot(collectSound);
            }
            
            // Spawn particle effect
            if (collectEffect != null)
            {
                GameObject effect = Instantiate(collectEffect, transform.position, transform.rotation);
                Destroy(effect, 2f);
            }
        }
        
        private void SetPowerUpVisibility(bool visible)
        {
            if (powerUpModel != null)
            {
                powerUpModel.SetActive(visible);
            }
            
            powerUpCollider.enabled = visible;
        }
        
        private void RespawnPowerUp()
        {
            isCollected = false;
            respawnTimer = 0f;
            SetPowerUpVisibility(true);
            transform.position = startPosition;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !isCollected)
            {
                CarController car = other.GetComponent<CarController>();
                if (car != null)
                {
                    Collect(car);
                }
            }
        }
        
        // Public methods for external control
        public void ForceRespawn()
        {
            RespawnPowerUp();
        }
        
        public void SetPowerUpType(PowerUpType newType)
        {
            powerUpType = newType;
        }
        
        public PowerUpType GetPowerUpType()
        {
            return powerUpType;
        }
        
        public bool IsCollected()
        {
            return isCollected;
        }
    }
}
