using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string mapBoundary;
    public List<InventorySaveData> inventorySaveData;
    public List<InventorySaveData> hotbarSaveData;

    // Health & Stamina
    public float health;
    public float stamina;
    public float maxHealth;
    public float maxStamina;

    // Level & EXP
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // Core Stats
    public int strength = 5;
    public int defense = 5;
    public float speed = 5f;

    // Day & Season
    public int dayNumber = 1;
    public int seasonIndex = 0;

}
