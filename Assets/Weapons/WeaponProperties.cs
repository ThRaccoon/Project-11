using UnityEngine;

public class WeaponProperties : MonoBehaviour
{
    [SerializeField] private Animator[] _animators;
    [SerializeField] private AudioClip _audioClip;
    
    [Tooltip("Maximum distance the bullet can travel.")]
    [SerializeField] private float _shootingDistance = 100f;
    [Tooltip("Delay between shots.")]
    [SerializeField] private float _shootCooldownTime = 0.5f;
    [SerializeField] private int _damage = 25;


    public Animator[] GetAnimators() 
    {
        return _animators; 
    }
    
    public AudioClip GetAudioClip()
    { 
        return _audioClip; 
    }

    public float GetShootDistance()
    {
        return _shootingDistance;
    }

    public float GetShootCooldownTime() 
    {  
        return _shootCooldownTime; 
    }

    public int GetDamage()
    {
        return _damage;
    }
}
