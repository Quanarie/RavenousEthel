using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCorpse : MonoBehaviour
{
    [SerializeField] private float toHeal;

    public void Drain()
    {
        GameManager.Instance.playerHealth.GetBigger();
        GameManager.Instance.playerHealth.Heal(toHeal);

        Destroy(gameObject);
    }
}
