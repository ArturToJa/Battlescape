/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class CancelUnitsDeployment : MonoBehaviour
{
    DropZone dz;
    VERY_POORLY_WRITTEN_CLASS ab;
    PhotonView photonView;

    void Start()
    {
        dz = FindObjectOfType<DropZone>();
        ab = FindObjectOfType<VERY_POORLY_WRITTEN_CLASS>();
        photonView = GetComponent<PhotonView>();
    }
    public void CommandCancel()
    {
        if (Global.instance.MatchType == MatchTypes.Online)
        {
            //UnitPositionKeeper.Instance.photonView.RPC("RPCDeleteAll", PhotonTargets.All, GameRound.instance.currentPlayer);
            photonView.RPC("RPCCancel", PhotonTargets.All, GameRound.instance.currentPlayer);
        }
        else
        {
            Cancel(GameRound.instance.currentPlayer);
        }
        
    }

    
    [PunRPC]
    void RPCCancel(int ID)
    {
        Cancel(ID);        
    }

    void Cancel(int ID)
    {
        BattlescapeLogic.Unit[] AllUnits = FindObjectsOfType<BattlescapeLogic.Unit>();
        foreach (BattlescapeLogic.Unit unit in AllUnits)
        {
            if (unit.PlayerID == ID && unit.isRealUnit)
            {
                dz.DestroyRealUnit(unit);
                unit.currentPosition.SetMyUnitTo(null);
                if (Player.players[ID].type == PlayerType.Local)
                {
                    if (unit.unitUnit.ThisRealSprite == null)
                    {
                        ab.CreateUnit(unit.unitUnit.thisBox, unit.unitUnit.thisSprite, unit.unitUnit);
                    }
                    else
                    {
                        ab.CreateHero(unit.unitUnit.thisBox, unit.unitUnit.ThisRealSprite, unit.unitUnit);
                    }
                }
                if (Application.isEditor)
                {
                    DestroyImmediate(unit.gameObject);
                }
                else
                {
                    Destroy(unit.gameObject);
                }
            }

        }
    }
}*/
