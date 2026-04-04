using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class PlayerStatsData
{
    [Header("Level")]
    public int level = 1;
    public int currentExp = 0;
    public int expToNextLevel = 100;

    [Header("Core Stats")]
    public int strength = 5;
    public int defense = 5;
    public float speed = 5f;

    [Header("Health & Stamina")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float currentHealth;
    public float currentStamina;

    public PlayerStatsData()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
    }
}