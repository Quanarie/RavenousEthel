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

    public Transform EnemiesParent;

    public State state;

    public FloatingTextManager floatingTextManager;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

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
        if (EnemiesParent.childCount == 0)
            return null;

        Transform closestEnemy = EnemiesParent.GetChild(0);
        Vector3 playerPos = Player.position;

        foreach (Transform enemy in EnemiesParent)
        {
            if (Vector3.Distance(enemy.position, playerPos) < Vector3.Distance(closestEnemy.position, playerPos))
            {
                closestEnemy = enemy;
            }
        }

        if (Vector3.Distance(closestEnemy.position, playerPos) <= range)
        {
            return closestEnemy;
        }
        return null;
    }

}
