using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        ApplySpawn();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplySpawn();
    }

    void ApplySpawn()
    {
        if (string.IsNullOrEmpty(SpawnManager.spawnPointName))
            return;

        GameObject spawn =
            GameObject.Find(SpawnManager.spawnPointName);

        if (spawn != null)
        {
            transform.position = spawn.transform.position;
            Physics2D.SyncTransforms();
        }

        StartCoroutine(DelayedCameraFix());
        System.Collections.IEnumerator DelayedCameraFix()
        {
            yield return null;

            UpdateCameraFollow();
            UpdateCameraBounds();
        }
    }

    void UpdateCameraFollow()
    {
        CinemachineVirtualCamera cam =
            FindFirstObjectByType<CinemachineVirtualCamera>();

        if (cam != null)
        {
            cam.Follow = transform;
            cam.LookAt = transform;
            cam.PreviousStateIsValid = false;
        }
    }

    void UpdateCameraBounds()
    {
        CinemachineConfiner confiner =
            FindFirstObjectByType<CinemachineConfiner>();

        if (confiner == null)
            return;

        string boundsName = "HouseBounds";

        if (SpawnManager.spawnPointName == "CaveExit")
            boundsName = "CaveBounds";

        GameObject boundsObj =
            GameObject.Find(boundsName);

        if (boundsObj == null)
        {
            Debug.LogWarning("Camera bounds not found: " + boundsName);
            return;
        }

        PolygonCollider2D bounds =
            boundsObj.GetComponent<PolygonCollider2D>();

        if (bounds == null)
        {
            Debug.LogWarning(boundsName + " has no PolygonCollider2D");
            return;
        }

        confiner.m_BoundingShape2D = bounds;
        confiner.InvalidatePathCache();

        Debug.Log(
            "Camera bounds changed to: " +
            bounds.name +
            " on object: " +
            bounds.gameObject.name
        );
    }
}