﻿using System.Collections;
using System.Collections.Generic;
//using OdinSerializer;
using UnityEngine;
using System;


namespace BattlescapeLogic
{
    public class Unit : NewTurnMonoBehaviour
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
        public AbstractAttack attack { get; private set; }
        //NOTE - this needs to be done some other way, this 'readonly' thing will NOT work here :)))
        //Also - IDK why we need it at all
        public readonly int index;
        public Player owner { get; set; }
        [SerializeField] string _unitName;
        public string unitName
        {
            get
            {
                return _unitName;
            }
            private set
            {
                _unitName = value;
            }
        }
        public Tile currentPosition { get; set; }
        public UnitSounds unitSounds;
        public Statistics statistics;
        public List<AbstractAbility> abilities;
        public List<AbstractBuff> buffs { get; private set; }
        public GameObject visuals { get; private set; }
        public GameObject meleeWeaponVisual { get; private set; }
        public Animator animator { get; private set; }
   
        [SerializeField] string _fluffText;
        public string fluffText
        {
            get
            {
                return _fluffText;
            }
            private set
            {
                _fluffText = value;
            }
        }

        [SerializeField] Faction _race;
        public Faction race
        {
            get
            {
                return _race;
            }
            private set
            {
                _race = value;
            }
        }

        protected override void Start()
        {
            base.Start();
            animator = GetComponentInChildren<Animator>();
            visuals = Helper.FindChildWithTag(gameObject, "Body");
            meleeWeaponVisual = Helper.FindChildWithTag(gameObject, "Sword");            
            buffs = new List<AbstractBuff>();
            abilities = new List<AbstractAbility>();
            movement = GetMovementType();
            if (movement == null)
            {
                statistics.NullMaxMovementPoints();
            }

            attack = GetAttackType();
            if (attack == null)
            {
                statistics.NullBaseAttack();
                statistics.NullMaxNumberOfAttacks();
            }
            statistics.healthPoints = statistics.maxHealthPoints;
            statistics.currentEnergy = Statistics.maxEnergy / 2;
            statistics.currentMaxNumberOfRetaliations = statistics.defaultMaxNumberOfRetaliations;
            FaceMiddleOfMap();
        }

        public void FaceMiddleOfMap()
        {
            Vector3 mapMiddle = new Vector3(Map.MapMiddle.x, visuals.transform.position.y, Map.MapMiddle.z);
            transform.LookAt(mapMiddle);
            visuals.transform.LookAt(mapMiddle);
        }

        public override void OnNewTurn()
        {
            statistics.movementPoints = statistics.GetCurrentMaxMovementPoints();
            statistics.numberOfAttacks = statistics.maxNumberOfAttacks;
            statistics.numberOfRetaliations = statistics.currentMaxNumberOfRetaliations;
            statistics.currentEnergy += statistics.energyRegen;
            if (statistics.currentEnergy >= Statistics.maxEnergy)
            {
                statistics.currentEnergy = Statistics.maxEnergy;
            }
        }
        AbstractMovement GetMovementType()
        {
            return Global.instance.movementTypes[(int)movementType];
        }
        AbstractAttack GetAttackType()
        {
            switch (attackType)
            {
                case AttackTypes.Melee:
                    return new MeleeAttack(this);
                case AttackTypes.Ranged:
                    return new ShootingAttack(this);
                case AttackTypes.Instant:
                    return new InstantAttack(this);
                default:
                    //unit cannot attack
                    return null;
            }
        }

        public bool IsAlive()
        {
            return (statistics.healthPoints > 0);
        }

        public bool IsInCombat()
        {
            return currentPosition.IsProtectedByEnemyOf(this);
        }

        public bool IsRanged()
        {
            return attack is ShootingAttack;
        }

        public bool CanStillAttack()
        {
            return (statistics.numberOfAttacks > 0);
        }

        public bool CanStillMove()
        {
            return (statistics.movementPoints > 0);
        }

        public bool CanStillRetaliate()
        {
            return (statistics.numberOfRetaliations > 0);
        }

