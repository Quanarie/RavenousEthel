using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour, Weapon
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float rechargeTime;
    [SerializeField] protected float range;

    private float lastShootTime;

    public void Shoot()
    {
        if (Time.time - lastShootTime >= rechargeTime)
        {

            Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

            if (enemyToAttack == null)
                return;

            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();

            Vector3 playerPos = GameManager.Instance.Player.position;
            spawnedProjectile.direction = new Vector3(enemyToAttack.position.x - playerPos.x, enemyToAttack.position.y - playerPos.y, 0);

            lastShootTime = Time.time;
        }
    }
}
