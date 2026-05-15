using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class FarmingSystem : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private float toolCooldown = 0.5f;
    [SerializeField] private float impactDelay = 0.25f;

    [Header("References")]
    [SerializeField] private HotbarControler hotbar;

    [Header("Stamina Costs")]
    [SerializeField] private float axeStaminaCost = 8f;
    [SerializeField] private float pickaxeStaminaCost = 8f;
    [SerializeField] private float hoeStaminaCost = 5f;
    [SerializeField] private float wateringStaminaCost = 4f;
    [SerializeField] private float plantingStaminaCost = 2f;
    [SerializeField] private float harvestStaminaCost = 1f;

    private Camera mainCamera;
    private PlayerMovement playerMovement;
    private PlayerStatsManager statsManager;
    private bool isUsingTool = false;

    void Start()
    {
        mainCamera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();
        statsManager = GetComponent<PlayerStatsManager>();

        if (hotbar == null)
            hotbar = FindFirstObjectByType<HotbarControler>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
            TryInteract();
    }

    void TryInteract()
    {
        if (isUsingTool)
        {
            Debug.Log("Still using tool...");
            return;
        }

        if (hotbar == null)
        {
            Debug.Log("Hotbar missing");
            return;
        }

        Item equippedItem = hotbar.GetSelectedItem();

        if (equippedItem == null)
        {
            Debug.Log("No equipped item");
            return;
        }

        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(mouseScreen);

        float dist = Vector2.Distance(transform.position, mouseWorldPos);

        if (dist > interactRange)
        {
            Debug.Log("Too far: " + dist);
            return;
        }

        Vector2 direction = GetDirection(mouseWorldPos);

        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorldPos, 0.25f);

        // CROP HARVEST
        foreach (Collider2D h in hits)
        {
            Crop crop = h.GetComponent<Crop>();

            if (crop != null && crop.CanHarvest())
            {
                if (!UseStamina(harvestStaminaCost))
                    return;

                crop.Harvest();
                return;
            }
        }

        // HARVESTABLE PROP: AXE / PICKAXE
        if (equippedItem.Name == "Axe" || equippedItem.Name == "Pickaxe")
        {
            foreach (Collider2D h in hits)
            {
                HarvestableProp prop = h.GetComponentInParent<HarvestableProp>();

                if (prop != null)
                {
                    float cost = equippedItem.Name == "Axe"
                        ? axeStaminaCost
                        : pickaxeStaminaCost;

                    if (!UseStamina(cost))
                        return;

                    StartCoroutine(UseToolCooldown());

                    if (playerMovement != null)
                    {
                        if (equippedItem.Name == "Axe")
                            playerMovement.PlayAxeAnimation(direction);
                        else
                            playerMovement.PlayPickAxeAnimation(direction);
                    }

                    StartCoroutine(DelayedPropHit(prop, equippedItem.Name));
                    return;
                }
            }

            Debug.Log("No prop clicked.");
            return;
        }

        // FARM TILE: HOE / WATER / SEED
        foreach (Collider2D h in hits)
        {
            FarmTile tile = h.GetComponent<FarmTile>();

            if (tile != null)
            {
                float cost = GetFarmTileStaminaCost(equippedItem);

                if (!UseStamina(cost))
                    return;

                StartCoroutine(UseToolCooldown());

                if (playerMovement != null)
                {
                    if (equippedItem.Name == "Hoe")
                        playerMovement.PlayHoeAnimation(direction);
                    else if (equippedItem.Name == "WateringCan")
                        playerMovement.PlayHoeAnimation(direction);
                }

                if (equippedItem.Name == "WateringCan" &&
                    SkillManager.Instance != null &&
                    SkillManager.Instance.water3Unlocked)
                {
                    WaterNearbyTiles(mouseWorldPos, equippedItem);
                }
                else
                {
                    StartCoroutine(DelayedFarmInteract(tile, equippedItem));
                }
                return;
            }
        }

        Debug.Log("No farm tile clicked.");
    }

    float GetFarmTileStaminaCost(Item item)
    {
        if (item.Name == "Hoe")
            return hoeStaminaCost;

        if (item.Name == "WateringCan")
        {
            float cost = wateringStaminaCost;

            if (SkillManager.Instance != null &&
                SkillManager.Instance.water1Unlocked)
            {
                cost -= 1f;
            }

            return Mathf.Max(1f, cost);
        }

        if (item.itemType == Item.ItemType.Seed)
            return plantingStaminaCost;

        return 0f;
    }

    bool UseStamina(float amount)
    {
        if (amount <= 0f)
            return true;

        if (statsManager == null)
            return true;

        return statsManager.UseStamina(amount);
    }

    Vector2 GetDirection(Vector2 targetPos)
    {
        Vector2 direction = (targetPos - (Vector2)transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return new Vector2(Mathf.Sign(direction.x), 0);

        return new Vector2(0, Mathf.Sign(direction.y));
    }

    IEnumerator UseToolCooldown()
    {
        isUsingTool = true;
        yield return new WaitForSeconds(toolCooldown);
        isUsingTool = false;
    }

    IEnumerator DelayedFarmInteract(FarmTile tile, Item item)
    {
        yield return new WaitForSeconds(impactDelay);

        if (tile != null && item != null)
            tile.Interact(item);
    }

    IEnumerator DelayedPropHit(HarvestableProp prop, string toolName)
    {
        yield return new WaitForSeconds(impactDelay);

        if (prop != null)
            prop.HitProp(toolName);
    }

    void WaterNearbyTiles(Vector2 center, Item item)
    {
        Vector2[] offsets =
        {
            Vector2.zero,
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right
        };

        foreach (Vector2 offset in offsets)
        {
            Collider2D hit =
                Physics2D.OverlapCircle(center + offset, 0.2f);

            if (hit == null)
                continue;

            FarmTile tile = hit.GetComponent<FarmTile>();

            if (tile != null)
            {
                tile.Interact(item);
            }
        }
    }
}