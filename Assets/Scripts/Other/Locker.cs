using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : PickupableObject
{
    [SerializeField] private GameObject insideObject;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite closed;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        offsetY = 0.3f;

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void PerformAction()
    {
        base.PerformAction();

        spriteRenderer.sprite = opened;
        Instantiate(insideObject, spawnPoint.position, transform.rotation);

        Destroy(arrow);
        enabled = false;
    }
}
