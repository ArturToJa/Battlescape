using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class NextPhaseTextScript : MonoBehaviour
{

    public Text NextPhaseText;

    void Update()
    {
        if (GameRound.instance.currentPhase == TurnPhases.Movement)
        {
            NextPhaseText.text = "Next Phase";
        }        
    }
}
