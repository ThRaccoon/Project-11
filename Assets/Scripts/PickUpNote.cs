using TMPro;
using UnityEngine;

public class PickUpNote : MonoBehaviour, IInteractable
{
    [SerializeField, TextArea(3,20)] private string _description;
    private GameObject _player;
    private AudioSource _audioSource;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _audioSource = GetComponentInChildren<AudioSource>();
    }
    public void Interact()
    {
        var invetoryManager = _player.GetComponent<InventoryManager>();
        if (invetoryManager != null) 
        {
            invetoryManager.AddNote(_description);
        }

        DestroyAfterSound();
    }

    public void DestroyAfterSound()
    {
        if (_audioSource != null && _audioSource.clip != null)
        {
            // Detach audio so it survives after this GameObject is destroyed
            _audioSource.transform.parent = null;
            _audioSource.Play();

            // Destroy the audio source GameObject after it's done playing
            Destroy(_audioSource.gameObject, _audioSource.clip.length);
        }

        // Destroy the main GameObject right away
        Destroy(gameObject);
    }
}

