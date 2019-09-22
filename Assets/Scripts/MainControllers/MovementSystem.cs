using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if (IsMovementInput() && IsPathNotEmpty() && IsMovementLegal()) // note it is a question about legality of movement to CURRENT MOUSE LOCATION, not IN GENERAL.
        {
            SendCommandToMove(MouseManager.Instance.SelectedUnit.GetComponent<UnitMovement>());
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
            IsLegalTimeToMove() &&
            IsThereLegalUnitToMove() &&
            IsNotDuringQuittingCombat() &&
            IsCurrentPlayerLocalHuman();
    }

    #region IsMovementLegal Subfunctions

    bool IsPathNotEmpty()
    {
        return PathCreator.Instance.Path.Count > 0;
    }

    bool IsTileMouseovered()
    {
        return MouseManager.Instance.mouseoveredTile != null;
    }

    bool IsLegalTimeToMove()
    {
        return (Ability_Basic.IsForcingMovementStuff || IsNormalMovementTime());
    }

    bool IsNormalMovementTime()
    {
        return TurnManager.Instance.CurrentPhase == TurnPhases.Movement && GameStateManager.Instance.GameState != GameStates.TargettingState;
    }

    bool IsThereLegalUnitToMove()
    {
        return
            MouseManager.Instance.SelectedUnit != null &&
            MouseManager.Instance.SelectedUnit.GetComponent<UnitMovement>().isMoving == false &&
            MovementQuestions.Instance.CanMove(MouseManager.Instance.SelectedUnit, MouseManager.Instance.mouseoveredTile, !Ability_Basic.IsForcingMovementStuff);
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
    public void SendCommandToMove(UnitMovement unit)
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            Tile target = GetFinalTile();
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
            DoMovement(unit);
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
        PathCreator.Instance.AddSteps(unit.myTile, destination);
        DoMovement(unit.GetComponent<UnitMovement>());
    }
    // NOTE - currently THIS is JUST doing NORMAL MOVEMENT. Please do not add special movments (like QC) here in ANY WAY!
    // ALSO - use THIS instead of SendCommand, if its already in a command, obviously.
    public void DoMovement(UnitMovement unit)
    {
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            GameStateManager.Instance.Animate();
        }
        MovementExecutor METool = new MovementExecutor(unit, PathCreator.Instance.GetMovementPath());
        if (unit is UnitFlight)
        {
            StartCoroutine(METool.Fly((unit as UnitFlight), PathCreator.Instance.Path[PathCreator.Instance.Path.Count - 1].transform.position));
        }
        else
        {
            StartCoroutine(METool.Travel());
        }
        TileColouringTool.UncolourAllTiles();
        MovementQuestions.Instance.CheckIfAnyMoreUnitsToMove();
    }

    Tile GetFinalTile()
    {
        if (QCManager.Instance.PlayerChoosesWhetherToQC)
        {
            return QCManager.Instance.FinalTile;
        }
        else
        {
            return PathCreator.Instance.Path[PathCreator.Instance.Path.Count - 1];
        }
    }

    public void CheckForAddingSteps(UnitScript unit, Tile oldTile, Tile newTile)
    {        
        if (IsTimeToAddSteps(unit, oldTile, newTile))
        {
            PathCreator.Instance.AddSteps(unit.myTile, newTile);
        }
    }

    bool IsTimeToAddSteps(UnitScript unit, Tile oldTile, Tile newTile)
    {
        return                       
            IsMovementLegal() &&
            oldTile != newTile;
    }
}
