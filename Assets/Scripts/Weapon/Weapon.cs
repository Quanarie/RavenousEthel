using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float rechargeTime;
    [SerializeField] protected float range;
    [HideInInspector] public Image rechargeImage;

    protected float lastShootTime;
    protected Vector3 weaponDirectionLast;

    public virtual void Shoot()
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

                if (weaponDirectionLast.magnitude == 0)
                {
                    spawnedProjectile.direction = Vector3.right;
                }
            }

            lastShootTime = Time.time;
        }
    }

    protected virtual void Update()
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
