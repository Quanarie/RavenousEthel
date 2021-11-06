using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeMovement : Movement
{
    [SerializeField] private float chasingDistance;

    private bool isAttacking;

    private void Update()
    {
        if (isAttacking)
        {
            UpdateMotor(Vector3.zero);
            return;
        }

        Vector3 playerPos = GameManager.Instance.Player.position;

        if (Vector3.Distance(transform.position, playerPos) <= chasingDistance)
        {
            UpdateMotor(new Vector3(playerPos.x - transform.position.x, playerPos.y - transform.position.y, 0).normalized);
        }
        else
        {
            UpdateMotor(Vector3.zero);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
            isAttacking = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
            isAttacking = false;
    }
}
