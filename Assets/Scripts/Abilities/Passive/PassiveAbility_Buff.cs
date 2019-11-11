using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Buff : PassiveAbility
{
    public int BuffDuration;

    public int AttackBuffValue = 0;
    public int DefenceBuffValue = 0;
    public int MovementBuffValue = 0;
    bool alreadyRemovedBuff = false;
    protected GameObject vfx;
    public bool IsFrozen = false;
    public bool isNegative;
    public BattlescapeLogic.Unit watchForAttacksBy;
    public int numberOfRetaliations;
    public bool IsRemovedByGettingHit;
    //this is ONLY for BattleCry for now, its generally "unit that if it attacks something will change to this buff"

    protected override void ChangableStart()
    {
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        CombatController.Instance.AttackEvent += OnAttack;
    }

    public void OnGotDestroyed()
    {
        Debug.Log("Destrroyed");
        TurnManager.Instance.NewTurnEvent -= OnNewTurn;
        CombatController.Instance.AttackEvent -= OnAttack;
    }


    protected override void ChangableUpdate()
    {
        if (BuffDuration <= 0 && alreadyRemovedBuff == false)
        {
            UndoBuff();
        }

    }

    public void OnAttack(BattlescapeLogic.Unit Attacker, BattlescapeLogic.Unit Defender)
    {
        // NOTE - for now "IAttack and IGotAttacked are NOT used by anyone, but i can easily imagine a new ability like this in near future!
        if (Attacker == myUnit)
        {
            OnIAttack();
        }
        if (Defender == myUnit)
        {
            OnIGotAttacked();
        }
        if (Attacker == watchForAttacksBy)
        {
            OnMasterAttacked();
        }
    }

    protected virtual void OnIAttack()
    {
        //nothing! Note - nobody cares for this one YET
    }

    protected virtual void OnIGotAttacked()
    {
        if (IsRemovedByGettingHit && alreadyRemovedBuff == false)
        {
            UndoBuff();
        }
    }

    protected virtual void OnMasterAttacked()
    {
        //in fact this is the oNLY one used currently - but not by every buff but only by Battlecry, so NOTHING again ;)
    }
    public void OnNewTurn()
    {
        BuffDuration--;
    }

    public void DoBuff()
    {
        myUnit = GetComponentInParent<BattlescapeLogic.Unit>();
        myUnit.statistics.bonusAttack += AttackBuffValue;
        myUnit.statistics.bonusDefence += DefenceBuffValue;
        //myUnit.GetComponent<BattlescapeLogic.Unit>().IncrimentMoveSpeedBy(MovementBuffValue);
        myUnit.statistics.currentMaxNumberOfRetaliations = numberOfRetaliations;
        if (IsFrozen)
        {
            //myUnit.IsFrozen = true;
        }
    }



    public void TemporarilyUndooBuff()
    {
        myUnit = GetComponentInParent<BattlescapeLogic.Unit>();
        //myUnit.IsFrozen = false;
        myUnit.statistics.bonusAttack -= AttackBuffValue;
        myUnit.statistics.bonusDefence -= DefenceBuffValue;
        //myUnit.GetComponent<BattlescapeLogic.Unit>().IncrimentMoveSpeedBy(-MovementBuffValue);
        myUnit.statistics.currentMaxNumberOfRetaliations = myUnit.statistics.defaultMaxNumberOfRetaliations;
    }

    public void UndoBuff()
    {
        myUnit = GetComponentInParent<BattlescapeLogic.Unit>();
        //myUnit.IsFrozen = false;
        myUnit.statistics.bonusAttack -= AttackBuffValue;
        myUnit.statistics.bonusDefence -= DefenceBuffValue;
        //myUnit.GetComponent<BattlescapeLogic.Unit>().IncrimentMoveSpeedBy(-MovementBuffValue);
        myUnit.statistics.currentMaxNumberOfRetaliations = myUnit.statistics.defaultMaxNumberOfRetaliations;
        alreadyRemovedBuff = true;
        if (vfx != null)
        {
            var Particles = vfx.GetComponentsInChildren<ParticleSystem>();
            foreach (var p in Particles)
            {
                var m = p.main;
                m.loop = false;
            }
            vfx = null;
        }
        OnGotDestroyed();
        if (Application.isEditor)
        {
            DestroyImmediate(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public static void AddBuff(GameObject target, int Time, int Attack, int Defence, int MS, int retalCount, string buffIconName, GameObject vfx, float delayForVFX, bool _IsFrozen, bool _IsNegative, bool doesGettingHitRemove)
    {

        PassiveAbility_Buff theBuff = target.AddComponent<PassiveAbility_Buff>();
        if (buffIconName != string.Empty)
        {
            theBuff.HasIcon = true;
        }
        theBuff.AbilityIconName = buffIconName;
        theBuff.AttackBuffValue = Attack;
        theBuff.DefenceBuffValue = Defence;
        theBuff.MovementBuffValue = MS;
        theBuff.BuffDuration = Time;
        theBuff.IsFrozen = _IsFrozen;
        theBuff.isNegative = _IsNegative;
        theBuff.numberOfRetaliations = retalCount;
        theBuff.IsRemovedByGettingHit = doesGettingHitRemove;
        theBuff.DoBuff();
        theBuff.StartCoroutine(theBuff.DoBuffVFX(theBuff, delayForVFX, target, vfx));

    }

    IEnumerator DoBuffVFX(PassiveAbility_Buff theBuff, float delay, GameObject target, GameObject vfx)
    {
        yield return new WaitForSeconds(delay);
        if (vfx != null)
        {
            theBuff.vfx = Instantiate(vfx, target.transform.position, vfx.transform.rotation, target.transform);
        }
    }

    /*public static void AddBuff(GameObject target, int Time, int Attack, int Defence, int MS, int QCP, string buffIconName, bool _IsFrozen, bool _IsNegative)
    {

        PassiveAbility_Buff theBuff = target.AddComponent<PassiveAbility_Buff>();
        theBuff.HasIcon = true;
        theBuff.AbilityIconName = buffIconName;
        theBuff.AttackBuffValue = Attack;
        theBuff.DefenceBuffValue = Defence;
        theBuff.MovementBuffValue = MS;
        theBuff.QuitCombatChanceBuffValue = QCP;
        theBuff.BuffDuration = Time;
        theBuff.IsFrozen = _IsFrozen;
        theBuff.isNegative = _IsNegative;
        theBuff.DoBuff();
    }*/

    public override int GetAttack(BattlescapeLogic.Unit other)
    {
        return 0;
    }

    public override int GetDefence(BattlescapeLogic.Unit other)
    {
        return 0;
    }
}
