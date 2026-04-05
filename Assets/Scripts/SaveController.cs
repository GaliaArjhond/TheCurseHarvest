using Cinemachine;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    public static SaveController Instance;

    private InventoryController inventoryController;
    private HotBarController hotbarController;
    private CinemachineConfiner cinemachineConfiner;
    private string saveFilePath;

    void Awake() 
    {
        Instance = this;
        saveFilePath = Path.Combine(Application.persistentDataPath, "savegame.json");

        inventoryController = Object.FindFirstObjectByType<InventoryController>();
        hotbarController = Object.FindFirstObjectByType<HotBarController>();
        cinemachineConfiner = Object.FindFirstObjectByType<CinemachineConfiner>();
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

        if (cinemachineConfiner != null && cinemachineConfiner.m_BoundingShape2D != null)
            data.mapBoundaryName = cinemachineConfiner.m_BoundingShape2D.gameObject.name;

        if (inventoryController != null)
            data.inventorySaveData = inventoryController.GetInventoryItems();

        if (hotbarController != null)
            data.hotbarSaveData = hotbarController.GetHotbarItems();

        File.WriteAllText(saveFilePath, JsonUtility.ToJson(data, true));
        Debug.Log("Game saved to: " + saveFilePath);
    }

    public void LoadGame()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("No save file — skipping load, keeping default inventory");
            return; // ← exits early, inventory stays as is
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

        // map boundary
        if (!string.IsNullOrEmpty(data.mapBoundaryName) && cinemachineConfiner != null)
        {
            GameObject boundaryObj = GameObject.Find(data.mapBoundaryName);
            if (boundaryObj != null)
            {
                cinemachineConfiner.m_BoundingShape2D = boundaryObj.GetComponent<PolygonCollider2D>();
                cinemachineConfiner.InvalidatePathCache();
            }
        }

        // inventory — only load if there is actual saved data
        if (inventoryController != null
            && data.inventorySaveData != null
            && data.inventorySaveData.Count > 0) // ← only if items were actually saved
        {
            inventoryController.SetInventoryItems(data.inventorySaveData);
            Debug.Log("Inventory loaded — " + data.inventorySaveData.Count + " items");
        }
        else
            Debug.Log("No inventory data to load — keeping default");

        // hotbar — only load if there is actual saved data
        if (hotbarController != null
            && data.hotbarSaveData != null
            && data.hotbarSaveData.Count > 0) // ← only if items were actually saved
        {
            hotbarController.SetHotbarItems(data.hotbarSaveData);
            Debug.Log("Hotbar loaded — " + data.hotbarSaveData.Count + " items");
        }
        else
            Debug.Log("No hotbar data to load — keeping default");

        Debug.Log("Game loaded — Level: " + data.level + " Day: " + data.dayNumber);
    }

    public bool HasSave() { return File.Exists(saveFilePath); }

    public void DeleteSave()
    {
        if (File.Exists(saveFilePath))
            File.Delete(saveFilePath);
        Debug.Log("Save deleted!");
    }
}