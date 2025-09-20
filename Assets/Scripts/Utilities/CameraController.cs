using UnityEngine;

namespace ChonkerRaces.Utilities
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
        [SerializeField] private float followSpeed = 10f;
        [SerializeField] private float rotationSpeed = 5f;
        [SerializeField] private float lookAheadDistance = 5f;
        [SerializeField] private float lookAheadSpeed = 2f;
        
        [Header("Camera Modes")]
        [SerializeField] private CameraMode currentMode = CameraMode.ThirdPerson;
        [SerializeField] private bool smoothFollow = true;
        [SerializeField] private bool lookAtTarget = true;
        
        [Header("Third Person Settings")]
        [SerializeField] private float thirdPersonDistance = 10f;
        [SerializeField] private float thirdPersonHeight = 5f;
        [SerializeField] private float thirdPersonAngle = 15f;
        
        [Header("First Person Settings")]
        [SerializeField] private Vector3 firstPersonOffset = new Vector3(0, 1.5f, 0);
        
        [Header("Cinematic Settings")]
        [SerializeField] private float cinematicDistance = 15f;
        [SerializeField] private float cinematicHeight = 8f;
        [SerializeField] private float cinematicAngle = 25f;
        
        private Vector3 currentOffset;
        private Vector3 targetPosition;
        private Quaternion targetRotation;
        private Vector3 lookAheadPosition;
        private Rigidbody targetRigidbody;
        
        public enum CameraMode
        {
            ThirdPerson,
            FirstPerson,
            Cinematic,
            Free
        }
        
        private void Start()
        {
            if (target != null)
            {
                targetRigidbody = target.GetComponent<Rigidbody>();
            }
            
            // Set initial offset based on current mode
            UpdateOffsetForMode();
        }
        
        private void LateUpdate()
        {
            if (target == null) return;
            
            UpdateLookAhead();
            UpdateCameraPosition();
            UpdateCameraRotation();
        }
        
        private void UpdateLookAhead()
        {
            if (targetRigidbody != null)
            {
                // Calculate look ahead position based on velocity
                Vector3 velocity = targetRigidbody.velocity;
                velocity.y = 0; // Ignore vertical velocity
                
                lookAheadPosition = target.position + velocity.normalized * lookAheadDistance;
            }
            else
            {
                lookAheadPosition = target.position;
            }
        }
        
        private void UpdateCameraPosition()
        {
            // Calculate target position
            targetPosition = target.position + currentOffset;
            
            // Apply look ahead
            if (lookAheadDistance > 0)
            {
                Vector3 lookAheadOffset = (lookAheadPosition - target.position) * 0.3f;
                targetPosition += lookAheadOffset;
            }
            
            // Move camera to target position
            if (smoothFollow)
            {
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = targetPosition;
            }
        }
        
        private void UpdateCameraRotation()
        {
            if (lookAtTarget)
            {
                // Calculate target rotation to look at target
                Vector3 lookDirection = target.position - transform.position;
                lookDirection.y = 0; // Keep camera level
                
                if (lookDirection != Vector3.zero)
                {
                    targetRotation = Quaternion.LookRotation(lookDirection);
                }
                
                // Apply rotation
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        private void UpdateOffsetForMode()
        {
            switch (currentMode)
            {
                case CameraMode.ThirdPerson:
                    currentOffset = new Vector3(0, thirdPersonHeight, -thirdPersonDistance);
                    break;
                    
                case CameraMode.FirstPerson:
                    currentOffset = firstPersonOffset;
                    break;
                    
                case CameraMode.Cinematic:
                    currentOffset = new Vector3(0, cinematicHeight, -cinematicDistance);
                    break;
                    
                case CameraMode.Free:
                    currentOffset = offset;
                    break;
            }
        }
        
        // Public methods for external control
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
            if (target != null)
            {
                targetRigidbody = target.GetComponent<Rigidbody>();
            }
        }
        
        public void SetCameraMode(CameraMode mode)
        {
            currentMode = mode;
            UpdateOffsetForMode();
        }
        
        public void SetOffset(Vector3 newOffset)
        {
            offset = newOffset;
            if (currentMode == CameraMode.Free)
            {
                currentOffset = offset;
            }
        }
        
        public void SetFollowSpeed(float speed)
        {
            followSpeed = speed;
        }
        
        public void SetRotationSpeed(float speed)
        {
            rotationSpeed = speed;
        }
        
        public void SetLookAheadDistance(float distance)
        {
            lookAheadDistance = distance;
        }
        
        public void SetSmoothFollow(bool smooth)
        {
            smoothFollow = smooth;
        }
        
        public void SetLookAtTarget(bool look)
        {
            lookAtTarget = look;
        }
        
        // Camera mode specific settings
        public void SetThirdPersonSettings(float distance, float height, float angle)
        {
            thirdPersonDistance = distance;
            thirdPersonHeight = height;
            thirdPersonAngle = angle;
            
            if (currentMode == CameraMode.ThirdPerson)
            {
                UpdateOffsetForMode();
            }
        }
        
        public void SetFirstPersonOffset(Vector3 offset)
        {
            firstPersonOffset = offset;
            
            if (currentMode == CameraMode.FirstPerson)
            {
                UpdateOffsetForMode();
            }
        }
        
        public void SetCinematicSettings(float distance, float height, float angle)
        {
            cinematicDistance = distance;
            cinematicHeight = height;
            cinematicAngle = angle;
            
            if (currentMode == CameraMode.Cinematic)
            {
                UpdateOffsetForMode();
            }
        }
        
        // Utility methods
        public void SnapToTarget()
        {
            if (target != null)
            {
                transform.position = target.position + currentOffset;
                transform.LookAt(target);
            }
        }
        
        public void ResetCamera()
        {
            currentMode = CameraMode.ThirdPerson;
            UpdateOffsetForMode();
            SnapToTarget();
        }
        
        // Getters
        public Transform GetTarget()
        {
            return target;
        }
        
        public CameraMode GetCurrentMode()
        {
            return currentMode;
        }
        
        public Vector3 GetCurrentOffset()
        {
            return currentOffset;
        }
    }
}
