using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public Button button;
    public GameObject highlight;

    private ShopUIManager shopUI;
    private ShopItemData shopData;
    private InventorySaveData playerData;
    private bool isStoreSlot;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (icon == null)
            icon = transform.Find("Icon")?.GetComponent<Image>();

        if (amountText == null)
            amountText = transform.Find("AmountText")?.GetComponent<TextMeshProUGUI>();

        if (highlight == null)
            highlight = transform.Find("Highlight")?.gameObject;

        SetSelected(false);
    }

    public void SetupStore(Sprite sprite, ShopItemData data, ShopUIManager manager)
    {
        shopUI = manager;
        shopData = data;
        isStoreSlot = true;

        icon.sprite = sprite;
        amountText.text = "₱" + data.price;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            shopUI.ToggleStoreSelection(shopData, this);
        });
    }

    public void SetupPlayer(Sprite sprite, InventorySaveData data, ShopUIManager manager)
    {
        shopUI = manager;
        playerData = data;
        isStoreSlot = false;

        icon.sprite = sprite;
        amountText.text = data.amount.ToString();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            shopUI.TogglePlayerSelection(playerData, this);
        });
    }

    public void SetSelected(bool selected)
    {
        if (highlight != null)
            highlight.SetActive(selected);
    }
}