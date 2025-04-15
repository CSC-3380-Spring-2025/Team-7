using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundSlider;

    void Start()
    {
        
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        soundSlider.value = PlayerPrefs.GetFloat("SoundVolume", 1f);

       
        musicSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        soundSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
    }

    void OnMusicVolumeChanged(float value)
    {
        
        foreach(var music in Audios.Instance.musics.Values)
        {
            music.volume = value;
        }
    }

    void OnSoundVolumeChanged(float value)
    {
        
        foreach(var sound in Audios.Instance.sounds.Values)
        {
            sound.volume = value;
        }
    }
}