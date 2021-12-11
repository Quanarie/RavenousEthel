using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public static PlayerIdentifier Instance;

    [HideInInspector] public Animator Animator;
    [HideInInspector] public AnimationChanger AnimationChanger;
    [HideInInspector] public PlayerHealth Health;
    [HideInInspector] public PlayerAttack Attack;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public CircleCollider2D HitBox;

    private void Awake()
    {
        Instance = this;

        Animator = GetComponent<Animator>();
        AnimationChanger = GetComponent<AnimationChanger>();
        Health = GetComponent<PlayerHealth>();
        Attack = GetComponent<PlayerAttack>();
        Movement = GetComponent<PlayerMovement>();
        HitBox = GetComponent<CircleCollider2D>();
    }
}
