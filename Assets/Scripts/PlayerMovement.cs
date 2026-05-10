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
    private PlayerStatsManager playerStats;
    private bool canMove = true;
    private Knockback knockback;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStatsManager>();
        knockback = GetComponent<Knockback>();
    }

    void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockbacking)
            return;

        // stop movement when chest is open
        if (ChestUIManager.Instance != null &&
            ChestUIManager.Instance.IsChestOpen())
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // movement speed
        float speed = playerStats != null
            ? playerStats.GetSpeed()
            : moveSpeed;

        rb.linearVelocity = moveInput * speed;

        // stamina drain
        if (moveInput != Vector2.zero && playerStats != null)
        {
            playerStats.DrainStamina(
                playerStats.walkStaminaDrain * Time.fixedDeltaTime
            );
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        // stop input when chest open
        if (ChestUIManager.Instance != null &&
            ChestUIManager.Instance.IsChestOpen())
        {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        if (!canMove)
        {
            moveInput = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        moveInput = context.ReadValue<Vector2>();

        animator.SetBool("isWalking", moveInput != Vector2.zero);

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("InputX", moveInput.x);
            animator.SetFloat("InputY", moveInput.y);

            animator.SetFloat("LastInputX", moveInput.x);
            animator.SetFloat("LastInputY", moveInput.y);
        }
    }

    public void PlayAxeAnimation(Vector2 direction)
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isWalking", false);

        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);

        animator.SetTrigger("UseAxe");

        CancelInvoke(nameof(EndToolAnimation));
        Invoke(nameof(EndToolAnimation), 0.5f);
    }

    public void PlayHoeAnimation(Vector2 direction)
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isWalking", false);

        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);

        animator.SetTrigger("UseHoe");

        CancelInvoke(nameof(EndToolAnimation));
        Invoke(nameof(EndToolAnimation), 0.5f);
    }

    public void PlayPickAxeAnimation(Vector2 direction)
    {
        canMove = false;
        rb.linearVelocity = Vector2.zero;

        animator.SetBool("isWalking", false);

        animator.SetFloat("InputX", direction.x);
        animator.SetFloat("InputY", direction.y);

        animator.SetTrigger("UsePickAxe");

        CancelInvoke(nameof(EndToolAnimation));
        Invoke(nameof(EndToolAnimation), 0.5f);
    }

    public void EndToolAnimation()
    {
        canMove = true;
    }
}