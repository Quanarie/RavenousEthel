using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameMenus : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private GameObject deathMenuObject;

    [SerializeField] private AnimationClip pauseMenuClip;
    [SerializeField] private AnimationClip deathMenuClip;

    private Animator pauseMenuAnimator;
    private Animator deathMenuAnimator;

    private void Start()
    {
        PlayerIdentifier.Instance.Health.OnPlayerDeath += ActivateDeathMenu;

        pauseMenuAnimator = pauseMenuObject.GetComponent<Animator>();
        deathMenuAnimator = deathMenuObject.GetComponent<Animator>();

        SceneManager.sceneLoaded += CloseAllSilently;
    }

    private void CloseAllSilently(Scene scene, LoadSceneMode load)
    {
        pauseMenuObject.SetActive(false);
        deathMenuObject.SetActive(false);

        DeactivatePauseMenu();
        DeactivateDeathMenu();
    }

    public void ActivatePauseMenu()
    {
        pauseMenuObject.SetActive(true);
        pauseMenuAnimator.SetTrigger("open");

        StartCoroutine(SetTimeZero(pauseMenuClip.length));
    }

    private IEnumerator SetTimeZero(float time)
    {
        yield return new WaitForSeconds(time);
        Time.timeScale = 0f;
    }

    public void ChangePauseMenuState()
    {
        pauseMenuObject.SetActive(true);
        pauseMenuAnimator.SetTrigger("open");
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenuAnimator.SetTrigger("close");
    }

    public void ActivateDeathMenu()
    {
        pauseMenuObject.SetActive(true);
        deathMenuAnimator.SetTrigger("open");

        StartCoroutine(SetTimeZero(deathMenuClip.length));
    }

    public void DeactivateDeathMenu()
    {
        Time.timeScale = 1f;
        deathMenuAnimator.SetTrigger("close");
    }

    public void StartLevelAgain()
    {
        DeactivateDeathMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


    public void OpenMenu()
    {
        GameManager.Instance.DontDestroyOnLoadContainer.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }
}
