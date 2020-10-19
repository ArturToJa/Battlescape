using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BattlescapeSound
{
    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;
        [Range(0.1f, 3f)]
        public float pitch;
        [HideInInspector]
        //DEPRECATED
        public AudioSource oldSource;

        public LinkedList<AudioSource> audioSources = new LinkedList<AudioSource>();
        public bool loop;
        public SoundType soundType;


        public AudioSource GetSourceFromObject(GameObject target)
        {
            foreach (AudioSource source in audioSources)
            {
                if (source.gameObject == target)
                {
                    return source;
                }
            }
            Debug.LogError("No sound source found!");
            return null;
        }
    }
    public enum SoundType
    {
        Menu, Game, Preparation, Win, Loose, Selection, Other, Wind
    }
}