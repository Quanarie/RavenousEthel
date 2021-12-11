using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    public delegate void PickupClick();
    public static event PickupClick OnPickupClicked;

    [SerializeField] private float radius;
    public Image rechargeImage;
    [SerializeField] protected Image weaponImage;
    [SerializeField] protected Sprite weaponSpriteStandart;

    [HideInInspector] public int currentWeapon = 0;
    private List<Weapon> weapons = new List<Weapon>();

    private void Start()
    {
        weapons.Add(null);
        weaponImage.sprite = weaponSpriteStandart;
    }

    public void AddWeapon(Weapon weapon)
    {
        weapons.Add(weapon);
        currentWeapon++;
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

            Vector2 randomOffset = Random.insideUnitCircle / 3;
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
        }

        PlayerIdentifier.Instance.Attack.SetWeapon(weapons[currentWeapon]);
    }

    public void Pickup()
    {
        OnPickupClicked?.Invoke();

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
        PlayerIdentifier.Instance.Attack.SetWeapon(newWeapon);
        this.weapons.Add(newWeapon);
        if (this.weapons[currentWeapon] != null)
            this.weapons[currentWeapon].gameObject.SetActive(false);
        currentWeapon++;
        newWeapon.UpdateWeaponStock();

        weaponImage.sprite = newWeapon.gameObject.GetComponent<SpriteRenderer>().sprite;

        newWeapon.gameObject.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        newWeapon.gameObject.transform.localScale = new Vector3(1, 1, 0);

        newWeapon.rechargeImage = rechargeImage;
    }
}
