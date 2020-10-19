using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using BattlescapeUI;

public class RaceChoosingButton : MonoBehaviour
{

    [SerializeField] string raceName;
    [SerializeField] Race race;
    [SerializeField] string raceDescription;
    AMScreen_RaceChoice screen;

    void Start()
    {
        screen = FindObjectOfType<AMScreen_RaceChoice>();
    }


    public void OnHover()
    {
        if (screen == null)
        {
            RaceChoosingManager.instance.raceDescriptionText.text = raceDescription;
            RaceChoosingManager.instance.raceNameText.text = raceName;
        }
        else
        {
            screen.raceDescriptionText.text = raceDescription;
            screen.raceNameText.text = raceName;
        }

    }

    public void ChooseRace()
    {
        Global.instance.armySavingManager.currentSave.SetRace(race);
        screen.forwardButton.gameObject.SetActive(true);
    }
}
