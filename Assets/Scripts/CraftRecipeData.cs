using UnityEngine;

[CreateAssetMenu(fileName = "CraftRecipeData", menuName = "Crafting/Recipe")]
public class CraftRecipeData : ScriptableObject
{
    public string recipeName;

    [Header("Visual")]
    public Sprite recipeIcon;

    [Header("Requirements")]
    public int woodAmount;
    public int stoneAmount;

    [Header("Result")]
    public GameObject resultPrefab;
    public int resultAmount = 1;
}