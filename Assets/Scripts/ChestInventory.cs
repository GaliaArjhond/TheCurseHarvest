using UnityEngine;
using System.Collections.Generic;

public class ChestInventory : MonoBehaviour
{
    [Header("Chest")]
    public Transform chestSlotGrid;
    public GameObject slotPrefab;
    public int slotCount = 24;

    private ItemDictionary itemDictionary;

    void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>(FindObjectsInactive.Include);
    }

    void Start()
    {
        CreateSlotsIfMissing();
    }

    void CreateSlotsIfMissing()
    {
        if (chestSlotGrid == null)
        {
            Debug.LogError("ChestInventory: Chest Slot Grid is not assigned!");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("ChestInventory: Slot Prefab is not assigned!");
            return;
        }

        if (chestSlotGrid.childCount > 0)
            return;

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, chestSlotGrid);
        }
    }

    public List<InventorySaveData> GetChestItems()
    {
        List<InventorySaveData> data = new List<InventorySaveData>();

        if (chestSlotGrid == null)
            return data;

        foreach (Transform slotTransform in chestSlotGrid)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null || slot.currentItem == null)
                continue;

            Item item = slot.currentItem.GetComponent<Item>();

            if (item == null)
                continue;

            data.Add(new InventorySaveData
            {
                itemID = item.ID,
                slotIndex = slotTransform.GetSiblingIndex(),
                amount = item.amount
            });
        }

        return data;
    }

    public void SetChestItems(List<InventorySaveData> data)
    {
        if (itemDictionary == null)
            itemDictionary = FindFirstObjectByType<ItemDictionary>(FindObjectsInactive.Include);

        if (itemDictionary == null)
        {
            Debug.LogError("ChestInventory: ItemDictionary not found!");
            return;
        }

        CreateSlotsIfMissing();

        if (chestSlotGrid == null)
        {
            Debug.LogError("ChestInventory: Chest Slot Grid is not assigned!");
            return;
        }

        foreach (Transform slotTransform in chestSlotGrid)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }

        if (data == null)
            return;

        foreach (InventorySaveData saveItem in data)
        {
            if (saveItem.slotIndex < 0 || saveItem.slotIndex >= chestSlotGrid.childCount)
                continue;

            Slot slot = chestSlotGrid.GetChild(saveItem.slotIndex).GetComponent<Slot>();

            if (slot == null)
                continue;

            GameObject prefab = itemDictionary.GetItemPrefab(saveItem.itemID);

            if (prefab == null)
            {
                Debug.LogWarning("ChestInventory: Missing item prefab ID " + saveItem.itemID);
                continue;
            }

            GameObject itemObj = Instantiate(prefab, slot.transform);

            RectTransform rt = itemObj.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            Item item = itemObj.GetComponent<Item>();

            if (item != null)
            {
                item.amount = Mathf.Max(1, saveItem.amount);
                item.UpdateAmountText();
            }

            slot.currentItem = itemObj;
        }
    }
}