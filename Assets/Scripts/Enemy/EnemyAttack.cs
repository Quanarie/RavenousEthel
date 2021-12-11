using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float rechargeTime;

    public float attackDistance;

    protected float lastAttackTime;

    protected virtual void Update()
    {
        if (Vector3.Distance(PlayerIdentifier.Instance.transform.position, transform.position) <= attackDistance && !EnemyMovement.IsThereAWallOnTheWayToPlayer(transform.position))
        {
            if (Time.time - lastAttackTime < rechargeTime)
                return;

            Attack();
            lastAttackTime = Time.time;
        }
    }

    protected virtual void Attack() { }
}
