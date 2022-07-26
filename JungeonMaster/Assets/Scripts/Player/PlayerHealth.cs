using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth;
        public float currentHealth;
        private float _damage;
        public HealthBar healthBar;
        
        [Header("Fall")]
        public float fallDamageRatio;
        public float fallHeight;

        [Header("Health Regeneration")] 
        public bool regenerationActivation;
        public float regenerationFactor;
        public float regenerationCooldown;
        
        [Header("Player Movement")]
        [SerializeField] private GameObject player;
        private PlayerMovement _playerMovement;
        
        private Rigidbody _rb;
        
        // Getting component from another script
        private void Awake()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }

        void Start()
        {
            // Setting the current health value
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);
            
            // Starting coroutines for regeneration hp
            StartCoroutine(Regeneration());
            
            _rb = GetComponent<Rigidbody>();
        }
    
        void Update()
        {
            // Taking fall damage
            FallDamage();
        }

        private void FallDamage()
        {
            if (_rb.velocity.y < fallHeight && _playerMovement.grounded)
            {
                currentHealth -= Mathf.Abs(_rb.velocity.y) / fallDamageRatio;
                healthBar.SetHealth(currentHealth);
            }
        }

        private IEnumerator Regeneration()
        {
            while (true)
            {
                // Add hp, if hp regeneration on and current health is less than maximum health
                if (regenerationActivation && currentHealth < maxHealth)
                {
                    if (currentHealth + regenerationFactor < maxHealth)
                        currentHealth += regenerationFactor;
                    else
                        currentHealth = maxHealth;
                    
                    // Update health bar
                    healthBar.SetHealth(currentHealth);
                }
                
                // Cooldown
                yield return new WaitForSeconds(regenerationCooldown);
            }
        }
    }
}
