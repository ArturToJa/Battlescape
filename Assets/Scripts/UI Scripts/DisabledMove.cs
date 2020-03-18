using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class DisabledMove : MonoBehaviour
{
    public Button MoveButton;
    
    void Update()
    {
        if (GameRound.instance.currentPlayer.selectedUnit != null)
        {

            if (GameRound.instance.currentPlayer.selectedUnit.CanStillMove() == false)
            {
                MoveButton.interactable = false;
                MoveButton.GetComponentInChildren<Text>().text = "Already moved.";
            }
            else
            {
                if (MoveButton.interactable == false)
                {
                    MoveButton.interactable = true;
                    MoveButton.GetComponentInChildren<Text>().text = "Move!";
                }
            }
        }
    }
}
