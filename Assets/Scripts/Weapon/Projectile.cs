using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float damageAmount;
    [SerializeField] protected float pushForce;
    [SerializeField] protected float stunTime;
    [SerializeField] protected float speed;
    [SerializeField] protected float lifetime;

    [HideInInspector] public Vector3 direction;
    public static float offsetY = 0.175f;

    private EnemyHealth enemyToAttack;

    protected virtual void Update()
    {
        transform.Translate(new Vector3(direction.x, direction.y + offsetY, 0).normalized * speed * Time.deltaTime);

        Destroy(gameObject, lifetime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemyToAttack))
        {
            Vector3 enemyPos = enemyToAttack.transform.position;
            Vector3 playerPos = GameManager.Instance.Player.position;

            enemyToAttack.ReceiveDamage(damageAmount, new Vector3(enemyPos.x - playerPos.x, enemyPos.y - playerPos.y, 0).normalized * pushForce, stunTime);

            Destroy(gameObject);
        }

        print(collision.gameObject.layer);
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
