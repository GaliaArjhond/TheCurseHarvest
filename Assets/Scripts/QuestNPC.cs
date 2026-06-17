using UnityEngine;

public class QuestNPC : MonoBehaviour
{
    [SerializeField]
    private string npcName = "Maria";

    private bool playerNear;

    void Update()
    {
        if (playerNear &&
            Input.GetKeyDown(KeyCode.E))
        {
            Talk();
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

        // already completed
        if (QuestManager.Instance.quest1Completed)
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

        "Reward:\n" +

        "100 Gold\n" +

        "25 EXP"

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

            Debug.Log(
                "Press E to talk."
            );
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
}