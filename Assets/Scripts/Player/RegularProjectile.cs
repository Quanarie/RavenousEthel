using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfLerping;
    [SerializeField] private float distanceToMutate;

    private Transform enemyToAttack;

    private void Start()
    {
        enemyToAttack = EnemyFinder.FindClosestEnemyInRange(Camera.main.orthographicSize * Screen.width / Screen.height);

        if (enemyToAttack == null)
        {
            Destroy(gameObject);
            return;
        }

        if (GameManager.Instance.state == GameManager.State.regular)
            PlayerIdentifier.Instance.Animator.SetTrigger("attack");
        else PlayerIdentifier.Instance.Animator.SetTrigger("transform");

        PlayerIdentifier.Instance.Animator.SetFloat("enemyPosX", (enemyToAttack.position.x - PlayerIdentifier.Instance.transform.position.x) / Mathf.Abs(enemyToAttack.position.x - PlayerIdentifier.Instance.transform.position.x));
    }

    private void Update()
    {
        if (enemyToAttack == null)
            return;

        Vector3 endPos = new Vector3(enemyToAttack.position.x, enemyToAttack.position.y + Projectile.offsetY, 0);

        transform.position = Vector3.Lerp(transform.position, endPos, speedOfLerping);

        if (Vector3.Distance(transform.position, endPos) < distanceToMutate)
        {
            PlayerIdentifier.Instance.Mutation.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }
}
