using UnityEngine;
using System.Collections;

public class ExhaustionUI : MonoBehaviour
{
    public static ExhaustionUI Instance;

    [SerializeField]
    private GameObject panel;

    void Awake()
    {
        Instance = this;

        panel.SetActive(false);
    }

    public void Show()
    {
        // Ensure this GameObject is active so coroutine can run
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        StartCoroutine(ExhaustSequence());
    }

    IEnumerator ExhaustSequence()
    {
        panel.SetActive(true);

        yield return new WaitForSecondsRealtime(3f);

        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(SpawnManager.bedSpawnPointName))
            SpawnManager.spawnPointName =
                SpawnManager.bedSpawnPointName;

        if (DayManager.Instance != null)
            DayManager.Instance.AdvanceDay();

        if (PlayerStatsManager.Instance != null)
            PlayerStatsManager.Instance.RestoreAll();

        DayNightCycle cycle =
            FindFirstObjectByType<DayNightCycle>();

        if (cycle != null)
            cycle.ResetToMorning();

        // Teleport player to bed
        if (!string.IsNullOrEmpty(SpawnManager.bedSpawnPointName))
        {
            GameObject bed = GameObject.Find(SpawnManager.bedSpawnPointName);
            if (bed != null)
            {
                GameObject player = GameObject.FindWithTag("Player");
                if (player != null)
                {
                    player.transform.position = bed.transform.position;
                    Physics2D.SyncTransforms();
                    Debug.Log("Teleported player to bed at: " + bed.transform.position);
                }
                else
                {
                    Debug.LogWarning("ExhaustionUI: Player not found!");
                }
            }
            else
            {
                Debug.LogWarning("ExhaustionUI: Bed spawn point '" + SpawnManager.bedSpawnPointName + "' not found!");
            }
        }
        else
        {
            Debug.LogWarning("ExhaustionUI: bedSpawnPointName not set!");
        }

        panel.SetActive(false);
    }
}