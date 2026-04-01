using UnityEngine;
using UnityEngine.Rendering.Universal;

public class HorrorLight : MonoBehaviour
{
    [SerializeField] private float minIntensity = 0.3f;
    [SerializeField] private float maxIntensity = 0.6f;
    [SerializeField] private float flickerSpeed = 3f;
    [SerializeField] private bool flickerEnabled = true;

    private Light2D light2D;
    private float targetIntensity;
    private float timer;

    void Start()
    {
        light2D = GetComponent<Light2D>();
        targetIntensity = light2D.intensity;
    }

    void Update()
    {
        if (!flickerEnabled) return;

        timer += Time.deltaTime * flickerSpeed;

        if (timer >= 1f)
        {
            timer = 0f;
            targetIntensity = Random.Range(minIntensity, maxIntensity);
        }

        light2D.intensity = Mathf.Lerp(
            light2D.intensity,
            targetIntensity,
            Time.deltaTime * 10f
        );
    }
}