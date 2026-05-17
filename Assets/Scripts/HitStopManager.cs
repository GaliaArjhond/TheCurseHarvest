using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    void Awake()
    {
        Instance = this;
    }

    public void Stop(float duration)
    {
        StartCoroutine(StopRoutine(duration));
    }

    IEnumerator StopRoutine(float duration)
    {
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = 1f;
    }
}