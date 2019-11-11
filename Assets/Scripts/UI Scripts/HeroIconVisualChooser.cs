using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIconVisualChooser : MonoBehaviour
{   
    void Update()
    {
        if (SaveLoadManager.Instance.Race != BattlescapeLogic.Faction.Neutral)
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
        transform.GetChild((int)SaveLoadManager.Instance.Race).gameObject.SetActive(true);
    }
}
