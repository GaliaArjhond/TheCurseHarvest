using UnityEngine;

public class SkillTreeTabController : MonoBehaviour
{
    [SerializeField] private GameObject skillCategoryButtons;

    [SerializeField] private GameObject combatTreePanel;
    [SerializeField] private GameObject farmingTreePanel;
    [SerializeField] private GameObject craftingTreePanel;
    [SerializeField] private GameObject curseTreePanel;

    void OnEnable()
    {
        ShowCategories();
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