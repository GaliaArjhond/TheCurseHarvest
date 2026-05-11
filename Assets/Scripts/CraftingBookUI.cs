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
        foreach (Transform child in recipeContent)
            Destroy(child.gameObject);

        foreach (CraftRecipeData recipe in recipes)
        {
            GameObject entry = Instantiate(recipeEntryPrefab, recipeContent);
            RecipeEntryUI ui = entry.GetComponent<RecipeEntryUI>();
            ui.Setup(recipe, craftingManager);
        }
    }
}