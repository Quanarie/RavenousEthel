using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player;

    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public PlayerAttack playerAttack;

    public Joystick Joystick;
    public Button AttackButton;

    public Transform EnemiesParent;

    public State state;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

        playerAnimator = Player.GetComponent<Animator>();
        playerAttack = Player.GetComponent<PlayerAttack>();

        state = State.regular;
    }

    public enum State
    {
        regular,
        mutated,
    }

}
