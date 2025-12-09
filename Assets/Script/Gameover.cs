using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    [Header("Assign")]
    public GameObject player;          
    public GameObject gameOverUI;      

    private bool isGameOver = false;

    void Start()
    {
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        if (!isGameOver && player == null)
        {
            GameOver();
            Debug.Log("GameOver");
        }
    }

    void GameOver()
    {
        isGameOver = true;
        gameOverUI.SetActive(true);

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
