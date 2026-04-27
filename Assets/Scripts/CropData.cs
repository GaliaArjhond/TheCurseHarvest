using UnityEngine;

[CreateAssetMenu(fileName = "CropData", menuName = "Farming/Crop Data")]
public class CropData : ScriptableObject
{
    public string cropName = "Wheat";
    public int daysToGrow = 4;
    public Sprite[] growProgressSprites; // one sprite per day of growth
    public Sprite readyToHarvestSprite;

    // what item drops when harvested
    public int harvestItemID = 0;
    public string harvestItemName = "Wheat";
    public int harvestMin = 1;
    public int harvestMax = 3;
}