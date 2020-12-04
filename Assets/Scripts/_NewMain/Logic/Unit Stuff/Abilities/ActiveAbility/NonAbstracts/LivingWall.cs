using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class LivingWall : AbstractActiveNoTargetAbility
    {
        [SerializeField] GameObject buffPrefab;


        public override void DoAbility()
        {
            ApplyBuffToUnit(buffPrefab, owner);
        }
        
    }
}