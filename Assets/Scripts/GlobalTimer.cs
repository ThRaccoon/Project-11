using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTimer
{
    private float _duration = 0.0f;
    private float _elapsedTime = 0.0f;
    
    public bool flag { get; private set; } = false;


    public GlobalTimer(float duration, bool flag = false)
    {
        _duration = duration;
        _elapsedTime = duration;
    }

    public bool CountDownTimer()
    {
        if (flag) 
        {
            return flag;
        }
        
        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            flag = true;
            return flag;
        }
        return flag;
    }

    public bool ReversedCountDownTimer()
    {
        if (!flag) 
        {
            return flag;
        }
        
        _elapsedTime -= Time.deltaTime;

        if (_elapsedTime <= 0)
        {
            flag = false;
            return flag;
        }
        return flag;
    }

    public void Reset()
    {
        _elapsedTime = _duration;
        flag = false;
    }

    public void ReversedReset() 
    {
        _elapsedTime = _duration;
        flag = true;
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
