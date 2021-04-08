using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

namespace BattlescapeSound
{
    public class SoundManager : MonoBehaviour
    {

        static SoundManager _instance;
        public static SoundManager instance
        {
            get
            {
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
        [SerializeField] Sound _selectionSound;
        public Sound selectionSound
        {
            get
            {
                return _selectionSound;
            }
        }
        [SerializeField] Sound _clickSound;
        public Sound clickSound
        {
            get
            {
                return _clickSound;
            }
        }
        [SerializeField] Sound _hoverSound;
        public Sound hoverSound
        {
            get
            {
                return _hoverSound;
            }
        }
        [SerializeField] Sound _newTurnSound;
        public Sound newTurnSound
        {
            get
            {
                return _newTurnSound;
            }
        }
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
        [SerializeField] Sound winSound;
        [SerializeField] Sound drawSound;
        [SerializeField] Sound loseSound;



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

        void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(this.gameObject);
                GameScore.OnGameEnded += OnGameEnded;
            }
            else
            {
                Destroy(this.gameObject);
                return;
            }
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

        public void PlayTheme()
        {
            PlayTheme(ChooseTheme());
        }

        BackgroundMusic ChooseTheme()
        {
            List<BackgroundMusic> soundList = GetAllThemesFor(SceneManager.GetActiveScene().name);
            BackgroundMusic theme = soundList[UnityEngine.Random.Range(0, soundList.Count)];
            return theme;
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
            Play(this.gameObject, theme, true);            
            theme.myRoutine = StartCoroutine(theme.SwellUp());
            currentlyPlayedTheme = theme;
        }

        void Play(GameObject target, Sound sound, bool isLoop)
        {
            if (isAudioMute)
            {
                return;
            }
            AudioSource source = GetFreeAudioSource(target);
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
                PlayTheme();
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

        Sound GetCorrectEndgameSound(string result)
        {            

            switch (result)
            {
                case "draw":
                    return drawSound;
                case "win":
                    return winSound;
                case "loss":
                    return loseSound;
                default:
                    Debug.LogError("no such option!");
                    return null;
            }
        }

        public void OnGameEnded(string result)
        {
            if (isAudioMute)
            {
                return;
            }
            instance.currentlyPlayedTheme.audioSources.Last.Value.Stop();
            PlaySound(this.gameObject, GetCorrectEndgameSound(result));
        }
    }
}
