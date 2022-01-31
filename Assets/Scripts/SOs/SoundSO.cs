using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Sound", menuName = "SoundSO")]
public class SoundSO : ScriptableObject
{
    public string soundName;
    public AudioClip clip;
    public List<AudioClip> clipsList;
    [Range(0f,1f)] public float volume = 1f;
    [Range(.1f,3f)] public float pitch = 1f;
    public bool loop;
    public bool overlap;
    
    [HideInInspector]
    public AudioSource source;

    public void AddSource(AudioSource audioSource)
    {
        source = audioSource;
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
        source.loop = loop;
    }

    public bool IsPlaying()
    {
        return !overlap && source.isPlaying;
    }

    public void Play()
    {
        if (clipsList.Count > 0)
        {
            source.clip = clipsList[Random.Range(0, clipsList.Count)];
        }
        source.Play();
    }
    
    public void Stop()
    {
        source.Stop();
    }
}
