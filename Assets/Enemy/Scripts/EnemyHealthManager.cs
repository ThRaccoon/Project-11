using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyHealthManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private NavMeshAgent _navMeshAgent = null;
    [SerializeField] private Animator _animator;
    [SerializeField] private Enemy _enemy;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Hit Multipliers")]
    [SerializeField] private float _headMultiplier;
    [SerializeField] private float _bodyMultiplier;
    [SerializeField] private float _armsMultiplier;
    [SerializeField] private float _legsMultiplier;
    // ----------------------------------------------------------------------------------------------------------------------------------
    private void Awake()
    {
        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }

    public void TakeDamage(float baseDamage, string hitTag, GameObject attacker)
    {
        float finalDamage = baseDamage;

        switch (hitTag)
        {
            case "Head":
                finalDamage *= _headMultiplier;
                break;
            case "Body":
                finalDamage *= _bodyMultiplier;
                break;
            case "Arms":
                finalDamage *= _armsMultiplier;
                break;
            case "Legs":
                finalDamage *= _legsMultiplier;
                break;
            default:
                Debug.LogWarning("Unknown hit tag: " + hitTag);
                break;
        }

        if (attacker != null)
        {
            if (_enemy != null && attacker.CompareTag("Player"))
            {
                _enemy.ShouldChase = true;
            }
        }

        _health -= finalDamage;

        if (_health <= 0)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.isKinematic = false;
            }

            if (_boxCollider != null)
                _boxCollider.enabled = false;

            if (_navMeshAgent != null)
                _navMeshAgent.enabled = false;

            if (_animator != null)
                _animator.enabled = false;

            if (_enemy != null)
                _enemy.enabled = false;
        }
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }

    public float GetHealth()
    {
        return _health;
    }
}