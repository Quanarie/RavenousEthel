using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : AliveCreature
{
    public Action OnPlayerDeath = delegate { };
    public Action OnDemutation = delegate { };

    [SerializeField] private float maxRegularHp;
    [SerializeField] private float maxMonsterHp;
    [SerializeField] private float maxShield;

    [SerializeField] private Slider healhtSlider;
    [SerializeField] private Text healhtText;
    [SerializeField] private Slider shieldSlider;
    [SerializeField] private Text shieldText;

    public float Shield { get; private set; }

    [Header("Camera Shake")]
    private CameraShake cameraShakeComponent;
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;

    protected override void Start()
    {
        movementScript = GetComponent<Movement>();
        animator = GetComponent<Animator>();

        currentHp = PlayerPrefs.GetFloat("Health", maxRegularHp);

        UpdateHealth();
        UpdateShield();

        cameraShakeComponent = Camera.main.GetComponent<CameraShake>();

        PlayerIdentifier.Instance.Mutation.OnMutation += Mutate;
    }

    public override void ReceiveDamage(float damageAmount)
    {
        if (Shield > 0f)
        {
            Shield -= damageAmount;
            animator.SetTrigger("damage");

            if (Shield < 0f)
            {
                GameManager.Instance.floatingTextManager.Show("-" + damageAmount + Shield.ToString(), 15, Color.blue, transform.position, new Vector3(40, 80, 0), 0.5f);
                base.ReceiveDamage(-Shield);
                Shield = 0f;
            }
            else
            {
                GameManager.Instance.floatingTextManager.Show("-" + damageAmount.ToString(), 15, Color.blue, transform.position, new Vector3(70, 80, 0), 0.5f);
            }
        }
        else if (Shield == 0)
            base.ReceiveDamage(damageAmount);

        UpdateHealth();
        UpdateShield();

        StartCoroutine(cameraShakeComponent.Shake(duration, magnitude));
    }

    public void Heal(float toHeal)
    {
        if (toHeal == 0)
            return;

        GameManager.Instance.floatingTextManager.Show("+" + toHeal.ToString(), 20, Color.green, transform.position, new Vector3(50, 60, 0), 1f);
        
        if (currentHp == maxHp)
            return;

        currentHp += toHeal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        UpdateHealth();
    }

    public void UpdateHealth()
    {
        healhtSlider.maxValue = maxHp;
        healhtSlider.value = currentHp;
        healhtText.text = currentHp.ToString();
    }

    public void UpdateShield()
    {
        if (Shield == 0)
        {
            shieldSlider.gameObject.SetActive(false);
        }
        else
        {
            shieldSlider.gameObject.SetActive(true);

            shieldSlider.maxValue = maxShield;
            shieldSlider.value = Shield;
            shieldText.text = Shield.ToString();
        }
    }

    public void SetCurrentHealth(float health)
    {
        if (health <= 0)
            currentHp = maxRegularHp;
        else
            currentHp = health;

        UpdateHealth();
    }

    public void AddShield(float toAdd)
    {
        if (Shield + toAdd <= maxShield)
            Shield += toAdd;
        else Shield = maxShield;

        UpdateShield();
    }

    public void Mutate(float toHeal)
    {
        maxHp = maxMonsterHp;

        Heal(toHeal);

        UpdateHealth();
    }

    public void DeMutate()
    {
        maxHp = maxRegularHp;
        currentHp = maxRegularHp;

        UpdateHealth();
    }

    public override void Death()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
        {
            OnPlayerDeath?.Invoke();

            base.Death();
        }
        else
        {
            OnDemutation?.Invoke();
            DeMutate();
        }
    }
}
