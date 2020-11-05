using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

namespace BattlescapeUI
{
    public class Flag : MonoBehaviour
    {
        Image flag;
        Image emblem;
        TurnChanger turnChanger;

        public void OnNewPhase()
        {
            return;
        }

        public void OnNewRound()
        {
            return;
        }

        public void OnNewTurn()
        {
            SetFlagToCurrentPlayer();
        }

        void Awake()
        {
            flag = GetComponent<Image>();
            emblem = GetComponentsInChildren<Image>()[1];
            turnChanger = new TurnChanger(OnNewRound, OnNewTurn, OnNewPhase);
        }

        public void SetFlagToCurrentPlayer()
        {
            flag.sprite = Global.instance.flags[(int)GameRound.instance.currentPlayer.colour];
            emblem.sprite = Global.instance.emblems[(int)GameRound.instance.currentPlayer.race];
        }

       public void SetFlagToWinner()
        {
            flag.sprite = Global.instance.flags[(int)GameResult.instance.winner.players[0].colour];
            emblem.sprite = Global.instance.emblems[(int)GameResult.instance.winner.players[0].race];

        }
    }
}
