using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private Transform enemyParent;
    [SerializeField] private Transform[] spawnPoints;

    private Transform player;

    [Header("Spawn Area")]
    [SerializeField] private float spawnWidth = 15f;
    [SerializeField] private float spawnHeight = 10f;

    [Header("Night Spawn Settings")]
    [SerializeField] private int maxNightEnemies = 10;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float daytimeSpawnInterval = 2f;
    [SerializeField] private int enemiesToSpawn = 4;
    [SerializeField] private float minDistanceFromPlayer = 5f;
    [SerializeField] private bool allowDaytimeTesting = true;

    [Header("Enemy Prefabs")]
    [FormerlySerializedAs("halimawPrefab")]
    [SerializeField] private GameObject kaprePrefab;
    [FormerlySerializedAs("batPrefab")]
    [SerializeField] private GameObject tikbalangPrefab;
    [FormerlySerializedAs("skeletonPrefab")]
    [SerializeField] private GameObject bakunawaPrefab;
    [SerializeField] private GameObject manananggalPrefab;

    [Header("Spawn Chances")]
    [Range(0, 100)]
    [SerializeField] private int kapreChance = 35;
    [Range(0, 100)]
    [SerializeField] private int tikbalangChance = 30;
    [Range(0, 100)]
    [SerializeField] private int bakunawaChance = 20;
    [Range(0, 100)]
    [SerializeField] private int manananggalChance = 15;

    private float timer;
    private List<GameObject> spawnedEnemies = new List<GameObject>();
    private bool spawnCycleActive = false;

    void Start()
    {
        Debug.Log("EnemySpawner Started");
        FindPlayer();
    }

    void Update()
    {
        if (PauseManager.Instance != null &&
            PauseManager.Instance.IsPaused)
        {
            return;
        }

        if (DayNightCycle.Instance == null)
            return;

        bool isNight = DayNightCycle.Instance.IsNight();
        bool shouldSpawn = isNight || allowDaytimeTesting;

        if (!shouldSpawn)
        {
            if (spawnCycleActive)
            {
                spawnCycleActive = false;
                RemoveNightEnemies();
            }

            return;
        }

        if (!spawnCycleActive)
        {
            spawnCycleActive = true;
            timer = isNight ? spawnInterval : daytimeSpawnInterval;
        }

        timer -= Time.deltaTime;

        if (timer <= 0f && spawnedEnemies.Count < maxNightEnemies)
        {
            SpawnNightEnemy();
            timer = isNight ? spawnInterval : daytimeSpawnInterval;
        }
    }

    void FindPlayer()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
            Debug.Log("Player Found: " + player.name);
        }
        else
        {
            Debug.LogWarning("Player not found.");
        }
    }

    public void SpawnEnemies(int caveLevel)
    {
        Debug.Log("SPAWNING ENEMIES FOR LEVEL " + caveLevel);

        if (player == null)
        {
            FindPlayer();

            if (player == null)
            {
                Debug.LogError("Cannot spawn enemies. Player not found.");
                return;
            }
        }

        ClearEnemies();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 pos = GetValidSpawnPosition();
            GameObject prefab = GetEnemyPrefab(caveLevel);

            if (prefab == null)
            {
                prefab = GetFirstAvailableEnemyPrefab();

                if (prefab == null)
                {
                    Debug.LogError("EnemySpawner: No enemy prefabs are assigned. Cannot spawn enemies.");
                    return;
                }

                Debug.LogWarning("EnemySpawner: GetEnemyPrefab returned null, falling back to " + prefab.name);
            }

            GameObject enemy =
                Instantiate(
                    prefab,
                    pos,
                    Quaternion.identity,
                    enemyParent
                );

            Debug.Log("Spawned: " + enemy.name);
        }
    }

    void SpawnNightEnemy()
    {
        if (spawnedEnemies.Count >= maxNightEnemies)
            return;

        GameObject prefab = GetNightEnemyPrefab();

        if (prefab == null)
            return;

        Transform point = GetRandomSpawnPoint();
        Vector2 spawnPosition = GetSpawnPosition(point);

        GameObject enemy = Instantiate(
            prefab,
            spawnPosition,
            Quaternion.identity,
            enemyParent
        );

        spawnedEnemies.Add(enemy);
        Debug.Log("Night enemy spawned: " + prefab.name);
    }

    Transform GetRandomSpawnPoint()
    {
        if (spawnPoints != null && spawnPoints.Length > 0)
            return spawnPoints[Random.Range(0, spawnPoints.Length)];

        return spawnCenter;
    }

    Vector2 GetSpawnPosition(Transform point)
    {
        if (point != null)
            return point.position;

        if (spawnCenter == null)
            return Vector2.zero;

        return new Vector2(
            spawnCenter.position.x + Random.Range(-spawnWidth / 2f, spawnWidth / 2f),
            spawnCenter.position.y + Random.Range(-spawnHeight / 2f, spawnHeight / 2f)
        );
    }

    void RemoveNightEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] != null)
                Destroy(spawnedEnemies[i]);
        }

        spawnedEnemies.Clear();
    }

    Vector2 GetValidSpawnPosition()
    {
        Vector2 pos;
        int attempts = 0;

        do
        {
            pos = new Vector2(
                spawnCenter.position.x +
                Random.Range(-spawnWidth / 2f, spawnWidth / 2f),

                spawnCenter.position.y +
                Random.Range(-spawnHeight / 2f, spawnHeight / 2f)
            );

            attempts++;

        } while (
            player != null &&
            Vector2.Distance(pos, player.position) < minDistanceFromPlayer &&
            attempts < 50
        );

        return pos;
    }

    GameObject GetNightEnemyPrefab()
    {
        int roll = Random.Range(0, 100);
        int threshold = 0;

        threshold += kapreChance;
        if (roll < threshold && kaprePrefab != null)
            return kaprePrefab;

        threshold += tikbalangChance;
        if (roll < threshold && tikbalangPrefab != null)
            return tikbalangPrefab;

        threshold += bakunawaChance;
        if (roll < threshold && bakunawaPrefab != null)
            return bakunawaPrefab;

        threshold += manananggalChance;
        if (roll < threshold && manananggalPrefab != null)
            return manananggalPrefab;

        GameObject fallback = GetFirstAvailableEnemyPrefab();
        if (fallback == null)
        {
            Debug.LogError("EnemySpawner: No night enemy prefabs are assigned.");
        }

        return fallback;
    }

    GameObject GetFirstAvailableEnemyPrefab()
    {
        if (kaprePrefab != null)
            return kaprePrefab;

        if (tikbalangPrefab != null)
            return tikbalangPrefab;

        if (bakunawaPrefab != null)
            return bakunawaPrefab;

        if (manananggalPrefab != null)
            return manananggalPrefab;

        return null;
    }

    GameObject GetEnemyPrefab(int level)
    {
        int roll = Random.Range(0, 100);

        if (level <= 5)
        {
            return kaprePrefab;
        }

        if (level <= 10)
        {
            if (roll < 70)
                return kaprePrefab;

            return tikbalangPrefab;
        }

        if (level <= 15)
        {
            if (roll < 40)
                return kaprePrefab;

            if (roll < 80)
                return tikbalangPrefab;

            return bakunawaPrefab;
        }

        if (roll < 30)
            return kaprePrefab;

        if (roll < 60)
            return tikbalangPrefab;

        if (roll < 85)
            return bakunawaPrefab;

        return manananggalPrefab;
    }

    void ClearEnemies()
    {
        if (enemyParent == null)
            return;

        foreach (Transform child in enemyParent)
        {
            Destroy(child.gameObject);
        }
    }
}