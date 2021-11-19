using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthUpgrade : PickupableObject
{
    [SerializeField] private float upgradeHealth;
    [SerializeField] private float duration;

    protected override void PerformAction()
    {
        base.PerformAction();


        GameManager.Instance.playerHealth.upgradeHealth = upgradeHealth;

        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(arrow);

        StartCoroutine(ReturnNormalHealth());
    }

    private IEnumerator ReturnNormalHealth()
    {
        yield return new WaitForSeconds(duration);

        if (GameManager.Instance.playerHealth.upgradeHealth > 0)
            GameManager.Instance.playerHealth.upgradeHealth = 0;

        Destroy(gameObject);
    }
}
