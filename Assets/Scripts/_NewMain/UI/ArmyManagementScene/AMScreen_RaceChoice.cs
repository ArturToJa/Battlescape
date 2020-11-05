using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class AMScreen_RaceChoice : ArmyManagementScreen
    {
        //FOr now there is really not much to do here :D Maybe the structure could be changed so taht more stuff happenned here, but pretty much this screen just is simpel enough that the base class is enough.
        [SerializeField] Text _raceDescriptionText;
        public Text raceDescriptionText
        {
            get
            {
                return _raceDescriptionText;
            }
        }
        [SerializeField] Text _raceNameText;
        public Text raceNameText
        {
            get
            {
                return _raceNameText;
            }
        }

        public override void OnChoice()
        {
            base.OnChoice();
            if (Global.instance.armySavingManager.currentSave.GetRace() != Race.Neutral)
            {
                //we are loading and we can skip this part
                ArmyManagementScreens.instance.GoForward();
            }
        }
    }
}