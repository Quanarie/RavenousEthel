using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetActiveObjectAfterDeath : MonoBehaviour
{
    [SerializeField] private GameObject obj;

    private void OnDestroy()
    {
        obj.SetActive(true);
    }
}
