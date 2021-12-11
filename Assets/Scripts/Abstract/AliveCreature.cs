using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveCreature : MonoBehaviour
{
    [SerializeField] private string damageAudioName;
    [SerializeField] private string deathAudioName;
    public float maxHp;

    [HideInInspector] public float currentHp;

    protected Movement movementScript;
    protected Animator animator;

    protected virtual void Start()
    {
        currentHp = maxHp;

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

        GameManager.Instance.floatingTextManager.Show("-" + damageAmount.ToString(), 15, Color.red, transform.position, new Vector3(70, 80, 0), 0.5f);

        animator.SetTrigger("damage");
    }

    public virtual void ReceiveDamage(float damageAmount, Vector3 pushDirection, float stunTime)
    {
        ReceiveDamage(damageAmount);

        movementScript.SetPushDirection(pushDirection);

        movementScript.Stun(stunTime);
    }

    public virtual void Death()
    {
        AudioManager.Instance.Play(deathAudioName);
    }
}
