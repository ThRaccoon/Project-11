using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class VideoSettingsHelper : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown _resDropDown;        // Dropdown for screen resolutions
    [SerializeField] private Toggle _fullScreenToggle;         // Toggle for fullscreen mode
    [SerializeField] private TMP_Dropdown _fpsDropDown;        // Dropdown for selecting FPS
    [SerializeField] private Toggle _vSyncToggle;              // Toggle for enabling/disabling VSync
    [SerializeField] private Slider _brightnessSlider;
    

    private List<Resolution> _selectedResolutionList = new List<Resolution>(); // Filtered unique resolutions
    private bool _isFullScreen = true;

    private Exposure _exposure;
    private Volume _volume;
    private const float MIN_EXPOSURE = -2.0f;
    private const float MAX_EXPOSURE = 2.0f;


    // Predefined target framerates for the FPS dropdown
    private readonly int[] targetFramerates = { 30, 60, 75, 120, 144, 165, 240 };

    void Awake()
    {
        
        // Load saved settings or default values
        int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
        bool savedFullscreen = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        int savedFpsIndex = PlayerPrefs.GetInt("FpsIndex", 1);
        bool savedVSync = PlayerPrefs.GetInt("VSync", 0) == 1;
        float savedBrightness = PlayerPrefs.GetFloat("Brightness", -1f);

        // Get all available screen resolutions
        Resolution[] allResolutions = Screen.resolutions; //Double semicolon removed
        List<string> resolutionStringList = new List<string>();
        string newRes;
        int currentResolutionIndex = 0;

        // Filter unique resolutions (by width x height)
        for (int i = allResolutions.Length - 1; i >= 0; i--)
        {
            Resolution res = allResolutions[i];
            newRes = res.width + " x " + res.height;

            if (!resolutionStringList.Contains(newRes))
            {
                resolutionStringList.Add(newRes);
                _selectedResolutionList.Add(res);

                // If no saved index, match current screen resolution
                if (savedResolutionIndex == -1 &&
                    res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = _selectedResolutionList.Count - 1;
                }
            }
        }

        // Set up resolution dropdown
        _resDropDown.ClearOptions();
        _resDropDown.AddOptions(resolutionStringList);

        if (savedResolutionIndex != -1 && savedResolutionIndex < _selectedResolutionList.Count)
            currentResolutionIndex = savedResolutionIndex;

        _resDropDown.value = currentResolutionIndex;
        _resDropDown.RefreshShownValue();

        // Apply saved fullscreen setting
        _isFullScreen = savedFullscreen;
        _fullScreenToggle.isOn = _isFullScreen;
        Screen.fullScreen = _isFullScreen;

        // Apply resolution setting
        ApplyResolution(currentResolutionIndex);

        // Set up FPS dropdown
        List<string> fpsOptions = new List<string>();
        foreach (int fps in targetFramerates)
            fpsOptions.Add(fps.ToString());

        _fpsDropDown.ClearOptions();
        _fpsDropDown.AddOptions(fpsOptions);
        _fpsDropDown.value = Mathf.Clamp(savedFpsIndex, 0, targetFramerates.Length - 1);
        _fpsDropDown.RefreshShownValue();

        // Set up VSync toggle
        _vSyncToggle.isOn = savedVSync;
        ApplyVSyncAndFramerate();

        _brightnessSlider.minValue = MIN_EXPOSURE;
        _brightnessSlider.maxValue = MAX_EXPOSURE;
        _brightnessSlider.value = savedBrightness;
        CreateVolume();
        if (_volume != null && _volume.profile.TryGet(out Exposure exposure))
        {
            ApplyBrightness(savedBrightness);
        }
        

        // Register event listeners
        _resDropDown.onValueChanged.AddListener(OnResolutionChanged);
        _fullScreenToggle.onValueChanged.AddListener(OnFullscreenChanged);
        _fpsDropDown.onValueChanged.AddListener(OnFpsChanged);
        _vSyncToggle.onValueChanged.AddListener(OnVSyncChanged);
        _brightnessSlider.onValueChanged.AddListener(OnBrightnessChanged);
    }

    void OnResolutionChanged(int index)
    {
        ApplyResolution(index);
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();
    }

    void OnFullscreenChanged(bool isFullscreen)
    {
        _isFullScreen = isFullscreen;
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
        PlayerPrefs.Save();

        // Reapply current resolution for fullscreen effect to take place
        ApplyResolution(_resDropDown.value);
    }

    void ApplyResolution(int index)
    {
        if (index >= 0 && index < _selectedResolutionList.Count)
        {
            Resolution selected = _selectedResolutionList[index];
            Screen.SetResolution(selected.width, selected.height, _isFullScreen);
        }
    }

    void OnFpsChanged(int index)
    {
        PlayerPrefs.SetInt("FpsIndex", index);
        PlayerPrefs.Save();
        ApplyVSyncAndFramerate();
    }

    void OnVSyncChanged(bool isVSync)
    {
        PlayerPrefs.SetInt("VSync", isVSync ? 1 : 0);
        PlayerPrefs.Save();
        ApplyVSyncAndFramerate();
    }

    void ApplyVSyncAndFramerate()
    {
        if (_vSyncToggle.isOn)
        {
            QualitySettings.vSyncCount = 1;
            Application.targetFrameRate = -1; // VSync takes over
        }
        else
        {
            QualitySettings.vSyncCount = 0;
            int fps = targetFramerates[Mathf.Clamp(_fpsDropDown.value, 0, targetFramerates.Length - 1)];
            Application.targetFrameRate = fps;
        }
    }

    private void OnBrightnessChanged(float value)
    {
        ApplyBrightness(value);
        PlayerPrefs.SetFloat("Brightness", value);
        PlayerPrefs.Save();
    }

    private void ApplyBrightness(float value)
    {
        if (_exposure != null)
        {
            _exposure.fixedExposure.value = Mathf.Clamp(value, MIN_EXPOSURE, MAX_EXPOSURE);
        }
    }

    private void CreateVolume()
    {
        // 1. Create a GameObject
        GameObject volumeObject = new GameObject("RuntimePostFXVolume");

        // 2. Add Volume component
        _volume = volumeObject.AddComponent<Volume>();
        _volume.isGlobal = true;  // Make it affect the whole scene

        // 3. Create a new VolumeProfile
        _volume.profile = ScriptableObject.CreateInstance<VolumeProfile>();

        // 4. Add an Exposure override
        if (!_volume.profile.TryGet(out _exposure))
        {
            _exposure = _volume.profile.Add<Exposure>(true);
        }

        // 5. Configure it
        _exposure.mode.overrideState = true;
        _exposure.mode.value = ExposureMode.Fixed;
        _exposure.fixedExposure.overrideState = true;
        _exposure.fixedExposure.value = 1.2f;
    }
}
