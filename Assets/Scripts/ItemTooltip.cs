using UnityEngine;
using TMPro;

public class ItemTooltip : MonoBehaviour
{
    public static ItemTooltip Instance;

    [SerializeField] private GameObject panel;

    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text descriptionText;
    [SerializeField] private TMP_Text typeText;
    [SerializeField] private TMP_Text buyPriceText;
    [SerializeField] private TMP_Text sellPriceText;

    private void Awake()
    {
        Instance = this;
        HideTooltip();
    }

    public void ShowTooltip(Item item)
    {
        panel.SetActive(true);

        itemNameText.text = item.Name;
        descriptionText.text = item.description;

        typeText.text =
            "Type: " + item.itemType.ToString();

        buyPriceText.text =
            "Buy: " + item.buyPrice;

        sellPriceText.text =
            "Sell: " + item.sellPrice;
    }

    public void HideTooltip()
    {
        panel.SetActive(false);
    }
}