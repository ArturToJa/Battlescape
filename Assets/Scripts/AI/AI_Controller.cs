using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class AI_Controller : MonoBehaviour
{
    public static AI_Controller Instance { get; private set; }
    [SerializeField] ArmyBuildingEndButton endButton;
    public Transform DeploymentPanel;
    [Header("AI/Human Players")]

    [HideInInspector]
    public AI_BaseClass[] PlayerAIs;

    [Header("ValuesForAI")]    
    public float ChooseFromTopPercent = 90f;

    public static bool actionCooldown;
    public static bool didAllTheQCDecisionsHappen = false;
    public static bool isEvaluatingTiles = false;
    public static bool tilesAreEvaluated = false;

    public static bool isRetaliating = false;

    private void Start()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        if (GameStateManager.Instance.MatchType != MatchTypes.Singleplayer)
        {
            return;
        }
        PlayerAIs = new AI_BaseClass[2];

        for (int i = 0; i < PlayerAIs.Length; i++)
        {            
            if (Global.instance.playerTeams[i].players[0].type == PlayerType.AI)
            {
                PlayerAIs[i] = new AI_BaseClass(i);
            }
            else
            {
                PlayerAIs[i] = null;
            }
        }
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
    }

    private void LateUpdate()
    {
        if (GameStateManager.Instance.MatchType != MatchTypes.Singleplayer)
        {
            return;
        }
        if (TurnManager.Instance.TurnCount == 0 && GameStateManager.Instance.IsCurrentPlayerAI())
        {
            //PreGameAI tool = new PreGameAI();
            //tool.PositionUnits();
            endButton.OK();
        }
        if (VictoryLossChecker.IsGameOver || /*QCManager.QCisHappening ||*/ GameStateManager.Instance.GameState == GameStates.AnimatingState || GameStateManager.Instance.IsItPreGame() || actionCooldown || GameStateManager.Instance.IsCurrentPlayerAI() == false)
        {
            return;
        }
        // Already we know it is decisionmaking moment!
        // If green player has tim,e to move and he is an AI, do AI.
        if (CheckIfTimeForAI(0))
        {
            ChangeAIToAppropriateOne(0);
            PlayerAIs[0].DoAI();
        }
        if (CheckIfTimeForAI(1))
        {
            ChangeAIToAppropriateOne(1);
            PlayerAIs[1].DoAI();
        }
        if (GameStateManager.Instance.GameState == GameStates.RetaliationState && IsCurrentTurnHuman()  && isRetaliating == false)
        {
            isRetaliating = true;
            CombatController.Instance.RetaliationForAI();
        }
    }

    public void OnNewTurn()
    {
        didAllTheQCDecisionsHappen = false;
    }

    private bool CheckIfTimeForAI(int ID)
    {
        if (ID > 1)
        {
            Debug.LogError("Wrong ID! Too high!");
            return false;
        }
        if (isEvaluatingTiles)
        {
            return false;
        }       
        if (PlayerAIs[ID] != null && Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].team.index == ID)
        {
            return true;
        }

        return false;
    }

    public void EvaluateTiles(AI_BaseClass ai, BattlescapeLogic.Unit currentUnit, List<Tile> possibleMoves)
    {
        Debug.Log("Evaluation for: " + currentUnit + "on tile: " + currentUnit.currentPosition);
        StartCoroutine(ai.EvaluatePossibleMoves(currentUnit, possibleMoves));
    }

    public void ChangeAIToAppropriateOne(int ID)
    {
        if (ID > 1)
        {
            Debug.LogError("Wrong ID! Too high!");
            return;
        }

        if ((GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.AttackState) && TurnManager.Instance.CurrentPhase == TurnPhases.Attack && TurnManager.Instance.PlayerHavingTurn == TurnManager.Instance.PlayerToMove && IsCurrentTurnHuman() == false)
        {
            PlayerAIs[ID] = new AI_Attack(ID);
            return;
        }

        //if ((GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.ShootingState) && TurnManager.Instance.CurrentPhase == TurnPhases.Shooting)
        //{
        //    PlayerAIs[ID] = new AI_Shooting(ID);
        //    return;
        //}

        if ((GameStateManager.Instance.GameState == GameStates.IdleState || GameStateManager.Instance.GameState == GameStates.MoveState) && TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
        {
             if (!didAllTheQCDecisionsHappen)
             {
                 PlayerAIs[ID] = new AI_QuitCombat(ID);
             }
             else
                PlayerAIs[ID] = new AI_Movement(ID);
            return;
        }
    }
    bool IsCurrentTurnHuman()
    {
        return PlayerAIs[Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index] == null;
    }

    public void ClearAIs()
    {
        if (PlayerAIs[0] != null)
        {
            PlayerAIs[0] = new AI_BaseClass(0);
        }
        if (PlayerAIs[1] != null)
        {
            PlayerAIs[1] = new AI_BaseClass(1);
        }
    }
    public void UsePreGameAIForHuman()
    {
        //FindObjectOfType<CancelUnitsDeployment>().CommandCancel();
        var temp = new PreGameAI();
        temp.RepositionUnits();
    }



}

