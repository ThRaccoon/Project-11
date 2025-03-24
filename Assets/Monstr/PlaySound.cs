using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource m_audioSource;
    void Start()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void playSound()
    {
        m_audioSource.Play();
    }
}
