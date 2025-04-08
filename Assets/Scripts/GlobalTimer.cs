using UnityEngine;

public class GlobalTimer
{
    private float _duration = 0.0f;
    private float _elapsedTime = 0.0f;

    private float _accumulatedTime = 0.0f;

    private int _maxIterations = 0;
    private int _iterations = 0;

    public bool flag { get; private set; } = false;



    public GlobalTimer(float duration, int iterations = 0, bool flag = false)
    {
        _duration = duration;
        _elapsedTime = duration;

        _maxIterations = iterations;
    }

    public bool CountDownTimer()
    {
        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            flag = true;
            return true;
        }
        return false;
    }

    public bool ReversedCountDownTimer()
    {
        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            flag = false;
            return false;
        }
        return true;
    }

    public bool IntervalTimer()
    {
        if (_maxIterations <= 0)
        {
            return false;
        }

        _accumulatedTime += Time.deltaTime;

        while (_accumulatedTime > 0 && _iterations < _maxIterations)
        {
            _elapsedTime -= _duration;
            _accumulatedTime -= _duration;
            _iterations++;

            if (_elapsedTime <= 0)
            {
                Reset();
                flag = true;
                return true;
            }

            if (_iterations >= _maxIterations)
            {
                Reset();
                flag = false;
            }
        }
        return false;
    }

    public void Reset()
    {
        _elapsedTime = _duration;
        _iterations = 0;
        _accumulatedTime = 0.0f;
        flag = false;
    }


    // Getters / Setters:
    public void SetDuration(float duration)
    {
        _duration = duration;
    }

    public void SetFlag(bool newFlag)
    {
        flag = newFlag;
    }
}
