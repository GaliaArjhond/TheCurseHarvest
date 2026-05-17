using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float lifeTime = 0.7f;

    private TextMeshPro text;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
    }

    public void Setup(float damage, bool criticalHit = false)
    {
        text.text = Mathf.CeilToInt(damage).ToString();

        if (criticalHit)
        {
            text.text = "CRIT! " + text.text;

            text.color = Color.red;

            transform.localScale = Vector3.one * 1.4f;
        }

        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.position +=
            Vector3.up * moveSpeed * Time.deltaTime;
    }
}