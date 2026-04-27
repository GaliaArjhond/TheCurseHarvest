using UnityEngine;
using TMPro;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Day Settings")]
    public int dayNumber = 1;
    public int seasonIndex = 0;
    public int daysPerSeason = 30;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;

    public string[] seasonNames = { "Tag-init", "Tag-ulan" };

    public event System.Action onNewDay;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        UpdateDayUI();
    }

    public void AdvanceDay()
    {
        dayNumber++;

        if (dayNumber > daysPerSeason)
        {
            dayNumber = 1;
            seasonIndex = (seasonIndex + 1) % seasonNames.Length;
        }

        // fire new day event — FarmTiles and BedInteract listen to this
        onNewDay?.Invoke();

        UpdateDayUI();
        Debug.Log("Day advanced: " + GetDayString());
    }

    public void UpdateDayUI()
    {
        if (dayText != null)
            dayText.text = seasonNames[seasonIndex] + " — Day " + dayNumber;
    }

    public string GetDayString()
    {
        return seasonNames[seasonIndex] + " Day " + dayNumber;
    }

    public void LoadFromData(SaveData data)
    {
        dayNumber = data.dayNumber;
        seasonIndex = data.seasonIndex;
        UpdateDayUI();
    }
}