using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class DropZone : MonoBehaviour//, IDropHandler
{
    public static DropZone Instance;


    private void Start()
    {
        Instance = this;
    }
    /*public void OnDrop(PointerEventData eventData)
    {
        if (!DidHitLegalTile() || DidHitUnit())
        {
            return;
        }
        DragableUnitIcon.objectBeingDragged.transform.SetParent(transform);
        CommandInstantiateUnit((int)DragableUnitIcon.objectBeingDragged.GetComponent<DragableUnitIcon>().me.myUnitID, TurnManager.Instance.PlayerHavingTurn, ElSecondRay());
        if (Application.isEditor)
        {
            DestroyImmediate(DragableUnitIcon.objectBeingDragged);
        }
        else
        {
            Destroy(DragableUnitIcon.objectBeingDragged);
        }
    }

    

   

    bool DidHitLegalTile()
    {
        Ray tileRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileInfo;
        int tileMask = 1 << 9;
        if (Physics.Raycast(tileRay, out tileInfo, Mathf.Infinity, tileMask) && CheckIfCorrectDropzone(tileInfo.transform) && tileInfo.transform.gameObject.GetComponent<Tile>().myUnit == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    Vector3 ElSecondRay()
    {
        Ray tileRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit tileInfo;
        int tileMask = 1 << 9;
        if (Physics.Raycast(tileRay, out tileInfo, Mathf.Infinity, tileMask))
        {
            return tileInfo.transform.position;
        }
        else
        {
            throw new System.Exception("");
        }
    }
    bool DidHitUnit()
    {
        Ray unitRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit unitInfo;
        int unitMask = 1 << 8;

        if (Physics.Raycast(unitRay, out unitInfo, Mathf.Infinity, unitMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckIfCorrectDropzone(Transform tile)
    {
        return tile.GetComponent<Tile>().isDropzoneOfPlayer[TurnManager.Instance.PlayerToMove];        
    }
    public void DestroyRealUnit(UnitScript unit)
    {
        if (unit.PlayerID == 0)
        {
            NewGameScript.PlayerOneArmy.Remove(unit.transform.position);
        }
        else
        {
            NewGameScript.PlayerTwoArmy.Remove(unit.transform.position);
        }
    }*/

    public void CommandInstantiateUnit(int UnitID, int PlayerID, Vector3 position)
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            GetComponent<PhotonView>().RPC("RPCInstantiateUnit", PhotonTargets.All, UnitID, PlayerID, position);
            //UnitPositionKeeper.Instance.photonView.RPC("RPCAddUnit", PhotonTargets.All, Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z));
        }
        else
        {
            InstantiateUnit(UnitID, PlayerID, position);
        }
    }

    [PunRPC]
    void RPCInstantiateUnit(int unitID, int PlayerID, Vector3 position)
    {
        InstantiateUnit(unitID, PlayerID, position);

    }

    void InstantiateUnit(int unitID, int PlayerID, Vector3 position)
    {
        GameObject UnitObject = UnitCreator.FindUnitPrefabByIndex(unitID);
        GameObject InstantiatedUnit = Instantiate(UnitObject, position, UnitObject.transform.rotation);
        Unit myUnit = InstantiatedUnit.GetComponent<Unit>();
        Global.instance.playerTeams[PlayerID].players[0].AddUnit(myUnit);
        InstantiatedUnit.name = myUnit.unitName;
        if (PlayerID == 0)
        {
            NewGameScript.PlayerOneArmy.Add(position, myUnit);
        }
        else
        {
            NewGameScript.PlayerTwoArmy.Add(position, myUnit);
        }
        Map.Board[(int)position.x, (int)position.z].SetMyUnitTo(myUnit);
    }

    [PunRPC]
    void RPCSetUnitPosition(int startPosX, int startPosZ, int endPosX, int endPosZ)
    {
        DragableUnit.SetNewPosition(startPosX, startPosZ, endPosX, endPosZ);
        
    }
}
