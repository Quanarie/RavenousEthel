using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
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
            SceneManager.LoadScene(sceneName);
            if (doneLevelIndex >= 0)
                GameManager.Instance.levels[doneLevelIndex] = true;

            if (SceneManager.GetActiveScene().name == "Tutorial")
                return;

            GameManager.Instance.SaveState();
        }
    }
}
