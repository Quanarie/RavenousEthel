using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistanceAttack : EnemyAttack
{
    [SerializeField] private Weapon weapon;

    protected override void Attack()
    {
        weapon.Shoot();
    }
}
