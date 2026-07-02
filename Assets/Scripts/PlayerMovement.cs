using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    [Header("Footstep Sound")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private float footstepDelay = 0.35f;

    [Header("Agility Boost")]
    [SerializeField] private float agilityBoostMultiplier = 1.5f;
    [SerializeField] private float agilityBoostDuration = 1f;

    private bool agilityBoostActive = false;

    private float footstepTimer;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private PlayerStatsManager playerStats;
    private bool canMove = true;
    private Knockback knockback;

    public void SetMovementEnabled(bool enabled)
    {
        canMove = enabled;

        if (!enabled)
        {
            moveInput = Vector2.zero;
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
                animator.SetBool("isWalking", false);
        }
    }

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
            footstepTimer = 0f;
            return;
        }

        if (!canMove)
        {
            rb.linearVelocity = Vector2.zero;
            footstepTimer = 0f;
            return;
        }

        // movement speed
        float finalSpeed = playerStats != null
            ? playerStats.GetSpeed()
            : moveSpeed;

        // Anino I: faster at night
        DayNightCycle cycle =
            FindFirstObjectByType<DayNightCycle>();

        if (cycle != null &&
            SkillManager.Instance != null &&
            SkillManager.Instance.anino1Unlocked)
        {
            float hour = cycle.GetCurrentHour();

            bool isNight =
                hour >= 19f || hour <= 5f;

            if (isNight)
            {
                finalSpeed *= 1.15f;

                Debug.Log("Anino I night speed active");
            }
        }

        // Agility I
        if (SkillManager.Instance != null &&
            SkillManager.Instance.agility1Unlocked)
        {
            finalSpeed *= 1.10f;
        }

        // Agility III
        if (agilityBoostActive)
        {
            finalSpeed *= agilityBoostMultiplier;
        }

        rb.linearVelocity = moveInput * finalSpeed;
        
        
        HandleFootsteps();

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

    private void HandleFootsteps()
    {
        bool isMoving = moveInput != Vector2.zero;

        if (!isMoving)
        {
            footstepTimer = 0f;

            if (footstepSource != null && footstepSource.isPlaying)
            {
                footstepSource.Stop();
            }

            return;
        }

        footstepTimer -= Time.fixedDeltaTime;

        if (footstepTimer <= 0f)
        {
            if (footstepSource != null && footstepClip != null)
            {
                footstepSource.Stop();
                footstepSource.clip = footstepClip;
                footstepSource.Play();
            }

            footstepTimer = footstepDelay;
        }
    }

    public void ActivateAgilityBoost()
    {
        StopCoroutine(nameof(AgilityBoostRoutine));
        StartCoroutine(nameof(AgilityBoostRoutine));
    }

    IEnumerator AgilityBoostRoutine()
    {
        agilityBoostActive = true;

        Debug.Log("Agility boost active!");

        yield return new WaitForSeconds(agilityBoostDuration);

        agilityBoostActive = false;
    }
}