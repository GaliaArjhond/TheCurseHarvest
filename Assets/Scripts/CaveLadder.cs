using UnityEngine;

public class CaveLadder : MonoBehaviour
{
    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;
        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Show("[E] Climb Ladder");

        if (used)
            return;

        used = true;

        if (CaveManager.Instance != null)
        {
            CaveManager.Instance.GoNextLevel();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (InteractionUI.Instance != null)
            InteractionUI.Instance.Hide();
    }
}