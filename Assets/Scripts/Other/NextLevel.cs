using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private int doneLevelNumber;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth _))
        {
            SceneManager.LoadScene(sceneName);
            GameManager.Instance.levels[doneLevelNumber - 1] = true;
        }
    }
}
