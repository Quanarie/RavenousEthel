using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectile;
    public float chanceOfDropping;

    public void Shoot()
    {
        Vector3 playerPos = GameManager.Instance.Player.position;

        Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        spawnedProjectile.direction = new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0);
    }

    private void Update()
    {
        Vector3 playerPos = GameManager.Instance.Player.position;

        if (playerPos.x < transform.position.x)
            transform.localScale = new Vector3(1, -1, 0);
        else
            transform.localScale = new Vector3(1, 1, 0);

        Vector3 weaponPos = transform.position;
        Vector3 weaponDir = new Vector3(playerPos.x - weaponPos.x, playerPos.y - weaponPos.y + Projectile.offsetY, 0);

        float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
        if (playerPos.y < transform.position.y) angleBetweenEnemyAndWeapon *= -1;

        transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);
    }
}
