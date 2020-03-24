using System.Collections;
using System.Collections.Generic;
//using OdinSerializer;
using UnityEngine;
using System;


namespace BattlescapeLogic
{
    public class Unit : TurnChangeMonoBehaviour, IMouseTargetable, IDamageable, IActiveEntity
    {
        [SerializeField] GameObject _missilePrefab;

        Missile _myMissile;
        public Missile myMissile
        {
            get
            {
                if (_myMissile == null)
                {
                    _myMissile = _missilePrefab.GetComponent<Missile>();
                }
                return _myMissile;
            }            
        }

        [SerializeField] public AttackTypes attackType;
        [SerializeField] public MovementTypes movementType;
        public AbstractMovement movement { get; private set; }
        public AbstractAttack attack { get; set; }       

        Player owner { get; set; }

        [SerializeField] UnitInfo _info;
        public UnitInfo info
        {
            get
            {
                return _info;
            }
            private set
            {
                info = value;
            }
        }

        public Tile currentPosition { get; set; }
        public UnitSounds unitSounds;
        public Statistics statistics;

        public List<AbstractAbility> abilities { get; private set; }
        public BuffGroup buffs { get; private set; }
        public GameObject visuals { get; private set; }
        [SerializeField] Equipment _equipment;
        public Equipment equipment
        {
            get
            {
                return _equipment;
            }
            private set
            {
                _equipment = value;
            }
        }
        public Animator animator { get; private set; }        

        [SerializeField] Race _race;
        public Race race
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

        public static event Action<Unit> OnUnitSelected = delegate { };
        public static event Action OnUnitDeselected = delegate { };

        protected override void Start()
        {
            equipment.EquipPrimaryWeapon();
            base.Start();
            animator = GetComponentInChildren<Animator>();
            visuals = Helper.FindChildWithTag(gameObject, "Body");
            buffs = new BuffGroup(this);
            abilities = new List<AbstractAbility>();
            movement = GetMovementType();
            if (movement == null)
            {
                statistics.NullMaxMovementPoints();
            }

            SetAttackToDefault();
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
            Tile.OnMouseHoverTileEnter += OnTileHovered;
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
            Vector3 mapMiddle = new Vector3((Global.instance.currentMap.mapWidth - 1) / 2, visuals.transform.position.y, (Global.instance.currentMap.mapHeight - 1) / 2);
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

        public Tile GetMyTile()
        {
            return currentPosition;
        }

        public void SetAttackToDefault()
        {
            attack = GetAttackType();
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
        public void Attack(IDamageable target)
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
            attack = new BackstabAttack(attack, damage, this);
            attack.Attack(target);
        }



        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        //public event Action<Unit, Unit, int> AttackEvent;       


        //if damage is 0, it's a miss, if it's somehow TOTALLY blocked it could be negative maybe or just not send this.
        public void HitTarget(IDamageable target, int damage)
        {
            foreach (AbstractAttackModifierBuff modifierBuff in buffs)
            {
                modifierBuff.ModifyAttack(target, damage);
            }
            PlayerInput.instance.isInputBlocked = false;
            if (damage == 0)
            {
                StatisticChangeBuff defenceDebuff = Instantiate(Resources.Load("Buffs/MechanicsBuffs/Combat Wound") as GameObject).GetComponent<StatisticChangeBuff>();
                defenceDebuff.ApplyOnTarget(target);
                Log.SpawnLog(this.info.unitName + " attacks " + target.GetMyName() + ", but misses completely!");
                Log.SpawnLog(target.GetMyName() + " loses 1 point of Defence temporarily.");
                PopupTextController.AddPopupText("-1 Defence", PopupTypes.Stats);

            }
            else if (damage > 0)
            {
                Log.SpawnLog(this.info.unitName + " deals " + damage + " damage to " + target.GetMyName() + "!");
                PopupTextController.AddPopupText("-" + damage, PopupTypes.Damage);
                target.TakeDamage(this, damage);
                foreach(AbstractBuff buff in target.buffs.FindAllBuffsOfType("Combat Wound"))
                {
                    buff.RemoveFromTargetInstantly();
                }
            }
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit.CanRetaliate(this) && owner.type != PlayerType.Network)
                {
                    Networking.instance.SendCommandToGiveChoiceOfRetaliation(targetUnit, this);
                }
            }
            

        }

