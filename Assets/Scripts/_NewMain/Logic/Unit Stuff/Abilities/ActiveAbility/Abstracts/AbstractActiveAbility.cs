using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BattlescapeSound;

namespace BattlescapeLogic
{
    public abstract class AbstractActiveAbility : AbstractAbility, IActiveEntity
    {
        [Header("Limitations")]

        [SerializeField]
        protected TurnPhases legalPhases = TurnPhases.All;

        [SerializeField]
        bool costsMovement;

        [SerializeField] int _cooldown = 1;
        public int cooldown
        {
            get
            {
                return _cooldown;
            }
            protected set
            {
                _cooldown = value;
            }
        }

        [SerializeField] int _range;
        public int range
        {
            get
            {
                return _range;
            }
            protected set
            {
                _range = value;
            }
        }

        public int roundsTillOffCooldown { get; private set; }



        [SerializeField] int _energyCost;       

        public int energyCost
        {
            get
            {
                return _energyCost;
            }
            protected set
            {
                _energyCost = value;
            }
        }

        [SerializeField] int _usesPerBattle;
        public int usesPerBattle
        {
            get
            {
                return _usesPerBattle;
            }
            protected set
            {
                _usesPerBattle = value;
            }
        }

        int _usesLeft = -1;
        public int usesLeft
        {
            get
            {
                if (_usesLeft == -1)
                {
                    _usesLeft = usesPerBattle;
                }
                return _usesLeft;
            }
            protected set
            {
                _usesLeft = value;
            }
        }

        [Header("Visuals and Sound")]
        [Space]
        [SerializeField]
        string _animationTrigger;
        public string animationTrigger
        {
            get
            {
                return _animationTrigger;
            }
            private set
            {
                _animationTrigger = value;
            }
        }
        [SerializeField] Sound onStartSound;
        [SerializeField] Sound onEventSound;

        [SerializeField] GameObject selfVisualEffect;


        public static event Action OnAbilityClicked = delegate { };

        public static event Action OnAbilityFinished = delegate { };

        public abstract void DoAbility();

        public void OnAnimationEvent()
        {
            if (Global.instance.currentEntity.Equals(this))
            {
                BattlescapeSound.SoundManager.instance.PlaySound(owner.gameObject, onEventSound);
                DoAbility();
                OnFinish();
                DoVisualEffectFor(selfVisualEffect, owner.gameObject);
            }
        }

        public virtual bool IsUsableNow()
        {
            //Debug.Log("Is movement cost req. fulfilled: " + CheckMovementCost() + ", check uses left: " + HasUsesLeft() + ", has energy: " + HasEnoughEnergy() + ", is off cooldown: " + IsOffCooldown() + ", is correct phase: " + IsCorrectPhase() + ", is free from states: " + IsFreeFromWrongStates());
            return CheckMovementCost() && HasUsesLeft() && HasEnoughEnergy() && IsOffCooldown() && IsCorrectPhase() && IsFreeFromWrongStates();
        }

        virtual protected bool IsFreeFromWrongStates()
        {
            return owner.states.IsSilenced() == false && owner.states.IsStunned() == false;
        }

        protected bool CheckMovementCost()
        {
            return costsMovement == false || (owner.statistics.movementPoints >= owner.statistics.GetCurrentMaxMovementPoints());
        }

        protected bool IsOffCooldown()
        {
            return roundsTillOffCooldown == 0;
        }

        protected bool HasEnoughEnergy()
        {
            return owner.statistics.energy.current >= energyCost;
        }

        protected bool HasUsesLeft()
        {
            return (usesLeft > 0 || usesPerBattle == 0);
        }

        protected bool IsCorrectPhase()
        {
            switch (legalPhases)
            {
                case TurnPhases.None:
                    Debug.LogWarning("Legal phases for: " + abilityName + " is set to none; it makes no sense, consider changing it");
                    return false;
                case TurnPhases.All:
                    return true;
                default:
                    return legalPhases == GameRound.instance.currentPhase;
            }
        }

        public virtual void OnClickIcon()
        {
            Global.instance.currentEntity = this;
            BattlescapeGraphics.ColouringTool.UncolourAllObjects();
            OnAbilityClicked();
        }

        public abstract Color GetColourForTargets();
        public abstract void ColourPossibleTargets();

        public abstract void OnMouseHovered();
        public abstract void OnMouseUnHovered();

        protected virtual bool IsLegalTarget(IMouseTargetable _target)
        {
            IOnTilePlaceable target = _target as IOnTilePlaceable;
            return IsInRange(target);
        }

        protected bool IsInRange(IOnTilePlaceable target)
        {
            return this.owner.currentPosition.DistanceTo(target.currentPosition) <= range;
        }

        protected bool CheckTarget(IMouseTargetable target)
        {
            return filter.FilterTarget(target) && IsLegalTarget(target);            
        }

        protected virtual void Activate(IMouseTargetable target)
        {
            //perhaps should also be an event?
            BattlescapeGraphics.ColouringTool.UncolourAllObjects();
            owner.statistics.energy.current -= energyCost;
            if (usesPerBattle > 0)
            {
                usesLeft--;
            }
            if (costsMovement)
            {
                owner.statistics.movementPoints = 0;
            }
            roundsTillOffCooldown = cooldown;
            LogConsole.instance.SpawnLog(GetLogMessage());
            Animate();            
            SoundManager.instance.PlaySound(owner.gameObject, onStartSound);
            PlayerInput.instance.LockInput();
        }

        public void OnFinish()
        {
            OnAbilityFinished();
            owner.GetMyOwner().SelectUnit(owner); //this would need to know what ability was used to determine what unit to select, and would complicate things. Easier to remain as a separate line :D     
        }

        public override void OnNewOwnerTurn()
        {
            if (roundsTillOffCooldown > 0)
            {
                roundsTillOffCooldown--;
            }
        }

        protected void Animate()
        {
            if (string.IsNullOrEmpty(animationTrigger) == false)
            {
                owner.animator.SetTrigger(animationTrigger);
            }            
        }

        protected void DoVisualEffectFor(GameObject vfx, GameObject target)
        {
            if (vfx != null)
            {
                Instantiate(vfx, target.transform.position, vfx.transform.rotation,target.transform);
            }
        }

        public abstract void OnLeftClick(IMouseTargetable clickedObject, Vector3 exactClickPosition);

        public void OnRightClick(IMouseTargetable target)
        {
            OnFinish();
        }

        public virtual void OnCursorOver(IMouseTargetable target, Vector3 exactMousePosition)
        {
            if (CheckTarget(target))
            {
                Cursor.instance.OnLegalTargetHovered();
            }
            else
            {
                Cursor.instance.OnInvalidTargetHovered();
            }
        }

       virtual protected string GetLogMessage()
        {
            return owner.name + " uses " + abilityName + "!";
        }

        public void Deselect()
        {
            //cancel ability and reselect owner -> exactly the same effect as canceling/rightclicking
            OnFinish();
        }
    }
}