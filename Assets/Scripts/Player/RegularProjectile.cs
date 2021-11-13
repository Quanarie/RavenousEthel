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
        enemyToAttack = GameManager.Instance.FindClosestEnemyInRange(Camera.main.orthographicSize * Screen.width / Screen.height);

        if (enemyToAttack == null || !enemyToAttack.GetComponent<Renderer>().isVisible)
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
        Vector3 endPos = new Vector3(enemyToAttack.position.x, enemyToAttack.position.y + Projectile.offsetY, 0);

        transform.position = Vector3.Lerp(transform.position, endPos, speedOfLerping);

        if (Vector3.Distance(transform.position, endPos) < distanceToMutate)
        {
            GameManager.Instance.playerAttack.Mutate(enemyToAttack.gameObject);
            Destroy(gameObject);
        }
    }
}
