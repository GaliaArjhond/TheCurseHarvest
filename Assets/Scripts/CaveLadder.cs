using UnityEngine;

public class CaveLadder : MonoBehaviour
{
    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (used)
            return;

        used = true;

        if (CaveManager.Instance != null)
        {
            CaveManager.Instance.GoNextLevel();
        }
    }
}