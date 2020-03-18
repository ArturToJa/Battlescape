using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public abstract class AbstractPassiveAbility : AbstractAbility
    {
        [SerializeField] protected List<GameObject> placeableBuffs;        
    }
}