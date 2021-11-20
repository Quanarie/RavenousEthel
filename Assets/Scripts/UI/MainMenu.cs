using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private int levelQuantity;
    [SerializeField] private Button levelButtonPrefab;
    [SerializeField] private RectTransform content;

    private void Start()
    {
        int currentLevel = PlayerPrefs.GetInt("Level") + 1;

        for (int i = 1; i <= levelQuantity; i++)
        {
            Button levelButton = Instantiate(levelButtonPrefab, content.transform);
            Text lvlButtonText = levelButton.GetComponentInChildren<Text>();

            lvlButtonText.text = i.ToString();
            if (i > currentLevel)
                continue;

            lvlButtonText.color = Color.green;

            levelButton.onClick.AddListener(
            () =>
            {
                SceneManager.LoadScene("Level" + lvlButtonText.text);
            });
        }
    }

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