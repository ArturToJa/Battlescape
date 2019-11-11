using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class QCManager : MonoBehaviour
{

    // ATTENTION!!!!!!!!!!!!!!!!!!

    //Currently its IMPOSSIBLE (!) to get QCWindow if you are NOT in Movement Phase. If any ability wants to make QC with attacks etc, it needs to either change this rule or trick it into thinking that we are in movement phase or bypass the window alltogether.

    public Tile FinalTile;
    public static QCManager Instance { get; private set; }

    [SerializeField] GameObject windowQC;
    public bool PlayerChoosesWhetherToQC = false;
    public bool IsTimeForBackstabs = false;
    // ^ THIS is for player to not change "path" during mouse movements done while QC window is on
    PhotonView photonView;
    BattlescapeLogic.Unit quittingUnit;

    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        photonView = GetComponent<PhotonView>();
    }

    public void StartQC(Tile t)
    {
        FinalTile = t;
        PlayerChoosesWhetherToQC = true;
        if (GameStateManager.Instance.IsCurrentPlayerLocal())
        {
            windowQC.SetActive(true);
        }
    }
    public void CommandFinishQuitCombat(bool didDie)
    {
        if (quittingUnit == null)
        {
            Log.SpawnLog("BUG! line 39 QCManager!");
            return;
        }
        int unitX = Mathf.RoundToInt(quittingUnit.currentPosition.transform.position.x);
        int unitZ = Mathf.RoundToInt(quittingUnit.currentPosition.transform.position.z);
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCFinishQC", PhotonTargets.All, didDie, unitX, unitZ);
        }
        else
        {
            FinishQC(didDie, unitX, unitZ);
        }
    }

    [PunRPC]
    void RPCFinishQC(bool didDie, int unitX, int unitZ)
    {
        FinishQC(didDie, unitX, unitZ);
    }

    void FinishQC(bool didDie, int unitX, int unitZ)
    {
        //if (didDie == false)
        //{
        //    BattlescapeLogic.Unit QCUnit = Map.Board[unitX, unitZ].myUnit;
        //    QCUnit.isQuittingCombat = true;
        //    BattlescapeLogic.Unit uMovement = QCUnit.GetComponent<BattlescapeLogic.Unit>();
        //    uMovement.CanMove = true;
        //    MovementSystem.Instance.DoMovement(uMovement);

        //    if (QCUnit.IsRanged())
        //    {
        //        QCUnit.statistics.numberOfAttacks = 0;
        //    }
        //    uMovement.CanMove = true;
        //    FinalTile = null;
        //}
        //PlayerChoosesWhetherToQC = false;

    }

    public void AcceptQC()
    {
        StartCoroutine(CheckForBackstabsInCoroutine());
        windowQC.SetActive(false);
    }

    public void CancelQC()
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCCancelQC", PhotonTargets.All);
        }
        else
        {
            Cancel();
        }

        windowQC.SetActive(false);
    }

    [PunRPC]
    void RPCCancelQC()
    {
        Cancel();
    }

    void Cancel()
    {
        PlayerChoosesWhetherToQC = false;
        // PathCreator.Instance.ClearPath();
    }

    IEnumerator CheckForBackstabsInCoroutine()
    {
        IsTimeForBackstabs = true;
        quittingUnit = MouseManager.Instance.SelectedUnit;

        List<BattlescapeLogic.Unit> enemies = new List<BattlescapeLogic.Unit>();
        foreach (Tile tile in quittingUnit.currentPosition.neighbours)
        {
            if (tile.myUnit != null && tile.myUnit.owner.team != quittingUnit.owner.team)
            {
                enemies.Add(tile.myUnit);
            }
        }
        for (int i = 0; i < enemies.Count; i++)
        {
            BattlescapeLogic.Unit enemy = enemies[i];
            CombatController.Instance.SendCommandToAttack(enemy, quittingUnit);
            yield return new WaitForSeconds(2f);
            if (quittingUnit.IsAlive() == false)
            {
                break;
            }

            if (quittingUnit == null)
            {
                yield return null;
            }
        }
        IsTimeForBackstabs = false;
        CommandFinishQuitCombat(quittingUnit.IsAlive() == false);
    }

    public void QCForAI(Tile destination)
    {
        StartCoroutine(QCAIRoutine(destination));
    }

    private IEnumerator QCAIRoutine(Tile destination)
    {
        BattlescapeLogic.Unit unit = MouseManager.Instance.SelectedUnit;
        StartCoroutine(CheckForBackstabsInCoroutine());
        yield return new WaitForSeconds(1f);

        if (unit != null && unit.IsAlive())
        {
            //PathCreator.Instance.AddSteps(unit, destination);
            MovementSystem.Instance.SendCommandToMove(unit, destination);
        }
        else
        {
            yield return null;
        }

    }
}
