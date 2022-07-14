using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public Transform orientation;
    
    public GameObject player;
    private PlayerCamera _camera;
    
    private float _yRotation;
    private float _sensX;
    void Start()
    {
        _camera = player.GetComponent<PlayerCamera>();
        _sensX = _camera.sensX;
    }
    
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * _sensX;

        _yRotation = mouseX;
        Vector3 rotation = new Vector3(0, _yRotation, 0);
        transform.Rotate(rotation);
    }
}
