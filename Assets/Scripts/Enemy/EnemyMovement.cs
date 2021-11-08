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

    private void Update()
    {
        if (Vector3.Distance(GameManager.Instance.Player.position, transform.position) <= enemyAttack.attackDistance)
        {
            UpdateMotor(Vector3.zero);
            return;
        }

        Vector3 playerPos = GameManager.Instance.Player.position;
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
}
