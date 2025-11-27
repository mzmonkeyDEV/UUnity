using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public float moveSpeed = 5f;         
    public Rigidbody rb;                
    public Animator animator;           
    public Camera cam;                  

    Vector3 movement;

    void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        movement = new Vector3(x, 0, z).normalized;

        if (movement == Vector3.zero)
        {
            animator.SetFloat("Speed", 0); //idle
        }
        else
        {
            animator.SetFloat("Speed", 1); //Run
        }

        if (movement.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(movement.x, movement.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);
            //Rotation 
        }

    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);

    }
}
