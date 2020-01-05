using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BattlescapeLogic;

public class CurrentPlayerInfo : MonoBehaviour
{
    public Text CurrentPlayer;
    public bool isOff = false;

    void Update()
    {
        if (isOff)
        {
            return;
        }
        if (GameRound.instance.currentPlayer != null)
        {
            CurrentPlayer.text = GameRound.instance.currentPlayer.playerName.ToString() + "'s Turn.";
        }
    }
}
