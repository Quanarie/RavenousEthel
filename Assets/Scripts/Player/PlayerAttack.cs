using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float rechargeTime;

    private float previousAttackTime;

    public void Attack()
    {
        if (Time.time - previousAttackTime < rechargeTime)
            return;

        Animator animator = GameManager.Instance.playerAnimator;

        animator.SetTrigger("attack");
        animator.SetTrigger("death");

        previousAttackTime = Time.time;
    }
}
