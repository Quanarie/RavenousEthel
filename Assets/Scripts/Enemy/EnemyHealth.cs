using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : AliveCreature
{
    protected override void Death()
    {
        Destroy(gameObject);
    }
}
