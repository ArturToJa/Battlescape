using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class HeroIconVisualChooser : MonoBehaviour
{   
    void Update()
    {
        if (Global.instance.armySavingManager.currentSave.GetRace() != BattlescapeLogic.Race.Neutral)
        {
            SetCorrectVisual();
        }
    }

    void SetCorrectVisual()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
        transform.GetChild((int)Global.instance.armySavingManager.currentSave.GetRace()).gameObject.SetActive(true);
    }
}
