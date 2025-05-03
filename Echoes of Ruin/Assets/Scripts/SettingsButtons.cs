using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Button applyButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    // Called when the GameObject becomes active/enabled.
    void OnEnable()
    {
        InitializeSliders(); 
        RegisterListeners(); 
    }

    // Called when the GameObject becomes inactive/disabled.
    void OnDisable()
    {
        UnregisterListeners(); 
    }

    // Sets the slider positions to match the current audio volume.
    void InitializeSliders()
    {
        if (Audios.Instance != null && musicSlider != null && soundSlider != null)
        {
            musicSlider.SetValueWithoutNotify(Audios.Instance.GetMusicVolume());
            soundSlider.SetValueWithoutNotify(Audios.Instance.GetSoundVolume());
        }
        else
        {
             if(musicSlider) musicSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("MusicVolume", 1f));
             if(soundSlider) soundSlider.SetValueWithoutNotify(PlayerPrefs.GetFloat("SoundVolume", 1f));
        }
    }

    // Adds functions to be called when UI elements are interacted with.
    void RegisterListeners()
    {
        if (applyButton != null) applyButton.onClick.RemoveListener(OnApplyClick);
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        if (soundSlider != null) soundSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);
        if (applyButton != null) applyButton.onClick.AddListener(OnApplyClick);
        if (musicSlider != null) musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
        if (soundSlider != null) soundSlider.onValueChanged.AddListener(OnSoundSliderChanged);
    }

     // Removes functions assigned to UI element interactions.
     void UnregisterListeners()
    {
        if (applyButton != null) applyButton.onClick.RemoveListener(OnApplyClick);
        if (musicSlider != null) musicSlider.onValueChanged.RemoveListener(OnMusicSliderChanged);
        if (soundSlider != null) soundSlider.onValueChanged.RemoveListener(OnSoundSliderChanged);
    }

    // Called in real-time whenever the music slider's value changes.
    void OnMusicSliderChanged(float value)
    {
        if (Audios.Instance != null)
        {
            Audios.Instance.SetMusicVolume(value);
        }
    }

    // Called in real-time whenever the sound slider's value changes.
    void OnSoundSliderChanged(float value)
    {
        if (Audios.Instance != null)
        {
            Audios.Instance.SetSoundVolume(value);
        }
    }

    // Called when the Apply button is clicked.
    void OnApplyClick()
    {
        if (Audios.Instance == null) return;
        float finalMusicVol = musicSlider.value;
        float finalSoundVol = soundSlider.value;
        PlayerPrefs.SetFloat("MusicVolume", finalMusicVol);
        PlayerPrefs.SetFloat("SoundVolume", finalSoundVol);
        PlayerPrefs.Save();
    }
}