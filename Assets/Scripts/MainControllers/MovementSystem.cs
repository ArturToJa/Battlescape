using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class MovementSystem : MonoBehaviour
{

    public static MovementSystem Instance { get; set; }
    // NOTE - MouseManager is STILL in controll of creating PATHS and colouring tiles and stuff like that., It shouldnt, but it works and it is very ugly os i just dont want to waste time on redoing it.

    PhotonView photonView;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        photonView = GetComponent<PhotonView>();
    }

    void Update()
    {
        if (IsMovementInput() && IsMovementLegal()) // note it is a question about legality of movement to CURRENT MOUSE LOCATION, not IN GENERAL.
        {
            SendCommandToMove(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredTile);
        }
    }

    bool IsMovementInput()
    {
        return
            Input.GetMouseButtonDown(0);
    }
    bool IsMovementLegal()
    {
        return            
            IsTileMouseovered() &&
            IsThereLegalUnitToMove() &&
            IsNotDuringQuittingCombat() &&
            IsCurrentPlayerLocalHuman();
    }

    #region IsMovementLegal Subfunctions   

    bool IsTileMouseovered()
    {
        return MouseManager.Instance.mouseoveredTile != null;
    }    

    bool IsThereLegalUnitToMove()
    {
        return
            MouseManager.Instance.SelectedUnit != null &&
            MouseManager.Instance.SelectedUnit.newMovement.isMoving == false &&
            MovementQuestions.Instance.CanMove(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredTile);
    }

    bool IsNotDuringQuittingCombat()
    {
        return QCManager.Instance.PlayerChoosesWhetherToQC == false;
    }

    bool IsCurrentPlayerLocalHuman()
    {
        return GameStateManager.Instance.IsCurrentPlayerLocal();
    }

    #endregion


    /// <summary>
    /// Used to either perform movement in offline modes or send an RPC in online mode.
    /// </summary>
    /// <param name="unit"> Unit to be moved to the last tile in Path made by PathCreator</param>
    public void SendCommandToMove(UnitScript unit, Tile target)
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            int startX = Mathf.RoundToInt(unit.transform.position.x);
            int startZ = Mathf.RoundToInt(unit.transform.position.z);
            int endX = Mathf.RoundToInt(target.transform.position.x);
            int endZ = Mathf.RoundToInt(target.transform.position.z);

            photonView.RPC(
                "RPCDoMovement",
                PhotonTargets.All,
                startX,
                startZ,
                endX,
                endZ);
        }
        else
        {
            DoMovement(unit,target);
        }
    }



    [PunRPC]
    void RPCDoMovement(int startX, int startZ, int endX, int endZ)
    {
        Tile startTile = Map.Board[startX, startZ];
        UnitScript unit = startTile.myUnit;
        if (unit == null)
        {
            Debug.LogError("NoUnit!");
            Log.SpawnLog("NO UNIT TO MOVE!");
            return;
        }
        Tile destination = Map.Board[endX, endZ];        
        DoMovement(unit,destination);
    }
    // NOTE - currently THIS is JUST doing NORMAL MOVEMENT. Please do not add special movments (like QC) here in ANY WAY!
    // ALSO - use THIS instead of SendCommand, if its already in a command, obviously.
    public void DoMovement(UnitScript unit,Tile tile)
    {
        unit.newMovement.ApplyUnit(unit);
        unit.Move(tile);
    }   
}
