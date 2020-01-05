using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;

namespace BattlescapeSound
{
    public class SoundManager : MonoBehaviour
    {

        public static SoundManager instance;

        bool isAudioMute;
        public Sound[] sounds;
        public Sound selectionSound;

        //its here cause too hard to add it where it should be (no MBs there).
        Dictionary<GameObject, AudioSource> objectsPlayingSounds;

        Dictionary<GameObject, Queue<string>> SoundQueues = new Dictionary<GameObject, Queue<string>>();


        void Start()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogError("Second instance of singleton - Sound Manager!");
            }
        }        


        public void PlaySound(GameObject target, Sound sound)
        {
            Play(target, sound, false);
        }

        public void PlaySoundInLoop(GameObject target, Sound sound)
        {
            Play(target, sound, true);
        }

        void Play(GameObject target, Sound sound, bool isLoop)
        {
            AudioSource source = null;
            source = GetFreeAudioSource(target, source);
            sound.audioSources.AddLast(source);
            SetSoundSettingsToSource(source, sound);
            source.loop = isLoop;
            source.Play();
        }
        public void StopSound(GameObject target, Sound sound)
        {
            AudioSource source = sound.GetSourceFromObject(target);
            source.Stop();
            sound.audioSources.Remove(source);
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
}