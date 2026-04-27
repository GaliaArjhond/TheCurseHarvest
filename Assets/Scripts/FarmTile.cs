using UnityEngine;

public class FarmTile : MonoBehaviour
{
    [Header("Sprites")]
    [SerializeField] private Sprite grassSprite;
    [SerializeField] private Sprite tilledSprite;
    [SerializeField] private Sprite wateredSprite;

    [Header("Crop")]
    [SerializeField] private GameObject cropPrefab;

    [Header("Interact Prompt")]
    [SerializeField] private GameObject harvestPrompt;

    private SpriteRenderer sr;
    private Crop curCrop;
    private bool tilled = false;
    private bool playerInRange = false;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (harvestPrompt != null)
            harvestPrompt.SetActive(false);
    }

    void Update()
    {
        if (!playerInRange) return;

        // press E to harvest when ready
        if (UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (HasCrop() && curCrop.CanHarvest())
                curCrop.Harvest();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = true;

        if (harvestPrompt != null && HasCrop() && curCrop.CanHarvest())
            harvestPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        playerInRange = false;

        if (harvestPrompt != null)
            harvestPrompt.SetActive(false);
    }

    // called by FarmingSystem when player uses equipped item on this tile
    public void Interact(Item equippedItem)
    {
        if (equippedItem == null) return;

        if (equippedItem.Name == "Hoe")
        {
            if (!tilled) Till();
        }
        else if (equippedItem.Name == "WateringCan")
        {
            if (tilled) Water();
        }
        else if (equippedItem.itemType == Item.ItemType.Seed)
        {
            if (tilled && !HasCrop() && equippedItem.cropData != null)
                PlantCrop(equippedItem.cropData);
        }
        else
        {
            Debug.Log("This item can't interact with farm tiles: " + equippedItem.Name);
        }
    }

    void Till()
    {
        tilled = true;
        sr.sprite = tilledSprite;
        Debug.Log("Ground tilled");
    }

    void Water()
    {
        sr.sprite = wateredSprite;
        if (HasCrop())
            curCrop.Water();
        Debug.Log("Ground watered");
    }

    void PlantCrop(CropData crop)
    {
        if (cropPrefab == null)
        {
            Debug.LogError("Crop prefab not assigned on FarmTile!");
            return;
        }

        GameObject cropObj = Instantiate(cropPrefab, transform.position, Quaternion.identity);
        curCrop = cropObj.GetComponent<Crop>();
        curCrop.Plant(crop, DayManager.Instance.dayNumber);

        // subscribe to new day
        DayManager.Instance.onNewDay += OnNewDay;
        Debug.Log("Planted: " + crop.cropName);
    }

    void OnNewDay()
    {
        // crop was destroyed (died or harvested)
        if (curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;
            DayManager.Instance.onNewDay -= OnNewDay;

            if (harvestPrompt != null)
                harvestPrompt.SetActive(false);
            return;
        }

        sr.sprite = tilledSprite; // resets to dry after watering
        curCrop.NewDayCheck(DayManager.Instance.dayNumber);

        // show harvest prompt if ready
        if (harvestPrompt != null && playerInRange && curCrop != null && curCrop.CanHarvest())
            harvestPrompt.SetActive(true);
    }

    bool HasCrop() { return curCrop != null; }
}