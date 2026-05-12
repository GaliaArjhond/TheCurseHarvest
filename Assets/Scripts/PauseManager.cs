using UnityEngine;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance;

    public bool IsPaused { get; private set; }

    void Awake()
    {
        Instance = this;
        Time.timeScale = 1f;
    }

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;

        Debug.Log("PAUSE SET: " + paused + " | TimeScale = " + Time.timeScale);
    }

    void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}