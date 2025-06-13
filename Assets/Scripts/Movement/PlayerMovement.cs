using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public Vector2 movement;
    public float moveSpeedModifier = 1.00f;
    public float finalSpeed;

    public bool isWalking = false; // Animation flag
    private Animator animator;     // Animator reference
    public Vector2 lastDirection { get; private set; }
    public Dash dash;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Assumes Animator is on the same GameObject
        finalSpeed = moveSpeed * moveSpeedModifier;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement != Vector2.zero)
        {
            lastDirection = movement.normalized;
        }

        isWalking = movement != Vector2.zero;

        if (animator != null)
        {
            animator.SetBool("isWalking", isWalking);
        }
    }

    void FixedUpdate()
    {
        if(dash.isDashing == false)
        {
           // Apply movement
            rb.MovePosition(rb.position + movement * finalSpeed * Time.fixedDeltaTime);
        }
        
    }
}
