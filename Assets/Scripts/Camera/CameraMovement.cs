using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float cameraLerpSpeed;

    private Vector3 cameraStartPosition;

    private void Start()
    {
        cameraStartPosition = transform.position;
    }

    private void FixedUpdate()
    {
        Vector3 lerpedPosition = Vector3.Lerp(transform.position, PlayerIdentifier.Instance.transform.position, cameraLerpSpeed);

        transform.position = new Vector3(lerpedPosition.x, lerpedPosition.y, cameraStartPosition.z);
    }
}
