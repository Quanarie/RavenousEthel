using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private Joystick joystick;

    public float Horizontal { get; private set; }
    public float Vertical { get; private set; }

    public Action OnSwallowButtonPressed = delegate { };
    public Action OnInteractButtonPressed = delegate { };
    public Action OnWeaponButtonPressed = delegate { };

    private void Update()
    {
        Horizontal = joystick.Horizontal;
        Vertical = joystick.Vertical;
    }

    public void SwallowPressed() => OnSwallowButtonPressed?.Invoke();
    public void InteractPressed() => OnInteractButtonPressed?.Invoke();
    public void WeaponPressed() => OnWeaponButtonPressed?.Invoke();
}
