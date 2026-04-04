using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance;

    [Header("Stats")]
    public PlayerStatsData stats = new PlayerStatsData();

    [Header("Stamina Drain")]
    public float walkStaminaDrain = 3f;

    [Header("Stamina Regen")]
    [SerializeField] private float staminaRegenRate = 5f;
    [SerializeField] private float staminaRegenDelay = 1f;
    private float staminaRegenTimer = 0f;

    [Header("Health Regen")]
    [SerializeField] private float healthRegenRate = 2f;
    [SerializeField] private float healthRegenDelay = 5f;
    private float healthRegenTimer = 0f;

    [Header("HUD — Health")]
    [SerializeField] private Slider healthBarHUD;
    [SerializeField] private TextMeshProUGUI healthTextHUD;

    [Header("HUD — Stamina")]
    [SerializeField] private Slider staminaBarHUD;
    [SerializeField] private TextMeshProUGUI staminaTextHUD;

    [Header("HUD — Level & EXP")]
    [SerializeField] private TextMeshProUGUI levelTextHUD;
    [SerializeField] private Slider expBarHUD;

    [Header("Book Page — Stats")]
    [SerializeField] private TextMeshProUGUI bookLevelText;
    [SerializeField] private TextMeshProUGUI bookExpText;
    [SerializeField] private TextMeshProUGUI bookStrengthText;
    [SerializeField] private TextMeshProUGUI bookDefenseText;
    [SerializeField] private TextMeshProUGUI bookSpeedText;
    [SerializeField] private TextMeshProUGUI bookHealthText;
    [SerializeField] private TextMeshProUGUI bookStaminaText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        stats.currentHealth = stats.maxHealth;
        stats.currentStamina = stats.maxStamina;
        UpdateAllUI();
    }

    void Update()
    {
        HandleStaminaRegen();
        HandleHealthRegen();
        UpdateAllUI();
    }

    // ── called by PlayerMovement ──
    public void DrainStamina(float amount)
    {
        if (stats.currentStamina <= 0) return;
        stats.currentStamina -= amount;
        stats.currentStamina = Mathf.Max(stats.currentStamina, 0);
        staminaRegenTimer = staminaRegenDelay;
    }

    void HandleStaminaRegen()
    {
        if (staminaRegenTimer > 0) { staminaRegenTimer -= Time.deltaTime; return; }
        if (stats.currentStamina < stats.maxStamina)
        {
            stats.currentStamina += staminaRegenRate * Time.deltaTime;
            stats.currentStamina = Mathf.Min(stats.currentStamina, stats.maxStamina);
        }
    }

    void HandleHealthRegen()
    {
        if (healthRegenTimer > 0) { healthRegenTimer -= Time.deltaTime; return; }
        if (stats.currentHealth < stats.maxHealth)
        {
            stats.currentHealth += healthRegenRate * Time.deltaTime;
            stats.currentHealth = Mathf.Min(stats.currentHealth, stats.maxHealth);
        }
    }

    // ── add EXP and check level up ──
    public void AddExp(int amount)
    {
        stats.currentExp += amount;
        Debug.Log("+" + amount + " EXP — Total: " + stats.currentExp);

        while (stats.currentExp >= stats.expToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        stats.currentExp -= stats.expToNextLevel;
        stats.level++;
        stats.expToNextLevel = Mathf.RoundToInt(stats.expToNextLevel * 1.5f);

        // boost stats on level up
        stats.strength++;
        stats.defense++;
        stats.speed += 0.2f;
        stats.maxHealth += 10f;
        stats.maxStamina += 5f;
        stats.currentHealth = stats.maxHealth;
        stats.currentStamina = stats.maxStamina;

        Debug.Log("LEVEL UP! Now level " + stats.level);
        UpdateAllUI();
    }

    // ── damage and heal ──
    public void TakeDamage(float amount)
    {
        // reduce by defense
        float reduced = Mathf.Max(amount - stats.defense * 0.5f, 1f);
        stats.currentHealth -= reduced;
        stats.currentHealth = Mathf.Max(stats.currentHealth, 0);
        healthRegenTimer = healthRegenDelay;

        if (stats.currentHealth <= 0) Die();
    }

    public void Heal(float amount)
    {
        stats.currentHealth += amount;
        stats.currentHealth = Mathf.Min(stats.currentHealth, stats.maxHealth);
    }

    public void RestoreAll()
    {
        stats.currentHealth = stats.maxHealth;
        stats.currentStamina = stats.maxStamina;
    }

    void Die()
    {
        Debug.Log("Player died!");
    }

    // ── getters for SaveController ──
    public float GetHealth() { return stats.currentHealth; }
    public float GetMaxHealth() { return stats.maxHealth; }
    public float GetStamina() { return stats.currentStamina; }
    public float GetMaxStamina() { return stats.maxStamina; }
    public int GetLevel() { return stats.level; }
    public int GetExp() { return stats.currentExp; }
    public int GetExpToNext() { return stats.expToNextLevel; }
    public int GetStrength() { return stats.strength; }
    public int GetDefense() { return stats.defense; }
    public float GetSpeed() { return stats.speed; }

    // ── setters for SaveController ──
    public void SetHealth(float v) { stats.currentHealth = Mathf.Clamp(v, 0, stats.maxHealth); }
    public void SetStamina(float v) { stats.currentStamina = Mathf.Clamp(v, 0, stats.maxStamina); }
    public void SetLevel(int v) { stats.level = v; }
    public void SetExp(int v) { stats.currentExp = v; }
    public void SetExpToNext(int v) { stats.expToNextLevel = v; }
    public void SetStrength(int v) { stats.strength = v; }
    public void SetDefense(int v) { stats.defense = v; }
    public void SetSpeed(float v) { stats.speed = v; }
    public void SetMaxHealth(float v) { stats.maxHealth = v; }
    public void SetMaxStamina(float v) { stats.maxStamina = v; }

    void UpdateAllUI()
    {
        UpdateHUD();
        UpdateBookPage();
    }

    void UpdateHUD()
    {
        if (healthBarHUD != null) healthBarHUD.value = stats.currentHealth / stats.maxHealth;
        if (staminaBarHUD != null) staminaBarHUD.value = stats.currentStamina / stats.maxStamina;
        if (healthTextHUD != null) healthTextHUD.text = Mathf.CeilToInt(stats.currentHealth) + "/" + (int)stats.maxHealth;
        if (staminaTextHUD != null) staminaTextHUD.text = Mathf.CeilToInt(stats.currentStamina) + "/" + (int)stats.maxStamina;
        if (levelTextHUD != null) levelTextHUD.text = "Lv." + stats.level;
        if (expBarHUD != null) expBarHUD.value = (float)stats.currentExp / stats.expToNextLevel;
    }

    void UpdateBookPage()
    {
        if (bookLevelText != null) bookLevelText.text = "Level: " + stats.level;
        if (bookExpText != null) bookExpText.text = "EXP: " + stats.currentExp + " / " + stats.expToNextLevel;
        if (bookStrengthText != null) bookStrengthText.text = "Strength: " + stats.strength;
        if (bookDefenseText != null) bookDefenseText.text = "Defense: " + stats.defense;
        if (bookSpeedText != null) bookSpeedText.text = "Speed: " + stats.speed.ToString("F1");
        if (bookHealthText != null) bookHealthText.text = "Health: " + Mathf.CeilToInt(stats.currentHealth) + " / " + (int)stats.maxHealth;
        if (bookStaminaText != null) bookStaminaText.text = "Stamina: " + Mathf.CeilToInt(stats.currentStamina) + " / " + (int)stats.maxStamina;
    }
}