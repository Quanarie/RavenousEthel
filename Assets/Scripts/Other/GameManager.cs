using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Transform Player;

    public Transform WeaponParent;

    [HideInInspector] public Animator playerAnimator;
    [HideInInspector] public PlayerAttack playerAttack;
    [HideInInspector] public PlayerHealth playerHealth;

    [HideInInspector] public CircleCollider2D playerHitBox;

    public Joystick Joystick;
    public Button AttackButton;

    public State state;

    public FloatingTextManager floatingTextManager;

    [SerializeField] private GameObject DontDestroyOnLoadContainer;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(DontDestroyOnLoadContainer);
            return;
        }

        playerAnimator = Player.GetComponent<Animator>();
        playerAttack = Player.GetComponent<PlayerAttack>();
        playerHealth = Player.GetComponent<PlayerHealth>();
        playerHitBox = Player.GetComponent<CircleCollider2D>();
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
