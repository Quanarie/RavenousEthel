using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public Image rechargeImage;
    [SerializeField] private float radius;
    [SerializeField] protected Image weaponImage;
    [SerializeField] protected Sprite weaponSpriteStandart;

    [HideInInspector] public int currentWeapon = 0;
    private List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        weaponImage.sprite = weaponSpriteStandart;

        PlayerIdentifier.Instance.Input.OnWeaponButtonPressed += NextWeapon;
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        NextWeapon();
    }

    public void Break(Weapon weapon)
    {
        GameManager.Instance.weaponStock.value = 0;
        currentWeapon = 0;
        weaponImage.sprite = weaponSpriteStandart;

        weapons.Remove(weapon);
        Destroy(weapon.gameObject);
    }

    public void ClearWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] == null)
                continue;

            weapons[i].transform.SetParent(null);

            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle / 3;
            weapons[i].transform.position += new Vector3(randomOffset.x, randomOffset.y, 0);

            weapons[i].gameObject.SetActive(true);
        }

        weapons = new List<Weapon>();
        weapons.Add(null);
        weaponImage.sprite = weaponSpriteStandart;

        currentWeapon = 0;
    }

    public void DeleteWeapons()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (weapons[i] != null)
                Destroy(weapons[i].gameObject);
        }

        weapons = new List<Weapon>();
        weapons.Add(null);
        weaponImage.sprite = weaponSpriteStandart;

        currentWeapon = 0;
    }

    public void NextWeapon()
    {
        if (weapons[currentWeapon] != null)
            weapons[currentWeapon].gameObject.SetActive(false);

        currentWeapon++;
        if (currentWeapon >= weapons.Count)
        {
            currentWeapon = 0;
            weaponImage.sprite = weaponSpriteStandart;
        }

        if (weapons[currentWeapon] != null)
        {
            weapons[currentWeapon].gameObject.SetActive(true);
            weapons[currentWeapon].UpdateWeaponStock();
            weaponImage.sprite = weapons[currentWeapon].gameObject.GetComponent<SpriteRenderer>().sprite;
            print(weapons[currentWeapon].gameObject.GetComponent<SpriteRenderer>().sprite.name);
        }
        else
        {
            weaponImage.sprite = weaponSpriteStandart;
        }

        SetWeapon(currentWeapon);
    }

    public void ChangeCurrentWeapon(int current)
    {
        if (weapons[currentWeapon] != null)
            weapons[currentWeapon].gameObject.SetActive(false);

        currentWeapon = current;

        if (weapons[currentWeapon] != null)
        {
            weapons[currentWeapon].gameObject.SetActive(true);
            weapons[currentWeapon].UpdateWeaponStock();
            weaponImage.sprite = weapons[currentWeapon].gameObject.GetComponent<SpriteRenderer>().sprite;
        }

        SetWeapon(currentWeapon);
    }

    private void SetWeapon(int index) => PlayerIdentifier.Instance.Attack.SetWeapon(weapons[index]);

    public bool Pickup()
    {
        if (GameManager.Instance.state == GameManager.State.regular)
            return false;

        Weapon newWeapon = FindClosestWeapon(weapons);

        if (newWeapon == null)
            return false;

        Transform weaponParent = GameManager.Instance.WeaponParent;
        newWeapon.transform.SetParent(weaponParent);
        newWeapon.transform.position = weaponParent.position;

        PlayerIdentifier.Instance.Attack.SetWeapon(newWeapon);
        if (weapons[currentWeapon] != null)
            weapons[currentWeapon].gameObject.SetActive(false);

        weapons.Add(newWeapon);
        currentWeapon++;
        SetWeapon(currentWeapon);

        newWeapon.UpdateWeaponStock();

        weaponImage.sprite = newWeapon.gameObject.GetComponent<SpriteRenderer>().sprite;

        newWeapon.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        newWeapon.rechargeImage = rechargeImage;

        return true;
    }

    private Weapon FindClosestWeapon(List<Weapon> weapons)
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

        Collider2D[] weaponsLocal = new Collider2D[weaponQuantity];
        int weaponCounter = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].transform.parent == GameManager.Instance.WeaponParent)
                continue;

            if (items[i].TryGetComponent(out Weapon _))
            {
                weaponsLocal[weaponCounter] = items[i];
                weaponCounter++;
            }
        }

        if (weaponsLocal.Length == 0)
            return null;

        int closestWeapon = 0;
        for (int i = 0; i < weaponsLocal.Length; i++)
        {
            if (Vector3.Distance(transform.position, weaponsLocal[i].transform.position) < Vector3.Distance(transform.position, weaponsLocal[closestWeapon].transform.position))
            {
                closestWeapon = i;
            }
        }

        return weaponsLocal[closestWeapon].GetComponent<Weapon>();
    }
}
