using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeSound
{
    [System.Serializable]
    public class BackgroundMusic : Sound
    {        
        [SerializeField] string[] _scenesToPlayOn;
        public string[] scenesToPlayOn
        {
            get
            {
                return _scenesToPlayOn;
            }
            private set
            {
                scenesToPlayOn = value;
            }
        }
        public Coroutine myRoutine;

        public BackgroundMusic()
        {          
        }

        

        public IEnumerator SwellUp()
        {
            audioSources.Last.Value.volume = 0;
            while (audioSources.Last.Value.volume < 0.9f * volume && SoundManager.instance.isAudioMute == false)
            {
                audioSources.Last.Value.volume = Mathf.Lerp(audioSources.Last.Value.volume, volume, SoundManager.instance.swellSpeed * Time.deltaTime);
                yield return null;
            }
            if (SoundManager.instance.isAudioMute == false)
            {
                audioSources.Last.Value.volume = volume;
            }

        }
        public IEnumerator SwellDown()
        {
            if (myRoutine != null)
            {
                SoundManager.instance.StopCoroutine(myRoutine);
            }
            while (audioSources.Last.Value.volume > 0.001f)
            {
                audioSources.Last.Value.volume = Mathf.Lerp(audioSources.Last.Value.volume, 0, SoundManager.instance.swellSpeed * 20 * Time.deltaTime);
                yield return null;
            }
            audioSources.Last.Value.Stop();
            audioSources.Last.Value = null;
        }

        public bool IsThemeOf(string scene)
        {
            foreach (string myScene in scenesToPlayOn)
            {
                if (myScene == scene)
                {
                    return true;
                }
            }
            return false;
        }
    }
}