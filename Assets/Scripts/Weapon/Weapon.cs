using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float rechargeTime;
    [SerializeField] protected float range;

    private float lastShootTime;
    private Vector3 weaponDirectionLast;

    public void Shoot()
    {
        if (Time.time - lastShootTime >= rechargeTime)
        {

            Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            Vector3 playerPos = GameManager.Instance.Player.position;

            if (enemyToAttack != null)
            {
                spawnedProjectile.direction = new Vector3(enemyToAttack.position.x - playerPos.x, enemyToAttack.position.y - playerPos.y, 0);
            }
            else
            {
                spawnedProjectile.direction = weaponDirectionLast;
            }

            lastShootTime = Time.time;
        }
    }

    private void Update()
    {
        if (transform.parent != GameManager.Instance.WeaponParent)
            return;

        Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

        if (enemyToAttack != null)
        {
            Vector3 playerPos = GameManager.Instance.Player.position;
            Vector3 weaponDir = new Vector3(enemyToAttack.position.x - playerPos.x, enemyToAttack.position.y - playerPos.y, 0);

            float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
            if (enemyToAttack.position.y < GameManager.Instance.Player.position.y) angleBetweenEnemyAndWeapon *= -1;

            transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);
        }
        else
        {
            Animator animator = GameManager.Instance.playerAnimator;

            Vector3 weaponDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"), 0);

            if (weaponDir.magnitude == 0)
                return;
            weaponDirectionLast = weaponDir;

            float angleBetweenWalkDirectionAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
            if (weaponDir.y < 0) angleBetweenWalkDirectionAndWeapon *= -1;

            transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenWalkDirectionAndWeapon);
        }
    }
}
