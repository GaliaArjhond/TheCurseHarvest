using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HotbarControler : MonoBehaviour
{
    [Header("Hotbar")]
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 8;

    [Header("Starting Items")]
    [SerializeField] private List<GameObject> startingItemPrefabs;

    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;
    private int selectedSlotIndex = 0;

    void Awake()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        hotbarKeys = new Key[slotCount];

        for (int i = 0; i < slotCount; i++)
        {
            hotbarKeys[i] = i < 7 ? (Key)((int)Key.Digit1 + i) : Key.Digit0;
        }
    }

    void Start()
    {
        CreateSlotsIfMissing();

        if (SaveController.Instance == null || !SaveController.Instance.HasSave())
        {
            GiveStartingItem();
        }

        SelectSlot(0);
    }

    void Update()
    {
        for (int i = 0; i < slotCount; i++)
        {
            if (Keyboard.current[hotbarKeys[i]].wasPressedThisFrame)
            {
                SelectSlot(i);
            }
        }
    }

    void CreateSlotsIfMissing()
    {
        if (hotbarPanel == null)
        {
            Debug.LogError("HotbarPanel is not assigned!");
            return;
        }

        if (hotbarPanel.transform.childCount > 0)
            return;

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, hotbarPanel.transform);
        }

        Debug.Log("Hotbar slots created.");
    }

   void GiveStartingItem()
    {
        if (hotbarPanel == null) return;
        if (hotbarPanel.transform.childCount == 0) return;

        for (int i = 0; i < startingItemPrefabs.Count; i++)
        {
            if (i >= hotbarPanel.transform.childCount) break;

            Slot slot = hotbarPanel.transform.GetChild(i).GetComponent<Slot>();

            if (slot == null) continue;
            if (slot.currentItem != null) continue;
            if (startingItemPrefabs[i] == null) continue;

            GameObject item = Instantiate(startingItemPrefabs[i], slot.transform);

            RectTransform rt = item.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            slot.currentItem = item;
        }
    }

    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        HighlightSlot(index);

        Item item = GetSelectedItem();
        Debug.Log("Equipped: " + (item != null ? item.Name : "None"));
    }

    public Item GetSelectedItem()
    {
        if (hotbarPanel == null) return null;
        if (selectedSlotIndex >= hotbarPanel.transform.childCount) return null;

        Slot slot = hotbarPanel.transform
            .GetChild(selectedSlotIndex)
            .GetComponent<Slot>();

        if (slot == null || slot.currentItem == null)
            return null;

        return slot.currentItem.GetComponent<Item>();
    }

    void HighlightSlot(int index)
    {
        if (hotbarPanel == null) return;

        for (int i = 0; i < hotbarPanel.transform.childCount; i++)
        {
            Image img = hotbarPanel.transform.GetChild(i).GetComponent<Image>();

            if (img != null)
                img.color = (i == index) ? Color.yellow : Color.white;
        }
    }

    public List<InventorySaveData> GetHotbarItems()
    {
        List<InventorySaveData> hotbarData = new List<InventorySaveData>();

        if (hotbarPanel == null) return hotbarData;

        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot == null) continue;

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();
                if (item == null) continue;

                hotbarData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex(),
                    amount = item.amount
                });
            }
        }

        return hotbarData;
    }

    public void SetHotbarItems(List<InventorySaveData> inventorySaveData)
    {
        CreateSlotsIfMissing();

        // Clear only items, NOT slots
        foreach (Transform slotTransform in hotbarPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot != null && slot.currentItem != null)
            {
                Destroy(slot.currentItem);
                slot.currentItem = null;
            }
        }

        if (inventorySaveData == null || inventorySaveData.Count == 0)
        {
            Debug.Log("No saved hotbar data.");
            SelectSlot(0);
            return;
        }

        foreach (InventorySaveData data in inventorySaveData)
        {
            if (data.slotIndex >= hotbarPanel.transform.childCount) continue;

            Slot slot = hotbarPanel.transform
                .GetChild(data.slotIndex)
                .GetComponent<Slot>();

            if (slot == null) continue;

            GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);
            if (itemPrefab == null) continue;

            GameObject item = Instantiate(itemPrefab, slot.transform);

            RectTransform rt = item.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            Item newItem = item.GetComponent<Item>();
            if (newItem != null)
            {
                newItem.amount = Mathf.Max(1, data.amount);
                newItem.UpdateAmountText();
            }

            slot.currentItem = item;
        }

        SelectSlot(selectedSlotIndex);
    }
}