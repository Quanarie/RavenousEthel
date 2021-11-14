using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableObject : MonoBehaviour
{
    public static float pickupDistance = 0.5f;
    public static float offsetY = 0.2f;
    private static float arrowSpeed = 2f;
    private float argumentForSin;
    protected GameObject arrow;

    protected virtual void Start()
    {
        WeaponManager.OnPickupClicked += TryPickup;

        arrow = Instantiate(GameManager.Instance.pickupableArrowObject, transform.position, transform.rotation, transform);
        arrow.SetActive(false);
    }

    private void Update()
    {
        if (Vector3.Distance(GameManager.Instance.Player.position, transform.position) <= pickupDistance)
        {
            if (!arrow.activeSelf)
                arrow.SetActive(true);

            Vector3 arrPos = arrow.transform.localPosition;
            arrow.transform.localPosition = new Vector3(arrPos.x, offsetY + Mathf.Abs(Mathf.Sin(argumentForSin) / 4), 0);
            argumentForSin += Time.deltaTime * arrowSpeed;
        }
        else if (arrow.activeSelf)
        {
            arrow.SetActive(false);
            argumentForSin = 0f;
        }
    }

    protected virtual void TryPickup()
    {
        if (Vector3.Distance(GameManager.Instance.Player.position, transform.position) <= pickupDistance)
        {
            PerformAction();
        }
    }

    protected virtual void PerformAction() { }
}
