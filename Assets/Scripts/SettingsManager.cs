using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Brightness")]
    [SerializeField] private CanvasGroup brightnessOverlay;

    private Resolution[] resolutions;

    private int pendingResolutionIndex;
    private bool pendingFullscreen;
    private float pendingMusic;
    private float pendingSFX;
    private float pendingBrightness;

    void Start()
    {
        SetupResolutionDropdown();

        pendingResolutionIndex = PlayerPrefs.GetInt(
            "ResolutionIndex",
            resolutionDropdown.value
        );

        pendingFullscreen = PlayerPrefs.GetInt(
            "Fullscreen",
            Screen.fullScreen ? 1 : 0
        ) == 1;

        pendingMusic = PlayerPrefs.GetFloat("MusicVolume", 1f);
        pendingSFX = PlayerPrefs.GetFloat("SFXVolume", 1f);
        pendingBrightness = PlayerPrefs.GetFloat("Brightness", 0f);

        resolutionDropdown.value = pendingResolutionIndex;
        fullscreenToggle.isOn = pendingFullscreen;
        musicSlider.value = pendingMusic;
        sfxSlider.value = pendingSFX;
        brightnessSlider.value = pendingBrightness;

        resolutionDropdown.RefreshShownValue();

        ApplyMusicVolume(pendingMusic);
        ApplySFXVolume(pendingSFX);
        ApplyBrightness(pendingBrightness);

        resolutionDropdown.onValueChanged.AddListener(SetPendingResolution);
        fullscreenToggle.onValueChanged.AddListener(SetPendingFullscreen);
        musicSlider.onValueChanged.AddListener(SetPendingMusic);
        sfxSlider.onValueChanged.AddListener(SetPendingSFX);
        brightnessSlider.onValueChanged.AddListener(SetPendingBrightness);

        ConfirmSettings();
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        List<Resolution> uniqueResolutions = new List<Resolution>();

        int currentIndex = 0;

        foreach (Resolution res in resolutions)
        {
            bool exists = false;

            foreach (Resolution unique in uniqueResolutions)
            {
                if (unique.width == res.width &&
                    unique.height == res.height)
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                uniqueResolutions.Add(res);

                options.Add($"{res.width} x {res.height}");

                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    currentIndex = uniqueResolutions.Count - 1;
                }
            }
        }

        resolutions = uniqueResolutions.ToArray();

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void SetPendingResolution(int index)
    {
        pendingResolutionIndex = index;
    }

    public void SetPendingFullscreen(bool value)
    {
        pendingFullscreen = value;
    }

    public void SetPendingMusic(float value)
    {
        pendingMusic = value;

        // preview immediately
        ApplyMusicVolume(value);
    }

    public void SetPendingSFX(float value)
    {
        pendingSFX = value;

        // preview immediately
        ApplySFXVolume(value);
    }

    public void SetPendingBrightness(float value)
    {
        pendingBrightness = value;

        // preview immediately
        ApplyBrightness(value);
    }

    public void ConfirmSettings()
    {
        if (resolutions != null &&
            pendingResolutionIndex >= 0 &&
            pendingResolutionIndex < resolutions.Length)
        {
            Resolution res = resolutions[pendingResolutionIndex];

            FullScreenMode mode = pendingFullscreen
                ? FullScreenMode.FullScreenWindow
                : FullScreenMode.Windowed;

            Screen.SetResolution(
                res.width,
                res.height,
                mode
            );

            Screen.fullScreenMode = mode;
        }

        ApplyMusicVolume(pendingMusic);
        ApplySFXVolume(pendingSFX);
        ApplyBrightness(pendingBrightness);

        PlayerPrefs.SetInt("ResolutionIndex", pendingResolutionIndex);
        PlayerPrefs.SetInt("Fullscreen", pendingFullscreen ? 1 : 0);
        PlayerPrefs.SetFloat("MusicVolume", pendingMusic);
        PlayerPrefs.SetFloat("SFXVolume", pendingSFX);
        PlayerPrefs.SetFloat("Brightness", pendingBrightness);

        PlayerPrefs.Save();

        Debug.Log("Settings confirmed and saved");
    }

    public void RestoreDefaults()
    {
        pendingResolutionIndex =
            resolutions != null && resolutions.Length > 0
                ? resolutions.Length - 1
                : 0;

        pendingFullscreen = true;
        pendingMusic = 1f;
        pendingSFX = 1f;
        pendingBrightness = 0f;

        if (resolutionDropdown != null)
        {
            resolutionDropdown.value = pendingResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = pendingFullscreen;

        if (musicSlider != null)
            musicSlider.value = pendingMusic;

        if (sfxSlider != null)
            sfxSlider.value = pendingSFX;

        if (brightnessSlider != null)
            brightnessSlider.value = pendingBrightness;

        ApplyMusicVolume(pendingMusic);
        ApplySFXVolume(pendingSFX);
        ApplyBrightness(pendingBrightness);

        Debug.Log("Settings restored to default. Press Confirm to save.");
    }

    void ApplyMusicVolume(float value)
    {
        float db = Mathf.Lerp(-80f, 0f, value);

        bool success = audioMixer.SetFloat("MusicVolume", db);

        Debug.Log($"MusicVolume = {db}, Success = {success}");
    }
    void ApplySFXVolume(float value)
    {
        audioMixer.SetFloat(
            "SFXVolume",
            Mathf.Log10(Mathf.Clamp(value, 0.0001f, 1f)) * 20f
        );
    }
    void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
            brightnessOverlay.alpha = value;
    }
}