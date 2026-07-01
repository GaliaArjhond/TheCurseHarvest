using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RecipeEntryUI : MonoBehaviour
{
    public Image recipeIconImage;
    public TextMeshProUGUI recipeNameText;
    public TextMeshProUGUI requirementText;
    public Button craftButton;

    private CraftRecipeData recipe;
    private CraftingManager craftingManager;

    public void Setup(CraftRecipeData newRecipe, CraftingManager manager)
    {
        recipe = newRecipe;
        craftingManager = manager;

        if (recipeIconImage != null)
            recipeIconImage.sprite = recipe.recipeIcon;
        else
            Debug.LogWarning("RecipeEntryUI: Recipe Icon Image is not assigned.");

        if (recipeNameText != null)
            recipeNameText.text = recipe.recipeName;

        if (requirementText != null)
            requirementText.text = recipe.GetRequirementsText();

        if (craftButton != null)
        {
            craftButton.onClick.RemoveAllListeners();
            craftButton.onClick.AddListener(() =>
            {
                craftingManager.Craft(recipe);
            });
        }
        else
        {
            Debug.LogWarning("RecipeEntryUI: Craft Button is not assigned.");
        }
    }

    
}