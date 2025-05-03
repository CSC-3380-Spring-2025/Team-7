using UnityEngine;
using System.Collections.Generic;

public class Audios : MonoBehaviour
{
    public static Audios Instance { get; private set; }
    public const string MUSIC_VOLUME_KEY = "MusicVolume";
    public const string SOUND_VOLUME_KEY = "SoundVolume";
    public const float DEFAULT_VOLUME = 1.0f;


    [System.Serializable]
    public class AudioData {
        public string name; 
        public AudioClip clip; 
        public bool isLoop; 
        [Range(0f, 1f)]
        public float defaultVolume = 1f; 
    }

    [SerializeField] private AudioData[] backgroundMusics;
    [SerializeField] private AudioData[] soundEffects;
    public Dictionary<string, AudioSource> musicSources = new Dictionary<string, AudioSource>();
    public Dictionary<string, AudioSource> soundSources = new Dictionary<string, AudioSource>();
    private float currentMusicVolume;
    private float currentSoundVolume;

    // Called when the script instance is first loaded.
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
            LoadVolumes();
            InitializeAudio(); 
        } else {
            Destroy(gameObject);
        }
    }

    // Loads volume settings from PlayerPrefs.
    private void LoadVolumes() {
        currentMusicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, DEFAULT_VOLUME);
        currentSoundVolume = PlayerPrefs.GetFloat(SOUND_VOLUME_KEY, DEFAULT_VOLUME);
        currentMusicVolume = Mathf.Clamp01(currentMusicVolume);
        currentSoundVolume = Mathf.Clamp01(currentSoundVolume);
    }

    // Creates and configures AudioSource components for each audio clip.
    private void InitializeAudio() {
        foreach (AudioData music in backgroundMusics) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = music.clip;
            source.loop = music.isLoop;
            source.volume = currentMusicVolume; 
            source.playOnAwake = false; 
            musicSources.Add(music.name, source); 
        }

        // Create sources for sound effects.
        foreach (AudioData sound in soundEffects) {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.loop = sound.isLoop;
            source.volume = currentSoundVolume;
            source.playOnAwake = false;
            soundSources.Add(sound.name, source);
        }
    }

    // Plays a background music track by name.
    public void PlayMusic(string name){
        if (musicSources.TryGetValue(name, out AudioSource source))
        {
            source.volume = currentMusicVolume;
            source.Play();
        } else {
             Debug.LogWarning($"Music '{name}' not found"); 
        }
    }

    // Plays a sound effect by name.
    public void PlaySound(string name){
         if (soundSources.TryGetValue(name, out AudioSource source))
         {
            source.volume = currentSoundVolume;
            source.Play();
        } else {
             Debug.LogWarning($"Sound '{name}' not found");
        }
    }


    // Sets the current music volume level and applies it to all music sources.
    public void SetMusicVolume(float volume) {
        currentMusicVolume = Mathf.Clamp01(volume);
        foreach (var source in musicSources.Values) {
            source.volume = currentMusicVolume;
        }
    }

    // Sets the current sound effect volume level and applies it to all sound sources.
    public void SetSoundVolume(float volume) {
        currentSoundVolume = Mathf.Clamp01(volume); 
        foreach (var source in soundSources.Values) {
            source.volume = currentSoundVolume;
        }
    }

    // Returns the current music volume level stored in memory.
    public float GetMusicVolume() {
        return currentMusicVolume;
    }

    // Returns the current sound effect volume level stored in memory.
    public float GetSoundVolume() {
        return currentSoundVolume;
    }

     // Example method to stop all currently playing music tracks.
     public void StopAllMusic() {
        foreach(var source in musicSources.Values) {
            source.Stop();
        }
    }
}