using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PickupableObject : MonoBehaviour
{
    public float pickupDistance = 0.5f;
    public float offsetY = 0.2f;
    private float arrowSpeed = 2f;
    private float argumentForSin;
    protected GameObject arrow;

    private bool isPickedUp;

    protected virtual void Start()
    {
        WeaponManager.OnPickupClicked += TryPickup;

        arrow = Instantiate(GameManager.Instance.pickupableArrowObject, transform.position, transform.rotation, transform);
        arrow.SetActive(false);
    }

    private void Update()
    {
        if (isPickedUp)
            return;

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
            isPickedUp = true;
        }
    }

    protected virtual void PerformAction() 
    {
        WeaponManager.OnPickupClicked -= TryPickup;
    }

    private void OnDestroy()
    {
        WeaponManager.OnPickupClicked -= TryPickup;
    }
}
