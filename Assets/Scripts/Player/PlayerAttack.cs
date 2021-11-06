using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Weapon weapon;

    [Header("Regular State")]
    [SerializeField] private float rechargeTimeRegular;
    [SerializeField] private GameObject regularProjectilePrefab;
    [SerializeField] private GameObject deadSlimePrefab;
    private float previousRegularStateAttackTime;

    [Header("Mutated State")]
    [SerializeField] private GameObject mutantProjectilePrefab;
    [SerializeField] private float rechargeTimeMutated;
    private float previousMutatedStateAttackTime;

    private void Start()
    {
        weapon = GameManager.Instance.WeaponParent.GetChild(0).GetComponent<Weapon>();
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

        transform.position = enemy.transform.position;

        Destroy(enemy);
    }

    public void DeMutate()
    {
        GameManager.Instance.state = GameManager.State.regular;
    }
}
