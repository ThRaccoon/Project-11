using UnityEngine;
using UnityEngine.AI;

public class EnemyHealthManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Enemy _enemy;

    [Space(10)]
    [SerializeField] private Rigidbody _headRb;
    [SerializeField] private Rigidbody _bodyRb;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Health")]
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

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private bool _shouldApplyKnockbackForce;
    // ----------------------------------------------------------------------------------------------------------------------------------

    private void Awake()
    {
        _shouldApplyKnockbackForce = true;

        foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
        {
            rb.isKinematic = true;
        }
    }

    public void TakeDamage(float baseDamage, float knockbackForce, string hitTag, GameObject attacker)
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

            if (_shouldApplyKnockbackForce)
            {
                Vector3 forceDir = (transform.position - attacker.transform.position).normalized;

                switch (hitTag)
                {
                    case "Head":
                        if (_headRb != null)
                        {
                            _headRb.AddForce(forceDir * knockbackForce, ForceMode.VelocityChange);
                        }
                        break;

                    case "Body":
                        if (_bodyRb != null)
                        {
                            _bodyRb.AddForce(forceDir * knockbackForce, ForceMode.VelocityChange);
                        }
                        break;

                    default:
                        break;
                }

                _shouldApplyKnockbackForce = false;
            }
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