using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerStatsManager playerStats; // ← add this

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStatsManager>(); // ← add this
    }

    void FixedUpdate()
    {
        // use speed from stats if available, otherwise use moveSpeed
        float speed = playerStats != null ? playerStats.GetSpeed() : moveSpeed;
        rb.linearVelocity = moveInput * speed;

        // drain stamina while walking ← add this
        if (moveInput != Vector2.zero && playerStats != null)
            playerStats.DrainStamina(playerStats.walkStaminaDrain * Time.fixedDeltaTime);
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true);
        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
        moveInput = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", moveInput.x);
        animator.SetFloat("InputY", moveInput.y);
    }
}