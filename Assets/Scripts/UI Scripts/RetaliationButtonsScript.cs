using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class RetaliationButtonsScript : MonoBehaviour
{
    UIHitChanceInformation hitChanceInfo;

    void Start()
    {
        hitChanceInfo = FindObjectOfType<UIHitChanceInformation>();
    }

    public void Yes()
    {
        TurnOff();
        Networking.instance.SendCommandToRetaliate(Networking.instance.retaliatingUnit, Networking.instance.retaliationTarget);
    }

    public void No()
    {
        TurnOff();
        Networking.instance.SendCommandToNotRetaliate();
    }

    public void TurnOn()
    {
        GetComponent<CanvasGroup>().alpha = 1;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        GetComponent<CanvasGroup>().interactable = true;
    }
    public void TurnOff()
    {
        GetComponent<CanvasGroup>().alpha = 0;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
        GetComponent<CanvasGroup>().interactable = false;
    }

    public void MouseEnterYesButton()
    {
        hitChanceInfo.TurnOnFor(Networking.instance.retaliatingUnit, Networking.instance.retaliationTarget);
    }
    public void MouseExitYesButton()
    {
        hitChanceInfo.TurnOff();
    }
}
