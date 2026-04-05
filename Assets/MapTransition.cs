using Cinemachine;
using System.Collections;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [Header("Boundary for destination area")]
    [SerializeField] private PolygonCollider2D areaBoundary;

    [Header("Direction player is moving")]
    [SerializeField] private Direction direction;
    [SerializeField] private Transform TeleportTargetPosition;
    [SerializeField] private float additivePos = 2f;

    [Header("Cinemachine")]
    [SerializeField] private GameObject cmCamObject;

    // ← uses CinemachineConfiner (v2) not CinemachineConfiner2D (v3)
    private CinemachineConfiner confiner;
    private bool isTransitioning = false;

    enum Direction { Up, Down, Left, Right, Teleport }

    void Awake()
    {
        if (cmCamObject != null)
            confiner = cmCamObject.GetComponent<CinemachineConfiner>();

        if (confiner == null)
            Debug.LogError("CinemachineConfiner not found on " + gameObject.name);
        if (areaBoundary == null)
            Debug.LogError("Area boundary not assigned on " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isTransitioning) return;
        if (confiner == null || areaBoundary == null) return;

        StartCoroutine(DoTransition(collision.gameObject));
    }

    IEnumerator DoTransition(GameObject player)
    {
        isTransitioning = true;

        // move player past the edge
        UpdatePlayerPosition(player);

        // swap to new boundary — camera glides smoothly
        confiner.m_BoundingShape2D = areaBoundary;
        confiner.InvalidatePathCache();

        // prevent double trigger
        yield return new WaitForSeconds(1f);

        isTransitioning = false;
    }

    void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
        {
            player.transform.position = TeleportTargetPosition.position;
            return;
        }
        Vector3 newPos = player.transform.position;

        switch (direction)
        {
            case Direction.Up: newPos.y += additivePos; break;
            case Direction.Down: newPos.y -= additivePos; break;
            case Direction.Left: newPos.x -= additivePos; break;
            case Direction.Right: newPos.x += additivePos; break;
        }

        player.transform.position = newPos;
    }
}