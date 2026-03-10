using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using Object = UnityEngine.Object; // alias used for FindFirstObjectByType

public class SettingsManager : MonoBehaviour
{
    public AudioMixer mainMixer;
    [SerializeField] private Toggle fullscreenToggle;

    void Start()
    {
        int saved = PlayerPrefs.GetInt("SavedFullscreen", Screen.fullScreen ? 1 : 0);
        bool isFullscreen = saved == 1;

        if (fullscreenToggle == null)
        {
            // try to locate a toggle in scene automatically
            // using the newer API to avoid obsolete warning
            fullscreenToggle = Object.FindFirstObjectByType<Toggle>();
        }

        if (fullscreenToggle != null)
        {
            fullscreenToggle.onValueChanged.RemoveAllListeners();
            fullscreenToggle.isOn = isFullscreen;
            // use ToggleFullscreen so we always read the toggle state
            fullscreenToggle.onValueChanged.AddListener((val) => ToggleFullscreen());
        }
        else
        {
            // toggle still null; user must assign manually
        }
        ApplyFullscreen(isFullscreen);
        Screen.SetResolution(1920, 1080, isFullscreen);
    }

    public void ApplyFullscreen(bool isFullscreen)
    {
        if (isFullscreen)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
            Screen.fullScreen = true;
        }
        else
        {
            Screen.fullScreenMode = FullScreenMode.Windowed;
            Screen.fullScreen = false;
            Screen.SetResolution(1920, 1080, false);
        }
        PlayerPrefs.SetInt("SavedFullscreen", isFullscreen ? 1 : 0);
    }

    // helper that ignores the passed value and reads the toggle
    public void ToggleFullscreen()
    {
        bool val = fullscreenToggle != null ? fullscreenToggle.isOn : !Screen.fullScreen;
        ApplyFullscreen(val);
    }

    // Call this from the Toggle's OnValueChanged in Inspector if desired.
    public void OnToggleChanged()
    {
        bool val = fullscreenToggle != null ? fullscreenToggle.isOn : Screen.fullScreen;
        ApplyFullscreen(val);
    }

    public void SetVolume(float volume)
    {
        volume = Mathf.Clamp(volume, 0.0001f, 1f); // prevent log(0)
        mainMixer.SetFloat("volume", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SavedVolume", volume);
    }
}