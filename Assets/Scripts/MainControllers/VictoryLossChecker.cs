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
    public static bool isGameOver
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

    [SerializeField] BattlescapeSound.Sound winSound;
    [SerializeField] BattlescapeSound.Sound drawSound;
    [SerializeField] BattlescapeSound.Sound loseSound;

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
        UIManager.SmoothlyTransitionActivity(WinScreen, isGameOver, 0.2f);
        if (isGameOver)
        {
            BattlescapeSound.SoundManager.instance.currentlyPlayedTheme.audioSources.Last.Value.Stop();
            BattlescapeSound.SoundManager.instance.PlaySound(this.gameObject, GetCorrectEndgameSound());
        }
        
    }

    BattlescapeSound.Sound GetCorrectEndgameSound()
    {
        if ((Global.instance.playerTeams[0].players[0].type == PlayerType.AI && gameResult == GameResult.GreenWon) || (Global.instance.playerTeams[1].players[0].type == PlayerType.AI && gameResult == GameResult.RedWon))
        {            
            return loseSound;
        }

        if (Global.instance.playerTeams[0].players[0].type == PlayerType.AI || Global.instance.playerTeams[1].players[0].type == PlayerType.AI)
        {
            return winSound;
        }
        else
        {
            if (gameResult == GameResult.Draw)
            {
                return drawSound;
                // draw - in the future change this to a new track
            }
            else if ((GameRound.instance.currentPlayer.team.index == 0 && gameResult == GameResult.GreenWon) || (GameRound.instance.currentPlayer.team.index == 1 && gameResult == GameResult.RedWon))
            {
                return winSound;
            }
            else
            {
                return loseSound;
            }
        }
    }

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

    public static List<Unit> GetMyUnitList()
    {
        return GameRound.instance.currentPlayer.playerUnits;
    }
    public static List<Unit> GetEnemyUnitList()
    {
        return GameRound.instance.GetNextPlayer().playerUnits;
    }    
}
