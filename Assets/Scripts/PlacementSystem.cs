using UnityEngine;
using UnityEngine.InputSystem;

public class PlacementSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HotbarControler hotbar;

    [SerializeField] private LayerMask blockingLayer;

    [Header("Settings")]
    [SerializeField] private float placeRange = 3f;
    [SerializeField] private Color validColor = new Color(1f, 1f, 1f, 0.5f);
    [SerializeField] private Color invalidColor = new Color(1f, 0f, 0f, 0.5f);

    private Camera mainCamera;
    private GameObject ghostObject;
    private SpriteRenderer ghostRenderer;

    void Start()
    {
        mainCamera = Camera.main;

        if (hotbar == null)
            hotbar = FindFirstObjectByType<HotbarControler>();
    }

    void Update()
    {
        UpdateGhostPreview();

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            TryPlaceObject();
        }
    }

    void UpdateGhostPreview()
    {
        Item item = GetSelectedPlaceableItem();

        if (item == null)
        {
            HideGhost();
            return;
        }

        Vector2 placePos = GetMouseGridPosition();

        if (ghostObject == null)
            CreateGhost(item);

        ghostObject.SetActive(true);
        ghostObject.transform.position = placePos;

        bool valid = CanPlace(placePos);

        if (ghostRenderer != null)
            ghostRenderer.color = valid ? validColor : invalidColor;
    }

    void CreateGhost(Item item)
    {
        GameObject prefab = item.worldPrefab != null ? item.worldPrefab : item.placeablePrefab;
        if (prefab == null)
            return;

        ghostObject = Instantiate(prefab);
        ghostObject.name = "PlacementGhost";

        foreach (Collider2D col in ghostObject.GetComponentsInChildren<Collider2D>())
            col.enabled = false;

        foreach (MonoBehaviour script in ghostObject.GetComponentsInChildren<MonoBehaviour>())
            script.enabled = false;

        ghostRenderer = ghostObject.GetComponentInChildren<SpriteRenderer>();

        if (ghostRenderer != null)
            ghostRenderer.color = validColor;
    }

    void HideGhost()
    {
        if (ghostObject != null)
            ghostObject.SetActive(false);
    }

    void TryPlaceObject()
    {
        Item item = GetSelectedPlaceableItem();

        if (item == null) return;

        Vector2 placePos = GetMouseGridPosition();

        if (!CanPlace(placePos))
        {
            Debug.Log("Cannot place there.");
            return;
        }

        GameObject prefab = item.worldPrefab != null ? item.worldPrefab : item.placeablePrefab;
        if (prefab == null)
        {
            Debug.LogWarning("No prefab assigned for placeable item: " + item.Name);
            return;
        }

        Instantiate(prefab, placePos, Quaternion.identity);

        item.amount--;
        item.UpdateAmountText();

        if (item.amount <= 0)
        {
            Slot slot = item.transform.parent.GetComponent<Slot>();

            if (slot != null)
                slot.currentItem = null;

            Destroy(item.gameObject);
        }

        HideGhost();

        Debug.Log("Placed object.");
    }

    Item GetSelectedPlaceableItem()
    {
        if (hotbar == null) return null;

        Item item = hotbar.GetSelectedItem();

        if (item == null) return null;
        if (!item.isPlaceable) return null;
        if (item.worldPrefab == null && item.placeablePrefab == null) return null;

        return item;
    }

    Vector2 GetMouseGridPosition()
    {
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        mouseScreen.z = Mathf.Abs(mainCamera.transform.position.z);

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mouseScreen);

        return new Vector2(
            Mathf.Round(worldPos.x),
            Mathf.Round(worldPos.y)
        );
    }

    bool CanPlace(Vector2 position)
    {
        float distance = Vector2.Distance(transform.position, position);

        if (distance > placeRange)
            return false;

        Collider2D hit = Physics2D.OverlapCircle(position, 0.35f, blockingLayer);

        if (hit != null)
            return false;

        return true;
    }
}