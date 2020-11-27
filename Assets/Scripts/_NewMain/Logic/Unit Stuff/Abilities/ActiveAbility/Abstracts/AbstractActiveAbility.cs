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

        int roundsTillOffCooldown = 0;



        [SerializeField] int _energyCost;

        public abstract void DoAbility();

        public void OnAnimationEvent()
        {
            if(Global.instance.currentEntity.Equals(this))
            {
                BattlescapeSound.SoundManager.instance.PlaySound(owner.gameObject, onEventSound);
                DoAbility();
                OnFinish();
            }
        }

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
        [SerializeField] protected Color targetColouringColour;


        public static event Action OnAbilityClicked = delegate { };

        public static event Action OnAbilityFinished = delegate { };



        public virtual bool IsUsableNow()
        {
            return CheckMovementCost() && HasUsesLeft() && HasEnoughEnergy() && IsOffCooldown() && IsCorrectPhase();
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
            return owner.statistics.currentEnergy >= energyCost;
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

        public abstract void ColourPossibleTargets();

        public abstract void OnMouseHovered();
        public abstract void OnMouseUnHovered();

        protected virtual bool IsLegalTarget(IMouseTargetable target)
        {
            MonoBehaviour tar = target as MonoBehaviour;
            int x = (int)tar.transform.position.x;
            int y = (int)tar.transform.position.z;
            Tile targetTile = Tile.ToTile(new Position(x, y));
            return this.owner.currentPosition.DistanceTo(targetTile) <= range;
        }

        protected bool CheckTarget(IMouseTargetable target)
        {
            return filter.FilterTarget(target) && IsLegalTarget(target);
        }

        protected virtual void Activate()
        {
            BattlescapeGraphics.ColouringTool.UncolourAllObjects();
            owner.statistics.currentEnergy -= energyCost;
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
            DoVisualEffectFor(selfVisualEffect, owner.gameObject);
            BattlescapeSound.SoundManager.instance.PlaySound(owner.gameObject, onStartSound);
            PlayerInput.instance.isInputBlocked = true;
        }

        public void OnFinish()
        {
            PlayerInput.instance.isInputBlocked = false;
            owner.GetMyOwner().SelectUnit(owner);
            OnAbilityFinished();
        }

        public override void OnNewPlayerRound()
        {
            base.OnNewPlayerRound();
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

        public void OnAbilityOver()
        {
            PlayerInput.instance.isInputBlocked = false;
        }
       virtual protected string GetLogMessage()
        {
            return owner.name + " uses " + abilityName + "!";
        }
    }
}