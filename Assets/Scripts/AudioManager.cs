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
    public AudioSource source;
    public bool loop;
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
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
        }
    }

    public void PlaySound(string soundName)
    {
        SoundInstance sound = availableSounds.FirstOrDefault(s => s.clipName == soundName);
        if(sound == null)
        {
            Debug.LogError("Audio Manager doesn't contain sound with name: "+soundName);
            return;
        }

        sound.source.Play();
    }

    public void StopSound(string soundName)
    {
        SoundInstance sound = availableSounds.FirstOrDefault(s => s.clipName == soundName);
        if (sound == null)
        {
            Debug.LogError("Audio Manager doesn't contain sound with name: " + soundName);
            return;
        }

        sound.source.Stop();
    }
}
