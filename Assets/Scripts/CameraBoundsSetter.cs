using Cinemachine;
using UnityEngine;

public class CameraBoundsSetter : MonoBehaviour
{
    [SerializeField] private CinemachineConfiner confiner;

    [Header("Bounds")]
    [SerializeField] private PolygonCollider2D houseBounds;
    [SerializeField] private PolygonCollider2D caveExitBounds;

    void Start()
    {
        if (confiner == null)
            return;

        PolygonCollider2D targetBounds = houseBounds;

        // If player came from cave, use cave boundary
        if (SpawnManager.spawnPointName == "FarmCaveExit")
        {
            targetBounds = caveExitBounds;
        }

        if (targetBounds == null)
            return;

        confiner.m_BoundingShape2D = targetBounds;
        confiner.InvalidatePathCache();

        Debug.Log("Camera bounds set to: " + targetBounds.name);
    }
}