using TMPro;
using UnityEngine;

public class DialogueUI : MonoBehaviour
{
    public static DialogueUI Instance;

    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI nameText;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    void Awake()
    {
        Instance = this;

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

        nameText.text = "Talk to " + npcName;

        dialogueText.text = message;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);
    }
}