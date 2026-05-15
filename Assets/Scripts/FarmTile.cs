using UnityEngine;
using UnityEngine.InputSystem;

public class FarmTile : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Sprite tilledSprite;
    [SerializeField] private Sprite wateredSprite;

    [Header("Crop")]
    [SerializeField] private GameObject cropPrefab;

    private SpriteRenderer sr;

    private bool tilled;
    private bool watered;

    private Crop currentCrop;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite = grassSprite;
    }

    public void Interact(Item item)
    {
        // HARVEST
        if (currentCrop != null && currentCrop.CanHarvest())
        {
            Debug.Log("Harvesting crop");

            currentCrop.Harvest();
            currentCrop = null;

            tilled = true;
            watered = false;

            sr.sprite = tilledSprite;

            return;
        }

        if (item == null) return;

        Debug.Log("Using item: " + item.Name);

        // HOE
        if (item.Name == "Hoe")
        {
            TillSoil();
            return;
        }

        // WATER
        if (item.Name == "WateringCan")
        {
            WaterSoil();
            return;
        }

        // SEED
        if (item.itemType == Item.ItemType.Seed)
        {
            PlantSeed(item);
            return;
        }
    }

    void TillSoil()
    {
        if (tilled) return;

        tilled = true;

        sr.sprite = tilledSprite;

        Debug.Log("Soil tilled");
    }

    void WaterSoil()
    {
        if (!tilled) return;

        watered = true;

        sr.sprite = wateredSprite;

        Debug.Log("Soil watered");
    }

    void PlantSeed(Item seedItem)
    {
        if (!tilled) return;

        if (currentCrop != null) return;

        if (seedItem.cropData == null)
        {
            Debug.Log("Seed has no crop data");
            return;
        }

        GameObject cropObj =
            Instantiate(cropPrefab, transform.position, Quaternion.identity);

        currentCrop = cropObj.GetComponent<Crop>();

        currentCrop.Plant(seedItem.cropData);

        Debug.Log("Crop planted");

        // consume seed
        seedItem.amount--;

        seedItem.UpdateAmountText();

        if (seedItem.amount <= 0)
        {
            Slot slot = seedItem.GetComponentInParent<Slot>();

            if (slot != null)
                slot.currentItem = null;

            Destroy(seedItem.gameObject);
        }
    }

    public bool IsWatered()
    {
        return watered;
    }

   public void NewDay()
    {
        bool keepWatered = false;

        if (watered &&
            SkillManager.Instance != null &&
            SkillManager.Instance.water2Unlocked)
        {
            int chance = Random.Range(0, 100);

            if (chance < 25)
                keepWatered = true;
        }

        watered = keepWatered;

        if (watered)
            sr.sprite = wateredSprite;
        else if (tilled)
            sr.sprite = tilledSprite;
    }
}