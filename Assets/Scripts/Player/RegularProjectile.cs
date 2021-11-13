using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegularProjectile : MonoBehaviour
{
    [SerializeField] private float speedOfLerping;
    [SerializeField] private float distanceToMutate;
    [SerializeField] private float range;

    private Transform enemyToAttack;
    private Camera mainCamera;

    private void Start()
    {
        Time.timeScale = 0;
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (enemyToAttack != null)
        {
            if (GameManager.Instance.state == GameManager.State.regular)
                GameManager.Instance.playerAnimator.SetTrigger("attack");
            else GameManager.Instance.playerAnimator.SetTrigger("transform");

            GameManager.Instance.playerAnimator.SetFloat("enemyPosX",
            (enemyToAttack.position.x - GameManager.Instance.Player.position.x) / Mathf.Abs(enemyToAttack.position.x - GameManager.Instance.Player.position.x));

            Vector3 endPos = new Vector3(enemyToAttack.position.x, enemyToAttack.position.y + Projectile.offsetY, 0);

            transform.position = Vector3.Lerp(transform.position, endPos, speedOfLerping);

            if (Vector3.Distance(transform.position, endPos) < distanceToMutate)
            {
                GameManager.Instance.playerAttack.Mutate(enemyToAttack.gameObject);
                Destroy(gameObject);
            }

            return;
        }

        Touch[] touches = Input.touches;

        if (touches.Length == 0)
            return;

        foreach (Touch touch in touches)
        {
            Collider2D[] items = Physics2D.OverlapPointAll(mainCamera.ScreenToWorldPoint(touch.position));

            if (items.Length == 0)
                continue;

            foreach (Collider2D item in items)
            {
                if (item.TryGetComponent(out EnemyHealth _))
                {
                    enemyToAttack = item.transform;
                    Time.timeScale = 1;
                    return;
                }
            }
        }
    }
}