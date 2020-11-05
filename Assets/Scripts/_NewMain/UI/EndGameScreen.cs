using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;
using BattlescapeUI;

namespace BattlescapeUI
{
    public class EndGameScreen : MonoBehaviour
    {
        [SerializeField] Text titleText;
        [SerializeField] Text mainText;
        [SerializeField] Flag [] flags;


        public void ShowVictoryScreen()
        {
            foreach (Flag flag in flags)
            {
                flag.SetFlagToWinner();
            }
            titleText.text = GameResult.instance.winner.players[0].playerName + "'s Team is Victorious!";
            mainText.text = GetTextForWin();
        }

        public void ShowDrawScreen()
        {
            titleText.text = "It Is A Draw!";
            mainText.text = GetTextForDraw();
        }

        public void ShowLossScreen()
        {
            foreach (Flag flag in flags)
            {
                flag.SetFlagToWinner();
            }
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                if (team.HasCurrentLocalPlayer())
                {
                    //current local player exist and his team is reading this, kinda ;/
                    // note, no need to check if this team has lost, if team with current local player WON, we would not be here.

                    titleText.text = team.players[0].playerName + "'s Team has Lost!";
                    mainText.text = GetTextForLoss(team);
                    return;
                }
            }

            //everyone reading this has lost
            titleText.text = "You have all lost!";
            mainText.text = "All teams containing real people have lost the game. We are terribly sorry, the computer has pwned You.";
        }



        string GetTextForDraw()
        {
            string text = "This battle ended in a draw - ";
            if (Global.instance.GetAllUnits().Count == 0)
            {
                text += "what a bloodbath!";
            }
            else if (Global.instance.GetStillPlayingTeamsCount() == 0)
            {
                text += "all Heroes are dead!";
            }
            else
            {
                text += "nobody managed to win in time!";
            }
            return text;
        }

        string GetTextForWin()
        {
            string text = "Congratulations, " + GameResult.instance.winner.players[0].playerName.ToString() + "'s Team! You have won ";
            if (GameRound.instance.gameRoundCount >= GameRound.instance.maximumRounds)
            {
                text += "on time!";
            }
            else
            {
                text += ", as all the enemy Heroes are dead!";
            }
            return text;
        }

        string GetTextForLoss(PlayerTeam team)
        {
            string text = "You have lost, " + team.players[0].playerName.ToString() + "'s Team, ";
            if (team.HasLost())
            {
                text += "as all Your Heroes are dead!";
            }
            else
            {
                text += "as Your time has ended!";
            }
            return text;
        }
    }
}