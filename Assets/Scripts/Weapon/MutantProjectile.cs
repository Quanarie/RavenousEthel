using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutantProjectile : Projectile
{
    private void Start()
    {
        GameManager.Instance.playerAnimator.SetFloat("enemyPosX", direction.x / Mathf.Abs(direction.x));
        GameManager.Instance.playerAnimator.SetTrigger("attack");
    }
}
