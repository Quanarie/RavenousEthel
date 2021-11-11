using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    [SerializeField] private TextMesh text;
    [SerializeField] private Text uiText;
    [SerializeField] private int appearTime;

    private void Start()
    {
        text.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            text.gameObject.SetActive(true);
            StopAllCoroutines();
            StartCoroutine(ShowText());

            if (uiText == null)
                return;

            StartCoroutine(ShowUIText());
            uiText.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            StopAllCoroutines();
            StartCoroutine(HideText());

            if (uiText == null)
                return;

            StartCoroutine(HideUIText());
        }
    }

    IEnumerator ShowText()
    {
        Color textColor = text.color;
        textColor.a = 0;
        text.color = textColor;

        float timer = 0;

        while (timer < appearTime)
        {
            timer += Time.deltaTime;

            textColor.a = (1f / appearTime) * timer;
            text.color = textColor;

            yield return null;
        }
    }

    IEnumerator HideText()
    {
        text.gameObject.SetActive(true);
        Color textColor = text.color;
        textColor.a = 1;
        text.color = textColor;

        float timer = appearTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            textColor.a = (1f / appearTime) * timer;
            text.color = textColor;

            yield return null;
        }
        text.gameObject.SetActive(false);
    }

    IEnumerator ShowUIText()
    {
        Color textColor = uiText.color;
        textColor.a = 0;
        uiText.color = textColor;

        float timer = 0;

        while (timer < appearTime)
        {
            timer += Time.deltaTime;

            textColor.a = (1f / appearTime) * timer;
            uiText.color = textColor;

            yield return null;
        }
    }

    IEnumerator HideUIText()
    {
        text.gameObject.SetActive(true);
        Color textColor = uiText.color;
        textColor.a = 1;
        uiText.color = textColor;

        float timer = appearTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            textColor.a = (1f / appearTime) * timer;
            uiText.color = textColor;

            yield return null;
        }
        uiText.gameObject.SetActive(false);
    }
}
