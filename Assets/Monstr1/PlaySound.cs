using UnityEngine;

public class PlaySound : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private AudioSource m_audioSource;
    void Awake()
    {
        m_audioSource = GetComponent<AudioSource>();
    }

    public void playSound()
    {
        if(m_audioSource != null)
        {
            m_audioSource.Play();
        }
       
    }
}
