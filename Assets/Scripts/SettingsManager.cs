using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;
using System.Collections;

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

        pendingResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);

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

        Debug.Log(Screen.currentResolution);
        Debug.Log(Screen.width + " x " + Screen.height);
    }

    void SetupResolutionDropdown()
    {
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>()
        {
            "1280 x 720",
            "1920 x 1080",
            "2560 x 1440"
        };

        resolutionDropdown.AddOptions(options);

        resolutions = new Resolution[3];

        resolutions[0].width = 1280;
        resolutions[0].height = 720;

        resolutions[1].width = 1920;
        resolutions[1].height = 1080;

        resolutions[2].width = 2560;
        resolutions[2].height = 1440;

        pendingResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 1);

        resolutionDropdown.value = pendingResolutionIndex;
        resolutionDropdown.RefreshShownValue();
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

             Screen.SetResolution(res.width, res.height, mode);
             
             StartCoroutine(CheckResolution());

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
        {
            brightnessOverlay.alpha = 1f - value;
        }
    }

    IEnumerator CheckResolution()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Screen: " + Screen.width + " x " + Screen.height);
        Debug.Log("Current Resolution: " + Screen.currentResolution);
    }
}