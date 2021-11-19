using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : PickupableObject
{
    [SerializeField] private float toHeal;

    protected override void PerformAction()
    {
        base.PerformAction();

        GameManager.Instance.playerHealth.Heal(toHeal);

        Destroy(gameObject);
    }
}
