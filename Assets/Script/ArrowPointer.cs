using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFollowEnemyFromPlayer : MonoBehaviour
{
    public Transform player;       
    public Transform enemy;        
    public Camera mainCamera;     
    public Vector3 offset = new Vector3(0, 2f, 0); 

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

       
        Vector3 targetPos = enemy.position + offset;

       
        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetPos);

       
        if (screenPos.z <= 0)
        {
            arrowUI.gameObject.SetActive(false);
            return;
        }

        arrowUI.gameObject.SetActive(true);

       
        arrowUI.position = screenPos;

      
        Vector3 dir = enemy.position - player.position;
        float angle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

      
        arrowUI.rotation = Quaternion.Euler(0, 0, -angle);
    }
}
