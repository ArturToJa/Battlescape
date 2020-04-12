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
        NetworkingBaseClass.Instance.SendCommandToRetaliate();
    }

    public void No()
    {
        TurnOff();
        NetworkingBaseClass.Instance.SendCommandToNotRetaliate();
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
        Debug.LogError("Comment out by Pieta. Dont know what to do with this yet");
        //hitChanceInfo.TurnOnFor(NetworkingBaseClass.Instance.retaliatingUnit, NetworkingBaseClass.Instance.retaliationTarget);
    }
    public void MouseExitYesButton()
    {
        hitChanceInfo.TurnOff();
    }
}
