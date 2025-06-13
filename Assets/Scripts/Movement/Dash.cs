using UnityEngine;
using System.Collections;

public class Dash : MonoBehaviour
{
    public float dashSpeed = 12f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    private Rigidbody2D rb;
    private PlayerMovement playerMovement;

    public bool isDashing { get; private set; } = false;
    private bool canDash = true;
    private Vector2 dashDirection;

    private Collider2D mainCollider;
    private Collider2D childCollider;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovement>();

        // Cache both colliders
        mainCollider = GetComponent<Collider2D>();
        childCollider = GetComponentInChildren<Collider2D>();

        // Avoid assigning the same collider twice
        if (childCollider == mainCollider)
            childCollider = null;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash && playerMovement.lastDirection != Vector2.zero)
        {
            StartCoroutine(PerformDash());
        }
    }

    private IEnumerator PerformDash()
    {
        isDashing = true;
        canDash = false;

        // Disable both colliders during dash
        if (mainCollider != null) mainCollider.enabled = false;
        if (childCollider != null) childCollider.enabled = false;

        dashDirection = playerMovement.lastDirection;

        float dashTime = 0f;
        while (dashTime < dashDuration)
        {
            rb.MovePosition(rb.position + dashDirection * dashSpeed * Time.fixedDeltaTime);
            dashTime += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        isDashing = false;

        // Re-enable both colliders after dash
        if (mainCollider != null) mainCollider.enabled = true;
        if (childCollider != null) childCollider.enabled = true;

        // Cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
