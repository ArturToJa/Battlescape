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
        [SerializeField] private bool smallObstacle; 
        [SerializeField] private bool tallObstacle; 

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

        public bool FilterObstacle(Obstacle obstacle)
        {
            if (tallObstacle && smallObstacle)
            {
                return true;
            }
            else if (obstacle.isTall)
            {
                return tallObstacle && obstacle.isTall == true;
            }
            else
            {
                return smallObstacle && obstacle.isTall == false;
            }
        }

        bool FilterAlly(PlayerTeam team)
        {
            return ally && thisAbility.owner.GetMyOwner().team == team;
        }

        bool FilterEnemy(PlayerTeam team)
        {
            return enemy && thisAbility.owner.GetMyOwner().team != team;
        }



        bool FilterSelfPlayer(Player player)
        {
            return selfPlayer && thisAbility.owner.GetMyOwner() == player;
        }

        bool FilterOtherPlayer(Player player)
        {
            return otherPlayer && thisAbility.owner.GetMyOwner() != player;
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

    public abstract class AbstractAbility : MonoBehaviour
    {
        protected TurnChanger turnChanger;
        public Unit owner { get; set; }

        [SerializeField] string _abilityName;
        public string abilityName
        {
            get
            {
                return _abilityName;
            }
        }

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

        public void Start()
        {
            owner = this.GetComponent<Unit>();
            turnChanger = new TurnChanger(owner.GetMyOwner(), OnNewRound, OnNewTurn, OnNewPhase, OnNewPlayerRound);
            filter.SetAbility(this);
        }

        public virtual void OnNewRound()
        {
            return;
        }
        public virtual void OnNewTurn()
        {
            return;
        }
        public virtual void OnNewPhase()
        {
            return;
        }
        public virtual void OnNewPlayerRound()
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
                newBuff.ApplyOnTarget(target, this);
            }
        }
    }
}
