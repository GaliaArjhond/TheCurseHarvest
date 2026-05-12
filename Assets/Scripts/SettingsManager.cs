using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SettingsManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider brightnessSlider;

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

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
    }

    void SetupResolutionDropdown()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option =
                resolutions[i].width + " x " + resolutions[i].height;

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentIndex;
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

            Screen.SetResolution(
                res.width,
                res.height,
                pendingFullscreen
            );
        }

        Screen.fullScreen = pendingFullscreen;

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
        if (musicSource != null)
            musicSource.volume = value;
    }

    void ApplySFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }

    void ApplyBrightness(float value)
    {
        if (brightnessOverlay != null)
            brightnessOverlay.alpha = value;
    }
}