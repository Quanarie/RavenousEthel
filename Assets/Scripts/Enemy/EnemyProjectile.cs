using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private PlayerHealth playerHealth;

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out playerHealth))
        {
            Vector3 playerPos = GameManager.Instance.Player.position;

            playerHealth.ReceiveDamage(damageAmount, new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0).normalized * pushForce, stunTime);
            
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
