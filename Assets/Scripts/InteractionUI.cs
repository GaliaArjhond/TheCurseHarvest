using TMPro;
using UnityEngine;

public class InteractionUI : MonoBehaviour
{
    public static InteractionUI Instance;

    [SerializeField]
    private TextMeshProUGUI interactionText;

    [Header("Follow Player")]
    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 1.8f, 0f);

    private RectTransform textRect;
    private Canvas parentCanvas;

    void Awake()
    {
        Instance = this;

        if (interactionText != null)
        {
            interactionText.gameObject.SetActive(false);
            textRect = interactionText.GetComponent<RectTransform>();
        }

        parentCanvas = GetComponentInParent<Canvas>();

        if (player == null)
        {
            GameObject pgo = GameObject.FindWithTag("Player");
            if (pgo != null)
            {
                player = pgo.transform;
            }
        }
    }

    void Update()
    {
        if (interactionText == null || textRect == null || player == null || !interactionText.gameObject.activeSelf)
            return;

        Camera cam = Camera.main;
        if (cam == null) return;

        Vector3 screenPos = cam.WorldToScreenPoint(player.position + offset);

        if (parentCanvas != null && parentCanvas.renderMode != RenderMode.WorldSpace)
        {
            RectTransform canvasRect = parentCanvas.GetComponent<RectTransform>();
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPos, parentCanvas.worldCamera, out localPoint);
            textRect.anchoredPosition = localPoint;
        }
        else
        {
            textRect.position = screenPos;
        }
    }

    public void Show(string text)
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(true);
        interactionText.text = text;
    }

    public void Hide()
    {
        if (interactionText == null) return;

        interactionText.gameObject.SetActive(false);
    }

    private Coroutine hideCoroutine;

    public void ShowTemporary(string text, float seconds = 2f)
    {
        Show(text);

        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfter(seconds));
    }

    private System.Collections.IEnumerator HideAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Hide();
        hideCoroutine = null;
    }
}