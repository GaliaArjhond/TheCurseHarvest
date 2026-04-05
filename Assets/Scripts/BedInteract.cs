using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BedInteract : MonoBehaviour
{
    [Header("Sleep Dialog")]
    [SerializeField] private GameObject sleepDialog;
    [SerializeField] private Button restButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private TextMeshProUGUI nextDayText;

    [Header("Fade")]
    [SerializeField] private Image fadePanel;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Prompt")]
    [SerializeField] private GameObject interactPrompt;

    private bool playerInRange = false;
    private bool isTransitioning = false;
    private bool canTrigger = false; // ← NEW — prevents triggering on spawn

    void Start()
    {
        if (sleepDialog != null) sleepDialog.SetActive(false);
        if (interactPrompt != null) interactPrompt.SetActive(false);

        restButton?.onClick.AddListener(OnRest);
        cancelButton?.onClick.AddListener(OnCancel);

        // wait 1 second before allowing trigger
        // prevents dialog showing when player spawns on bed
        StartCoroutine(EnableTriggerAfterDelay());
    }

    IEnumerator EnableTriggerAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        canTrigger = true;
        Debug.Log("Bed trigger enabled");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (!canTrigger) return; // ← ignore trigger on spawn
        playerInRange = true;
        if (interactPrompt != null) interactPrompt.SetActive(true);
        ShowDialog();
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;
        if (interactPrompt != null) interactPrompt.SetActive(false);
        HideDialog();
    }

    void ShowDialog()
    {
        if (sleepDialog == null) return;
        sleepDialog.SetActive(true);

        if (nextDayText != null && DayManager.Instance != null)
        {
            int nextDay = DayManager.Instance.dayNumber + 1;
            int nextSeason = DayManager.Instance.seasonIndex;

            if (nextDay > DayManager.Instance.daysPerSeason)
            {
                nextDay = 1;
                nextSeason = (nextSeason + 1) % 2;
            }

            nextDayText.text = "Wake up on: "
                             + DayManager.Instance.seasonNames[nextSeason]
                             + " Day " + nextDay;
        }
    }

    void HideDialog()
    {
        if (sleepDialog != null) sleepDialog.SetActive(false);
    }

    void OnRest()
    {
        if (isTransitioning) return;
        StartCoroutine(SleepRoutine());
    }

    void OnCancel()
    {
        HideDialog();
    }

    IEnumerator SleepRoutine()
    {
        isTransitioning = true;
        HideDialog();

        yield return StartCoroutine(Fade(0f, 1f));

        if (DayManager.Instance != null)
            DayManager.Instance.AdvanceDay();

        PlayerStatsManager stats = FindObjectOfType<PlayerStatsManager>();
        if (stats != null)
            stats.RestoreAll();

        // move player to outdoor spawn BEFORE saving
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject outdoorSpawn = GameObject.Find("ExteriorSpawn");
        if (player != null && outdoorSpawn != null)
        {
            player.transform.position = outdoorSpawn.transform.position;
            Debug.Log("Player moved to: " + outdoorSpawn.transform.position);
        }
        else
            Debug.LogWarning("ExteriorSpawn not found!");

        if (SaveController.Instance != null)
            SaveController.Instance.SaveGame();

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(Fade(1f, 0f));

        isTransitioning = false;
    }

    IEnumerator Fade(float from, float to)
    {
        if (fadePanel == null) yield break;

        float elapsed = 0f;
        Color c = fadePanel.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, elapsed / fadeDuration);
            fadePanel.color = c;
            yield return null;
        }

        c.a = to;
        fadePanel.color = c;
    }
}