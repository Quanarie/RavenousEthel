using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSlowdownTrigger : MonoBehaviour
{
    [SerializeField] private float slowedTime;
    private float timeBeforeSlowing;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            Time.timeScale = slowedTime;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02f;
        }
    }
}
