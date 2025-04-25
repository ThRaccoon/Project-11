using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private PlayerInput _playerInput;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private float _bobSpeed;
    [SerializeField] private float _smooth;
    [SerializeField] private float _positiveZOffset;
    // ----------------------------------------------------------------------------------------------------------------------------------


    // --- Private Variables ---
    private float _sineTime;
    private float _sineResult;
    private float _weaponBobStartPosition;
    private float _weaponBobEndPosition;
    private float _scaledSineResult;
    private Vector3 _targetPosition;


    private void Awake()
    {
        _playerInput = GameObject.Find("PlayerInput").GetComponent<PlayerInput>();

        _weaponBobStartPosition = transform.localPosition.z;
        _weaponBobEndPosition = _weaponBobStartPosition + _positiveZOffset;

        _scaledSineResult = _weaponBobStartPosition;
    }

    private void Update()
    {
        if (Util.IsNotNull(_playerInput) && _playerInput.movementInput != Vector2.zero)
        {
            _sineTime += _bobSpeed * Time.deltaTime;
            _sineResult = Mathf.Sin(_sineTime);
            _scaledSineResult = MapValueInRange(_sineResult, -1, 1, _weaponBobStartPosition, _weaponBobEndPosition);
        }
        else
        {
            if (_scaledSineResult > _weaponBobStartPosition)
            {
                _scaledSineResult -= _smooth * Time.deltaTime;

                if (_scaledSineResult < _weaponBobStartPosition)
                {
                    _scaledSineResult = _weaponBobStartPosition;
                }
            }
            _sineTime = 0.0f;
        }

        _targetPosition.Set(transform.localPosition.x, transform.localPosition.y, _scaledSineResult);
        transform.localPosition = _targetPosition;
    }


    private float MapValueInRange(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        return (value - fromMin) / (fromMax - fromMin) * (toMax - toMin) + toMin;
    }
}