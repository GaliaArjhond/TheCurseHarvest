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

    private List<ShopItemData> selectedStoreItems = new List<ShopItemData>();
    private List<InventorySaveData> selectedPlayerItems = new List<InventorySaveData>();

    private List<ShopSlotUI> selectedStoreSlots = new List<ShopSlotUI>();
    private List<ShopSlotUI> selectedPlayerSlots = new List<ShopSlotUI>();

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();
        inventory = InventoryController.Instance;
    }

    void OnEnable()
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

            Sprite sprite = GetItemSprite(prefab);
            if (sprite == null) continue;

            GameObject slot = Instantiate(slotPrefab, storeGrid);
            ShopSlotUI ui = slot.GetComponent<ShopSlotUI>();

            ui.SetupStore(sprite, item, this);
        }
    }

    void GeneratePlayerInventory()
    {
        ClearGrid(playerGrid);

        if (inventory == null)
            inventory = InventoryController.Instance;

        List<InventorySaveData> items = inventory.GetBackpackItemsForShop();

        foreach (InventorySaveData data in items)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(data.itemID);
            if (prefab == null) continue;

            Sprite sprite = GetItemSprite(prefab);
            if (sprite == null) continue;

            GameObject slot = Instantiate(slotPrefab, playerGrid);
            ShopSlotUI ui = slot.GetComponent<ShopSlotUI>();

            ui.SetupPlayer(sprite, data, this);
        }
    }

    Sprite GetItemSprite(GameObject prefab)
    {
        Image image = prefab.GetComponent<Image>();
        if (image != null) return image.sprite;

        SpriteRenderer spriteRenderer = prefab.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null) return spriteRenderer.sprite;

        return null;
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
        if (selectedStoreItems.Count == 0)
        {
            Debug.Log("No store items selected");
            return;
        }

        int totalCost = 0;

        foreach (ShopItemData item in selectedStoreItems)
            totalCost += item.price;

        if (!MoneyManager.Instance.SpendMoney(totalCost))
        {
            Debug.Log("Not enough money");
            return;
        }

        foreach (ShopItemData item in selectedStoreItems)
            inventory.AddItem(item.itemID, 1);

        selectedStoreItems.Clear();
        selectedStoreSlots.Clear();

        RefreshShop();

        Debug.Log("Bought multiple items for ₱" + totalCost);
    }

    public void SellSelected()
    {
        if (selectedPlayerItems.Count == 0)
        {
            Debug.Log("No player items selected");
            return;
        }

        int totalSellPrice = 0;

        foreach (InventorySaveData selected in selectedPlayerItems)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(selected.itemID);
            if (prefab == null) continue;

            Item item = prefab.GetComponent<Item>();
            if (item == null || item.sellPrice <= 0) continue;

            bool removed = inventory.RemoveItem(selected.itemID, 1);

            if (removed)
                totalSellPrice += item.sellPrice;
        }

        if (totalSellPrice > 0)
            MoneyManager.Instance.AddMoney(totalSellPrice);

        selectedPlayerItems.Clear();
        selectedPlayerSlots.Clear();

        RefreshShop();

        Debug.Log("Sold multiple items for ₱" + totalSellPrice);
    }
    
    public void ToggleStoreSelection(ShopItemData item, ShopSlotUI slot)
    {
        if (selectedStoreItems.Contains(item))
        {
            selectedStoreItems.Remove(item);
            selectedStoreSlots.Remove(slot);
            slot.SetSelected(false);
        }
        else
        {
            selectedStoreItems.Add(item);
            selectedStoreSlots.Add(slot);
            slot.SetSelected(true);
        }
    }

    public void TogglePlayerSelection(InventorySaveData item, ShopSlotUI slot)
    {
        if (selectedPlayerItems.Contains(item))
        {
            selectedPlayerItems.Remove(item);
            selectedPlayerSlots.Remove(slot);
            slot.SetSelected(false);
        }
        else
        {
            selectedPlayerItems.Add(item);
            selectedPlayerSlots.Add(slot);
            slot.SetSelected(true);
        }
    }
}