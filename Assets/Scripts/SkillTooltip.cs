using UnityEngine;
using TMPro;

public class SkillTooltip : MonoBehaviour
{
    public static SkillTooltip Instance;

    [SerializeField] private GameObject tooltipRoot;
    [SerializeField] private TextMeshProUGUI tooltipText;

    void Awake()
    {
        Instance = this;

        if (tooltipRoot != null)
            tooltipRoot.SetActive(false);
    }

    public void ShowTooltip(string text)
    {
        if (tooltipRoot != null)
            tooltipRoot.SetActive(true);

        if (tooltipText != null)
            tooltipText.text = text;
    }

    public void HideTooltip()
    {
        if (tooltipRoot != null)
            tooltipRoot.SetActive(false);
    }
}