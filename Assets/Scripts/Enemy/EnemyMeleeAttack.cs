using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float pushForce;
    [SerializeField] private float stunTime;
    [SerializeField] private AnimationClip attackClip;

    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected override void Attack()
    {
        animator.SetTrigger("attack");
        animator.SetFloat("playerPosX", (PlayerIdentifier.Instance.transform.position.x - transform.position.x) / Mathf.Abs(PlayerIdentifier.Instance.transform.position.x - transform.position.x));
        StartCoroutine(AttackAfterAnimation());
    }

    private IEnumerator AttackAfterAnimation()
    {
        yield return new WaitForSeconds(attackClip.length);
        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;
        Vector3 pushDirection = new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0).normalized * pushForce;
        PlayerIdentifier.Instance.Health.ReceiveDamage(damageAmount, pushDirection, stunTime);
    }
}