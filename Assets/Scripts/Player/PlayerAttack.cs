using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void Transformation();
    public static event Transformation OnMutation;
    public static event Transformation OnDemutation;

    public RuntimeAnimatorController regularController;
    public RuntimeAnimatorController mutatedController;

    [HideInInspector] public Weapon weapon;
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
    [SerializeField] private GameObject mutantProjectilePrefab;
    [SerializeField] private GameObject deadMonsterPrefab;
    [SerializeField] private AnimationClip monsterDeath;
    private float previousMutatedStateAttackTime;

    private void Start()
    {
        GameManager.Instance.playerAnimator.SetTrigger("transform");

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        rechargeRegularImage.fillAmount = Mathf.Min((Time.time - previousRegularStateAttackTime) / rechargeTimeRegular, 1);
        rechargeMutatedImage.fillAmount = Mathf.Min((Time.time - previousMutatedStateAttackTime) / rechargeTimeMutated, 1);
    }

    public void SwallowUp()
    {
        Transform enemy = GameManager.Instance.FindClosestEnemyInRange(Camera.main.orthographicSize * Screen.width / Screen.height);
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
            Transform enemy = GameManager.Instance.FindClosestEnemyInRange(attackDistanceMutantState);

            if (enemy != null)
            {
                GameManager.Instance.playerAnimator.SetFloat("enemyPosX", (enemy.position.x - transform.position.x) / Mathf.Abs((enemy.position.x - transform.position.x)));
                GameManager.Instance.playerAnimator.SetTrigger("attack");

                enemy.GetComponent<EnemyHealth>().ReceiveDamage(damageMutantState,
                    new Vector3(enemy.position.x - transform.position.x, enemy.position.y - transform.position.y, 0).normalized * pushForceMutantState,
                    stunTimeMutantState);
            }
            else
            {
                GameManager.Instance.playerAnimator.SetFloat("enemyPosX", 0);
                GameManager.Instance.playerAnimator.SetTrigger("attack");
            }
            previousMutatedStateAttackTime = Time.time;
        }
        else
        {
            weapon.Shoot();
        }
    }

    public void Mutate(GameObject enemy)
    {

        OnMutation?.Invoke();

        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = mutatedController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");

        GameManager.Instance.playerHealth.Mutate(enemy.GetComponent<EnemyHealth>().corpse.GetComponent<EnemyCorpse>().toHeal);
        transform.position = enemy.transform.position;
        Destroy(enemy);

        CircleCollider2D hitbox = GameManager.Instance.playerHitBox;
        hitbox.radius = 0.115f;
        hitbox.offset = new Vector3(0f, 0.115f, 0f);

        GameManager.Instance.playerHealth.GetBigger();
    }

    public void DeMutate()
    {
        OnDemutation?.Invoke();

        GameManager.Instance.state = GameManager.State.regular;

        GameManager.Instance.playerAnimator.SetTrigger("death");

        CircleCollider2D hitbox = GameManager.Instance.playerHitBox;
        hitbox.radius = 0.09f;
        hitbox.offset = new Vector3(0f, 0.09f, 0f);

        StartCoroutine(ChangeStateToRegular());

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

        GameManager.Instance.playerAnimator.SetTrigger("drain");
        return true;
    }

    IEnumerator ChangeStateToRegular()
    {
        yield return new WaitForSeconds(monsterDeath.length);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = regularController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        Instantiate(deadMonsterPrefab, transform.position, Quaternion.identity);
    }
}
