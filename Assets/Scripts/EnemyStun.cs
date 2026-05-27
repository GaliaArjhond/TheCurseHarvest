using UnityEngine;
using System.Collections;

public class EnemyStun : MonoBehaviour
{
    private EnemyFollowPlayer follow;

    private bool stunned = false;

    void Awake()
    {
        follow = GetComponent<EnemyFollowPlayer>();
    }

    public void Stun(float duration)
    {
        if (!stunned)
            StartCoroutine(StunRoutine(duration));
    }

    IEnumerator StunRoutine(float duration)
    {
        stunned = true;

        if (follow != null)
            follow.enabled = false;

        Debug.Log("Enemy stunned!");

        yield return new WaitForSeconds(duration);

        if (follow != null)
            follow.enabled = true;

        stunned = false;
    }
}