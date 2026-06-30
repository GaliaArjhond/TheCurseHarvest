using UnityEngine;
using TMPro;
using System.Collections;

public class PickupUI : MonoBehaviour
{
    public static PickupUI Instance;

    [Header("UI")]
    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TextMeshProUGUI pickupText;

    private ItemDictionary itemDictionary;
    private Coroutine popupRoutine;

    void Awake()
    {
        Instance = this;

        itemDictionary = FindFirstObjectByType<ItemDictionary>();

        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    public void ShowPickup(int itemID, int amount)
    {
        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine =
            StartCoroutine(ShowRoutine(itemID, amount));
    }

    public void ShowReward(string message)
    {
        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine = StartCoroutine(ShowTextRoutine(message));
    }

    IEnumerator ShowRoutine(int itemID, int amount)
    {
        string itemName = "Unknown";

        if (itemDictionary != null)
        {
            GameObject prefab =
                itemDictionary.GetItemPrefab(itemID);

            if (prefab != null)
            {
                Item item = prefab.GetComponent<Item>();

                if (item != null)
                    itemName = item.Name;
            }
        }

        yield return StartCoroutine(ShowTextRoutine("+" + amount + " " + itemName));
    }

    IEnumerator ShowTextRoutine(string message)
    {
        if (popupRoot != null)
            popupRoot.SetActive(true);

        if (pickupText != null)
            pickupText.text = message;

        yield return new WaitForSeconds(1.5f);

        if (popupRoot != null)
            popupRoot.SetActive(false);
    }
}