using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (ChestUIManager.Instance != null)
            {
                if (ChestUIManager.Instance.IsChestOpen())
                    ChestUIManager.Instance.CloseChest();
                else
                    ChestUIManager.Instance.OpenChest();
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Show("[E] Open Chest");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (ChestUIManager.Instance != null)
                ChestUIManager.Instance.CloseChest();

            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Hide();
        }
    }
}