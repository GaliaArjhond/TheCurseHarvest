using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Shop Items")]
    public int seedItemID = 7;
    public int seedPrice = 10;
    public int seedAmount = 1;

    private InventoryController inventory;

    void Start()
    {
        inventory = InventoryController.Instance;
    }

    public void BuySeed()
    {
        if (inventory == null)
            inventory = InventoryController.Instance;

        if (MoneyManager.Instance == null || inventory == null)
        {
            Debug.LogError("Missing MoneyManager or InventoryController");
            return;
        }

        if (MoneyManager.Instance.SpendMoney(seedPrice))
        {
            bool added = inventory.AddItem(seedItemID, seedAmount);

            if (added)
                Debug.Log("Bought seed and added to inventory");
            else
                Debug.LogError("Bought seed BUT inventory is full or AddItem failed");
        }
    }
}