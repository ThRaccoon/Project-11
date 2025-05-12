using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class EnemyHealthManager : MonoBehaviour
{
    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Components")]
    [SerializeField] private BoxCollider _boxCollider;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Animator _animator;
    [SerializeField] private Enemy _enemy;
    // ----------------------------------------------------------------------------------------------------------------------------------

    // ----------------------------------------------------------------------------------------------------------------------------------
    [Header("Settings")]
    [SerializeField] private float _health;
    [SerializeField] private float _maxHealth;

    [SerializeField] private float _headMultiplier;
    [SerializeField] private float _bodyMultiplier;
    [SerializeField] private float _armsMultiplier;
    [SerializeField] private float _legsMultiplier;
    // ----------------------------------------------------------------------------------------------------------------------------------

    [SerializeField] private bool _kill = false; // Debug
    [SerializeField] private bool _killed = false; // Debug

    private void Awake()
    {
        // --- Components ---
        _boxCollider = GetComponent<BoxCollider>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _enemy = GetComponent<Enemy>();
    }

    private void Update() // Debug
    {
        if (_kill == true && _killed == false)
        {
            TakeDamage(1, "Head", gameObject);
            _killed = true;
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

        if (Util.IsNotNull(attacker))
        {
            if (attacker.CompareTag("Player"))
            {
                _enemy.ShouldAttack = true;
            }
        }

        _health -= finalDamage;

        if (_health <= 0)
        {
            foreach (Rigidbody rb in GetComponentsInChildren<Rigidbody>())
            {
                rb.linearVelocity = Vector3.zero;
            }

            if (Util.IsNotNull(_boxCollider))
                _boxCollider.enabled = false;

            if (Util.IsNotNull(_navMeshAgent))
                _navMeshAgent.enabled = false;

            if (Util.IsNotNull(_animator))
                _animator.enabled = false;

            if (Util.IsNotNull(_enemy))
                _enemy.enabled = false;
        }

        _killed = true;
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