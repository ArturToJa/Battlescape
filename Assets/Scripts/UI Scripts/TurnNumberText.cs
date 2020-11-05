using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class TurnNumberText : MonoBehaviour
{
    private TurnChanger turnChanger;
    public Text TurnMumber;
    [SerializeField] float speed = 1;
    bool isRaisingColour = true;

    protected void Start()
    {
        turnChanger = new TurnChanger(OnNewRound, OnNewTurn, OnNewPhase);
        TurnMumber.text = "Drag units to position them!";
        TurnMumber.color = Color.green;
    }

    void ColourChange()
    {
        if (TurnMumber.color.g >= 0.9f && isRaisingColour)
        {
            isRaisingColour = false;
        }
        else if (TurnMumber.color.g <= 0.1f && isRaisingColour == false)
        {
            isRaisingColour = true;
        }
        float g;
        float b;
        if (isRaisingColour)
        {
            b = TurnMumber.color.b + (Time.deltaTime * speed);
            g = TurnMumber.color.g + (Time.deltaTime * speed);
        }
        else
        {
            b = TurnMumber.color.b - (Time.deltaTime * speed);
            g = TurnMumber.color.g - (Time.deltaTime * speed);
        }
        TurnMumber.color = new Color(TurnMumber.color.r, g, b, TurnMumber.color.a);

    }

    public void ResetColour()
    {
        TurnMumber.color = Color.white;
    }

    public void SetTurnNumberText()
    {
        if (GameRound.instance.gameRoundCount > GameRound.instance.maximumRounds)
        {
            TurnMumber.text = "Time is up!";
            TurnMumber.color = Color.yellow;
            return;
        }
        if (GameRound.instance.gameRoundCount <= 0)
        {
            TurnMumber.text = "Drag units to position them!";
            TurnMumber.color = Color.green;
            return;
        }

        if (GameRound.instance.gameRoundCount == GameRound.instance.maximumRounds)
        {
            TurnMumber.text = "Final Round";
            ColourChange();
            return;
        }
        if (GameRound.instance.gameRoundCount >= GameRound.instance.maximumRounds - 5)
        {
            TurnMumber.text = "Rounds to end: " + (GameRound.instance.maximumRounds - GameRound.instance.gameRoundCount).ToString();
            ColourChange();
            return;
        }



        TurnMumber.color = Color.white;
        TurnMumber.text = "Round Number: " + GameRound.instance.gameRoundCount.ToString() + "/" + GameRound.instance.maximumRounds.ToString();

    }

    public void OnNewRound()
    {
        SetTurnNumberText();
    }

    public void OnNewTurn()
    {
        return;
    }

    public void OnNewPhase()
    {
        return;
    }
}
