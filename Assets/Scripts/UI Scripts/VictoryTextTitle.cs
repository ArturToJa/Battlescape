using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;
using UnityEngine.UI;

public class VictoryTextTitle : MonoBehaviour
{

    void Update()
    {
        if (VictoryLossChecker.IsGameOver == false)
        {
            return;
        }
        if (VictoryLossChecker.gameResult == GameResult.GreenWon)
        {
            GetComponent<Text>().text = Global.instance.playerTeams[0].players[0].playerName.ToString() + "'s Victory!";
        }
        if (VictoryLossChecker.gameResult == GameResult.RedWon)
        {
            GetComponent<Text>().text = Global.instance.playerTeams[1].players[0].playerName.ToString() + "'s Victory!";
        }
        if (VictoryLossChecker.gameResult == GameResult.Draw)
        {
            GetComponent<Text>().text = "Draw!";
        }
    }
}
