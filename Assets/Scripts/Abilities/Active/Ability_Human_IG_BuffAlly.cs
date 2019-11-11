using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Human_IG_BuffAlly : Ability_Basic
{
    public GameObject BuffVFX;

    protected override void OnStart()
    {
        return;
    }

    protected override void OnUpdate()
    {        
        return;
    }

    protected override bool IsUsableNow()
    {
        foreach (Tile tile in myUnit.currentPosition.neighbours)
        {
            if (tile.myUnit != null && tile.myUnit.owner == myUnit.owner)
            {
                return true;
            }
        }
        return false;
    }

    protected override void CancelUse()
    {
        return;
    }

    protected override void Use()
    {
        return;
    }

    protected override void SetTarget()
    {
        Target = MouseManager.Instance.mouseoveredTile;
    }


    public override void Activate()
    {        
        StartCoroutine(BuffAlly(Target.myUnit));
    }

    protected override bool ActivationRequirements()
    {
        return MouseManager.Instance.mouseoveredTile != null && IsLegalTarget(MouseManager.Instance.mouseoveredTile);
    }

    protected override void ColourTiles()
    {
        foreach (Tile tile in myUnit.currentPosition.neighbours)
        {
            if (IsLegalTarget(tile))
            {
                BattlescapeGraphics.ColouringTool.SetColour(tile, Color.green);
            }
        }
    }

    bool IsLegalTarget(Tile tile)
    {       
        return tile.myUnit != null && tile.myUnit.owner == myUnit.owner && myUnit.currentPosition.neighbours.Contains(tile);
    }

    IEnumerator BuffAlly(BattlescapeLogic.Unit ally)
    {
        Log.SpawnLog(myUnit.name + " empowers " + ally.name + ", giving a +2 bonus to Attack and Defence till next turn");
        PlayAbilitySound();
        GameObject vfx1 = CreateVFXOn(ally.transform, BasicVFX.transform.rotation);
        GetComponent<AnimController>().Cast();
        PassiveAbility_Buff.AddBuff(ally.gameObject, 2, 2, 2, 0, myUnit.statistics.currentMaxNumberOfRetaliations, "IGBuff", BuffVFX, 0, false, false, false);
        yield return null;
        FinishUsing();
        yield return new WaitForSeconds(15);

        if (Application.isEditor)
        {
            DestroyImmediate(vfx1);
        }
        else
        {
            Destroy(vfx1);
        }
    }


    // AI  section!!

    public override bool AI_IsGoodToUseNow()
    {
        // this ability is good enough for now that i want to use it every turn if possible cause why not
        return IsUsableNow();
    }

    public override void AI_Activate(GameObject Target)
    {
        AlreadyUsedThisTurn = true;
        StartCoroutine(BuffAlly(Target.GetComponent<BattlescapeLogic.Unit>()));
    }

    public override GameObject AI_ChooseTarget()
    {
        // no idea really, on what criteria this AI should choose correct unit - but maybe You, dear reader, will know any better ;/ For now Random will do.
        return /*myUnit.AllyList[Random.Range(0, myUnit.AllyList.Count)].gameObject*/null;
    }
}
