using UnityEngine;
using UnityEngine.InputSystem;

public class ShopInteract : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;

    private bool playerInRange;

    void Start()
    {
        if (shopPanel != null)
            shopPanel.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            shopPanel.SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Show("[E] Open Shop");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (shopPanel != null)
                shopPanel.SetActive(false);

            if (InteractionUI.Instance != null)
                InteractionUI.Instance.Hide();
        }
    }
}