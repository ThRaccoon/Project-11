using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private int _health = 100;
    [SerializeField] private int _maxHealth = 100;
    // ----------------------------------------------------------------------------------------------------------------------------------

    public GameObject Attacker = null;


    public void TakeDamage(int damage, GameObject attacker)
    {
        _health -= damage;
        Attacker = attacker;

        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void GainHealth(int health)
    {
        _health += health;

        if (_health > _maxHealth)
        {
            _health = _maxHealth;
        }
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetCurrentHealth()
    {
        return _health;
    }
}
