using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;
    public Button button;

    private ShopUIManager shopUI;
    private ShopItemData shopData;
    private InventorySaveData playerData;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (icon == null)
            icon = transform.Find("Icon")?.GetComponent<Image>();

        if (amountText == null)
            amountText = transform.Find("AmountText")?.GetComponent<TextMeshProUGUI>();
    }

    public void SetupStore(Sprite sprite, ShopItemData data, ShopUIManager manager)
    {
        shopUI = manager;
        shopData = data;

        if (icon != null)
            icon.sprite = sprite;

        if (amountText != null)
            amountText.text = "₱" + data.price;

        if (button == null)
        {
            Debug.LogError("ShopSlotUI missing Button on " + gameObject.name);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            shopUI.SelectStoreItem(shopData);
        });
    }

    public void SetupPlayer(Sprite sprite, InventorySaveData data, ShopUIManager manager)
    {
        shopUI = manager;
        playerData = data;

        if (icon != null)
            icon.sprite = sprite;

        if (amountText != null)
            amountText.text = data.amount.ToString();

        if (button == null)
        {
            Debug.LogError("ShopSlotUI missing Button on " + gameObject.name);
            return;
        }

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            shopUI.SelectPlayerItem(playerData);
        });
    }
}