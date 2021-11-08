using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected float damageAmount;
    [SerializeField] protected float rechargeTime;

    public float attackDistance;

    protected float lastAttackTime;

    protected virtual void Update()
    {
        if (Vector3.Distance(GameManager.Instance.Player.position, transform.position) <= attackDistance)
        {
            if (Time.time - lastAttackTime < rechargeTime)
                return;

            Attack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void Attack() { }
}