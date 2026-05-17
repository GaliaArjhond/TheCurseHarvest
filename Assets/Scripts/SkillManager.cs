using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("Skill Points")]
    [SerializeField] private int skillPoints = 0;
    [SerializeField] private TextMeshProUGUI skillPointText;

    [Header("Yield Buttons")]
    [SerializeField] private Button yield1Button;
    [SerializeField] private Button yield2Button;
    [SerializeField] private Button yield3Button;

    [Header("Yield Lines")]
    [SerializeField] private Image yieldLine1;
    [SerializeField] private Image yieldLine2;

    [Header("Water Buttons")]
    [SerializeField] private Button water1Button;
    [SerializeField] private Button water2Button;
    [SerializeField] private Button water3Button;

    [Header("Water Lines")]
    [SerializeField] private Image waterLine1;
    [SerializeField] private Image waterLine2;

    [Header("Growth Buttons")]
    [SerializeField] private Button growth1Button;
    [SerializeField] private Button growth2Button;
    [SerializeField] private Button growth3Button;

    [Header("Growth Lines")]
    [SerializeField] private Image growthLine1;
    [SerializeField] private Image growthLine2;

    [Header("Sword Buttons")]
    [SerializeField] private Button sword1Button;
    [SerializeField] private Button sword2Button;
    [SerializeField] private Button sword3Button;

    [Header("Sword Lines")]
    [SerializeField] private Image swordLine1;
    [SerializeField] private Image swordLine2;

    [Header("Defense Buttons")]
    [SerializeField] private Button defense1Button;
    [SerializeField] private Button defense2Button;
    [SerializeField] private Button defense3Button;

    [SerializeField] private Image defenseLine1;
    [SerializeField] private Image defenseLine2;

    [Header("Agility Skills")]
    [SerializeField] private Button agility1Button;
    [SerializeField] private Button agility2Button;
    [SerializeField] private Button agility3Button;

    [SerializeField] private Image agilityLine1;
    [SerializeField] private Image agilityLine2;

    // ── unlock states ──
    public bool sword1Unlocked;
    public bool sword2Unlocked;
    public bool sword3Unlocked;

    public bool yield1Unlocked;
    public bool yield2Unlocked;
    public bool yield3Unlocked;

    public bool water1Unlocked;
    public bool water2Unlocked;
    public bool water3Unlocked;

    public bool growth1Unlocked;
    public bool growth2Unlocked;
    public bool growth3Unlocked;

    // Defense unlocks
    public bool defense1Unlocked;
    public bool defense2Unlocked;
    public bool defense3Unlocked;

    // Agility unlocks
    public bool agility1Unlocked;
    public bool agility2Unlocked;
    public bool agility3Unlocked;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        RefreshUI();
    }

    // ─────────────────────────────
    // SKILL POINTS
    // ─────────────────────────────

    public void AddSkillPoint(int amount)
    {
        skillPoints += amount;

        Debug.Log("Skill Points: " + skillPoints);

        RefreshUI();
    }

    // ─────────────────────────────
    // YIELD
    // ─────────────────────────────

    public void UnlockYield1()
    {
        UnlockSkill(ref yield1Unlocked, true, 1);
    }

    public void UnlockYield2()
    {
        UnlockSkill(ref yield2Unlocked, yield1Unlocked, 2);
    }

    public void UnlockYield3()
    {
        UnlockSkill(ref yield3Unlocked, yield2Unlocked, 3);
    }

    // ─────────────────────────────
    // WATER
    // ─────────────────────────────

    public void UnlockWater1()
    {
        UnlockSkill(ref water1Unlocked, true, 1);
    }

    public void UnlockWater2()
    {
        UnlockSkill(ref water2Unlocked, water1Unlocked, 2);
    }

    public void UnlockWater3()
    {
        UnlockSkill(ref water3Unlocked, water2Unlocked, 3);
    }

    // ─────────────────────────────
    // GROWTH
    // ─────────────────────────────

    public void UnlockGrowth1()
    {
        UnlockSkill(ref growth1Unlocked, true, 1);
    }

    public void UnlockGrowth2()
    {
        UnlockSkill(ref growth2Unlocked, growth1Unlocked, 2);
    }

    public void UnlockGrowth3()
    {
        UnlockSkill(ref growth3Unlocked, growth2Unlocked, 3);
    }

    // ─────────────────────────────
    // SWORD
    // ─────────────────────────────

    public void UnlockSword1()
    {
        UnlockSkill(ref sword1Unlocked, true, 1);
    }

    public void UnlockSword2()
    {
        UnlockSkill(ref sword2Unlocked, sword1Unlocked, 2);
    }

    public void UnlockSword3()
    {
        UnlockSkill(ref sword3Unlocked, sword2Unlocked, 3);
    }

    // DEFENSE

    public void UnlockDefense1()
    {
        UnlockSkill(ref defense1Unlocked, true, 1);
    }

    public void UnlockDefense2()
    {
        UnlockSkill(ref defense2Unlocked, defense1Unlocked, 2);
    }

    public void UnlockDefense3()
    {
        UnlockSkill(ref defense3Unlocked, defense2Unlocked, 3);
    }

    // AGILITY

    public void UnlockAgility1()
    {
        UnlockSkill(ref agility1Unlocked, true, 1);
    }

    public void UnlockAgility2()
    {
        UnlockSkill(ref agility2Unlocked, agility1Unlocked, 2);
    }

    public void UnlockAgility3()
    {
        UnlockSkill(ref agility3Unlocked, agility2Unlocked, 3);
    }

    // ─────────────────────────────
    // CORE UNLOCK LOGIC
    // ─────────────────────────────

    void UnlockSkill(
        ref bool skillUnlocked,
        bool requirementMet,
        int requiredPoints)
    {
        if (skillUnlocked)
        {
            Debug.Log("Skill already unlocked");
            return;
        }

        if (!requirementMet)
        {
            Debug.Log("Previous skill required");
            return;
        }

        if (skillPoints < requiredPoints)
        {
            Debug.Log(
                "Need " +
                requiredPoints +
                " skill points"
            );

            return;
        }

        skillPoints -= requiredPoints;

        if (PlayerStatsManager.Instance != null)
            PlayerStatsManager.Instance.RefreshSkillBonuses();

        skillUnlocked = true;

        Debug.Log("Skill unlocked!");

        RefreshUI();
    }

    // ─────────────────────────────
    // UI
    // ─────────────────────────────

    void RefreshUI()
    {
        if (skillPointText != null)
            skillPointText.text =
                "Skill Points: " + skillPoints;

        // Yield
        SetButton(yield1Button, yield1Unlocked, true, 1);
        SetButton(yield2Button, yield2Unlocked, yield1Unlocked, 2);
        SetButton(yield3Button, yield3Unlocked, yield2Unlocked, 3);

        // Water
        SetButton(water1Button, water1Unlocked, true, 1);
        SetButton(water2Button, water2Unlocked, water1Unlocked, 2);
        SetButton(water3Button, water3Unlocked, water2Unlocked, 3);

        // Growth
        SetButton(growth1Button, growth1Unlocked, true, 1);
        SetButton(growth2Button, growth2Unlocked, growth1Unlocked, 2);
        SetButton(growth3Button, growth3Unlocked, growth2Unlocked, 3);

        // Sword
        SetButton(sword1Button, sword1Unlocked, true, 1);
        SetButton(sword2Button, sword2Unlocked, sword1Unlocked, 2);
        SetButton(sword3Button, sword3Unlocked, sword2Unlocked, 3);

        // Defense
        SetButton(defense1Button, defense1Unlocked, true, 1);
        SetButton(defense2Button, defense2Unlocked, defense1Unlocked, 2);
        SetButton(defense3Button, defense3Unlocked, defense2Unlocked, 3);

        // Agility
        SetButton(agility1Button, agility1Unlocked, true, 1);
        SetButton(agility2Button, agility2Unlocked, agility1Unlocked, 2);
        SetButton(agility3Button, agility3Unlocked, agility2Unlocked, 3);

        // Lines
        SetLine(yieldLine1, yield1Unlocked);
        SetLine(yieldLine2, yield2Unlocked);

        SetLine(waterLine1, water1Unlocked);
        SetLine(waterLine2, water2Unlocked);

        SetLine(growthLine1, growth1Unlocked);
        SetLine(growthLine2, growth2Unlocked);

        SetLine(swordLine1, sword1Unlocked);
        SetLine(swordLine2, sword2Unlocked);

        SetLine(defenseLine1, defense1Unlocked);
        SetLine(defenseLine2, defense2Unlocked);

        SetLine(agilityLine1, agility1Unlocked);
        SetLine(agilityLine2, agility2Unlocked);
    }

    void SetButton(
        Button button,
        bool unlocked,
        bool requirementMet,
        int cost)
    {
        if (button == null) return;

        bool enoughPoints = skillPoints >= cost;

        button.interactable =
            !unlocked &&
            requirementMet &&
            enoughPoints;

        Image img = button.GetComponent<Image>();

        if (img != null)
        {
            if (unlocked)
                img.color = Color.green;

            else if (!requirementMet)
                img.color = Color.gray;

            else if (!enoughPoints)
                img.color = new Color(0.5f, 0.5f, 0.5f);

            else
                img.color = Color.white;
        }
    }

    void SetLine(Image line, bool active)
    {
        if (line == null) return;

        line.color =
            active
            ? Color.green
            : Color.gray;
    }

    // ─────────────────────────────
    // SAVE / LOAD
    // ─────────────────────────────

    public void SaveToData(SaveData data)
    {
        data.skillPoints = skillPoints;

        data.yield1Unlocked = yield1Unlocked;
        data.yield2Unlocked = yield2Unlocked;
        data.yield3Unlocked = yield3Unlocked;

        data.water1Unlocked = water1Unlocked;
        data.water2Unlocked = water2Unlocked;
        data.water3Unlocked = water3Unlocked;

        data.growth1Unlocked = growth1Unlocked;
        data.growth2Unlocked = growth2Unlocked;
        data.growth3Unlocked = growth3Unlocked;

        data.sword1Unlocked = sword1Unlocked;
        data.sword2Unlocked = sword2Unlocked;
        data.sword3Unlocked = sword3Unlocked;

        data.defense1Unlocked = defense1Unlocked;
        data.defense2Unlocked = defense2Unlocked;
        data.defense3Unlocked = defense3Unlocked;

        data.agility1Unlocked = agility1Unlocked;
        data.agility2Unlocked = agility2Unlocked;
        data.agility3Unlocked = agility3Unlocked;
    }

    public void LoadFromData(SaveData data)
    {
        skillPoints = data.skillPoints;

        yield1Unlocked = data.yield1Unlocked;
        yield2Unlocked = data.yield2Unlocked;
        yield3Unlocked = data.yield3Unlocked;

        water1Unlocked = data.water1Unlocked;
        water2Unlocked = data.water2Unlocked;
        water3Unlocked = data.water3Unlocked;

        growth1Unlocked = data.growth1Unlocked;
        growth2Unlocked = data.growth2Unlocked;
        growth3Unlocked = data.growth3Unlocked;

        sword1Unlocked = data.sword1Unlocked;
        sword2Unlocked = data.sword2Unlocked;
        sword3Unlocked = data.sword3Unlocked;

        defense1Unlocked = data.defense1Unlocked;
        defense2Unlocked = data.defense2Unlocked;
        defense3Unlocked = data.defense3Unlocked;

        agility1Unlocked = data.agility1Unlocked;
        agility2Unlocked = data.agility2Unlocked;
        agility3Unlocked = data.agility3Unlocked;

        RefreshUI();
    }
}