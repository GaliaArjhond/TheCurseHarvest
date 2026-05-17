using UnityEngine;

public class DarkAura : MonoBehaviour
{
    [SerializeField] private int auraDamage = 1;
    [SerializeField] private float tickRate = 1f;

    private float timer;

    void Update()
    {
        if (SkillManager.Instance == null ||
            !SkillManager.Instance.anino3Unlocked)
        {
            if (gameObject.activeSelf)
                gameObject.SetActive(false);

            return;
        }

        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            timer = tickRate;

            Debug.Log("Aura ticking");

            Collider2D[] hits =
                Physics2D.OverlapCircleAll(
                    transform.position,
                    2f
                );

            foreach (Collider2D hit in hits)
            {
                EnemyHealth enemy =
                    hit.GetComponent<EnemyHealth>();

                if (enemy != null)
                {
                    Vector2 dir =
                        (enemy.transform.position - transform.position).normalized;

                    enemy.TakeDamage(
                        auraDamage,
                        dir,
                        false
                    );
                }
            }
        }
    }
}