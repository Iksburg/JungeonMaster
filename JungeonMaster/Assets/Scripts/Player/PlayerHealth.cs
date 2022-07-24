using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health")]
        public float maxHealth = 100.0f;
        public float currentHealth;
        private float _damage;
        public HealthBar healthBar;
        
        [Header("Fall")]
        public float fallDamageRatio = 15.0f;
        public float fallHeight = -11.0f;

        [Header("Health Regeneration")] 
        public bool regenerationActivation;
        public float regenerationFactor = 0.1f;
        public float regenerationCooldown = 1.0f;
        
        [Header("Player Movement")]
        [SerializeField] private GameObject player;
        private PlayerMovement _playerMovement;
        
        private Rigidbody _rb;

        private void Awake()
        {
            _playerMovement = player.GetComponent<PlayerMovement>();
        }

        void Start()
        {
            // Setting the current health value
            currentHealth = maxHealth;
            healthBar.SetMaxHealth(maxHealth);

            StartCoroutine(Regeneration());
            
            _rb = GetComponent<Rigidbody>();
        }
    
        void Update()
        {
            // Taking fall damage
            if (_rb.velocity.y < fallHeight && _playerMovement.grounded)
            {
                _damage = Mathf.Abs(_rb.velocity.y) / fallDamageRatio;
                TakeDamage(_damage);
            }
        }

        private void TakeDamage(float damage)
        {
            currentHealth -= damage;
            
            // Update health bar
            healthBar.SetHealth(currentHealth);
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
