using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private RectTransform panelRect;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text buyPriceText;
    [SerializeField] private TMP_Text sellPriceText;

    [SerializeField] private float offsetY = 18f;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(
        Item item,
        Vector2 pointerPosition,
        RectTransform itemRect)
    {
        if (panel == null || panelRect == null)
            return;

        panel.SetActive(true);

        itemNameText.text = item.Name;
        descriptionText.text = item.description;

        typeText.text =
            "Type: " + item.itemType.ToString();

        buyPriceText.text =
            "Buy: " + item.buyPrice;

        sellPriceText.text =
            "Sell: " + item.sellPrice;

        if (itemRect != null)
        {
            Vector3 itemTop =
                itemRect.position +
                new Vector3(
                    0f,
                    itemRect.rect.height * 0.5f,
                    0f
                );

            panelRect.position =
                itemTop +
                new Vector3(0f, offsetY, 0f);
        }
        else
        {
            panelRect.position =
                new Vector3(
                    pointerPosition.x,
                    pointerPosition.y + offsetY,
                    0f
                );
        }
    }

    public void HideTooltip()
    {
        if (panel != null)
            panel.SetActive(false);
    }
}