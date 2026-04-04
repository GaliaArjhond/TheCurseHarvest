using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;

public class DayManager : MonoBehaviour
{
    public static DayManager Instance;

    [Header("Day Settings")]
    public int dayNumber = 1;
    public int seasonIndex = 0; // 0=Tag-init 1=Tag-ulan
    public int daysPerSeason = 30;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI dayText;

    public string[] seasonNames = { "Tag-init", "Tag-ulan" };

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

        // switch season every daysPerSeason days
        if (dayNumber > daysPerSeason)
        {
            dayNumber = 1;
            seasonIndex = (seasonIndex + 1) % seasonNames.Length;
        }

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