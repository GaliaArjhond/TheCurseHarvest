using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class SaveData
{
    // Player position
    public Vector3 playerPosition;

    // Health & Stamina
    public float health;
    public float stamina;
    public float maxHealth;
    public float maxStamina;

    // Level & EXP
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    public int itemID;
    public int slotIndex;

    // Core Stats
    public int strength = 5;
    public int defense = 5;
    public float speed = 5f;

    // Day & Season
    public int dayNumber = 1;
    public int seasonIndex = 0;

    // Inventory
    public List<InventorySaveData> inventorySaveData;
}