using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : AliveCreature
{
    [SerializeField] private AnimationClip deathClip;
    [SerializeField] private Weapon dropWeapon;
    [SerializeField] private int money;
    public GameObject corpse;

    public override void Death()
    {
        GameManager.Instance.AddMoney(money);

        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }

        animator.SetTrigger("death");
        animator.SetBool("isDead", true);

        GetComponent<EnemyMovement>().enabled = false;

        StartCoroutine(SpawnCorpse());

        if (!TryGetComponent(out EnemyDistanceAttack _))
            return;

        if (Random.value <= GetComponent<EnemyDistanceAttack>().weapon.chanceOfDropping)
        {
            Weapon weapon = Instantiate(dropWeapon, transform.position, transform.rotation);

            weapon.currentShotQuantity = Random.Range(0, weapon.maxShootQuantity);

            Vector2 randomOffset = Random.insideUnitCircle / 3;
            weapon.transform.position += new Vector3(randomOffset.x, randomOffset.y, 0);
        }
    }

    private IEnumerator SpawnCorpse()
    {
        yield return new WaitForSeconds(deathClip.length);

        if (corpse != null)
        {
            GameObject spawnedCorpse = Instantiate(corpse, transform.position, transform.rotation);

            if (animator.GetFloat("prevMoveX") > 0)
                spawnedCorpse.transform.localScale = new Vector3(1, 1, 0);
            else
                spawnedCorpse.transform.localScale = new Vector3(-1, 1, 0);
        }
        Destroy(gameObject);
    }
}
