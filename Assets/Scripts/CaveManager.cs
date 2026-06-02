using UnityEngine;
using TMPro;
using Cinemachine;

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance;
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Cave Level")]
    [SerializeField] private int currentCaveLevel = 1;
    [SerializeField] private TextMeshProUGUI caveLevelText;

    [Header("Player Spawn")]
    [SerializeField] private Transform caveStartSpawn;
    [SerializeField] private Transform caveReturnSpawn;
    [SerializeField] private Transform caveLevelSpawn;

    [Header("Rock Spawning")]
    [SerializeField] private Transform rockSpawnParent;
    [SerializeField] private Transform rockSpawnCenter;
    [SerializeField] private int rocksToSpawn = 12;
    [SerializeField] private float spawnWidth = 10f;
    [SerializeField] private float spawnHeight = 6f;

    [Header("Ore Prefabs")]
    [SerializeField] private GameObject stoneRockPrefab;
    [SerializeField] private GameObject coalNodePrefab;
    [SerializeField] private GameObject ironNodePrefab;
    [SerializeField] private GameObject goldNodePrefab;

    [Header("Camera")]
    [SerializeField] private PolygonCollider2D startBounds;
    [SerializeField] private PolygonCollider2D levelBounds;

    [Header("Ladder")]
    [SerializeField] private GameObject ladderPrefab;
    [SerializeField] private float ladderChance = 0.08f;

    private GameObject currentLadder;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GenerateLevel();

        UseStartBounds();
        MoveActivePlayerTo(caveStartSpawn);

        UpdateLevelUI();
        UpdateCameraFollow();
    }

   public void GoNextLevel()
    {
        currentCaveLevel++;

        GenerateLevel();

        UseLevelBounds();

        MoveActivePlayerTo(caveLevelSpawn);

        UpdateLevelUI();
        UpdateCameraFollow();
    }

    public void ReturnToCaveStart(Transform playerTransform)
    {
        if (caveReturnSpawn == null)
        {
            Debug.LogError("Cave Return Spawn missing.");
            return;
        }

        UseStartBounds();

        playerTransform.position = caveReturnSpawn.position;
        Physics2D.SyncTransforms();

        UpdateCameraFollow();

        Debug.Log("Returned player to cave return spawn.");
    }

    void GenerateLevel()
    {
        ClearOldLadder();
        ClearOldRocks();

        if (enemySpawner != null)
        {
            enemySpawner.SpawnEnemies(currentCaveLevel);
        }

        if (rockSpawnParent == null ||
            rockSpawnCenter == null ||
            stoneRockPrefab == null ||
            coalNodePrefab == null ||
            ironNodePrefab == null ||
            goldNodePrefab == null)
        {
            Debug.LogError("Missing cave spawn references or ore prefabs.");
            return;
        }

        for (int i = 0; i < rocksToSpawn; i++)
        {
            Vector2 randomPos = new Vector2(
                rockSpawnCenter.position.x + Random.Range(-spawnWidth / 2f, spawnWidth / 2f),
                rockSpawnCenter.position.y + Random.Range(-spawnHeight / 2f, spawnHeight / 2f)
            );

            Instantiate(
                GetRandomRockPrefab(),
                randomPos,
                Quaternion.identity,
                rockSpawnParent
            );
        }

        Debug.Log("Generated cave level " + currentCaveLevel);
    }

    GameObject GetRandomRockPrefab()
    {
        int roll = Random.Range(0, 100);

        if (currentCaveLevel <= 5)
        {
            if (roll < 80) return stoneRockPrefab;
            return coalNodePrefab;
        }

        if (currentCaveLevel <= 10)
        {
            if (roll < 60) return stoneRockPrefab;
            if (roll < 85) return coalNodePrefab;
            return ironNodePrefab;
        }

        if (roll < 40) return stoneRockPrefab;
        if (roll < 70) return coalNodePrefab;
        if (roll < 90) return ironNodePrefab;

        return goldNodePrefab;
    }

    public void TrySpawnLadder(Vector3 position)
    {
        if (ladderPrefab == null)
        {
            Debug.LogError("Ladder Prefab missing.");
            return;
        }

        if (currentLadder != null)
            return;

        if (Random.value <= ladderChance)
        {
            currentLadder = Instantiate(
                ladderPrefab,
                position,
                Quaternion.identity
            );

            Debug.Log("Ladder spawned!");
        }
    }

    void MoveActivePlayerTo(Transform spawn)
    {
        if (spawn == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No Player found. Check Player tag.");
            return;
        }

        player.transform.position = spawn.position;
        Physics2D.SyncTransforms();
    }

    void UpdateCameraFollow()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        CinemachineVirtualCamera cmcam =
            FindFirstObjectByType<CinemachineVirtualCamera>();

        if (cmcam != null && player != null)
        {
            cmcam.Follow = player.transform;
            cmcam.LookAt = player.transform;
            cmcam.PreviousStateIsValid = false;
        }
    }

    public void UseStartBounds()
    {
        CinemachineConfiner confiner =
            FindFirstObjectByType<CinemachineConfiner>();

        if (confiner == null || startBounds == null)
            return;

        confiner.m_BoundingShape2D = startBounds;
        confiner.InvalidatePathCache();

        Debug.Log("Camera using START bounds.");
    }

    public void UseLevelBounds()
    {
        CinemachineConfiner confiner =
            FindFirstObjectByType<CinemachineConfiner>();

        if (confiner == null || levelBounds == null)
            return;

        confiner.m_BoundingShape2D = levelBounds;
        confiner.InvalidatePathCache();

        Debug.Log("Camera using LEVEL bounds.");
    }

    void ClearOldLadder()
    {
        if (currentLadder != null)
        {
            Destroy(currentLadder);
            currentLadder = null;
        }
    }

    void ClearOldRocks()
    {
        if (rockSpawnParent == null)
            return;

        foreach (Transform child in rockSpawnParent)
            Destroy(child.gameObject);
    }

    void UpdateLevelUI()
    {
        if (caveLevelText != null)
            caveLevelText.text = "Cave Level: " + currentCaveLevel;
    }

    public int GetCurrentCaveLevel()
    {
        return currentCaveLevel;
    }
}