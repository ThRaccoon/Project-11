using TMPro;
using UnityEngine;

public class InteractionPopUp : MonoBehaviour
{
    [Header("Components")]
    private Camera _mainCamera;
    [SerializeField] private float _activationRange = 0;
    private TextMeshPro _textMeshPro;
   

    void Awake()
    {
        _mainCamera = Camera.main;
        _textMeshPro = GetComponent<TextMeshPro>();
       
        if(_textMeshPro != null)
        {
            Color color = _textMeshPro.color;
            color.a = 0f;
            _textMeshPro.color = color;
        }
    }

    void LateUpdate()
    {
        float distance = (_mainCamera.transform.position - transform.position).sqrMagnitude;

        if (_textMeshPro != null)
        {
            float alpha = Mathf.Lerp(1, 0, distance / _activationRange);
           
            Color color = _textMeshPro.color;
            color.a = alpha;
            _textMeshPro.color = color;
        }
        Quaternion rotation = _mainCamera.transform.rotation;
        transform.LookAt(transform.position + rotation * Vector3.forward, rotation * Vector3.up);
    }
}
