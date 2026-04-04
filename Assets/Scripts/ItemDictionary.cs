using UnityEngine;
using System.Collections.Generic;

public class ItemDictionary : MonoBehaviour
{
    // fixed — was List<Item> but Item is a component not a standalone object
    public List<GameObject> itemPrefabs;
    private Dictionary<int, GameObject> itemsDictionary;

    void Awake()
    {
        itemsDictionary = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                Item item = itemPrefabs[i].GetComponent<Item>();
                if (item != null)
                {
                    // fixed — was itemsPrefabs[i].ID which doesn't work on GameObject
                    item.ID = i + 1;
                }
            }
        }

        foreach (GameObject prefab in itemPrefabs)
        {
            if (prefab == null) continue;
            Item item = prefab.GetComponent<Item>();
            if (item != null)
                itemsDictionary[item.ID] = prefab;
        }
    }

    public GameObject GetItemPrefab(int itemID)
    {
        itemsDictionary.TryGetValue(itemID, out GameObject prefab);
        if (prefab == null)
            Debug.LogWarning("Item with ID " + itemID + " not found in dictionary.");
        return prefab;
    }
}