using UnityEngine;
using UnityEngine.EventSystems;

public class ItemTooltipTrigger :
    MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private Item item;

    private void Awake()
    {
        item = GetComponent<Item>();
    }

    public void OnPointerEnter(
        PointerEventData eventData)
    {
        if (ItemTooltip.Instance != null)
            ItemTooltip.Instance.ShowTooltip(item);
    }

    public void OnPointerExit(
        PointerEventData eventData)
    {
        if (ItemTooltip.Instance != null)
            ItemTooltip.Instance.HideTooltip();
    }
}