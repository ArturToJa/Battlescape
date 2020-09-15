using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;


namespace BattlescapeUI
{
    public class ArmyManagementScreens : MonoBehaviour
    {
        public static ArmyManagementScreens instance { get; private set; }

        [SerializeField] ArmyManagementScreen[] allScreens;
        int currentScreenIndex;
        ArmyManagementScreen currentScreen
        {
            get
            {
                return allScreens[currentScreenIndex];
            }
        }
        

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Debug.LogError("WTF");
                Destroy(this);
            }
        }


        public void OnStart()
        {            
            currentScreenIndex = 0;
            currentScreen.OnChoice();
        }        

        public void GoForward()
        {
            //if next screen exists, go to next screen
            if (currentScreenIndex != allScreens.Length-1)
            {
                currentScreenIndex++;
                currentScreen.OnChoice();
            }
            else
            {
                Global.instance.armySavingManager.OnClickSaveArmy();
                Exit();
                // we finished, so save and go back to menu.
            }
        }
        public void GoBack()
        {
            //like forward but opposite - if we are not on the first screen, go to the previous
            if (currentScreenIndex != 0)
            {
                currentScreenIndex--;
                currentScreen.OnChoice();
            }
            else
            {
                // go back to menu.
                Exit();
            }
        }

        void Exit()
        {
            //i guess just use scene loader to go home?
            Global.instance.armySavingManager.ResetCurrentSaveToNull();
            Networking.instance.SendCommandToLoadScene("_MENU");
        }
    }
}