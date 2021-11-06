using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;

    public float pushRecoverySpeed;
    public Vector3 pushDirection;

    private bool isStunned;

    private Vector3 moveDelta;

    protected virtual void UpdateMotor(Vector3 input)
    {
        if (isStunned)
            return;

        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, 0);
    }

    public void Stun(float stunTime)
    {
        isStunned = true;
        StartCoroutine(UnStunCreature(stunTime));

        moveDelta += pushDirection;
        pushDirection = Vector3.zero;
        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, 0);
    }

    IEnumerator UnStunCreature(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);

        isStunned = false;
    }
}