using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    public float toHeal;

    public void Drain()
    {
        PlayerIdentifier.Instance.Health.Heal(toHeal);

        Destroy(gameObject);
    }
}
