using UnityEngine;

public class ChestInventory : MonoBehaviour
{
    public Transform chestSlotGrid;
    public GameObject slotPrefab;
    public int slotCount = 24;

    void Start()
    {
        if (chestSlotGrid.childCount > 0) return;

        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, chestSlotGrid);
        }
    }
}