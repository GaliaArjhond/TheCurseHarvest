using UnityEngine;
using UnityEngine.EventSystems;

public class HotbarSlotClick : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;
    private HotbarControler hotbar;

    public void Setup(HotbarControler controller, int index)
    {
        hotbar = controller;
        slotIndex = index;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hotbar != null)
            hotbar.SelectSlot(slotIndex);
    }
}