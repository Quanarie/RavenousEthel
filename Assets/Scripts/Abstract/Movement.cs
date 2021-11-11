using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    public float ySpeed = 0.75f;
    public float xSpeed = 1f;

    [HideInInspector] public float pushRecoverySpeed;
    public Vector3 pushDirection;

    protected bool isStunned;

    private Vector3 moveDelta;

    private Animator animator;
    private new Rigidbody2D rigidbody;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        if (isStunned)
            return;

        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        moveDelta += pushDirection;
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        rigidbody.MovePosition(transform.position + moveDelta * Time.deltaTime);

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

    public virtual void Stun(float stunTime)
    {
        isStunned = true;

        StartCoroutine(UnStunCreature(stunTime));

        UpdateAnimator(Vector3.zero);

        if (Physics2D.OverlapCircleAll(transform.position + (moveDelta + pushDirection) * Time.deltaTime, 0.1f, 1 << LayerMask.NameToLayer("Obstacles")).Length > 0)
        {
            pushDirection = Vector3.zero;
        }

        moveDelta += pushDirection;
        pushDirection = Vector3.zero;
        rigidbody.MovePosition(transform.position + moveDelta * Time.deltaTime);
    }

    protected virtual IEnumerator UnStunCreature(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }
}