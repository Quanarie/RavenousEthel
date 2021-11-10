using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class PlayerHealth : AliveCreature
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float getBiggerValue;
    [SerializeField] private float getSmallerValue;
    [SerializeField] private float timeToGetSmaller;

    [SerializeField] private Slider healhtSlider;

    private float prevTimeGotSmaller;

    [Header("Camera Shake")]
    private CameraShake cameraShakeComponent;
    [SerializeField] private float duration;
    [SerializeField] private float magnitude;

    protected override void Start()
    {
        base.Start();

        healhtSlider.maxValue = maxHp;
        healhtSlider.value = maxHp;

        cameraShakeComponent = Camera.main.GetComponent<CameraShake>();
    }

    public override void ReceiveDamage(float damageAmount)
    {
        base.ReceiveDamage(damageAmount);

        healhtSlider.value = currentHp;

        StartCoroutine(cameraShakeComponent.Shake(duration, magnitude));
    }

    private void Update()
    {
        if (Time.time - prevTimeGotSmaller > timeToGetSmaller)
        {
            GetSmaller();
            prevTimeGotSmaller = Time.time;
        }
    }

    public void Heal(float toHeal)
    {
        if (currentHp == maxHp)
            return;

        currentHp += toHeal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }

        healhtSlider.value = currentHp;
    }

    public void HealMax()
    {
        Heal(maxHp);
    }

    public void GetBigger()
    {
        if (transform.localScale.x + getBiggerValue < maxScale)
        {
            transform.localScale += new Vector3(getBiggerValue, getBiggerValue, 0);
        }
    }

    public void GetSmaller()
    {
        if (transform.localScale.x - getSmallerValue > minScale)
        {
            transform.localScale -= new Vector3(getSmallerValue, getSmallerValue, 0);
        }
        else if (GameManager.Instance.state == GameManager.State.mutated)
        {
            GameManager.Instance.playerAttack.DeMutate();
            currentHp = maxHp;
        }
    }

    public override void Death()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
        {
            SceneManager.LoadScene(0);
        }
        else
        {
            GameManager.Instance.playerAttack.DeMutate();
            currentHp = maxHp;
        }
    }
}