        public void OnMove(Tile oldTile, Tile newTile)
        {
            //played on EVERY change of tile. Two tiles are used here to avoid confusion in using currentPosition - this will work no matter if we use it slightly before or after movement.
            if (oldTile.IsProtectedByEnemyOf(this) == false && newTile.IsProtectedByEnemyOf(this))
            {
                //Unit just came from SAFETY to COMBAT, so inform it and all of the enemies around about it.
                OnCombatEnter();
                foreach (Tile tile in newTile.neighbours)
                {
                    if (tile.myUnit != null && tile.myUnit.owner.team != this.owner.team)
                    {
                        tile.myUnit.OnCombatEnter();
                    }
                }
            }
            if (oldTile.IsProtectedByEnemyOf(this) && newTile.IsProtectedByEnemyOf(this) == false)
            {
                //Unit just escaped from COMBAT to SAFETY, so lets inform it and all ofthe enemies around the old tile about it.
                OnCombatExit();
                foreach (Tile tile in oldTile.neighbours)
                {
                    if (tile.myUnit != null && tile.myUnit.owner.team != this.owner.team)
                    {
                        tile.myUnit.OnCombatExit();
                    }
                }
            }
        }

        public void OnCombatEnter()
        {
            //Add 'buff' limiting maxMovementPoints to 1;            
            if (attack is ShootingAttack)
            {
                attack = new MeleeAttack(this);
            }
        }

        public void OnCombatExit()
        {
            //Remove 'buff' limiting maxMovementPoints to 1;            
            if (attackType == AttackTypes.Ranged)
            {
                attack = new ShootingAttack(this);
            }
        }
            public void Move(Tile newPosition)
        {
            if (CanStillMove())
            {
                movement.ApplyUnit(this);
                StartCoroutine(movement.MoveTo(newPosition));
            }
        }

        //This should play when this Unit is selected and player clicks on enemy to attack him (and other situations like that)
        public void Attack(Unit target)
        {
            if (CanStillAttack())
            {
                statistics.numberOfAttacks--;
                attack.Attack(target);
            }
        }



        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        //public event Action<Unit, Unit, int> AttackEvent;       

        public void HitTarget(Unit target)
        {
            if (DamageCalculator.IsMiss(this, target))
            {
                // rzucamy buff na target obniżający obronę
                Log.SpawnLog(this.unitName + " attacks " + target.unitName + ", but misses completely!");
                Log.SpawnLog(target.unitName + " loses 1 point of Defence temporarily.");
                PopupTextController.AddPopupText("-1 Defence", PopupTypes.Stats);

            }
            else
            {
                int damage = DamageCalculator.CalculateBasicDamage(this, target);
                Log.SpawnLog(this.unitName + " deals " + damage + " damage to " + target.unitName + "!");
                PopupTextController.AddPopupText("-" + damage, PopupTypes.Damage);
                target.OnHit(this, damage);
            }
            CheckForRetaliation(target);
        }

        void CheckForRetaliation(Unit retaliatingUnit)
        {
            if
                                (
                                    retaliatingUnit.CanStillRetaliate() &&
                                    retaliatingUnit.currentPosition.neighbours.Contains(this.currentPosition) && //Means: is the attack in melee range?
                                    TurnManager.Instance.PlayerHavingTurn != retaliatingUnit.owner.team.index //Means: we cannot retaliate to a retaliation, so we can't retaliate in our own turn
                                    // && check for stopping retaliations in buffs/passives/idk
                                )
            {
                GameStateManager.Instance.StartRetaliationChoice();
            }
        }

        //this should play on attacked unit when it is time it should receive DMG
        public void OnHit(Unit source, int damage)
        {
            ReceiveDamage(source, damage);
        }

        private void ReceiveDamage(Unit source, int damage)
        {
            statistics.healthPoints -= damage;            
            if (IsAlive())
            {
                PlayWoundAnimation();
            }
            else
            {
                Die(source);
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

        public virtual void Die(Unit killer)
        {       
            currentPosition.SetMyUnitTo(null);
            //Note: this makes the Tile 'forget' about the unit, but the dead Unit 'remembers' its last Tile!
            if (currentPosition.IsProtectedByEnemyOf(this))
            {
                foreach (Tile tile in currentPosition.neighbours)
                {
                    if (tile.myUnit != null && tile.myUnit.owner.team != this.owner.team)
                    {
                        if (tile.IsProtectedByEnemyOf(tile.myUnit) == false)
                        {
                            //The guy just got free from combat by death of our Unit;
                            tile.myUnit.OnCombatExit();
                        }
                    }
                }
            }            
            if (MouseManager.Instance.SelectedUnit == this)
            {
                MouseManager.Instance.Deselect();
            }
            killer.owner.AddPoints(statistics.cost);
            HideHealthUI();
            PlayDeathAnimation();
            OnDestruction();
            //whatever else we need to do on death, i guess?
            //definitely a log to log window
        }

        private void HideHealthUI()
        {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }
    }
}

