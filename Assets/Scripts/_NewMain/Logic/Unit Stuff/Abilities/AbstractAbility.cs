using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    [System.Serializable]
    public class AbilityFilter
    {
        AbstractAbility thisAbility;
        [SerializeField] private bool ally;
        [SerializeField] private bool enemy;
        [SerializeField] private bool selfPlayer;
        [SerializeField] private bool otherPlayer;
        [SerializeField] private bool canSelf;
        [SerializeField] private bool hero;
        [SerializeField] private bool regular;
        [SerializeField] private bool ranged;
        [SerializeField] private bool melee;
        [SerializeField] private bool ground;
        [SerializeField] private bool flying;

        public void SetAbility(AbstractAbility ability)
        {
            thisAbility = ability;
        }


        public bool FilterTeam(PlayerTeam team)
        {
            return FilterAlly(team) || FilterEnemy(team);
        }

        public bool FilterPlayer(Player player)
        {
            return FilterSelfPlayer(player) || FilterOtherPlayer(player);
        }

        public bool FilterUnit(Unit unit)
        {
            return FilterSelf(unit) && (FilterHero(unit) || FilterRegular(unit)) && (FilterRanged(unit) || FilterMelee(unit)) && (FilterGround(unit) || FilterFlying(unit));
        }





        bool FilterAlly(PlayerTeam team)
        {
            return ally && thisAbility.owner.owner.team == team;
        }

        bool FilterEnemy(PlayerTeam team)
        {
            return enemy && thisAbility.owner.owner.team != team;
        }



        bool FilterSelfPlayer(Player player)
        {
            return selfPlayer && thisAbility.owner.owner == player;
        }

        bool FilterOtherPlayer(Player player)
        {
            return otherPlayer && thisAbility.owner.owner != player;
        }



        //This only disallows self-targetting if self is checked off.
        bool FilterSelf(Unit unit)
        {
            return canSelf || thisAbility.owner != unit;
        }

        bool FilterHero(Unit unit)
        {
            return hero && unit is Hero;
        }

        bool FilterRegular(Unit unit)
        {
            return regular && ((unit is Hero) == false);
        }

        bool FilterRanged(Unit unit)
        {
            return ranged && unit.attackType == AttackTypes.Ranged;
        }

        bool FilterMelee(Unit unit)
        {
            return melee && unit.attackType == AttackTypes.Melee;
        }

        bool FilterGround(Unit unit)
        {
            return ground && unit.movementType == MovementTypes.Ground;
        }

        bool FilterFlying(Unit unit)
        {
            return flying && unit.movementType == MovementTypes.Flying;
        }


    };

    public abstract class AbstractAbility : TurnChangeMonoBehaviour
    {
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

        public override void OnCreation()
        {
            base.OnCreation();
            filter.SetAbility(this);
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

        protected void ApplyBuffsToUnit(List<GameObject> buffs, Unit target)
        {
            foreach (GameObject buffPrefab in buffs)
            {
                var buffObject = Instantiate(buffPrefab, target.transform);
                var buffObjectBuffs = buffObject.GetComponents<AbstractBuff>();
                if (buffObjectBuffs.Length != 1)
                {
                    Debug.LogError("Wrong count of buffs on buff object: " + buffObject.name + ". Number should be 1, is: " + buffObjectBuffs.Length);
                }
                AbstractBuff newBuff = buffObjectBuffs[0];
                newBuff.ApplyOnUnit(target, this);
            }
        }
    }
}
