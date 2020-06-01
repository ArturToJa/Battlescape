using System;
using System.Collections.Generic;
using UnityEngine;
namespace BattlescapeLogic
{
    public class ActiveBuffInAreaAbility : AbstractActiveAbility
    {
        [Header("Buffs")]
        [Space]
        [SerializeField] private List<GameObject> buffsAfterCasting;
        [SerializeField] private List<GameObject> buffsAfterCondition;
        [Header("Conditions")]
        [Space]
        [SerializeField] private int basePhasesToComplteCondition;
        [SerializeField] private Animator anim; 

        private int conditionPhasesLeft;

        [SerializeField] private bool afterAttack;

        private List<Unit> buffedUnits = new List<Unit>();
        
        ///Trigger when Unit is attacking: AnimationEvent or smth idk
        /// <summary>
        /// if(conditionPhasesLeft > 0)
        /// {
        ///     foreach(Unit unit in buffedUnits)
        ///     {
        ///         foreach(GameObject addedBuffs in buffsAfterCasting)
        ///         {
        ///              foreach(AbstractBuff buff in owner.buffs)
        ///              {
        ///                 if(buff.name == addedBuffs.name)
        ///                 {
        ///                     buff.OnExpire();
        ///                 }
        ///              }
        ///         {
        ///     }
        ///     
        ///     
        /// foreach(Unit unit in buffedUnits)
        /// {
        ///     ApplyBuffsToUnit(buffsAfterCondition, unit);
        /// }
        /// 
        /// buffedUnits.Clear();
        ///    
        /// }
        /// </summary>


    protected override void Activate()
        {
            base.Activate();
            conditionPhasesLeft = basePhasesToComplteCondition;
            buffedUnits = GetListOfUnits();

            foreach(Unit unit in buffedUnits)
            {
                ApplyBuffsToUnit(buffsAfterCasting, unit);
            }
        }

        public override void OnNewPhase()
        {
            base.OnNewPhase();

            if (conditionPhasesLeft > 0)
            {
                conditionPhasesLeft--;
            }
            else
            {
                buffedUnits.Clear();
            }
        }

            private List<Unit> GetListOfUnits()
        {
            var list = new List<Unit>();

            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsInRange(tile) && tile.myUnit != null && filter.FilterUnit(tile.myUnit) && filter.FilterTeam(tile.myUnit.GetMyOwner().team))
                {
                    list.Add(tile.myUnit);
                }
            }

            return list;
        }

        public override bool IsLegalTarget(IMouseTargetable target)
        {
            return true;
        }
        public override void ColourPossibleTargets()
        {
            foreach (Tile tile in Global.instance.currentMap.board)
            {
                if (IsInRange(tile))
                {
                    tile.highlighter.TurnOn(targetColouringColour);
                }
            }
        }
        
        
    }
}
