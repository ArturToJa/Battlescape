using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class RaceChoosingButton : MonoBehaviour
{

    [SerializeField] string raceName;
    [SerializeField] Race race;
    [SerializeField] string raceDescription;


    public void OnHover()
    {
        if (ArmyBuilder.instance == null)
        {
            RaceChoosingManager.instance.raceDescriptionText.text = raceDescription;
            RaceChoosingManager.instance.raceNameText.text = raceName;
        }
        else
        {
            ArmyBuilder.instance.RaceDescriptionText.text = raceDescription;
            ArmyBuilder.instance.RaceNameText.text = raceName;
        }

    }

    public void ChooseRace()
    {
        SaveLoadManager.instance.race = race;
        foreach (Transform child in ArmyBuilder.instance.HeroesChoice.transform)
        {
            ArmyBuilder.instance.raceOK.SetActive(true);
        }
    }
}
