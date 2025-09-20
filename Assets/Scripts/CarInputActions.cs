using UnityEngine;
using UnityEngine.InputSystem;

namespace ChonkerRaces
{
    public class CarInputActions : MonoBehaviour
    {
        [Header("Input Actions")]
        public InputActionAsset inputActions;
        
        [Header("Input Settings")]
        [SerializeField] private bool useMobileControls = false;
        [SerializeField] private float mobileSensitivity = 1f;
        
        // Mobile UI references
        [Header("Mobile UI")]
        [SerializeField] private GameObject mobileControlsUI;
        [SerializeField] private UnityEngine.UI.Button accelerateButton;
        [SerializeField] private UnityEngine.UI.Button brakeButton;
        [SerializeField] private UnityEngine.UI.Button boostButton;
        [SerializeField] private UnityEngine.UI.Slider steeringSlider;
        
        private CarController carController;
        private Vector2 mobileInput;
        
        private void Awake()
        {
            carController = GetComponent<CarController>();
            
            // Detect platform and enable appropriate controls
            #if UNITY_ANDROID || UNITY_IOS
            useMobileControls = true;
            #endif
            
            SetupMobileControls();
        }
        
        private void Start()
        {
            if (useMobileControls)
            {
                EnableMobileControls();
            }
            else
            {
                EnableDesktopControls();
            }
        }
        
        private void SetupMobileControls()
        {
            if (mobileControlsUI != null)
            {
                mobileControlsUI.SetActive(useMobileControls);
            }
            
            if (useMobileControls)
            {
                // Setup mobile button events
                if (accelerateButton != null)
                {
                    accelerateButton.onClick.AddListener(() => SetMobileInput(Vector2.up));
                }
                
                if (brakeButton != null)
                {
                    brakeButton.onClick.AddListener(() => SetMobileInput(Vector2.down));
                }
                
                if (boostButton != null)
                {
                    boostButton.onClick.AddListener(() => TriggerBoost());
                }
                
                if (steeringSlider != null)
                {
                    steeringSlider.onValueChanged.AddListener(OnSteeringChanged);
                }
            }
        }
        
        private void EnableMobileControls()
        {
            if (mobileControlsUI != null)
            {
                mobileControlsUI.SetActive(true);
            }
            
            // Disable desktop input
            if (inputActions != null)
            {
                inputActions.Disable();
            }
        }
        
        private void EnableDesktopControls()
        {
            if (mobileControlsUI != null)
            {
                mobileControlsUI.SetActive(false);
            }
            
            // Enable desktop input
            if (inputActions != null)
            {
                inputActions.Enable();
            }
        }
        
        private void SetMobileInput(Vector2 input)
        {
            mobileInput = input;
        }
        
        private void OnSteeringChanged(float value)
        {
            mobileInput.x = (value - 0.5f) * 2f; // Convert 0-1 to -1 to 1
        }
        
        private void TriggerBoost()
        {
            // This would be handled by the CarController's boost system
            if (carController != null && carController.HasBoost)
            {
                // Trigger boost through input system
                var boostAction = inputActions?.FindAction("Boost");
                boostAction?.Trigger();
            }
        }
        
        public void SwitchToMobileControls()
        {
            useMobileControls = true;
            EnableMobileControls();
        }
        
        public void SwitchToDesktopControls()
        {
            useMobileControls = false;
            EnableDesktopControls();
        }
        
        private void OnDestroy()
        {
            if (inputActions != null)
            {
                inputActions.Disable();
            }
        }
    }
}
