using System.Collections;
using System.Collections.Generic;
using OdinSerializer;
using UnityEngine;
using System;


namespace BattlescapeLogic
{
    public class Unit : SerializedMonoBehaviour
    {
        [SerializeField] GameObject _missilePrefab;
        public GameObject missilePrefab
        {
            get
            {
                return _missilePrefab;
            }
            private set
            {
                _missilePrefab = value;
            }
        }
        [SerializeField] public AttackTypes attackType;
        [SerializeField] public MovementTypes movementType;
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
        public List<AbstractAbility> abilities;
        public List<Buff> buffs { get; private set; }      
        public GameObject visuals { get; private set; }
        public Animator animator { get; private set; }

        public void Start()
        {
            animator = GetComponentInChildren<Animator>();

            movement = GetMovementType();
            if (movement == null)
            {
                statistics.NullMaxMovementPoints();
            }

            attack = GetAttackType();
            if(attack == null)
            {
                statistics.NullBaseAttack();
            }
        }

        AbstractMovement GetMovementType()
        {
            return Global.instance.movementTypes[(int)movementType];
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
                    //unit cannot attack
                    return null;
            }
        }

        public void Move(Tile newPosition)
        {
            movement.ApplyUnit(this);
            StartCoroutine(movement.MoveTo(newPosition));
        }

        //This should play when this Unit is selected and player clicks on enemy to attack him (and other situations like that)
        public void Attack(Unit target)
        {
            attack.Attack(target);
        }

        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        //public event Action<Unit, Unit, int> AttackEvent;       

        public void HitTarget(Unit target)
        {
            if(DamageCalculator.IsMiss(this, target))
            {
                // rzucamy buff na target obniżający obronę
            }
            else
            {
                int damage = DamageCalculator.CalculateBasicDamage(this, target);
                target.OnHit(this, damage);
            }
        }

        //this should play on attacked unit when it is time it should receive DMG
        public void OnHit(Unit source, int damage)
        {
            ReceiveDamage(damage);
        }

        private void ReceiveDamage(int damage)
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

