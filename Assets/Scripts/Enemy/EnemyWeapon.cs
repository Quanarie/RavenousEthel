using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] private string weaponSoundName;
    public float chanceOfDropping;

    [SerializeField] private float damageAmount;
    [SerializeField] private float pushForce;
    [SerializeField] private float stunTime;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private Sprite projectileSprite;

    public void Shoot()
    {
        AudioManager.Instance.Play(weaponSoundName);

        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;

        EnemyProjectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<EnemyProjectile>();
        spawnedProjectile.direction = new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y + Projectile.offsetY, 0);
        spawnedProjectile.angleBetweenEnemyAndWeapon = transform.localRotation.eulerAngles.z;
        spawnedProjectile.Construct(damageAmount, pushForce, stunTime, speed, lifetime, projectileSprite);
    }

    private void Update()
    {
        if (EnemyMovement.IsThereAWallOnTheWayToPlayer(transform.position))
            return;

        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;

        if (playerPos.x < transform.position.x)
            transform.localScale = new Vector3(1, -1, 0);
        else
            transform.localScale = new Vector3(1, 1, 0);

        Vector3 weaponPos = transform.position;
        Vector3 weaponDir = new Vector3(playerPos.x - weaponPos.x, playerPos.y - weaponPos.y + Projectile.offsetY, 0);

        float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
        if (playerPos.y + Projectile.offsetY < transform.position.y) angleBetweenEnemyAndWeapon *= -1;

        transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);
    }
}
