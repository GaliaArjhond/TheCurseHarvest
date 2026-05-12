using UnityEngine;
using TMPro;
using System.Collections;

public class EquippedItemUI : MonoBehaviour
{
    public static EquippedItemUI Instance;

    [SerializeField] private GameObject popupRoot;
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private float showTime = 1.5f;

    private Coroutine popupRoutine;

    void Awake()
    {
        Instance = this;

        if (popupRoot != null)
            popupRoot.SetActive(false);
    }

    public void ShowItemName(string itemName)
    {
        if (popupRoutine != null)
            StopCoroutine(popupRoutine);

        popupRoutine =
            StartCoroutine(ShowRoutine(itemName));
    }

    IEnumerator ShowRoutine(string itemName)
    {
        popupRoot.SetActive(true);

        itemNameText.text = itemName;

        yield return new WaitForSeconds(showTime);

        popupRoot.SetActive(false);
    }
}