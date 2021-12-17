using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField] private float distanceToDestroy;
    [SerializeField] private float borderSize;
    [SerializeField] private Sprite pointer;
    [SerializeField] private Sprite cross;
    private Vector3 objectivePosition;
    private RectTransform pointerTransform;
    private Image image;

    private void Start()
    {
        pointerTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void Construct(Vector3 objectivePosition)
    {
        this.objectivePosition = objectivePosition;
    }

    private void Update()
    {
        if (Vector3.Distance(PlayerIdentifier.Instance.transform.position, Camera.main.ScreenToWorldPoint(transform.position)) < distanceToDestroy)
            Destroy(gameObject);

        Vector3 playerPos = PlayerIdentifier.Instance.transform.position;
        Vector3 direction = new Vector3(objectivePosition.x - playerPos.x, objectivePosition.y - playerPos.y, 0).normalized;

        float angleBetweenPlayerAndObejctive = Mathf.Acos(direction.x / direction.magnitude) * 180 / Mathf.PI;
        if (direction.y < 0) angleBetweenPlayerAndObejctive *= -1;

        pointerTransform.eulerAngles = new Vector3(0, 0, angleBetweenPlayerAndObejctive);

        Vector3 objectiveScreenPoint = Camera.main.WorldToScreenPoint(objectivePosition);
        bool isOffScreen = objectiveScreenPoint.x <= 0 || objectiveScreenPoint.x >= Screen.width || objectiveScreenPoint.y <= 0 || objectiveScreenPoint.y >= Screen.height;
        
        if (isOffScreen)
        {
            image.sprite = pointer;

            Vector3 cappenObjectiveScreenPosition = new Vector3(objectiveScreenPoint.x, objectiveScreenPoint.y, 0);
            if (cappenObjectiveScreenPosition.x <= borderSize) cappenObjectiveScreenPosition.x = borderSize;
            if (cappenObjectiveScreenPosition.x >= Screen.width - borderSize) cappenObjectiveScreenPosition.x = Screen.width - borderSize;
            if (cappenObjectiveScreenPosition.y <= borderSize) cappenObjectiveScreenPosition.y = borderSize;
            if (cappenObjectiveScreenPosition.y >= Screen.height - borderSize) cappenObjectiveScreenPosition.y = Screen.height - borderSize;

            pointerTransform.position = cappenObjectiveScreenPosition;
            pointerTransform.localPosition = new Vector3(pointerTransform.localPosition.x, pointerTransform.localPosition.y, 0);
        }
        else
        {
            image.sprite = cross;

            pointerTransform.position = objectiveScreenPoint;
            pointerTransform.eulerAngles = new Vector3(0, 0, 0);
        }
    }
}
