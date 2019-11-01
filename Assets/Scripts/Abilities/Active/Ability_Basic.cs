using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public abstract class Ability_Basic : MonoBehaviour
{
    public static bool IsForcingMovementStuff = false;
    //this static bool is for abilities that allow movement and want the game to behave as if we were in normal movement phase
    public Tile Target { get; set; }
    protected UnitScript myUnit;
    public bool AlreadyUsedThisTurn { get; set; }
    public string Name;
    public string TooltipInfo;
    public Sprite mySprite;
    public GameObject MyObject { get; set; }
    protected bool isBeingUsed;
    public static Ability_Basic currentlyUsedAbility;
    public int UsesPerBattle;
    public int UsesLeft { get; protected set; }
    public Sound AbilitySound;
    AudioSource AbilitySource;
    public GameObject BasicVFX;
    public AbilityStyle Style;
    public int EnergyCost;
    protected UnitEnergy myEnergy;
    public List<TurnPhases> LegalInPhases;
    public bool LimitedUses;
    public bool OnlyInCombat;
    public bool UnavailableInCombat;
    public bool RequiresCanMove;
    public bool RequiresShootingAbility;
    public bool IsAttack;

    public void BaseUse()
    {
        Use();
        currentlyUsedAbility = this;
        isBeingUsed = true;
        GameStateManager.Instance.SetState(Style);
        ColourTiles();
        myEnergy.CurrentEnergy -= EnergyCost;

    }
    public void BaseCancelUse()
    {
        CancelUse();
        ColouringTool.UncolourAllTiles();
        GameStateManager.Instance.BackToIdle();
        currentlyUsedAbility = null;
        isBeingUsed = false;
        myEnergy.CurrentEnergy += EnergyCost;
        Debug.Log("woff");
    }
    protected void SendCommandForActivation()
    {
        if (GameStateManager.Instance.MatchType == MatchTypes.Online)
        {
            GameStateManager.Instance.GetComponent<PhotonView>().RPC
                (
                "RPCActivateAbility",
                PhotonTargets.All,
                Mathf.RoundToInt(transform.position.x),
                Mathf.RoundToInt(transform.position.z),
                Mathf.RoundToInt(Target.transform.position.x),
                Mathf.RoundToInt(Target.transform.position.z),
                this.GetType().ToString()
                );
        }
        else
        {
            Activate();
        }
    }
    protected abstract void OnStart();
    protected abstract void OnUpdate();
    protected abstract void CancelUse();
    protected abstract void Use();
    protected abstract void SetTarget();
    public abstract void Activate();
    protected abstract bool ActivationRequirements();
    /// <summary>
     /// Note that this function should be empty unles really needed otherwise (all functionality should be somehow in base unlkess it is really hard/unpractical/single-time-only-very-special-condition
     /// </summary>
    protected abstract bool IsUsableNow();
    public bool IsUsableNowBase()
    {
        return
            IsUsableNow() &&
            (UsesLeft > 0 || LimitedUses == false) &&
            myEnergy.IsEnoughEnergyFor(this) &&
            Global.instance.playerTeams[TurnManager.Instance.PlayerHavingTurn].players[0].team.index == myUnit.PlayerID &&
            AlreadyUsedThisTurn == false &&
            GameStateManager.Instance.IsItPreGame() == false &&
            LegalInPhases.Contains(TurnManager.Instance.CurrentPhase) &&
            (!OnlyInCombat || (OnlyInCombat && myUnit.CheckIfIsInCombat())) &&
            (!UnavailableInCombat || (UnavailableInCombat && !myUnit.CheckIfIsInCombat())) &&
            (!RequiresCanMove || (RequiresCanMove && myUnit.GetComponent<UnitMovement>().CanMove)) &&
            (!RequiresShootingAbility ||(RequiresShootingAbility && myUnit.GetComponent<ShootingScript>().IsAbleToShoot())) &&
            (!IsAttack || (IsAttack && myUnit.hasAttacked == false) );

    }
    public abstract bool AI_IsGoodToUseNow();
    public abstract void AI_Activate(GameObject Target);
    public abstract GameObject AI_ChooseTarget();

    protected virtual void ColourTiles()
    {
        return;
    }
    public virtual void OnHover()
    {
        return;
    }


    protected void FinishUsing()
    {
        if (UsesLeft > 0)
        {
            UsesLeft--;
        }
        GameStateManager.Instance.BackToIdle();
        ColouringTool.UncolourAllTiles();
        AlreadyUsedThisTurn = true;
        currentlyUsedAbility = null;
        isBeingUsed = false;
    }

    void Start()
    {

        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        UsesLeft = UsesPerBattle;
        myUnit = GetComponent<UnitScript>();
        myEnergy = GetComponent<UnitEnergy>();
        AbilitySource = gameObject.AddComponent<AudioSource>();
        OnStart();
    }

    void Update()
    {
        if (MyObject != null)
        {
            MyObject.GetComponentInChildren<AbilityIconScript>().EnergyText.text = "- " + EnergyCost.ToString();
            if (LimitedUses)
            {
                MyObject.GetComponentInChildren<AbilityIconScript>().LimitedText.text = "x " + UsesLeft.ToString();                              
            }
            else
            {
                MyObject.GetComponentInChildren<AbilityIconScript>().LimitedText.text = "";
            }

        }
        OnUpdate();
        if (this == currentlyUsedAbility && Input.GetMouseButtonDown(1))
        {
            BaseCancelUse();
        }
        if (isBeingUsed && ActivationRequirements())
        {
            SetTarget();
            if (Input.GetMouseButtonDown(0))
            {
                SendCommandForActivation();
                //FinishUsing();
            }
        }
        if (isBeingUsed)
        {

            GameStateManager.Instance.isTargetValid = ActivationRequirements();
            ColourTiles();
        }

    }

    public virtual void OnNewTurn()
    {
        AlreadyUsedThisTurn = false;
    }

    public void PlayAbilitySound()
    {
        Sound s = AbilitySound;
        s.oldSource = AbilitySource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }

    public GameObject CreateVFXOn(Transform target, Quaternion rotation)
    {
        return Instantiate(BasicVFX, target.position, rotation, target);
    }
}

public enum AbilityStyle { Target, Movement, Shot, Attack }

