using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


namespace BattlescapeLogic
{
    public class Unit : OnTileObject, IDamageable, IActiveEntity
    {
        public int index { get; set; }


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
        public AbstractMovement movement { get; set; }
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

        public UnitSounds unitSounds;
        Statistics _statistics;
        public Statistics statistics
        {
            get
            {
                if (_statistics == null)
                {
                    _statistics = new Statistics(DataReader.Read(Resources.Load<TextAsset>("_Data_/Statistics")), info.unitName);
                }
                return _statistics;
            }
        }

        List<AbstractAbility> _abilities;
        public List<AbstractAbility> abilities
        {
            get
            {
                if (_abilities == null)
                {
                    _abilities = new List<AbstractAbility>();
                    UpdateAbilities();
                }
                return _abilities;
            }
        }
        public BuffGroup buffs { get; private set; }
        public ModifierGroup modifiers { get; private set; }
        public States states { get; private set; }
        public GameObject visuals { get; private set; }
        [SerializeField] float _attackRotation;
        public float attackRotation
        {
            get
            {
                return _attackRotation;
            }
        }
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
        public static event Action<Unit, MultiTile, MultiTile> OnUnitMoved = delegate { };

        protected void Start()
        {
            equipment.EquipPrimaryWeapon();
            animator = GetComponentInChildren<Animator>();
            visuals = Helper.FindChildWithTag(gameObject, "Body");
            buffs = new BuffGroup(this);
            modifiers = new ModifierGroup(this);
            states = new States(this);
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
            statistics.healthPoints = statistics.baseMaxHealthPoints;
            statistics.energy.current = Energy.starting;
            statistics.numberOfRetaliations = statistics.baseMaxNumberOfRetaliations;
            FaceMiddleOfMap();
            OnUnitMoved += OnOtherUnitMoved;
        }

        private void UpdateAbilities()
        {
            abilities.Clear();
            foreach (AbstractAbility ability in GetComponents<AbstractAbility>())
            {
                abilities.Add(ability);
            }
        }

        public void FaceMiddleOfMap()
        {
            visuals = Helper.FindChildWithTag(gameObject, "Body");
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
            return statistics.numberOfAttacks > 0 && states.IsDisarmed() == false && states.IsStunned() == false;
        }

        public bool CanStillMove()
        {
            return statistics.movementPoints > 0 && states.IsImmobile() == false && states.IsStunned() == false;
        }

        public bool CanStillRetaliate()
        {
            return statistics.numberOfRetaliations > 0 && states.IsOverwhelmed() == false && states.IsStunned() == false;
        }

        //Played BEFORE the first step in the whole movement. Check for ExitCombat here cause it only makes sense here.
        //Returns true if movement is exiting combat
        public bool IsExittingCombat(MultiTile newPosition)
        {
            if (newPosition.IsProtectedByEnemyOf(this))
            {
                return false;
            }
            if (currentPosition.IsProtectedByEnemyOf(this))
            {
                return true;
            }

            return false;
        }

        public void OnMove(MultiTile oldPosition, MultiTile newPosition)
        {
            OnUnitMoved(this, oldPosition, newPosition);
            //played on EVERY change of tile. Two tiles are used here to avoid confusion in using currentPosition - this will work no matter if we use it slightly before or after movement.
            if (newPosition.IsProtectedByEnemyOf(this))
            {
                //note we can only come to combat FROM SAFETY, so no need for check for that
                OnCombatEnter();
            }
            if (oldPosition.IsProtectedByEnemyOf(this))
            {
                OnCombatExit();
            }
        }

