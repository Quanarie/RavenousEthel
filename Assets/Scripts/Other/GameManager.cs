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

    [HideInInspector] public CircleCollider2D playerHitBox;

    public Joystick Joystick;
    public Button AttackButton;

    public State state;

    public FloatingTextManager floatingTextManager;

    public GameObject DontDestroyOnLoadContainer;

    [HideInInspector] public bool[] levels = new bool[14];
    public Weapon[] weapons = new Weapon[9];

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(DontDestroyOnLoadContainer);
            return;
        }

        Instance = this;

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneLoaded += SaveState;

        playerAnimator = Player.GetComponent<Animator>();
        playerAttack = Player.GetComponent<PlayerAttack>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerHitBox = Player.GetComponent<CircleCollider2D>();
        weaponManager = Player.GetComponent<WeaponManager>();

        LoadState();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            DontDestroyOnLoadContainer.SetActive(true);
        }

        GameObject spawnPoint = GameObject.Find("Respawn");

        if (spawnPoint != null)
        {
            Player.transform.position = spawnPoint.transform.position;
        }
    }

    public void SaveState(Scene scene, LoadSceneMode mode)
    {
        PlayerPrefs.SetInt("Level", GetCurrentLevel());

        if (state == State.regular)
            PlayerPrefs.SetInt("State", 0);
        else
            PlayerPrefs.SetInt("State", 1);

        PlayerPrefs.SetFloat("Health", playerHealth.currentHp);

        PlayerPrefs.SetFloat("Size", Player.transform.localScale.x);

        PlayerPrefs.SetInt("WeaponCount", WeaponParent.childCount);
        for (int i = 0; i < WeaponParent.childCount; i++)
        {
            PlayerPrefs.SetInt("Weapon" + i.ToString(), WeaponParent.GetChild(i).GetComponent<Weapon>().index);
            PlayerPrefs.SetInt("WeaponShoots" + i.ToString(), WeaponParent.GetChild(i).GetComponent<Weapon>().currentShotQuantity);
        }
    }

    public void LoadState()
    {
        SetCurrentLevel(PlayerPrefs.GetInt("Level"));

        if (PlayerPrefs.GetInt("State") == 0)
        {
            state = State.regular;
        }
        else
        {
            playerAttack.Mutate(null);
            for (int i = 0; i < WeaponParent.childCount; i++)
            {
                Destroy(WeaponParent.GetChild(i));
            }

            for (int i = 0; i < PlayerPrefs.GetInt("WeaponCount"); i++)
            {
                Weapon weapon = Instantiate(weapons[PlayerPrefs.GetInt("Weapon" + i.ToString())], transform.position, transform.rotation, WeaponParent);
                weapon.currentShotQuantity = PlayerPrefs.GetInt("WeaponShoots" + i.ToString());
                weapon.transform.localPosition = Vector3.zero;
                weapon.rechargeImage = weaponManager.rechargeImage;
                weapon.UpdateWeaponStock();
                playerAttack.weapon = weapon;
            }
        }

        playerHealth.currentHp = PlayerPrefs.GetFloat("Health");

        Player.transform.localScale = new Vector3(PlayerPrefs.GetFloat("Size"), PlayerPrefs.GetFloat("Size"), 0);
    }

    private int GetCurrentLevel()
    {
        int levelsDone = 0;
        foreach (bool isLevelDone in levels)
        {
            if (isLevelDone)
                levelsDone++;
        }

        return levelsDone + 1;
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
