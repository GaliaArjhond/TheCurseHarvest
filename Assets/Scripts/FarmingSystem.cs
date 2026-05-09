using UnityEngine;
using UnityEngine.InputSystem;

public class FarmingSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private HotbarControler hotbar;

    [Header("Settings")]
    [SerializeField] private float interactRange = 3f;

    private Camera cam;

    void Start()
    {
        cam = Camera.main;

        if (hotbar == null)
            hotbar = FindFirstObjectByType<HotbarControler>();
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        if (hotbar == null) return;

        Item selectedItem = hotbar.GetSelectedItem();

        if (selectedItem == null)
        {
            Debug.Log("No selected item");
            return;
        }

        Vector2 mouseWorld =
            cam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        Collider2D hit =
            Physics2D.OverlapCircle(mouseWorld, 0.2f);

        if (hit == null)
        {
            Debug.Log("Nothing clicked");
            return;
        }

        FarmTile tile = hit.GetComponent<FarmTile>();

        if (tile == null)
        {
            Debug.Log("Not a farm tile");
            return;
        }

        float dist =
            Vector2.Distance(transform.position, tile.transform.position);

        if (dist > interactRange)
        {
            Debug.Log("Too far away");
            return;
        }

        tile.Interact(selectedItem);
    }
}