using UnityEngine;
using UnityEngine.Events;

public class Crop : MonoBehaviour
{
    private CropData curCrop;
    private int plantDay;
    private int daysSinceLastWatered;

    public SpriteRenderer sr;

    public static event UnityAction<CropData> onPlantCrop;
    public static event UnityAction<CropData> onHarvestCrop;

    public void Plant(CropData crop, int currentDay)
    {
        curCrop = crop;
        plantDay = currentDay;
        daysSinceLastWatered = 1;
        UpdateCropSprite();
        onPlantCrop?.Invoke(crop);
    }

    public void Water()
    {
        daysSinceLastWatered = 0;
        Debug.Log("Crop watered");
    }

    public void Harvest()
    {
        if (!CanHarvest()) return;
        onHarvestCrop?.Invoke(curCrop);
        Destroy(gameObject);
    }

    public bool CanHarvest()
    {
        return CropProgress() >= curCrop.daysToGrow;
    }

    // called by FarmTile every new day
    public void NewDayCheck(int currentDay)
    {
        daysSinceLastWatered++;

        // crop dies if not watered for 3 days
        if (daysSinceLastWatered > 3)
        {
            Debug.Log("Crop died from lack of water!");
            Destroy(gameObject);
            return;
        }

        UpdateCropSprite();
    }

    int CropProgress()
    {
        return DayManager.Instance.dayNumber - plantDay;
    }

    void UpdateCropSprite()
    {
        if (curCrop == null) return;

        int progress = CropProgress();

        if (progress < curCrop.daysToGrow && curCrop.growProgressSprites.Length > 0)
        {
            int index = Mathf.Clamp(progress, 0, curCrop.growProgressSprites.Length - 1);
            sr.sprite = curCrop.growProgressSprites[index];
        }
        else if (curCrop.readyToHarvestSprite != null)
        {
            sr.sprite = curCrop.readyToHarvestSprite;
        }
    }
}