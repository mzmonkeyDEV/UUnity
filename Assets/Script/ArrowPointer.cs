using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFollowEnemyFromPlayer : MonoBehaviour
{
    public Transform player;       // Player
    public Transform enemy;        // Enemy
    public Camera mainCamera;      // Main camera (UI conversion)
    public Vector3 offset = new Vector3(0, 2f, 0); // ความสูงเหนือหัวศัตรู

    private RectTransform arrowUI;

    void Start()
    {
        arrowUI = GetComponent<RectTransform>();
        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        if (enemy == null || player == null) return;

        // 1️⃣ Player คำนวณตำแหน่งศัตรู
        Vector3 targetPos = enemy.position + offset;

        // 2️⃣ แปลง world → screen
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPos);

        // 3️⃣ ถ้าศัตรูอยู่ด้านหลัง player → ไม่โชว์
        if (screenPos.z <= 0)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        arrowUI.gameObject.SetActive(true);

        // 4️⃣ แสดงตำแหน่งบน UI
        arrowUI.position = screenPos;

        // 5️⃣ หมุนลูกศรให้มองไปทางศัตรู (คำนวณจาก player)
        Vector3 dir = enemy.position - player.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

        // หมุนบน Canvas (แกน Z)
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
