using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeUI;

namespace BattlescapeLogic
{
    public class GameScore : TurnChangeMonoBehaviour
    {
        [SerializeField] EndGameScreen winScreen;
        [SerializeField] Text scoreText;

        [SerializeField] BattlescapeSound.Sound winSound;
        [SerializeField] BattlescapeSound.Sound drawSound;
        [SerializeField] BattlescapeSound.Sound loseSound;

        void Awake()
        {
            GameRound.instance.OnGameStarted += UpdateScoreText;            
            Player.OnScoreChanged += UpdateScoreText;
            Hero.OnHeroDeath += OnHeroDeath;
        }

        void UpdateScoreText()
        {
            scoreText.text = string.Empty;
            foreach (PlayerTeam team in Global.instance.playerTeams)
            {
                scoreText.text += "\n" + "\n" + "Team " + (team.index + 1).ToString() + "\n" + "\n";
                foreach (Player player in team.players)
                {
                    if (player.hasLost == false)
                    {
                        scoreText.text += player.playerName + ": " + player.playerScore + "\n";
                    }
                }
            }
        }

        string GetCorrectResult()
        {
            if (GameResult.instance.isDraw)
            {
                return "draw";
            }
            if (Global.instance.matchType == MatchTypes.HotSeat)
            {
                //in local multiplayer (hotseat) we have a 'win' when a) winning team's member is currently playing and b) winning team has a human in it.
                //ALSO, if a) winning team HAS humans in it and b) currently playing team has NO humans in it (human victory in AI turn), its a 'win'
                //Sadly it needs two separate checks unless im not seeing some easy fix to simplify logic here

                if (GameResult.instance.winner.HasHumanPlayer() == false)
                {
                    return "loss";
                }

                if (GameResult.instance.winner.HasCurrentLocalPlayer() || GameRound.instance.currentPlayer.team.HasHumanPlayer() == false)
                {
                    return "win";
                }
                return "loss";
            }
            else
            {
                //we only have one human local player in the whole game. Lets find him and see if his team is winning.
                if (GameResult.instance.winner.HasCurrentLocalPlayer())
                {
                    return "win";
                }
                else
                {
                    return "loss";
                }
            }
        }

        BattlescapeSound.Sound GetCorrectEndgameSound()
        {
            string result = GetCorrectResult();
            switch (result)
            {
                case "draw":
                    return drawSound;
                case "win":
                    return winSound;
                case "loss":
                    return loseSound;
                default:
                    Debug.LogError("no such option!");
                    return null;
            }            
        }

        void ShowCorrectWinScreenText()
        {
            string result = GetCorrectResult();
            switch (result)
            {
                case "draw":
                    winScreen.ShowDrawScreen();
                    break;
                case "win":
                    winScreen.ShowVictoryScreen();
                    break;
                case "loss":
                    winScreen.ShowLossScreen();
                    break;
                default:
                    Debug.LogError("no such option!");
                    return;
            }
        }        

        public void OnGameEnd()
        {
            BattlescapeSound.SoundManager.instance.currentlyPlayedTheme.audioSources.Last.Value.Stop();
            GameResult.instance.isOver = true;
            FindWinner();
            StartCoroutine(ShowWinScreen());
            ShowCorrectWinScreenText();
            BattlescapeSound.SoundManager.instance.PlaySound(this.gameObject, GetCorrectEndgameSound());

        }
       
        void FindWinner()
        {
            Dictionary<PlayerTeam, int> teamScores = new Dictionary<PlayerTeam, int>();
            for (int i = 0; i < Global.instance.playerTeams.Count; i++)
            {
                teamScores[Global.instance.playerTeams[i]] = 0;
                foreach (Player player in Global.instance.playerTeams[i].players)
                {
                    if (player.hasLost == false)
                    {
                        teamScores[Global.instance.playerTeams[i]] += player.playerScore;
                    }
                }
            }
            int maxValue = -9999;
            PlayerTeam bestTeam = null;
            foreach (var item in teamScores)
            {
                if (item.Value > maxValue && item.Key.HasLost() == false)
                {
                    maxValue = item.Value;
                    bestTeam = item.Key;
                }
            }
            foreach (var item in teamScores)
            {
                if (item.Value == maxValue && item.Key != bestTeam && item.Key.HasLost() == false)
                {
                    GameResult.instance.isDraw = true;
                }
            }
            if (Global.instance.GetStillPlayingTeamsCount() == 0)
            {
                GameResult.instance.isDraw = true;
            }

            if (GameResult.instance.isDraw == false)
            {
                GameResult.instance.winner = bestTeam;
            }
        }

        public void OnHeroDeath(Hero hero)
        {
            UpdateScoreText();
            if (Global.instance.GetStillPlayingTeamsCount() <= 1)
            {
                OnGameEnd();
            }

        }

        IEnumerator ShowWinScreen()
        {
            CanvasGroup canvasGroup = winScreen.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = winScreen.gameObject.AddComponent<CanvasGroup>();
            }
            while (canvasGroup.alpha < 0.9f)
            {
                float Alpha = canvasGroup.alpha;
                float velocity = 0;

                Alpha = Mathf.SmoothDamp(Alpha, 1f, ref velocity, 0.2f);
                canvasGroup.alpha = Alpha;

                canvasGroup.interactable = canvasGroup.alpha > 0.9f;
                canvasGroup.blocksRaycasts = canvasGroup.alpha > 0.9f;
                yield return null;
            }
        }

        public override void OnNewRound()
        {
            if (GameRound.instance.gameRoundCount > GameRound.instance.maximumRounds)
            {
                OnGameEnd();
            }
        }

        public override void OnNewTurn()
        {
            return;
        }

        public override void OnNewPhase()
        {
            return;
        }
    }
}
