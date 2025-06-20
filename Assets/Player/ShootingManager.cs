using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] LayerMask _collisionLayersToIgnore;


    public void Shoot(float damage, float force)
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 500, ~_collisionLayersToIgnore))
        {
            var enemyHealthManager = hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();

            if (enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(damage, force, hit.collider.tag, _player);
            }
        }
    }
}
