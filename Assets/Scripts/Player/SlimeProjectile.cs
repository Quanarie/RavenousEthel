using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfLerping;
    [SerializeField] private float distanceToMutate;

    private Transform enemyToAttack;

    private void Start()
    {
        enemyToAttack = FindClosestEnemy();
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, enemyToAttack.position, speedOfLerping);

        if (Vector3.Distance(transform.position, enemyToAttack.position) < distanceToMutate)
        {
            //GameManager.Instance.Player.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }

    private Transform FindClosestEnemy()
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

        return closestEnemy;
    }
}
