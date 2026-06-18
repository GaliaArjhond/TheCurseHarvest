using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour
{
    [SerializeField]
    private GameObject questContent;

    [SerializeField]
    private TextMeshProUGUI minimizeText;

    [SerializeField]
    private RectTransform questPanel;

    [SerializeField]        
    private RectTransform questContentRect;

    [SerializeField]
    private float expandedHeight = 180f;

    [SerializeField]
    private float minimizedHeight = 40f;

    private bool isMinimized = false;

    [SerializeField]
    private Slider questSlider;
    public static QuestUI Instance;

    [SerializeField]
    private TextMeshProUGUI questTitle;

    [SerializeField]
    private TextMeshProUGUI questDescription;

    [SerializeField]
    private TextMeshProUGUI questProgress;

    [SerializeField]
    private TextMeshProUGUI rewardText;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        UpdateQuestUI();
    }

    void UpdateQuestUI()
    {
        if (QuestManager.Instance == null)
            return;
        int q = QuestManager.Instance.currentQuest;

        // reset default color
        questTitle.color = Color.white;

        switch (q)
        {
            case 0:
                questTitle.text = "A Farmer's Beginning";
                questDescription.text = "Talk to Maria";
                questProgress.text = "";
                rewardText.text = "100 Pesos\n25 EXP";
                if (questSlider != null) questSlider.gameObject.SetActive(false);
                break;
            case 1:
                if (QuestManager.Instance.woodCollected >= 10)
                {
                    questTitle.text = "QUEST COMPLETE";
                    questDescription.text = "Talk to Maria";
                    questProgress.text = "10 / 10 Wood";
                    rewardText.text = "+100 Pesos\n+25 EXP";
                }
                else
                {
                    questTitle.text = "A Farmer's Beginning";
                    questDescription.text = "Collect 10 Wood";
                    questProgress.text = QuestManager.Instance.woodCollected + " / 10 Wood";
                    rewardText.text = "100 Pesos\n25 EXP";
                }

                if (questSlider != null)
                {
                    questSlider.gameObject.SetActive(true);
                    questSlider.maxValue = 10;
                    questSlider.value = QuestManager.Instance.woodCollected;
                }
                break;
            case 2:
                questTitle.text = "Stone For Repairs";
                questDescription.text = "Collect 5 Stone";
                questProgress.text = QuestManager.Instance.stoneCollected + " / 5 Stone";
                rewardText.text = "150 Pesos\n35 EXP";
                if (questSlider != null)
                {
                    questSlider.gameObject.SetActive(true);
                    questSlider.maxValue = 5;
                    questSlider.value = QuestManager.Instance.stoneCollected;
                }
                break;
            case 3:
                questTitle.text = "New Beginnings";
                questDescription.text = "Plant 5 Carrots";
                questProgress.text = QuestManager.Instance.carrotsPlanted + " / 5";
                rewardText.text = "200 Pesos\n50 EXP";
                if (questSlider != null)
                {
                    questSlider.gameObject.SetActive(true);
                    questSlider.maxValue = 5;
                    questSlider.value = QuestManager.Instance.carrotsPlanted;
                }
                break;
            case 4:
                questTitle.text = "Harvest Time";
                questDescription.text = "Harvest 5 Carrots";
                questProgress.text = QuestManager.Instance.carrotsHarvested + " / 5";
                rewardText.text = "250 Pesos\n60 EXP";
                if (questSlider != null)
                {
                    questSlider.gameObject.SetActive(true);
                    questSlider.maxValue = 5;
                    questSlider.value = QuestManager.Instance.carrotsHarvested;
                }
                break;
            case 5:
                questTitle.text = "Protect The Farm";
                questDescription.text = "Defeat 3 Halimaws";
                questProgress.text = QuestManager.Instance.halimawsKilled + " / 3";
                rewardText.text = "500 Pesos\n100 EXP";
                if (questSlider != null)
                {
                    questSlider.gameObject.SetActive(true);
                    questSlider.maxValue = 3;
                    questSlider.value = QuestManager.Instance.halimawsKilled;
                }
                break;
            case 6:
                questTitle.color = Color.yellow;
                questTitle.text = "QUEST COMPLETE!";
                questDescription.text = "All beginner quests finished.";
                questProgress.text = "";
                rewardText.text = "";
                if (questSlider != null) questSlider.gameObject.SetActive(false);
                break;
            default:
                if (questSlider != null) questSlider.gameObject.SetActive(false);
                break;
        }
    }

    public void ToggleQuest()
    {
        isMinimized = !isMinimized;

        questContent.gameObject.SetActive(
            !isMinimized
        );

        Vector2 size =
            questPanel.sizeDelta;

        size.y =
            isMinimized
            ? minimizedHeight
            : expandedHeight;

        questPanel.sizeDelta = size;

        if (minimizeText != null)
        {
            minimizeText.text =
                isMinimized
                ? "▼"
                : "▲";
        }
    }
}