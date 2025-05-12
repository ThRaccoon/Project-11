using UnityEngine;

public class GlobalTimer
{
    private bool _flag = false;
    #region Getter / Setter
    public bool Flag
    {
        get => _flag;
        set => _flag = value;
    }
    #endregion

    private float _duration = 0f;
    #region Getter / Setter
    public float Duration
    {
        get => _duration;
        set => _duration = value;
    }
    #endregion

    private float _elapsedTime = 0f;
    #region Getter / Setter
    public float ElapsedTime
    {
        get => _elapsedTime;
        set => _elapsedTime = value;
    }
    #endregion

    public GlobalTimer(float duration, bool flag = false)
    {
        _duration = duration;
        _elapsedTime = duration;
    }

    public bool CountDownTimer()
    {
        if (_flag)
        {
            return _flag;
        }

        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            _flag = true;
            return _flag;
        }
        return _flag;
    }

    public bool ReversedCountDownTimer()
    {
        if (!_flag)
        {
            return _flag;
        }

        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            _flag = false;
            return _flag;
        }
        return _flag;
    }

    public void Reset()
    {
        _elapsedTime = _duration;
        _flag = false;
    }

    public void ReversedReset()
    {
        _elapsedTime = _duration;
        _flag = true;
    }
}