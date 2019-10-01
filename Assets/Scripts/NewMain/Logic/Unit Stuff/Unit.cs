using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace BattlescapeLogic
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] AttackTypes attackType;
        [SerializeField] MovementTypes movementType;
        public AbstractMovement movement { get; private set; }     
        public BaseAttack attack { get; private set; }
        //NOTE - this needs to be done some other way, this 'readonly' thing will NOT work here :)))
        //Also - IDK why we need it at all
        public readonly int index;
        public Player owner { get; private set; }
        [SerializeField] string _unitName;
        public string unitName { get; private set; }
        public Tile currentPosition { get; set; }
        public Statistics statistics;
        public List<Ability> abilities;
        public List<Buff> buffs { get; private set; }      
        public GameObject visuals { get; private set; }
        public Animator animator { get; private set; }

        public void Start()
        {
            animator = GetComponentInChildren<Animator>();            
        }

        public void Move(Tile newPosition)
        {
            movement = GetMovementType();
            movement.ApplyUnit(this);
            StartCoroutine(movement.MoveTo(newPosition));            
        }

        AbstractMovement GetMovementType()
        {
            switch (movementType)
            {
                case MovementTypes.Ground:
                    //return Ground one;
                    break;
                case MovementTypes.Flying:
                    //return Flying one;
                    break;
                default:
                    Debug.LogError("New movement type not implemented!");
                    return null;
            }
            return null;
        }

        BaseAttack GetAttackType()
        {
            switch (attackType)
            {
                case AttackTypes.Melee:
                    return new BaseAttack(this);
                case AttackTypes.Ranged:
                    return new ShootingAttack(this);
                case AttackTypes.Instant:
                    return new InstantAttack(this);
                default:
                    Debug.LogError("New attack type not implemented!");
                    return null;
            }
        }


        //This should play when this Unit is selected and player clicks on enemy to attack him (and other situations like that)
        public void Attack(Unit target)
        {
            attack = GetAttackType();
            attack.Attack(target);
        }

        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        public event Action<Unit, Unit, int> AttackEvent;       

        //this should play on attacked unit when it is time it should receive DMG
        public void OnHit(Unit source)
        {
            //Currently (in old code system) we should check right now if damage is dealt at all - maybe the attack is a miss (and just reduces Defence).
            int damage = DamageCalculator.instance.CalculateDamage(source, this);
            if (AttackEvent != null)
            {
                AttackEvent(source, this, damage);
            }
            DealDamageToSelf(damage);
        }

        private void DealDamageToSelf(int damage)
        {
            statistics.healthPoints -= damage;
            //show some popup/info in the log window - i actually think we could just 'import' the old ones, they were pretty good ;) if you think they are OK we can just use them ;)
            if (statistics.healthPoints > 0)
            {
                PlayWoundAnimation();
            }
            else
            {
                Die();
            }
        }

        void PlayWoundAnimation()
        {
            animator.SetTrigger("Wound");
        }

        void PlayDeathAnimation()
        {
            animator.SetTrigger("Death");
        }

        public void Die()
        {
            PlayDeathAnimation();
            //whatever else we need to do on death, i guess?
            //definitely a log to log window
        }
            
    }
}

