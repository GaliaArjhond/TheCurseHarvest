using UnityEngine;

public class SortingOrderHandler : MonoBehaviour
{
    private SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        if (sr != null)
            sr.sortingOrder = Mathf.RoundToInt(-transform.position.y * 10);
    }
}