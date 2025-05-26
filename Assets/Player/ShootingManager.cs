using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShootingManager : MonoBehaviour
{
    [SerializeField] LayerMask _collisionLayerToIgnore;
    [SerializeField] GameObject _player;
    public bool _canShoot = true;
    public void Shoot(int damage)
    {
       if (Physics.Raycast(this.transform.position, this.transform.forward, out RaycastHit hit, 500, ~_collisionLayerToIgnore))
        {
           var enemyHM =  hit.collider.gameObject.GetComponentInParent<EnemyHealthManager>();
            if(enemyHM != null) 
            {
                enemyHM.TakeDamage(damage, hit.collider.tag, _player);
            }
        }
    }
}
