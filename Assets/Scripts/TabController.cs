using UnityEngine;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [Header("Tabs")]
    public Image[] tabImages;
    public GameObject[] pages;

    [Header("Crafting")]
    [SerializeField] private CraftingInventoryPreview inventoryPreview;

    [Header("Menu Root")]
    [SerializeField] private GameObject menuRoot;

    void Start()
    {
        ActivateTab(0);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            ToggleMenu();
    }

    public void ActivateTab(int tabNo)
    {
        if (ChestUIManager.Instance != null)
            ChestUIManager.Instance.CloseChest();

        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
                pages[i].SetActive(false);

            if (i < tabImages.Length && tabImages[i] != null)
                tabImages[i].color = Color.grey;
        }

        if (tabNo >= 0 && tabNo < pages.Length)
        {
            if (pages[tabNo] != null)
                pages[tabNo].SetActive(true);

            if (tabNo < tabImages.Length && tabImages[tabNo] != null)
                tabImages[tabNo].color = Color.white;

            if (pages[tabNo] != null &&
                pages[tabNo].name.Contains("Crafting") &&
                inventoryPreview != null)
            {
                inventoryPreview.RefreshPreview();
            }
        }
    }

    public void ToggleMenu()
    {
        Debug.Log("TAB PRESSED");

        if (ChestUIManager.Instance != null &&
            ChestUIManager.Instance.IsChestOpen())
        {
            ChestUIManager.Instance.CloseMenuAndChest();
            return;
        }

        if (menuRoot == null) return;

        menuRoot.SetActive(!menuRoot.activeSelf);
    }
}