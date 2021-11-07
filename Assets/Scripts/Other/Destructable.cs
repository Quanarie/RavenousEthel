using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructable : MonoBehaviour
{
    [SerializeField] private GameObject destructedVersion;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.TryGetComponent(out Projectile _))
            return;

        if (destructedVersion != null) 
            Instantiate(destructedVersion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
