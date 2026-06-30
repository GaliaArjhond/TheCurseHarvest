using UnityEngine;

public class Slot : MonoBehaviour
{
    public enum SlotType
    {
        Inventory,
        Helmet,
        Armor,
        Boots,
        Charm
    }

    public SlotType slotType = SlotType.Inventory;
    public GameObject currentItem; //item currently in the slot
}
