using UnityEngine;
using System.Collections.Generic;

public class Audios : MonoBehaviour
{
    public static Audios Instance {
        get;
        private set;
    }

    [System.Serializable]
    public class AudioData{
        public string name;
        public AudioClip clip;
        public bool isLoop;
        public float volume = 1f;
    }

    [SerializeField] private AudioData[] backgroundMusics;
    [SerializeField] private AudioData[] soundEffects;
    private Dictionary<string, AudioSource> musics = new Dictionary<string, AudioSource>();
    private Dictionary<string, AudioSource> sounds = new Dictionary<string, AudioSource>();

    private void Awake(){
        if (Instance == null){
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudio();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void InitializeAudio(){
        foreach(AudioData music in backgroundMusics){
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = music.clip;
            source.loop = music.isLoop;
            source.volume = music.volume;
            musics.Add(music.name, source);
        }

        foreach(AudioData sound in soundEffects){
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.clip = sound.clip;
            source.loop = sound.isLoop;
            source.volume = sound.volume;
            sounds.Add(sound.name, source);
        }
    }

    public void PlayMusic(string name){
        if (musics.ContainsKey(name)){
            musics[name].Play();
        }
        else {
            Debug.LogWarning($"Music {name} not found!");
        }
    }

    public void PlaySound(string name){
        if (sounds.ContainsKey(name)){
            sounds[name].Play();
        }
        else {
             Debug.LogWarning($"Sounds {name} not found!");
        }
    }
}
