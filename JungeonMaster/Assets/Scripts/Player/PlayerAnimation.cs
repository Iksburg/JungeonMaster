using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Player Camera")]
        [SerializeField] private GameObject playerCamera;
        [SerializeField] private GameObject player;
        private PlayerCamera _playerCamera;
        private PlayerMovement _playerMovement;
        
        [Header("Rotation")]
        private float _yRotation;
        private float _sensX;
        private float _xRotation;

        [Header("Movement")] 
        private float _walkSpeed;
        private float _sprintSpeed;
        private float _playerSpeed;
        private float _animatorSpeed;
        
        [Header("Animator")]
        private Rigidbody _rb;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");
        private const float IdleThreshold = 0;
        private const float WalkThreshold = 0.5f;
        private const float SprintThreshold = 1;

        // Getting component from another script
        private void Awake()
        {
            _playerCamera = playerCamera.GetComponent<PlayerCamera>();
            _playerMovement = player.GetComponent<PlayerMovement>();
        }

        void Start()
        {
            _sensX = _playerCamera.sensX;
            _walkSpeed = _playerMovement.walkSpeed;
            _sprintSpeed = _playerMovement.sprintSpeed;
            
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }
    
        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
            _yRotation += mouseX;

            transform.rotation = Quaternion.Euler(0, _yRotation, 0);

            var velocity = _rb.velocity;
            _playerSpeed = Mathf.Sqrt(velocity.z * velocity.z + velocity.x * velocity.x);

            // Converting coordinates
            if (_playerSpeed == 0)
            {
                _animatorSpeed = 0;
            }
            else if (_playerSpeed <= _walkSpeed)
            {
                _animatorSpeed = (_playerSpeed - IdleThreshold) * (WalkThreshold - IdleThreshold) / _walkSpeed;
            }
            else if (_playerSpeed <= _sprintSpeed)
            {
                _animatorSpeed =
                    (_playerSpeed - _walkSpeed) * (SprintThreshold - WalkThreshold) / (_sprintSpeed - _walkSpeed) +
                    WalkThreshold;
            }

            _animator.SetFloat(Speed, _animatorSpeed);
        }
    }
}
