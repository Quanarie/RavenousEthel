using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    public static Action<int> OnNextLevelTransition = delegate { };

    [SerializeField] private string sceneName;
    private int doneLevelIndex;

    private void Start()
    {
        doneLevelIndex = int.Parse(sceneName[sceneName.Length - 1].ToString()) - 2;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            SceneFader.Instance.FadeOut(sceneName);

            if (doneLevelIndex >= 0)
            {
                OnNextLevelTransition(doneLevelIndex);
            }
        }
    }
}
