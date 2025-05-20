using UnityEngine;

public class AnimationManager
{
    private Animator _animator;
    #region Getters / Setters
    public Animator Animator
    {
        get => _animator;
        set => _animator = value;
    }
    #endregion

    private GlobalTimer _waitBeforeAnimSwitchTimer;
    #region Getters / Setters
    public GlobalTimer WaitBeforeAnimSwitchTimer
    {
        get => _waitBeforeAnimSwitchTimer;
        set => _waitBeforeAnimSwitchTimer = value;
    }
    #endregion

    private float _animSwitchDuration;
    #region Getters / Setters
    public float AnimSwitchDuration
    {
        get => _animSwitchDuration;
        set => _animSwitchDuration = value;
    }
    #endregion

    private string _currentAnim;
    #region Getters / Setters
    public string CurrentAnim
    {
        get => _currentAnim;
        set => _currentAnim = value;
    }
    #endregion

    public AnimationManager(Animator animator, float waitBeforeAnimSwitchDuration = 0.05f)
    {
        _animator = animator;
        _animSwitchDuration = waitBeforeAnimSwitchDuration;

        _waitBeforeAnimSwitchTimer = new GlobalTimer(_animSwitchDuration);
    }

    public void PlayCrossFadeAnimation(string animationName, float blendValue = 0.2f, bool useTimer = true)
    {
        _waitBeforeAnimSwitchTimer.Tick();

        if (_waitBeforeAnimSwitchTimer.Flag || !useTimer)
        {
            if (Util.IsNotNull(_animator))
            {
                if (_currentAnim != animationName)
                {
                    _animator.CrossFade(animationName, blendValue);

                    _currentAnim = animationName;

                    _waitBeforeAnimSwitchTimer.Reset();
                }
            }
        }
    }
}