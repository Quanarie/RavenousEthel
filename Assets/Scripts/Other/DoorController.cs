using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : PickupableObject
{
    [SerializeField] private ControlledDoor[] doors;

    protected override void PerformAction()
    {
        foreach (ControlledDoor door in doors)
        {
            door.Open();
        }

        Destroy(arrow);
        Destroy(this);
    }
}
