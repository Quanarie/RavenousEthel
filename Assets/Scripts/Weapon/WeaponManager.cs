using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] protected Image rechargeImage;
    [SerializeField] protected Image weaponImage;

    private int currentWeapon = 0;
    private List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        GameManager.Instance.WeaponParent.GetChild(0).GetComponent<Weapon>().rechargeImage = rechargeImage;
        weapons.Add(GameManager.Instance.playerAttack.weapon);
        weaponImage.sprite = weapons[0].gameObject.GetComponent<SpriteRenderer>().sprite;
    }

    public void ClearWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == GameManager.Instance.playerAttack.startWeapon)
                continue;

            weapons[i].transform.SetParent(null);

            Vector2 randomOffset = Random.insideUnitCircle / 4;
            weapons[i].transform.position += new Vector3(randomOffset.x, randomOffset.y, 0);

            weapons[i].gameObject.SetActive(true);
        }

        weapons = new List<Weapon>()
        {
            weapons[0]
        };

        weaponImage.sprite = weapons[0].GetComponent<SpriteRenderer>().sprite;

        currentWeapon = 0;
    }

    public void NextWeapon()
    {
        weapons[currentWeapon].gameObject.SetActive(false);

        if (currentWeapon + 1 >= weapons.Count)
        {
            currentWeapon = 0;
        }
        else
        {
            currentWeapon++;
        }
        weapons[currentWeapon].gameObject.SetActive(true);
        weaponImage.sprite = weapons[currentWeapon].gameObject.GetComponent<SpriteRenderer>().sprite;

        GameManager.Instance.playerAttack.weapon = weapons[currentWeapon];
    }

    public void Pickup()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
            return;

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
        weapons[closestWeapon].transform.SetParent(weaponParent);
        weapons[closestWeapon].transform.position = weaponParent.position;

        Weapon newWeapon = weapons[closestWeapon].GetComponent<Weapon>();
        GameManager.Instance.playerAttack.weapon = newWeapon;
        this.weapons.Add(newWeapon);
        this.weapons[currentWeapon].gameObject.SetActive(false);
        currentWeapon++;

        weaponImage.sprite = newWeapon.gameObject.GetComponent<SpriteRenderer>().sprite;

        weapons[closestWeapon].GetComponent<Weapon>().rechargeImage = rechargeImage;
    }
}
