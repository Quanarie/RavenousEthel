using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Movement : MonoBehaviour
{
    [SerializeField] protected float ySpeed = 0.75f;
    [SerializeField] protected float xSpeed = 1f;

    [SerializeField] private float pushRecoverySpeed;

    protected bool isStunned;
    private Vector3 pushDirection;
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

    public void SetPushDirection(Vector3 pushDirection)
    {
        this.pushDirection = pushDirection;
    }

    public virtual void Stun(float stunTime)
    {
        isStunned = true;

        StartCoroutine(UnStunCreature(stunTime));
        StartCoroutine(PushWhileStunned(stunTime));

        UpdateAnimator(Vector3.zero);
    }

    protected virtual IEnumerator UnStunCreature(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }

    protected virtual IEnumerator PushWhileStunned(float stunTime)
    {
        for (float i = 0; i < stunTime; )
        {
            pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);
            rigidbody.velocity = pushDirection;

            i += Time.deltaTime;
            yield return null;
        }

    }
}