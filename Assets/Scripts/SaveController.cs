using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    private string saveFilePath;

    void Awake()
    {
        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        // position
        if (player != null)
            data.playerPosition = player.transform.position;

        // stats
        PlayerStatsManager stats = player?.GetComponent<PlayerStatsManager>();
        if (stats != null)
        {
            data.health = stats.GetHealth();
            data.stamina = stats.GetStamina();
            data.maxHealth = stats.GetMaxHealth();
            data.maxStamina = stats.GetMaxStamina();
            data.level = stats.GetLevel();
            data.currentExp = stats.GetExp();
            data.expToNextLevel = stats.GetExpToNext();
            data.strength = stats.GetStrength();
            data.defense = stats.GetDefense();
            data.speed = stats.GetSpeed();
        }

        // day
        if (DayManager.Instance != null)
        {
            data.dayNumber = DayManager.Instance.dayNumber;
            data.seasonIndex = DayManager.Instance.seasonIndex;
        }

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(data, true));
        Debug.Log("Game saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file found — starting fresh!");
            return;
        }

        SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveFilePath));

        // position
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            player.transform.position = data.playerPosition;

        // stats
        PlayerStatsManager stats = player?.GetComponent<PlayerStatsManager>();
        if (stats != null)
        {
            stats.SetMaxHealth(data.maxHealth);
            stats.SetMaxStamina(data.maxStamina);
            stats.SetHealth(data.health);
            stats.SetStamina(data.stamina);
            stats.SetLevel(data.level);
            stats.SetExp(data.currentExp);
            stats.SetExpToNext(data.expToNextLevel);
            stats.SetStrength(data.strength);
            stats.SetDefense(data.defense);
            stats.SetSpeed(data.speed);
        }

        // day
        if (DayManager.Instance != null)
        {
            DayManager.Instance.dayNumber = data.dayNumber;
            DayManager.Instance.seasonIndex = data.seasonIndex;
            DayManager.Instance.UpdateDayUI();
        }

        Debug.Log("Game loaded — Level: " + data.level + " Day: " + data.dayNumber);
    }

    public bool HasSave()
    {
        return File.Exists(saveFilePath);
    }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
            File.Delete(saveFilePath);
        Debug.Log("Save deleted!");
    }
}