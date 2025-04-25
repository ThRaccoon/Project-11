using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }


    public void playSound()
    {
        if (Util.IsNotNull(_audioSource))
        {
            _audioSource.Play();
        }
    }
}