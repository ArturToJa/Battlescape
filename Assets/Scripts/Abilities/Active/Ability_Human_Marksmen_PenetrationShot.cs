using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_Marksmen_PenetrationShot : Ability_Basic
{
    [SerializeField] int RangeModifier;
    ShootingScript myShootingScript;
    UIHitChanceInformation hitInfo;

    protected override void OnStart()
    {
        hitInfo = FindObjectOfType<UIHitChanceInformation>();
        myShootingScript = GetComponent<ShootingScript>();
    }

    protected override void OnUpdate()
    {
        if (isBeingUsed)
        {
            if (MouseManager.Instance.MouseoveredUnit != null)
            {
                if (hitInfo.IsNewTargetToCalculate())
                {
                    hitInfo.ChangeUnitsStatsBy(-Mathf.CeilToInt(0.5f * MouseManager.Instance.MouseoveredUnit.CurrentDefence));
                    hitInfo.ShowNewInformation();
                }
            }
        }
    }

    protected override bool IsUsableNow()
    {
        return true;
    }


    protected override void CancelUse()
    {
        myShootingScript.currShootingRange -= RangeModifier;
        hitInfo.ChangeUnitsStatsBy(0);
    }

    protected override void Use()
    {
        myShootingScript.currShootingRange += RangeModifier;
    }

    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

    public override void Activate()
    {
        StartCoroutine(ActivateThisAbility(Target.myUnit));
    }

    protected override bool ActivationRequirements()
    {
        return ((MouseManager.Instance.MouseoveredUnit != null) && ShootingScript.WouldItBePossibleToShoot(myShootingScript, transform.position, MouseManager.Instance.MouseoveredUnit.transform.position).Key);
    }


    void PerformPenetratingShot(UnitScript target)
    {
        int oldDef = target.CurrentDefence;
        target.CurrentDefence = target.CurrentDefence - Mathf.CeilToInt(0.5f * target.CurrentDefence);
        myShootingScript.hasAlreadyShot = true;
        CombatController.Instance.AttackTarget = target;
        CombatController.Instance.Shoot(myUnit, target, false,false);
        target.CurrentDefence = oldDef;
        myShootingScript.currShootingRange -= RangeModifier;
        GameStateManager.Instance.BackToIdle();
    }

    IEnumerator ActivateThisAbility(UnitScript target)
    {
        yield return null;
        FinishUsing();
        Log.SpawnLog(myUnit.name + " shoots a Penetrating Shot at " + target.name + ", temporarily decreasing " + target.name + "'s defence by half");
        myShootingScript.hasAlreadyShot = true;
        DoArtisticStuff();
        GameStateManager.Instance.Animate();
        yield return new WaitForSeconds(1.5f);
        GameStateManager.Instance.EndAnimation();
        if (Player.Players[TurnManager.Instance.PlayerToMove].Type != PlayerType.Network)
        {
            // We cannot send this through network if we are just doing this ONLY because enemy used it in online game - cause he will also send this command and we will shoot 2 times).
            PerformPenetratingShot(target);
        }
        hitInfo.ChangeUnitsStatsBy(0);
    }

    void DoArtisticStuff()
    {
        PlayAbilitySound();
        GetComponent<AnimController>().Cast();
        CreateVFXOn(transform, transform.rotation);
    }
    protected override void ColourTiles()
    {
        foreach (Tile tile in Map.Board)
        {
            if (tile.myUnit != null && tile.myUnit.PlayerID != myUnit.PlayerID && ShootingScript.WouldItBePossibleToShoot(this.GetComponent<ShootingScript>(), this.transform.position, tile.transform.position).Key)
            {
                ColouringTool.SetColour(tile, Color.red);
            }
            else
            {
                ColouringTool.SetColour(tile, Color.white);
            }
        }
    }


    // AI Segment

    public override bool AI_IsGoodToUseNow()
    {
        List<UnitScript> targets = ShootingScript.PossibleTargets(myShootingScript, transform.position);
        if (targets.Count > 0 && myUnit.CurrentHP == 1)
        {
            return true;
        }
        foreach (UnitScript enemy in targets)
        {
            if (enemy.CurrentDefence >= 4)
            {
                return true;
            }
        }
        return false;
    }

    public override void AI_Activate(GameObject Target)
    {
        StartCoroutine(ActivateThisAbility(Target.GetComponent<UnitScript>()));
    }

    public override GameObject AI_ChooseTarget()
    {
        List<UnitScript> targets = ShootingScript.PossibleTargets(myShootingScript, transform.position);
        int startingDefence = 10;
        while (true)
        {
            if (startingDefence < -10)
            {
                Debug.LogError("This loop didn't stop for some reason");
                Log.SpawnLog("AI of Marksman went boom. Tell Dogo about it.");
                return null;
            }
            foreach (UnitScript unit in targets)
            {
                if (unit.CurrentDefence >= startingDefence)
                {
                    return unit.gameObject;
                }
            }
            startingDefence--;
        }
        
        
    }
}
