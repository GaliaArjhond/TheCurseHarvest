using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform spawnCenter;
    [SerializeField] private Transform enemyParent;

    private Transform player;

    [Header("Spawn Area")]
    [SerializeField] private float spawnWidth = 15f;
    [SerializeField] private float spawnHeight = 10f;

    [Header("Settings")]
    [SerializeField] private int enemiesToSpawn = 4;
    [SerializeField] private float minDistanceFromPlayer = 5f;

    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject slimePrefab;
    [SerializeField] private GameObject batPrefab;
    [SerializeField] private GameObject skeletonPrefab;

    void Start()
    {
        Debug.Log("EnemySpawner Started");
        FindPlayer();
        SpawnEnemies(1);
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

        Debug.Log("SPAWNING ENEMIES FOR LEVEL " + caveLevel);

        ClearEnemies();

        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Vector2 pos = GetValidSpawnPosition();

            GameObject enemy =
                Instantiate(
                    GetEnemyPrefab(caveLevel),
                    pos,
                    Quaternion.identity,
                    enemyParent
                );

            Debug.Log("Spawned: " + enemy.name);
        }
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

    GameObject GetEnemyPrefab(int level)
    {
        int roll = Random.Range(0, 100);

        if (level <= 5)
        {
            return slimePrefab;
        }

        if (level <= 10)
        {
            if (roll < 70)
                return slimePrefab;

            return batPrefab;
        }

        if (roll < 40)
            return slimePrefab;

        if (roll < 80)
            return batPrefab;

        return skeletonPrefab;
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