using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform WeaponParent;
    public WeaponManager weaponManager;
    public Slider weaponStock;

    public GameObject pickupableArrowObject;

    public State state;

    public FloatingTextManager floatingTextManager;

    public GameObject DontDestroyOnLoadContainer;

    public bool[] levels;
    public Weapon[] weapons;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(DontDestroyOnLoadContainer);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(DontDestroyOnLoadContainer);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (SceneManager.GetActiveScene().name == "MainMenu")
            DontDestroyOnLoadContainer.SetActive(false);
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;

        LoadState();

        DontDestroyOnLoadContainer.SetActive(true);

        GameObject spawnPoint = GameObject.Find("Respawn");

        if (spawnPoint != null)
        {
            PlayerIdentifier.Instance.transform.position = spawnPoint.transform.position;
        }
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt("Level", GetCurrentLevel());

        if (state == State.regular)
            PlayerPrefs.SetInt("State", 0);
        else
            PlayerPrefs.SetInt("State", 1);

        PlayerPrefs.SetFloat("Health", PlayerIdentifier.Instance.Health.currentHp);

        PlayerPrefs.SetFloat("Size", PlayerIdentifier.Instance.transform.localScale.x);

        PlayerPrefs.SetInt("WeaponCount", WeaponParent.childCount);
        PlayerPrefs.SetInt("CurrentWeapon", weaponManager.currentWeapon);
        for (int i = 0; i < WeaponParent.childCount; i++)
        {
            PlayerPrefs.SetInt("Weapon" + i.ToString(), WeaponParent.GetChild(i).GetComponent<Weapon>().index);
            PlayerPrefs.SetInt("WeaponShoots" + i.ToString(), WeaponParent.GetChild(i).GetComponent<Weapon>().currentShotQuantity);
        }
    }

    public void LoadState()
    {
        SetCurrentLevel(PlayerPrefs.GetInt("Level", 0));

        weaponManager.DeleteWeapons();
        if (PlayerPrefs.GetInt("State", 0) == 0)
        {
            state = State.regular;
            PlayerIdentifier.Instance.Animator.runtimeAnimatorController = PlayerIdentifier.Instance.AnimationChanger.regularController;
        }
        else
        {
            PlayerIdentifier.Instance.Health.Mutate(0f);
            PlayerIdentifier.Instance.Animator.runtimeAnimatorController = PlayerIdentifier.Instance.AnimationChanger.mutatedController;

            int currentWeapon = PlayerPrefs.GetInt("CurrentWeaponCount");
            for (int i = 0; i < PlayerPrefs.GetInt("WeaponCount", 0); i++)
            {
                Weapon weapon = Instantiate(weapons[PlayerPrefs.GetInt("Weapon" + i.ToString())], transform.position, transform.rotation, WeaponParent);

                weapon.currentShotQuantity = PlayerPrefs.GetInt("WeaponShoots" + i.ToString());
                weapon.transform.localPosition = Vector3.zero;
                weapon.rechargeImage = weaponManager.rechargeImage;

                weapon.UpdateWeaponStock();
                weaponManager.AddWeapon(weapon);

                if (i == currentWeapon)
                {
                    PlayerIdentifier.Instance.Attack.SetWeapon(weapon);
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
            }
        }

        PlayerIdentifier.Instance.Health.SetCurrentHealth(PlayerPrefs.GetFloat("Health", 0f));
        
        float size = PlayerPrefs.GetFloat("Size", 1f);
        PlayerIdentifier.Instance.transform.localScale = new Vector3(size, size, 0);
    }

    private int GetCurrentLevel()
    {
        int levelsDone = 0;
        foreach (bool isLevelDone in levels)
        {
            if (isLevelDone)
                levelsDone++;
        }

        return levelsDone;
    }

    private void SetCurrentLevel(int level)
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i < level)
            {
                levels[i] = true;
            }
        }
    }

    public enum State
    {
        regular,
        mutated,
    }
}
