using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveCreature : MonoBehaviour
{
    [SerializeField] protected float maxHp;

    protected float currentHp;

    private Color startColor;
    private Movement movementScript;
    private Animator animator;

    private void Start()
    {
        currentHp = maxHp;

        startColor = GetComponent<SpriteRenderer>().color;
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
    }

    public virtual void ReceiveDamage(float damageAmount)
    {
        currentHp -= damageAmount;

        if (currentHp <= 0)
        {
            currentHp = 0;

            Death();
        }

        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(ChangeColorBack());

        GameManager.Instance.floatingTextManager.Show("-" + damageAmount.ToString(), 15, Color.red, transform.position, new Vector3(70, 80, 0), 0.5f);

        if (animator != null)
            animator.SetTrigger("damage");
    }

    public virtual void ReceiveDamage(float damageAmount, Vector3 pushDirection, float stunTime)
    {
        ReceiveDamage(damageAmount);

        movementScript.pushDirection = pushDirection;

        movementScript.Stun(stunTime);
    }

    private IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().color = startColor;
    }

    protected virtual void Death() { }
}
