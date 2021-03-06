using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledDoor : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void Open()
    {
        animator.SetTrigger("open");
    }

    public void Close ()
    {
        animator.SetTrigger("close");
    }
}
