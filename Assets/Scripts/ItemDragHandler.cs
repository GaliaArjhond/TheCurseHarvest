using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private Slot originalSlot;
    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSlot = originalParent.GetComponent<Slot>();

        transform.SetParent(transform.root);
        transform.SetAsLastSibling();

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

        Slot dropSlot = eventData.pointerEnter?.GetComponentInParent<Slot>();

        if (dropSlot == null || dropSlot == originalSlot)
        {
            ReturnToOriginalSlot();
            return;
        }

        Item draggedItem = GetComponent<Item>();

        if (draggedItem == null)
        {
            ReturnToOriginalSlot();
            return;
        }

        if (dropSlot.slotType != Slot.SlotType.Inventory)
        {
            if (!IsValidEquipmentDrop(dropSlot, draggedItem))
            {
                ReturnToOriginalSlot();
                return;
            }

            EquipToSlot(dropSlot);
            return;
        }

        if (originalSlot != null &&
            originalSlot.slotType != Slot.SlotType.Inventory)
        {
            MoveToSlot(dropSlot);

            EquipmentManager equipmentManager =
                EquipmentManager.Instance ??
                FindFirstObjectByType<EquipmentManager>();

            if (equipmentManager != null)
            {
                equipmentManager.Unequip(
                    GetEquipmentTypeFromSlot(originalSlot)
                );
            }

            return;
        }

        if (dropSlot.currentItem == null)
        {
            MoveToSlot(dropSlot);
            return;
        }

        Item targetItem = dropSlot.currentItem.GetComponent<Item>();

        if (draggedItem != null && targetItem != null &&
            draggedItem.ID == targetItem.ID &&
            draggedItem.isStackable)
        {
            int space = targetItem.maxStack - targetItem.amount;
            int moveAmount = Mathf.Min(space, draggedItem.amount);

            if (moveAmount > 0)
            {
                targetItem.amount += moveAmount;
                draggedItem.amount -= moveAmount;

                targetItem.UpdateAmountText();
                draggedItem.UpdateAmountText();
            }

            if (draggedItem.amount <= 0)
            {
                if (originalSlot != null)
                    originalSlot.currentItem = null;

                Destroy(gameObject);
            }
            else
            {
                ReturnToOriginalSlot();
            }

            return;
        }

        SwapWithSlot(dropSlot);
    }

    bool IsValidEquipmentDrop(Slot slot, Item item)
    {
        switch (slot.slotType)
        {
            case Slot.SlotType.Helmet:
                return item.itemType == Item.ItemType.Helmet;

            case Slot.SlotType.Armor:
                return item.itemType == Item.ItemType.Armor;

            case Slot.SlotType.Boots:
                return item.itemType == Item.ItemType.Boots;

            case Slot.SlotType.Charm:
                return item.itemType == Item.ItemType.Charm;

            default:
                return false;
        }
    }

    Item.ItemType GetEquipmentTypeFromSlot(Slot slot)
    {
        switch (slot.slotType)
        {
            case Slot.SlotType.Helmet:
                return Item.ItemType.Helmet;

            case Slot.SlotType.Armor:
                return Item.ItemType.Armor;

            case Slot.SlotType.Boots:
                return Item.ItemType.Boots;

            case Slot.SlotType.Charm:
                return Item.ItemType.Charm;

            default:
                return Item.ItemType.Charm;
        }
    }

    void MoveToSlot(Slot dropSlot)
    {
        if (originalSlot != null)
            originalSlot.currentItem = null;

        transform.SetParent(dropSlot.transform);
        transform.SetAsLastSibling();

        dropSlot.currentItem = gameObject;

        FitItemToSlot(transform);
    }

    void SwapWithSlot(Slot dropSlot)
    {
        GameObject otherItem = dropSlot.currentItem;

        if (originalSlot != null)
            originalSlot.currentItem = otherItem;

        otherItem.transform.SetParent(originalParent);
        otherItem.transform.SetAsLastSibling();
        FitItemToSlot(otherItem.transform);

        transform.SetParent(dropSlot.transform);
        transform.SetAsLastSibling();
        FitItemToSlot(transform);

        dropSlot.currentItem = gameObject;
    }

    void ReturnToOriginalSlot()
    {
        transform.SetParent(originalParent);
        transform.SetAsLastSibling();

        if (originalSlot != null)
            originalSlot.currentItem = gameObject;

        FitItemToSlot(transform);
    }

    void FitItemToSlot(Transform itemTransform)
    {
        RectTransform rt = itemTransform.GetComponent<RectTransform>();

        if (rt != null)
        {
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            rt.offsetMin = Vector2.zero;
            rt.offsetMax = Vector2.zero;
            rt.localScale = Vector3.one;
            rt.localRotation = Quaternion.identity;
        }
        else
        {
            itemTransform.localPosition = Vector3.zero;
            itemTransform.localScale = Vector3.one;
            itemTransform.localRotation = Quaternion.identity;
        }

        TMPro.TMP_Text text = itemTransform.GetComponentInChildren<TMPro.TMP_Text>();

        if (text != null)
        {
            text.gameObject.SetActive(true);
            text.transform.SetAsLastSibling();

            text.raycastTarget = false;
            text.enableAutoSizing = false;
            text.fontSize = 10;
            text.alignment = TMPro.TextAlignmentOptions.BottomRight;

            RectTransform textRT = text.GetComponent<RectTransform>();
            textRT.anchorMin = new Vector2(1, 0);
            textRT.anchorMax = new Vector2(1, 0);
            textRT.pivot = new Vector2(1, 0);
            textRT.sizeDelta = new Vector2(20, 20);
            textRT.anchoredPosition = new Vector2(-3, 3);
            textRT.localScale = Vector3.one;
        }

        Item item = itemTransform.GetComponent<Item>();
        if (item != null)
            item.UpdateAmountText();
    }

    void EquipToSlot(Slot slot)
    {
        if (slot == null)
            return;

        GameObject oldItem = slot.currentItem;

        if (originalSlot != null)
            originalSlot.currentItem = null;

        transform.SetParent(slot.transform);
        transform.SetAsLastSibling();

        slot.currentItem = gameObject;

        FitItemToSlot(transform);

        if (oldItem != null)
        {
            oldItem.transform.SetParent(originalParent);
            oldItem.transform.SetAsLastSibling();

            if (originalSlot != null)
                originalSlot.currentItem = oldItem;

            FitItemToSlot(oldItem.transform);
        }

        EquipmentManager equipmentManager =
            EquipmentManager.Instance ??
            FindFirstObjectByType<EquipmentManager>();

        if (equipmentManager != null)
        {
            equipmentManager.Equip(
                GetComponent<Item>(),
                slot
            );
        }
    }
}