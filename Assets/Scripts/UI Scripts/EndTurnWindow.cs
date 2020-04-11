using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class EndTurnWindow : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    public static EndTurnWindow instance;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            yesButton.onClick.AddListener(NetworkingBaseClass.Instance.SendCommandToEndTurnPhase);
            yesButton.onClick.AddListener(TurnOff);
            noButton.onClick.AddListener(TurnOff);
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    void TurnOff()
    {
        UIManager.InstantlyTransitionActivity(gameObject, false);
    }
}
