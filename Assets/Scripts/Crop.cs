using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData cropData;
    private int growthDay = 0;
    private bool readyToHarvest = false;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Plant(CropData data)
    {
        cropData = data;
        growthDay = 0;
        readyToHarvest = false;

        sr.sprite = cropData.growProgressSprites[0];

        DayManager.Instance.onNewDay += Grow;
    }

    void Grow()
    {
        if (readyToHarvest) return;

        growthDay++;

        if (growthDay >= cropData.daysToGrow)
        {
            readyToHarvest = true;
            sr.sprite = cropData.readyToHarvestSprite;

            Debug.Log("Crop ready to harvest!");
            return;
        }

        if (growthDay < cropData.growProgressSprites.Length)
        {
            sr.sprite = cropData.growProgressSprites[growthDay];
        }
    }

    public bool CanHarvest()
    {
        return readyToHarvest;
    }

    public void Harvest()
    {
        if (!readyToHarvest)
        {
            Debug.Log("Crop is not ready yet.");
            return;
        }

        int amount =
            Random.Range(
                cropData.harvestMin,
                cropData.harvestMax + 1
            );

        if (InventoryController.Instance != null)
        {
            bool added = InventoryController.Instance.AddItem(
                cropData.harvestItemID,
                amount
            );

            if (added && PickupUI.Instance != null)
            {
                PickupUI.Instance.ShowPickup(
                    cropData.harvestItemID,
                    amount
                );
            }
            Debug.Log(
                "Harvested " +
                cropData.cropName +
                " x" +
                amount
            );
        }

        if (DayManager.Instance != null)
            DayManager.Instance.onNewDay -= Grow;

        Destroy(gameObject);
    }
}