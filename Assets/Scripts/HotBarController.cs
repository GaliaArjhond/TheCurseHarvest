using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.InputSystem;

public class HotBarController : MonoBehaviour
{
    public GameObject hotBarPanel;
    public GameObject slotPrefab;
    public int slotCount = 7;

    private ItemDictionary itemDictionary;

    // Use InputSystem.Key instead of KeyCode
    private Key[] hotbarKeys;

    private void Awake()
    {
        itemDictionary = Object.FindFirstObjectByType<ItemDictionary>();

        hotbarKeys = new Key[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            // Map 1-9 to Key.Digit1 - Key.Digit9, 10th to Key.Digit0
            if (i < 9)
            {
                hotbarKeys[i] = Key.Digit1 + i;
            }
            else
            {
                hotbarKeys[i] = Key.Digit0;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < slotCount; i++) 
        {
            if (Keyboard.current != null && Keyboard.current[hotbarKeys[i]].wasPressedThisFrame) 
            {
                UseItemInSlot(i);
            }
        }
    }

    void UseItemInSlot(int index) 
    { 
        Slot slot = hotBarPanel.transform.GetChild(index).GetComponent<Slot>();
        if(slot.currentItem != null) 
        {
            Item item = slot.currentItem.GetComponent<Item>();
            item.UseItem();
        }
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        // fixed — was invData which was never declared
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();

        foreach (Transform slotTransform in hotBarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                hotbarData.Add(new InventorySaveData { itemID = item.ID, slotIndex = slotTransform.GetSiblingIndex() });
            }
        }

        return hotbarData;
    }

    // fixed — was missing parameter name
    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        // clear existing slots
        foreach (Transform child in hotBarPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // create fresh slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotBarPanel.transform);
        }

        // populate slots with saved items
        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex < slotCount)
            {
                Slot slot = hotBarPanel.transform
                            .GetChild(data.slotIndex)
                            .GetComponent<Slot>();

                // fixed — was passing data.slotIndex, should be data.itemID
                GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

                if (itemPrefab != null)
                {
                    GameObject item = Instantiate(itemPrefab, slot.transform);
                    item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                    slot.currentItem = item;
                }
            }
        }
    }
}
