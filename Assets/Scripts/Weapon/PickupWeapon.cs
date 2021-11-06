using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupWeapon : MonoBehaviour
{
    [SerializeField] private float radius;

    public void Pickup()
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), radius);

        int weaponQuantity = 0;
        foreach (Collider2D item in items)
        {
            if (item.transform.parent == GameManager.Instance.WeaponParent)
                continue;

            if (item.TryGetComponent(out Weapon _))
                weaponQuantity++;
        }

        Collider2D[] weapons = new Collider2D[weaponQuantity];
        int weaponCounter = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].transform.parent == GameManager.Instance.WeaponParent)
                continue;

            if (items[i].TryGetComponent(out Weapon _))
            {
                weapons[weaponCounter] = items[i];
                weaponCounter++;
            }
        }

        if (weapons.Length == 0)
            return;

        int closestWeapon = 0;
        for (int i = 0; i < weapons.Length; i++)
        {
            if (Vector3.Distance(transform.position, weapons[i].transform.position) < Vector3.Distance(transform.position, weapons[closestWeapon].transform.position))
            {
                closestWeapon = i;
            }
        }

        Transform weaponParent = GameManager.Instance.WeaponParent;
        if (weaponParent.childCount != 0)
        {
            Transform currentWeapon = weaponParent.GetChild(0);
            currentWeapon.SetParent(null);
            currentWeapon.position = transform.position;
        }
        weapons[closestWeapon].transform.SetParent(weaponParent);
        weapons[closestWeapon].transform.position = weaponParent.position;
        GameManager.Instance.playerAttack.weapon = weapons[closestWeapon].GetComponent<Weapon>();
    }
}
