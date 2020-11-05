using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractAttackModifierBuff : AbstractBuff
    {
        //THIS is not for giving stat changes - a different type of buff might be used and deffinitely a different function (ApplyChange).
        //THIS has no 'undo' - its for applying effects that persist (aoe, onhit effects, weird stuff liek lifesteal etc) not things  you would remove after attack.
        public abstract void ModifyAttack(IDamageable targetObject, int damageToTarget);

    }
}