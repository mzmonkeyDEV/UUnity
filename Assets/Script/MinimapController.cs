using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
    [Header("Player & Camera")]
    public Transform player;                   
    public Camera minimapCamera;               

    [Header("Settings")]
    public bool rotateWithPlayer = false;      
    public float cameraHeight = 50f;           
    public float mapScale = 2f;                

    [Header("UI")]
    public RectTransform minimapPanel;         
    public RectTransform playerIcon;           

    void LateUpdate()
    {
        UpdateCameraFollow();
        UpdatePlayerIcon();
    }

    
    void UpdateCameraFollow()
    {
        if (minimapCamera == null || player == null) return;

        
        minimapCamera.transform.position = new Vector3(
            player.position.x,
            cameraHeight,
            player.position.z
        );

       
        if (rotateWithPlayer)
        {
            minimapCamera.transform.rotation =
                Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
        else
        {
            minimapCamera.transform.rotation =
                Quaternion.Euler(90f, 0f, 0f);
        }
    }

  
    void UpdatePlayerIcon()
    {
        if (playerIcon == null || player == null) return;

        if (rotateWithPlayer)
        {
            
            playerIcon.localRotation = Quaternion.identity;
        }
        else
        {
            
            playerIcon.localRotation =
                Quaternion.Euler(0f, 0f, -player.eulerAngles.y);
        }
    }

   
    public Vector2 WorldToMinimapPosition(Vector3 worldPos)
    {
        Vector3 offset = worldPos - player.position;

       
        if (rotateWithPlayer)
        {
            float angle = -player.eulerAngles.y * Mathf.Deg2Rad;
            float rotatedX = offset.x * Mathf.Cos(angle) - offset.z * Mathf.Sin(angle);
            float rotatedZ = offset.x * Mathf.Sin(angle) + offset.z * Mathf.Cos(angle);
            offset = new Vector3(rotatedX, 0, rotatedZ);
        }

        return new Vector2(offset.x * mapScale, offset.z * mapScale);
    }
}
