using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;

    [HideInInspector] public float pushRecoverySpeed;
    public Vector3 pushDirection;

    private bool isStunned;

    private Vector3 moveDelta;

    private Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        if (isStunned)
            return;

        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        transform.Translate(moveDelta.x * Time.deltaTime, moveDelta.y * Time.deltaTime, 0);

        UpdateAnimator(input);
    }

    private void UpdateAnimator(Vector3 input)
    {
        animator.SetFloat("moveX", input.x);
        animator.SetFloat("moveY", input.y);

        if (input.x != 0)
        {
            animator.SetFloat("prevMoveX", input.x);
        }
    }

    public void Stun(float stunTime)
    {
        isStunned = true;
        StartCoroutine(UnStunCreature(stunTime));

        UpdateAnimator(Vector3.zero);

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