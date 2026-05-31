using UnityEngine;
using TMPro;
using Cinemachine;

public class CaveManager : MonoBehaviour
{
    public static CaveManager Instance;

    [Header("Cave Level")]
    [SerializeField] private int currentCaveLevel = 1;
    [SerializeField] private TextMeshProUGUI caveLevelText;

    [Header("Player Spawn")]
    [SerializeField] private Transform caveStartSpawn;
    [SerializeField] private Transform caveReturnSpawn;

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
    [SerializeField] private PolygonCollider2D caveBounds;

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
        MoveActivePlayerTo(caveStartSpawn);
        UpdateLevelUI();
        UpdateCameraFollowAndBounds();
    }

    public void GoNextLevel()
    {
        currentCaveLevel++;

        GenerateLevel();
        MoveActivePlayerTo(caveStartSpawn);
        UpdateLevelUI();
        UpdateCameraFollowAndBounds();

        Debug.Log("Entered Cave Level " + currentCaveLevel);
    }

    public void ReturnToCaveStart(Transform playerTransform)
    {
        if (caveReturnSpawn == null)
        {
            Debug.LogError("Cave Return Spawn missing.");
            return;
        }

        playerTransform.position = caveReturnSpawn.position;
        Physics2D.SyncTransforms();

        UpdateCameraFollowAndBounds();

        Debug.Log("Returned player to cave return spawn.");
    }

    void GenerateLevel()
    {
        ClearOldLadder();
        ClearOldRocks();

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

            GameObject rockToSpawn = GetRandomRockPrefab();

            Instantiate(
                rockToSpawn,
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

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("No Player found. Check Player tag.");
            return;
        }

        player.transform.position = spawn.position;
        Physics2D.SyncTransforms();
    }

    void UpdateCameraFollowAndBounds()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        CinemachineVirtualCamera cmcam =
            FindFirstObjectByType<CinemachineVirtualCamera>();

        if (cmcam != null && player != null)
        {
            cmcam.Follow = player.transform;
            cmcam.LookAt = player.transform;
            cmcam.PreviousStateIsValid = false;
        }

        CinemachineConfiner confiner =
            FindFirstObjectByType<CinemachineConfiner>();

        if (confiner != null && caveBounds != null)
        {
            confiner.m_BoundingShape2D = caveBounds;
            confiner.InvalidatePathCache();
        }
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
        {
            Destroy(child.gameObject);
        }
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