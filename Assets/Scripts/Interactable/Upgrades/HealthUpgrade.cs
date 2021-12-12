using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : PickupableObject
{
    [SerializeField] private float upgradeHealth;

    protected override void PerformAction()
    {
        base.PerformAction();

        PlayerIdentifier.Instance.Health.AddShield(upgradeHealth);

        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(gameObject);
    }
}
