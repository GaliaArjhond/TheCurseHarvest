using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MonsterPageUI : MonoBehaviour
{
    public static MonsterPageUI Instance;

    [Header("Info Panel")]
    public Image monsterIcon;
    public TMP_Text monsterName;
    public TMP_Text loreText;
    public TMP_Text weaknessText;

    private MonsterButton[] monsterButtons;

    private void Awake()
    {
        Instance = this;
        monsterButtons = GetComponentsInChildren<MonsterButton>(true);
    }

    private void OnEnable()
    {
        if (monsterButtons == null)
            monsterButtons = GetComponentsInChildren<MonsterButton>(true);

        foreach (MonsterButton button in monsterButtons)
        {
            if (button != null)
                button.RefreshButton();
        }
    }

    public void ShowMonster(HalimawEntry entry)
    {
        if (entry == null)
            return;

        bool unlocked = HalimawLogManager.Instance != null && HalimawLogManager.Instance.IsUnlocked(entry);

        if (unlocked)
        {
            if (monsterIcon != null)
                monsterIcon.sprite = entry.icon;

            if (monsterName != null)
                monsterName.text = entry.monsterName;

            if (loreText != null)
                loreText.text = entry.lore;

            if (weaknessText != null)
                weaknessText.text = entry.weakness;
        }
        else
        {
            if (monsterIcon != null)
                monsterIcon.sprite = null;

            if (monsterName != null)
                monsterName.text = "???";

            if (loreText != null)
                loreText.text = "You haven't encountered this creature yet.";

            if (weaknessText != null)
                weaknessText.text = "Unknown";
        }
    }
}