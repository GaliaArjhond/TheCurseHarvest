using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopUIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform storeGrid;
    [SerializeField] private GameObject slotPrefab;

    [Header("Store Items")]
    [SerializeField] private List<ShopItemData> storeItems;

    private ItemDictionary itemDictionary;

    void Start()
    {
        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        GenerateStore();
    }

    void GenerateStore()
    {
        if (storeGrid == null)
        {
            Debug.LogError("ShopUIManager: Store Grid is not assigned.");
            return;
        }

        if (slotPrefab == null)
        {
            Debug.LogError("ShopUIManager: Slot Prefab is not assigned.");
            return;
        }

        if (itemDictionary == null)
        {
            Debug.LogError("ShopUIManager: ItemDictionary not found.");
            return;
        }

        foreach (Transform child in storeGrid)
            Destroy(child.gameObject);

        foreach (ShopItemData item in storeItems)
        {
            GameObject prefab = itemDictionary.GetItemPrefab(item.itemID);

            if (prefab == null)
            {
                Debug.LogError("Shop item ID not found: " + item.itemID);
                continue;
            }

            Item itemData = prefab.GetComponent<Item>();

            if (itemData == null)
            {
                Debug.LogError(prefab.name + " has no Item script.");
                continue;
            }

            Image itemImage = prefab.GetComponent<Image>();

            if (itemImage == null)
            {
                Debug.LogError(prefab.name + " has no Image component.");
                continue;
            }

            GameObject slot = Instantiate(slotPrefab, storeGrid);

            ShopSlotUI ui = slot.GetComponent<ShopSlotUI>();

            if (ui == null)
            {
                Debug.LogError("Shop slot prefab has no ShopSlotUI script.");
                continue;
            }

            ui.Setup(
                itemImage.sprite,
                item.price,
                item
            );
        }
    }
}