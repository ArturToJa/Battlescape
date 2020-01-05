using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class TurnNumberText : MonoBehaviour
{

    public Text TurnMumber;
    [SerializeField] float speed = 1;
    bool isRaisingColour = true;
    public bool isOff;

    void Update()
    {
        if (isOff)
        {
            return;
        }
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
            TurnMumber.text = "Final Turn";
            ColourChange();
            return;
        }
        if (GameRound.instance.gameRoundCount >= GameRound.instance.maximumRounds - 5)
        {
            TurnMumber.text = "Turns to end: " + (GameRound.instance.maximumRounds - GameRound.instance.gameRoundCount).ToString();
            ColourChange();
            return;
        }



        TurnMumber.color = Color.white;
        TurnMumber.text = "Turn Number: " + GameRound.instance.gameRoundCount.ToString() + "/" + GameRound.instance.maximumRounds.ToString();

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
}
