using Cinemachine;
using UnityEngine;

public class PlayerSpawn : MonoBehaviour
{
    void Start()
    {
        if (string.IsNullOrEmpty(SpawnManager.spawnPointName))
            return;

        GameObject spawn =
            GameObject.Find(SpawnManager.spawnPointName);

        if (spawn != null)
            transform.position = spawn.transform.position;

        UpdateCameraBounds();
    }

    void UpdateCameraBounds()
    {
        CinemachineConfiner confiner =
            FindFirstObjectByType<CinemachineConfiner>();

        if (confiner == null)
            return;

        GameObject boundsObj = null;

        if (SpawnManager.spawnPointName == "CaveExit")
            boundsObj = GameObject.Find("Cave");
        else
            boundsObj = GameObject.Find("HouseBounds");

        if (boundsObj == null)
        {
            Debug.LogWarning("Camera bounds object not found.");
            return;
        }

        PolygonCollider2D bounds =
            boundsObj.GetComponent<PolygonCollider2D>();

        if (bounds == null)
            return;

        confiner.m_BoundingShape2D = bounds;
        confiner.InvalidatePathCache();

        Debug.Log("Camera bounds changed to: " + boundsObj.name);
    }
}