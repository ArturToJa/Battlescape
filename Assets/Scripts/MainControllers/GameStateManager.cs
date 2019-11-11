using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using BattlescapeLogic;

public enum GameStates { IdleState, MoveState, AttackState, /*QuittingCombatState, */AnimatingState, TargettingState, RetaliationState }

public enum MatchTypes { Online, HotSeat, Singleplayer, None }


public class GameStateManager : MonoBehaviour
{

    public static GameStateManager Instance;




    TurnPhases preEnemyPhase;
    public int startingArmyPoints;
    public bool isTargetValid;

    [SerializeField] GameObject ConnectionLossScreen;
    //note this bool above only makes any sense if game state is targetting!

    public GameStates GameState { get; private set; }

    public MatchTypes MatchType;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        MatchType = MatchTypes.None;
    }


    public void Update()
    {
        if (IsSceneGameScene(SceneManager.GetActiveScene()) == false || GameState == GameStates.RetaliationState)
        {
            return;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement && GameState == GameStates.IdleState && MouseManager.Instance.SelectedUnit != null)
        {
            GameState = GameStates.MoveState;
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack && GameState == GameStates.IdleState && MouseManager.Instance.SelectedUnit != null)
        {
            GameState = GameStates.AttackState;
        }
        if (MouseManager.Instance.SelectedUnit == null)
        {
            GameState = GameStates.IdleState;
        }
    }    

    public void SetState(AbilityStyle style)
    {
        switch (style)
        {
            case AbilityStyle.Target:
                GameState = GameStates.TargettingState;
                break;
            case AbilityStyle.Movement:
                GameState = GameStates.MoveState;
                break;
            case AbilityStyle.Shot:
                GameState = GameStates.AttackState;
                break;
            case AbilityStyle.Attack:
                GameState = GameStates.AttackState;
                break;
            default:
                break;
        }
    }

    public void Targetting()
    {
        GameState = GameStates.TargettingState;
    }

    public void Animate()
    {
        GameState = GameStates.AnimatingState;
    }

    public void EndAnimation()
    {
        GameState = GameStates.IdleState;
    }

    public void StartRetaliationChoice()
    {
        if (MatchType == MatchTypes.Online)
        {
            GetComponent<PhotonView>().RPC("RPCRetaliation", PhotonTargets.Others);
            Retaliation();
        }
        else
        {
            Retaliation();
        }

    }
    [PunRPC]
    void RPCRetaliation()
    {
        Retaliation();
        UIManager.SmoothlyTransitionActivity(GameObject.FindGameObjectWithTag("InfoRetal"), true, 0.001f);
    }

    void Retaliation()
    {
        // NOTE that switching to retaliation state is ENOUGH to trigger retaliation choice, as the RetaliationButtonsScript takes care of everything.
        preEnemyPhase = TurnManager.Instance.CurrentPhase;
        GameState = GameStates.RetaliationState;
        TurnManager.Instance.CurrentPhase = TurnPhases.Enemy;
    }

    public void FinishRetaliation()
    {
        UIManager.SmoothlyTransitionActivity(GameObject.FindGameObjectWithTag("InfoRetal"), false, 0.001f);
        TurnManager.Instance.CurrentPhase = preEnemyPhase;
        GameState = GameStates.IdleState;
        CombatController.Instance.MakeAIWait(0.5f);
    }

    public void BackToIdle()
    {
        GameState = GameStates.IdleState;
    }

    public bool IsItPreGame()
    {
        return TurnManager.Instance.TurnCount <= 0;
    }

    public bool IsGameStateNormal()
    {
        switch (GameState)
        {
            case GameStates.IdleState:
                return true;
            case GameStates.MoveState:
                return true;
            case GameStates.AttackState:
                return true;
            case GameStates.AnimatingState:
                return false;
            case GameStates.TargettingState:
                return false;
            case GameStates.RetaliationState:
                return false;
            default:
                Debug.Log("New game state exists and it is not enlisted here");
                return false;
        }
    }

    public bool IsCurrentPlayerAI()
    {
        if (Global.instance.playerBuilders[TurnManager.Instance.PlayerToMove] != null)
        {
            return Global.instance.playerBuilders[TurnManager.Instance.PlayerToMove].type == PlayerType.AI;
        }
        else
        {
            return (Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].type == PlayerType.AI);
        }
    }

    public bool IsCurrentPlayerLocal()
    {
        if (Global.instance.playerBuilders[TurnManager.Instance.PlayerToMove] != null)
        {
            return Global.instance.playerBuilders[TurnManager.Instance.PlayerToMove].type == PlayerType.Local;
        }
        else
        {
            return (Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].type == PlayerType.Local);
        }
    }

    public static void NextPhase()
    {
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
        {
            TurnManager.Instance.NewTurn(true);
        }
        else
        {
            TurnManager.Instance.NextPhase(true);
        }
    }
    public bool CanUnitActInThisPhase(BattlescapeLogic.Unit unit)
    {
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Movement)
        {
            return unit.CanStillMove();
        }
        if (TurnManager.Instance.CurrentPhase == TurnPhases.Attack)
        {
            return (unit.IsRanged() ||unit.IsInCombat()) && unit.CanStillAttack() == true && unit.attack != null;
        }
        else return false;
    }

    [PunRPC]
    public void RPCActivateAbility(int UnitX, int UnitZ, int targetX, int targetZ, string abilityName)
    {
        Ability_Basic Ability = Map.Board[UnitX, UnitZ].myUnit.GetComponent(abilityName) as Ability_Basic;
        Tile Target = Map.Board[targetX, targetZ];
        Ability.Target = Target;
        Ability.Activate();
    }

    [PunRPC]
    public void RPCConnectionLossScreen(string text)
    {
        StartCoroutine(MakeTheScreenVisible(text));

    }

    IEnumerator MakeTheScreenVisible(string text)
    {
        CanvasGroup screen = GameObject.FindGameObjectWithTag("DisconnectedScreen").GetComponent<CanvasGroup>();
        GameObject.FindGameObjectWithTag("DisconnectedText").GetComponent<Text>().text = text;
        while (screen.alpha < 1)
        {
            UIManager.SmoothlyTransitionActivity(screen.gameObject, true, 0.01f);
            yield return null;
        }

    }

    [PunRPC]
    void RPCSetSeed(int s)
    {
        StartCoroutine(PutObstaclesWhenPossible(s));

    }

    [PunRPC]
    void RPCSetHeroName(int ID, string name)
    {
        HeroNames.SetHeroName(ID, name);
    }

    IEnumerator PutObstaclesWhenPossible(int s)
    {
        while (FindObjectOfType<MapVisuals>() == null)
        {
            yield return new WaitForSeconds(1f);
        }
        FindObjectOfType<MapVisuals>().RandomlyPutObstacles(s);
    }

    public bool IsSceneGameScene(Scene scene)
    {
        return scene.name.Contains("_GameScene_");        
    }
}
