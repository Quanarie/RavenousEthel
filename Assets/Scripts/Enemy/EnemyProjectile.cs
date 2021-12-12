using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : Projectile
{
    private PlayerHealth playerHealth;
    private Vector3 pushDirection;

    protected override void Start()
    {
        base.Start();

        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;
        pushDirection = new Vector3(playerPos.x - transform.position.x, playerPos.y + Projectile.offsetY - transform.position.y, 0).normalized * pushForce;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out playerHealth))
        {
            playerHealth.ReceiveDamage(damageAmount, pushDirection, stunTime);
            
            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