        void OnOtherUnitMoved(Unit unit, MultiTile from, MultiTile to)
        {
            foreach (Tile fromNeighbour in from.closeNeighbours)
            {
                if (fromNeighbour.GetMyObject<Unit>() == this && IsEnemyOf(unit) && IsInCombat() == false)
                {
                    OnCombatExit();
                }
            }

            foreach (Tile toNeighbour in to.closeNeighbours)
            {
                if (toNeighbour.GetMyObject<Unit>() == this && IsEnemyOf(unit))
                {
                    OnCombatEnter();
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
        public void Move(MultiTile newPosition)
        {
            movement.ApplyUnit(this);
            StartCoroutine(movement.MoveTo(newPosition));
        }

        public void RetaliateTo(Unit target)
        {
            LogConsole.instance.SpawnLog(this.name + " strikes back!");
            statistics.numberOfRetaliations--;
            attack.BasicAttack(target);
            Networking.instance.FinishRetaliation();
        }

        //in the future most likely more functions might want to do things OnAttack - abilities and so on
        //public event Action<Unit, Unit, int> AttackEvent;       

        public bool CanRetaliateTo(Damage damage)
        {
            if (damage.source is AbstractAttack == false)
            {
                return false;
            }
            var target = (damage.source as AbstractAttack).sourceUnit;
            if (
                this.IsAlive() == false
                || target.IsAlive() == false
                || this.CanStillRetaliate() == false
                || GameRound.instance.currentPlayer != target.GetMyOwner()
                || currentPosition.DistanceTo(target.currentPosition) != 1
                || target.states.IsPreventingRetaliation())
            {
                return false;
            }
            return true;
        }

        //this should play on attacked unit when it is time it should receive DMG
        public void TakeDamage(Damage damage)
        {
            statistics.healthPoints -= damage;
            if (IsAlive())
            {
                PlayWoundAnimation();
            }
            else
            {
                Die(damage.source);
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

        public virtual void Die(IDamageSource killer)
        {
            if (owner.selectedUnit == this)
            {
                Deselect();
            }
            currentPosition.SetMyObjectTo(null);
            if (currentPosition.IsProtectedByEnemyOf(this))
            {
                foreach (Tile neighbour in currentPosition.closeNeighbours)
                {
                    Unit neighbourUnit = neighbour.GetMyObject<Unit>();
                    if (neighbourUnit != null && IsEnemyOf(neighbourUnit) && neighbour.IsProtectedByEnemyOf(neighbourUnit) == false)
                    {
                        //The enemy dude exists and just got free from combat by death of our Unit;
                        neighbour.GetMyObject<Unit>().OnCombatExit();
                    }
                }
            }

            //Note: this makes the Tile 'forget' about the unit, but the dead Unit 'remembers' its last Tile!

            if (GameRound.instance.currentPlayer.selectedUnit == this)
            {
                owner.DeselectUnit();
            }
            killer.OnKillUnit(this);
            HideHealthUI();
            PlayDeathAnimation();
            turnChanger.OnDestruction();
            //whatever else we need to do on death, i guess?
            //definitely a log to log window
        }

        void HideHealthUI()
        {
            GetComponentInChildren<Canvas>().gameObject.SetActive(false);
        }

        public bool IsInAttackRange(int distance)
        {
            if (IsInCombat())
            {
                return distance <= statistics.attackRange.GetCurrentAttackRangeInCombat() && distance >= statistics.attackRange.minimalAttackRange;
            }
            else
            {
                return distance <= statistics.attackRange.GetCurrentAttackRangeOutOfCombat() && distance >= statistics.attackRange.minimalAttackRange;
            }
            
        }

        public int GetCurrentAttack()
        {
            if (IsInCombat())
            {
                return statistics.GetCurrentMeleeAttack();
            }
            else
            {
                return statistics.GetCurrentRangeAttack();
            }
        }

        public bool HasClearView(Vector3 defender)
        {
            foreach (var targetable in Global.FindAllTargetablesInLine(transform.position + new Vector3(0,0.1f,0), defender + new Vector3(0,0.1f, 0)))
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

        public void OnNewRound()
        {
            return;
        }

        public void OnOwnerTurn()
        {
            statistics.OnOwnerTurn();
        }

        public void OnNewTurn()
        {
            return;
        }

        public void OnNewPhase()
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

        public void OnLeftClick(IMouseTargetable target, Vector3 exactClickPosition)
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
                else if (targetUnit.CanBeSelected())
                {
                    targetUnit.GetMyOwner().SelectUnit(targetUnit);
                }

            }
            else if (target is Tile)
            {
                MultiTile destination = (target as Tile).PositionRelatedToMouse(currentPosition.size, exactClickPosition);
                if (CanMoveTo(destination))
                {
                    Networking.instance.SendCommandToMove(this, destination);
                }

            }
        }

        public bool CanBeSelected()
        {
            return PlayerInput.instance.isLocked == false && owner.IsCurrentLocalPlayer() && GameRound.instance.currentPhase != TurnPhases.Enemy && GameRound.instance.currentPhase != TurnPhases.None && IsAlive();
        }

        public void Select()
        {
            OnUnitSelected(this);
            if (GameRound.instance.currentPlayer.type != PlayerType.AI)
            {
                PlaySelectionSound();
            }
        }

        void PlaySelectionSound()
        {
            BattlescapeSound.SoundManager.instance.PlaySound(GameRound.instance.currentPlayer.selectedUnit.gameObject, BattlescapeSound.SoundManager.instance.selectionSound);
        }

        public bool CanMoveTo(MultiTile position)
        {
            movement.ApplyUnit(this);
            return movement.CanMoveTo(position);
        }

        public void OnTileHovered(Tile hoveredTile, Vector3 exactMousePosition)
        {
            MultiTile hoveredMultitile = hoveredTile.PositionRelatedToMouse(currentPosition.size, exactMousePosition);
            if (CanMoveTo(hoveredMultitile))
            {
                foreach (Unit otherUnit in Global.instance.GetAllUnits())
                {
                    if (IsEnemyOf(otherUnit) && CouldAttackEnemyFromTile(otherUnit, hoveredMultitile))
                    {
                        BattlescapeGraphics.ColouringTool.ColourObject(otherUnit, Color.red);
                    }
                }
            }
        }

        public void OnMouseHoverEnter(Vector3 exactMousePosition)
        {
            if ((Global.instance.currentEntity is AbstractActiveAbility) == false)
            {
                BattlescapeGraphics.ColouringTool.ColourUnitAsAllyOrEnemyOf(this, GameRound.instance.currentPlayer);
            }

        }

        public void OnMouseHoverExit()
        {
            if ((Global.instance.currentEntity is AbstractActiveAbility) == false)
            {
                BattlescapeGraphics.ColouringTool.ColourObject(this, Color.white);
            }
            UIHitChanceInformation.instance.TurnOff();
        }

        bool CouldAttackEnemyFromTile(Unit enemy, MultiTile position)
        {
            return IsInAttackRange(enemy.currentPosition.DistanceTo(position));
        }

        public void OnCursorOver(IMouseTargetable target, Vector3 exactMousePosition)
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
                if (targetDamagableObject.IsAlive() == false)
                {
                    int x = Mathf.RoundToInt(exactMousePosition.x);
                    int z = Mathf.RoundToInt(exactMousePosition.z);
                    Tile tile = Global.instance.currentMap.board[x, z];
                    OnCursorOver(tile, exactMousePosition);
                    return;
                }
                if (attack.CanAttack(targetDamagableObject))
                {
                    Cursor.instance.OnEnemyHovered(this, targetDamagableObject);
                    return;
                }                
            }
            if (target is Tile)
            {
                var targetTile = target as Tile;
                MultiTile position = targetTile.PositionRelatedToMouse(currentPosition.size, exactMousePosition);

                if (CanMoveTo(position))
                {
                    BattlescapeGraphics.ColouringTool.ColourLegalTilesFor(this);
                    BattlescapeGraphics.ColouringTool.OnPositionHovered(position);
                    Cursor.instance.OnTileToMoveHovered(this, position);
                    return;
                }
                else
                {
                    Cursor.instance.OnInvalidTargetHovered();
                    return;
                }
            }
            else
            {
                Cursor.instance.OnInvalidTargetHovered();
                return;
            }
        }

        public Player GetMyOwner()
        {
            return owner;
        }
        public void SetMyOwner(Player player)
        {
            owner = player;
            turnChanger = new TurnChanger(owner, OnNewRound, OnNewTurn, OnNewPhase, OnOwnerTurn);
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

        public float ChanceOfBeingHitBy(IDamageSource source)
        {
            if (states.IsInvulnerable())
            {
                return 0;
            }
            return Maths.Sigmoid(DamageCalculator.GetStatisticsDifference(source, this), DamageCalculator.sigmoidGrowthRate);
        }

        public bool IsInvulnerable()
        {
            return states.IsInvulnerable();
        }

        public void OnHitReceived(Damage damage)
        {
            LogConsole.instance.SpawnLog(damage.source.GetMyOwner().info.unitName + " deals " + damage + " damage to " + info.unitName + "!");
            PopupTextController.AddPopupText("-" + damage, PopupTypes.Damage);
            TakeDamage(damage);
            buffs.RemoveDefenseDebuffsOnHit();
            if (CanRetaliateTo(damage))
            {
                Networking.instance.SendCommandToGiveChoiceOfRetaliation(this, (damage.source as AbstractAttack).sourceUnit);
            }
        }

        public void OnMissReceived(Damage damage)
        {
            LogConsole.instance.SpawnLog(damage.source.GetMyOwner().info.unitName + " attacks " + info.unitName + ", but misses completely!");

            buffs.AddDefenseDebuffOnMiss();

            if (CanRetaliateTo(damage))
            {
                Networking.instance.SendCommandToGiveChoiceOfRetaliation(this, (damage.source as AbstractAttack).sourceUnit);
            }
        }

        public void OnHitReceivedWhenInvulnerable(Damage damage)
        {
            LogConsole.instance.SpawnLog(damage.source.GetMyOwner().info.unitName + "'s attack has no effect - " + info.unitName + " is invulnerable!");
            PopupTextController.AddPopupText("Invulnerable!", PopupTypes.Info);

            if (CanRetaliateTo(damage))
            {
                Networking.instance.SendCommandToGiveChoiceOfRetaliation(this, (damage.source as AbstractAttack).sourceUnit);
            }
        }

        public void Deselect()
        {
            OnUnitDeselected();
            if (Global.instance.currentEntity == this as IActiveEntity)
            {
                Global.instance.currentEntity = GameRound.instance.currentPlayer;
            }
        }
    }
}

