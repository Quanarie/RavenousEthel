using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUpgrade : PickupableObject
{
    [SerializeField] private float xSpeedUpgrade;
    [SerializeField] private float ySpeedUpgrade;
    [SerializeField] private float duration;

    protected override void PerformAction()
    {
        base.PerformAction();

        PlayerIdentifier.Instance.Movement.xSpeed += xSpeedUpgrade;
        PlayerIdentifier.Instance.Movement.ySpeed += ySpeedUpgrade;

        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(arrow);

        StartCoroutine(ReturnNormalSpeed());
    }

    private IEnumerator ReturnNormalSpeed()
    {
        yield return new WaitForSeconds(duration);

        PlayerIdentifier.Instance.Movement.xSpeed -= xSpeedUpgrade;
        PlayerIdentifier.Instance.Movement.ySpeed -= ySpeedUpgrade;

        Destroy(gameObject);
    }
}
