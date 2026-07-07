using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public Vector3 playerPosition;
    public string mapBoundaryName = "";

    public List<InventorySaveData> inventorySaveData = new List<InventorySaveData>();
    public List<InventorySaveData> hotbarSaveData = new List<InventorySaveData>();
    public List<PropSaveData> forestProps = new List<PropSaveData>();
    public List<InventorySaveData> chestSaveData = new List<InventorySaveData>();


    // Health & Stamina
    public float health;
    public float stamina;
    public float maxHealth;
    public float maxStamina;

    // Level & EXP
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    // Skills
    public int skillPoints;
    public bool yield1Unlocked;
    public bool yield2Unlocked;
    public bool yield3Unlocked;

    public bool water1Unlocked;
    public bool water2Unlocked;
    public bool water3Unlocked;

    public bool growth1Unlocked;
    public bool growth2Unlocked;
    public bool growth3Unlocked;

    public bool sword1Unlocked;
    public bool sword2Unlocked;
    public bool sword3Unlocked;

    public bool defense1Unlocked;
    public bool defense2Unlocked;
    public bool defense3Unlocked;

    public bool agility1Unlocked;
    public bool agility2Unlocked;
    public bool agility3Unlocked;

    public bool anino1Unlocked;
    public bool anino2Unlocked;
    public bool anino3Unlocked;

    public bool kulam1Unlocked;
    public bool kulam2Unlocked;
    public bool kulam3Unlocked;

    public bool bangungot1Unlocked;
    public bool bangungot2Unlocked;
    public bool bangungot3Unlocked;

    // Stats
    public int strength = 5;
    public int defense = 5;
    public float speed = 5f;

    // Day & Season
    public int dayNumber = 1;
    public int seasonIndex = 0;

    // Save metadata
    public string playerName = "Player";
    public string lastPlayed = "";
    public string saveFileName = "";

    // Tutorial
    public bool tutorialCompleted = false;
}