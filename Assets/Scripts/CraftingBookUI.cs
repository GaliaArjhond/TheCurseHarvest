using UnityEngine;
using UnityEngine.InputSystem;

public class CraftingBookUI : MonoBehaviour
{
    [SerializeField] private CraftingInventoryPreview inventoryPreview;
    public GameObject bookPanel;
    public Transform recipeContent;
    public GameObject recipeEntryPrefab;
    public CraftRecipeData[] recipes;
    public CraftingManager craftingManager;

    void Awake()
    {
        if (bookPanel == null)
            bookPanel = gameObject;

        bookPanel.SetActive(false);
    }

    void Start()
    {
        if (craftingManager == null)
            craftingManager = FindFirstObjectByType<CraftingManager>();

        BuildRecipeList();
    }

    void Update()
    {
        if (Keyboard.current.bKey.wasPressedThisFrame)
            ToggleBook();
    }

    void LateUpdate()
    {
        if (bookPanel != null &&
            bookPanel.activeSelf &&
            inventoryPreview != null)
        {
            inventoryPreview.RefreshPreview();
        }
    }

    public void ToggleBook()
    {
        bookPanel.SetActive(!bookPanel.activeSelf);

        if (bookPanel.activeSelf && inventoryPreview != null)
        {
            Debug.Log("Refreshing crafting inventory preview");
            inventoryPreview.RefreshPreview();
        }
    }

    public void CloseBook()
    {
        bookPanel.SetActive(false);
    }

    public bool IsOpen()
    {
        return bookPanel.activeSelf;
    }

    void BuildRecipeList()
    {
        if (recipeContent == null)
        {
            Debug.LogError("CraftingBookUI: recipeContent is not assigned.");
            return;
        }

        if (recipeEntryPrefab == null)
        {
            Debug.LogError("CraftingBookUI: recipeEntryPrefab is not assigned.");
            return;
        }

        foreach (Transform child in recipeContent)
            Destroy(child.gameObject);

        if (recipes == null || recipes.Length == 0)
        {
            Debug.LogWarning("CraftingBookUI: No recipes assigned to the crafting book.");
            return;
        }

        int createdCount = 0;

        foreach (CraftRecipeData recipe in recipes)
        {
            if (recipe == null)
                continue;

            GameObject entry = Instantiate(recipeEntryPrefab, recipeContent, false);
            entry.transform.localScale = Vector3.one;

            RecipeEntryUI ui = entry.GetComponent<RecipeEntryUI>();
            if (ui != null)
                ui.Setup(recipe, craftingManager);
            else
                Debug.LogWarning("CraftingBookUI: Recipe entry prefab does not contain RecipeEntryUI.");

            createdCount++;
        }

        Debug.Log($"CraftingBookUI: Built {createdCount} recipe entries.");

        RectTransform contentRect = recipeContent as RectTransform;
        if (contentRect != null)
            UnityEngine.UI.LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
    }
}