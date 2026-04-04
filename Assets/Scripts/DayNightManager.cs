using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D globalLight;

    [Header("Time Settings")]
    [SerializeField] private float startHour = 6f;
    [SerializeField] private float endHour = 27f;
    [SerializeField] private float dayDuration = 120f;

    [Header("UI (optional)")]
    [SerializeField] private TextMeshProUGUI clockText;

    private float currentHour;
    private float totalHours;

    void Start()
    {
        currentHour = startHour;
        totalHours = endHour - startHour;
    }

    void Update()
    {
        currentHour += (totalHours / dayDuration) * Time.deltaTime;

        if (currentHour >= endHour)
            currentHour = startHour;

        UpdateLight();

        if (clockText != null)
            clockText.text = GetFormattedTime(currentHour);
    }

    void UpdateLight()
    {
        Color color;
        float intensity;

        // use currentHour — not currentTime
        if (currentHour >= 6f && currentHour < 8f)
        {
            float t = Mathf.InverseLerp(6f, 8f, currentHour);
            color = Color.Lerp(new Color(0.8f, 0.6f, 0.4f), new Color(1f, 0.95f, 0.8f), t);
            intensity = Mathf.Lerp(0.8f, 1f, t);
        }
        else if (currentHour >= 8f && currentHour < 17f)
        {
            color = new Color(1f, 0.98f, 0.9f);
            intensity = 1f;
        }
        else if (currentHour >= 17f && currentHour < 19f)
        {
            float t = Mathf.InverseLerp(17f, 19f, currentHour);
            color = Color.Lerp(new Color(1f, 0.85f, 0.5f), new Color(0.9f, 0.5f, 0.3f), t);
            intensity = Mathf.Lerp(1f, 0.8f, t);
        }
        else if (currentHour >= 19f && currentHour < 21f)
        {
            float t = Mathf.InverseLerp(19f, 21f, currentHour);
            color = Color.Lerp(new Color(0.9f, 0.5f, 0.3f), new Color(0.4f, 0.45f, 0.65f), t);
            intensity = Mathf.Lerp(0.8f, 0.65f, t);
        }
        else
        {
            // night — soft moonlit blue, not too dark
            color = new Color(0.4f, 0.45f, 0.65f);
            intensity = 0.65f;
        }

        globalLight.color = color;
        globalLight.intensity = intensity;
    }

    string GetFormattedTime(float hour)
    {
        float displayHour = hour % 24f;
        int h = Mathf.FloorToInt(displayHour);
        int m = Mathf.FloorToInt((displayHour - h) * 60f);
        string period = h >= 12 ? "PM" : "AM";
        int displayH = h % 12;
        if (displayH == 0) displayH = 12;
        return string.Format("{0}:{1:00} {2}", displayH, m, period);
    }

    public float GetCurrentHour() { return currentHour % 24f; }
    public string GetTimeString() { return GetFormattedTime(currentHour); }
}