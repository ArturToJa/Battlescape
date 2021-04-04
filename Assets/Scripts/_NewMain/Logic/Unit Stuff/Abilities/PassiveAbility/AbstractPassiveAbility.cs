using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAbility : AbstractAbility
    {
        [SerializeField] List<AbstractBuff> _placeableBuffs;
        public List<AbstractBuff> placeableBuffs
        {
            get
            {
                return _placeableBuffs;
            }
            protected set
            {
                _placeableBuffs = value;
            }
        }

        protected void ApplyBuffsToUnit(Unit unit)
        {
            foreach (AbstractBuff buffPrefab in placeableBuffs)
            {
                AbstractBuff newBuff = Instantiate(buffPrefab);
                newBuff.ApplyOnTarget(unit);
            }
        }
    }
}
