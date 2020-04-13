using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeLogic;

public class Ability_Elves_Wolf_Howl : Ability_Basic
{
    [SerializeField] int Duration;
    [SerializeField] int AttackBuff;
    [SerializeField] int DefenceBuff;
    [SerializeField] Sound[] HowlSounds;
    AudioSource HowlSource;
    public GameObject BuffVFX;
    [SerializeField] int RangeBetweenWolves = 4;
    [SerializeField] float TimeBetweenHowls;
    protected override void OnStart()
    {
        HowlSource = gameObject.AddComponent<AudioSource>();
        //Target = myUnit.currentPosition;
    }

    protected override void OnUpdate()
    {
        return;
    }
    public override void OnHover()
    {
        //TileBattlescapeGraphics.ColouringTool.UncolourAllTiles();
        ColourTiles();
    }
    protected override void ColourTiles()
    {
        //foreach (BattlescapeLogic.Unit ally in Helper.GetAlliesInRange(myUnit, RangeBetweenWolves))
        //{
        //    //if (ally.unitUnit.GetMyObject<Unit>()ID == UnitID.Wolf)
        //    //{
        //    //    BattlescapeGraphics.ColouringTool.SetColour(ally.currentPosition,Color.green);
        //    //}
        //}
        //foreach (BattlescapeLogic.Unit enemy in Helper.GetEnemiesInRange(myUnit, RangeBetweenWolves))
        //{
        //    //if (enemy.unitUnit.GetMyObject<Unit>()ID == UnitID.Wolf)
        //    //{
        //    //    BattlescapeGraphics.ColouringTool.SetColour(enemy.currentPosition, Color.green);                
        //    //}
        //}
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

    public override bool ActivationRequirements()
    {
        return true;
    }

    public override void Activate()
    {
        StartCoroutine(HowlRoutine());        
    }

    IEnumerator HowlRoutine()
    {
        DoMyArt();
        yield return null;
        FinishUsing();
        yield return new WaitForSeconds(TimeBetweenHowls);
        foreach (Tile tile in Global.instance.currentMap.board)
        {
            //    if (
            //        (Mathf.Abs(tile.transform.position.x - myUnit.currentPosition.transform.position.x) <= RangeBetweenWolves)
            //        &&
            //        (Mathf.Abs(tile.transform.position.z - myUnit.currentPosition.transform.position.z) <= RangeBetweenWolves)
            //        && tile != myUnit.currentPosition
            //        && tile.GetMyObject<Unit>() != null
            //       // && tile.GetMyObject<Unit>().unitUnit.GetMyObject<Unit>()ID == UnitID.Wolf
            //        && CheckIfNoHowlYetOn(tile.GetMyObject<Unit>())
            //        )
            //    {

            //        StartCoroutine(DoOtherWolfStuff(tile.GetMyObject<Unit>()));
            //        yield return new WaitForSeconds(TimeBetweenHowls);
            //    }
            //
        }

    }

    bool CheckIfNoHowlYetOn(BattlescapeLogic.Unit unit)
    {
        foreach (PassiveAbility_Buff buff in unit.GetComponents<PassiveAbility_Buff>())
        {
            if (buff.AbilityIconName == "WolfBuff")
            {
                return false;
            }
        }
        return true;
    }
    void DoMyArt()
    {
        //myUnit.GetComponent<AnimController>().Cast();
    }

    IEnumerator DoOtherWolfStuff(BattlescapeLogic.Unit wolf)
    {

        Log.SpawnLog("Nearby wolf gets affected by a powerfull Howl!");
        //wolf.myUnit.GetComponent<AnimController>().Cast();
        //PassiveAbility_Buff.AddBuff(wolf.gameObject, Duration, AttackBuff, DefenceBuff, 0, wolf.baseQuitCombatPercent, true, "WolfBuff", BuffVFX, 0, false, false, false);
        GameObject vfx1 = CreateVFXOn(wolf.transform, BasicVFX.transform.rotation);
        yield return new WaitForSeconds(6);

        if (Application.isEditor)
        {
            DestroyImmediate(vfx1);
        }
        else
        {
            Destroy(vfx1);
        }

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

    public override bool AI_IsGoodToUseNow()
    {
        int wolfCount = 0;
        foreach (Tile tile in Global.instance.currentMap.board)
        {
            //if (
            //    (Mathf.Abs(tile.transform.position.x - myUnit.currentPosition.transform.position.x) <= RangeBetweenWolves)
            //    &&
            //    (Mathf.Abs(tile.transform.position.z - myUnit.currentPosition.transform.position.z) <= RangeBetweenWolves)
            //    && tile != myUnit.currentPosition
            //    && tile.GetMyObject<Unit>() != null
            //    //&& tile.GetMyObject<Unit>().unitUnit.GetMyObject<Unit>()ID == UnitID.Wolf
            //    && tile.GetMyObject<Unit>().GetMyOwner() == myUnit.GetMyOwner()
            //    )
            //{
            //    wolfCount++;
            //}
        }
        return wolfCount >= 2;
    }

    protected override void SetTarget()
    {
        // Due to specifics of this ability, this function does NOT ever run apparently, therefore we just set target on Start ;).
        return;
    }

    public void Howl()
    {
        Sound s = HowlSounds[Random.Range(0, HowlSounds.Length)];
        s.oldSource = HowlSource;
        s.oldSource.clip = s.clip;
        s.oldSource.volume = s.volume;
        s.oldSource.Play();
    }


}
