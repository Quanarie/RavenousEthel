using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : Movement
{
    private void FixedUpdate()
    {
        float inputX = PlayerIdentifier.Instance.Input.Horizontal;
        float inputY = PlayerIdentifier.Instance.Input.Vertical;

        UpdateMotor(new Vector3(inputX, inputY, 0).normalized);
    }
}
