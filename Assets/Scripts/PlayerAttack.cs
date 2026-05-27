using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Settings")]
    public int damage = 1;
    public float attackRange = 0.7f;
    public float attackRadius = 0.35f;
    public float attackCooldown = 0.4f;
    public LayerMask enemyLayer;
    private float berserkDamageMultiplier = 1f;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerMovement movement;
    

    private bool canAttack = true;
    private Vector2 lastDirection = Vector2.down;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        float x = animator.GetFloat("LastInputX");
        float y = animator.GetFloat("LastInputY");

        if (Mathf.Abs(x) > Mathf.Abs(y))
            lastDirection = x > 0 ? Vector2.right : Vector2.left;
        else if (Mathf.Abs(y) > 0)
            lastDirection = y > 0 ? Vector2.up : Vector2.down;
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (!context.started)
            return;

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (!canAttack)
            return;

        Debug.Log("ATTACK INPUT WORKING");

        HotbarControler hotbar =
            FindFirstObjectByType<HotbarControler>();

        if (hotbar == null)
            return;

        Item selectedItem =
            hotbar.GetSelectedItem();

        if (selectedItem == null)
        {
            Debug.Log("No item equipped.");
            return;
        }

        if (selectedItem.itemType != Item.ItemType.Weapon)
        {
            Debug.Log("Selected item is not a weapon.");
            return;
        }

        StartAttack();
    }

   void StartAttack()
    {
        canAttack = false;

        if (movement != null)
        {
            movement.PlaySwordAnimation(lastDirection);
        }

        Invoke(nameof(DoDamage), 0.15f);

        float finalCooldown = attackCooldown;

        // Bangungot III attack speed
        if (SkillManager.Instance != null &&
            SkillManager.Instance.bangungot3Unlocked)
        {
            PlayerStatsManager stats =
                GetComponent<PlayerStatsManager>();

            if (stats != null)
            {
                float hpPercent =
                    stats.stats.currentHealth /
                    stats.stats.maxHealth;

                if (hpPercent <= 0.30f)
                {
                    finalCooldown *= 0.7f;
                }
            }
        }

        // Agility II
        if (SkillManager.Instance != null &&
            SkillManager.Instance.agility2Unlocked)
        {
            finalCooldown *= 0.80f;
        }

        Invoke(nameof(EndAttack), finalCooldown);
    }

    void DoDamage()
    {
        Vector2 attackPos =
            (Vector2)transform.position + lastDirection * attackRange;

        Collider2D[] hits = Physics2D.OverlapCircleAll(
            attackPos,
            attackRadius,
            enemyLayer
        );

        Debug.Log("Hits found: " + hits.Length);

        foreach (Collider2D hit in hits)
        {
            EnemyHealth enemy = hit.GetComponent<EnemyHealth>();

            if (enemy == null)
                continue;
            int finalDamage = damage;

            // Bangungot III Berserk
            if (SkillManager.Instance != null &&
                SkillManager.Instance.bangungot3Unlocked)
            {
                PlayerStatsManager stats =
                    GetComponent<PlayerStatsManager>();

                if (stats != null)
                {
                    float hpPercent =
                        stats.stats.currentHealth /
                        stats.stats.maxHealth;

                    if (hpPercent <= 0.30f)
                    {
                        finalDamage =
                            Mathf.CeilToInt(finalDamage * 1.5f);

                        Debug.Log("BERSERK MODE!");
                    }
                }
            }

            // Sword I
            if (SkillManager.Instance != null &&
                SkillManager.Instance.sword1Unlocked)
            {
                finalDamage =
                    Mathf.CeilToInt(finalDamage * 1.10f);
            }

            bool criticalHit = false;

            // Bangungot I: madness damage
            if (SkillManager.Instance != null &&
                SkillManager.Instance.bangungot1Unlocked)
            {
                int madnessChance = Random.Range(0, 100);

                if (madnessChance < 15)
                {
                    finalDamage *= 2;

                    Debug.Log("Bangungot frenzy damage!");
                }
            }

            // Sword II
            if (SkillManager.Instance != null &&
                SkillManager.Instance.sword2Unlocked)
            {
                int critChance = Random.Range(0, 100);

                if (critChance < 10)
                {
                    finalDamage *= 2;
                    criticalHit = true;

                    Debug.Log("CRITICAL HIT!");
                }
            }

            Vector2 hitDirection =
                (hit.transform.position - transform.position).normalized;

            enemy.TakeDamage(
                finalDamage,
                hitDirection,
                criticalHit
            );
            // Kulam I
            if (SkillManager.Instance != null &&
                SkillManager.Instance.kulam1Unlocked)
            {
                CurseBurn burn =
                    enemy.GetComponent<CurseBurn>();

                if (burn != null)
                {
                    burn.ApplyBurn(
                        1,      // damage
                        4f,     // duration
                        1f      // tick rate
                    );
                }
            }

            // Agility III
            if (SkillManager.Instance != null &&
                SkillManager.Instance.agility3Unlocked)
            {
                PlayerMovement movement =
                    GetComponent<PlayerMovement>();

                if (movement != null)
                {
                    movement.ActivateAgilityBoost();
                }
            }

            // Sword III
            if (SkillManager.Instance != null &&
                SkillManager.Instance.sword3Unlocked)
            {
                Collider2D[] nearbyEnemies =
                    Physics2D.OverlapCircleAll(
                        enemy.transform.position,
                        1f,
                        enemyLayer
                    );

                foreach (Collider2D nearby in nearbyEnemies)
                {
                    if (nearby.gameObject == enemy.gameObject)
                        continue;

                    EnemyHealth nearbyEnemy =
                        nearby.GetComponent<EnemyHealth>();

                    if (nearbyEnemy != null)
                    {
                        Vector2 splashDirection =
                            (nearby.transform.position - transform.position).normalized;

                        nearbyEnemy.TakeDamage(
                            Mathf.Max(1, finalDamage / 2),
                            splashDirection,
                            false
                        );
                    }
                    
                }
            }
        }
    }

    void EndAttack()
    {
        canAttack = true;
    }

    void OnDrawGizmosSelected()
    {
        Vector2 direction = lastDirection == Vector2.zero ? Vector2.down : lastDirection;
        Vector2 attackPos = (Vector2)transform.position + direction * attackRange;

        Gizmos.DrawWireSphere(attackPos, attackRadius);
    }
}