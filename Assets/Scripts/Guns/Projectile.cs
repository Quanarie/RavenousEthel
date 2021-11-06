using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float damageAmount;
    [SerializeField] protected float rechargeTime;
    [SerializeField] protected float pushForce;
    [SerializeField] protected float stunTime;
    [SerializeField] protected float speed;
    [SerializeField] protected float range;
    [SerializeField] protected float lifetime;

    protected Vector3 direction;
    protected EnemyHealth enemyToAttack;

    protected virtual void Start()
    {
        Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Destroy(gameObject, lifetime);
        }

        Vector3 playerPos = GameManager.Instance.Player.position;

        direction = new Vector3(enemyToAttack.position.x - playerPos.x, enemyToAttack.position.y - playerPos.y, 0);
    }

    protected virtual void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
            return;

        if (collision.TryGetComponent(out enemyToAttack))
        {
            Vector3 enemyPos = enemyToAttack.transform.position;
            Vector3 playerPos = GameManager.Instance.Player.position;

            enemyToAttack.ReceiveDamage(damageAmount, new Vector3(enemyPos.x - playerPos.x, enemyPos.y - playerPos.y, 0).normalized * pushForce, stunTime);
        }

        Destroy(gameObject);
    }
}
