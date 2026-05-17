using UnityEngine;
using UnityEngine.UI;

public class SkillTreeTabController : MonoBehaviour
{
    [Header("Category Buttons")]
    [SerializeField] private GameObject skillCategoryButtons;

    [Header("Tree Panels")]
    [SerializeField] private GameObject combatTreePanel;
    [SerializeField] private GameObject farmingTreePanel;
    [SerializeField] private GameObject craftingTreePanel;
    [SerializeField] private GameObject curseTreePanel;

    [Header("Curse Unlock")]
    [SerializeField] private Button curseTabButton;


    void OnEnable()
    {
        UpdateCurseUnlock();
        ShowCategories();
    }

    void UpdateCurseUnlock()
    {
        PlayerStatsManager stats =
            FindFirstObjectByType<PlayerStatsManager>();

        if (stats == null)
            return;

        bool unlocked = stats.stats.level >= 5;

        // disable tooltip trigger after unlock
        SkillTooltipTrigger trigger =
            curseTabButton.GetComponent<SkillTooltipTrigger>();

        if (trigger != null)
            trigger.enabled = !unlocked;
    }

    public void ShowCategories()
    {
        skillCategoryButtons.SetActive(true);

        combatTreePanel.SetActive(false);
        farmingTreePanel.SetActive(false);
        craftingTreePanel.SetActive(false);
        curseTreePanel.SetActive(false);
    }

    public void ShowCombatTree()
    {
        ShowOnly(combatTreePanel);
    }

    public void ShowFarmingTree()
    {
        ShowOnly(farmingTreePanel);
    }

    public void ShowCraftingTree()
    {
        ShowOnly(craftingTreePanel);
    }

    public void ShowCurseTree()
    {
        PlayerStatsManager stats =
            FindFirstObjectByType<PlayerStatsManager>();

        if (stats != null &&
            stats.stats.level < 5)
        {
            Debug.Log("Curse tree locked.");
            return;
        }

        ShowOnly(curseTreePanel);
    }

    void ShowOnly(GameObject panelToShow)
    {
        skillCategoryButtons.SetActive(false);

        combatTreePanel.SetActive(false);
        farmingTreePanel.SetActive(false);
        craftingTreePanel.SetActive(false);
        curseTreePanel.SetActive(false);

        panelToShow.SetActive(true);
    }
}