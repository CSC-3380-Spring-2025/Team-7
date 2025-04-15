using UnityEngine;
using UnityEngine.UI;

public class SettingsButtons : MonoBehaviour
{
    [SerializeField] private Button applyButton;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    void Start()
    {
        applyButton.onClick.AddListener(OnApplyClick);
    }

    void OnApplyClick()
    {
        
        PlayerPrefs.SetFloat("MusicVolume", musicSlider.value);
        PlayerPrefs.SetFloat("SoundVolume", soundSlider.value);
        PlayerPrefs.Save();

        
        Debug.Log("Settings saved!");
    }
}