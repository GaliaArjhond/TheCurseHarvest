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

        if (
            QuestManager.Instance.quest1Accepted &&
            !QuestManager.Instance.quest1Completed
        )
        {
            bool questReady =
                QuestManager.Instance.woodCollected >= 10;

            if (questReady)
            {
                questTitle.text =
                    "QUEST COMPLETE";

                questDescription.text =
                    "Talk to Maria";

                questProgress.text =
                    "10 / 10 Wood";

                rewardText.text =
                    "+100 Gold\n+25 EXP";
            }
            else
            {
                questTitle.text =
                    "A Farmer's Beginning";

                questDescription.text =
                    "Collect 10 Wood";

                questProgress.text =
                    QuestManager.Instance.woodCollected
                    + " / 10 Wood";

                rewardText.text =
                    "100 Gold\n25 EXP";
            }

            if (questSlider != null)
            {
                questSlider.maxValue = 10;
                questSlider.value = QuestManager.Instance.woodCollected;
            }
        }

        // QUEST COMPLETE

        else if (
            QuestManager.Instance.quest1Completed
        )
        {
            questTitle.color = Color.yellow;
            questTitle.text = "QUEST COMPLETE!";
            questDescription.text = "Talk to Maria";
            questProgress.text = "";
            rewardText.text =
                "+100 Gold\n+25 EXP";
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