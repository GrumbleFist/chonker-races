using UnityEngine;
using UnityEngine.InputSystem;

namespace ChonkerRaces
{
    [RequireComponent(typeof(Rigidbody))]
    public class CarController : MonoBehaviour
    {
        [Header("Car Settings")]
        [SerializeField] private float motorForce = 1500f;
        [SerializeField] private float brakeForce = 3000f;
        [SerializeField] private float maxSteerAngle = 30f;
        [SerializeField] private float downForce = 100f;
        
        [Header("Wheels")]
        [SerializeField] private WheelCollider frontLeftWheelCollider;
        [SerializeField] private WheelCollider frontRightWheelCollider;
        [SerializeField] private WheelCollider rearLeftWheelCollider;
        [SerializeField] private WheelCollider rearRightWheelCollider;
        
        [SerializeField] private Transform frontLeftWheelTransform;
        [SerializeField] private Transform frontRightWheelTransform;
        [SerializeField] private Transform rearLeftWheelTransform;
        [SerializeField] private Transform rearRightWheelTransform;
        
        [Header("Damage System")]
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float damageMultiplier = 0.5f;
        private float currentHealth;
        private bool isDamaged = false;
        
        [Header("Power-ups")]
        [SerializeField] private float boostForce = 3000f;
        [SerializeField] private float boostDuration = 2f;
        private bool hasBoost = false;
        private float boostTimer = 0f;
        
        // Input
        private Vector2 inputVector;
        private bool isBraking;
        
        // Components
        private Rigidbody carRigidbody;
        private CarInputActions inputActions;
        
        // Properties
        public float CurrentHealth => currentHealth;
        public float MaxHealth => maxHealth;
        public bool IsDamaged => isDamaged;
        public bool HasBoost => hasBoost;
        public float Speed => carRigidbody.velocity.magnitude * 3.6f; // Convert to km/h
        
        private void Awake()
        {
            carRigidbody = GetComponent<Rigidbody>();
            inputActions = new CarInputActions();
            currentHealth = maxHealth;
            
            // Center of mass adjustment for better stability
            carRigidbody.centerOfMass = new Vector3(0, -0.5f, 0);
        }
        
        private void OnEnable()
        {
            inputActions.Enable();
        }
        
        private void OnDisable()
        {
            inputActions.Disable();
        }
        
        private void Start()
        {
            // Bind input actions
            inputActions.Car.Move.performed += OnMove;
            inputActions.Car.Move.canceled += OnMove;
            inputActions.Car.Brake.performed += OnBrake;
            inputActions.Car.Brake.canceled += OnBrake;
            inputActions.Car.Boost.performed += OnBoost;
        }
        
        private void Update()
        {
            HandleBoost();
            UpdateWheelPoses();
        }
        
        private void FixedUpdate()
        {
            HandleMotor();
            HandleSteering();
            AddDownForce();
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            inputVector = context.ReadValue<Vector2>();
        }
        
        private void OnBrake(InputAction.CallbackContext context)
        {
            isBraking = context.performed;
        }
        
        private void OnBoost(InputAction.CallbackContext context)
        {
            if (hasBoost)
            {
                ActivateBoost();
            }
        }
        
        private void HandleMotor()
        {
            float motor = motorForce * inputVector.y;
            
            // Apply damage penalty
            if (isDamaged)
            {
                motor *= damageMultiplier;
            }
            
            // Apply boost
            if (boostTimer > 0)
            {
                motor += boostForce;
            }
            
            frontLeftWheelCollider.motorTorque = motor;
            frontRightWheelCollider.motorTorque = motor;
            
            // Handle braking
            float brake = isBraking ? brakeForce : 0f;
            ApplyBraking(brake);
        }
        
        private void HandleSteering()
        {
            float steerAngle = maxSteerAngle * inputVector.x;
            frontLeftWheelCollider.steerAngle = steerAngle;
            frontRightWheelCollider.steerAngle = steerAngle;
        }
        
        private void ApplyBraking(float brake)
        {
            frontRightWheelCollider.brakeTorque = brake;
            frontLeftWheelCollider.brakeTorque = brake;
            rearLeftWheelCollider.brakeTorque = brake;
            rearRightWheelCollider.brakeTorque = brake;
        }
        
        private void AddDownForce()
        {
            carRigidbody.AddForce(-transform.up * downForce * carRigidbody.velocity.magnitude);
        }
        
        private void UpdateWheelPoses()
        {
            UpdateWheelPose(frontLeftWheelCollider, frontLeftWheelTransform);
            UpdateWheelPose(frontRightWheelCollider, frontRightWheelTransform);
            UpdateWheelPose(rearLeftWheelCollider, rearLeftWheelTransform);
            UpdateWheelPose(rearRightWheelCollider, rearRightWheelTransform);
        }
        
        private void UpdateWheelPose(WheelCollider wheelCollider, Transform wheelTransform)
        {
            Vector3 pos;
            Quaternion rot;
            wheelCollider.GetWorldPose(out pos, out rot);
            wheelTransform.rotation = rot;
            wheelTransform.position = pos;
        }
        
        private void HandleBoost()
        {
            if (boostTimer > 0)
            {
                boostTimer -= Time.deltaTime;
                if (boostTimer <= 0)
                {
                    boostTimer = 0;
                }
            }
        }
        
        public void TakeDamage(float damage)
        {
            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            
            if (currentHealth < maxHealth * 0.5f)
            {
                isDamaged = true;
            }
            
            if (currentHealth <= 0)
            {
                // Car is destroyed - could trigger respawn or elimination
                Debug.Log("Car destroyed!");
            }
        }
        
        public void RepairCar()
        {
            currentHealth = maxHealth;
            isDamaged = false;
        }
        
        public void CollectBoost()
        {
            hasBoost = true;
        }
        
        private void ActivateBoost()
        {
            if (hasBoost)
            {
                boostTimer = boostDuration;
                hasBoost = false;
            }
        }
        
        public void ResetCar()
        {
            currentHealth = maxHealth;
            isDamaged = false;
            hasBoost = false;
            boostTimer = 0f;
            carRigidbody.velocity = Vector3.zero;
            carRigidbody.angularVelocity = Vector3.zero;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PowerUp"))
            {
                PowerUp powerUp = other.GetComponent<PowerUp>();
                if (powerUp != null)
                {
                    powerUp.Collect(this);
                }
            }
            else if (other.CompareTag("DamageZone"))
            {
                DamageZone damageZone = other.GetComponent<DamageZone>();
                if (damageZone != null)
                {
                    damageZone.ApplyDamage(this);
                }
            }
        }
    }
}
