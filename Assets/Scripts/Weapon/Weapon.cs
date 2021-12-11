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
    [HideInInspector] public int currentShotQuantity;
    public int index;
    [SerializeField] protected string weaponSoundName;

    [SerializeField] private float damageAmount;
    [SerializeField] private float pushForce;
    [SerializeField] private float stunTime;
    [SerializeField] private float speed;
    [SerializeField] private float lifetime;
    [SerializeField] private Sprite projectileSprite;

    protected virtual void Update()
    {
        if (transform.parent != GameManager.Instance.WeaponParent)
            return;

        rechargeImage.fillAmount = Mathf.Min((Time.time - lastShootTime) / rechargeTime, 1);

        Transform enemyToAttack = EnemyFinder.FindClosestEnemyInRange(range);

        Vector3 weaponPos = transform.position;
        Vector3 weaponDir = Vector3.zero;

        if (enemyToAttack != null)
        {
            weaponDir = new Vector3(enemyToAttack.position.x - weaponPos.x, enemyToAttack.position.y - weaponPos.y + Projectile.offsetY, 0);
        }

        bool isWallBetweenPlayerAndEnemy = IsThereAWallOnTheWay(weaponDir);

        if (!isWallBetweenPlayerAndEnemy && enemyToAttack != null)
        {
            float angleBetweenEnemyAndWeapon = Mathf.Acos(weaponDir.x / weaponDir.magnitude) * 180 / Mathf.PI;
            if (enemyToAttack.position.y + Projectile.offsetY < weaponPos.y) angleBetweenEnemyAndWeapon *= -1;

            transform.rotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);

            if (enemyToAttack.position.x < transform.position.x)
                transform.localScale = new Vector3(1, -1, 0);
            else
                transform.localScale = new Vector3(1, 1, 0);
        }
        else
        {
            float inputX = PlayerIdentifier.Instance.Input.Horizontal;
            float inputY = PlayerIdentifier.Instance.Input.Vertical;

            weaponDir = new Vector3(inputX, inputY, 0);

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

    public virtual void Shoot()
    {
        if (Time.time - lastShootTime < rechargeTime)
            return;

        AudioManager.Instance.Play(weaponSoundName);

        Transform enemyToAttack = EnemyFinder.FindClosestEnemyInRange(range);

        Projectile spawnedProjectile = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        spawnedProjectile.Construct(damageAmount, pushForce, stunTime, speed, lifetime, projectileSprite);
        Vector3 weaponPos = transform.position;

        Vector3 weaponDir = Vector3.zero;
        if (enemyToAttack != null)
        {
            weaponDir = new Vector3(enemyToAttack.position.x - weaponPos.x, enemyToAttack.position.y - weaponPos.y + Projectile.offsetY, 0);
        }

        if (!IsThereAWallOnTheWay(weaponDir) && enemyToAttack != null)
        {
            spawnedProjectile.direction = weaponDir;
            spawnedProjectile.angleBetweenEnemyAndWeapon = transform.localRotation.eulerAngles.z;
        }
        else
        {
            spawnedProjectile.direction = new Vector3(weaponDirectionLast.x, weaponDirectionLast.y, 0);
            spawnedProjectile.angleBetweenEnemyAndWeapon = transform.localRotation.eulerAngles.z;

            if (weaponDirectionLast.magnitude == 0)
            {
                spawnedProjectile.direction = Vector3.right;
                spawnedProjectile.angleBetweenEnemyAndWeapon = transform.localRotation.eulerAngles.z;
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

    public void UpdateWeaponStock()
    {
        GameManager.Instance.weaponStock.minValue = 0f;
        GameManager.Instance.weaponStock.maxValue = maxShootQuantity;
        GameManager.Instance.weaponStock.value = maxShootQuantity - currentShotQuantity;
    }

    private bool IsThereAWallOnTheWay(Vector3 weaponDir)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, weaponDir);

        bool isWallBetweenPlayerAndEnemy = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out EnemyHealth _))
            {
                break;
            }

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
            {
                isWallBetweenPlayerAndEnemy = true;
            }
        }

        return isWallBetweenPlayerAndEnemy;
    }
}
