using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player;
    public Transform WeaponParent;
    public WeaponManager weaponManager;
    public Slider weaponStock;

    public GameObject pickupableArrowObject;

    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public PlayerHealth playerHealth;
    [HideInInspector] public PlayerMovement playerMovement;

    [HideInInspector] public CircleCollider2D playerHitBox;

    public Joystick Joystick;
    public Button AttackButton;

    public State state;

    public FloatingTextManager floatingTextManager;

    public GameObject DontDestroyOnLoadContainer;

    public bool[] levels = new bool[14];
    public Weapon[] weapons;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(DontDestroyOnLoadContainer);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;

        playerAnimator = Player.GetComponent<Animator>();
        playerAttack = Player.GetComponent<PlayerAttack>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerMovement = Player.GetComponent<PlayerMovement>();
        playerHitBox = Player.GetComponent<CircleCollider2D>();
        weaponManager = Player.GetComponent<WeaponManager>();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
            return;

        DontDestroyOnLoadContainer.SetActive(true);
        LoadState();

        GameObject spawnPoint = GameObject.Find("Respawn");

        if (spawnPoint != null)
        {
            Player.transform.position = spawnPoint.transform.position;
        }
    }

    public void SaveState()
    {
        PlayerPrefs.SetInt("Level", GetCurrentLevel());

        if (state == State.regular)
            PlayerPrefs.SetInt("State", 0);
        else
            PlayerPrefs.SetInt("State", 1);

        PlayerPrefs.SetFloat("Health", playerHealth.currentHp);

        PlayerPrefs.SetFloat("Size", Player.transform.localScale.x);

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

        if (PlayerPrefs.GetInt("State", 0) == 0)
        {
            state = State.regular;
            playerAnimator.runtimeAnimatorController = playerAttack.regularController;
        }
        else
        {
            state = State.mutated;
            playerAnimator.runtimeAnimatorController = playerAttack.mutatedController;
            for (int i = 0; i < WeaponParent.childCount; i++)
            {
                Destroy(WeaponParent.GetChild(i).gameObject);
            }

            weaponManager.DeleteWeapons();

        
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
                    playerAttack.weapon = weapon;
                }
                else
                {
                    weapon.gameObject.SetActive(false);
                }
            }
        }

        playerHealth.currentHp = PlayerPrefs.GetFloat("Health", playerHealth.maxRegularHp);

        float size = PlayerPrefs.GetFloat("Size", 1f);
        Player.transform.localScale = new Vector3(size, size, 0);
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

    public Transform FindClosestEnemyInRange(float range)
    {
        Collider2D[] items = Physics2D.OverlapCircleAll(new Vector2(Player.transform.position.x, Player.transform.position.y), range);

        int enemiesQuantity = 0;
        foreach (Collider2D item in items)
        {
            if (item.TryGetComponent(out EnemyHealth _))
                enemiesQuantity++;
        }

        Collider2D[] enemies = new Collider2D[enemiesQuantity];
        int enemyCounter = 0;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i].TryGetComponent(out EnemyHealth _))
            {
                enemies[enemyCounter] = items[i];
                enemyCounter++;
            }
        }

        if (enemies.Length == 0)
            return null;

        int closestEnemy = 0;
        for (int i = 0; i < enemies.Length; i++)
        {
            if (Vector3.Distance(Player.transform.position, enemies[i].transform.position) < Vector3.Distance(Player.transform.position, enemies[closestEnemy].transform.position))
            {
                closestEnemy = i;
            }
        }

        return enemies[closestEnemy].transform;
    }
}
