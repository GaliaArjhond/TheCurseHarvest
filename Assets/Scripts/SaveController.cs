using Cinemachine;
using System.IO;
using UnityEngine;

public class SaveController : MonoBehaviour
{
    private string saveLocation;
    private InventoryController inventoryController;

    void Start()
    {
        saveLocation = Path.Combine(Application.persistentDataPath, "saveData.json");
        inventoryController = FindObjectOfType<InventoryController>();
        LoadGame();
    }

    public void SaveGame()
    {
        CinemachineConfiner confiner = FindObjectOfType<CinemachineConfiner>();

        SaveData saveData = new SaveData()
        {
            playerPosition = GameObject.FindWithTag("Player").transform.position,
            mapBoundary = confiner.m_BoundingShape2D.gameObject.name,
            inventorySaveData = inventoryController.GetInventoryItems()  
        };

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(saveLocation, json);
    }

    public void LoadGame()
    {
        if (File.Exists(saveLocation))  // Fixed: removed ! so we read when file EXISTS
        {
            SaveData saveData = JsonUtility.FromJson<SaveData>(File.ReadAllText(saveLocation));

            GameObject.FindWithTag("Player").transform.position = saveData.playerPosition;  // Fixed: singular FindWithTag

            FindObjectOfType<CinemachineConfiner>().m_BoundingShape2D =
                GameObject.Find(saveData.mapBoundary).GetComponent<PolygonCollider2D>();

            inventoryController.SetInventoryItems(saveData.inventorySaveData);  
        }
        else  // File does not exist, create a fresh save
        {
            SaveGame();
            Debug.LogWarning("No save file found, creating a new one.");  // Fixed: saveData was out of scope here
        }
    }
}