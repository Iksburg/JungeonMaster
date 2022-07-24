using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerStamina : MonoBehaviour
    {
        [Header("Stamina")]
        public float maxStamina = 100.0f;
        public float currentStamina;
        public float staminaDecrease = 1.0f;
        public float decreasingCooldown = 0.05f;
        public StaminaBar staminaBar;
        private KeyCode _sprintKey;
        public bool zeroStamina;

        [Header("Stamina Regeneration")] 
        public bool regenerationActivation;
        public float regenerationFactor = 0.1f;
        public float regenerationCooldown = 0.1f;

        [Header("Player Movement")] 
        [SerializeField] private GameObject player;
        private PlayerMovement _playerMovement;
        
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
            _sprintKey = _playerMovement.sprintKey;
        }
        
        private void Awake()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }

        private IEnumerator StaminaDecreasing()
        {
            while (true)
            {
                if (Input.GetKey(_sprintKey) && currentStamina > 0)
                {
                    if (currentStamina - staminaDecrease > 0)
                    {
                        currentStamina -= staminaDecrease;
                    }
                    else
                    {
                        currentStamina = 0;
                        zeroStamina = true;
                    }

                    staminaBar.SetStamina(currentStamina);
                }
                
                yield return new WaitForSeconds(decreasingCooldown);
            }
        }

        private IEnumerator StaminaRegeneration()
        {
            while (true)
            {
                // Add stamina, if stamina regeneration on and current stamina is less than maximum stamina
                if (regenerationActivation && currentStamina < maxStamina && !Input.GetKey(_sprintKey))
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
