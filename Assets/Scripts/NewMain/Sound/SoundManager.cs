using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{

    public static SoundManager Instance;

    bool isAudioMute;
    public Sound[] sounds;
    Dictionary<GameObject, AudioSource> objectsPlayingSounds;

    Dictionary<GameObject, Queue<string>> SoundQueues = new Dictionary<GameObject, Queue<string>>();


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Second instance of singleton - Sound Manager!");
        }
    }


    public void PlaySound(GameObject target, string sound)
    {
        PlaySound(target, sound, false);
    }

    public void PlaySoundInLoop(GameObject target, string sound)
    {
        PlaySound(target, sound, true);
    }
    public void StopSound(GameObject target, string sound)
    {
        Sound s = GetSound(sound);
        AudioSource source = s.GetSourceFromObject(target);
        source.Stop();
        s.audioSources.Remove(source);
        Destroy(source);
    }

    public float GetSoundDuration(string sound)
    {
        Sound s = GetSound(sound);
        AudioClip clip = s.clip;
        return clip.length;
    }
    // THIESE functions are not needed RN but someone on the team wanted them so here you are:

    /*public float GetCurrentSoundRemainingTime(GameObject target, string sound)
    {

    }*/

    void AddSoundToQueue(GameObject target, string sound)
    {
        if (SoundQueues.ContainsKey(target))
        {
            SoundQueues[target].Enqueue(sound);
        }
        else
        {
            Queue<string> q = new Queue<string>();
            SoundQueues.Add(target, q);
            q.Enqueue(sound);
        }
    }

    void ClearQueue(GameObject target)
    {
        if (SoundQueues.ContainsKey(target))
        {
            SoundQueues[target].Clear();
        }
    }

    Sound GetNextSoundFromObject(GameObject target)
    {
        return GetSound(SoundQueues[target].Dequeue());
    }




    Sound GetSound(string soundName)
    {
        if (soundName == null)
        {
            Debug.LogError("Sound called: " + soundName + " does not exist!");
            return null;
        }
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        return s;
    }

    void SetSoundSettingsToSource(AudioSource audioSource, Sound s)
    {
        audioSource.clip = s.clip;
        audioSource.volume = s.volume;
        audioSource.pitch = s.pitch;
        audioSource.loop = s.loop;
    }

    void PlaySound(GameObject target, string sound, bool isLoop)
    {
        Sound s = GetSound(sound);
        AudioSource source = null;
        source = GetFreeAudioSource(target, source);
        s.audioSources.AddLast(source);
        SetSoundSettingsToSource(source, s);
        source.loop = isLoop;
        source.Play();
    }



    //Finds unused audio source or creates new if there are none.
    AudioSource GetFreeAudioSource(GameObject target, AudioSource source)
    {
        foreach (AudioSource existingSource in target.GetComponents<AudioSource>())
        {
            if (existingSource.isPlaying == false)
            {
                source = existingSource;
            }
        }
        if (source == null)
        {
            source = target.AddComponent<AudioSource>();
        }

        return source;
    }
}
