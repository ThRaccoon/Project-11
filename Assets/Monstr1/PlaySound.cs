using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private AudioSource _AudioSource;
    // ----------------------------------------------------------------------------------------------------------------------------------


    private void Awake()
    {
        _AudioSource = GetComponent<AudioSource>();
    }


    public void playSound()
    {
        if (NullChecker.Check(_AudioSource))
        {
            _AudioSource.Play();
        }
    }
}
