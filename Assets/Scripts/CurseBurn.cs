using UnityEngine;
using System.Collections;

public class CurseBurn : MonoBehaviour
{
    private EnemyHealth enemyHealth;

    private bool burning = false;

    void Awake()
    {
        enemyHealth = GetComponent<EnemyHealth>();
    }

    public void ApplyBurn(
        int damagePerTick,
        float duration,
        float tickRate)
    {
        if (enemyHealth != null)
            enemyHealth.isCursed = true;

        if (!burning)
        {
            StartCoroutine(
                BurnRoutine(
                    damagePerTick,
                    duration,
                    tickRate
                )
            );
        }
    }

    IEnumerator BurnRoutine(
        int damagePerTick,
        float duration,
        float tickRate)
    {
        burning = true;

        float timer = duration;

        while (timer > 0f)
        {
            timer -= tickRate;

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(
                    damagePerTick,
                    Vector2.zero,
                    false
                );

                Debug.Log("Curse burn tick!");
            }

            yield return new WaitForSeconds(tickRate);
        }

        burning = false;

        if (enemyHealth != null)
            enemyHealth.isCursed = false;
    }
}