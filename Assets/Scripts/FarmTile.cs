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
    private bool watered = false;
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

    public void Interact(Item equippedItem)
    {
        if (equippedItem == null) return;

        if (equippedItem.Name == "Hoe")
        {
            if (!tilled)
                Till();

            return;
        }

        if (equippedItem.Name == "WateringCan")
        {
            if (tilled)
                Water();

            return;
        }

        if (equippedItem.itemType == Item.ItemType.Seed)
        {
            if (tilled && !HasCrop() && equippedItem.cropData != null)
            {
                PlantCrop(equippedItem.cropData);

                equippedItem.amount--;
                equippedItem.UpdateAmountText();

                if (equippedItem.amount <= 0)
                {
                    Slot slot = equippedItem.GetComponentInParent<Slot>();
                    if (slot != null)
                        slot.currentItem = null;

                    Destroy(equippedItem.gameObject);
                }
                ConsumeSeed(equippedItem);
            }

            return;
        }

        Debug.Log("This item can't interact with farm tiles: " + equippedItem.Name);
    }

    void Till()
    {
        tilled = true;
        watered = false;
        sr.sprite = tilledSprite;

        Debug.Log("Ground tilled");
    }

    void Water()
    {
        if (!tilled) return;

        watered = true;
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

        if (curCrop == null)
        {
            Debug.LogError("Crop prefab has no Crop script!");
            Destroy(cropObj);
            return;
        }

        curCrop.Plant(crop, DayManager.Instance.dayNumber);

        if (DayManager.Instance != null)
            DayManager.Instance.onNewDay += OnNewDay;

        Debug.Log("Planted: " + crop.cropName);
    }

    void ConsumeSeed(Item seedItem)
    {
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

    void OnNewDay()
    {
        watered = false;
        sr.sprite = tilledSprite;

        if (curCrop == null)
        {
            tilled = false;
            sr.sprite = grassSprite;

            if (DayManager.Instance != null)
                DayManager.Instance.onNewDay -= OnNewDay;

            if (harvestPrompt != null)
                harvestPrompt.SetActive(false);

            return;
        }

        curCrop.NewDayCheck(DayManager.Instance.dayNumber);

        if (harvestPrompt != null && playerInRange && curCrop.CanHarvest())
            harvestPrompt.SetActive(true);
    }

    bool HasCrop()
    {
        return curCrop != null;
    }
}