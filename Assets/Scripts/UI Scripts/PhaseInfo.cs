using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhaseInfo : MonoBehaviour {

    public Text PhaseInfoText;

    void Update()
    {
        if (TurnManager.Instance.TurnCount <= 0)
        {
            PhaseInfoText.text = "Positioning Phase";
            return;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Enemy)
        {
            PhaseInfoText.text = Player.Players[Player.Players[ TurnManager.Instance.PlayerHavingTurn].Opponent].Colour.ToString() + "'s Responding!";
        }
        else
        {
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
            {
                PhaseInfoText.text = "Movement Phase";
            }
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
            {
                PhaseInfoText.text = "Shooting Phase";
            }
            if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
            {
                PhaseInfoText.text = "Attack Phase";
            }

        }

    }
}
