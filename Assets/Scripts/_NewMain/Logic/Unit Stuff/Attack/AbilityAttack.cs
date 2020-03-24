using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BattlescapeLogic
{
    public class AbilityAttack : AbstractAttack
    {
        ActiveAttackAbility myAbility;
        int damage;


        public AbilityAttack(ActiveAttackAbility _myAbility) : base(_myAbility.owner)
        {
            sourceUnit = _myAbility.owner;
            myAbility = _myAbility;            
        }


        public override void Attack(IDamageable target)
        {
            base.Attack(target);
            damage = myAbility.damage + sourceUnit.statistics.GetCurrentAttack() - targetObject.GetCurrentDefence();
            sourceUnit.statistics.numberOfAttacks--;
            TurnTowardsTarget();
            PlayAttackAnimation();
        }

        //IF the ability is melee, this will trigger
        public override void OnAttackAnimation()
        {
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                Networking.instance.SendCommandToHit(sourceUnit, targetObject, damage);
                sourceUnit.SetAttackToDefault();
            }
        }

        //IF the ability makes the guy shoot, this will trigger
        public override void OnRangedAttackAnimation()
        {
            if (sourceUnit.GetMyOwner().type != PlayerType.Network)
            {
                SpawnMissile(damage);
                sourceUnit.SetAttackToDefault();
            }            
        }

        //Note, this has a Tile as a target and not a Unit - the reason being we might have AOE Abilities targetting 'empty' tiles (or e.g. Obstacles).
        void SpawnMissile(int damage)
        {
            Missile missile = GameObject.Instantiate(sourceUnit.myMissile, sourceUnit.transform.position, sourceUnit.transform.rotation);           
            missile.startingPoint = missile.transform.position;
            //this should actually be SPAWNING POINT on shooter, not SHOOTER POSITION (not middle of a shooter lol)
            missile.sourceUnit = sourceUnit;
            missile.target = targetObject;
            missile.damage = damage;
        }

        protected override void PlayAttackAnimation()
        {
            sourceUnit.animator.SetTrigger(myAbility.animationTrigger);
        }
    }
}