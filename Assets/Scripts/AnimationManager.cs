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

    private string _currentAnim;
    #region Getters / Setters

    public string CurrentAnim
    {
        get => _currentAnim;
        set => _currentAnim = value;
    }
    #endregion


    public AnimationManager(Animator animator) 
    {
        _animator = animator;
    }

    public void PlayAnim(string animationName, float blendValue = 0.1f)
    {
        if (Util.IsNotNull(_animator))
        {
            if (_currentAnim != animationName)
            {
                _animator.CrossFade(animationName, blendValue);

                _currentAnim = animationName;
            }
        }
    }
}