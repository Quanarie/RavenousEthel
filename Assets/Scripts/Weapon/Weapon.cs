using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected GameObject projectile;
    [SerializeField] protected float rechargeTime;
    [SerializeField] protected float range;
    public int maxShootQuantity;
    [HideInInspector] public Image rechargeImage;

    protected float lastShootTime;
    protected Vector3 weaponDirectionLast;
    public int currentShotQuantity;
    public int index;

    public void UpdateWeaponStock()
    {
        GameManager.Instance.weaponStock.minValue = 0f;
        GameManager.Instance.weaponStock.maxValue = maxShootQuantity;
        GameManager.Instance.weaponStock.value = maxShootQuantity - currentShotQuantity;
    }

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
                spawnedProjectile.direction = new Vector3(weaponDirectionLast.x, weaponDirectionLast.y - Projectile.offsetY, 0);

                if (weaponDirectionLast.magnitude == 0)
                {
                    spawnedProjectile.direction = Vector3.right;
                }
            }

            lastShootTime = Time.time;

            currentShotQuantity++;
            if (currentShotQuantity >= maxShootQuantity)
            {
                GameManager.Instance.weaponManager.Break(this);
                rechargeImage.fillAmount = 0f;
                return;
            }
            UpdateWeaponStock();
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
            Vector3 weaponDir = new Vector3(enemyToAttack.position.x - weaponPos.x, enemyToAttack.position.y - weaponPos.y + Projectile.offsetY, 0);

            float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
            if (enemyToAttack.position.y < transform.position.y) angleBetweenEnemyAndWeapon *= -1;

            transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);

            if (enemyToAttack.position.x < transform.position.x)
                transform.localScale = new Vector3(1, -1, 0);
            else
                transform.localScale = new Vector3(1, 1, 0);
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

            if (weaponDir.x < 0)
                transform.localScale = new Vector3(1, -1, 0);
            else
                transform.localScale = new Vector3(1, 1, 0);
        }
    }
}
