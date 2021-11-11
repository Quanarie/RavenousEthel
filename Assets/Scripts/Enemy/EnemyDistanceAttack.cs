using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistanceAttack : EnemyAttack
{
    [SerializeField] private EnemyWeapon weapon;

    protected override void Attack()
    {
        if (weapon != null)
            weapon.Shoot();
    }
}
