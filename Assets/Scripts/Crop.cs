using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData cropData;
    private SpriteRenderer sr;

    private int growthStage = 0;
    private bool wateredToday = false;
    private bool readyToHarvest = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Plant(CropData data, int currentDay)
    {
        cropData = data;
        growthStage = 0;
        wateredToday = false;
        readyToHarvest = false;

        UpdateSprite();
    }

    public void Water()
    {
        wateredToday = true;
    }

    public void NewDayCheck(int currentDay)
    {
        if (!wateredToday)
        {
            Debug.Log(cropData.cropName + " did not grow because it was not watered.");
            return;
        }

        wateredToday = false;
        growthStage++;

        if (growthStage >= cropData.daysToGrow)
        {
            readyToHarvest = true;
            sr.sprite = cropData.readyToHarvestSprite;
            return;
        }

        UpdateSprite();
    }

    void UpdateSprite()
    {
        if (cropData == null) return;

        if (cropData.growProgressSprites != null &&
            cropData.growProgressSprites.Length > 0)
        {
            int index = Mathf.Clamp(growthStage, 0, cropData.growProgressSprites.Length - 1);
            sr.sprite = cropData.growProgressSprites[index];
        }
    }

    public bool CanHarvest()
    {
        return readyToHarvest;
    }

    public void Harvest()
    {
        if (!readyToHarvest) return;

        int amount = Random.Range(cropData.harvestMin, cropData.harvestMax + 1);

        if (InventoryController.Instance != null)
            InventoryController.Instance.AddItem(cropData.harvestItemID, amount);

        Destroy(gameObject);
    }
}