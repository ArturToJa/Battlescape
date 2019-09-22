using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndGameText : MonoBehaviour
{
    void Update()
    {
        if (VictoryLossChecker.HasGameEnded() == false)
        {
            return;
        }
        if (VictoryLossChecker.IsGameDrawn())
        {
            DrawText();
        }
        else
        {
            switch (GameStateManager.Instance.MatchType)
            {
                case MatchTypes.Online:
                    for (int i = 0; i < Player.Players.Count; i++)
                    {
                        if (Player.Players[i].Type == PlayerType.Local)
                        {
                            if (Player.Players[i].HasWon)
                            {
                                WinText(i);
                            }
                            else
                            {
                                LoseText(i);
                            }
                        }
                    }
                    break;
                    
                case MatchTypes.HotSeat:
                    for (int i = 0; i < Player.Players.Count; i++)
                    {
                        if (TurnManager.Instance.PlayerToMove == i)
                        {
                            if (Player.Players[i].HasWon)
                            {
                                WinText(i);
                            }
                            else
                            {
                                LoseText(i);
                            }
                        }
                    }
                        break;
                case MatchTypes.Singleplayer:
                    for (int i = 0; i < Player.Players.Count; i++)
                    {
                        if (Player.Players[i].Type == PlayerType.Local)
                        {
                            if (Player.Players[i].HasWon)
                            {
                                WinText(i);
                            }
                            else
                            {
                                LoseText(i);
                            }
                        }
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
        if (VictoryLossChecker.isAHeroDead)
        {
            //note that if it is a draw and one hero is dead, both heroes have to be dead!
            GetComponent<Text>().text += "All Heroes are dead!";
        }
        else if (Player.Players[0].PlayerUnits.Count == 0 && Player.Players[1].PlayerUnits.Count == 0)
        {
            GetComponent<Text>().text += "What a bloodbath!";
        }
        else if (TurnManager.Instance.TurnCount >= TurnManager.Instance.TurnsInTheGame)
        {
            GetComponent<Text>().text += "Nobody managed to win in time!";
        }
    }

    void WinText(int i)
    {
        GetComponent<Text>().text = "Congratulations, " + Player.Players[i].Colour.ToString() + "! You have won!";
        if (VictoryLossChecker.isAHeroDead)
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

    void LoseText(int i)
    {
        GetComponent<Text>().text = "You have lost, " + Player.Players[i].Colour.ToString() + "!";
        if (VictoryLossChecker.isAHeroDead)
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
