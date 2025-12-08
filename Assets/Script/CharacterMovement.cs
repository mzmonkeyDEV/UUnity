using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : Character
{
    [Header("Player")]
    public float moveSpeed = 5f;         
    public Rigidbody rb;                
    public Animator animator;           
    public Camera cam;                  

    Vector3 movement;

    [Header("Attack")]
    public float attackCooldown = 0.5f;
    private bool isAttacking = false;
    private float nextAttackTime = 0f;


    void Update()
    {
        if (isAttacking)
        {
            animator.SetFloat("Speed", 0);
            return;
        }

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

        if (Input.GetButtonDown("Fire1") && Time.time >= nextAttackTime)
        {
            StartCoroutine(DoAttack());
        }
    }

    void FixedUpdate()
    {
        if (!isAttacking)
        {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            animator.SetFloat("Atk", 0);
        }

    }
    IEnumerator DoAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        animator.SetFloat("Atk", 1);

        yield return new WaitForSeconds(0.4f);

        isAttacking = false;
    }
}
