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
        void Start()
        {
            _camera = player.GetComponent<PlayerCamera>();
            _sensX = _camera.sensX;
        }
    
        void Update()
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;
            _yRotation += mouseX;

            transform.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
    }
}
