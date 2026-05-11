using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    private bool playerInRange;

    void Update()
    {
        if (!playerInRange) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
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
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (ChestUIManager.Instance != null)
                ChestUIManager.Instance.CloseChest();
        }
    }
}