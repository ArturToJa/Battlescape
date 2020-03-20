using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAbility : AbstractAbility
    {
        [SerializeField] List<GameObject> _placeableBuffs;
        public List<GameObject> placeableBuffs
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
            foreach (GameObject buffPrefab in placeableBuffs)
            {
                AbstractBuff newBuff = Instantiate(buffPrefab).GetComponent<AbstractBuff>();
                newBuff.ApplyOnTarget(unit);
            }
        }
    }
}