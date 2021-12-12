using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : Movement
{
    [SerializeField] private float chasingDistance;

    private EnemyAttack enemyAttack;

    protected override void Start()
    {
        base.Start();
        enemyAttack = GetComponent<EnemyAttack>();
    }

    private void FixedUpdate()
    {
        if (Vector3.Distance(PlayerIdentifier.Instance.transform.position, transform.position) <= enemyAttack.attackDistance || IsThereAWallOnTheWayToPlayer(transform.position))
        {
            UpdateMotor(Vector3.zero);
            return;
        }

        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;
        float inputX = playerPos.x - transform.position.x;
        float inputY = playerPos.y - transform.position.y;

        if (Vector3.Distance(transform.position, playerPos) <= chasingDistance)
        {
            UpdateMotor(new Vector3(inputX, inputY, 0).normalized);
        }
        else
        {
            UpdateMotor(Vector3.zero);
        }
    }

    public override void Stun(float stunTime)
    {
        base.Stun(stunTime);
        enemyAttack.enabled = false;
    }

    protected override IEnumerator UnStunCreature(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        enemyAttack.enabled = true;
        isStunned = false;
    }

    public static bool IsThereAWallOnTheWayToPlayer(Vector3 selfPos)
    {
        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;

        RaycastHit2D[] hits = Physics2D.RaycastAll(selfPos, new Vector3(playerPos.x - selfPos.x, playerPos.y - selfPos.y, 0f));

        bool isWallBetweenPlayerAndEnemy = false;
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.collider.TryGetComponent(out PlayerHealth _))
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
