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

    public void SellSelectedItem()
    {
        HotbarControler hotbar =
            FindFirstObjectByType<HotbarControler>();

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