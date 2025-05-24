using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private EWeaponType _EWeaponType = 0;
    // ----------------------------------------------------------------------------------------------------------------------------------

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            InventoryManager inventoryManager = other.GetComponent<InventoryManager>();
            
            if (inventoryManager != null)
            {
                inventoryManager.OnWeaponFound(_EWeaponType);
                Destroy(gameObject); 
            }
        }
    }
}