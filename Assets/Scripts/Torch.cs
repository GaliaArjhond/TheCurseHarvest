using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    [Header("References")]
    public Light2D torchLight;

    [Header("Flicker")]
    public float minIntensity = 1.8f;
    public float maxIntensity = 2.3f;
    public float flickerSpeed = 5f;

    private float randomOffset;

    void Awake()
    {
        if (torchLight == null)
            torchLight = GetComponentInChildren<Light2D>();

        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (torchLight == null)
            return;

        bool night = DayNightCycle.Instance != null && DayNightCycle.Instance.IsNight();
        torchLight.enabled = night;

        if (!night)
            return;

        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);

        torchLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}