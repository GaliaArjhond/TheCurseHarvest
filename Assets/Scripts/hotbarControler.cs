using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HotbarControler : MonoBehaviour
{
    [System.Serializable]
    public class StartingItem
    {
        public GameObject itemPrefab;
        public int amount = 1;
    }
    public static HotbarControler Instance;

    [Header("Hotbar")]
    public GameObject hotbarPanel;
    public GameObject slotPrefab;
    public int slotCount = 8;

    [Header("Starting Items")]
    [SerializeField] private List<StartingItem> startingItems;

    private ItemDictionary itemDictionary;
    private Key[] hotbarKeys;
    private int selectedSlotIndex = 0;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
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

        InventoryController inventory =
            InventoryController.Instance ??
            FindFirstObjectByType<InventoryController>();

        if (inventory != null)
            inventory.EnsureSlotsCreated();

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
            GameObject slotObj = Instantiate(slotPrefab, hotbarPanel.transform);

            HotbarSlotClick click = slotObj.GetComponent<HotbarSlotClick>();
            if (click == null)
                click = slotObj.AddComponent<HotbarSlotClick>();

            click.Setup(this, i);
        }

        Debug.Log("Hotbar slots created.");
    }

    public void GiveStartingItem()
    {
        Debug.Log("========== GIVE STARTING ITEMS ==========");

        if (hotbarPanel == null)
        {
            Debug.LogError("Hotbar Panel is NULL!");
            return;
        }

        if (hotbarPanel.transform.childCount == 0)
        {
            Debug.LogError("Hotbar has NO slots!");
            return;
        }

        Debug.Log("Hotbar Slots: " + hotbarPanel.transform.childCount);
        Debug.Log("Starting Items Count: " + startingItems.Count);

        InventoryController inventory =
            InventoryController.Instance ??
            FindFirstObjectByType<InventoryController>();

        for (int i = 0; i < startingItems.Count; i++)
        {
            StartingItem start = startingItems[i];

            Debug.Log("Checking Element " + i);

            if (start == null)
            {
                Debug.LogError("Element " + i + " is NULL!");
                continue;
            }

            if (start.itemPrefab == null)
            {
                Debug.LogError("Element " + i + " Item Prefab is NOT assigned!");
                continue;
            }

            Debug.Log("Giving " + start.itemPrefab.name + " x" + start.amount);

            if (i < hotbarPanel.transform.childCount)
            {
                Slot slot = hotbarPanel.transform
                    .GetChild(i)
                    .GetComponent<Slot>();

                if (slot == null)
                {
                    Debug.LogError("Slot " + i + " has no Slot component!");
                    continue;
                }

                if (slot.currentItem != null)
                {
                    Debug.LogWarning("Slot " + i + " already has an item.");
                    continue;
                }

                GameObject item = Instantiate(start.itemPrefab, slot.transform);

                CopyRectTransform(item);

                Item itemComponent = item.GetComponent<Item>();

                if (itemComponent != null)
                {
                    itemComponent.amount = start.amount;
                    itemComponent.UpdateAmountText();
                }

                slot.currentItem = item;

                Debug.Log("Placed into Hotbar Slot " + i);
            }
            else
            {
                if (inventory == null)
                {
                    Debug.LogError("InventoryController not found!");
                    continue;
                }

                Item prefabItem = start.itemPrefab.GetComponent<Item>();

                if (prefabItem == null)
                {
                    Debug.LogError(start.itemPrefab.name + " has no Item component!");
                    continue;
                }

                inventory.AddItem(prefabItem.ID, start.amount);

                Debug.Log("Added to Inventory: " + prefabItem.Name);
            }
        }

        Debug.Log("========== DONE ==========");
    }

    void CopyRectTransform(GameObject item)
    {
        RectTransform rt = item.GetComponent<RectTransform>();
        if (rt == null) return;

        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
        rt.localScale = Vector3.one;
    }

    public void SelectSlot(int index)
    {
        selectedSlotIndex = index;
        HighlightSlot(index);

        Item item = GetSelectedItem();

        if (item != null)
        {
            Debug.Log("Equipped: " + item.Name);

            if (EquippedItemUI.Instance != null)
                EquippedItemUI.Instance.ShowItemName(item.Name);
        }
        else
        {
            Debug.Log("Equipped: None");

            if (EquippedItemUI.Instance != null)
                EquippedItemUI.Instance.ShowItemName("Empty");
        }
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