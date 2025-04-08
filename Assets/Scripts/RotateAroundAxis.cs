using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private float _z;
    [SerializeField] private bool _shouldRotate = true;

    private void FixedUpdate()
    {
        if (_shouldRotate)
        {
            transform.RotateAround(transform.position, new Vector3(_x, _y, _z) , Time.deltaTime * _rotateSpeed);
        }
    }
}
