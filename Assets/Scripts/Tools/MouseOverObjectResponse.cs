using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[System.Serializable]
public class MouseOverObjectResponse
{
    // This script makes the part of MouseManager, where hovering the mouse over stuff make it change colour etc.

    public void Mouseover(GameObject Object)
    {
        if (Object == null)
        {
            return;
        }
        switch (Object.tag)
        {
            case "Unit":
                switch (GameStateManager.Instance.MatchType)
                {
                    case MatchTypes.Online:
                        if (Global.instance.playerTeams[Object.GetComponent<UnitScript>().PlayerID].Players[0].type == PlayerType.Local)
                        {
                            PaintObject(Object, Color.green);
                        }
                        else
                        {
                            PaintObject(Object, Color.red);
                        }
                        break;
                    case MatchTypes.HotSeat:
                        if (Object.GetComponent<UnitScript>().PlayerID == Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].Players[0].team.index)
                        {
                            PaintObject(Object, Color.green);
                        }
                        else
                        {
                            PaintObject(Object, Color.red);
                        }
                        break;
                    case MatchTypes.Singleplayer:
                        if (Global.instance.playerTeams[Object.GetComponent<UnitScript>().PlayerID].Players[0].type == PlayerType.Local)
                        {
                            PaintObject(Object, Color.green);
                        }
                        else
                        {
                            PaintObject(Object, Color.red);
                        }
                        break;
                    default:
                        Debug.LogError("New type of MatchType exists but was not included!");
                        break;
                }
                
                break;
            case "Tile":
                break;
            case "Dice":
                if (Object.layer == 11)
                {
                    PaintObject(Object, Color.red);
                }
                else
                {
                    Debug.LogError("wtf bro why u colour nondestructible");
                }
               
                break;
            default:
                Debug.LogError("Why are we selecting a non-tile non-unit and non-destructible?");
                break;

        }


    }

    public void ClearMouseover(GameObject Object)
    {
        if (Object == null)
        {
            return;
        }
        switch (Object.tag)
        {
            case "Unit":
                PaintObject(Object, Color.white);
                break;
            case "Tile":
                break;
            case "Dice":
                if (Object.layer == 11)
                {
                    PaintObject(Object, Color.white);
                }
                else
                {
                    Debug.LogError("WTF u mad bro why colour nondestructiblethings");
                }
                
                break;
            default:
                Debug.LogError("Why are we deselecting a non-tile non-unit?");
                break;

        }
    }


    public void PaintObject(GameObject Object, Color color)
    {
        Renderer[] rs = Object.GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            Material m = r.material;
            m.color = color;
            r.material = m;
        }
    }
}
