using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterruptionTextScript : MonoBehaviour
{
    public Text InterruText;

    void Update()
    {
        if (TurnManager.Instance.CurrentPhase != TurnPhases.Enemy)
        {
            InterruText.text = "Interrupt!";
        }
        else
        {
            InterruText.text = "Finish Interruption";
        }
    }
}
