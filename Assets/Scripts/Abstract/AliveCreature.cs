using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveCreature : MonoBehaviour
{
    [SerializeField] private AudioClip damageAudio;
    [SerializeField] private AudioClip deathAudio;
    public float maxHp;

    [HideInInspector] public float currentHp;

    private Movement movementScript;
    protected Animator animator;
    private AudioSource audioSource;

    protected virtual void Start()
    {
        currentHp = maxHp;

        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
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

        audioSource.PlayOneShot(damageAudio);
    }

    public virtual void ReceiveDamage(float damageAmount, Vector3 pushDirection, float stunTime)
    {
        ReceiveDamage(damageAmount);

        movementScript.pushDirection = pushDirection;

        movementScript.Stun(stunTime);
    }

    public virtual void Death()
    {
        audioSource.PlayOneShot(deathAudio);
    }
}
