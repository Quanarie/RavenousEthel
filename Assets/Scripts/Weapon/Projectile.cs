using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected float damageAmount;
    protected float pushForce;
    protected float stunTime;
    protected float speed;
    protected float lifetime;

    [HideInInspector] public Vector3 direction;
    [HideInInspector] public float angleBetweenEnemyAndWeapon;
    public static float offsetY = 0.175f;

    private EnemyHealth enemyToAttack;

    public void Construct(float dmg, float push, float stun, float speed, float lifetime, Sprite sprite)
    {
        damageAmount = dmg;
        pushForce = push;
        stunTime = stun;
        this.speed = speed;
        this.lifetime = lifetime;
        GetComponent<SpriteRenderer>().sprite = sprite;
    }

    protected virtual void Start()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, angleBetweenEnemyAndWeapon);
    }

    protected virtual void Update()
    {
        transform.Translate(new Vector3(direction.x, direction.y, 0).normalized * speed * Time.deltaTime, Space.World);

        Destroy(gameObject, lifetime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out enemyToAttack))
        {
            Vector3 enemyPos = enemyToAttack.transform.position;
            Vector3 playerPos = GameManager.Instance.Player.position;

            enemyToAttack.ReceiveDamage(damageAmount, new Vector3(enemyPos.x - playerPos.x, enemyPos.y - playerPos.y, 0).normalized * pushForce, stunTime);

            Destroy(gameObject);
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacles"))
        {
            Destroy(gameObject);
        }
    }
}
