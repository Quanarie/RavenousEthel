using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public delegate void Transformation();
    public static event Transformation OnMutation;
    public static event Transformation OnDemutation;

    [SerializeField] private RuntimeAnimatorController regularController;
    [SerializeField] private RuntimeAnimatorController mutatedController;

    [HideInInspector] public Weapon weapon;
    private WeaponManager weaponManager;

    [SerializeField] private float distanceToDrain = 0.5f;

    [Header("Regular State")]
    [SerializeField] private float regularAttackDistanceX;
    [SerializeField] private float regularAttackDistanceY;
    [SerializeField] private float rechargeTimeRegular;
    [SerializeField] private GameObject regularProjectilePrefab;
    [SerializeField] private GameObject deadSlimePrefab;
    [SerializeField] private Image rechargeRegularImage;
    private float previousRegularStateAttackTime;

    [Header("Mutated State")]
    [SerializeField] private float attackDistanceMutantState;
    [SerializeField] private float damageMutantState;
    [SerializeField] private float stunTimeMutantState;
    [SerializeField] private float pushForceMutantState;
    [SerializeField] private GameObject mutantProjectilePrefab;
    [SerializeField] private GameObject deadMonsterPrefab;
    [SerializeField] private AnimationClip monsterDeath;

    private void Start()
    {
        GameManager.Instance.playerAnimator.runtimeAnimatorController = regularController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");

        weaponManager = GetComponent<WeaponManager>();
    }

    private void Update()
    {
        rechargeRegularImage.fillAmount = Mathf.Min((Time.time - previousRegularStateAttackTime) / rechargeTimeRegular, 1);
    }

    public void SwallowUp()
    {
        Transform closestEnemy = GameManager.Instance.FindClosestEnemyInRange(Camera.main.orthographicSize * Screen.width / Screen.height);
        if (closestEnemy == null)
            return;

        if (Mathf.Abs(closestEnemy.position.y - transform.position.y) > regularAttackDistanceY ||
            Mathf.Abs(closestEnemy.position.x - transform.position.x) > regularAttackDistanceX)
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

        TryDrainCorpses();

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
            return;
        }

        weapon.Shoot();
    }

    public void Mutate(GameObject enemy)
    {
        OnMutation?.Invoke();

        GameManager.Instance.state = GameManager.State.mutated;

        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = mutatedController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        GameManager.Instance.playerHealth.Mutate(enemy.GetComponent<EnemyHealth>().corpse.GetComponent<EnemyCorpse>().toHeal);
        CircleCollider2D hitbox = GameManager.Instance.playerHitBox;
        hitbox.radius = 0.115f;
        hitbox.offset = new Vector3(0f, 0.115f, 0f);

        transform.position = enemy.transform.position;

        GameManager.Instance.playerHealth.GetBigger();
        Destroy(enemy);
    }

    public void DeMutate()
    {
        OnDemutation?.Invoke();

        GameManager.Instance.state = GameManager.State.regular;

        GameManager.Instance.playerAnimator.SetTrigger("death");

        CircleCollider2D hitbox = GameManager.Instance.playerHitBox;
        hitbox.radius = 0.08f;
        hitbox.offset = new Vector3(0f, 0.08f, 0f);

        StartCoroutine(ChangeStateToRegular());

        weaponManager.ClearWeapons();
    }

    public void TryDrainCorpses()
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
            return;

        GameManager.Instance.playerAnimator.SetTrigger("drain");
    }

    IEnumerator ChangeStateToRegular()
    {
        yield return new WaitForSeconds(monsterDeath.length);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = regularController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        Instantiate(deadMonsterPrefab, transform.position, Quaternion.identity);
    }
}
