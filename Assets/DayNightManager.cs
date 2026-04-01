using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class DayNightCycle : MonoBehaviour
{
    [Header("Light Reference")]
    [SerializeField] private Light2D globalLight;

    [Header("Time Settings")]
    [SerializeField] private float startHour = 6f;       // 6:00 AM
    [SerializeField] private float endHour = 27f;      // 3:00 AM next day = 24+3
    [SerializeField] private float dayDuration = 120f;   // real seconds for full cycle

    [Header("UI (optional)")]
    [SerializeField] private TextMeshProUGUI clockText;

    // current time in hours (6.0 to 27.0)
    private float currentHour;
    private float totalHours;

    // light colors for each time of day
    private Color dawn = new Color(1.0f, 0.75f, 0.5f, 1f);   // warm orange
    private Color morning = new Color(1.0f, 0.95f, 0.8f, 1f);   // soft yellow
    private Color noon = new Color(1.0f, 1.0f, 1.0f, 1f);   // pure white
    private Color afternoon = new Color(1.0f, 0.9f, 0.7f, 1f);   // golden
    private Color sunset = new Color(1.0f, 0.5f, 0.2f, 1f);   // deep orange
    private Color dusk = new Color(0.4f, 0.3f, 0.6f, 1f);   // purple
    private Color night = new Color(0.05f, 0.05f, 0.2f, 1f);   // dark blue
    private Color midnight = new Color(0.02f, 0.02f, 0.1f, 1f);   // near black

    void Start()
    {
        currentHour = startHour;
        totalHours = endHour - startHour; // = 21 hours
    }

    void Update()
    {
        // advance time
        currentHour += (totalHours / dayDuration) * Time.deltaTime;

        // loop back to start when cycle ends
        if (currentHour >= endHour)
            currentHour = startHour;

        // apply light based on current hour
        UpdateLight(currentHour);

        // update clock UI if assigned
        if (clockText != null)
            clockText.text = GetFormattedTime(currentHour);
    }

    void UpdateLight(float hour)
    {
        Color targetColor;
        float targetIntensity;

        if (hour < 7f)        // 6AM-7AM dawn
        {
            float t = Mathf.InverseLerp(6f, 7f, hour);
            targetColor = Color.Lerp(night, dawn, t);
            targetIntensity = Mathf.Lerp(0.2f, 0.7f, t);
        }
        else if (hour < 9f)   // 7AM-9AM morning
        {
            float t = Mathf.InverseLerp(7f, 9f, hour);
            targetColor = Color.Lerp(dawn, morning, t);
            targetIntensity = Mathf.Lerp(0.7f, 1.0f, t);
        }
        else if (hour < 13f)  // 9AM-1PM full day
        {
            float t = Mathf.InverseLerp(9f, 13f, hour);
            targetColor = Color.Lerp(morning, noon, t);
            targetIntensity = 1.0f;
        }
        else if (hour < 17f)  // 1PM-5PM afternoon
        {
            float t = Mathf.InverseLerp(13f, 17f, hour);
            targetColor = Color.Lerp(noon, afternoon, t);
            targetIntensity = 1.0f;
        }
        else if (hour < 19f)  // 5PM-7PM sunset
        {
            float t = Mathf.InverseLerp(17f, 19f, hour);
            targetColor = Color.Lerp(afternoon, sunset, t);
            targetIntensity = Mathf.Lerp(1.0f, 0.8f, t);
        }
        else if (hour < 21f)  // 7PM-9PM dusk
        {
            float t = Mathf.InverseLerp(19f, 21f, hour);
            targetColor = Color.Lerp(sunset, dusk, t);
            targetIntensity = Mathf.Lerp(0.8f, 0.4f, t);
        }
        else if (hour < 24f)  // 9PM-12AM night
        {
            float t = Mathf.InverseLerp(21f, 24f, hour);
            targetColor = Color.Lerp(dusk, night, t);
            targetIntensity = Mathf.Lerp(0.4f, 0.15f, t);
        }
        else                  // 12AM-3AM deep night
        {
            float t = Mathf.InverseLerp(24f, 27f, hour);
            targetColor = Color.Lerp(night, midnight, t);
            targetIntensity = Mathf.Lerp(0.15f, 0.05f, t);
        }

        globalLight.color = targetColor;
        globalLight.intensity = targetIntensity;
    }

    string GetFormattedTime(float hour)
    {
        // wrap hour back to 0-24 range for display
        float displayHour = hour % 24f;
        int h = Mathf.FloorToInt(displayHour);
        int m = Mathf.FloorToInt((displayHour - h) * 60f);
        string period = h >= 12 ? "PM" : "AM";
        int displayH = h % 12;
        if (displayH == 0) displayH = 12;
        return string.Format("{0}:{1:00} {2}", displayH, m, period);
    }

    // call this from other scripts to get current time
    public float GetCurrentHour() { return currentHour % 24f; }
    public string GetTimeString() { return GetFormattedTime(currentHour); }
}