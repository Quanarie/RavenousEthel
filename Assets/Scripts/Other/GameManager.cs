using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player;

    public Animator playerAnimator;

    public Joystick Joystick;
    public Button AttackButton;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
