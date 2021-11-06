using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PlayerHealth : AliveCreature
{
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

    protected override void Death()
    {
        SceneManager.LoadScene(0);
    }
}
