using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroNameText : MonoBehaviour {

    InputField field;

    private void OnEnable()
    {
        field = GetComponentInParent<InputField>();
        StartCoroutine(SetItLater());      
    }

    private IEnumerator SetItLater()
    {
        yield return null;
        if (string.IsNullOrEmpty(SaveLoadManager.instance.heroName) == false)
        {
            field.text = SaveLoadManager.instance.heroName;
        }
    }
}
