using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public enum GameResult { Unfinished, GreenWon, RedWon, Draw }

public class VictoryLossChecker : MonoBehaviour
{
    public static GameResult gameResult { get; set; }
    public static bool IsGameOver
    {
        get
        {
            return
                gameResult == GameResult.GreenWon ||
                gameResult == GameResult.Draw ||
                gameResult == GameResult.RedWon;
        }
    }
    [SerializeField] GameObject WinScreen;
    [SerializeField] Text GreenPoints;
    [SerializeField] Text RedPoints;
    public static bool isAnyHeroDead = false;

    void Start()
    {
        gameResult = GameResult.Unfinished;    
    }
    private void Update()
    {
        //UpdateUnitLists();
        if (GameRound.instance.gameRoundCount > 0)
        {
            GreenPoints.text = Global.instance.playerTeams[0].players[0].playerScore.ToString();
            RedPoints.text = Global.instance.playerTeams[1].players[0].playerScore.ToString();
            if (GameRound.instance.gameRoundCount > 1 && GameRound.instance.gameRoundCount > GameRound.instance.maximumRounds)
            {
                CalculateWinner();
            }
        }
        UIManager.SmoothlyTransitionActivity(WinScreen, IsGameOver, 0.2f);
        
    }

    /*static void UpdateUnitLists()
    {
        Global.instance.playerTeams[0].players[0].playerUnits.Clear();
        Global.instance.playerTeams[1].players[0].playerUnits.Clear();
        foreach (BattlescapeLogic.Unit unit in FindObjectsOfType<BattlescapeLogic.Unit>())
        {
            if (unit.isRealUnit == false)
            {
                continue;
            }
            if (unit.statistics.healthPoints > 0)
            {
                Global.instance.playerTeams[unit.PlayerID].players[0].playerUnits.Add(unit);

            }
        }



    }*/

    private void CalculateWinner()
    {
        if (Global.instance.playerTeams[0].players[0].playerScore > Global.instance.playerTeams[1].players[0].playerScore)
        {
            gameResult = GameResult.GreenWon;
        }
        else if (Global.instance.playerTeams[0].players[0].playerScore < Global.instance.playerTeams[1].players[0].playerScore)
        {
            gameResult = GameResult.RedWon;
        }
        else
        {
            gameResult = GameResult.Draw;
        }        
    }

    public static List<BattlescapeLogic.Unit> GetMyUnitList()
    {
        return GameRound.instance.currentPlayer.playerUnits;
    }
    public static List<BattlescapeLogic.Unit> GetEnemyUnitList()
    {
        return Global.instance.GetNextPlayer(GameRound.instance.currentPlayer).playerUnits;
    }    
}
