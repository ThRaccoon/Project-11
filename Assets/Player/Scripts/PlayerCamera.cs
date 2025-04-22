using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [Header("----------")]
    [SerializeField] private Transform _cameraHolder = null;
    [SerializeField] private Transform _cameraRotation = null;
    [SerializeField] private PlayerInput _playerInput = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private float _mouseSensitivity = 0.0f;
    [SerializeField] private Vector2 _verticalCap = Vector2.zero;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Private Variables ---
    private bool _isRotationEnable = true;
    private float _rotationX = 0.0f;
    private float _rotationY = 0.0f;
    
    private void Update()
    {

        if (_isRotationEnable)
        { 
            if (NullChecker.Check(_playerInput))

            {
                if (_playerInput != null)
                {
                    _rotationX -= _playerInput.RotationInput.y * _mouseSensitivity * Time.deltaTime;
                }
                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);

                if (_playerInput != null)
                {
                    _rotationY += _playerInput.RotationInput.x * _mouseSensitivity * Time.deltaTime;
                }


                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);

                if (NullChecker.Check(_playerInput))
                {
                    _rotationY += _playerInput.RotationInput.x * _mouseSensitivity * Time.deltaTime;
                }

                Quaternion xRotation = Quaternion.AngleAxis(_rotationX, Vector3.right);
                Quaternion yRotation = Quaternion.AngleAxis(_rotationY, Vector3.up);

                transform.rotation = yRotation * xRotation;

                if (NullChecker.Check(_cameraRotation))
                {
                    _cameraRotation.transform.rotation = yRotation;
                }


            }

        }

        if (NullChecker.Check(_cameraHolder))
        {
            transform.position = _cameraHolder.position;
        }


    }

    public void EnableRotate()
    {
        _isRotationEnable = true;
    }

    public void DisableRotate() 
    {
        _isRotationEnable = false;
    }
        
    
}