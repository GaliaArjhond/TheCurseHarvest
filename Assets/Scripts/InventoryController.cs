using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;

    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public GameObject[] itemPrefabs;

    void Start()
    {
        itemDictionary = FindObjectOfType<ItemDictionary>();
    }

    public List<InventorySaveData> GetInventorySaveDatas()
    {
        // fixed — was invData which was never declared
        List<InventorySaveData> saveDatas = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item != null)
                {
                    // fixed — was invData.Add, should be saveDatas.Add
                    saveDatas.Add(new InventorySaveData
                    {
                        itemID = item.ID,
                        slotIndex = slotTransform.GetSiblingIndex()
                    });
                }
            }
        }

        return saveDatas;
    }

    // fixed — was missing parameter name
    public void SetInventoryItems(List<InventorySaveData> inventorySaveData)
    {
        // clear existing slots
        foreach (Transform child in inventoryPanel.transform)
            Destroy(child.gameObject);

        // create fresh slots
        for (int i = 0; i < slotCount; i++)
            Instantiate(slotPrefab, inventoryPanel.transform);

        // populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = inventoryPanel.transform
                            .GetChild(data.slotIndex)
                            .GetComponent<Slot>();

                // fixed — was passing data.slotIndex, should be data.itemID
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null && slot != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}