﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;
using UnityEngine.SceneManagement;
using BattlescapeLogic;

public class AudioManager : MonoBehaviour
{
    public bool IsPlayingWind = true;
    bool startedWindAlready = false;
    public static AudioManager Instance;
    public Sound[] sounds;
    [HideInInspector]
    public AudioSource currentMusic;
    public float swellSpeed;
    Coroutine upSwellRoutine;
    public static bool isPlayingGameOverMusic = false;
    float currentSongTime = 0f;
    bool isAudioMute = false;

    // does it work?
    void Awake()
    {
        if (PlayerPrefs.HasKey("IsAudioMute") && PlayerPrefs.GetInt("IsAudioMute") == 1)
        {
            isAudioMute = true;
        }
        else
        {
            isAudioMute = false;
        }
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        foreach (Sound s in sounds)
        {
            s.oldSource = gameObject.AddComponent<AudioSource>();
            s.oldSource.clip = s.clip;
            s.oldSource.volume = s.volume;
            s.oldSource.pitch = s.pitch;
            s.oldSource.loop = s.loop;
        }
    }

    public void Play(string name)
    {
        Sound s = GetSound(name);
        s.oldSource.Play();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && Application.isEditor)
        {
            Debug.Log(currentMusic.clip.name);
        }

        if ((SceneManager.GetActiveScene().name == "_MENU" || SceneManager.GetActiveScene().name == "_ManagementScene" || SceneManager.GetActiveScene().name == "_LobbyScene") && (currentMusic == null || currentMusic.isPlaying == false))
        {
            PlayRandomClipOfType(SoundType.Menu);
        }
        if ((SceneManager.GetActiveScene().name.Contains("_GameScene_") && isPlayingGameOverMusic == false && VictoryLossChecker.isGameOver))
        {
            PlayCorrectEndgameClip();
        }
        else if (VictoryLossChecker.isGameOver == false && (SceneManager.GetActiveScene().name.Contains("_GameScene_")) && (currentMusic == null || currentMusic.isPlaying == false))
        {
            PlayRandomClipOfType(SoundType.Game);
            currentSongTime = 0f;
        }
        currentSongTime += Time.deltaTime;
        if ((SceneManager.GetActiveScene().name.Contains("_GameScene_")) && IsPlayingWind && startedWindAlready == false)
        {
            PlayRandomClipOfType(SoundType.Wind);
            startedWindAlready = true;
        }
    }

    private void PlayCorrectEndgameClip()
    {

        if ((Global.instance.playerTeams[0].players[0].type == PlayerType.AI && VictoryLossChecker.gameResult == GameResult.GreenWon) || (Global.instance.playerTeams[1].players[0].type == PlayerType.AI && VictoryLossChecker.gameResult == GameResult.RedWon))
        {
            PlayEndGameMusic(SoundType.Loose);
            return;
        }



        if (Global.instance.playerTeams[0].players[0].type == PlayerType.AI || Global.instance.playerTeams[1].players[0].type == PlayerType.AI)
        {
            PlayEndGameMusic(SoundType.Win);
        }
        else
        {
            if (VictoryLossChecker.gameResult == GameResult.Draw)
            {
                PlayEndGameMusic(SoundType.Loose);
                // draw - in the future change this to a new track
            }
            else if ((GameRound.instance.currentPlayer.team.index == 0 && VictoryLossChecker.gameResult == GameResult.GreenWon) || (GameRound.instance.currentPlayer.team.index == 1 && VictoryLossChecker.gameResult == GameResult.RedWon))
            {
                PlayEndGameMusic(SoundType.Win);
            }
            else
            {
                PlayEndGameMusic(SoundType.Loose);
            }
        }
    }

    void PlayEndGameMusic(SoundType type)
    {
        isPlayingGameOverMusic = true;
        if (currentMusic == null)
        {
            return;
        }
        currentMusic.Stop();
        List<Sound> soundList = GetSoundsOfType(type);
        Sound sound = soundList[UnityEngine.Random.Range(0, soundList.Count)];
        Play(sound.name);
        upSwellRoutine = StartCoroutine(SwellUp(sound, sound.volume));
        currentMusic = sound.oldSource;
    }

    public Sound GetSound(string name)
    {
        if (name == null)
        {
            Debug.LogError("Sound called: " + name + " does not exist!");
            return null;
        }
        return Array.Find(sounds, sound => sound.name == name);
    }

    List<Sound> GetSoundsOfType(SoundType type)
    {
        List<Sound> returnList = new List<Sound>();
        foreach (Sound s in sounds)
        {
            if (s.soundType == type)
            {
                returnList.Add(s);
            }
        }
        return returnList;
    }
    void PlayRandomClipOfType(SoundType type)
    {
        List<Sound> soundList = GetSoundsOfType(type);
        Sound sound = soundList[UnityEngine.Random.Range(0, soundList.Count)];
        Play(sound.name);
        upSwellRoutine = StartCoroutine(SwellUp(sound, sound.volume));
        if (type != SoundType.Wind)
        {
            currentMusic = sound.oldSource;
        }
    }

    IEnumerator SwellUp(Sound sound, float maxVolume)
    {
        sound.oldSource.volume = 0;
        while (sound.oldSource.volume < 0.9f * maxVolume && isAudioMute == false)
        {
            sound.oldSource.volume = Mathf.Lerp(sound.oldSource.volume, maxVolume, swellSpeed * Time.deltaTime);
            yield return null;
        }
        if (isAudioMute == false)
        {
            sound.oldSource.volume = maxVolume;
        }

    }
    public IEnumerator SwellDown(AudioSource source)
    {
        if (upSwellRoutine != null)
        {
            StopCoroutine(upSwellRoutine);
        }
        while (source.volume > 0.001f)
        {
            source.volume = Mathf.Lerp(source.volume, 0, swellSpeed * 20 * Time.deltaTime);
            yield return null;
        }
        if (source == currentMusic)
        {
            currentMusic.Stop();
            currentMusic = null;
        }
    }

    public void ToggleMute()
    {
        if (isAudioMute == true)
        {
            isAudioMute = false;
            PlayerPrefs.SetInt("IsAudioMute", 0);
            currentMusic.volume = 0.2f;
        }
        else
        {
            isAudioMute = true;
            PlayerPrefs.SetInt("IsAudioMute", 1);
            currentMusic.volume = 0;
        }
    }    
}
