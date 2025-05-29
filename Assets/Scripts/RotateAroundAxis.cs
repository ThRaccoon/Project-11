using UnityEngine;

public class RotateAroundAxis : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private bool _shouldRotate = true;
    [SerializeField] private float _rotateSpeed;
    [SerializeField] private float _x;
    [SerializeField] private float _y;
    [SerializeField] private float _z;
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Update()
    {
        if (_shouldRotate)
        {
            transform.RotateAround(transform.position, new Vector3(_x, _y, _z), Time.deltaTime * _rotateSpeed);
        }
    }
}