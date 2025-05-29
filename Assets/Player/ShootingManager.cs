using UnityEngine;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] LayerMask _collisionLayerToIgnore;


    public void Shoot(int damage)
    {
        if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, 500, ~_collisionLayerToIgnore))
        {
            var enemyHealthManager = hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();

            if (enemyHealthManager != null)
            {
                enemyHealthManager.TakeDamage(damage, hit.collider.tag, _player);
            }
        }
    }
}
