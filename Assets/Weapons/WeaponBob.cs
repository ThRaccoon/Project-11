using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private PlayerInput _PlayerInput;


    [Header("Settings")]
    [SerializeField] private float _bobSpeed = 2.0f;
    [SerializeField] private float _smooth = 0.1f;
    [SerializeField] private float _positiveZOffset = 0.05f;


    // --- Private Variables ---
    private float _sineTime = 0.0f;
    private float _sineResult = 0.0f;
    private float _weaponBobStartPosition = 0.0f;
    private float _weaponBobEndPosition = 0.0f;
    private float _scaledSineResult = 0.0f;
    private Vector3 _targetPosition = Vector3.zero;
    private GameObject _playerInput = null;



    private void Awake()
    {
        GetPlayerInput();

        _weaponBobStartPosition = transform.localPosition.z;
        _weaponBobEndPosition = _weaponBobStartPosition + _positiveZOffset;

        _scaledSineResult = _weaponBobStartPosition;
    }

    private void Update()
    {
        if (_PlayerInput.movementInput != null && _PlayerInput.movementInput != Vector2.zero)
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

    private void GetPlayerInput()
    {
        _playerInput = GameObject.Find("PlayerInput");
        
        if (_playerInput != null)
        {
            _PlayerInput = _playerInput.GetComponent<PlayerInput>();
        }
    }
}

