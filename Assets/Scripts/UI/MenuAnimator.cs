using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void OpenMenu()
    {
        animator.SetTrigger("open");
    }

    public void CloseMenu()
    {
        animator.SetTrigger("close");
    }
}
