using UnityEngine;
using Cinemachine;

public class CameraFollowSetter : MonoBehaviour
{
    [SerializeField] private PolygonCollider2D sceneBounds;

    private CinemachineVirtualCamera cam;
    private CinemachineConfiner confiner;

    void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner>();

        GameObject player =
            GameObject.FindGameObjectWithTag("Player");

        if (cam != null && player != null)
        {
            cam.Follow = player.transform;
            cam.LookAt = player.transform;
            cam.PreviousStateIsValid = false;
        }

        if (confiner != null && sceneBounds != null)
        {
            confiner.m_BoundingShape2D = sceneBounds;
            confiner.InvalidatePathCache();
        }
    }
}