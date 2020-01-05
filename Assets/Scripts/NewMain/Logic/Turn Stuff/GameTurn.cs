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


        public LinkedList<INewRound> newRoundObjects = new LinkedList<INewRound>();

        public int gameRoundCount { get; private set; }
        int countdown;
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
        Sound newTurnSound;

        public TurnPhases currentPhase { get; private set; }
        //Phase to return to after EnemyPhase - Movement or Attack? Most likely Attack, but MAYBE there will be a way to (by ability) get into Enemy Phase from Movement Phase.
        public TurnPhases previousPhase { get; private set; }




        //THESE most likely belong outside of this class ^ ^ it will require changes after we make new UI code ;> but this mess WILL disappear ;)
        bool DoesWantToShowPhaseWindow
        {
            get
            {
                return
                    PlayerPrefs.HasKey("SkipNextPhaseNotification") == false ||
                    PlayerPrefs.GetInt("SkipNextPhaseNotification") == 0;
            }
        }
        GameObject _endPhaseWindow;
        GameObject endPhaseWindow
        {
            get
            {
                if (_endPhaseWindow == null)
                {
                    _endPhaseWindow = GameObject.FindGameObjectWithTag("EndPhaseWindow");
                    _endPhaseWindow.GetComponentsInChildren<Button>()[0].onClick.AddListener(Networking.instance.SendCommandToEndTurnPhase);
                    _endPhaseWindow.GetComponentsInChildren<Button>()[0].onClick.AddListener(TurnOffNextPhaseWindow);
                    _endPhaseWindow.GetComponentsInChildren<Button>()[1].onClick.AddListener(TurnOffNextPhaseWindow);
                }
                return _endPhaseWindow;
            }
        }


        NewTurnButton nextPhaseButton
        {
            get
            {
                return GameObject.FindObjectOfType<NewTurnButton>();
            }
        }



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
            newTurnSound = new Sound();
            newTurnSound.clip = Resources.Load<AudioClip>("NewTurnSound");
            newTurnSound.volume = 0.02f;
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
            int index = 0;
            while (allPlayers.Count < newPlayerOrder.Length)
            {
                for (int i = 0; i < Global.instance.playerTeams.Count; i++)
                {
                    if (Global.instance.playerTeams[i].players[index].isObserver == false)
                    {
                        allPlayers.Enqueue(Global.instance.playerTeams[i].players[index]);
                    }
                    
                }
                index++;
            }
            for (int i = 0; i < newPlayerOrder.Length; i++)
            {
                newPlayerOrder[i] = allPlayers.Dequeue();
            }
            return newPlayerOrder;

        }

        public void SetPhaseToEnemy()
        {
            nextPhaseButton.TurnOff();
            previousPhase = currentPhase;
            currentPhase = TurnPhases.Enemy;
        }
        public void ResetPhaseAfterEnemy()
        {
            currentPhase = previousPhase;
            nextPhaseButton.TurnOn();
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
            if (currentPlayer.HasAttacksOrMovesLeft() && DoesWantToShowPhaseWindow)
            {
                UIManager.InstantlyTransitionActivity(endPhaseWindow, true);
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
            if (turnOfPlayerNumber == playerOrder.Length-1)
            {
                NewRound();
            }
            else
            {
                TurnOfPlayer(GetNextPlayer());
            }            
        }

        void NewRound()
        {
            UnitHealth.TurnOnHealthbars();
            nextPhaseButton.TurnOn();
            foreach (INewRound roundObject in newRoundObjects)
            {
                roundObject.OnNewRound();
            }
            gameRoundCount++;
            AddNewRoundPopup();
            TurnOfPlayer(GetNextPlayer());

        }

        void TurnOfPlayer(Player player)
        {
            currentPlayer = player;
            AddNewTurnPopup(player);
            BattlescapeSound.SoundManager.instance.PlaySound(Camera.main.gameObject, newTurnSound);
            NextPhase();
            UnitHealth.SetColour();
        }

        void NextPhase()
        {
            if (currentPhase == TurnPhases.Movement)
            {
                currentPhase = TurnPhases.Attack;
                nextPhaseButton.SetTextTo("End Turn");
            }
            else
            {
                currentPhase = TurnPhases.Movement;
                nextPhaseButton.SetTextTo("Next Phase");
            }
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            MouseManager.instance.unitSelector.DeselectUnit();
            PopupTextController.AddPopupText("Next Phase!", PopupTypes.Info);
            Log.SpawnLog(currentPhase.ToString() + " begins.");
        }

        Player GetNextPlayer()
        {
            if (additionalTurns.Count > 0)
            {
                return additionalTurns.Pop();
            }
            else
            {
                turnOfPlayerNumber++;
                if (turnOfPlayerNumber == playerOrder.Length)
                {
                    turnOfPlayerNumber = 0;
                }
                return playerOrder[turnOfPlayerNumber];
            }
        }

        void AddTurnFor(Player player)
        {
            additionalTurns.Push(player);
        }

        void AddNewTurnPopup(Player player)
        {
            PopupTextController.AddPopupText("New Turn!", PopupTypes.Info);
            Log.SpawnLog("New turn of player: " + player.playerName + ".");
        }

        void AddNewRoundPopup()
        {
            if (gameRoundCount == 1)
            {
                PopupTextController.AddPopupText("Press Escape to see Victory Conditions!", PopupTypes.Info);
                Log.SpawnLog("Prepare for the Battle! Press Escape to see Victory Conditions!");
            }
            else if (gameRoundCount < maximumRounds - countdown)
            {
                PopupTextController.AddPopupText("New Round!", PopupTypes.Info);
                Log.SpawnLog("New round");
            }
            else if (gameRoundCount < maximumRounds)
            {
                PopupTextController.AddPopupText("Remaining rounds: " + (maximumRounds - gameRoundCount).ToString() + "!", PopupTypes.Damage);
                Log.SpawnLog("New round. Remaining rounds: " + (maximumRounds - gameRoundCount).ToString() + ".");
            }
            else if (gameRoundCount == maximumRounds)
            {
                PopupTextController.AddPopupText("Final Turn!", PopupTypes.Damage);
                Log.SpawnLog("The last turn of the game has begun!");
            }
            else
            {
                PopupTextController.AddPopupText("Time is up!", PopupTypes.Stats);
            }
        }

        public bool IsGameGoing()
        {
            return gameRoundCount > 0 && gameRoundCount <= maximumRounds;
        }

        //THIS should also disappear
        void TurnOffNextPhaseWindow()
        {
            UIManager.InstantlyTransitionActivity(endPhaseWindow, false);
        }
    }

    public enum TurnPhases
    {
        None, Movement, Attack, Enemy
    }

}
