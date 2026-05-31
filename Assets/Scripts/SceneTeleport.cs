using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTeleport : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string sceneToLoad;

    [Header("Spawn")]
    [SerializeField] private string destinationSpawnName;

    private bool isTeleporting = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        if (isTeleporting) return;

        isTeleporting = true;

        SpawnManager.spawnPointName = destinationSpawnName;

        SceneManager.LoadScene(sceneToLoad);
    }
}