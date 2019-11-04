using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_Marksmen_PenetrationShot : Ability_Basic
{
    [SerializeField] int RangeModifier;
    UIHitChanceInformation hitInfo;

    protected override void OnStart()
    {
        hitInfo = FindObjectOfType<UIHitChanceInformation>();
    }

    protected override void OnUpdate()
    {
        if (isBeingUsed)
        {
            if (MouseManager.Instance.MouseoveredUnit != null)
            {
                if (hitInfo.IsNewTargetToCalculate())
                {
                    hitInfo.ChangeUnitsStatsBy(-Mathf.CeilToInt(0.5f * MouseManager.Instance.MouseoveredUnit.statistics.GetCurrentDefence()));
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
        myUnit.statistics.bonusAttackRange -= RangeModifier;
        hitInfo.ChangeUnitsStatsBy(0);
    }

    protected override void Use()
    {
        myUnit.statistics.bonusAttackRange += RangeModifier;
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
        return ((MouseManager.Instance.MouseoveredUnit != null) && ShootingScript.WouldItBePossibleToShoot(myUnit, transform.position, MouseManager.Instance.MouseoveredUnit.transform.position).Key);
    }


    void PerformPenetratingShot(UnitScript target)
    {
        //int oldDef = target.statistics.GetCurrentDefence();
        //target.statistics.defence = target.statistics.GetCurrentDefence() - Mathf.CeilToInt(0.5f * target.statistics.GetCurrentDefence());
        myUnit.statistics.numberOfAttacks = 0;
        CombatController.Instance.AttackTarget = target;
        CombatController.Instance.Shoot(myUnit, target, false,false);
        //target.statistics.GetCurrentDefence() = oldDef;
        myUnit.statistics.bonusAttackRange -= RangeModifier;
        GameStateManager.Instance.BackToIdle();
    }

    IEnumerator ActivateThisAbility(UnitScript target)
    {
        yield return null;
        FinishUsing();
        Log.SpawnLog(myUnit.name + " shoots a Penetrating Shot at " + target.name + ", temporarily decreasing " + target.name + "'s defence by half");
        myUnit.statistics.numberOfAttacks = 0;
        DoArtisticStuff();
        GameStateManager.Instance.Animate();
        yield return new WaitForSeconds(1.5f);
        GameStateManager.Instance.EndAnimation();
        if (Global.instance.playerTeams[TurnManager.Instance.PlayerToMove].players[0].type != PlayerType.Network)
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
            if (tile.myUnit != null && tile.myUnit.PlayerID != myUnit.PlayerID && ShootingScript.WouldItBePossibleToShoot(myUnit, this.transform.position, tile.transform.position).Key)
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.red);
            }
            else
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.white);
            }
        }
    }


    // AI Segment

    public override bool AI_IsGoodToUseNow()
    {
        List<UnitScript> targets = ShootingScript.PossibleTargets(myUnit, transform.position);
        if (targets.Count > 0 && myUnit.statistics.healthPoints == 1)
        {
            return true;
        }
        foreach (UnitScript enemy in targets)
        {
            if (enemy.statistics.GetCurrentDefence() >= 4)
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
        List<UnitScript> targets = ShootingScript.PossibleTargets(myUnit, transform.position);
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
                if (unit.statistics.GetCurrentDefence() >= startingDefence)
                {
                    return unit.gameObject;
                }
            }
            startingDefence--;
        }
        
        
    }
}
