using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

[RequireComponent(typeof(ShootingScript))]
public class Ability_Human_Archers_HailOfArrows : Ability_Basic
{
    [SerializeField] int RangeModifier;
    Tile oldMouseoveredTile;
    [SerializeField] Vector3[] positions;
    [SerializeField] GameObject HOA_Arrows;
    [SerializeField] GameObject LightningFrame;

    protected override void OnStart()
    {
        return;
    }

    protected override void OnUpdate()
    {
        if (currentlyUsedAbility == this && (oldMouseoveredTile != MouseManager.Instance.mouseoveredTile))
        {
            BattlescapeGraphics.ColouringTool.UncolourAllTiles();
            ColourTiles();
        }
        oldMouseoveredTile = MouseManager.Instance.mouseoveredTile;
    }


    ////////////////////////////////


    protected override void Use()
    {
        myUnit.statistics.bonusAttackRange += RangeModifier;
    }

    protected override void CancelUse()
    {
        myUnit.statistics.bonusAttackRange -= RangeModifier;
    }

    protected override bool IsUsableNow()
    {
        return
            true;
    }

    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }

    protected override void ColourTiles()
    {
        if (MouseManager.Instance.mouseoveredTile == null)
        {
            return;
        }

        if (ShootingScript.WouldItBePossibleToShoot(myUnit, transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key == false)
        {
            return;
        }

        if (MouseManager.Instance.mouseoveredTile.myUnit != null)
        {
            BattlescapeGraphics.ColouringTool.SetColour(MouseManager.Instance.mouseoveredTile, Color.red);
        }
        else
        {
            BattlescapeGraphics.ColouringTool.SetColour(MouseManager.Instance.mouseoveredTile, Color.green);
        }
        foreach (Tile tile in MouseManager.Instance.mouseoveredTile.neighbours)
        {
            if (tile.myUnit != null)
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.red);
            }
            else
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.green);
            }
        }


    }


    ////////////////////////////////

    public override void Activate()
    {
        StartCoroutine(HailOfArrows());
    }

    IEnumerator HailOfArrows()
    {
        HOA_Logic();
        StartCoroutine(HOA_Art());
        yield return null;
        FinishUsing();
    }

    IEnumerator HOA_Art()
    {
        Instantiate(LightningFrame, Target.transform.position, LightningFrame.transform.rotation, Target.transform);
        GameObject visual = Helper.FindChildWithTag(gameObject, "Body");
        //myUnit.LookAtTheTarget(Target.transform.position, myUnit.GetComponentInChildren<BodyTrigger>().RotationInAttack);
        myUnit.GetComponent<AnimController>().MyAnimator.SetTrigger("ShootOnce");
        List<GameObject> visuals = new List<GameObject>();
        foreach (Vector3 position in positions)
        {
            yield return new WaitForSeconds(0.2f);
            GameObject copy = Instantiate(visual, position + transform.position, visual.transform.rotation, transform);
            visuals.Add(copy);
            Animator a = copy.GetComponent<Animator>();
            a.SetTrigger("ShootOnce");
        }
        Instantiate(HOA_Arrows, Target.transform.position, HOA_Arrows.transform.rotation, Target.transform);
        yield return new WaitForSeconds(2f);
        foreach (GameObject disposable in visuals)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(disposable);
            }
            else
            {
                Destroy(disposable);
            }

        }
        Log.SpawnLog(myUnit.name + " uses Hail of Arrows!");
    }

    void HOA_Logic()
    {
        myUnit.statistics.bonusAttackRange -= RangeModifier;
        myUnit.statistics.numberOfAttacks = 0;
        if (Target.myUnit != null)
        {
            Debuff(Target.myUnit);
        }
        foreach (Tile tile in Target.neighbours)
        {
            if (tile.myUnit != null)
            {
                Debuff(tile.myUnit);
            }
        }
    }

    void Debuff(UnitScript target)
    {
        Log.SpawnLog(target.name + " gets hit by arrows, receiving a -1 Defence penalty");
        PassiveAbility_Buff.AddBuff(target.gameObject, 2, 0, -1, 0, 100, true, "HailOfArrowsDebuff", BasicVFX, 0, false, true, false);
    }

    protected override bool ActivationRequirements()
    {
        return MouseManager.Instance.mouseoveredTile != null && ShootingScript.WouldItBePossibleToShoot(myUnit, this.transform.position, MouseManager.Instance.mouseoveredTile.transform.position).Key;
    }

    ///////////////////////////////

    public override void AI_Activate(GameObject Target)
    {
        throw new System.NotImplementedException();
    }

    public override GameObject AI_ChooseTarget()
    {
        throw new System.NotImplementedException();
    }

    public override bool AI_IsGoodToUseNow()
    {
        return false;
    }








}
