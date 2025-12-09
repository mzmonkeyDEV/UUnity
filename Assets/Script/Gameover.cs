using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    [Header("Assign")]
    public GameObject player;          // ลาก Player มาใส่ใน Inspector
    public GameObject gameOverUI;      // ลาก Canvas/PANEL GameOver มาใส่

    private bool isGameOver = false;

    void Start()
    {
        // ปิด GameOver UI ก่อน
        gameOverUI.SetActive(false);
    }

    void Update()
    {
        // ถ้า Player ถูก Destroy หรือหายไปจาก Scene
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

        // หยุดเวลา (ถ้าไม่ต้องการให้คอมเมนต์ทิ้ง)
        Time.timeScale = 0f;
    }

    // เรียกจากปุ่ม Restart
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // เรียกจากปุ่ม Quit (ไม่ต้องมีก็ได้)
    public void QuitGame()
    {
        Application.Quit();
    }
}
