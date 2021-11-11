using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDecreasingScale : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.Instance.playerHealth.getSmallerValue = 0.01f;
    }
}
