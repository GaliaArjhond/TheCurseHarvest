using UnityEngine;

public class HideWhenBehind : MonoBehaviour
{
    private SpriteRenderer sprite;
    private Color originalColor;

    [SerializeField] private float fadeAlpha = 0.4f;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();

        if (sprite == null)
            sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite != null)
            originalColor = sprite.color;
        else
            Debug.LogWarning("No SpriteRenderer found on " + gameObject.name);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetAlpha(fadeAlpha);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            SetAlpha(1f);
    }

    void SetAlpha(float alpha)
    {
        if (sprite == null) return;

        Color c = sprite.color;
        c.a = alpha;
        sprite.color = c;
    }
}