using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopWeaponLoader : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject containerPrefab;

    private void Start()
    {
        int weaponQuantity = GameManager.Instance.weapons.Length;

        for (int i = 0; i < weaponQuantity; i++)
        {
            GameObject newWeaponItem = Instantiate(containerPrefab, content.transform);
            newWeaponItem.GetComponent<BuyableWeapon>().Construct(GameManager.Instance.weapons[i]);
        }
    }
}
