using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HalimawPopupUI : MonoBehaviour
{
    public static HalimawPopupUI Instance;

    public Image monsterIcon;
    public TMP_Text titleText;
    public TMP_Text monsterNameText;
    public TMP_Text subtitleText;

    private CanvasGroup canvasGroup;
    private Coroutine hideRoutine;

    private const float DisplayDuration = 3f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (canvasGroup == null)
            canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.interactable = false;
    }

    public void Show(HalimawEntry entry)
    {
        Debug.Log("Showing Halimaw Popup");

        if (entry == null)
            return;

        if (monsterIcon != null)
            monsterIcon.sprite = entry.icon;

        if (titleText != null)
            titleText.text = "New Halimaw Discovered!";

        if (monsterNameText != null)
            monsterNameText.text = entry.monsterName;

        if (subtitleText != null)
            subtitleText.text = "Added to the Halimaw Log";

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            canvasGroup.interactable = true;
        }

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HidePopup());
    }

    private IEnumerator HidePopup()
    {
        yield return new WaitForSeconds(DisplayDuration);

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
    }
}