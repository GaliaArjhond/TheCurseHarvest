using UnityEngine;

public class Crop : MonoBehaviour
{
    private CropData cropData;

    private int growthDay = 0;

    private SpriteRenderer sr;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void Plant(CropData data)
    {
        cropData = data;

        growthDay = 0;

        sr.sprite = cropData.growProgressSprites[0];

        DayManager.Instance.onNewDay += Grow;
    }

    void Grow()
    {
        growthDay++;

        if (growthDay >= cropData.daysToGrow)
        {
            sr.sprite = cropData.readyToHarvestSprite;

            Debug.Log("Crop ready!");
            return;
        }

        if (growthDay < cropData.growProgressSprites.Length)
        {
            sr.sprite =
                cropData.growProgressSprites[growthDay];
        }
    }
}