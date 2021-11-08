using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float rechargeTime;
    [SerializeField] private AnimationClip attackClip;

    private float lastAttackTime;
    private new Rigidbody2D rigidbody;
    private Animator animator;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        rigidbody.WakeUp();
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            if (Time.time - lastAttackTime < rechargeTime)
                return;

            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
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