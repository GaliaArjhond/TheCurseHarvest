using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryContextMenu : MonoBehaviour
{
    public static InventoryContextMenu Instance;

    [Header("UI")]
    public RectTransform panel;
    public RectTransform contextMenuAnchor;

    public Image itemIcon;
    public TMP_Text itemName;
    public TMP_Text itemDescription;

    private Item currentItem;
    private Slot currentSlot;

    void Awake()
    {
        Instance = this;

        if (panel != null)
            panel.gameObject.SetActive(false);
    }

    public void Open(Item item, Slot slot, Vector2 position)
    {
        currentItem = item;
        currentSlot = slot;

        panel.gameObject.SetActive(true);

        // Always open above the hotbar
        if (contextMenuAnchor != null)
            panel.position = contextMenuAnchor.position;

        itemName.text = item.Name;
        itemDescription.text = item.description;

        if (itemIcon != null)
        {
            Image sourceImage = item.GetComponent<Image>();

            if (sourceImage != null)
                itemIcon.sprite = sourceImage.sprite;
            else
                itemIcon.sprite = null;
        }
    }

    public void Close()
    {
        panel.gameObject.SetActive(false);

        currentItem = null;
        currentSlot = null;
    }

    public void DropItem()
    {
        if (currentItem == null || currentSlot == null)
            return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && currentItem.worldPrefab != null)
        {
            Vector3 dropPos = player.transform.position + player.transform.right;

            Instantiate(
                currentItem.worldPrefab,
                dropPos,
                Quaternion.identity
            );
        }

        currentSlot.currentItem = null;
        Destroy(currentItem.gameObject);

        Close();
    }

    public void TrashItem()
    {
        if (currentItem == null || currentSlot == null)
            return;

        currentSlot.currentItem = null;
        Destroy(currentItem.gameObject);

        Close();
    }
}