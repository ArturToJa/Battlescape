using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveAbility_Buff : PassiveAbility
{
    public int BuffDuration;

    public int AttackBuffValue = 0;
    public int DefenceBuffValue = 0;
    public int MovementBuffValue = 0;
    public int QuitCombatChanceBuffValue = 0;
    bool alreadyRemovedBuff = false;
    protected GameObject vfx;
    public bool IsFrozen = false;
    public bool isNegative;
    public UnitScript watchForAttacksBy;
    public bool CanRetal;
    public bool IsRemovedByGettingHit;
    //this is ONLY for BattleCry for now, its generally "unit that if it attacks something will change to this buff"

    protected override void ChangableStart()
    {
        TurnManager.Instance.NewTurnEvent += OnNewTurn;
        CombatController.Instance.AttackEvent += OnAttack;
        myUnit.DeathEvent += OnGotDestroyed;
    }

    public void OnGotDestroyed()
    {
        Debug.Log("Destrroyed");
        TurnManager.Instance.NewTurnEvent -= OnNewTurn;
        CombatController.Instance.AttackEvent -= OnAttack;
        myUnit.DeathEvent -= OnGotDestroyed;
    }


    protected override void ChangableUpdate()
    {
        if (BuffDuration <= 0 && alreadyRemovedBuff == false)
        {
            UndoBuff();
        }

    }

    public void OnAttack(UnitScript Attacker, UnitScript Defender, int damage)
    {
        // NOTE - for now "IAttack and IGotAttacked are NOT used by anyone, but i can easily imagine a new ability like this in near future!
        if (Attacker == myUnit)
        {
            OnIAttack(damage);
        }
        if (Defender == myUnit)
        {
            OnIGotAttacked(damage);
        }
        if (Attacker == watchForAttacksBy)
        {
            OnMasterAttacked(damage);
        }
    }

    protected virtual void OnIAttack(int damage)
    {
        //nothing! Note - nobody cares for this one YET
    }

    protected virtual void OnIGotAttacked(int damage)
    {
        if (IsRemovedByGettingHit && alreadyRemovedBuff == false)
        {
            UndoBuff();
        }
    }

    protected virtual void OnMasterAttacked(int damage)
    {
        //in fact this is the oNLY one used currently - but not by every buff but only by Battlecry, so NOTHING again ;)
    }
    public void OnNewTurn()
    {
        BuffDuration--;
    }

    public void DoBuff()
    {
        myUnit = GetComponentInParent<UnitScript>();
        myUnit.statistics.bonusAttack += AttackBuffValue;
        myUnit.statistics.bonusDefence += DefenceBuffValue;
        myUnit.QuitCombatPercent += QuitCombatChanceBuffValue;
        //myUnit.GetComponent<UnitScript>().IncrimentMoveSpeedBy(MovementBuffValue);
        myUnit.DoesRetaliate = CanRetal;
        if (IsFrozen)
        {
            myUnit.IsFrozen = true;
        }
    }



    public void TemporarilyUndooBuff()
    {
        myUnit = GetComponentInParent<UnitScript>();
        myUnit.IsFrozen = false;
        myUnit.statistics.bonusAttack -= AttackBuffValue;
        myUnit.statistics.bonusDefence -= DefenceBuffValue;
        myUnit.QuitCombatPercent -= QuitCombatChanceBuffValue;
        //myUnit.GetComponent<UnitScript>().IncrimentMoveSpeedBy(-MovementBuffValue);
        myUnit.DoesRetaliate = myUnit.DoesRetalByDefault;
    }

    public void UndoBuff()
    {
        myUnit = GetComponentInParent<UnitScript>();
        myUnit.IsFrozen = false;
        myUnit.statistics.bonusAttack -= AttackBuffValue;
        myUnit.statistics.bonusDefence -= DefenceBuffValue;
        myUnit.QuitCombatPercent -= QuitCombatChanceBuffValue;
        //myUnit.GetComponent<UnitScript>().IncrimentMoveSpeedBy(-MovementBuffValue);
        myUnit.DoesRetaliate = myUnit.DoesRetalByDefault;
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

    public static void AddBuff(GameObject target, int Time, int Attack, int Defence, int MS, int QCP, bool canRetal, string buffIconName, GameObject vfx, float delayForVFX, bool _IsFrozen, bool _IsNegative, bool doesGettingHitRemove)
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
        theBuff.QuitCombatChanceBuffValue = QCP;
        theBuff.BuffDuration = Time;
        theBuff.IsFrozen = _IsFrozen;
        theBuff.isNegative = _IsNegative;
        theBuff.CanRetal = canRetal;
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

    public override int GetAttack(UnitScript other)
    {
        return 0;
    }

    public override int GetDefence(UnitScript other)
    {
        return 0;
    }
}
