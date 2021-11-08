using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : Weapon
{
    public override void Shoot()
    {
        if (Time.time - lastShootTime >= rechargeTime)
        {
            Vector3 playerPos = GameManager.Instance.Player.position;

            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            spawnedProjectile.direction = new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0);

            lastShootTime = Time.time;
        }
    }

    protected override void Update()
    {
        Vector3 playerPos = GameManager.Instance.Player.position;

        Vector3 weaponPos = transform.position;
        Vector3 weaponDir = new Vector3(playerPos.x - weaponPos.x, playerPos.y - weaponPos.y, 0);

        float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
        if (playerPos.y < transform.position.y) angleBetweenEnemyAndWeapon *= -1;

        transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);
    }
}
