using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_Marksmen_PenetrationShot : Ability_Basic
{
    [SerializeField] int RangeModifier;
//    UIHitChanceInformation hitInfo;

    protected override void OnStart()
    {
        //hitInfo = FindObjectOfType<UIHitChanceInformation>();
    }

    protected override void OnUpdate()
    {
        //if (isBeingUsed)
        //{
        //    if (MouseManager.Instance.MouseoveredUnit != null)
        //    {
        //        if (hitInfo.IsNewTargetToCalculate())
        //        {
        //            hitInfo.ChangeUnitsStatsBy(-Mathf.CeilToInt(0.5f * MouseManager.Instance.MouseoveredUnit.statistics.GetCurrentDefence()));
        //            hitInfo.ShowNewInformation();
        //        }
        //    }
        //}
    }

    protected override bool IsUsableNow()
    {
        return true;
    }


    protected override void CancelUse()
    {
        myUnit.statistics.bonusAttackRange -= RangeModifier;
        //hitInfo.ChangeUnitsStatsBy(0);
    }

    protected override void Use()
    {
        myUnit.statistics.bonusAttackRange += RangeModifier;
    }

    protected override void SetTarget()
    {
        Target = null; //MouseManager.Instance.mouseoveredTile;
    }

    public override void Activate()
    {
        StartCoroutine(ActivateThisAbility(Target.GetMyObject<Unit>()));
    }

    public override bool ActivationRequirements()
    {
        return true;// ((MouseManager.Instance.MouseoveredUnit != null) && CombatController.Instance.WouldItBePossibleToShoot(myUnit, transform.position, MouseManager.Instance.MouseoveredUnit.transform.position));
    }


    void PerformPenetratingShot(Unit target)
    {
        //int oldDef = target.statistics.GetCurrentDefence();
        //target.statistics.defence = target.statistics.GetCurrentDefence() - Mathf.CeilToInt(0.5f * target.statistics.GetCurrentDefence());
        myUnit.statistics.numberOfAttacks = 0;
        myUnit.Attack(target);
        //target.statistics.GetCurrentDefence() = oldDef;
        myUnit.statistics.bonusAttackRange -= RangeModifier;
    }

    IEnumerator ActivateThisAbility(BattlescapeLogic.Unit target)
    {
        yield return null;
        FinishUsing();
        Log.SpawnLog(myUnit.name + " shoots a Penetrating Shot at " + target.name + ", temporarily decreasing " + target.name + "'s defence by half");
        myUnit.statistics.numberOfAttacks = 0;
        DoArtisticStuff();
        PlayerInput.instance.isInputBlocked = true;
        yield return new WaitForSeconds(1.5f);
        PlayerInput.instance.isInputBlocked = false;
        if (GameRound.instance.currentPlayer.type != PlayerType.Network)
        {
            // We cannot send this through network if we are just doing this ONLY because enemy used it in online game - cause he will also send this command and we will shoot 2 times).
            PerformPenetratingShot(target);
        }
        //hitInfo.ChangeUnitsStatsBy(0);
    }

    void DoArtisticStuff()
    {
        PlayAbilitySound();
        //myUnit.GetComponent<AnimController>().Cast();
        CreateVFXOn(transform, transform.rotation);
    }
    protected override void ColourTiles()
    {
    //    foreach (Tile tile in Global.instance.map.board)
    //    {
    //        if (tile.GetMyObject<Unit>() != null && tile.GetMyObject<Unit>().GetMyOwner() != myUnit.GetMyOwner() && CombatController.Instance.WouldItBePossibleToShoot(myUnit, this.transform.position, tile.transform.position))
    //        {
    //            BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.red);
    //        }
    //        else
    //        {
    //            BattlescapeGraphics.ColouringTool.ColourObject(tile, Color.white);
    //        }
    //    }
    }


    // AI Segment
    //not working cause who cares
    // really, NOT WORKING cause i deleted stuff from here xD.
    public override bool AI_IsGoodToUseNow()
    {
        List<BattlescapeLogic.Unit> targets = new List<BattlescapeLogic.Unit>();
        if (targets.Count > 0 && myUnit.statistics.healthPoints == 1)
        {
            return true;
        }
        foreach (BattlescapeLogic.Unit enemy in targets)
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
        StartCoroutine(ActivateThisAbility(Target.GetComponent<BattlescapeLogic.Unit>()));
    }

    public override GameObject AI_ChooseTarget()
    {
        List<BattlescapeLogic.Unit> targets = new List<BattlescapeLogic.Unit>();
        int startingDefence = 10;
        while (true)
        {
            if (startingDefence < -10)
            {
                Debug.LogError("This loop didn't stop for some reason");
                Log.SpawnLog("AI of Marksman went boom. Tell Dogo about it.");
                return null;
            }
            foreach (BattlescapeLogic.Unit unit in targets)
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
