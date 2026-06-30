using UnityEngine;
using TMPro;

public class Item : MonoBehaviour
{
    public int ID;
    public string Name;
    
    [Header("Description")]
    [TextArea(3, 5)]
    public string description;

    public ItemType itemType = ItemType.Misc;

    [Header("Placeable")]
    public bool isPlaceable = true;
    public GameObject placeablePrefab;

    [Header("Shop")]
    public int buyPrice = 10;
    public int sellPrice = 5;

    [Header("Stacking")]
    public bool isStackable = true;
    public int amount = 1;
    public int maxStack = 99;

    [Header("UI")]
    public TMP_Text amountText;

    public enum ItemType
    {
        Misc,
        Tool,
        Weapon,
        Helmet,
        Armor,
        Boots,
        Charm,
        Seed,
        Food,
        Resource
    }

    public CropData cropData;

    [Header("Equipment Stats")]

    public int hpBonus;

    public int attackBonus;

    public int defenseBonus;

    public int staminaBonus;

    public float moveSpeedBonus;

    public float fishingLuckBonus;

    public float miningSpeedBonus;

    void Awake()
    {
        if (amountText == null)
            amountText = GetComponentInChildren<TMP_Text>(true);

        FixAmountText();
        UpdateAmountText();
    }

    public void UpdateAmountText()
    {
        if (amountText == null) return;

        FixAmountText();

        amountText.text = amount > 1 ? amount.ToString() : "";
    }

    void FixAmountText()
    {
        if (amountText == null) return;

        amountText.gameObject.SetActive(true);
        amountText.transform.SetAsLastSibling();

        amountText.raycastTarget = false;
        amountText.enableAutoSizing = false;
        amountText.fontSize = 18;
        amountText.alignment = TextAlignmentOptions.BottomRight;

        RectTransform textRT = amountText.GetComponent<RectTransform>();

        textRT.anchorMin = new Vector2(1, 0);
        textRT.anchorMax = new Vector2(1, 0);
        textRT.pivot = new Vector2(1, 0);

        textRT.sizeDelta = new Vector2(30, 25);
        textRT.anchoredPosition = new Vector2(-3, 3);

        textRT.localScale = Vector3.one;
        textRT.localRotation = Quaternion.identity;
    }

    public virtual void UseItem()
    {
        Debug.Log("Using item: " + Name);
    }
}