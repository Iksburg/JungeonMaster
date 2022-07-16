using UnityEngine;

namespace Player
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;
    
        void Update()
        {
            transform.position = cameraPosition.position;
        }
    }
}
