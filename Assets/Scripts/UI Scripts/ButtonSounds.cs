using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSounds : MonoBehaviour
{
    AudioClip ClickSound;
    AudioClip HoverSound;
    AudioSource ClickSource;
    public AudioSource HoverSource { get; private set; }
    public bool isReal = false;
    void Start()
    {
        if (isReal == false)
        {
            Destroy(this);
            return;
        }
        ClickSound = Resources.Load<AudioClip>("ClickSound");
        HoverSound = Resources.Load<AudioClip>("HoverSound");
        ClickSource = gameObject.AddComponent<AudioSource>();
        ClickSource.clip = ClickSound;
        ClickSource.playOnAwake = false;
        ClickSource.volume = 0.1f;
        HoverSource = gameObject.AddComponent<AudioSource>();
        HoverSource.clip = HoverSound;
        HoverSource.playOnAwake = false;
        HoverSource.volume = 0.05f;
        foreach (Button b in Resources.FindObjectsOfTypeAll<Button>())
        {
            for (int i = 0; i < b.GetComponents<ButtonHover>().Length; i++)
            {
                if (Application.isEditor)
                {
                    DestroyImmediate(b.GetComponent<ButtonHover>(), true);
                }
            }
            if (b.gameObject.GetComponent<ButtonHover>() == null)
            {
                b.gameObject.AddComponent<ButtonHover>();
            }
            b.onClick.AddListener(() => PlaySound(ClickSource));
        }
        foreach (Toggle t in Resources.FindObjectsOfTypeAll<Toggle>())
        {
            if (t.gameObject.GetComponent<ButtonHover>() == null)
            {
                t.gameObject.AddComponent<ButtonHover>();
            }
            t.onValueChanged.AddListener(delegate {
                ToggleValueChanged();
            });
        }

    }



    private void ToggleValueChanged()
    {
        PlaySound(ClickSource);
    }

    public void PlaySound(AudioSource s)
    {
        s.Play();
    }
    void OnMouseEnter()
    {
        Debug.Log("ok");
        PlaySound(HoverSource);
    }
}
