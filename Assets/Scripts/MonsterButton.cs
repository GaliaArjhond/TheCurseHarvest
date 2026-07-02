using TMPro;
using UnityEngine;

public class MonsterButton : MonoBehaviour
{
    public HalimawEntry halimawEntry;
    public TMP_Text buttonText;

    private void Start()
    {
        RefreshButton();
    }

    public void RefreshButton()
    {
        if (buttonText == null)
            return;

        if (HalimawLogManager.Instance != null && HalimawLogManager.Instance.IsUnlocked(halimawEntry))
        {
            buttonText.text = halimawEntry != null ? halimawEntry.monsterName : "???";
        }
        else
        {
            buttonText.text = "???";
        }
    }

    public void ShowInfo()
    {
        if (MonsterPageUI.Instance != null)
            MonsterPageUI.Instance.ShowMonster(halimawEntry);
    }
}