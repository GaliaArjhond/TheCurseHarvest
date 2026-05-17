using UnityEngine;

public class EnemyFollowPlayer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float stopDistance = 0.6f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private Knockback knockback;

    private float attackCooldown = 1f;
    private float lastAttackTime;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        knockback = GetComponent<Knockback>();
    }

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
            player = playerObj.transform;
    }

  void FixedUpdate()
    {
        if (knockback != null && knockback.IsKnockbacking)
        {
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
                animator.SetBool("isMoving", false);

            return;
        }

        if (player == null) return;

        float detectionRange = 6f;

        // Anino II: enemies detect player slower
        if (SkillManager.Instance != null &&
            SkillManager.Instance.anino2Unlocked)
        {
            detectionRange *= 0.75f;
        }

        float distance =
            Vector2.Distance(transform.position, player.position);

        // Player too far
        if (distance > detectionRange)
        {
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
                animator.SetBool("isMoving", false);

            return;
        }

        // Follow player
        if (distance > stopDistance)
        {
            Vector2 direction =
                (player.position - transform.position).normalized;

            rb.linearVelocity = direction * moveSpeed;

            if (animator != null)
            {
                animator.SetBool("isMoving", true);
                animator.SetFloat("MoveX", direction.x);
                animator.SetFloat("MoveY", direction.y);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;

            if (animator != null)
                animator.SetBool("isMoving", false);
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Time.time - lastAttackTime < attackCooldown) return;

            PlayerStatsManager playerStats = collision.gameObject.GetComponent<PlayerStatsManager>();

            if (playerStats != null)
            {
                int finalDamage = 1;

                EnemyHealth hp = GetComponent<EnemyHealth>();

                // Kulam II
                if (hp != null &&
                    hp.isCursed &&
                    SkillManager.Instance != null &&
                    SkillManager.Instance.kulam2Unlocked)
                {
                    finalDamage = Mathf.Max(1, finalDamage - 1);

                    Debug.Log("Kulam II weakened enemy!");
                }

                playerStats.TakeDamage(finalDamage, transform);
                lastAttackTime = Time.time;
            }
        }
    }
}