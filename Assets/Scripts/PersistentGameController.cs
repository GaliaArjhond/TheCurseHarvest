using UnityEngine;

public class PersistentGameController : MonoBehaviour
{
    public static PersistentGameController Instance;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}