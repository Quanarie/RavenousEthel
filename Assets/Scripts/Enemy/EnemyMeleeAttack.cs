using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float rechargeTime;

    private float lastAttackTime;
    private new Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rigidbody.WakeUp();
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            if (Time.time - lastAttackTime < rechargeTime)
                return;

            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        GameManager.Instance.playerHealth.ReceiveDamage(damageAmount);
    }
}
