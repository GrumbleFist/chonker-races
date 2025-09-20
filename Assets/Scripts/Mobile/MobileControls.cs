using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ChonkerRaces.Mobile
{
    public class MobileControls : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        [Header("Control Elements")]
        [SerializeField] private Button accelerateButton;
        [SerializeField] private Button brakeButton;
        [SerializeField] private Button boostButton;
        [SerializeField] private Button handbrakeButton;
        [SerializeField] private Slider steeringSlider;
        [SerializeField] private RectTransform steeringArea;
        
        [Header("Steering Settings")]
        [SerializeField] private float steeringSensitivity = 2f;
        [SerializeField] private bool useSteeringWheel = true;
        [SerializeField] private bool useTiltSteering = false;
        
        [Header("Visual Feedback")]
        [SerializeField] private Image accelerateButtonImage;
        [SerializeField] private Image brakeButtonImage;
        [SerializeField] private Image boostButtonImage;
        [SerializeField] private Color pressedColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;
        
        private CarController carController;
        private Vector2 inputVector;
        private bool isAccelerating = false;
        private bool isBraking = false;
        private bool isBoosting = false;
        private bool isHandbraking = false;
        
        private void Start()
        {
            carController = FindObjectOfType<CarController>();
            
            if (carController == null)
            {
                Debug.LogError("CarController not found! Mobile controls will not work.");
                return;
            }
            
            SetupMobileControls();
        }
        
        private void Update()
        {
            UpdateInput();
            UpdateVisualFeedback();
            
            if (useTiltSteering)
            {
                HandleTiltSteering();
            }
        }
        
        private void SetupMobileControls()
        {
            // Setup button events
            if (accelerateButton != null)
            {
                accelerateButton.onClick.AddListener(() => SetAccelerate(true));
            }
            
            if (brakeButton != null)
            {
                brakeButton.onClick.AddListener(() => SetBrake(true));
            }
            
            if (boostButton != null)
            {
                boostButton.onClick.AddListener(() => SetBoost(true));
            }
            
            if (handbrakeButton != null)
            {
                handbrakeButton.onClick.AddListener(() => SetHandbrake(true));
            }
            
            // Setup steering slider
            if (steeringSlider != null)
            {
                steeringSlider.onValueChanged.AddListener(OnSteeringChanged);
            }
            
            // Setup steering area for touch steering
            if (steeringArea != null && useSteeringWheel)
            {
                // Add event trigger for steering area
                EventTrigger trigger = steeringArea.gameObject.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = steeringArea.gameObject.AddComponent<EventTrigger>();
                }
                
                // Add pointer down event
                EventTrigger.Entry pointerDown = new EventTrigger.Entry();
                pointerDown.eventID = EventTriggerType.PointerDown;
                pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
                trigger.triggers.Add(pointerDown);
                
                // Add pointer up event
                EventTrigger.Entry pointerUp = new EventTrigger.Entry();
                pointerUp.eventID = EventTriggerType.PointerUp;
                pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(pointerUp);
                
                // Add drag event
                EventTrigger.Entry drag = new EventTrigger.Entry();
                drag.eventID = EventTriggerType.Drag;
                drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
                trigger.triggers.Add(drag);
            }
        }
        
        private void UpdateInput()
        {
            // Create input vector based on current state
            inputVector = Vector2.zero;
            
            if (isAccelerating)
            {
                inputVector.y = 1f;
            }
            else if (isBraking)
            {
                inputVector.y = -1f;
            }
            
            // Steering is handled by steering slider or touch steering
            if (steeringSlider != null)
            {
                inputVector.x = (steeringSlider.value - 0.5f) * 2f; // Convert 0-1 to -1 to 1
            }
            
            // Send input to car controller
            if (carController != null)
            {
                // This would need to be implemented in CarController to accept mobile input
                // For now, we'll use the existing input system
                SendInputToCar();
            }
        }
        
        private void SendInputToCar()
        {
            // This method would need to be implemented in CarController
            // to accept mobile input directly
            // For now, we'll simulate keyboard input
            
            if (isAccelerating)
            {
                // Simulate W key press
                InputSystem.QueueKeyboardEvent(Key.W, true);
            }
            else
            {
                InputSystem.QueueKeyboardEvent(Key.W, false);
            }
            
            if (isBraking)
            {
                // Simulate S key press
                InputSystem.QueueKeyboardEvent(Key.S, true);
            }
            else
            {
                InputSystem.QueueKeyboardEvent(Key.S, false);
            }
            
            if (isBoosting)
            {
                // Simulate Shift key press
                InputSystem.QueueKeyboardEvent(Key.LeftShift, true);
            }
            else
            {
                InputSystem.QueueKeyboardEvent(Key.LeftShift, false);
            }
            
            if (isHandbraking)
            {
                // Simulate Space key press
                InputSystem.QueueKeyboardEvent(Key.Space, true);
            }
            else
            {
                InputSystem.QueueKeyboardEvent(Key.Space, false);
            }
            
            // Handle steering
            if (inputVector.x > 0.1f)
            {
                InputSystem.QueueKeyboardEvent(Key.D, true);
                InputSystem.QueueKeyboardEvent(Key.A, false);
            }
            else if (inputVector.x < -0.1f)
            {
                InputSystem.QueueKeyboardEvent(Key.A, true);
                InputSystem.QueueKeyboardEvent(Key.D, false);
            }
            else
            {
                InputSystem.QueueKeyboardEvent(Key.A, false);
                InputSystem.QueueKeyboardEvent(Key.D, false);
            }
        }
        
        private void UpdateVisualFeedback()
        {
            // Update button colors based on state
            if (accelerateButtonImage != null)
            {
                accelerateButtonImage.color = isAccelerating ? pressedColor : normalColor;
            }
            
            if (brakeButtonImage != null)
            {
                brakeButtonImage.color = isBraking ? pressedColor : normalColor;
            }
            
            if (boostButtonImage != null)
            {
                boostButtonImage.color = isBoosting ? pressedColor : normalColor;
            }
        }
        
        private void HandleTiltSteering()
        {
            // Get device orientation for tilt steering
            Vector3 acceleration = Input.acceleration;
            float tiltX = acceleration.x;
            
            // Convert tilt to steering input
            inputVector.x = Mathf.Clamp(tiltX * steeringSensitivity, -1f, 1f);
        }
        
        // Button event handlers
        private void SetAccelerate(bool state)
        {
            isAccelerating = state;
        }
        
        private void SetBrake(bool state)
        {
            isBraking = state;
        }
        
        private void SetBoost(bool state)
        {
            isBoosting = state;
        }
        
        private void SetHandbrake(bool state)
        {
            isHandbraking = state;
        }
        
        private void OnSteeringChanged(float value)
        {
            // Steering is handled in UpdateInput
        }
        
        // Touch steering event handlers
        public void OnPointerDown(PointerEventData eventData)
        {
            if (useSteeringWheel && steeringArea != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    steeringArea, eventData.position, eventData.pressEventCamera, out localPoint);
                
                // Calculate steering value based on touch position
                float steeringValue = localPoint.x / steeringArea.rect.width;
                steeringValue = Mathf.Clamp(steeringValue, 0f, 1f);
                
                if (steeringSlider != null)
                {
                    steeringSlider.value = steeringValue;
                }
            }
        }
        
        public void OnPointerUp(PointerEventData eventData)
        {
            // Reset steering to center
            if (steeringSlider != null)
            {
                steeringSlider.value = 0.5f;
            }
        }
        
        public void OnDrag(PointerEventData eventData)
        {
            if (useSteeringWheel && steeringArea != null)
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    steeringArea, eventData.position, eventData.pressEventCamera, out localPoint);
                
                // Calculate steering value based on drag position
                float steeringValue = localPoint.x / steeringArea.rect.width;
                steeringValue = Mathf.Clamp(steeringValue, 0f, 1f);
                
                if (steeringSlider != null)
                {
                    steeringSlider.value = steeringValue;
                }
            }
        }
        
        // Public methods for external control
        public void SetSteeringSensitivity(float sensitivity)
        {
            steeringSensitivity = sensitivity;
        }
        
        public void SetUseSteeringWheel(bool use)
        {
            useSteeringWheel = use;
        }
        
        public void SetUseTiltSteering(bool use)
        {
            useTiltSteering = use;
        }
        
        public void ResetControls()
        {
            isAccelerating = false;
            isBraking = false;
            isBoosting = false;
            isHandbraking = false;
            
            if (steeringSlider != null)
            {
                steeringSlider.value = 0.5f;
            }
        }
        
        // Getters
        public Vector2 GetInputVector()
        {
            return inputVector;
        }
        
        public bool IsAccelerating()
        {
            return isAccelerating;
        }
        
        public bool IsBraking()
        {
            return isBraking;
        }
        
        public bool IsBoosting()
        {
            return isBoosting;
        }
        
        public bool IsHandbraking()
        {
            return isHandbraking;
        }
    }
}
