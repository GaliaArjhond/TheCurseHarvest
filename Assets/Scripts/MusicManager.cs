using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource dayMusic;
    public AudioSource nightMusic;
    

    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private float sunsetHour = 18f;
    [SerializeField] private float sunriseHour = 6f;

    private bool wasNight;
    private float masterVolume = 1f;
    void Start()
    {
        if (!ValidateAudioSources())
            return;

        bool isNight = GetCurrentMusicState();
        wasNight = isNight;
        SetMusicState(isNight, true);
    }

    void Update()
    {
        if (!ValidateAudioSources())
            return;

        if (DayNightCycle.Instance == null)
            return;

        bool isNight = GetCurrentMusicState();

        if (isNight != wasNight)
        {
            StopAllCoroutines();

            if (isNight)
                StartCoroutine(CrossFade(dayMusic, nightMusic));
            else
                StartCoroutine(CrossFade(nightMusic, dayMusic));

            wasNight = isNight;
        }
    }

    bool GetCurrentMusicState()
    {
        if (DayNightCycle.Instance == null)
            return false;

        float hour = DayNightCycle.Instance.GetCurrentHour();
        return hour >= sunsetHour || hour < sunriseHour;
    }

    void SetMusicState(bool isNight, bool instant)
    {
        if (!ValidateAudioSources())
            return;

        if (instant)
        {
            if (isNight)
            {
                nightMusic.volume = masterVolume;
                dayMusic.volume = 0f;

                if (!nightMusic.isPlaying)
                    nightMusic.Play();

                if (dayMusic.isPlaying)
                    dayMusic.Stop();
            }
            else
            {
                dayMusic.volume = masterVolume;
                nightMusic.volume = 0f;

                if (!dayMusic.isPlaying)
                    dayMusic.Play();

                if (nightMusic.isPlaying)
                    nightMusic.Stop();
            }

            return;
        }
    }

    public void SetMusicVolume(float volume)
    {
        masterVolume = volume;
    }

    IEnumerator CrossFade(AudioSource from, AudioSource to)
    {
        if (from == null || to == null)
            yield break;

        if (!to.isPlaying)
            to.Play();

        float timer = 0f;

        while (timer < fadeDuration)
        {
            if (from == null || to == null)
                yield break;

            timer += Time.deltaTime;

            float t = timer / fadeDuration;

            from.volume = Mathf.Lerp(masterVolume, 0f, t);
            to.volume = Mathf.Lerp(0f, masterVolume, t);

            yield return null;
        }

        if (from != null)
        {
            from.volume = 0f;
            from.Stop();
        }

        if (to != null)
            to.volume = masterVolume;
    }

    bool ValidateAudioSources()
    {
        if (dayMusic == null || nightMusic == null)
        {
            Debug.LogWarning("MusicManager references are missing. Assign the day/night AudioSources or place them on this object.");
            return false;
        }

        return true;
    }
}