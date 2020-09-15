using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeSound
{
    public class BackgroundMusic : Sound
    {
        [SerializeField] float swellSpeed;
        public BackgroundMusic()
        {
            isAudioMute = (PlayerPrefs.HasKey("IsAudioMute") && PlayerPrefs.GetInt("IsAudioMute") == 1);
        }
        public void ToggleMute()
        {
            isAudioMute = !isAudioMute;
            int isAudioMuteInt = Convert.ToInt32(isAudioMute);
            PlayerPrefs.SetInt("IsAudioMute", isAudioMuteInt);
            audioSources.First.Value.volume = isAudioMuteInt * 0.2f;
        }

        bool isAudioMute;

        IEnumerator SwellUp(Sound sound, float maxVolume)
        {
            audioSources.First.Value.volume = 0;
            while (audioSources.First.Value.volume < 0.9f * maxVolume && isAudioMute == false)
            {
                audioSources.First.Value.volume = Mathf.Lerp(audioSources.First.Value.volume, maxVolume, swellSpeed * Time.deltaTime);
                yield return null;
            }
            if (isAudioMute == false)
            {
                audioSources.First.Value.volume = maxVolume;
            }

        }
        public IEnumerator SwellDown(AudioSource source)
        {
            while (source.volume > 0.001f)
            {
                source.volume = Mathf.Lerp(source.volume, 0, swellSpeed * 20 * Time.deltaTime);
                yield return null;
            }
            audioSources.First.Value.Stop();
            audioSources.First.Value = null;
        }
    }
}