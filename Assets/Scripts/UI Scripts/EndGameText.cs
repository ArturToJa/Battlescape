using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class EndGameText : MonoBehaviour
{
    void Update()
    {
        if (VictoryLossChecker.IsGameOver == false)
        {
            return;
        }
        if (VictoryLossChecker.gameResult == GameResult.Draw)
        {
            DrawText();
        }
        else
        {
            switch (Global.instance.MatchType)
            {
                case MatchTypes.Online:
                    if ((VictoryLossChecker.gameResult == GameResult.GreenWon && Global.instance.playerTeams[0].players[0].type == PlayerType.Local) ||(VictoryLossChecker.gameResult == GameResult.RedWon && Global.instance.playerTeams[1].players[0].type == PlayerType.Local))
                    {
                        WinText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }
                    else
                    {
                        LoseText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }
                    break;

                case MatchTypes.HotSeat:
                    if ((VictoryLossChecker.gameResult == GameResult.GreenWon && TurnManager.Instance.PlayerToMove == 0) ||(VictoryLossChecker.gameResult == GameResult.RedWon && TurnManager.Instance.PlayerToMove == 1))
                    {
                        WinText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }
                    else
                    {
                        LoseText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }                    
                        break;
                case MatchTypes.Singleplayer:
                    if ((VictoryLossChecker.gameResult == GameResult.GreenWon && Global.instance.playerTeams[0].players[0].type == PlayerType.Local) || (VictoryLossChecker.gameResult == GameResult.RedWon && Global.instance.playerTeams[1].players[0].type == PlayerType.Local))
                    {
                        WinText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }
                    else
                    {
                        LoseText(Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0]);
                    }
                    break;
                default:
                    break;
            }            
        }                              
    }

    void DrawText()
    {
        GetComponent<Text>().text = "This battle ended in a draw! ";
        if (VictoryLossChecker.isAnyHeroDead)
        {
            //note that if it is a draw and one hero is dead, both heroes have to be dead!
            GetComponent<Text>().text += "All Heroes are dead!";
        }
        else if (Global.instance.playerTeams[0].players[0].playerUnits.Count == 0 && Global.instance.playerTeams[1].players[0].playerUnits.Count == 0)
        {
            GetComponent<Text>().text += "What a bloodbath!";
        }
        else if (TurnManager.Instance.TurnCount >= TurnManager.Instance.TurnsInTheGame)
        {
            GetComponent<Text>().text += "Nobody managed to win in time!";
        }
    }

    void WinText(Player player)
    {
        GetComponent<Text>().text = "Congratulations, " + player.playerName.ToString() + "! You have won!";
        if (VictoryLossChecker.isAnyHeroDead)
        {
            GetComponent<Text>().text += "\n" + "Enemy Hero is dead!";
        }

        else if (TurnManager.Instance.TurnCount >= TurnManager.Instance.TurnsInTheGame)
        {
            GetComponent<Text>().text += "\n" + "You won on time!";
        }
        else
        {
            Log.SpawnLog("No way!");
            Debug.LogError("no way");
        }
    }

    void LoseText(Player player)
    {
        GetComponent<Text>().text = "You have lost, " + player.playerName.ToString() + "!";
        if (VictoryLossChecker.isAnyHeroDead)
        {
            GetComponent<Text>().text += "\n" + "Your Hero is dead!";
        }
        else if (TurnManager.Instance.TurnCount >= TurnManager.Instance.TurnsInTheGame)
        {
            GetComponent<Text>().text += "\n" + "Your time has ended!";
        }
        else
        {
            Log.SpawnLog("No way!");
            Debug.LogError("no way");
        }
    }


}
