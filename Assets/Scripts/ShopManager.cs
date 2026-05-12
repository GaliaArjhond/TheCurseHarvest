using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [Header("Wheat Seed")]
    public int seedItemID = 7;
    public int seedPrice = 10;
    public int seedAmount = 1;

    [Header("Carrot Seed")]
    public int carrotSeedItemID = 8;
    public int carrotSeedPrice = 15;
    public int carrotSeedAmount = 1;

    private InventoryController inventory;

    void Start()
    {
        inventory = InventoryController.Instance;
    }

    public void BuySeed()
    {
        BuyItem(seedItemID, seedPrice, seedAmount, "Wheat Seed");
    }

    public void BuyCarrotSeed()
    {
        BuyItem(carrotSeedItemID, carrotSeedPrice, carrotSeedAmount, "Carrot Seed");
    }

    void BuyItem(int itemID, int price, int amount, string itemName)
    {
        if (inventory == null)
            inventory = InventoryController.Instance;

        if (MoneyManager.Instance == null || inventory == null)
        {
            Debug.LogError("Missing MoneyManager or InventoryController");
            return;
        }

        if (MoneyManager.Instance.SpendMoney(price))
        {
            bool added = inventory.AddItem(itemID, amount);

            if (added)
                Debug.Log("Bought " + itemName);
            else
                Debug.LogError("Bought " + itemName + " BUT inventory is full or AddItem failed");
        }
    }

    public void SellSelectedItem()
    {
        HotbarControler hotbar = FindFirstObjectByType<HotbarControler>();
        if (hotbar == null) return;

        Item item = hotbar.GetSelectedItem();

        if (item == null)
        {
            Debug.Log("No item selected");
            return;
        }

        int value = item.sellPrice;

        if (value <= 0)
        {
            Debug.Log("Item cannot be sold");
            return;
        }

        MoneyManager.Instance.AddMoney(value);

        item.amount--;
        item.UpdateAmountText();

        if (item.amount <= 0)
        {
            Slot slot = item.transform.parent.GetComponent<Slot>();

            if (slot != null)
                slot.currentItem = null;

            Destroy(item.gameObject);
        }

        Debug.Log("Sold " + item.Name);
    }
}