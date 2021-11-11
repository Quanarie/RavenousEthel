using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAndSetActiveObjectAfterDeath : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private void OnDestroy()
    {
        obj.SetActive(true);
        obj.transform.position = transform.position;
    }
}
