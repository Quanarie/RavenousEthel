using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    public float toHeal;

    public void Drain()
    {
        GameManager.Instance.playerHealth.Heal(toHeal);

        Destroy(gameObject);
    }
}
