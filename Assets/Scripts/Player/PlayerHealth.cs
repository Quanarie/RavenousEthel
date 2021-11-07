using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealth : AliveCreature
{
    [SerializeField] private float maxScale;
    [SerializeField] private float minScale;
    [SerializeField] private float getBiggerValue;
    [SerializeField] private float getSmallerValue;
    [SerializeField] private float timeToGetSmaller;

    private float prevTimeGotSmaller;

    public void Heal(float toHeal)
    {
        if (currentHp == maxHp)
            return;

        currentHp += toHeal;

        if (currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    private void Update()
    {
        if (Time.time - prevTimeGotSmaller > timeToGetSmaller)
        {
            GetSmaller();
            prevTimeGotSmaller = Time.time;
        }
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
    }

    protected override void Death()
    {
        SceneManager.LoadScene(0);
    }
}
