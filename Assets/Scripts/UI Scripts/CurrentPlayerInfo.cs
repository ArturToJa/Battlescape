using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        if (Player.Players.ContainsKey(TurnManager.Instance.PlayerHavingTurn))
        {
            CurrentPlayer.text = Player.Players[TurnManager.Instance.PlayerHavingTurn].Colour.ToString() + "'s Turn.";
        }
    }
}
