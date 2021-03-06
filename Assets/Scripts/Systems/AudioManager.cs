using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<String, SoundSO> _sounds = new Dictionary<string, SoundSO>();

    protected AudioManager() {}

    private void Awake()
    {
        foreach (SoundSO s in Resources.LoadAll<SoundSO>("Sounds")){
            _sounds[s.soundName] = s;
            s.AddSource(gameObject.AddComponent<AudioSource>());
        }
    }
    
    public void Play(string soundName){
        if(!_sounds.ContainsKey(soundName)){
            // Debug.LogWarning("Missing Sound:" + soundName);
            return;
        }
        if (_sounds[soundName].overlap || !_sounds[soundName].IsPlaying())
        {
            _sounds[soundName].Play();
        }
    }
    
    public void Stop(string soundName){
        if(!_sounds.ContainsKey(soundName)){
            // Debug.LogWarning("Missing Sound:" + soundName);
            return;
        }
        if (_sounds[soundName].IsPlaying())
        {
            _sounds[soundName].Stop();
        }
    }
}