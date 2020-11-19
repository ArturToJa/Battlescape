using System.Collections;
using System.Collections.Generic;
//using OdinSerializer;
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

        public UnitSounds unitSounds;
        public Statistics statistics;

        public List<AbstractAbility> abilities { get; private set; }
        public BuffGroup buffs { get; private set; }
        public List<AbstractAttackModifier> modifiers { get; private set; }
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

        protected void Start()
        {
            equipment.EquipPrimaryWeapon();
            animator = GetComponentInChildren<Animator>();
            visuals = Helper.FindChildWithTag(gameObject, "Body");
            buffs = new BuffGroup(this);
            modifiers = new List<AbstractAttackModifier>();
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
        }

        private void UpdateAbilities()
        {
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

        public void ExitCombat()
        {
            OnCombatExit();
            foreach (Tile neighbour in currentPosition.closeNeighbours)
            {
                if (neighbour.GetMyObject<Unit>() != null && IsEnemyOf(neighbour.GetMyObject<Unit>()) && neighbour.IsProtectedByEnemyOf(neighbour.GetMyObject<Unit>()) == false)
                {
                    neighbour.GetMyObject<Unit>().OnCombatExit();
                }
            }
        }

        public void OnMove(MultiTile oldPosition, MultiTile newPosition)
        {
            //played on EVERY change of tile. Two tiles are used here to avoid confusion in using currentPosition - this will work no matter if we use it slightly before or after movement.
            if (oldPosition.IsProtectedByEnemyOf(this) == false && newPosition.IsProtectedByEnemyOf(this))
            {
                //Unit just came from SAFETY to COMBAT, so inform it and all of the enemies around about it.
                OnCombatEnter();
                foreach (Tile neighbour in newPosition.closeNeighbours)
                {
                    if (neighbour.GetMyObject<Unit>() != null && IsEnemyOf(neighbour.GetMyObject<Unit>()))
                    {
                        neighbour.GetMyObject<Unit>().OnCombatEnter();
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
        public void Move(MultiTile newPosition)
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
            LogConsole.instance.SpawnLog(this.name + " strikes back!");
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
        public void HitTarget(IDamageable target, Damage damage)
        {
            foreach (AbstractAttackModifier modifier in modifiers)
            {
                modifier.ModifyDamage(damage);
            }
            foreach (AbstractAttackModifier modifier in modifiers)
            {
                modifier.ModifyAttack(target, damage);
            }
            PlayerInput.instance.isInputBlocked = false;
            if (damage == 0)
            {
                StatisticChangeBuff defenceDebuff = Instantiate(Resources.Load("Buffs/MechanicsBuffs/Combat Wound") as GameObject).GetComponent<StatisticChangeBuff>();
                defenceDebuff.ApplyOnTarget(target);
                LogConsole.instance.SpawnLog(this.info.unitName + " attacks " + target.GetMyName() + ", but misses completely!");
                LogConsole.instance.SpawnLog(target.GetMyName() + " loses 1 point of Defence temporarily.");
                PopupTextController.AddPopupText("-1 Defence", PopupTypes.Stats);

            }
            else if (damage > 0)
            {
                LogConsole.instance.SpawnLog(this.info.unitName + " deals " + damage + " damage to " + target.GetMyName() + "!");
                PopupTextController.AddPopupText("-" + damage, PopupTypes.Damage);
                target.TakeDamage(this, damage);
                foreach (AbstractBuff buff in target.buffs.FindAllBuffsOfType("Combat Wound"))
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

        public bool CanRetaliate(Unit target)
        {
            if (this.IsAlive() == false || this.CanStillRetaliate() == false || GameRound.instance.currentPlayer != target.GetMyOwner())
            {
                return false;
            }
            if (currentPosition.DistanceTo(target.currentPosition) == 1)
            {
                return true;
            }
            
            return false;
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
            currentPosition.SetMyObjectTo(null);
            if (currentPosition.IsProtectedByEnemyOf(this))
            {
                foreach (Tile tile in currentPosition.closeNeighbours)
                {
                    if (tile.GetMyObject<Unit>() != null && IsEnemyOf(tile.GetMyObject<Unit>()) && tile.IsProtectedByEnemyOf(tile.GetMyObject<Unit>()) == false)
                    {
                        //The enemy dude exists and just got free from combat by death of our Unit;
                        tile.GetMyObject<Unit>().OnCombatExit();
                    }
                }
            }

            //Note: this makes the Tile 'forget' about the unit, but the dead Unit 'remembers' its last Tile!

            if (GameRound.instance.currentPlayer.selectedUnit == this)
            {
                owner.DeselectUnit();
            }
            if (killer.owner != owner)
            {
                killer.owner.AddPoints(statistics.cost);
            }
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
            return distance <= statistics.GetCurrentAttackRange() && distance >= statistics.minimalAttackRange;
        }

        public bool HasClearView(Vector3 defender)
        {
            foreach (var targetable in Global.FindAllTargetablesInLine(transform.position, defender))
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

        public void OnNewPlayerRound()
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
                else if (targetUnit.GetMyOwner().team == owner.team)
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
            return PlayerInput.instance.isInputBlocked == false && owner.IsCurrentLocalPlayer() && GameRound.instance.currentPhase != TurnPhases.Enemy && GameRound.instance.currentPhase != TurnPhases.None && IsAlive();
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
            BattlescapeGraphics.ColouringTool.ColourUnitAsAllyOrEnemyOf(this, GameRound.instance.currentPlayer);
        }

        public void OnMouseHoverExit()
        {
            BattlescapeGraphics.ColouringTool.ColourObject(this, Color.white);
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
                MultiTile position = targetTile.PositionRelatedToMouse(currentPosition.size, exactMousePosition);

                if (CanMoveTo(position))
                {
                    BattlescapeGraphics.ColouringTool.ColourLegalTilesFor(this);
                    BattlescapeGraphics.ColouringTool.OnPositionHovered(position);
                    Cursor.instance.OnTileToMoveHovered(this, position);
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

        public Player GetMyOwner()
        {
            return owner;
        }
        public void SetMyOwner(Player player)
        {
            owner = player;
            turnChanger = new TurnChanger(owner, OnNewRound, OnNewTurn, OnNewPhase, OnNewPlayerRound);
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

        public float ChanceOfBeingHitBy(Unit source)
        {
            return Maths.Sigmoid(DamageCalculator.GetStatisticsDifference(source, this), DamageCalculator.sigmoidGrowthRate);
        }




    }
}

