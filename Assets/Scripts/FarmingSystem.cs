using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingSystem : MonoBehaviour
{
    [SerializeField] private float interactRange = 3f;
    [SerializeField] private HotbarControler hotbar;

    private Camera mainCamera;
    private PlayerMovement playerMovement;

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

    private System.Collections.IEnumerator DelayedFarmInteract(FarmTile tile, Item item, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (tile != null && item != null)
            tile.Interact(item);
    }

    private System.Collections.IEnumerator DelayedPropHit(HarvestableProp prop, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (prop != null)
            prop.HitProp();
    }

    void TryInteract()
    {
        Debug.Log("CLICKED");

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

        Debug.Log("Equipped: " + equippedItem.Name);

        Vector2 mouseWorldPos =
            mainCamera.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        float dist = Vector2.Distance(transform.position, mouseWorldPos);

        if (dist > interactRange)
        {
            Debug.Log("Too far: " + dist);
            return;
        }

        Vector2 direction = (mouseWorldPos - (Vector2)transform.position).normalized;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction = new Vector2(Mathf.Sign(direction.x), 0);
        else
            direction = new Vector2(0, Mathf.Sign(direction.y));

        Collider2D[] hits = Physics2D.OverlapCircleAll(mouseWorldPos, 0.25f);

        Debug.Log("Hits: " + hits.Length);

        foreach (Collider2D h in hits)
            Debug.Log("Hit: " + h.name);

        // AXE
        if (equippedItem.Name == "Axe")
        {
            foreach (Collider2D h in hits)
            {
                HarvestableProp prop = h.GetComponentInParent<HarvestableProp>();

                if (prop != null)
                {
                    Debug.Log("Axe hit prop: " + prop.name);

                    if (playerMovement != null)
                        playerMovement.PlayAxeAnimation(direction);

                    StartCoroutine(DelayedPropHit(prop, 0.25f));
                    return;
                }
            }

            Debug.Log("No harvestable prop clicked.");
            return;
        }

        foreach (Collider2D h in hits)
        {
            FarmTile tile = h.GetComponent<FarmTile>();

            if (tile != null)
            {
                Debug.Log("Farm tile clicked.");

                if (playerMovement != null)
                {
                    if (equippedItem.Name == "Hoe")
                    {
                        playerMovement.PlayHoeAnimation(direction);
                    }
                    else if (equippedItem.Name == "WateringCan")
                    {
                        playerMovement.PlayHoeAnimation(direction);
                    }
                }

                StartCoroutine(DelayedFarmInteract(tile, equippedItem, 0.25f));
                return;
            }
        }

        Debug.Log("No farm tile clicked.");
    }
}