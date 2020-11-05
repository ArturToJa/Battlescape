using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeSound;

public class ButtonSounds : MonoBehaviour
{       
    public bool isReal = false;
    void Start()
    {
        if (isReal == false)
        {
            Destroy(this);
            return;
        }               
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
            b.onClick.AddListener(() => SoundManager.instance.PlaySound(this.gameObject,SoundManager.instance.clickSound));
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
        SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.clickSound);
    }

    
    void OnMouseEnter()
    {
        SoundManager.instance.PlaySound(this.gameObject, SoundManager.instance.hoverSound);
    }
}
