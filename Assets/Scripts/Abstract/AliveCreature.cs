using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveCreature : MonoBehaviour
{
    [SerializeField] protected float maxHp;

    protected float currentHp;

    private Color startColor;
    private Movement movementScript;

    private void Start()
    {
        currentHp = maxHp;

        startColor = GetComponent<SpriteRenderer>().color;
        movementScript = GetComponent<Movement>();
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
    }

    public virtual void ReceiveDamage(float damageAmount, Vector3 pushDirection, float stunTime)
    {
        ReceiveDamage(damageAmount);

        movementScript.pushDirection = pushDirection;

        //stun
    }

    private IEnumerator ChangeColorBack()
    {
        yield return new WaitForSeconds(0.5f);

        GetComponent<SpriteRenderer>().color = startColor;
    }

    protected virtual void Death() { }
}
