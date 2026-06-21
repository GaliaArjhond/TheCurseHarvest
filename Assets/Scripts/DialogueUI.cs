using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;
    public bool IsDialogueOpen { get; private set; }

    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    void Awake()
    {
        Instance = this;
        IsDialogueOpen = false;
        dialoguePanel.SetActive(false);
    }

    public void AcceptQuest()
    {
        QuestManager.Instance.AcceptQuest1();

        HideDialogue();
    }

    public void DeclineQuest()
    {
        HideDialogue();
    }

    public void ShowDialogue(
        string npcName,
        string message)
    {
        dialoguePanel.SetActive(true);
        IsDialogueOpen = true;

        if (PauseManager.Instance != null)
        {
            PauseManager.Instance.SetPaused(true);
        }

        nameText.text = "Talk to " + npcName;
        dialogueText.text = message;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueOpen = false;

        if (PauseManager.Instance != null &&
            PauseManager.Instance.IsPaused)
        {
            PauseManager.Instance.SetPaused(false);
        }
    }
}