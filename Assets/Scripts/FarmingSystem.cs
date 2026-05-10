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

    private Camera mainCamera;
    private PlayerMovement playerMovement;
    private bool isUsingTool = false;

    void Start()
    {
        mainCamera = Camera.main;
        playerMovement = GetComponent<PlayerMovement>();

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

        Vector2 mouseWorldPos =
            mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float dist = Vector2.Distance(transform.position, mouseWorldPos);

        if (dist > interactRange)
        {
            Debug.Log("Too far: " + dist);
            return;
        }

        Vector2 direction = GetDirection(mouseWorldPos);

        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorldPos, 0.25f);

        // AXE / TREE
        if (equippedItem.Name == "Axe")
        {
            foreach (Collider2D h in hits)
            {
                HarvestableProp prop = h.GetComponentInParent<HarvestableProp>();

                if (prop != null)
                {
                    StartCoroutine(UseToolCooldown());

                    if (playerMovement != null)
                        playerMovement.PlayAxeAnimation(direction);

                    StartCoroutine(DelayedPropHit(prop));
                    return;
                }
            }

            Debug.Log("No harvestable prop clicked.");
            return;
        }

        // FARM TILE
        foreach (Collider2D h in hits)
        {
            FarmTile tile = h.GetComponent<FarmTile>();

            if (tile != null)
            {
                StartCoroutine(UseToolCooldown());

                if (playerMovement != null)
                {
                    if (equippedItem.Name == "Hoe")
                        playerMovement.PlayHoeAnimation(direction);
                    else if (equippedItem.Name == "WateringCan")
                        playerMovement.PlayHoeAnimation(direction);
                }

                StartCoroutine(DelayedFarmInteract(tile, equippedItem));
                return;
            }
        }

        Debug.Log("No farm tile clicked.");
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

    IEnumerator DelayedPropHit(HarvestableProp prop)
    {
        yield return new WaitForSeconds(impactDelay);

        if (prop != null)
            prop.HitProp();
    }

    IEnumerator DelayedFarmInteract(FarmTile tile, Item item)
    {
        yield return new WaitForSeconds(impactDelay);

        if (tile != null && item != null)
            tile.Interact(item);
    }
}