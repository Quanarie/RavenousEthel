using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFinder : MonoBehaviour
{
    public static Transform FindClosestEnemyInRange(float range)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(new Vector2(PlayerIdentifier.Instance.transform.position.x, PlayerIdentifier.Instance.transform.position.y), range);

        int enemiesQuantity = 0;
        foreach (Collider2D item in items)
        {
            if (item.TryGetComponent(out EnemyHealth _))
                enemiesQuantity++;
        }

        Collider2D[] enemies = new Collider2D[enemiesQuantity];
        int enemyCounter = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].TryGetComponent(out EnemyHealth _))
            {
                enemies[enemyCounter] = items[i];
                enemyCounter++;
            }
        }

        if (enemies.Length == 0)
            return null;

        int closestEnemy = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(PlayerIdentifier.Instance.transform.position, enemies[i].transform.position) < Vector3.Distance(PlayerIdentifier.Instance.transform.position, enemies[closestEnemy].transform.position))
            {
                closestEnemy = i;
            }
        }

        return enemies[closestEnemy].transform;
    }
}
