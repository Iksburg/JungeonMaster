using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Stamina")]
        public float maxStamina;
        public float currentStamina;
        public float sprintingStaminaDecrease;
        public float jumpingStaminaDecrease;
        public float decreasingCooldown;
        public StaminaBar staminaBar;
        
        [Header("Stamina Regeneration")] 
        public bool regenerationActivation;
        public float regenerationFactor;
        public float regenerationCooldown;
        
        [Header("KeyBinds")]
        private KeyCode _sprintKey;
        private KeyCode _jumpKey;
        
        [Header("Restrictions")]
        public bool zeroStamina;
        public bool enoughStaminaToJump = true;
        
        [Header("Jump")]
        private float _jumpCooldown;
        private bool _readyToJump = true;

        [Header("Player Movement")] 
        [SerializeField] private GameObject player;
        private PlayerMovement _playerMovement;

        [Header("Input")] 
        private float _horizontalInput;
        private float _verticalInput;
        
        // Getting component from another script
        private void Awake()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }
        
        void Start()
        {
            // Setting the current stamina value
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            
            // Starting coroutines for decrease and regeneration stamina
            StartCoroutine(StaminaDecreasing());
            StartCoroutine(StaminaRegeneration());
        }
    
        void Update()
        {
            // Function, that checking that player have enough stamina to jump
            ReadyToJump();
            
            // Set and update variables from another script
            _sprintKey = _playerMovement.sprintKey;
            _jumpKey = _playerMovement.jumpKey;
            _jumpCooldown = _playerMovement.jumpCooldown;
            
            // Variables to check player movement
            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void ReadyToJump()
        {
            enoughStaminaToJump = !(currentStamina < jumpingStaminaDecrease);
        }

        private IEnumerator StaminaDecreasing()
        {
            while (true)
            {
                // Reduce stamina while sprinting. If it is not enough, turn off sprinting
                if (Input.GetKey(_sprintKey) && currentStamina > 0 && (_horizontalInput != 0 || _verticalInput != 0))
                {
                    if (currentStamina - sprintingStaminaDecrease > 0)
                    {
                        currentStamina -= sprintingStaminaDecrease;
                    }
                    else
                    {
                        currentStamina = 0;
                        zeroStamina = true;
                    }
                }
                
                // Reduce stamina while jumping. If it is not enough, disable the ability to jump
                if (_readyToJump && Input.GetKey(_jumpKey) && _playerMovement.grounded && currentStamina > 0)
                {
                    _readyToJump = false;
                    
                    if (currentStamina - jumpingStaminaDecrease > 0)
                    {
                        currentStamina -= jumpingStaminaDecrease;
                    }
                    
                    Invoke(nameof(ResetJump), _jumpCooldown);
                }
                
                staminaBar.SetStamina(currentStamina);
                
                // Cooldown
                yield return new WaitForSeconds(decreasingCooldown);
            }
        }

        private IEnumerator StaminaRegeneration()
        {
            while (true)
            {
                // Add stamina, if stamina regeneration on and current stamina is less than maximum stamina
                if (regenerationActivation && currentStamina < maxStamina &&
                    (!Input.GetKey(_sprintKey) || _horizontalInput == 0 && _verticalInput == 0))
                {
                    if (currentStamina + regenerationFactor < maxStamina)
                    {
                        currentStamina += regenerationFactor;
                        zeroStamina = false;
                    }
                    else
                    {
                        currentStamina = maxStamina;
                    }

                    // Update stamina bar
                    staminaBar.SetStamina(currentStamina);
                }
                
                // Cooldown
                yield return new WaitForSeconds(regenerationCooldown);
            }
        }

        private void ResetJump()
        {
            _readyToJump = true;
        }
    }
}
