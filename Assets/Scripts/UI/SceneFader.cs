using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneFader : MonoBehaviour
{
    public static SceneFader Instance;

    private Animator animator;
    [SerializeField] private AnimationClip fadingClip;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        animator = GetComponentInChildren<Animator>();
    }

    public void FadeOut(string sceneToLoad)
    {
        animator.SetTrigger("fadeOut");

        StartCoroutine(WaitAndLoad(fadingClip.length, sceneToLoad));
    }
    public void FadeIn()
    {
        animator.SetTrigger("fadeIn");
    }

    private IEnumerator WaitAndLoad(float timeToWait, string sceneName)
    {
        yield return new WaitForSeconds(timeToWait);

        SceneManager.LoadScene(sceneName);
    }
}
