using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSlimeAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float rechargeTime;
    [SerializeField] private GameObject projectilePrefab;

    private float previousAttackTime;

    public void Attack()
    {
        if (Time.time - previousAttackTime < rechargeTime)
            return;

        Animator animator = GameManager.Instance.playerAnimator;

        animator.SetTrigger("attack");
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);

        previousAttackTime = Time.time;
    }
}
