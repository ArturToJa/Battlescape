using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public BattlescapeLogic.Unit Owner;
    public GameObject ItemHidden;
    public GameObject ItemInUse;
    public int AttackBonus;
    public int DefenceBonus;
    public int DamageBonus;
    public int SpeedBonus;

    public void SetVisuallyInUse(bool inUse)
    {
        ItemInUse.SetActive(inUse);
        ItemHidden.SetActive(!inUse);       
    }

    public void SetLogicallyInUse(bool inUse)
    {
        if (inUse)
        {
            Owner.statistics.bonusAttack += AttackBonus;
            Owner.statistics.bonusDefence += DefenceBonus;
            //Owner.CurrentDamage += DamageBonus;
            //Owner.GetComponent<BattlescapeLogic.Unit>().IncrimentMoveSpeedBy(SpeedBonus);
        }
        else
        {
            Owner.statistics.bonusAttack -= AttackBonus;
            Owner.statistics.bonusDefence -= DefenceBonus;
            //Owner.CurrentDamage -= DamageBonus;
            //Owner.GetComponent<BattlescapeLogic.Unit>().IncrimentMoveSpeedBy(-SpeedBonus);

        }
    }
}
[System.Serializable]
public class Weapon:Item
{
    public BattlescapeLogic.AttackTypes attackType;        
}
