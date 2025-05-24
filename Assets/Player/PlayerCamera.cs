using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private Transform _cameraHolder;
    [SerializeField] private Transform _cameraRotation;
    [SerializeField] private PlayerInput _playerInput;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Space(30)]
    [Header("Settings")]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private Vector2 _verticalCap;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // --- Private Variables ---
    private bool _isRotationEnable = true;
    private float _rotationX;
    private float _rotationY;

    private void Update()
    {
        if (_isRotationEnable)
        {
            if (_playerInput != null)
            {
                _rotationX -= _playerInput.rotationInput.y * _mouseSensitivity * Time.deltaTime;
                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);

                _rotationY += _playerInput.rotationInput.x * _mouseSensitivity * Time.deltaTime;
                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);


                Quaternion xRotation = Quaternion.AngleAxis(_rotationX, Vector3.right);
                Quaternion yRotation = Quaternion.AngleAxis(_rotationY, Vector3.up);

                transform.rotation = yRotation * xRotation;

                if (_cameraRotation != null)
                {
                    _cameraRotation.transform.rotation = yRotation;
                }
            }
        }

        if (_cameraHolder != null)
        {
            transform.position = _cameraHolder.position;
        }
    }


    public void EnableRotation()
    {
        _isRotationEnable = true;
    }

    public void DisableRotation()
    {
        _isRotationEnable = false;
    }
}