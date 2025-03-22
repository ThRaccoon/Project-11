using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Transform _CameraHolder = null;
    [SerializeField] private Transform _CameraRotation = null;
    [SerializeField] private PlayerInput _PlayerInput = null;


    [Header("Settings")]
    [SerializeField] private float _mouseSensitivity = 0.0f;
    [SerializeField] private Vector2 _verticalCap = Vector2.zero;
     

    // --- Private Variables ---
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;



    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (_PlayerInput != null)
        {
            _rotationX -= _PlayerInput.RotationInput.y * _mouseSensitivity * Time.deltaTime;
        }
        _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);

        if (_PlayerInput != null)
        {
            _rotationY += _PlayerInput.RotationInput.x * _mouseSensitivity * Time.deltaTime;
        }
        
        Quaternion xRotation = Quaternion.AngleAxis(_rotationX, Vector3.right);
        Quaternion yRotation = Quaternion.AngleAxis(_rotationY, Vector3.up);

        transform.rotation = yRotation * xRotation;

        if (_CameraHolder != null)
        {
            transform.position = _CameraHolder.position;
        }

        if (_CameraRotation != null)
        {
            _CameraRotation.transform.rotation = yRotation;
        }
    }
}
