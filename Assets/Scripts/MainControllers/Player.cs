using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType { Local, AI, Network }

public enum PlayerColour { Green, Red }

public enum Faction { Human, Elves, Neutral }

[System.Serializable]
public class Player
{
    public static Dictionary<int, Player> Players = new Dictionary<int, Player>();
    public Faction? Race;
    public PlayerType Type;
    public PlayerColour Colour;
    public List<UnitScript> PlayerUnits;
    public int PlayerScore;
    public bool HasWon;
    public int Opponent
    {
        get
        {
            if (this == Players[0])
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
    // ATTENTION - if both players "hasWon" - it is a draw. That is also THE ONLY way to write "draw".

    public Player(PlayerType _type, PlayerColour _colour)
    {
        Type = _type;
        Colour = _colour;
        PlayerUnits = new List<UnitScript>();
        PlayerScore = 0;
        HasWon = false;
        Race = Faction.Human;
    }

    public static bool IsPlayerLocal(int ID)
    {
        return Players[ID].Type == PlayerType.Local;
    }
}
