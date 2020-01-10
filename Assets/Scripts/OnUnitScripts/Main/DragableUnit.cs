using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class DragableUnit : MonoBehaviour
{
    Unit myUnit;

    void Start()
    {
        myUnit = transform.root.GetComponent<Unit>();
    }
    void OnMouseDrag()
    {
        if (GameRound.instance.gameRoundCount > 0)
        {
            return;
        }
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        int tileMask = 1 << 9;
        if (Physics.Raycast(cameraRay,out hitInfo, Mathf.Infinity,tileMask))
        {
            Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
            if ((tile.DropzoneOfPlayer == GameRound.instance.currentPlayer.team.index) && tile.IsWalkable())
            {
                DropZone.Instance.SendCommandToSetUnitPosition(myUnit, tile);
                
            }

        }       
    }

    public static void SetNewPosition(int startPosX, int startPosZ, int endPosX, int endPosZ)
    {             
        Vector3 oldPos = Map.Board[startPosX, startPosZ].transform.position;
        Vector3 newPos = Map.Board[endPosX, endPosZ].transform.position;
        Unit me = Map.Board[startPosX, startPosZ].myUnit;
        if (me.owner.team.index == 0)
        {
            if (NewGameScript.PlayerOneArmy.ContainsKey(oldPos))
            {
                NewGameScript.PlayerOneArmy.Remove(oldPos);
            }            
            NewGameScript.PlayerOneArmy.Add(newPos, me);
        }
        else
        {
            if (NewGameScript.PlayerTwoArmy.ContainsKey(oldPos))
            {
                NewGameScript.PlayerTwoArmy.Remove(oldPos);
            }
            NewGameScript.PlayerTwoArmy.Add(newPos, me);
        }

        Tile tile = Map.Board[endPosX, endPosZ];

        Quaternion q = me.transform.rotation;
        me.transform.position = tile.transform.position;
        me.transform.rotation = q;
        tile.SetMyUnitTo(me);
        me.currentPosition = tile;
        tile.SetMyUnitTo(me);
        me.FaceMiddleOfMap();
    }
}
