using UnityEngine;
using Cinemachine;

public class CameraFollowSetter : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D sceneBounds;

    void Start()
    {
        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        CinemachineVirtualCamera vcam =
            GetComponent<CinemachineVirtualCamera>();

        CinemachineConfiner confiner =
            GetComponent<CinemachineConfiner>();

        if (vcam != null && player != null)
        {
            vcam.Priority = 50;
            vcam.Follow = player.transform;
            vcam.LookAt = player.transform;
            vcam.PreviousStateIsValid = false;

            vcam.ForceCameraPosition(
                player.transform.position + new Vector3(0, 0, -10),
                Quaternion.identity
            );
        }

        if (confiner != null && sceneBounds != null)
        {
            confiner.m_BoundingShape2D = sceneBounds;
            confiner.InvalidatePathCache();
        }

        Debug.Log("Cave camera fixed to player.");
    }
}