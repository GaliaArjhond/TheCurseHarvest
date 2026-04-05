using Cinemachine;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{

    public static SaveController Instance;
    private string saveLocation;
    private InventoryController inventoryController;
    private hotbarControler hotbarController;
    private PlayerStatsManager statsManager;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");

        inventoryController = FindObjectOfType<InventoryController>();
        hotbarController = FindObjectOfType<hotbarControler>();
        statsManager = FindObjectOfType<PlayerStatsManager>();
        LoadGame();
    }

    public void SaveGame()
    {
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position,
            mapBoundary = confiner.m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems(),
            hotbarSaveData = hotbarController.GetHotbarItems(),

            health = statsManager.GetHealth(),
            stamina = statsManager.GetStamina(),
            maxHealth = statsManager.GetMaxHealth(),
            maxStamina = statsManager.GetMaxStamina(),
            level = statsManager.GetLevel(),
            currentExp = statsManager.GetExp(),
            expToNextLevel = statsManager.GetExpToNext(),
            strength = statsManager.GetStrength(),
            defense = statsManager.GetDefense(),
            speed = statsManager.GetSpeed()
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveLocation, json);
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindWithTag("Player").transform.position = saveData.playerPosition;

            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
                GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);
            hotbarController.SetHotbarItems(saveData.hotbarSaveData);

            if (statsManager != null)
            {
                statsManager.SetMaxHealth(saveData.maxHealth);
                statsManager.SetMaxStamina(saveData.maxStamina);
                statsManager.SetHealth(saveData.health);
                statsManager.SetStamina(saveData.stamina);
                statsManager.SetLevel(saveData.level);
                statsManager.SetExp(saveData.currentExp);
                statsManager.SetExpToNext(saveData.expToNextLevel);
                statsManager.SetStrength(saveData.strength);
                statsManager.SetDefense(saveData.defense);
                statsManager.SetSpeed(saveData.speed);
            }
        }
        else
        {
            SaveGame();
            Debug.LogWarning("No save file found, creating a new one.");
        }
    }
    public bool HasSave()
    {
        return File.Exists(saveLocation);
    }
}