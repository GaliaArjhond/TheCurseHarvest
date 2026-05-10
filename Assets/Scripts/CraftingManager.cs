using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public CraftRecipe chestRecipe;

    private InventoryController inventory;

    void Start()
    {
        inventory = FindFirstObjectByType<InventoryController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            CraftChest();
        }
    }

    public void CraftChest()
    {
        if (inventory == null || chestRecipe == null)
            return;

        bool hasWood =
            inventory.CountItem(
                chestRecipe.requiredItem1ID
            ) >= chestRecipe.requiredItem1Amount;

        bool hasStone =
            inventory.CountItem(
                chestRecipe.requiredItem2ID
            ) >= chestRecipe.requiredItem2Amount;

        if (!hasWood || !hasStone)
        {
            Debug.Log("Not enough materials");
            return;
        }

        inventory.RemoveItem(
            chestRecipe.requiredItem1ID,
            chestRecipe.requiredItem1Amount
        );

        inventory.RemoveItem(
            chestRecipe.requiredItem2ID,
            chestRecipe.requiredItem2Amount
        );

        Item resultItem =
            chestRecipe.resultPrefab.GetComponent<Item>();

        if (resultItem == null)
        {
            Debug.LogError("Result prefab has no Item component!");
            return;
        }

        Debug.Log("Craft result ID: " + resultItem.ID);

        if (resultItem != null)
        {
            inventory.AddItem(
                resultItem.ID,
                chestRecipe.resultAmount
            );
        }

        Debug.Log("Crafted Chest!");
    }
}