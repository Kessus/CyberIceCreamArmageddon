using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class SoundInstance
{
    public string clipName;
    public AudioClip clip;
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    [Range(0.1f, 2.0f)]
    public float pitch = 1.0f;
    [HideInInspector]
    public List<AudioSource> sources;
    public bool loop;
    public bool isMusic;
}
public class AudioManager : MonoBehaviour
{
    public List<SoundInstance> availableSounds;
    public static AudioManager Manager { get; private set; }

    private void Awake()
    {
        if(Manager != null)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        Manager = this;

        foreach(SoundInstance sound in availableSounds)
        {
            AddAudioSourceToSoundInstance(sound);
        }
    }

    private AudioSource AddAudioSourceToSoundInstance(SoundInstance sound)
    {
        AudioSource addedSource = gameObject.AddComponent<AudioSource>();
        sound.sources.Add(addedSource);
        addedSource.clip = sound.clip;
        addedSource.volume = sound.volume;
        addedSource.pitch = sound.pitch;
        addedSource.loop = sound.loop;

        return addedSource;
    }

    public int PlaySound(string soundName)
    {
        SoundInstance sound = availableSounds.FirstOrDefault(s => s.clipName == soundName);
        if(sound == null)
        {
            Debug.LogWarning("Audio Manager doesn't contain sound with name: "+soundName);
            return -1;
        }

        //There can only be one music clip active at a given time
        if (sound.isMusic)
        {
            List<SoundInstance> musicSoundInstances = availableSounds.Where(s => s.isMusic).ToList();
            foreach(SoundInstance music in musicSoundInstances)
            {
                for (int i = 0; i < music.sources.Count; i++)
                {
                    StopSound(music.clipName, i);
                }
            }
        }

        AudioSource sourceToPlay = sound.sources.FirstOrDefault(s => !s.isPlaying);
        if (sourceToPlay == null)
            sourceToPlay = AddAudioSourceToSoundInstance(sound);
        sourceToPlay.Play();
        return sound.sources.IndexOf(sourceToPlay);
    }

    public void StopSound(string soundName, int sourceIndex = 0)
    {
        SoundInstance sound = availableSounds.FirstOrDefault(s => s.clipName == soundName);
        AudioSource sourceToStop = sound?.sources[sourceIndex];

        if (sound == null || sourceToStop == null)
        {
            Debug.LogWarning("Audio Manager doesn't contain sound with name: " + soundName);
            return;
        }
        sourceToStop.Stop();
    }
}
