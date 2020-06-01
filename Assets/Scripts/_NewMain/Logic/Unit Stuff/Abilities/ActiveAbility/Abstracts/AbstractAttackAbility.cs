using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractAttackAbility : AbstractActiveAbility
    {
        [Header("Attack Settings")]
        [Space]
        [SerializeField] protected int bonusDamage;
        [SerializeField] protected List<GameObject> selfBuffs;
        [SerializeField] protected List<GameObject> targetBuffs;

        protected Unit targetedUnit;
        public int damage
        {
            get
            {
                return owner.statistics.baseAttack + bonusDamage;
            }
        }

        protected override void Activate()
        {
            doAnimate = false;
            base.Activate();
        }


        public void ApplyBuffsOnTarget()
        {
            ApplyBuffsToUnit(targetBuffs, targetedUnit);
        }


    }
}
