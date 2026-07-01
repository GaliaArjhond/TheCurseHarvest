using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CraftIngredient
{
    public int itemID;
    public int amount = 1;
}

[CreateAssetMenu(fileName = "CraftRecipeData", menuName = "Crafting/Recipe")]
public class CraftRecipeData : ScriptableObject
{
    public string recipeName;

    [Header("Visual")]
    public Sprite recipeIcon;

    [Header("Requirements")]
    public List<CraftIngredient> ingredients = new List<CraftIngredient>();

    [Header("Legacy Requirements")]
    [HideInInspector]
    public int woodAmount;
    [HideInInspector]
    public int stoneAmount;

    [Header("Result")]
    public GameObject resultPrefab;
    public int resultAmount = 1;

    public IEnumerable<CraftIngredient> GetIngredients()
    {
        if (ingredients != null && ingredients.Count > 0)
            return ingredients;

        return new[] {
            new CraftIngredient { itemID = 2, amount = woodAmount },
            new CraftIngredient { itemID = 9, amount = stoneAmount }
        };
    }

    public string GetRequirementsText()
    {
        List<string> lines = new List<string>();

        foreach (CraftIngredient ingredient in GetIngredients())
        {
            if (ingredient.amount <= 0)
                continue;

            lines.Add(GetIngredientText(ingredient));
        }

        if (lines.Count == 0)
            return "No ingredients";

        return string.Join("\n", lines);
    }

    string GetIngredientText(CraftIngredient ingredient)
    {
        string name = GetItemName(ingredient.itemID);
        return ingredient.amount + " " + name;
    }

    string GetItemName(int itemID)
    {
        if (ItemDictionary.Instance != null)
        {
            GameObject prefab = ItemDictionary.Instance.GetItemPrefab(itemID);
            if (prefab != null)
            {
                Item item = prefab.GetComponent<Item>();
                if (item != null && !string.IsNullOrEmpty(item.Name))
                    return item.Name;
            }
        }

        return "Item " + itemID;
    }
}