        public bool CanRetaliate(Unit retaliatingUnit)
        {
            return
                (
                    retaliatingUnit.CanStillRetaliate() &&
                    retaliatingUnit.currentPosition.neighbours.Contains(this.currentPosition) && //Means: is the attack in melee range?
                    GameRound.instance.currentPlayer != retaliatingUnit.GetMyOwner() //Means: we cannot retaliate to a retaliation, so we can't retaliate in our own turn
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
            if (GameRound.instance.currentPlayer.selectedUnit == this)
            {
                owner.DeselectUnit();
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

        public bool IsInAttackRange(int distance)
        {            
            return distance <= statistics.GetCurrentAttackRange() && distance >= statistics.minimalAttackRange;            
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

        public bool IsEnemyOf(IDamageable other)
        {
            return other.GetMyOwner() == null || owner.team != other.GetMyOwner().team;
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
            if (owner.selectedUnit == this)
            {
                owner.DeselectUnit();
            }
        }

        public void OnRightClick(IMouseTargetable target)
        {
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit.IsAlive())
                {
                    EnemyTooltipHandler.instance.SetOnFor(targetUnit);
                }
            }

        }

        public void OnLeftClick(IMouseTargetable target)
        {
            if (target is IDamageable)
            {
                var targetObject = target as IDamageable;
                if (attack.CanAttack(targetObject))
                {
                    Networking.instance.SendCommandToStartAttack(this, targetObject);
                    statistics.numberOfAttacks--;
                }
            }
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit == this)
                {
                    targetUnit.GetMyOwner().DeselectUnit();
                }
                else if (targetUnit.GetMyOwner().team == owner.team)
                {
                    targetUnit.GetMyOwner().SelectUnit(targetUnit);
                }
                
            }
            else if (target is Tile)
            {
                var targetTile = target as Tile;
                if (CanMoveTo(targetTile))
                {
                    Networking.instance.SendCommandToMove(this, targetTile);
                }

            }
        }

        public bool CanBeSelected()
        {
            return PlayerInput.instance.isInputBlocked == false && owner.IsCurrentLocalPlayer() && GameRound.instance.currentPhase != TurnPhases.Enemy && GameRound.instance.currentPhase != TurnPhases.None;
        }

        public void OnSelection()
        {
            OnUnitSelected(this);
            if (GameRound.instance.currentPlayer.type != PlayerType.AI)
            {
                PlaySelectionSound();
            }
        }

        public void OnDeselection()
        {
            OnUnitDeselected();
            Global.instance.currentEntity = GameRound.instance.currentPlayer;
        }

        void PlaySelectionSound()
        {
            BattlescapeSound.SoundManager.instance.PlaySound(GameRound.instance.currentPlayer.selectedUnit.gameObject, BattlescapeSound.SoundManager.instance.selectionSound);
        }

        public bool CanMoveTo(Tile tile)
        {
            movement.ApplyUnit(this);
            return movement.CanMoveTo(tile);
        }

        void OnTileHovered(Tile hoveredTile)
        {
            if (GameRound.instance.currentPlayer.selectedUnit == this)
            {
                if (CanMoveTo(hoveredTile))
                {
                    foreach (Unit otherUnit in Global.instance.GetAllUnits())
                    {
                        if (IsEnemyOf(otherUnit) && CouldAttackEnemyFromTile(otherUnit, hoveredTile))
                        {
                            BattlescapeGraphics.ColouringTool.ColourObject(otherUnit, Color.red);
                        }
                    }
                }
            }
        }

        public void OnMouseHoverEnter()
        {
            BattlescapeGraphics.ColouringTool.ColourUnitAsAllyOrEnemyOf(this, GameRound.instance.currentPlayer);
            UIHitChanceInformation.instance.OnMouseHoverEnter(this);
        }

        public void OnMouseHoverExit()
        {
            BattlescapeGraphics.ColouringTool.ColourObject(this, Color.white);
            UIHitChanceInformation.instance.TurnOff();
        }

        bool CouldAttackEnemyFromTile(Unit enemy, Tile tile)
        {
            return tile.position.DistanceTo(enemy.currentPosition.position) <= statistics.GetCurrentAttackRange();
        }

        public void OnCursorOver(IMouseTargetable target)
        {
            if (target is Unit)
            {
                var targetUnit = target as Unit;
                if (targetUnit.CanBeSelected())
                {
                    Cursor.instance.OnSelectableHovered();
                    return;
                }
            }
            if (target is IDamageable)
            {
                var targetDamagableObject = target as IDamageable;                
                if (attack.CanAttack(targetDamagableObject))
                {
                    Cursor.instance.OnEnemyHovered(this, targetDamagableObject);
                    return;
                }
                else
                {
                    Cursor.instance.ShowInfoCursor();
                }
            }
            if (target is Tile)
            {
                var targetTile = target as Tile;
                if (CanMoveTo(targetTile))
                {
                    Cursor.instance.OnTileToMoveHovered(this, targetTile);
                }
                else
                {
                    Cursor.instance.OnInvalidTargetHovered();
                }
            }
            else
            {
                Cursor.instance.OnInvalidTargetHovered();
            }
        }

        public int GetDistanceTo(Position position)
        {
           return position.DistanceTo(currentPosition.position);
        }

        public Player GetMyOwner()
        {
            return owner;
        }
        public void SetMyOwner(Player player)
        {
            owner = player;
        }

        public Vector3 GetMyPosition()
        {
            return transform.position;
        }

        public int GetCurrentDefence()
        {
            return statistics.GetCurrentDefence();
        }

        public string GetMyName()
        {
            return info.unitName;
        }        
    }
}

