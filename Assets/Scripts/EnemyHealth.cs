using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;

    [Header("Rewards")]
    public int expReward = 10;
    public int pesosReward = 5;

    [Header("Effects")]
    public GameObject damagePopupPrefab;
    [SerializeField] private GameObject explosionPrefab;

    [HideInInspector] public bool isCursed = false;

    private int currentHealth;

    private HitFlash hitFlash;
    private Knockback knockback;

    private bool isDead = false;

    void Awake()
    {
        currentHealth = maxHealth;

        hitFlash = GetComponent<HitFlash>();
        knockback = GetComponent<Knockback>();
    }

    public void TakeDamage(
        int damage,
        Vector2 hitDirection,
        bool criticalHit = false,
        GameObject attacker = null)
    {
        // already dead
        if (isDead)
            return;

        currentHealth -= damage;

        // flash
        if (hitFlash != null)
            hitFlash.Flash();

        // knockback
        if (knockback != null)
            knockback.ApplyKnockback(hitDirection);

        // damage popup
        if (damagePopupPrefab != null)
        {
            GameObject popup =
                Instantiate(
                    damagePopupPrefab,
                    transform.position + Vector3.up * 0.7f,
                    Quaternion.identity
                );

            DamagePopup dp =
                popup.GetComponent<DamagePopup>();

            if (dp != null)
                dp.Setup(damage, criticalHit);
        }

        // death
        if (currentHealth <= 0)
        {
            isDead = true;

            if (QuestManager.Instance != null)
            {
                QuestManager.Instance.halimawsKilled++;
            }

            // Bangungot II
            if (attacker != null &&
                SkillManager.Instance != null &&
                SkillManager.Instance.bangungot2Unlocked)
            {
                PlayerStatsManager stats =
                    attacker.GetComponent<PlayerStatsManager>();

                if (stats != null)
                {
                    stats.RestoreStamina(10f);

                    Debug.Log("Bangungot II restored stamina!");
                }
            }

            // Kulam III explosion
            if (PlayerCurrency.Instance != null)
            {
                PlayerCurrency.Instance.AddExperience(expReward);
                PlayerCurrency.Instance.AddPesos(pesosReward);
            }

            if (isCursed &&
                SkillManager.Instance != null &&
                SkillManager.Instance.kulam3Unlocked)
            {
                Collider2D[] nearby =
                    Physics2D.OverlapCircleAll(
                        transform.position,
                        2f
                    );

                foreach (Collider2D hit in nearby)
                {
                    EnemyHealth enemy =
                        hit.GetComponent<EnemyHealth>();

                    if (enemy != null &&
                        enemy.gameObject != gameObject &&
                        !enemy.isDead)
                    {
                        Vector2 dir =
                            (enemy.transform.position - transform.position).normalized;

                        enemy.TakeDamage(
                            2,
                            dir,
                            false
                        );
                    }
                }

                Debug.Log("Kulam III curse explosion!");

                // spawn explosion FX
                if (explosionPrefab != null)
                {
                    Vector3 spawnPos =
                        transform.position +
                        new Vector3(0f, 0.3f, 0f);

                    GameObject fx =
                        Instantiate(
                            explosionPrefab,
                            spawnPos,
                            Quaternion.identity
                        );

                    SpriteRenderer sr =
                        fx.GetComponent<SpriteRenderer>();

                    if (sr != null)
                    {
                        sr.sortingLayerName = "Effects";
                        sr.sortingOrder = 999;
                    }

                    Debug.Log("Explosion spawned");
                }
            }

            Destroy(gameObject);
        }
    }
}