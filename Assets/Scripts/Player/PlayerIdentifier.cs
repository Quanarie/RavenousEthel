using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdentifier : MonoBehaviour
{
    public static PlayerIdentifier Instance;

    [HideInInspector] public PlayerInput Input;
    [HideInInspector] public Animator Animator;
    [HideInInspector] public AnimationChanger AnimationChanger;
    [HideInInspector] public PlayerHealth Health;
    [HideInInspector] public PlayerAttack Attack;
    [HideInInspector] public PlayerMutation Mutation;
    [HideInInspector] public PlayerMovement Movement;
    [HideInInspector] public CircleCollider2D HitBox;

    private void Awake()
    {
        Instance = this;

        Input = GetComponent<PlayerInput>();
        Animator = GetComponent<Animator>();
        AnimationChanger = GetComponent<AnimationChanger>();
        Health = GetComponent<PlayerHealth>();
        Attack = GetComponent<PlayerAttack>();
        Mutation = GetComponent<PlayerMutation>();
        Movement = GetComponent<PlayerMovement>();
        HitBox = GetComponent<CircleCollider2D>();
    }
}
