using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    private InventoryController inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventoryController>();
    }

    public void Craft(CraftRecipeData recipe)
    {
        if (inventory == null || recipe == null) return;

        if (inventory.CountItem(2) < recipe.woodAmount ||
            inventory.CountItem(9) < recipe.stoneAmount)
        {
            Debug.Log("Not enough materials");
            return;
        }

        inventory.RemoveItem(2, recipe.woodAmount);
        inventory.RemoveItem(9, recipe.stoneAmount);

        Item resultItem = recipe.resultPrefab.GetComponent<Item>();
        if (resultItem != null)
            inventory.AddItem(resultItem.ID, recipe.resultAmount);

        Debug.Log("Crafted: " + recipe.recipeName);
    }
}