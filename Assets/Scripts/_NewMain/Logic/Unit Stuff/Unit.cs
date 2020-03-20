using System.Collections;
using System.Collections.Generic;
//using OdinSerializer;
using UnityEngine;
using System;


namespace BattlescapeLogic
{
    public class Unit : TurnChangeMonoBehaviour, IMouseTargetable, IDamageable
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
        public AbstractAttack attack { get; set; }

        static int counter = -1;
        public int _unitTypeIndex = -1;
        public int unitTypeIndex
        {
            get
            {
                return _unitTypeIndex;
            }
            set
            {
                counter++;
                _unitTypeIndex = counter;
            }
        }

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
        [SerializeField] UnitClass unitClass;
        
        public List<AbstractAbility> abilities;
        public BuffGroup buffs { get; private set; }
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
            buffs = new BuffGroup(this);
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
            UpdateAbilities();
        }

        private void UpdateAbilities()
        {
            foreach (AbstractAbility ability in GetComponents<AbstractAbility>())
            {
                abilities.Add(ability);
                ability.owner = this;
            }
        }

        public void FaceMiddleOfMap()
        {
            Vector3 mapMiddle = new Vector3(Map.MapMiddle.x, visuals.transform.position.y, Map.MapMiddle.z);
            transform.LookAt(mapMiddle);
            visuals.transform.LookAt(mapMiddle);
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

        public bool CanAttackOrMoveNow()
        {
            if (GameRound.instance.currentPhase == TurnPhases.Movement)
            {
                return CanStillMove();
            }
            if (GameRound.instance.currentPhase == TurnPhases.Attack)
            {
                return (IsRanged() || IsInCombat()) && CanStillAttack();
            }
            else return false;
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

        //Played BEFORE the first step in the whole movement. Check for ExitCombat here cause it only makes sense here.
        //Returns true if movement is exiting combat
        public bool IsExittingCombat(Tile newPosition)
        {
            return (currentPosition.IsProtectedByEnemyOf(this) && (newPosition.IsProtectedByEnemyOf(this) == false));            
        }

        public void ExitCombat()
        {
            OnCombatExit();
            foreach (Tile tile in currentPosition.neighbours)
            {
                if (tile.myUnit != null && IsEnemyOf(tile.myUnit) && tile.IsProtectedByEnemyOf(tile.myUnit) == false)
                {
                    tile.myUnit.OnCombatExit();
                }
            }
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
                    if (tile.myUnit != null && IsEnemyOf(tile.myUnit))
                    {
                        tile.myUnit.OnCombatEnter();
                    }
                }
            }
            
        }

        public void OnCombatEnter()
        {         
            if (attackType == AttackTypes.Ranged)
            {
                attack = new MeleeAttack(this);
            }            
            animator.SetBool("InCombat", true);
        }

        public void OnCombatExit()
        {
            animator.SetBool("InCombat", false);            
            if (IsAlive())
            {          
                if (attackType == AttackTypes.Ranged)
                {
                    attack = new ShootingAttack(this);
                }
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
            statistics.numberOfAttacks--;
            attack.Attack(target);
        }

        public void RetaliateTo(Unit target)
        {
            Log.SpawnLog(this.name + " strikes back!");
            statistics.numberOfRetaliations--;
            attack.Attack(target);
            Networking.instance.FinishRetaliation();
        }

        //This is the attack on enemy exiting combat with us... Needed to separate it to a) change chances and b) calculate damage beforehand (to be able to know if I can or cannot quit combat)
        public void Backstab(Unit target, int damage)
        {
            attack = new BackstabAttack(attack, damage,this);
            attack.Attack(target);
        }



        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        //public event Action<Unit, Unit, int> AttackEvent;       


            //if damage is 0, it's a miss, if it's somehow TOTALLY blocked it could be negative maybe or just not send this.
        public void HitTarget(Unit target, int damage)
        {
            PlayerInput.instance.isInputBlocked = false;
            if (damage == 0)
            {
                StatisticChangeBuff defenceDebuff = Instantiate(Resources.Load("Buffs/MechanicsBuffs/Combat Wound") as GameObject).GetComponent<StatisticChangeBuff>();
                defenceDebuff.ApplyOnTarget(target);
                Log.SpawnLog(this.unitName + " attacks " + target.unitName + ", but misses completely!");
                Log.SpawnLog(target.unitName + " loses 1 point of Defence temporarily.");
                PopupTextController.AddPopupText("-1 Defence", PopupTypes.Stats);

            }
            else if (damage > 0)
            {
                Log.SpawnLog(this.unitName + " deals " + damage + " damage to " + target.unitName + "!");
                PopupTextController.AddPopupText("-" + damage, PopupTypes.Damage);
                target.TakeDamage(this, damage);
                foreach(AbstractBuff buff in target.buffs.FindAllBuffsOfType("Combat Wound"))
                {
                    buff.RemoveFromTargetInstantly();
                }
            }
            if (IsRetaliationPossible(target) && owner.type != PlayerType.Network)
            {
                Networking.instance.SendCommandToGiveChoiceOfRetaliation(target, this);
            }

        }

        bool IsRetaliationPossible(Unit retaliatingUnit)
        {
            return
                (
                    retaliatingUnit.CanStillRetaliate() &&
                    retaliatingUnit.currentPosition.neighbours.Contains(this.currentPosition) && //Means: is the attack in melee range?
                    GameRound.instance.currentPlayer != retaliatingUnit.owner //Means: we cannot retaliate to a retaliation, so we can't retaliate in our own turn
                                                                              // && check for stopping retaliations in buffs/passives/idk
                );

        }       

        //this should play on attacked unit when it is time it should receive DMG
        public void TakeDamage(Unit source, int damage)
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
                    if (tile.myUnit != null && IsEnemyOf(tile.myUnit) && tile.IsProtectedByEnemyOf(tile.myUnit) == false)
                    {
                        //The enemy dude exists and just got free from combat by death of our Unit;
                        tile.myUnit.OnCombatExit();
                    }
                }
            }
            if (MouseManager.instance.selectedUnit == this)
            {
                MouseManager.instance.unitSelector.DeselectUnit();
            }
            killer.owner.AddPoints(statistics.cost);
            HideHealthUI();
            PlayDeathAnimation();
            OnDestruction();
            //whatever else we need to do on death, i guess?
            //definitely a log to log window
        }

        void HideHealthUI()
        {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }

        public bool IsInAttackRange(Vector3 target)
        {
            Bounds FullRange = new Bounds(this.transform.position, new Vector3(2 * this.statistics.GetCurrentAttackRange() + 0.25f, 5, 2 * this.statistics.GetCurrentAttackRange() + 0.25f));
            if (this.statistics.minimalAttackRange > 0)
            {
                Bounds miniRange = new Bounds(this.transform.position, new Vector3(2 * this.statistics.minimalAttackRange + 0.25f, 5, 2 * this.statistics.minimalAttackRange + 0.25f));
                return miniRange.Contains(target) == false && FullRange.Contains(target);
            }
            else
            {
                return FullRange.Contains(target);
            }
        }

        public bool HasClearView(Vector3 defender)
        {
            foreach (var targetable in Global.FindAllTargetablesInLine(transform.position,defender))
            {
                var obstacle = targetable as Obstacle;

                if (obstacle != null && obstacle.isTall)
                {
                    return false;
                }
            }

            return true;
        }

        public bool IsEnemyOf(Unit other)
        {
            return owner.team != other.owner.team;
        }

        public override void OnNewRound()
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

        public override void OnNewTurn()
        {
            return;
        }

        public override void OnNewPhase()
        {
            return;
        }

        public int CompareUnitClass(Unit other)
        {
            int myClass = 0;
            int otherClass = 0;
            if (this is Hero || this.unitClass == UnitClass.Special)
            {
                myClass++;
            }
            if (this.unitClass == UnitClass.Cannonmeat)
            {
                myClass--;
            }
            if (other is Hero|| other.unitClass == UnitClass.Special)
            {
                otherClass++;
            }
            if (other.unitClass == UnitClass.Cannonmeat)
            {
                otherClass--;
            }
            return myClass - otherClass;
        }
    }
    public enum UnitClass
    {
        Special, Normal, Cannonmeat
    }
}

