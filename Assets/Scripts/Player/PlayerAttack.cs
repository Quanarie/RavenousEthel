using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    [SerializeField] private float rechargeTimeSlime;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject deadSlimePrefab;

    private float previousSlimeAttackTime;

    public void Attack()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
        {
            if (Time.time - previousSlimeAttackTime < rechargeTimeSlime)
                return;

            Animator animator = GameManager.Instance.playerAnimator;

            animator.SetTrigger("attack");
            Instantiate(projectilePrefab, transform.position, Quaternion.identity);

            previousSlimeAttackTime = Time.time;
        }
        else if (GameManager.Instance.state == GameManager.State.mutated)
        {

        }
    }

    public void Mutate(GameObject enemy)
    {
        GameManager.Instance.state = GameManager.State.mutated;

        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        transform.position = enemy.transform.position;

        Destroy(enemy);
    }

    public void DeMutate()
    {
        GameManager.Instance.state = GameManager.State.regular;
    }
}
