using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float rechargeTime;
    [SerializeField] protected float range;
    [HideInInspector] public Image rechargeImage;

    private float lastShootTime;
    private Vector3 weaponDirectionLast;

    public void Shoot()
    {
        if (Time.time - lastShootTime >= rechargeTime)
        {
            Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

            Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            Vector3 weaponPos = transform.position;

            if (enemyToAttack != null)
            {
                spawnedProjectile.direction = new Vector3(enemyToAttack.position.x - weaponPos.x, enemyToAttack.position.y - weaponPos.y, 0);
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

        rechargeImage.fillAmount = Mathf.Min((Time.time - lastShootTime) / rechargeTime, 1);

        Transform enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

        if (enemyToAttack != null)
        {
            Vector3 weaponPos = transform.position;
            Vector3 weaponDir = new Vector3(enemyToAttack.position.x - weaponPos.x, enemyToAttack.position.y - weaponPos.y, 0);

            float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
            if (enemyToAttack.position.y < transform.position.y) angleBetweenEnemyAndWeapon *= -1;

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
