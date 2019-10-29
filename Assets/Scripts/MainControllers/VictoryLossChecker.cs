using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryLossChecker : MonoBehaviour
{
    [SerializeField] GameObject WinScreen;
    public static bool isAHeroDead = false;
    [SerializeField] Text GreenPoints;
    [SerializeField] Text RedPoints;


    private void Update()
    {
        UpdateUnitLists();
        if (TurnManager.Instance.TurnCount > 0)
        {

            if (TurnManager.Instance.TurnCount > 1 && TurnManager.Instance.TurnCount > TurnManager.Instance.TurnsInTheGame)
            {
                CalculateWinner();
            }
        }
        UIManager.SmoothlyTransitionActivity(WinScreen, (Player.Players[0].HasWon || Player.Players[1].HasWon), 0.2f);
        GreenPoints.text = Player.Players[0].PlayerScore.ToString();
        RedPoints.text = Player.Players[1].PlayerScore.ToString();
    }

    static void UpdateUnitLists()
    {

        Player.Players[0].PlayerUnits.Clear();
        Player.Players[1].PlayerUnits.Clear();
        foreach (UnitScript unit in FindObjectsOfType<UnitScript>())
        {
            if (unit.isRealUnit == false)
            {
                continue;
            }
            if (unit.CurrentHP > 0)
            {
                Player.Players[unit.PlayerID].PlayerUnits.Add(unit);

            }
        }



    }

    private void CalculateWinner()
    {
        if (Player.Players[0].PlayerScore > Player.Players[1].PlayerScore)
        {
            Player.Players[0].HasWon = true;
        }
        if (Player.Players[1].PlayerScore > Player.Players[0].PlayerScore)
        {
            Player.Players[1].HasWon = true;
        }
        if (Player.Players[0].PlayerScore == Player.Players[1].PlayerScore)
        {
            Player.Players[0].HasWon = true;
            Player.Players[1].HasWon = true;
        }
    }

    public static List<UnitScript> GetMyUnitList()
    {
        UpdateUnitLists();
        return Player.Players[TurnManager.Instance.PlayerToMove].PlayerUnits;
    }
    public static List<UnitScript> GetEnemyUnitList()
    {
        UpdateUnitLists();
        return Player.Players[TurnManager.Instance.OpponentOfActivePlayer].PlayerUnits;
    }

    public static bool HasGameEnded()
    {
        if (Player.Players.ContainsKey(0) == false || Player.Players.ContainsKey(1) == false)
        {
            return false;
        }
        return Player.Players[0].HasWon || Player.Players[1].HasWon;
    }

    public static bool IsGameDrawn()
    {
        return Player.Players[0].HasWon && Player.Players[1].HasWon;
    }

    public static void Clear()
    {
        isAHeroDead = false;
        Player one = Player.Players[0];
        Player two = Player.Players[1];
        PlayerType type = one.Type;
        PlayerColour colour = one.Colour;
        Player.Players[0] = new Player(type, colour);
        type = two.Type;
        colour = two.Colour;
        Player.Players[1] = new Player(type, colour);
    }
}
