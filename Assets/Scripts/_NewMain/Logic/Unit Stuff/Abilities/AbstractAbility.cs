using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class AbilityFilter
    {
        [SerializeField] private AbstractAbility thisAbility;
        [SerializeField] private bool ally;
        [SerializeField] private bool enemy;
        [SerializeField] private bool selfPlayer;
        [SerializeField] private bool otherPlayer;
        [SerializeField] private bool self;
        [SerializeField] private bool hero;
        [SerializeField] private bool regular;
        [SerializeField] private bool ranged;
        [SerializeField] private bool melee;
        [SerializeField] private bool ground;
        [SerializeField] private bool flying;

        public bool FilterSelf(Unit unit)
        {
            return self && thisAbility.owner == unit;
        }

    };

    public abstract class AbstractAbility : TurnChangeMonoBehaviour
    {

        [SerializeField] string _abilityName;
        public string abilityName
        {
            get
            {
                return _abilityName;
            }
            protected set
            {
                _abilityName = value;
            }
        }

        [SerializeField] string _description;
        public string description
        {
            get
            {
                return _description;
            }
            protected set
            {
                _description = value;
            }
        }

        [SerializeField] Sprite _icon;
        public Sprite icon
        {
            get
            {
                return _icon;
            }
            protected set
            {
                _icon = value;
            }
        }

        public Unit owner { get; set; }

        [SerializeField] AbilityFilter _filter;
        public AbilityFilter filter
        {
            get
            {
                return _filter;
            }
            protected set
            {
                _filter = value;
            }
        }        

        public override void OnNewRound()
        {
            return;
        }
        public override void OnNewTurn()
        {
            return;
        }
        public override void OnNewPhase()
        {
            return;
        }
    }
}