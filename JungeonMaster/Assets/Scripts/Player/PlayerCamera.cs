using UnityEngine;

namespace Player
{
    public class PlayerCamera : MonoBehaviour
    {
        [Header("Sensitivity")]
        public float sensX;
        public float sensY;
        
        [Header("Orientation")]
        public Transform orientation;

        private float _xRotation;
        private float _yRotation;
        void Start()
        {
            //
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            // Get mouse input
            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensX;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensY;

            _yRotation += mouseX;

            _xRotation -= mouseY;
            _xRotation = Mathf.Clamp(_xRotation, -80f, 70f);
        
            // Rotate cam and orientation
            transform.rotation = Quaternion.Euler(_xRotation, _yRotation, 0);
            orientation.rotation = Quaternion.Euler(0, _yRotation, 0);
        }
    }
}