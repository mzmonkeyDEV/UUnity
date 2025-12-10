using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpinY : MonoBehaviour
{
    public float speed = 360f;

    void Update()
    {
        transform.Rotate(0, speed * Time.deltaTime, 0);
    }
}