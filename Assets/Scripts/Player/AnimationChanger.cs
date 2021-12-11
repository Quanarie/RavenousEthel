using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChanger : MonoBehaviour
{
    public RuntimeAnimatorController regularController;
    public RuntimeAnimatorController mutatedController;

    [SerializeField] private GameObject deadMonsterPrefab;
    [SerializeField] private AnimationClip monsterDeath;

    private void Start()
    {
        PlayerIdentifier.Instance.Mutation.OnMutation += Mutate;
        PlayerIdentifier.Instance.Health.OnDemutation += DeMutate;
    }

    public void Mutate(float compatibility)
    {
        PlayerIdentifier.Instance.Animator.runtimeAnimatorController = mutatedController;
        PlayerIdentifier.Instance.Animator.SetTrigger("transform");
    }

    public void DeMutate()
    {
        PlayerIdentifier.Instance.Animator.SetTrigger("death");
        StartCoroutine(ChangeStateToRegular());

    }

    IEnumerator ChangeStateToRegular()
    {
        yield return new WaitForSeconds(monsterDeath.length);

        PlayerIdentifier.Instance.Animator.runtimeAnimatorController = regularController;
        PlayerIdentifier.Instance.Animator.SetTrigger("transform");
        Instantiate(deadMonsterPrefab, transform.position, Quaternion.identity);
    }
}
