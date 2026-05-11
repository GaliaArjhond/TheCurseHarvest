using UnityEngine;

public class ChestUIManager : MonoBehaviour
{
    public static ChestUIManager Instance;

    [Header("UI")]
    public GameObject menu;
    public GameObject inventoryPage;
    public GameObject chestWindow;

    void Awake()
    {
        Instance = this;

        if (chestWindow != null)
            chestWindow.SetActive(false);
    }

    public void OpenChest()
    {
        Debug.Log("OPEN CHEST");

        if (menu != null)
            menu.SetActive(true);

        if (inventoryPage != null)
            inventoryPage.SetActive(true);

        if (chestWindow != null)
            chestWindow.SetActive(true);
        else
            Debug.LogError("Chest Window is NOT assigned!");
    }

    public void CloseChest()
    {
        Debug.Log("CLOSE CHEST: " + (chestWindow != null ? chestWindow.name : "NULL"));

        if (chestWindow != null)
            chestWindow.SetActive(false);
    }

    public void CloseMenuAndChest()
    {
        Debug.Log("CLOSE MENU AND CHEST");

        if (chestWindow != null)
            chestWindow.SetActive(false);
        else
            Debug.LogError("Chest Window is NOT assigned!");

        if (menu != null)
            menu.SetActive(false);
        else
            Debug.LogError("Menu is NOT assigned!");
    }

    public bool IsChestOpen()
    {
        return chestWindow != null && chestWindow.activeSelf;
    }
}