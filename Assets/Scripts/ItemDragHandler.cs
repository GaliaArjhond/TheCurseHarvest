using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform orginalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        orginalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSlot = eventData.pointerEnter?.GetComponent<Slot>();
        Slot orginalSlot = orginalParent.GetComponent<Slot>();

        if (dropSlot != null) 
        {
            if (dropSlot.currentItem != null)
            {
                dropSlot.currentItem.transform.SetParent(orginalSlot.transform);
                orginalSlot.currentItem = dropSlot.currentItem;
                dropSlot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else 
            { 
                orginalSlot.currentItem = null;
            }
        }
        // Restore the dragged item to its original parent if not dropped on a valid slot
        transform.SetParent(orginalParent);
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}