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
        if (Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn] != null &&Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players.Count > 0 && Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0] != null)
        {
            CurrentPlayer.text = Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].playerName.ToString() + "'s Turn.";
        }
    }
}
