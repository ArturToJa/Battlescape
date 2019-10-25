using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class DragableUnit : MonoBehaviour
{  
    void OnMouseDrag()
    {
        if (GameStateManager.Instance.IsItPreGame() == false)
        {
            return;
        }
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        int tileMask = 1 << 9;
        if (Physics.Raycast(cameraRay,out hitInfo, Mathf.Infinity,tileMask))
        {
            Tile tile = hitInfo.transform.gameObject.GetComponent<Tile>();
            if (tile.isDropzoneOfPlayer[TurnManager.Instance.PlayerToMove] && tile.IsWalkable())
            {
                if (GameStateManager.Instance.MatchType == MatchTypes.Online)
                {
                    FindObjectOfType<DropZone>().GetComponent<PhotonView>().RPC("RPCSetUnitPosition", PhotonTargets.All, Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.z), Mathf.RoundToInt(hitInfo.transform.position.x), Mathf.RoundToInt(hitInfo.transform.position.z));
                }
                else
                {
                    int startX = Mathf.RoundToInt(transform.position.x);
                    int startZ = Mathf.RoundToInt(transform.position.z);
                    int endX = Mathf.RoundToInt(hitInfo.transform.position.x);
                    int endZ = Mathf.RoundToInt(hitInfo.transform.position.z);
                    //UnitPositionKeeper.Instance.photonView.RPC("RPCMoveUnit", PhotonTargets.All, Map.Board[startX, startZ].myUnit, endX, endZ);
                    SetNewPosition(startX, startZ, endX, endZ);
                }
                
            }

        }       
    }

    public static void SetNewPosition(int startPosX, int startPosZ, int endPosX, int endPosZ)
    {       
        Vector3 oldPos = Map.Board[startPosX, startPosZ].transform.position;
        Vector3 newPos = Map.Board[endPosX, endPosZ].transform.position;
        Unit unit = Map.Board[startPosX, startPosZ].myUnit.unitUnit;
        UnitScript me = Map.Board[startPosX, startPosZ].myUnit;
        if (me.PlayerID == 0)
        {
            if (NewGameScript.PlayerOneArmy.ContainsKey(oldPos))
            {
                NewGameScript.PlayerOneArmy.Remove(oldPos);
            }            
            NewGameScript.PlayerOneArmy.Add(newPos, unit);
        }
        else
        {
            if (NewGameScript.PlayerTwoArmy.ContainsKey(oldPos))
            {
                NewGameScript.PlayerTwoArmy.Remove(oldPos);
            }
            NewGameScript.PlayerTwoArmy.Add(newPos, unit);
        }

        Tile tile = Map.Board[endPosX, endPosZ];

        Quaternion q = me.transform.rotation;
        me.transform.position = tile.transform.position;
        me.transform.rotation = q;
        me.GetComponent<UnitMovement>().TeleportTo(tile);
        tile.SetMyUnitTo(me);
    }
}
