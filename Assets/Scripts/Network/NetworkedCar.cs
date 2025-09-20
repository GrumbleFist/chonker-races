using UnityEngine;
using Mirror;

namespace ChonkerRaces.Networking
{
    [RequireComponent(typeof(CarController))]
    public class NetworkedCar : NetworkBehaviour
    {
        [Header("Network Settings")]
        [SerializeField] private float sendRate = 20f;
        [SerializeField] private float positionThreshold = 0.1f;
        [SerializeField] private float rotationThreshold = 1f;
        
        [Header("Smoothing")]
        [SerializeField] private float lerpSpeed = 10f;
        [SerializeField] private float rotationLerpSpeed = 10f;
        
        // Networked variables
        [SyncVar] private Vector3 networkPosition;
        [SyncVar] private Quaternion networkRotation;
        [SyncVar] private Vector3 networkVelocity;
        [SyncVar] private float networkHealth;
        [SyncVar] private bool networkHasBoost;
        
        // Components
        private CarController carController;
        private Rigidbody carRigidbody;
        
        // Interpolation
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 targetVelocity;
        
        // Last sent values for delta compression
        private Vector3 lastSentPosition;
        private Quaternion lastSentRotation;
        private Vector3 lastSentVelocity;
        private float lastSentHealth;
        private bool lastSentHasBoost;
        
        private float lastSendTime;
        
        public override void OnStartAuthority()
        {
            carController = GetComponent<CarController>();
            carRigidbody = GetComponent<Rigidbody>();
            
            // Initialize network values
            networkPosition = transform.position;
            networkRotation = transform.rotation;
            networkVelocity = carRigidbody.velocity;
            networkHealth = carController.CurrentHealth;
            networkHasBoost = carController.HasBoost;
            
            // Set initial target values
            targetPosition = networkPosition;
            targetRotation = networkRotation;
            targetVelocity = networkVelocity;
        }
        
        private void Update()
        {
            if (hasAuthority)
            {
                // Send updates if we're the authority
                SendNetworkUpdates();
            }
            else
            {
                // Interpolate to network values if we're not the authority
                InterpolateToNetworkValues();
            }
        }
        
        private void SendNetworkUpdates()
        {
            if (Time.time - lastSendTime < 1f / sendRate)
                return;
            
            bool shouldSend = false;
            
            // Check if position changed significantly
            if (Vector3.Distance(transform.position, lastSentPosition) > positionThreshold)
            {
                shouldSend = true;
            }
            
            // Check if rotation changed significantly
            if (Quaternion.Angle(transform.rotation, lastSentRotation) > rotationThreshold)
            {
                shouldSend = true;
            }
            
            // Check if velocity changed significantly
            if (Vector3.Distance(carRigidbody.velocity, lastSentVelocity) > 0.5f)
            {
                shouldSend = true;
            }
            
            // Check if health changed
            if (Mathf.Abs(carController.CurrentHealth - lastSentHealth) > 0.1f)
            {
                shouldSend = true;
            }
            
            // Check if boost status changed
            if (carController.HasBoost != lastSentHasBoost)
            {
                shouldSend = true;
            }
            
            if (shouldSend)
            {
                CmdUpdateCarState(transform.position, transform.rotation, carRigidbody.velocity, 
                                carController.CurrentHealth, carController.HasBoost);
                
                // Update last sent values
                lastSentPosition = transform.position;
                lastSentRotation = transform.rotation;
                lastSentVelocity = carRigidbody.velocity;
                lastSentHealth = carController.CurrentHealth;
                lastSentHasBoost = carController.HasBoost;
                
                lastSendTime = Time.time;
            }
        }
        
        [Command]
        private void CmdUpdateCarState(Vector3 position, Quaternion rotation, Vector3 velocity, 
                                     float health, bool hasBoost)
        {
            networkPosition = position;
            networkRotation = rotation;
            networkVelocity = velocity;
            networkHealth = health;
            networkHasBoost = hasBoost;
        }
        
        private void InterpolateToNetworkValues()
        {
            // Smoothly interpolate position
            targetPosition = networkPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, lerpSpeed * Time.deltaTime);
            
            // Smoothly interpolate rotation
            targetRotation = networkRotation;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationLerpSpeed * Time.deltaTime);
            
            // Apply velocity
            targetVelocity = networkVelocity;
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, targetVelocity, lerpSpeed * Time.deltaTime);
            
            // Update health and boost status (these are handled by the CarController)
            // The CarController will sync these values through the network
        }
        
        // Network callbacks for when sync vars change
        private void OnNetworkPositionChanged(Vector3 oldValue, Vector3 newValue)
        {
            if (!hasAuthority)
            {
                targetPosition = newValue;
            }
        }
        
        private void OnNetworkRotationChanged(Quaternion oldValue, Quaternion newValue)
        {
            if (!hasAuthority)
            {
                targetRotation = newValue;
            }
        }
        
        private void OnNetworkVelocityChanged(Vector3 oldValue, Vector3 newValue)
        {
            if (!hasAuthority)
            {
                targetVelocity = newValue;
            }
        }
        
        private void OnNetworkHealthChanged(float oldValue, float newValue)
        {
            // Health changes are handled by the CarController
            // This is just for logging or additional effects
        }
        
        private void OnNetworkHasBoostChanged(bool oldValue, bool newValue)
        {
            // Boost status changes are handled by the CarController
            // This is just for logging or additional effects
        }
        
        // Public methods for external access
        public bool IsLocalPlayer()
        {
            return hasAuthority;
        }
        
        public Vector3 GetNetworkPosition()
        {
            return networkPosition;
        }
        
        public Quaternion GetNetworkRotation()
        {
            return networkRotation;
        }
        
        public Vector3 GetNetworkVelocity()
        {
            return networkVelocity;
        }
        
        public float GetNetworkHealth()
        {
            return networkHealth;
        }
        
        public bool GetNetworkHasBoost()
        {
            return networkHasBoost;
        }
        
        // RPC for special events
        [ClientRpc]
        public void RpcPlayEffect(string effectName, Vector3 position)
        {
            // Play visual/audio effects on all clients
            Debug.Log($"Playing effect: {effectName} at {position}");
            
            // You can implement specific effect logic here
            // For example, spawning particle systems, playing sounds, etc.
        }
        
        [Command]
        public void CmdCollectPowerUp(int powerUpType)
        {
            // Handle power-up collection on server
            Debug.Log($"Player collected power-up type: {powerUpType}");
            
            // Broadcast to all clients
            RpcPlayEffect("PowerUpCollect", transform.position);
        }
        
        [Command]
        public void CmdTakeDamage(float damage)
        {
            // Handle damage on server
            Debug.Log($"Player took {damage} damage");
            
            // Broadcast damage effect to all clients
            RpcPlayEffect("Damage", transform.position);
        }
    }
}
