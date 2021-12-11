using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Action<float> OnMutation = delegate { };

    private Weapon weapon;
    private WeaponManager weaponManager;

    [SerializeField] private float distanceToDrain;

    [Header("Regular State")]
    [SerializeField] private float rechargeTimeRegular;
    [SerializeField] private GameObject regularProjectilePrefab;
    [SerializeField] private GameObject deadSlimePrefab;
    [SerializeField] private Image rechargeRegularImage;
    private float previousRegularStateAttackTime;

    [Header("Mutated State")]
    [SerializeField] private float attackDistanceMutantState;
    [SerializeField] private float rechargeTimeMutated;
    [SerializeField] private float damageMutantState;
    [SerializeField] private float stunTimeMutantState;
    [SerializeField] private float pushForceMutantState;
    [SerializeField] private Image rechargeMutatedImage;

    private float previousMutatedStateAttackTime;

    private void Start()
    {
        PlayerIdentifier.Instance.Animator.SetTrigger("transform");

        weaponManager = GetComponent<WeaponManager>();

        PlayerIdentifier.Instance.Health.OnDemutation += DeMutate;
    }

    private void Update()
    {
        rechargeRegularImage.fillAmount = Mathf.Min((Time.time - previousRegularStateAttackTime) / rechargeTimeRegular, 1);
        rechargeMutatedImage.fillAmount = Mathf.Min((Time.time - previousMutatedStateAttackTime) / rechargeTimeMutated, 1);
    }

    public void SwallowUp()
    {
        Transform enemy = EnemyFinder.FindClosestEnemyInRange(Camera.main.orthographicSize * Screen.width / Screen.height);
        if (enemy == null || !enemy.GetComponent<Renderer>().isVisible)
            return;

        if (Time.time - previousRegularStateAttackTime < rechargeTimeRegular)
            return;

        Instantiate(regularProjectilePrefab, transform.position, Quaternion.identity);

        previousRegularStateAttackTime = Time.time;
    }

    public void Attack()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
            return;

        if (Time.time - previousMutatedStateAttackTime < rechargeTimeMutated)
            return;

        if (TryDrainCorpses())
            return;

        if (weapon == null)
        {
            Transform enemy = EnemyFinder.FindClosestEnemyInRange(attackDistanceMutantState);

            if (enemy != null)
            {
                PlayerIdentifier.Instance.Animator.SetFloat("enemyPosX", (enemy.position.x - transform.position.x) / Mathf.Abs((enemy.position.x - transform.position.x)));
                PlayerIdentifier.Instance.Animator.SetTrigger("attack");

                enemy.GetComponent<EnemyHealth>().ReceiveDamage(damageMutantState,
                    new Vector3(enemy.position.x - transform.position.x, enemy.position.y - transform.position.y, 0).normalized * pushForceMutantState,
                    stunTimeMutantState);
            }
            else
            {
                PlayerIdentifier.Instance.Animator.SetFloat("enemyPosX", 0);
                PlayerIdentifier.Instance.Animator.SetTrigger("attack");
            }
            previousMutatedStateAttackTime = Time.time;
        }
        else
        {
            weapon.Shoot();
        }
    }

    public void SetWeapon(Weapon weapon) => this.weapon = weapon;

    public void Mutate(GameObject enemy)
    {
        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        OnMutation?.Invoke(enemy.GetComponent<EnemyHealth>().corpse.GetComponent<EnemyCorpse>().toHeal);
        transform.position = enemy.transform.position;
        Destroy(enemy);

        CircleCollider2D hitbox = PlayerIdentifier.Instance.HitBox;
        hitbox.radius = 0.115f;
        hitbox.offset = new Vector3(0f, 0.115f, 0f);
    }

    public void DeMutate()
    {
        GameManager.Instance.state = GameManager.State.regular;

        CircleCollider2D hitbox = PlayerIdentifier.Instance.HitBox;
        hitbox.radius = 0.09f;
        hitbox.offset = new Vector3(0f, 0.09f, 0f);

        weaponManager.ClearWeapons();
    }

    public bool TryDrainCorpses()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(transform.position, distanceToDrain);

        int corpsesCount = 0;
        foreach(Collider2D item in items)
        {
            if (item.TryGetComponent(out EnemyCorpse _))
            {
                item.GetComponent<EnemyCorpse>().Drain();
                corpsesCount++;
            }
        }

        if (corpsesCount == 0)
            return false;

        PlayerIdentifier.Instance.Animator.SetTrigger("drain");
        return true;
    }
}
