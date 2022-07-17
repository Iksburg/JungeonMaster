using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        public GameObject player;
        private PlayerCamera _camera;
    
        private float _yRotation;
        private float _sensX;
        private float _xRotation;
        private float _speed;
        private Rigidbody _rb;
        private Animator _animator;
        private static readonly int Speed = Animator.StringToHash("Speed");

        void Start()
        {
            _camera = player.GetComponent<PlayerCamera>();
            _sensX = _camera.sensX;
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
