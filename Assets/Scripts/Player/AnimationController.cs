using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public RuntimeAnimatorController regularController;
    public RuntimeAnimatorController mutatedController;

    [SerializeField] private GameObject deadMonsterPrefab;
    [SerializeField] private AnimationClip monsterDeath;

    private void Start()
    {
        GameManager.Instance.playerAttack.OnMutation += Mutate;
        GameManager.Instance.playerHealth.OnDemutation += DeMutate;
    }

    public void Mutate(float compatibility)
    {
        GameManager.Instance.playerAnimator.runtimeAnimatorController = mutatedController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
    }

    public void DeMutate()
    {
        GameManager.Instance.playerAnimator.SetTrigger("death");
        StartCoroutine(ChangeStateToRegular());

    }

    IEnumerator ChangeStateToRegular()
    {
        yield return new WaitForSeconds(monsterDeath.length);

        GameManager.Instance.playerAnimator.runtimeAnimatorController = regularController;
        GameManager.Instance.playerAnimator.SetTrigger("transform");
        Instantiate(deadMonsterPrefab, transform.position, Quaternion.identity);
    }
}
