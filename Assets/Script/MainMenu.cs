using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu1 : MonoBehaviour
{
    public GameObject GameObject;


     void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameObject.SetActive(true);
            Time.timeScale = 0;
        }
     }
    public void BackMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }
    public void ResumeGame()
    {
        GameObject.SetActive(false);
        Time.timeScale = 1;
    }
    public void PlayGame()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync(1);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
