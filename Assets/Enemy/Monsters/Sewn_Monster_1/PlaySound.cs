using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private AudioSource _audioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------

    public void playSound()
    {
        if (_audioSource != null)
        {
            _audioSource.Play();
        }
    }
}