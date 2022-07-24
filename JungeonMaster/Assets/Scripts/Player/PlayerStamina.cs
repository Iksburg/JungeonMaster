using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Stamina")]
        public float maxStamina = 100.0f;
        public float currentStamina;
        public float sprintingStaminaDecrease = 0.1f;
        public float jumpingStaminaDecrease = 15.0f;
        public float decreasingCooldown = 0.01f;
        public StaminaBar staminaBar;
        private KeyCode _sprintKey;
        private KeyCode _jumpKey;
        public bool zeroStamina;
        public bool readyToJump = true;

        [Header("Stamina Regeneration")] 
        public bool regenerationActivation;
        public float regenerationFactor = 0.06f;
        public float regenerationCooldown = 0.01f;

        [Header("Player Movement")] 
        [SerializeField] private GameObject player;
        private PlayerMovement _playerMovement;

        [Header("Input")] 
        private float _horizontalInput;
        private float _verticalInput;

        private void Awake()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }
        
        void Start()
        {
            // Setting the current stamina value
            currentStamina = maxStamina;
            staminaBar.SetMaxStamina(maxStamina);
            
            StartCoroutine(StaminaDecreasing());
            StartCoroutine(StaminaRegeneration());
        }
    
        void Update()
        {
            ReadyToJump();
            
            _sprintKey = _playerMovement.sprintKey;
            _jumpKey = _playerMovement.jumpKey;

            _horizontalInput = Input.GetAxisRaw("Horizontal");
            _verticalInput = Input.GetAxisRaw("Vertical");
        }

        private void ReadyToJump()
        {
            readyToJump = !(currentStamina < jumpingStaminaDecrease);
        }

        private IEnumerator StaminaDecreasing()
        {
            while (true)
            {
                if (currentStamina >= jumpingStaminaDecrease)
                    readyToJump = true;
                
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
                
                if (Input.GetKey(_jumpKey) && _playerMovement.grounded && currentStamina > 0)
                {
                    if (currentStamina - jumpingStaminaDecrease > 0)
                    {
                        currentStamina -= jumpingStaminaDecrease;
                    }
                }
                
                staminaBar.SetStamina(currentStamina);
                yield return new WaitForSeconds(decreasingCooldown);
            }
        }

        private IEnumerator StaminaRegeneration()
        {
            while (true)
            {
                // Add stamina, if stamina regeneration on and current stamina is less than maximum stamina
                if (regenerationActivation && currentStamina < maxStamina && (!Input.GetKey(_sprintKey) || _horizontalInput == 0 && _verticalInput == 0))
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
    }
}
