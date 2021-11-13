using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDistanceAttack : EnemyAttack
{
    public EnemyWeapon weapon;

    protected override void Attack()
    {
        if (weapon != null)
            weapon.Shoot();
    }
}
