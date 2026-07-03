using UnityEngine;

public class BakunawaSpawner : MonoBehaviour
{
    public static BakunawaSpawner Instance;

    public GameObject bakunawaPrefab;
    public Transform spawnPoint;

    private bool hasSpawnedTonight = false;

    void Awake()
    {
        Instance = this;
    }

    public void OnNightStarted(int day)
    {
        hasSpawnedTonight = false;

        if (day > 0 && day % 7 == 0)
        {
            SpawnBakunawa();
        }
    }

    public void OnDayStarted()
    {
        hasSpawnedTonight = false;
    }

    void SpawnBakunawa()
    {
        if (hasSpawnedTonight)
            return;

        Instantiate(bakunawaPrefab, spawnPoint.position, Quaternion.identity);

        hasSpawnedTonight = true;

        Debug.Log("Bakunawa has awakened!");
    }
}