using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BattlescapeSound
{
    [System.Serializable]
    public class Sound
    {
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume;

        public LinkedList<AudioSource> audioSources = new LinkedList<AudioSource>();     


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
}