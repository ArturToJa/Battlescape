using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

public class NewArmyScript : MonoBehaviour
{
    [SerializeField] Toggle the25toggle;
    [SerializeField] Toggle the50toggle;
    [SerializeField] Button OKbutton;
    string tempName;

    void Update()
    {
        OKbutton.interactable = (string.IsNullOrEmpty(tempName) == false);
        //if (string.IsNullOrEmpty(tempName) == false)
        //{
        //    Debug.LogError("WOOF");
        //}
    }

    public void OK()
    {
        Global.instance.armySavingManager.CreateNewArmy(tempName);

        //if (the25toggle.isOn)
        //{
        //    Global.instance.armySavingManager.currentSave.armySize = 25;
        //}
        //if (the50toggle.isOn)
        //{
        //    Global.instance.armySavingManager.currentSave.armySize = 50;
        //}

        this.gameObject.SetActive(false);
        ArmyManagementScreens.instance.GoForward();
    }

    public void InputText(string text)
    {
        tempName = text;
    }
}
