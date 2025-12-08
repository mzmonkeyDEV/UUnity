using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
     public Transform target;  
    public float followSpeed = 8f;

    public Vector3 offset = new Vector3(0, 10f, -7.5f);

    void LateUpdate()
    {
        if (target == null) return;

        //ตำแหน่งที่กล้องควรอยู่
        Vector3 desiredPosition = target.position + offset;

        //movement กล้อง
        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        //มุมมองกล้อง
        transform.rotation = Quaternion.Euler(50f, 0f, 0f);
    }
    
}
