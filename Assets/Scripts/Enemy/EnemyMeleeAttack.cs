using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [SerializeField] private float damageAmount;
    [SerializeField] private AnimationClip attackClip;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Attack()
    {
        animator.SetTrigger("attack");
        animator.SetFloat("playerPosX", (GameManager.Instance.Player.position.x - transform.position.x) / Mathf.Abs(GameManager.Instance.Player.position.x - transform.position.x));
        StartCoroutine(AttackAfterAnimation());
    }

    private IEnumerator AttackAfterAnimation()
    {
        yield return new WaitForSeconds(attackClip.length);
        GameManager.Instance.playerHealth.ReceiveDamage(damageAmount);
    }
}