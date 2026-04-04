using System.Collections.Generic;
using UnityEngine;

public class PropsSpawner : MonoBehaviour
{
    [Header("Spawn Zone")]
    [SerializeField] private PolygonCollider2D spawnZone;

    [Header("Props")]
    [SerializeField] private GameObject[] treePrefabs;
    [SerializeField] private GameObject[] stonePrefabs;
    [SerializeField] private GameObject[] vegetationPrefabs;

    [Header("Spawn Count")]
    [SerializeField] private int treeCount = 15;
    [SerializeField] private int stoneCount = 20;
    [SerializeField] private int vegCount = 10;

    [Header("Spacing")]
    [SerializeField] private float minDistanceBetweenProps = 1.5f;
    [SerializeField] private int maxAttempts = 30;

    [Header("House Exclusion")]
    [SerializeField] private Transform houseTransform;
    [SerializeField] private float houseRadius = 4f;

    [Header("Chunk Settings")]
    [SerializeField] private bool spawnOnStart = true;
    [HideInInspector] public bool hasSpawned = false; // ← must be public

    private List<Vector3> spawnedPositions = new List<Vector3>();
    private List<GameObject> spawnedProps = new List<GameObject>();

    void Start()
    {
        if (spawnOnStart)
            SpawnAll();
    }

    // ── called by ChunkLoader when chunk is enabled ──
    public void SpawnAll()
    {
        if (hasSpawned) return; // don't spawn twice

        SpawnGroup(treePrefabs, treeCount);
        SpawnGroup(stonePrefabs, stoneCount);
        SpawnGroup(vegetationPrefabs, vegCount);

        hasSpawned = true;
    }

    // ── called by ChunkLoader when chunk is disabled ──
    public void DespawnAll()
    {
        foreach (GameObject prop in spawnedProps)
        {
            if (prop != null)
                prop.SetActive(false);
        }
    }

    // ── called when chunk is re-enabled ──
    public void ShowAll()
    {
        foreach (GameObject prop in spawnedProps)
        {
            if (prop != null)
                prop.SetActive(true);
        }
    }

    void SpawnGroup(GameObject[] prefabs, int count)
    {
        if (prefabs.Length == 0) return;

        int spawned = 0;
        int attempts = 0;

        while (spawned < count && attempts < count * maxAttempts)
        {
            attempts++;

            Vector3 randomPos = GetRandomPointInPolygon();

            if (!spawnZone.OverlapPoint(randomPos)) continue;
            if (!IsFarEnough(randomPos)) continue;
            if (IsTooCloseToHouse(randomPos)) continue;

            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject prop = Instantiate(prefab, randomPos, Quaternion.identity, transform);

            spawnedPositions.Add(randomPos);
            spawnedProps.Add(prop); // track it so we can show/hide later
            spawned++;
        }
    }

    Vector3 GetRandomPointInPolygon()
    {
        Bounds bounds = spawnZone.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector3(x, y, 0);
    }

    bool IsFarEnough(Vector3 pos)
    {
        foreach (Vector3 existing in spawnedPositions)
        {
            if (Vector3.Distance(pos, existing) < minDistanceBetweenProps)
                return false;
        }
        return true;
    }

    bool IsTooCloseToHouse(Vector3 pos)
    {
        if (houseTransform == null) return false;
        return Vector3.Distance(pos, houseTransform.position) < houseRadius;
    }
}