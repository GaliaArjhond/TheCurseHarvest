using UnityEngine;
using UnityEngine.InputSystem;

public class FarmTileHighlighter : MonoBehaviour
{
    [SerializeField] private GameObject highlight;

    void Update()
    {
        if (highlight == null)
            return;

        if (Camera.main == null)
            return;

        Vector2 mouseWorld =
            Camera.main.ScreenToWorldPoint(
                Mouse.current.position.ReadValue()
            );

        Collider2D hit =
            Physics2D.OverlapCircle(mouseWorld, 0.2f);

        if (hit != null)
        {
            FarmTile tile =
                hit.GetComponent<FarmTile>();

            if (tile != null)
            {
                highlight.SetActive(true);
                highlight.transform.position =
                    tile.transform.position;

                return;
            }
        }

        highlight.SetActive(false);
    }
}