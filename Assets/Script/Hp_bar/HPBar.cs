using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    [Header("Target Character")]
    public Character target;       // ตัวละครที่มี HP

    [Header("HP Image (BG)")]
    public RectTransform hpRect;   // รูปเลือด (BG) ที่ต้องลด/เพิ่มความกว้าง

    private float maxWidth;        // ความกว้างสูงสุดตอนเลือดเต็ม

    void Start()
    {
        if (target == null || hpRect == null)
        {
            Debug.LogError("UIHpBar_Width: Missing references!");
            enabled = false;
            return;
        }

        // เก็บความกว้างตอนเลือดเต็ม
        maxWidth = hpRect.sizeDelta.x;
    }

    void Update()
    {
        UpdateHpBar();
    }

    void UpdateHpBar()
    {
        float hpPercent = (float)target.currentHp / target.maxHp;

        // ปรับความกว้างตามเปอร์เซ็นต์เลือด
        hpRect.sizeDelta = new Vector2(maxWidth * hpPercent, hpRect.sizeDelta.y);
    }
}
