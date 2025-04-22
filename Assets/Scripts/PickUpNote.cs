using TMPro;
using UnityEngine;

public class PickUpNote : MonoBehaviour, IInteractable
{
    [SerializeField, TextArea(3,20)] private string _description;
    private GameObject _player;
    private void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
    }
    public void Interact()
    {
        var invetoryManager = _player.GetComponent<InventoryManager>();
        if (invetoryManager != null) 
        {
            invetoryManager.AddNote(_description);
        }

        Destroy(gameObject);
    }

}

