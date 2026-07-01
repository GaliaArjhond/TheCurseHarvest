using System.Collections.Generic;
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

        foreach (CraftIngredient ingredient in recipe.GetIngredients())
        {
            if (inventory.CountItem(ingredient.itemID) < ingredient.amount)
            {
                Debug.Log("Not enough materials for: " + ingredient.itemID);
                return;
            }
        }

        foreach (CraftIngredient ingredient in recipe.GetIngredients())
        {
            inventory.RemoveItem(ingredient.itemID, ingredient.amount);
        }

        Item resultItem = recipe.resultPrefab.GetComponent<Item>();
        if (resultItem != null)
            inventory.AddItem(resultItem.ID, recipe.resultAmount);

        Debug.Log("Crafted: " + recipe.recipeName);
    }
}