using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BuyableWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private int price;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI rangeText;
    [SerializeField] private TextMeshProUGUI damageAmountText;
    [SerializeField] private TextMeshProUGUI pushForceText;
    [SerializeField] private TextMeshProUGUI stunTimeText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI lifetimeText;
    [SerializeField] private Image weaponSprite;
    [SerializeField] private Image projectileSprite;

    public void Construct(Weapon weapon)
    {
        this.weapon = weapon;

        int price;
        float range;
        float damageAmount;
        float pushForce;
        float stunTime;
        float speed;
        float lifetime;
        Sprite weaponSprite;
        Sprite projectileSprite;

        weapon.GetProperties(out price,out range, out damageAmount, out pushForce, out stunTime, out speed, out lifetime, out weaponSprite, out projectileSprite);

        this.price = price;
        priceText.text = price.ToString();
        rangeText.text = range.ToString();
        damageAmountText.text = damageAmount.ToString();
        pushForceText.text = pushForce.ToString();
        stunTimeText.text = stunTime.ToString();
        speedText.text = speed.ToString();
        lifetimeText.text = lifetime.ToString();
        this.weaponSprite.sprite = weaponSprite;
        this.projectileSprite.sprite = projectileSprite;
    }

    public void Buy()
    {
        if (GameManager.Instance.BuyObject(price))
        {
            int weaponCount = PlayerPrefs.GetInt("WeaponCount");
            PlayerPrefs.SetInt("WeaponCount", weaponCount + 1);
            PlayerPrefs.SetInt("Weapon" + weaponCount.ToString(), weapon.index);
            PlayerPrefs.SetInt("WeaponShoots" + weaponCount.ToString(), 0);
        }
        else
        {
            Debug.Log("NOT ENOUGH MONEY");
        }
    }
}
