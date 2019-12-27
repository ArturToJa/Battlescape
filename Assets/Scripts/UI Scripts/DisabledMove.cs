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
        if (MouseManager.instance.selectedUnit != null)
        {

            if (MouseManager.instance.selectedUnit.CanStillMove() == false)
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
