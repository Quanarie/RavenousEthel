using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class MoneyUpdaterRealTime : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    private void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        moneyText.text = GameManager.Instance.GetCurrentMoney().ToString();

        GameManager.Instance.OnMoneyAdded += UpdateText;
        SceneManager.sceneLoaded += UpdateText;
    }

    private void UpdateText()
    {
        moneyText.text = GameManager.Instance.GetCurrentMoney().ToString();
    }

    private void UpdateText(Scene scene, LoadSceneMode mode)
    {
        UpdateText();
    }
}
