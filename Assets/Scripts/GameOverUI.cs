using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public static GameOverUI Instance;

    [SerializeField]
    private GameObject panel;

    [Header("Buttons")]
    [SerializeField]
    private Button retryButton;

    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private Button quitButton;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("[GAME OVER UI] Initialized. Panel: " + (panel != null ? panel.name : "NULL"));
        }
        else if (Instance != this)
        {
            Debug.LogWarning("[GAME OVER UI] Duplicate instance found, destroying.");
            Destroy(gameObject);
            return;
        }

        if (panel != null)
            panel.SetActive(false);
        else
            Debug.LogError("[GAME OVER UI] Panel NOT assigned in Inspector!");

        SetupButtons();
    }

    private void SetupButtons()
    {
        if (retryButton != null)
        {
            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(RetryDay);
        }
        else
        {
            Debug.LogWarning("[GAME OVER UI] RetryButton is not assigned.");
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(MainMenu);
        }
        else
        {
            Debug.LogWarning("[GAME OVER UI] MainMenuButton is not assigned.");
        }

        if (quitButton != null)
        {
            quitButton.onClick.RemoveAllListeners();
            quitButton.onClick.AddListener(QuitGame);
        }
        else
        {
            Debug.LogWarning("[GAME OVER UI] QuitButton is not assigned.");
        }
    }

    public void Show()
    {
        Debug.Log("[GAME OVER UI] Show() called. Panel: " + (panel != null ? panel.name : "NULL"));

        if (panel != null)
        {
            panel.SetActive(true);
            Debug.Log("[GAME OVER UI] Panel activated. Active: " + panel.activeSelf);
        }
        else
        {
            Debug.LogError("[GAME OVER UI ERROR] Panel is NULL!");
        }

        Time.timeScale = 0f;
        Debug.Log("[GAME OVER UI] Time paused. timeScale: " + Time.timeScale);
    }

    public void RetryDay()
    {
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(SpawnManager.bedSpawnPointName))
            SpawnManager.spawnPointName = SpawnManager.bedSpawnPointName;

        if (DayManager.Instance != null)
            DayManager.Instance.AdvanceDay();

        if (PlayerStatsManager.Instance != null)
            PlayerStatsManager.Instance.RestoreAll();

        DayNightCycle cycle = FindFirstObjectByType<DayNightCycle>();
        if (cycle != null)
            cycle.ResetToMorning();

        if (panel != null)
            panel.SetActive(false);

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }

    public void MainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}