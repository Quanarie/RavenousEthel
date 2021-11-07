using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfLerping;
    [SerializeField] private float distanceToMutate;
    [SerializeField] private float range;

    private Transform enemyToAttack;

    private void Start()
    {
        enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(range);

        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }

        if (GameManager.Instance.state == GameManager.State.regular)
            GameManager.Instance.playerAnimator.SetTrigger("attack");
        else GameManager.Instance.playerAnimator.SetTrigger("transform");

        GameManager.Instance.playerAnimator.SetFloat("enemyPosX", (enemyToAttack.position.x - GameManager.Instance.Player.position.x) / Mathf.Abs(enemyToAttack.position.x - GameManager.Instance.Player.position.x));
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, enemyToAttack.position, speedOfLerping);

        if (Vector3.Distance(transform.position, enemyToAttack.position) < distanceToMutate)
        {
            GameManager.Instance.playerAttack.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }
}
