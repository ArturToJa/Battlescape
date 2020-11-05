using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class HeroNameText : MonoBehaviour
{

    InputField field;

    private void OnEnable()
    {
        field = GetComponentInParent<InputField>();
        StartCoroutine(SetItLater());
    }

    private IEnumerator SetItLater()
    {
        yield return null;
        if (string.IsNullOrEmpty(Global.instance.armySavingManager.currentSave.heroName) == false)
        {
            field.text = Global.instance.armySavingManager.currentSave.heroName;
        }
    }
}
