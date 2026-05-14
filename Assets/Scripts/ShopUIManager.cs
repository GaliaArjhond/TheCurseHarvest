using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform storeGrid;
    [SerializeField] private Transform playerGrid;
    [SerializeField] private GameObject slotPrefab;

    [Header("Store Items")]
    [SerializeField] private List<ShopItemData> storeItems;

    private ItemDictionary itemDictionary;
    private InventoryController inventory;

    private ShopItemData selectedStoreItem;
    private InventorySaveData selectedPlayerItem;

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        inventory = InventoryController.Instance;

        RefreshShop();
    }

    public void RefreshShop()
    {
        GenerateStore();
        GeneratePlayerInventory();
    }

    void GenerateStore()
    {
        ClearGrid(storeGrid);

        foreach (ShopItemData item in storeItems)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(item.itemID);
            if (prefab == null) continue;

            Image img = prefab.GetComponent<Image>();
            if (img == null) continue;

            GameObject slot = Instantiate(slotPrefab, storeGrid);
            ShopSlotUI ui = slot.GetComponent<ShopSlotUI>();

            ui.SetupStore(img.sprite, item, this);
        }
    }

    void GeneratePlayerInventory()
    {
        ClearGrid(playerGrid);

        if (inventory == null)
            inventory = InventoryController.Instance;

        List<InventorySaveData> items = inventory.GetInventoryItems();

        foreach (InventorySaveData data in items)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(data.itemID);
            if (prefab == null) continue;

            Image img = prefab.GetComponent<Image>();
            if (img == null) continue;

            GameObject slot = Instantiate(slotPrefab, playerGrid);
            ShopSlotUI ui = slot.GetComponent<ShopSlotUI>();

            ui.SetupPlayer(img.sprite, data, this);
        }
    }

    void ClearGrid(Transform grid)
    {
        foreach (Transform child in grid)
            Destroy(child.gameObject);
    }

    public void SelectStoreItem(ShopItemData item)
    {
        selectedStoreItem = item;
        selectedPlayerItem = null;

        Debug.Log("Selected store item ID: " + item.itemID);
    }

    public void SelectPlayerItem(InventorySaveData item)
    {
        selectedPlayerItem = item;
        selectedStoreItem = null;

        Debug.Log("Selected player item ID: " + item.itemID);
    }

    public void BuySelected()
    {
        if (selectedStoreItem == null)
        {
            Debug.Log("No store item selected");
            return;
        }

        if (MoneyManager.Instance.SpendMoney(selectedStoreItem.price))
        {
            inventory.AddItem(selectedStoreItem.itemID, 1);
            RefreshShop();
            Debug.Log("Bought item");
        }
    }

    public void SellSelected()
    {
        if (selectedPlayerItem == null)
        {
            Debug.Log("No player item selected");
            return;
        }

        GameObject prefab = itemDictionary.GetItemPrefab(selectedPlayerItem.itemID);
        if (prefab == null) return;

        Item item = prefab.GetComponent<Item>();
        if (item == null || item.sellPrice <= 0)
        {
            Debug.Log("Item cannot be sold");
            return;
        }

        bool removed = inventory.RemoveItem(selectedPlayerItem.itemID, 1);

        if (removed)
        {
            MoneyManager.Instance.AddMoney(item.sellPrice);
            RefreshShop();
            Debug.Log("Sold item");
        }
    }
}