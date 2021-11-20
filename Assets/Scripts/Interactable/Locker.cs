using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Locker : PickupableObject
{
    [SerializeField] private GameObject insideObject;
    [SerializeField] private Transform insideSpawnPoint;
    [SerializeField] private Transform outsideSpawnPoint;
    [SerializeField] private Sprite opened;
    [SerializeField] private Sprite closed;

    private SpriteRenderer spriteRenderer;

    protected override void Start()
    {
        base.Start();

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void PerformAction()
    {
        base.PerformAction();

        spriteRenderer.sprite = opened;
        GameObject insideObj = Instantiate(insideObject, insideSpawnPoint.position, transform.rotation);

        if (insideObject.TryGetComponent(out Weapon _))
        {
            insideObj.transform.position = outsideSpawnPoint.position;
        }

        Destroy(arrow);
        Destroy(this);
    }
}
