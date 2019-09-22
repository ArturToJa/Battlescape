using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnManager : MonoBehaviour
{

    bool DoesWantToShowPhaseWindow
    {
        get
        {
            return
                PlayerPrefs.HasKey("SkipNextPhaseNotification") == false ||
                PlayerPrefs.GetInt("SkipNextPhaseNotification") == 0;
        }
    }
    [SerializeField] TurnNumberText text1;
    [SerializeField] CurrentPlayerInfo text2;

    
    PhotonView photonView;
    public static TurnManager Instance { get; private set; }
    [Header("Important")]
    public int TurnsInTheGame = 10;
    [Header("Nonsense")]
    public bool isEndgameTrue = false;
    public int PlayerHavingTurn = 0;
    public int PlayerToMove
    {
        get
        {
            if (CurrentPhase == TurnPhases.Enemy)
            {
                return Mathf.Abs(PlayerHavingTurn - 1);
            }
            else
            {
                return PlayerHavingTurn;
            }
        }
    }
    public int OpponentOfActivePlayer
    {
        get
        {
            return Mathf.Abs(PlayerToMove - 1);
        }

    }
    public int TurnCount;
    public Button Next;
    public GameObject nexxt;
    public GameObject EndTurner;
    [SerializeField] GameObject EndTurnWindow;
    [SerializeField] GameObject EndPhaseWindow;
    AudioSource turnSource;
    public TurnPhases CurrentPhase;

    int PlayerCountOfEndPreGame;



    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (Instance == null)
        {
            Instance = this;
        }
        turnSource = gameObject.AddComponent<AudioSource>();
        turnSource.clip = Resources.Load<AudioClip>("NewTurnSound");
        turnSource.volume = 0.02f;
        CurrentPhase = TurnPhases.None;
        TurnCount = -1;

        if (GameStateManager.Instance.MatchType == MatchTypes.Online && Player.Players[1].Type == PlayerType.Local)
        {
            // we are in an online game, AND we are not a host (precisely: second (red) player is the local player) so we should set CurrentPlayer stuff to 1 ;)
            PlayerHavingTurn = 1;
        }

    }

    public void Update()
    {
        SetButtons();
        CheckInput();
    }

    void CheckInput()
    {
        if (Input.GetKeyDown(KeyCode.End) && (Player.Players[PlayerHavingTurn].Type == PlayerType.Local) && GameStateManager.Instance.IsGameStateNormal() && TurnCount > 0 && InGameInputField.IsNotTypingInChat())
        {
            if (CurrentPhase == TurnPhases.Movement || CurrentPhase == TurnPhases.Shooting)
            {
                NextPhase(false);
            }
            else
            {
                NewTurn(true);
            }
        }
    }

    void SetButtons()
    {
    
        if (TurnCount > 10 && isEndgameTrue == false)
        {
            isEndgameTrue = true;
        }

        if ((CurrentPhase == TurnPhases.Movement || CurrentPhase == TurnPhases.Shooting) && TurnCount > 0)
        {
            nexxt.SetActive(true);

        }
        else
        {
            nexxt.SetActive(false);

        }
        if (CurrentPhase != TurnPhases.Attack)
        {
            EndTurner.SetActive(false);
        }
        else
        {
            EndTurner.SetActive(true);
        }
        if (GameStateManager.Instance.IsCurrentPlayerLocal() == false)
        {
            EndTurner.SetActive(false);
            nexxt.SetActive(false);
        }
        if (TurnCount > 16)
        {
            nexxt.SetActive(false);
        }
    }

    public void NewTurnWrapper()
    {
        if (CombatController.CheckIfLastAttacker() == false || DoesWantToShowPhaseWindow)
        {
            EndTurnWindow.SetActive(true);
        }
        else
        {
            NewTurn(true);
        }
    }


    public void NewTurn(bool isRealGame)
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online && isRealGame)
        {
            photonView.RPC("RPCNewTurn", PhotonTargets.All, isRealGame);
        }
        else
        {
            SetNewTurn(isRealGame);
        }
    }

    [PunRPC]
    void RPCNewTurn(bool isRealGame)
    {
        SetNewTurn(isRealGame);
    }

    public event Action NewTurnEvent;

    public void SetNewTurn(bool isRealGame)
    {
        if (NewTurnEvent != null)
        {
            NewTurnEvent();
        }
        text1.isOff = false;
        text2.isOff = false;
        SwitchPlayerHavingTurn();
        CurrentPhase = TurnPhases.Movement;
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            Debug.Log("New AI turn.");
        }
        if (isRealGame)
        {
            turnSource.Play();
            TurnCount++;
            if ( TurnCount > 16)
            {
                PopupTextController.AddPopupText("Time is up!", PopupTypes.Stats);
            }
            else if (TurnCount == 16)
            {
                PopupTextController.AddPopupText("Final Turn!", PopupTypes.Damage);
                Log.SpawnLog("The last turn of the game has begun!");
                Log.SpawnLog("Movement phase begins.");
            }
            else if (TurnCount >= 11)
            {
                PopupTextController.AddPopupText("Remaining turns: " + (16 - TurnCount).ToString() + "!", PopupTypes.Damage);
                Log.SpawnLog("New turn. Remaining turns: " + (16 - TurnCount).ToString() + ".");
                Log.SpawnLog("Movement phase begins.");
            }
            else if (TurnCount > 1)
            {
                PopupTextController.AddPopupText("New Turn!", PopupTypes.Info);
                Log.SpawnLog("New turn.");
                Log.SpawnLog("Movement phase begins.");
            }
            else
            {
                PopupTextController.AddPopupText("Press Escape to see Victory Conditions!", PopupTypes.Info);
                Log.SpawnLog("Prepare for the Battle! Press Escape to see Victory Conditions!");
                Log.SpawnLog("Movement phase begins.");
            }
        }
        StartCoroutine(SetColour());
    }

    IEnumerator SetColour()
    {
        yield return null;
        UnitHealth.SetColour();
    }

    public void MovementPhase()
    {
        CurrentPhase = TurnPhases.Movement;
        MouseManager.Instance.Deselect();
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            Debug.Log("Movement phase beggins.");
        }
    }

    public void ShootingPhase()
    {
        CurrentPhase = TurnPhases.Shooting;
        MouseManager.Instance.Deselect();
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            Debug.Log("Shooting phase beggins.");
        }
    }

    public void AttackPhase()
    {
        CurrentPhase = TurnPhases.Attack;
        MouseManager.Instance.Deselect();
        if (GameStateManager.Instance.IsCurrentPlayerAI())
        {
            Debug.Log("Attack phase beggins.");
        }
    }

    public void NextPhase(bool didAI)
    {
        if (DoesWantToShowPhaseWindow && didAI == false && GameStateManager.Instance.MatchType != MatchTypes.Online)
        {
            EndPhaseWindow.SetActive(true);
        }
        else
        {
            RealNextPhase(didAI);
        }
    }

    public void RealNextPhase(bool didAI)
    {
        PopupTextController.ClearPopups();
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            photonView.RPC("RPCNextPhase", PhotonTargets.All, didAI);
        }
        else
        {
            SetNextPhase(didAI);
        }
    }
    
    [PunRPC]
    void RPCNextPhase(bool didAI)
    {
        SetNextPhase(didAI);
    }

    void SetNextPhase(bool didAI)
    {
        if ((GameStateManager.Instance.IsCurrentPlayerAI() == true && didAI == false) || (GameStateManager.Instance.IsCurrentPlayerAI() == false && didAI == true) || CurrentPhase == TurnPhases.Enemy || GameStateManager.Instance.GameState == GameStates.AnimatingState)
        {
            return;
        }
        if (CurrentPhase == TurnPhases.Movement)
        {
            TileColouringTool.UncolourAllTiles();
            PopupTextController.AddPopupText("Next Phase!", PopupTypes.Info);
            Log.SpawnLog("Shooting phase begins.");
            PathCreator.Instance.ClearPath();
            ShootingPhase();
        }
        else if (CurrentPhase == TurnPhases.Shooting)
        {
            PopupTextController.AddPopupText("Next Phase!", PopupTypes.Info);
            Log.SpawnLog("Attack phase begins.");
            AttackPhase();
        }
    }

    public void SwitchPlayerHavingTurn()
    {
        PlayerHavingTurn = Mathf.Abs(PlayerHavingTurn - 1);
    }

    [PunRPC]
    void RPCPlayerEndedPreGame()
    {
        PlayerCountOfEndPreGame++;
        if (IsTimeToEndPreGame())
        {
            PlayerHavingTurn = 1;
            SetNewTurn(true);
        }
    }

    public void PlayerEndedPreGame()
    {
        photonView.RPC("RPCPlayerEndedPreGame", PhotonTargets.All);
    }

    public bool IsTimeToEndPreGame()
    {
        return PlayerCountOfEndPreGame == 2;
    }

}

public enum TurnPhases
{
    None, Movement, Shooting, Attack, Enemy
}
