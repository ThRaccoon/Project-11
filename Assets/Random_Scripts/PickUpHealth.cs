using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpHealth : MonoBehaviour
{

    [SerializeField] private int _heal = 20;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Health hpScript = other.GetComponent<Health>();
            if (hpScript != null)
            {
                if (hpScript.GetCurrentHealth() < hpScript.GetMaxHealth())
                {
                    hpScript.GainHealth(_heal);
                    Destroy(gameObject);
                }
            }
        }
    }
}
