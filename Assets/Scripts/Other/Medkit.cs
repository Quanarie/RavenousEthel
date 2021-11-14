using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : PickupableObject
{
    [SerializeField] private float toHeal;

    protected override void PerformAction()
    {
        GameManager.Instance.playerHealth.Heal(toHeal);

        WeaponManager.OnPickupClicked -= TryPickup;

        Destroy(gameObject);
    }
}
