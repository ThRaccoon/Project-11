using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class TutorialPopup : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _textMeshPro;
    [SerializeField] private string _text;
    [SerializeField] private float _duration;

    private GlobalTimer _timer;
    private bool _shouldShowPopup = false;

    private void Awake()
    {
        _timer = new GlobalTimer(_duration);
    }


    // Update is called once per frame
    void Update()
    {
        if (_shouldShowPopup)
        {
            _textMeshPro.text = _text;
            _timer.CountDownTimer();
        }
       
        if (_timer.flag)
        {
            _textMeshPro.text = "";
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _timer.Reset();
            _shouldShowPopup = true;
        }
    }


}
