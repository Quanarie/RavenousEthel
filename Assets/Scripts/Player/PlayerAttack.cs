using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private RuntimeAnimatorController regularController;
    [SerializeField] private RuntimeAnimatorController mutatedController;

    [HideInInspector] public Weapon weapon;
    [HideInInspector] public Weapon startWeapon;
    private WeaponManager weaponManager;

    [Header("Regular State")]
    [SerializeField] private float rechargeTimeRegular;
    [SerializeField] private GameObject regularProjectilePrefab;
    [SerializeField] private GameObject deadSlimePrefab;
    [SerializeField] private Image rechargeRegularImage;
    private float previousRegularStateAttackTime;

    [Header("Mutated State")]
    [SerializeField] private GameObject mutantProjectilePrefab;
    [SerializeField] private GameObject deadMonsterPrefab;
    [SerializeField] private AnimationClip monsterDeath;

    private void Start()
    {
        weapon = GameManager.Instance.WeaponParent.GetChild(0).GetComponent<Weapon>();
        startWeapon = weapon;
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
        if (Time.time - previousRegularStateAttackTime < rechargeTimeRegular)
            return;

        Instantiate(regularProjectilePrefab, transform.position, Quaternion.identity);

        previousRegularStateAttackTime = Time.time;
    }

    public void Attack()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
            return;

        weapon.Shoot();
    }

    public void Mutate(GameObject enemy)
    {
        GameManager.Instance.state = GameManager.State.mutated;

        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = mutatedController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        GameManager.Instance.playerHealth.Mutate();

        transform.position = enemy.transform.position;

        enemy.GetComponent<EnemyHealth>().Death();
    }

    public void DeMutate()
    {
        GameManager.Instance.state = GameManager.State.regular;

        GameManager.Instance.playerAnimator.SetTrigger("death");

        StartCoroutine(ChangeStateToRegular());

        weaponManager.ClearWeapons();
    }

    IEnumerator ChangeStateToRegular()
    {
        yield return new WaitForSeconds(monsterDeath.length);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = regularController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        GameManager.Instance.playerHealth.DeMutate();
        Instantiate(deadMonsterPrefab, transform.position, Quaternion.identity);
    }
}
