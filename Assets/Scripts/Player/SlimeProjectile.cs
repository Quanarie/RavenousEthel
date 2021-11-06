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
        enemyToAttack = FindClosestEnemyInRange();
    }

    private void Update()
    {
        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }

        transform.position = Vector3.Lerp(transform.position, enemyToAttack.position, speedOfLerping);

        if (Vector3.Distance(transform.position, enemyToAttack.position) < distanceToMutate)
        {
            GameManager.Instance.playerAttack.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }

    private Transform FindClosestEnemyInRange()
    {
        Transform closestEnemy = GameManager.Instance.EnemiesParent.GetChild(0);
        Vector3 playerPos = GameManager.Instance.Player.position;

        foreach (Transform enemy in GameManager.Instance.EnemiesParent.transform)
        {
            if (Vector3.Distance(enemy.position, playerPos) < Vector3.Distance(closestEnemy.position, playerPos))
            {
                closestEnemy = enemy;
            }
        }

        if (Vector3.Distance(closestEnemy.position, playerPos) <= range)
        {
            return closestEnemy;
        }
        return null;
    }
}
