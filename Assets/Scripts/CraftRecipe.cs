using UnityEngine;

[System.Serializable]
public class CraftRecipe
{
    public string recipeName;

    [Header("Requirements")]
    public int requiredItem1ID;
    public int requiredItem1Amount;

    public int requiredItem2ID;
    public int requiredItem2Amount;

    [Header("Result")]
    public GameObject resultPrefab;
    public int resultAmount = 1;
}