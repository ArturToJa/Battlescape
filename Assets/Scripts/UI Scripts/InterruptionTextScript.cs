using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class InterruptionTextScript : MonoBehaviour
{
    public Text InterruText;

    void Update()
    {
        if (GameRound.instance.currentPhase != TurnPhases.Enemy)
        {
            InterruText.text = "Interrupt!";
        }
        else
        {
            InterruText.text = "Finish Interruption";
        }
    }
}
