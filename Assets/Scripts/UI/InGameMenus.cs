using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class InGameMenus : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuObject;
    [SerializeField] private GameObject deathMenuObject;

    private void Start()
    {
        GameManager.Instance.playerHealth.OnPlayerDeath += ActivateDeathMenu;
    }

    public void ActivatePauseMenu()
    {
        Time.timeScale = 0f;
        pauseMenuObject.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1f;
        pauseMenuObject.SetActive(false);
    }

    public void ActivateDeathMenu()
    {
        Time.timeScale = 0f;
        deathMenuObject.SetActive(true);
    }

    public void StartLevelAgain()
    {
        DeactivateDeathMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void DeactivateDeathMenu()
    {
        Time.timeScale = 1f;
        deathMenuObject.SetActive(false);
    }

    public void OpenMenu()
    {
        DeactivatePauseMenu();

        GameManager.Instance.DontDestroyOnLoadContainer.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }
}
