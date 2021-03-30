using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;
using Photon.Pun;

public class DropZone : MonoBehaviour//, IDropHandler
{
    public static DropZone instance;
    PhotonView photonView;
    int tEMPORARY_INDEX_DELETE_THIS_PLEASE;


    private void Start()
    {
        instance = this;
        photonView = GetComponent<PhotonView>();
    }

    public void CommandInstantiateUnit(string UnitID, int PlayerID)
    {
        tEMPORARY_INDEX_DELETE_THIS_PLEASE++;
        int index = tEMPORARY_INDEX_DELETE_THIS_PLEASE;
        if (Global.instance.matchType == MatchTypes.Online)
        {
            GetComponent<PhotonView>().RPC("RPCInstantiateUnit", RpcTarget.All, UnitID, PlayerID, index);
        }
        else
        {
            InstantiateUnit(UnitID, PlayerID, index);
        }
    }

    [PunRPC]
    void RPCInstantiateUnit(string unitID, int PlayerID, int index)
    {
        InstantiateUnit(unitID, PlayerID, index);

    }

    void InstantiateUnit(string unitID, int playerID, int index)
    {
        GameObject UnitObject = UnitCreator.FindUnitPrefabByName(unitID);
        GameObject InstantiatedUnit = Instantiate(UnitObject, Vector3.zero, UnitObject.transform.rotation);
        Unit myUnit = InstantiatedUnit.GetComponent<Unit>();
        myUnit.index = index;
        Global.instance.playerTeams[playerID].players[0].AddUnit(myUnit);
        InstantiatedUnit.name = myUnit.info.unitName;       
    }

    public void SendCommandToSetUnitPosition(Unit unit, MultiTile targetPosition)
    {
        int endPosX = targetPosition.bottomLeftCorner.position.x;
        int endPosZ = targetPosition.bottomLeftCorner.position.z;
        if (Global.instance.matchType == MatchTypes.Online)
        {
            photonView.RPC("RPCSetUnitPosition", RpcTarget.All, unit.index, endPosX, endPosZ);
        }
        else
        {
            DragableUnit.SetNewPosition(unit.index, endPosX, endPosZ);
        }
    }

    [PunRPC]
    void RPCSetUnitPosition(int index, int endPosX, int endPosZ)
    {
        DragableUnit.SetNewPosition(index, endPosX, endPosZ);

    }
}
