using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartSavedGame()
    {
        int currentLevelIndex = PlayerPrefs.GetInt("Level", 0);
        SceneManager.LoadScene("Level" + (currentLevelIndex + 1).ToString());
    }

    public void DeletePlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    public void StartNewGame()
    {
        PlayerPrefs.DeleteAll();
        if (PlayerPrefs.GetInt("TutorialStart", 0) == 0)
        {
            StartTutorial();
        }
        else
        {
            PlayerPrefs.SetInt("TutorialStart", 1);
            SceneManager.LoadScene("Level1");
        }
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetInt("TutorialStart", 1);
        SceneManager.LoadScene("Tutorial");
    }
}