using UnityEngine;

public class CaveReturnLadder : MonoBehaviour
{
    [SerializeField] private float delay = 0.2f;
    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (used) return;

        used = true;

        if (CaveManager.Instance != null)
            CaveManager.Instance.ReturnToCaveStart(other.transform);

        Invoke(nameof(ResetUse), delay);
    }

    void ResetUse()
    {
        used = false;
    }
}