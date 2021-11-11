using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveTrigger : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            obj.SetActive(true);
        }
    }
}
