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


    private void Awake()
    {
        _cameraHolder = GameObject.FindGameObjectWithTag("CameraHolder").transform;
        _cameraRotation = GameObject.FindGameObjectWithTag("CameraRotation").transform;
        _playerInput = GameObject.FindGameObjectWithTag("PlayerInput").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_isRotationEnable)
        {
            if (Util.IsNotNull(_playerInput))
            {
                _rotationX -= _playerInput.rotationInput.y * _mouseSensitivity * Time.deltaTime;
                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);

                _rotationY += _playerInput.rotationInput.x * _mouseSensitivity * Time.deltaTime;
                _rotationX = Mathf.Clamp(_rotationX, _verticalCap.x, _verticalCap.y);


                Quaternion xRotation = Quaternion.AngleAxis(_rotationX, Vector3.right);
                Quaternion yRotation = Quaternion.AngleAxis(_rotationY, Vector3.up);

                transform.rotation = yRotation * xRotation;

                if (Util.IsNotNull(_cameraRotation))
                {
                    _cameraRotation.transform.rotation = yRotation;
                }
            }
        }
        
        if (Util.IsNotNull(_cameraHolder))
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