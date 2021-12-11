using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMutation : MonoBehaviour
{
    public Action<float> OnMutation = delegate { };

    [SerializeField] private GameObject deadSlimePrefab;
    private WeaponManager weaponManager;

    private void Start()
    {
        weaponManager = GetComponent<WeaponManager>();

        PlayerIdentifier.Instance.Health.OnDemutation += DeMutate;
    }

    public void Mutate(GameObject enemy)
    {
        Instantiate(deadSlimePrefab, transform.position, Quaternion.identity);

        OnMutation?.Invoke(enemy.GetComponent<EnemyHealth>().corpse.GetComponent<EnemyCorpse>().toHeal);
        transform.position = enemy.transform.position;
        Destroy(enemy);

        CircleCollider2D hitbox = PlayerIdentifier.Instance.HitBox;
        hitbox.radius = 0.115f;
        hitbox.offset = new Vector3(0f, 0.115f, 0f);
    }

    public void DeMutate()
    {
        GameManager.Instance.state = GameManager.State.regular;

        CircleCollider2D hitbox = PlayerIdentifier.Instance.HitBox;
        hitbox.radius = 0.09f;
        hitbox.offset = new Vector3(0f, 0.09f, 0f);

        weaponManager.ClearWeapons();
    }
}
