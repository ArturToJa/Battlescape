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
            MultiTile position = tile.PositionRelatedToMouse(myUnit.currentPosition.width, myUnit.currentPosition.height, hitInfo.point);
            if (position != null && (position.IsDropzoneOfTeam(GameRound.instance.currentPlayer.team.index)) && position.IsFreeFor(myUnit))
            {
                DropZone.instance.SendCommandToSetUnitPosition(myUnit, position);                
            }

        }       
    }

    public static void SetNewPosition(int index, int endPosX, int endPosZ)
    {                     
        Vector3 newPos = Global.instance.currentMap.board[endPosX, endPosZ].transform.position;
        
        Unit unit = UnitFactory.GetUnitByIndex(index);        

        Tile tile = Global.instance.currentMap.board[endPosX, endPosZ];

        unit.TryToSetMyPositionAndMoveTo(tile);
        unit.FaceMiddleOfMap();
    }
}
