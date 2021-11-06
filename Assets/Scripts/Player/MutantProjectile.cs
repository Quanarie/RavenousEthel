using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantProjectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float range;
    [SerializeField] private float damageAmount;
    [SerializeField] private float lifetime;

    private Vector3 direction;

    private EnemyHealth enemyToAttack;

    private void Start()
    {
        Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);
        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            GameManager.Instance.playerAnimator.SetTrigger("attack");
            Destroy(gameObject, lifetime);
        }


        Vector3 playerPos = GameManager.Instance.Player.position;
        direction = new Vector3(enemyToAttack.position.x - playerPos.x, enemyToAttack.position.y - playerPos.y, 0);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemyToAttack))
        {
            enemyToAttack.ReceiveDamage(damageAmount);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
