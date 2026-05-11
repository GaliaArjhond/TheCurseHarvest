using UnityEngine;

public class CraftingInventoryPreview : MonoBehaviour
{
    public Transform previewGrid;
    public GameObject slotPrefab;
    public int slotCount = 24;

    private InventoryController inventory;

    void Start()
    {
        inventory = InventoryController.Instance;
        CreatePreviewSlots();
    }

    public void RefreshPreview()
    {
        if (inventory == null)
            inventory = InventoryController.Instance;

        if (inventory == null) return;

        CreatePreviewSlots();
        ClearPreviewItems();

        CopyPanel(inventory.backpackPanel);
    }

    void CreatePreviewSlots()
    {
        if (previewGrid == null || slotPrefab == null) return;
        if (previewGrid.childCount > 0) return;

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, previewGrid);

            PreviewSlotBlocker blocker = slotObj.GetComponent<PreviewSlotBlocker>();
            if (blocker == null)
                slotObj.AddComponent<PreviewSlotBlocker>();
        }
    }

    void ClearPreviewItems()
    {
        foreach (Transform slotTransform in previewGrid)
        {
            foreach (Transform child in slotTransform)
            {
                if (child.GetComponent<Item>() != null)
                    Destroy(child.gameObject);
            }

            Slot slot = slotTransform.GetComponent<Slot>();
            if (slot != null)
                slot.currentItem = null;
        }
    }

    void CopyPanel(Transform sourcePanel)
    {
        if (sourcePanel == null) return;

        foreach (Transform sourceSlotTransform in sourcePanel)
        {
            Slot sourceSlot = sourceSlotTransform.GetComponent<Slot>();

            if (sourceSlot == null || sourceSlot.currentItem == null)
                continue;

            Item sourceItem = sourceSlot.currentItem.GetComponent<Item>();
            if (sourceItem == null) continue;

            AddPreviewItem(sourceItem);
        }
    }

    void AddPreviewItem(Item sourceItem)
    {
        foreach (Transform previewSlotTransform in previewGrid)
        {
            Slot previewSlot = previewSlotTransform.GetComponent<Slot>();

            if (previewSlot != null && previewSlot.currentItem != null)
                continue;

            GameObject copy = Instantiate(sourceItem.gameObject, previewSlotTransform);

            RectTransform rt = copy.GetComponent<RectTransform>();
            if (rt != null)
            {
                rt.anchorMin = Vector2.zero;
                rt.anchorMax = Vector2.one;
                rt.offsetMin = Vector2.zero;
                rt.offsetMax = Vector2.zero;
                rt.localScale = Vector3.one;
            }

            Item copyItem = copy.GetComponent<Item>();
            if (copyItem != null)
            {
                copyItem.amount = sourceItem.amount;
                copyItem.UpdateAmountText();
            }

            ItemDragHandler drag = copy.GetComponent<ItemDragHandler>();
            if (drag != null)
                Destroy(drag);

            CanvasGroup cg = copy.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }

            if (previewSlot != null)
                previewSlot.currentItem = copy;

            return;
        }
    }
}