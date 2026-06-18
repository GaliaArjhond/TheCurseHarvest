using UnityEngine;
using TMPro;

public class QuestNPC : MonoBehaviour
{
    [SerializeField]
    private string npcName = "Maria";

    private bool playerNear;

    [Header("Quest UI (TextMeshPro)")]
    [SerializeField] private TMP_Text questTitleTMP;
    [SerializeField] private TMP_Text questDescriptionTMP;
    [SerializeField] private TMP_Text questProgressTMP;

    private void SetTitle(string s)
    {
        if (questTitleTMP != null) questTitleTMP.text = s;
    }

    private void SetDescription(string s)
    {
        if (questDescriptionTMP != null) questDescriptionTMP.text = s;
    }

    private void SetProgress(string s)
    {
        if (questProgressTMP != null) questProgressTMP.text = s;
    }

    void Update()
    {
        if (playerNear &&
            Input.GetKeyDown(KeyCode.E))
        {
            Talk();
        }
    }

    void UpdateQuestUI()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogWarning("QuestManager not found.");
            return;
        }
        switch (QuestManager.Instance.currentQuest)
        {
            case 0:
                SetTitle("A Farmer's Beginning");
                SetDescription("Talk to Maria");
                SetProgress("");
                break;
            case 1:
                SetTitle("A Farmer's Beginning");
                SetDescription("Collect 10 Wood");
                SetProgress(QuestManager.Instance.woodCollected + " / 10");
                break;
            case 2:
                SetTitle("Stone For Repairs");
                SetDescription("Collect 5 Stone");
                SetProgress(QuestManager.Instance.stoneCollected + " / 5");
                break;
            case 3:
                SetTitle("New Beginnings");
                SetDescription("Plant 5 Carrots");
                SetProgress(QuestManager.Instance.carrotsPlanted + " / 5");
                break;
            case 4:
                SetTitle("Harvest Time");
                SetDescription("Harvest 5 Carrots");
                SetProgress(QuestManager.Instance.carrotsHarvested + " / 5");
                break;
            case 5:
                SetTitle("Protect The Farm");
                    SetDescription("Defeat 3 Halimaws");
                    SetProgress(QuestManager.Instance.halimawsKilled + " / 3");
                break;
            case 6:
                SetTitle("QUEST COMPLETE");
                SetDescription("All beginner quests finished.");
                SetProgress("");
                break;
        }
    }

    void Talk()
    {
        if (QuestManager.Instance == null)
        {
            Debug.LogWarning(
                "QuestManager not found."
            );
            return;
        }

        if (DialogueUI.Instance == null)
        {
            Debug.LogWarning(
                "DialogueUI not found."
            );
            return;
        }

        // already completed (quest 1 finished -> currentQuest > 1)
        if (QuestManager.Instance.currentQuest > 1)
        {
            Debug.Log(
                npcName +
                ": Thank you for helping!"
            );

            return;
        }

        // first talk
        DialogueUI.Instance.ShowDialogue(

        npcName,

        "The kubo needs repairs.\n\n" +

        "Can you collect 10 Wood for me?\n\n" +

        "Rewards:\n" +

        "• 100 Pesos\n" +

        "• 25 EXP"

        );

        Debug.Log(
            npcName +
            ": Progress "
            + QuestManager.Instance.woodCollected
            + "/10"
        );
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = true;
            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Show("[E] Talk to Maria");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Hide();
        }
    }

    void Start()
    {
        UpdateQuestUI();
    }
}