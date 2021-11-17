using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void StartSavedGame()
    {
        SceneManager.LoadScene("Level1");
    }

    public void StartNewGame()
    {
        if (PlayerPrefs.GetInt("TutorialStart", 0) == 0)
        {
            StartTutorial();
        }
        else
        {
            SceneManager.LoadScene("Level1");
        }
    }

    public void StartTutorial()
    {
        PlayerPrefs.SetInt("TutorialStart", 1);
        SceneManager.LoadScene("Tutorial");
    }

}