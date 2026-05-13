using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopSlotUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText;

    [HideInInspector] public ShopItemData data;

    public void Setup(
        Sprite sprite,
        int amount,
        ShopItemData shopData
    )
    {
        icon.sprite = sprite;
        amountText.text = amount.ToString();

        data = shopData;
    }
}