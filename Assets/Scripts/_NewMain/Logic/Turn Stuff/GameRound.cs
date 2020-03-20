using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BattlescapeLogic
{
    public class GameRound
    {
        //THere is ONE AND ONLY ONE GameRound!


        public LinkedList<ITurnInteractable> newRoundObjects = new LinkedList<ITurnInteractable>();

        public int gameRoundCount { get; private set; }
        public int countdown { get; private set; }
        public int maximumRounds { get; private set; }

        int turnOfPlayerNumber;
        public Player currentPlayer { get; set; }
        Player[] _playerOrder;
        Player[] playerOrder
        {
            get
            {
                if (_playerOrder == null)
                {
                    _playerOrder = SetPlayerOrder();
                }
                return _playerOrder;
            }
        }
        Stack<Player> additionalTurns = new Stack<Player>();


        public TurnPhases currentPhase { get; private set; }
        //Phase to return to after EnemyPhase - Movement or Attack? Most likely Attack, but MAYBE there will be a way to (by ability) get into Enemy Phase from Movement Phase.
        public TurnPhases previousPhase { get; private set; }

        static GameRound _instance;
        public static GameRound instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameRound();
                }
                return _instance;
            }
        }
        private GameRound()
        {
            gameRoundCount = 0;
            countdown = 3;
            maximumRounds = 10;
            turnOfPlayerNumber = -1;
            currentPlayer = null;
            currentPhase = TurnPhases.None;
        }

        public void ResetGameTurn()
        {
            _instance = new GameRound();
        }

        //THIS function most likely works, but can be deffly refactored.
        Player[] SetPlayerOrder()
        {
            int playerCount = Global.instance.GetActivePlayerCount();
            Player[] newPlayerOrder = new Player[playerCount];
            Queue<Player> allPlayers = new Queue<Player>();
            int playerIndex = 0;
            while (allPlayers.Count < newPlayerOrder.Length)
            {
                for (int teamIndex = 0; teamIndex < Global.instance.playerTeams.Count; teamIndex++)
                {
                    if (Global.instance.playerTeams[teamIndex].players[playerIndex].isObserver == false)
                    {
                        allPlayers.Enqueue(Global.instance.playerTeams[teamIndex].players[playerIndex]);
                    }

                }
                playerIndex++;
            }
            for (int i = 0; i < newPlayerOrder.Length; i++)
            {
                newPlayerOrder[i] = allPlayers.Dequeue();
            }
            return newPlayerOrder;

        }

        public void SetPhaseToEnemy()
        {
            previousPhase = currentPhase;
            currentPhase = TurnPhases.Enemy;
            LinkedListNode<ITurnInteractable> element = newRoundObjects.First;
            while (element != null)
            {
                element.Value.OnNewPhase();
                element = element.Next;
            }

        }
        public void ResetPhaseAfterEnemy()
        {
            currentPhase = previousPhase;
            LinkedListNode<ITurnInteractable> element = newRoundObjects.First;
            while (element != null)
            {
                element.Value.OnNewPhase();
                element = element.Next;
            }
        }

        public void OnPressEndButton()
        {
            if (
             (currentPlayer.type == PlayerType.Local) &&
             PlayerInput.instance.isInputBlocked == false &&
             //Some check for ability not being used here 
             gameRoundCount > 0 &&
             InGameInputField.IsNotTypingInChat())
            {
                OnClick();
            }
        }

        public void OnClick()
        {
            if (PlayerInput.instance.isInputBlocked)
            {
                return;
            }
            if (currentPlayer.HasAttacksOrMovesLeft() && PlayerPrefs.GetInt("SkipNextPhaseNotification", 0) == 0)
            {
                UIManager.InstantlyTransitionActivity(EndTurnWindow.instance.gameObject, true);
            }
            else
            {
                Networking.instance.SendCommandToEndTurnPhase();
            }

        }

        public void EndOfPhase()
        {
            switch (currentPhase)
            {
                case TurnPhases.None:
                    NewRound();
                    break;
                case TurnPhases.Movement:
                    NextPhase();
                    break;
                case TurnPhases.Attack:
                    EndOfTurn();
                    break;
                case TurnPhases.Enemy:
                    Debug.LogError("Can't phase-change during retaliation");
                    return;
                default:
                    break;
            }
        }

        void EndOfTurn()
        {
            if (turnOfPlayerNumber == playerOrder.Length - 1)
            {
                NewRound();
            }
            else
            {
                SetNextPlayer();
                NewPlayerTurn();
            }
        }

        void NewRound()
        {
            gameRoundCount++;
            LinkedListNode<ITurnInteractable> element = newRoundObjects.First;
            while (element != null)
            {
                element.Value.OnNewRound();
                element = element.Next;
            }
            SetNextPlayer();
            NewPlayerTurn();
        }

        void NewPlayerTurn()
        {
            Global.instance.currentEntity = currentPlayer;
            LinkedListNode<ITurnInteractable> element = newRoundObjects.First;
            while (element != null)
            {
                element.Value.OnNewTurn();
                element = element.Next;
            }
            NextPhase();
        }

        void NextPhase()
        {
            if (currentPhase == TurnPhases.Movement)
            {
                currentPhase = TurnPhases.Attack;
            }
            else
            {
                currentPhase = TurnPhases.Movement;
            }
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            LinkedListNode<ITurnInteractable> element = newRoundObjects.First;
            while (element != null)
            {
                element.Value.OnNewPhase();
                element = element.Next;
            }
        }

        void SetNextPlayer()
        {
            turnOfPlayerNumber++;
            currentPlayer = GetNextPlayer();

        }

        public Player GetNextPlayer()
        {
            
            //If additional turns are added, use first one of them now.
            if (additionalTurns.Count > 0)
            {
                return additionalTurns.Pop();
            }

            //If its the ned of player list, start from beginning (new round case)

            if (turnOfPlayerNumber == playerOrder.Length)
            {
                turnOfPlayerNumber = 0;
            }

            return playerOrder[turnOfPlayerNumber];

        }

        void AddTurnFor(Player player)
        {
            additionalTurns.Push(player);
        }

        public bool IsGameGoing()
        {
            return gameRoundCount > 0 && gameRoundCount <= maximumRounds;
        }

        public void StartGame()
        {
            //MAYBE we want something 'extra' on a new game idk
            //Its AFTER the positioning phase
            NewRound();
        }
    }

    public enum TurnPhases
    {
        None, Movement, Attack, Enemy, All
    }

}
