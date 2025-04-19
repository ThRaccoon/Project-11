using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [Header("Auto Assigned")]
    [SerializeField] private AudioSource _AudioSource = null;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }


    public void playSound()
    {
        if (_AudioSource != null)
        {
            _AudioSource.Play();
        }
    }
}
