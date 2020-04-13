using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Hero_Warrior_TheWall : Ability_Basic
{
    [SerializeField] GameObject vfx;

    // This ability should NOT have cancel use - it should happen imidiately
    protected override void OnStart()
    {
        //Target = myUnit.currentPosition;
    }

    protected override void OnUpdate()
    {
        return;
    }

    protected override bool IsUsableNow()
    {
        return true;
    }

    protected override void Use()
    {
        SendCommandForActivation();
    }

    protected override void CancelUse()
    {
        return;
    }

    protected override void SetTarget()
    {
        // Due to specifics of this ability, this function does NOT ever run apparently, therefore we just set target on Start ;).
        return;
    }

    public override bool ActivationRequirements()
    {
        return true;
    }

    public override void Activate()
    {
        StartCoroutine(TheWall());
    }

    IEnumerator TheWall()
    {
        Log.SpawnLog(myUnit.name + " uses The Wall, becoming invulnerable for 2 turns");
        DoArtisticStuff();
        DoLogic();
        yield return null;
        FinishUsing();

    }

    void DoLogic()
    {
        //myUnit.BlockHitsForTurns(2);
    }

    void DoArtisticStuff()
    {
        //myUnit.GetComponent<AnimController>().Cast();
        PlayAbilitySound();
        CreateVFXOn(transform, transform.rotation);
        PassiveAbility_Buff.AddBuff(gameObject, 2, 0, 0, 0, myUnit.statistics.currentMaxNumberOfRetaliations, "WarriorWallBuff", vfx, 3f, false, false, false);
    }

    public override bool AI_IsGoodToUseNow()
    {
        return /*(myUnit.EnemyList.Count >= 2) || (myUnit.EnemyList.Count == 1 && myUnit.statistics.healthPoints == 1)*/false;
    }

    public override void AI_Activate(GameObject Target)
    {
        SendCommandForActivation();
    }

    public override GameObject AI_ChooseTarget()
    {
        // this function does not even need this override xD but this is abstract so that i never forget to add it where it IS necessary.
        return null;
    }
}
