using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryTextTitle : MonoBehaviour
{

    void Update()
    {
        if (VictoryLossChecker.HasGameEnded() == false)
        {
            return;
        }
        if (Player.Players[0].HasWon && Player.Players[1].HasWon == false)
        {
            GetComponent<Text>().text = Player.Players[0].Colour.ToString() + "'s Victory!";
        }
        if (Player.Players[1].HasWon && Player.Players[0].HasWon == false)
        {
            GetComponent<Text>().text = Player.Players[1].Colour.ToString() + "'s Victory!";
        }
        if (Player.Players[0].HasWon && Player.Players[1].HasWon)
        {
            GetComponent<Text>().text = "Draw!";
        }
    }
}
