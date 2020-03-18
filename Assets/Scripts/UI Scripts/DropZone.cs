using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using BattlescapeLogic;

public class DropZone : MonoBehaviour//, IDropHandler
{
    public static DropZone Instance;
    PhotonView photonView;


    private void Start()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }
   
    public void CommandInstantiateUnit(int UnitID, int PlayerID, Vector3 position)
    {
        if (Global.instance.matchType == MatchTypes.Online)
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

    void InstantiateUnit(int unitID, int playerID, Vector3 position)
    {
        GameObject UnitObject = UnitCreator.FindUnitPrefabByIndex(unitID);
        GameObject InstantiatedUnit = Instantiate(UnitObject, position, UnitObject.transform.rotation);
        Unit myUnit = InstantiatedUnit.GetComponent<Unit>();
        Global.instance.playerTeams[playerID].players[0].AddUnit(myUnit);
        InstantiatedUnit.name = myUnit.info.unitName;
        if (playerID == 0)
        {
            NewGameScript.PlayerOneArmy.Add(position, myUnit);
        }
        else
        {
            NewGameScript.PlayerTwoArmy.Add(position, myUnit);
        }
        Global.instance.currentMap.board[(int)position.x, (int)position.z].SetMyUnitTo(myUnit);
    }

    public void SendCommandToSetUnitPosition(Unit unit, Tile targetTile)
    {
        int startPosX = unit.currentPosition.position.x;
        int startPosZ = unit.currentPosition.position.z;
        int endPosX = targetTile.position.x;
        int endPosZ = targetTile.position.z;
        if (Global.instance.matchType == MatchTypes.Online)
        {
            photonView.RPC("RPCSetUnitPosition", PhotonTargets.All, startPosX, startPosZ, endPosX, endPosZ);
        }
        else
        {
            DragableUnit.SetNewPosition(startPosX, startPosZ, endPosX, endPosZ);
        }
    }

    [PunRPC]
    void RPCSetUnitPosition(int startPosX, int startPosZ, int endPosX, int endPosZ)
    {
        DragableUnit.SetNewPosition(startPosX, startPosZ, endPosX, endPosZ);
        
    }
}
