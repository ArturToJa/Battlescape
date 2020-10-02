using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

namespace BattlescapeSound
{
    public class SoundManager : BattlescapeLogic.TurnChangeMonoBehaviour
    {

        static SoundManager _instance;
        public static SoundManager instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SoundManager>();
                }
                return _instance;
            }
        }


        public bool isAudioMute
        {
            get
            {
                return PlayerPrefs.HasKey("IsAudioMute") && PlayerPrefs.GetInt("IsAudioMute") == 1;
            }
        }
        public Sound selectionSound;
        [SerializeField] Sound newTurnSound;
        [SerializeField] BackgroundMusic[] sceneThemes;
        public BackgroundMusic currentlyPlayedTheme { get; private set; }
        [SerializeField] Sound _logSound;
        public Sound logSound
        {
            get
            {
                return _logSound;
            }
            private set
            {
                _logSound = value;
            }
        }

        

        //its here cause too hard to add it where it should be (no MBs there).
        Dictionary<GameObject, AudioSource> objectsPlayingSounds;

        Dictionary<GameObject, Queue<string>> soundQueues = new Dictionary<GameObject, Queue<string>>();
        [SerializeField] Sound _lobbySound;
        public Sound lobbySound
        {
            get
            {
                return _lobbySound;            
            }
            private set
            {
                _lobbySound = value;
            }
        }

        [SerializeField] float _swellSpeed;
        public float swellSpeed
        {
            get
            {
                return _swellSpeed;
            }
            private set
            {
                _swellSpeed = value;
            }
        }

        protected override void Start()
        {
            if (instance == this)
            {
                base.Start(); 
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogError("Second instance of singleton - Sound Manager!");
            }
        }

        public void PlayThemeFor(string scene)
        {
            List<BackgroundMusic> soundList = GetAllThemesFor(scene);
            BackgroundMusic theme = soundList[UnityEngine.Random.Range(0, soundList.Count)];
            PlayTheme(theme);          
        }

        List<BackgroundMusic> GetAllThemesFor(string scene)
        {
            List<BackgroundMusic> returnList = new List<BackgroundMusic>();
            foreach (BackgroundMusic theme in sceneThemes)
            {
                if (theme.IsThemeOf(scene))
                {
                    returnList.Add(theme);
                }         
            }
            return returnList;
        }


        public void PlaySound(GameObject target, Sound sound)
        {
            Play(target, sound, false);
        }

        public void PlaySoundInLoop(GameObject target, Sound sound)
        {
            Play(target, sound, true);
        }

        void PlayTheme(BackgroundMusic theme)
        {
            if (isAudioMute)
            {
                return;
            }
            if (currentlyPlayedTheme != null)
            {
                StartCoroutine(currentlyPlayedTheme.SwellDown());
            }
            Play(this.gameObject, theme, false);            
            theme.myRoutine = StartCoroutine(theme.SwellUp());
            currentlyPlayedTheme = theme;
            Invoke("PlayTheme", theme.clip.length - swellSpeed);
        }

        void Play(GameObject target, Sound sound, bool isLoop)
        {
            if (isAudioMute)
            {
                return;
            }
            AudioSource source = GetFreeAudioSource(target);
            sound.audioSources.AddLast(source);
            Debug.Log(sound.audioSources.Count);
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
        
        void AddSoundToQueue(GameObject target, string sound)
        {
            if (soundQueues.ContainsKey(target))
            {
                soundQueues[target].Enqueue(sound);
            }
            else
            {
                Queue<string> q = new Queue<string>();
                soundQueues.Add(target, q);
                q.Enqueue(sound);
            }
        }

        void ClearQueue(GameObject target)
        {
            if (soundQueues.ContainsKey(target))
            {
                soundQueues[target].Clear();
            }
        }

        void SetSoundSettingsToSource(AudioSource audioSource, Sound s)
        {
            audioSource.clip = s.clip;
            audioSource.volume = s.volume;
        }


        public void ToggleMute()
        {
            PlayerPrefs.SetInt("IsAudioMute", Convert.ToInt32(!isAudioMute));
            if (isAudioMute == false)
            {
                PlayThemeFor(SceneManager.GetActiveScene().name);
            }
        }


        //Finds unused audio source or creates new if there are none.
        AudioSource GetFreeAudioSource(GameObject target)
        {
            AudioSource source = null;
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

        public override void OnNewRound()
        {
            return;
        }

        public override void OnNewTurn()
        {
            PlaySound(Camera.main.gameObject, newTurnSound);
        }

        public override void OnNewPhase()
        {
            return;
        }
    }
}