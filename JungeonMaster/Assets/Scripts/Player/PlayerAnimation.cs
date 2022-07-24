using System;
using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        [Header("Player Camera")]
        public GameObject player;
        private PlayerCamera _playerCamera;
        
        [Header("Rotation")]
        private float _yRotation;
        private float _sensX;
        private float _xRotation;
        private float _speed;
        
        [Header("Animator")]
        private Rigidbody _rb;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        private void Awake()
        {
            _playerCamera = player.GetComponent<PlayerCamera>();
        }

        void Start()
        {
            _sensX = _playerCamera.sensX;
            _rb = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
        }
    
        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
            _yRotation += mouseX;

            transform.rotation = Quaternion.Euler(0, _yRotation, 0);
            
            _speed = _rb.velocity.magnitude;
            _animator.SetFloat(Speed, _speed);
        }
    }
}
