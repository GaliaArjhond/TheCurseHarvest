using UnityEngine;

public class ChestUIManager : MonoBehaviour
{
    public static ChestUIManager Instance;

    [SerializeField] private GameObject chestPanel;

    void Awake()
    {
        Instance = this;

        if (chestPanel != null)
            chestPanel.SetActive(false);
    }

    public void ToggleChest()
    {
        if (chestPanel == null) return;

        chestPanel.SetActive(!chestPanel.activeSelf);
    }

    public void CloseChest()
    {
        if (chestPanel == null) return;

        chestPanel.SetActive(false);
    }

    public bool IsChestOpen()
    {
        return chestPanel.activeSelf;
    }
}