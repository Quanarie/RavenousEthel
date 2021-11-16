using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : PickupableObject
{
    [SerializeField] private ControlledDoor door;

    protected override void PerformAction()
    {
        door.Open();
        Destroy(arrow);
        Destroy(this);
    }
}
