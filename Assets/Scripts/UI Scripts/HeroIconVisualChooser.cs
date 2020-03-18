using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroIconVisualChooser : MonoBehaviour
{   
    void Update()
    {
        if (SaveLoadManager.instance.race != BattlescapeLogic.Race.Neutral)
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
        transform.GetChild((int)SaveLoadManager.instance.race).gameObject.SetActive(true);
    }
}
