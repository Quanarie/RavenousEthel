using System.Collections;
using System.Collections.Generic;
using UnityEngine.Advertisements;
using UnityEngine;
using TMPro;

public class MoneyUpdater : MonoBehaviour
{
    private TextMeshProUGUI moneyText;
    private void Start()
    {
        moneyText = GetComponent<TextMeshProUGUI>();
        moneyText.text = PlayerPrefs.GetInt("AllMoney").ToString();
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        UpdateText();
    }

    private void UpdateText()
    {
        moneyText.text = PlayerPrefs.GetInt("AllMoney").ToString();
    }
}
