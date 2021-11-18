using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;

    public void ActivatePauseMenu()
    {
        Time.timeScale = 0f;
        menuObject.SetActive(true);
    }

    public void DeactivatePauseMenu()
    {
        Time.timeScale = 1f;
        menuObject.SetActive(false);
    }

    public void OpenMenu()
    {
        DeactivatePauseMenu();

        GameManager.Instance.DontDestroyOnLoadContainer.SetActive(false);

        SceneManager.LoadScene("MainMenu");
    }
}
