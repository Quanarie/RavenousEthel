using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfLerping;
    [SerializeField] private float distanceToMutate;
    [SerializeField] private float range;

    private Transform enemyToAttack;

    private void Start()
    {
        enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }

        GameManager.Instance.playerAnimator.SetTrigger("attack");
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, enemyToAttack.position, speedOfLerping);

        if (Vector3.Distance(transform.position, enemyToAttack.position) < distanceToMutate)
        {
            GameManager.Instance.playerAttack.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }
}
