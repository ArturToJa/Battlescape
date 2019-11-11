using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextPhaseTextScript : MonoBehaviour
{

    public Text NextPhaseText;

    void Update()
    {
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
        {
            NextPhaseText.text = "Next Phase";
        }        
    }
}
